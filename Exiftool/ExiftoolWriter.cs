using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Diagnostics;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using System.Drawing;
using NLog;
using System.Threading;
using WindowsProperty;
using ApplicationAssociations;

namespace Exiftool
{

    public static class ExiftoolWriter
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //public static object SystemProperties { get; private set; }

        #region Compare orginal red metadata with what user has updated 
        public static List<int> GetListOfMetadataChangedByUser(List<Metadata> metadataListOriginal, List<Metadata> metadataListToWrite)
        {
            List<int> listOfUpdates = new List<int>();

            if (metadataListToWrite == null && metadataListToWrite.Count > 0) return listOfUpdates;
            if (metadataListOriginal == null) return listOfUpdates;
            if (metadataListOriginal.Count != metadataListToWrite.Count) return listOfUpdates;

            for (int i = 0; i < metadataListOriginal.Count; i++)
            {
                if (metadataListOriginal[i] != metadataListToWrite[i]) listOfUpdates.Add(i);
            }
            return listOfUpdates;
        }
        #endregion

        #region Files locked, wait unlock, in cloud
        public static bool IsFileInCloud(string fullFileName)
        {
            /*
            FILE_ATTRIBUTE_RECALL_ON_DATA_ACCESS
            4194304 (0x400000)
            When this attribute is set, it means that the file or directory is not fully present locally. For a file that means that not all of its data is on local storage (e.g. it may be sparse with some data still in remote storage). For a directory it means that some of the directory contents are being virtualized from another location. Reading the file / enumerating the directory will be more expensive than normal, e.g. it will cause at least some of the file/directory content to be fetched from a remote store. Only kernel-mode callers can set this bit.
    
            1048576 (0x100000) Unknown flag
            */
            try
            {
                FileAttributes fileAttributes = File.GetAttributes(fullFileName);
                if ((((int)fileAttributes) & 0x000400000) == 0x000400000)
                    return true;
            }
            catch { }
            return false;
        }

