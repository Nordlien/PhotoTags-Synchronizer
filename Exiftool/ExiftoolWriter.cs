using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using NLog;
using System.Threading;
using WindowsProperty;
using ApplicationAssociations;
using FileHandeling;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exiftool
{

    public static class ExiftoolWriter
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
            out Dictionary<string, string> writeXtraAtomErrorMessageForFile, Form form)
        {
            Logger.Debug("WriteXtraAtom - started");
            writeXtraAtomErrorMessageForFile = new Dictionary<string, string>(); //Clear out values
            List<FileEntry> filesUpdatedByXtraAtom = new List<FileEntry>();
            
            if (metadataListToWrite.Count <= 0) return filesUpdatedByXtraAtom;
            if (metadataListToWrite.Count != metadataListOriginal.Count) return filesUpdatedByXtraAtom;
            int writeCount = metadataListToWrite.Count;

            for (int updatedRecord = 0; updatedRecord < writeCount; updatedRecord++)
            {
                Metadata metadataToWrite = metadataListToWrite[updatedRecord];
                Metadata metadataOriginal = metadataListOriginal[updatedRecord];

                Logger.Debug("WriteXtraAtom - " + metadataToWrite.FileFullPath);
                if (metadataToWrite == metadataOriginal) continue; //No changes found in data, No data to write

                #region Is Video or Image format?
                bool isVideoFormat = false;
                bool isImageFormat = false;

                if (ImageAndMovieFileExtentionsUtility.IsImageFormat(metadataToWrite.FileFullPath))
                {
                    isVideoFormat = false;
                    isImageFormat = true;
                }
                else if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(metadataToWrite.FileFullPath))
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
                    bool isFileUnLockedAndExist = FileHandler.WaitLockedFileToBecomeUnlocked(metadataToWrite.FileFullPath, true, form);

                    if (isFileUnLockedAndExist)
                    {
                        try
                        {
                            Logger.Debug("WriteXtraAtom - Start write XtraAtom: " + metadataToWrite.FileFullPath);
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

                                filesUpdatedByXtraAtom.Add(new FileEntry(metadataToWrite.FileFullPath, File.GetLastWriteTime(metadataToWrite.FileFullPath)));
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, "Failed write Xtra Atom Propery on file: " + metadataToWrite.FileFullPath);
                            writeXtraAtomErrorMessageForFile.Add(metadataToWrite.FileFullPath, ex.Message);
                        }
                    } else
                    { //File readonly or locked

                        string error = "Failed write Xtra Atom Propery on file: " + metadataToWrite.FileFullPath + "\r\n";
                        if (!File.Exists(metadataToWrite.FileFullPath)) error += "File not found.\r\n";
                        else if (FileHandler.IsFileReadOnly(metadataToWrite.FileFullPath)) error += "File is Read Only.\r\n";
                        else if (FileHandler.IsFileLockedByProcess(metadataToWrite.FileFullPath, FileHandler.GetFileLockedStatusTimeout)) error += "File is locked by another process.\r\n";
                        Logger.Error(error);
                        writeXtraAtomErrorMessageForFile.Add(metadataToWrite.FileFullPath, error);
                    }
                } else
                {
                    Logger.Debug("WriteXtraAtom - nothing to updated: " + metadataToWrite.FileFullPath);
                }
                #endregion

            }
            return filesUpdatedByXtraAtom;
        }
        #endregion

        #region CreateExiftoolArguFileText
        public static List<FileEntry> CreateExiftoolArguFileText(List<Metadata> metadataListToWrite, List<Metadata> metadataListOriginal, List<string> allowedFileNameDateTimeFormats,
            string writeMetadataTagsVariable, string writeMetadataKeywordDeleteVariable, string writeMetadataKeywordAddVariable, bool alwaysWrite, 
            out string exiftoolArguFileText)
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

                if (metadataOriginal.Broker == MetadataBrokerType.Empty) alwaysWrite = true;
                if (!alwaysWrite && metadataToWrite == metadataOriginal) continue; //No changes found in data, No data to write
                
                filesNeedToBeUpadted.Add(metadataToWrite.FileEntryBroker);

                string tagsToWrite = metadataToWrite.RemoveLines(writeMetadataTagsVariable, metadataOriginal, alwaysWrite);

                string personalKeywordDelete = metadataOriginal.ReplaceVariables(writeMetadataKeywordDeleteVariable, allowedFileNameDateTimeFormats);
                string personalKeywordAdd = metadataToWrite.ReplaceVariables(writeMetadataKeywordAddVariable, allowedFileNameDateTimeFormats);

                string personalKeywordDeleteItems = metadataOriginal.VariablePersonalKeywords(personalKeywordDelete, allowedFileNameDateTimeFormats);
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
        public static void WriteMetadata(List<Metadata> metadataListToWrite, List<Metadata> metadataListOriginal, List<string> allowedFileNameDateTimeFormats,
            string writeMetadataTagsVariable, string writeMetadataKeywordDeleteVariable, string writeMetadataKeywordAddVariable, 
            out List<FileEntry> mediaFilesWithChangesWillBeUpdated, bool showCliWindow, ProcessPriorityClass processPriorityClass)
        {
            mediaFilesWithChangesWillBeUpdated = CreateExiftoolArguFileText(
                metadataListToWrite, metadataListOriginal, allowedFileNameDateTimeFormats, writeMetadataTagsVariable, writeMetadataKeywordDeleteVariable, writeMetadataKeywordAddVariable, 
                false, out string resultReplaceVariables);

            if (mediaFilesWithChangesWillBeUpdated.Count > 0) //Save if has anything to save
            {
                Logger.Debug("WriteMetadata: started");
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
                        CreateNoWindow = !showCliWindow,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        RedirectStandardInput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    }
                })
                {                    
                    bool result = process.Start();
                    process.PriorityClass = processPriorityClass;
                    
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

                    while (!process.HasExited) Task.Delay(100).Wait();

                    process.Close();
                    process.Dispose();
                }
                Logger.Debug("WriteMetadata: ended");
                if (hasExiftoolErrorMessage) throw new Exception(exiftoolOutput);
                #endregion
            }
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
            if (verifyPosition == -1)
            {
                verifyPosition = Metadata.FindFullFilenameInList(metadataWrittenByExiftoolWaitVerify, metadataRead.FileEntryBroker.FileFullPath);
                if (verifyPosition != -1)
                {
                    //Debug way was not date updated, metadate read FileDateCreate date is Older Than Metadata acctual file.
                    //Remove from list and add back to Read Exif once more
                    if (metadataRead.FileEntryBroker.LastWriteDateTime > metadataWrittenByExiftoolWaitVerify[verifyPosition].FileDateModified)
                    {
                        message += "File been updated between read exiftool was run and verify: " + metadataRead.FileEntryBroker.FileFullPath + " " +
                            "File created: " + metadataRead.FileEntryBroker.LastWriteDateTime.ToString() + " " +
                            "Metadata file created: " + metadataWrittenByExiftoolWaitVerify[verifyPosition].FileDateModified.ToString();

                        Logger.Warn("File been updated between read exiftool was run and verify: " + metadataRead.FileEntryBroker.FileFullPath + " " +
                            "File created: " + metadataRead.FileEntryBroker.LastWriteDateTime.ToString() + " " +
                            "Metadata file created: " + metadataWrittenByExiftoolWaitVerify[verifyPosition].FileDateModified.ToString());
                        
                        foundErrors = true;
                    }
                }
            }
            if (verifyPosition == -1) return false; //No need for verify, the metadata was only read, most likly first time read (without save, read and verify)

            metadataUpdatedByUserCopy = new Metadata(metadataWrittenByExiftoolWaitVerify[verifyPosition]); //Copy data to verify
            metadataWrittenByExiftoolWaitVerify.RemoveAt(verifyPosition);

            //Remove old versions of "Need to be veriyfied"
            bool foundOldVersionToVerify; //Happens when multiple save are done and save faild, and veridify was not done for each media file
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

            //metadataUpdatedByUserCopy.FileDateModified = metadataRead.FileDateModified;   //After save, this was updated
            metadataUpdatedByUserCopy.FileDateAccessed = metadataRead.FileDateAccessed;   //This has changed, do not care
            metadataUpdatedByUserCopy.FileSize = metadataRead.FileSize;                   //This has changed, do not care
            metadataUpdatedByUserCopy.Errors = metadataRead.Errors;                       //This has changed, do not care, Hopefully this is gone
            metadataUpdatedByUserCopy.Broker = metadataRead.Broker;                       //This has changed, do not care

            if (metadataUpdatedByUserCopy.MediaHeight == metadataRead.MediaWidth &&
                metadataUpdatedByUserCopy.MediaWidth == metadataRead.MediaHeight) //Media has been Rotated
            {
                metadataUpdatedByUserCopy.MediaHeight = metadataRead.MediaHeight;
                metadataUpdatedByUserCopy.MediaWidth = metadataRead.MediaWidth;
            }

            if (metadataRead.TryParseDateTakenToUtc(out DateTime? dateTimeOffset) && 
                Math.Abs((((DateTime)dateTimeOffset).ToUniversalTime() - ((DateTime)metadataRead.FileDateCreated).ToUniversalTime()).TotalSeconds) < 2)
            {
                metadataUpdatedByUserCopy.FileDateCreated = metadataRead.FileDateCreated; //File create date has been set to Media Taken
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