        public static bool IsFileLockedByProcess(string fullFilePath)
        {
            FileStream fs = null;
            try
            {
                // NOTE: This doesn't handle situations where file is opened for writing by another process but put into write shared mode, it will not throw an exception and won't show it as write locked
                fs = File.Open(fullFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None); // If we can't open file for reading and writing then it's locked by another process for writing
            }
            catch (UnauthorizedAccessException) // https://msdn.microsoft.com/en-us/library/y973b725(v=vs.110).aspx
            {
                // This is because the file is Read-Only and we tried to open in ReadWrite mode, now try to open in Read only mode
                try
                {
                    fs = File.Open(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (Exception)
                {
                    return true; // This file has been locked, we can't even open it to read
                }
            }
            catch (Exception)
            {
                return true; // This file has been locked
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return false;
        }


        public static bool IsFileReadOnly(string fileFullPath)
        {
            bool isReadOnly; 
            try
            {
                isReadOnly = new FileInfo(fileFullPath).IsReadOnly; 
            } catch (Exception ex)
            {
                isReadOnly = true;
                Logger.Warn(ex.Message);
            }
            return isReadOnly;
        }

        

        public static void WaitLockedFileToBecomeUnlocked(string fileFullPath)
        {
            int maxRetry = 30;
            bool areAnyFileLocked;
            do
            {
                if (File.Exists(fileFullPath)) areAnyFileLocked = IsFileLockedByProcess(fileFullPath);
                else areAnyFileLocked = false;
                if (areAnyFileLocked) Thread.Sleep(500);
                if (maxRetry-- < 0) areAnyFileLocked = false;
            } while (areAnyFileLocked);
        }

        public static bool IsFileThatNeedUpdatedLockedByProcess(List<FileEntry> fileEntriesToCheck)
        {
            if (fileEntriesToCheck.Count == 0) return false;

            foreach (FileEntry fileEntryToCheck in fileEntriesToCheck)
            {

                if (!File.Exists(fileEntryToCheck.FileFullPath)) return true; //In process rename
                if (IsFileReadOnly(fileEntryToCheck.FileFullPath)) return false; //No need to wait, Attribute is set to read only
                if (IsFileLockedByProcess(fileEntryToCheck.FileFullPath)) return true; //In process OneDrive backup / update
            }
            return false;
        }

        public static bool IsFileThatNeedUpdatedLockedByProcess(List<Metadata> fileEntriesToCheck)
        {
            if (fileEntriesToCheck.Count == 0) return false;

            foreach (Metadata fileEntryToCheck in fileEntriesToCheck)
            {

                if (!File.Exists(fileEntryToCheck.FileFullPath)) return true; //In process rename
                if (IsFileReadOnly(fileEntryToCheck.FileFullPath)) return false; //No need to wait, Attribute is set to read only
                if (IsFileLockedByProcess(fileEntryToCheck.FileFullPath)) return true; //In process OneDrive backup / update
            }
            return false;
        }

        public static void WaitLockedFilesToBecomeUnlocked(List<FileEntry> fileEntriesToCheck)
        {
            int maxRetry = 30;
            bool areAnyFileLocked;
            do
            {
                areAnyFileLocked = IsFileThatNeedUpdatedLockedByProcess(fileEntriesToCheck);
                if (areAnyFileLocked) Thread.Sleep(1000);
                if (maxRetry-- < 0) areAnyFileLocked = false;
            } while (areAnyFileLocked);
        }

        public static void WaitLockedFilesToBecomeUnlocked(List<Metadata> fileEntriesToCheck)
        {
            int maxRetry = 30;
            bool areAnyFileLocked;
            do
            {
                areAnyFileLocked = IsFileThatNeedUpdatedLockedByProcess(fileEntriesToCheck);
                if (areAnyFileLocked) Thread.Sleep(1000);
                if (maxRetry-- < 0) areAnyFileLocked = false;
            } while (areAnyFileLocked);
        }
        #endregion

        #region WriteXtraAtom
        public static List<FileEntry> WriteXtraAtom(
            List<Metadata> metadataListToWrite, List<Metadata> metadataListOriginal, List<string> allowedFileNameDateTimeFormats,
            string writeXtraAtomAlbumVariable, bool writeXtraAtomAlbumVideo,
            string writeXtraAtomCategoriesVariable, bool writeXtraAtomCategoriesVideo,
            string writeXtraAtomCommentVariable, bool writeXtraAtomCommentPicture, bool writeXtraAtomCommentVideo,
            string writeXtraAtomKeywordsVariable, bool writeXtraAtomKeywordsVideo,
            bool writeXtraAtomRatingPicture, bool writeXtraAtomRatingVideo,
            string writeXtraAtomSubjectVariable, bool writeXtraAtomSubjectPicture, bool wtraAtomSubjectVideo,
            string writeXtraAtomSubtitleVariable, bool writeXtraAtomSubtitleVideo,
            string writeXtraAtomArtistVariable, bool writeXtraAtomArtistVideo,
            out Dictionary<string, string> writeXtraAtomErrorMessageForFile)
        {
            writeXtraAtomErrorMessageForFile = new Dictionary<string, string>(); //Clear out values
            List<FileEntry> filesUpdatedByWritePropertiesAndLastWriteTime = new List<FileEntry>();
            
            if (metadataListToWrite.Count <= 0) return filesUpdatedByWritePropertiesAndLastWriteTime;
            if (metadataListToWrite.Count != metadataListOriginal.Count) return filesUpdatedByWritePropertiesAndLastWriteTime;
            int writeCount = metadataListToWrite.Count;

            for (int updatedRecord = 0; updatedRecord < writeCount; updatedRecord++)
            {
                Metadata metadataToWrite = metadataListToWrite[updatedRecord];
                Metadata metadataOriginal = metadataListOriginal[updatedRecord];

                if (metadataToWrite == metadataOriginal) continue; //No changes found in data, No data to write

                #region Is Video or Image format?
                bool isVideoFormat = false;
                bool isImageFormat = true;
                if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(metadataToWrite.FileFullPath))
                {
                    isVideoFormat = true;
                    isImageFormat = false;
                }
                #endregion 

                #region Replace Variable 
                string writeXtraAtomAlbumReult = metadataToWrite.ReplaceVariables(writeXtraAtomAlbumVariable, allowedFileNameDateTimeFormats);
                string writeXtraAtomCategoriesResult = metadataToWrite.ReplaceVariables(writeXtraAtomCategoriesVariable, allowedFileNameDateTimeFormats);
                string writeXtraAtomCommentResult = metadataToWrite.ReplaceVariables(writeXtraAtomCommentVariable, allowedFileNameDateTimeFormats);
                string writeXtraAtomKeywordsResult = metadataToWrite.ReplaceVariables(writeXtraAtomKeywordsVariable, allowedFileNameDateTimeFormats);
                string writeXtraAtomSubjectResult = metadataToWrite.ReplaceVariables(writeXtraAtomSubjectVariable, allowedFileNameDateTimeFormats);
                string writeXtraAtomSubtitleResult = metadataToWrite.ReplaceVariables(writeXtraAtomSubtitleVariable, allowedFileNameDateTimeFormats);
                string writeXtraAtomArtistResult = metadataToWrite.ReplaceVariables(writeXtraAtomArtistVariable, allowedFileNameDateTimeFormats);
                #endregion

                #region Write Xtra Atrom using Property Writer
                if (writeXtraAtomKeywordsVideo || writeXtraAtomCategoriesVideo || writeXtraAtomAlbumVideo || writeXtraAtomSubtitleVideo ||
                    writeXtraAtomArtistVideo || wtraAtomSubjectVideo || writeXtraAtomCommentVideo || writeXtraAtomRatingVideo ||
                    writeXtraAtomSubjectPicture || writeXtraAtomCommentPicture || writeXtraAtomRatingPicture)
                {
                    WaitLockedFileToBecomeUnlocked(metadataToWrite.FileFullPath);
                    if (!IsFileReadOnly(metadataToWrite.FileFullPath) || !IsFileLockedByProcess(metadataToWrite.FileFullPath))
                    {
                        try
                        {
                            using (WindowsPropertyWriter windowsPropertyWriter = new WindowsPropertyWriter(metadataToWrite.FileFullPath))
                            {
                                if (isVideoFormat)
                                {

                                    if (writeXtraAtomKeywordsVideo) windowsPropertyWriter.WriteKeywords(string.IsNullOrEmpty(writeXtraAtomKeywordsResult) ? null : writeXtraAtomKeywordsResult);
                                    if (writeXtraAtomCategoriesVideo) windowsPropertyWriter.WriteCategories(string.IsNullOrEmpty(writeXtraAtomCategoriesResult) ? null : writeXtraAtomCategoriesResult);
                                    if (writeXtraAtomAlbumVideo) windowsPropertyWriter.WriteAlbum(string.IsNullOrEmpty(writeXtraAtomAlbumReult) ? null : writeXtraAtomAlbumReult);

                                    if (writeXtraAtomSubtitleVideo) windowsPropertyWriter.WriteSubtitle_Description(string.IsNullOrEmpty(writeXtraAtomSubtitleResult) ? null : writeXtraAtomSubtitleResult);
                                    if (writeXtraAtomArtistVideo) windowsPropertyWriter.WriteArtist_Author(string.IsNullOrEmpty(writeXtraAtomArtistResult) ? null : writeXtraAtomArtistResult);

                                    if (wtraAtomSubjectVideo) windowsPropertyWriter.WriteSubject_Description(string.IsNullOrEmpty(writeXtraAtomSubjectResult) ? null : writeXtraAtomSubjectResult);
                                    if (writeXtraAtomCommentVideo) windowsPropertyWriter.WriteComment(string.IsNullOrEmpty(writeXtraAtomCommentResult) ? null : writeXtraAtomCommentResult);
                                    if (writeXtraAtomRatingVideo) windowsPropertyWriter.WriteRating((metadataToWrite.PersonalRatingPercent == null ? (int)0 : (int)metadataToWrite.PersonalRatingPercent));
                                }
                                else if (isImageFormat)
                                {
                                    if (writeXtraAtomSubjectPicture) windowsPropertyWriter.WriteSubject_Description(string.IsNullOrEmpty(writeXtraAtomSubjectResult) ? null : writeXtraAtomSubjectResult);
                                    if (writeXtraAtomCommentPicture) windowsPropertyWriter.WriteComment(string.IsNullOrEmpty(writeXtraAtomCommentResult) ? null : writeXtraAtomCommentResult);
                                    if (writeXtraAtomRatingPicture) windowsPropertyWriter.WriteRating((metadataToWrite.PersonalRatingPercent == null ? (int)0 : (int)metadataToWrite.PersonalRatingPercent));
                                }

                                windowsPropertyWriter.Close();

                                filesUpdatedByWritePropertiesAndLastWriteTime.Add(new FileEntry(metadataToWrite.FileFullPath, File.GetLastWriteTime(metadataToWrite.FileFullPath)));
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Failed write Xtra Atom Propery on file: " + metadataToWrite.FileFullPath + "\r\n" + ex.Message);
                            writeXtraAtomErrorMessageForFile.Add(metadataToWrite.FileFullPath, ex.Message);
                        }
                    } else
                    { //File readonly or locked

                        string error = "Failed write Xtra Atom Propery on file: " + metadataToWrite.FileFullPath + "\r\n";
                        if (IsFileReadOnly(metadataToWrite.FileFullPath)) error += "File is Read Only.\r\n";
                        if (IsFileLockedByProcess(metadataToWrite.FileFullPath)) error += "File is locked by another process.\r\n";
                        Logger.Error(error);
                        writeXtraAtomErrorMessageForFile.Add(metadataToWrite.FileFullPath, error);
                    }
                }
                #endregion

            }
            return filesUpdatedByWritePropertiesAndLastWriteTime;
        }
        #endregion

        #region CreateExiftoolArguFileText
        public static List<FileEntry> CreateExiftoolArguFileText(List<Metadata> metadataListToWrite, List<Metadata> metadataListOriginal, List<string> allowedFileNameDateTimeFormats,
            string writeMetadataTagsVariable, string writeMetadataKeywordDeleteVariable, string writeMetadataKeywordAddVariable, bool alwaysWrite, out string exiftoolArguFileText)
        {
            exiftoolArguFileText = "";
            List<FileEntry> filesNeedToBeUpadted = new List<FileEntry>();

            if (metadataListToWrite.Count <= 0) return filesNeedToBeUpadted;
            if (metadataListToWrite.Count != metadataListOriginal.Count) return filesNeedToBeUpadted;
            int writeCount = metadataListToWrite.Count;

            for (int updatedRecord = 0; updatedRecord < writeCount; updatedRecord++)
            {
                Metadata metadataToWrite = metadataListToWrite[updatedRecord];
                Metadata metadataOriginal = metadataListOriginal[updatedRecord];

                if (!alwaysWrite && metadataToWrite == metadataOriginal) continue; //No changes found in data, No data to write
                filesNeedToBeUpadted.Add(metadataToWrite.FileEntryBroker);

                string tagsToWrite = metadataToWrite.RemoveLines(writeMetadataTagsVariable, metadataOriginal, alwaysWrite);

                string personalKeywordDelete = metadataOriginal.ReplaceVariables(writeMetadataKeywordDeleteVariable, allowedFileNameDateTimeFormats);
                string personalKeywordAdd = metadataToWrite.ReplaceVariables(writeMetadataKeywordAddVariable, allowedFileNameDateTimeFormats);

                string personalKeywordDeleteItems = metadataToWrite.VariablePersonalKeywords(personalKeywordDelete, allowedFileNameDateTimeFormats);
                string personalKeywordAddItems = metadataToWrite.VariablePersonalKeywords(personalKeywordAdd, allowedFileNameDateTimeFormats);

                tagsToWrite = metadataToWrite.ReplaceVariables(tagsToWrite, allowedFileNameDateTimeFormats, personalKeywordDeleteItems, personalKeywordAddItems);
                exiftoolArguFileText += (exiftoolArguFileText == "" ? "" : "\r\n") + tagsToWrite;
            }
            return filesNeedToBeUpadted;
        }
        #endregion 

        #region GetTempArguFileFullPath
        public static string GetTempArguFileFullPath(string tempfilename)
        {
            //Create directory, filename and remove old arg file
            string exiftoolArgFileDirecory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            if (!Directory.Exists(exiftoolArgFileDirecory)) Directory.CreateDirectory(exiftoolArgFileDirecory);
            string exiftoolArgFileFullPath = Path.Combine(exiftoolArgFileDirecory, tempfilename);
            if (File.Exists(exiftoolArgFileFullPath)) File.Delete(exiftoolArgFileFullPath);
            return exiftoolArgFileFullPath;
        }
        #endregion 

        #region WriteMetadata
        public static List<FileEntry> WriteMetadata(List<Metadata> metadataListToWrite, List<Metadata> metadataListOriginal, List<string> allowedFileNameDateTimeFormats,
            string writeMetadataTagsVariable, string writeMetadataKeywordDeleteVariable, string writeMetadataKeywordAddVariable)
        {
            List<FileEntry> filesNeedToBeUpadted = CreateExiftoolArguFileText(
                metadataListToWrite, metadataListOriginal, allowedFileNameDateTimeFormats, writeMetadataTagsVariable, writeMetadataKeywordDeleteVariable, writeMetadataKeywordAddVariable, 
                false, out string resultReplaceVariables);

            if (filesNeedToBeUpadted.Count > 0) //Save if has anything to save
            {
                //Create directory, filename and remove old arg file
                string exiftoolArgFileFullpath = GetTempArguFileFullPath("exiftool_arg.txt");

                using (StreamWriter sw = new StreamWriter(exiftoolArgFileFullpath, false, Encoding.UTF8))
                {
                    sw.WriteLine(resultReplaceVariables);
                }

                #region Exiftool Write
                String path = NativeMethods.GetFullPathOfFile("exiftool.exe");
                string arguments = "-charset utf8 -charset iptc=utf8 -codedcharacterset=utf8 -m -@ \"" + NativeMethods.ShortFileName(exiftoolArgFileFullpath) + "\"";
                bool hasExiftoolErrorMessage = false;
                string exiftoolOutput = "";

                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        RedirectStandardInput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    }
                })
                {
                    bool result = process.Start();
                    string line;

                    while (!process.StandardError.EndOfStream)
                    {
                        line = process.StandardError.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        if (!line.StartsWith("Warning")) hasExiftoolErrorMessage = true;
                        Logger.Error("EXIFTOOL WRITE ERROR: " + line);
                    }

                    while (!process.StandardOutput.EndOfStream)
                    {
                        line = process.StandardOutput.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        if (line.StartsWith("Error")) hasExiftoolErrorMessage = true;
                        Logger.Info("EXIFTOOL WRITE OUTPUT: " + line);
                    }

                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        hasExiftoolErrorMessage = true;
                        Logger.Info("process.WaitForExit() " + process.ExitCode);
                    }

                    while (!process.HasExited) Thread.Sleep(100);

                    process.Close();
                    process.Dispose();
                }
                if (hasExiftoolErrorMessage) throw new Exception(exiftoolOutput);
                #endregion
            }
            return filesNeedToBeUpadted;
        }
        #endregion

        #region Verify HasWriteMetadataErrors
        public static bool HasWriteMetadataErrors(Metadata metadataRead,    /* Data read back after saved and need to be verifyed */
            List<Metadata> metadataWrittenByExiftoolWaitVerify,             /* This what should have been saved, check if same info read back */ 
            out Metadata metadataUpdatedByUserCopy, out string message)
        {
            //Out parameter default
            message = "";
            metadataUpdatedByUserCopy = null;

            bool foundErrors = false;

            int verifyPosition = Metadata.FindFileEntryInList(metadataWrittenByExiftoolWaitVerify, metadataRead.FileEntryBroker);
            if (verifyPosition == -1) return false; //No need for verify, the metadata was only read, most likly first time read (without save, read and verify)

            metadataUpdatedByUserCopy = new Metadata(metadataWrittenByExiftoolWaitVerify[verifyPosition]); //Copy data to verify
            metadataWrittenByExiftoolWaitVerify.RemoveAt(verifyPosition);

            bool foundOldVersionToVerify = false; //Happens when multiple save are done and save faild, and veridify was not done for each media file
            do
            {
                foundOldVersionToVerify = false;
                int indexFound = Metadata.FindFullFilenameInList(metadataWrittenByExiftoolWaitVerify, metadataRead.FileFullPath);
                if (indexFound > -1 && indexFound < metadataWrittenByExiftoolWaitVerify.Count)
                {
                    metadataWrittenByExiftoolWaitVerify.RemoveAt(indexFound);
                    foundOldVersionToVerify = true;
                }
            } while (foundOldVersionToVerify);

            metadataUpdatedByUserCopy.FileDateModified = metadataRead.FileDateModified;   //After save, this was updated
            metadataUpdatedByUserCopy.FileLastAccessed = metadataRead.FileLastAccessed;   //This has changed, do not care
            metadataUpdatedByUserCopy.FileSize = metadataRead.FileSize;                   //This has changed, do not care
            metadataUpdatedByUserCopy.Errors = metadataRead.Errors;                       //This has changed, do not care, Hopefully this is gone
            metadataUpdatedByUserCopy.Broker = metadataRead.Broker;                       //This has changed, do not care

            if (metadataUpdatedByUserCopy.MediaHeight == metadataRead.MediaWidth &&
                metadataUpdatedByUserCopy.MediaWidth == metadataRead.MediaHeight) //Media has been Rotated
            {
                metadataUpdatedByUserCopy.MediaHeight = metadataRead.MediaHeight;
                metadataUpdatedByUserCopy.MediaWidth = metadataRead.MediaWidth;
            }
            
            if (metadataRead != metadataUpdatedByUserCopy)
            {
                message += "Filename: '" + metadataUpdatedByUserCopy.FileFullPath + "'\r\n" +
                    "Errors:\r\n" + Metadata.GetErrors(metadataUpdatedByUserCopy, metadataRead) + "\r\n-----------\r\n\r\n";
                Logger.Error("Verify metatdata failed! Data read back not equal to was supposted to be written on file: " + metadataUpdatedByUserCopy.FileFullPath);

                foundErrors = true;
            }                
            
            return foundErrors;  
        }
        #endregion
    }


}
