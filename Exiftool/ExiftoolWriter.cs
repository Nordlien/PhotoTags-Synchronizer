using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using NLog;
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
                if (metadataListOriginal[i] != metadataListToWrite[i]) 
                    listOfUpdates.Add(i);
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
            string writeXtraAtomKeywordsVariable, bool writeXtraAtomKeywordsPicture, bool writeXtraAtomKeywordsVideo,
            bool writeXtraAtomRatingPicture, bool writeXtraAtomRatingVideo,
            string writeXtraAtomSubjectVariable, bool writeXtraAtomSubjectPicture, bool writeXtraAtomSubjectVideo,
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
                if (writeXtraAtomKeywordsPicture || writeXtraAtomKeywordsVideo || writeXtraAtomCategoriesVideo || writeXtraAtomAlbumVideo || writeXtraAtomSubtitleVideo ||
                    writeXtraAtomArtistVideo || writeXtraAtomSubjectVideo || writeXtraAtomCommentVideo || writeXtraAtomRatingVideo ||
                    writeXtraAtomSubjectPicture || writeXtraAtomCommentPicture || writeXtraAtomRatingPicture)
                {

                    try
                    {
                        Logger.Debug("WriteXtraAtom - Start write XtraAtom: " + metadataToWrite.FileFullPath);
                        using (WindowsPropertyWriter windowsPropertyWriter = new WindowsPropertyWriter(metadataToWrite.FileFullPath))
                        {
                            if (isVideoFormat)
                            {
                                if (writeXtraAtomCategoriesVideo) windowsPropertyWriter.WriteCategories(string.IsNullOrEmpty(writeXtraAtomCategoriesResult) ? null : writeXtraAtomCategoriesResult);
                                if (writeXtraAtomAlbumVideo) windowsPropertyWriter.WriteAlbum(string.IsNullOrEmpty(writeXtraAtomAlbumReult) ? null : writeXtraAtomAlbumReult);
                                if (writeXtraAtomSubtitleVideo) windowsPropertyWriter.WriteSubtitle_Description(string.IsNullOrEmpty(writeXtraAtomSubtitleResult) ? null : writeXtraAtomSubtitleResult);
                                if (writeXtraAtomArtistVideo) windowsPropertyWriter.WriteArtist_Author(string.IsNullOrEmpty(writeXtraAtomArtistResult) ? null : writeXtraAtomArtistResult);
                                if (writeXtraAtomSubjectVideo) windowsPropertyWriter.WriteSubject_Description(string.IsNullOrEmpty(writeXtraAtomSubjectResult) ? null : writeXtraAtomSubjectResult);
                                if (writeXtraAtomCommentVideo) windowsPropertyWriter.WriteComment(string.IsNullOrEmpty(writeXtraAtomCommentResult) ? null : writeXtraAtomCommentResult);
                                if (writeXtraAtomRatingVideo) windowsPropertyWriter.WriteRating((metadataToWrite.PersonalRatingPercent == null ? (int)0 : (int)metadataToWrite.PersonalRatingPercent));
                                if (writeXtraAtomKeywordsVideo) windowsPropertyWriter.WriteKeywords(string.IsNullOrEmpty(writeXtraAtomKeywordsResult) ? null : writeXtraAtomKeywordsResult);
                            }
                            else if (isImageFormat)
                            {
                                if (writeXtraAtomSubjectPicture) windowsPropertyWriter.WriteSubject_Description(string.IsNullOrEmpty(writeXtraAtomSubjectResult) ? null : writeXtraAtomSubjectResult);
                                if (writeXtraAtomCommentPicture) windowsPropertyWriter.WriteComment(string.IsNullOrEmpty(writeXtraAtomCommentResult) ? null : writeXtraAtomCommentResult);
                                if (writeXtraAtomRatingPicture) windowsPropertyWriter.WriteRating((metadataToWrite.PersonalRatingPercent == null ? (int)0 : (int)metadataToWrite.PersonalRatingPercent));
                                if (writeXtraAtomKeywordsPicture) windowsPropertyWriter.WriteKeywords(string.IsNullOrEmpty(writeXtraAtomKeywordsResult) ? null : writeXtraAtomKeywordsResult);
                            }

                            windowsPropertyWriter.Close();

                            filesUpdatedByXtraAtom.Add(new FileEntry(metadataToWrite.FileFullPath, File.GetLastWriteTime(metadataToWrite.FileFullPath)));
                        }
                    }
                    catch (Exception ex)
                    {
                        string error = "Failed to write Microsoft's own Xtra Atom Propery on file. Exception message: " + ex.Message;
                        Logger.Error("File name: " + metadataToWrite.FileFullPath + " Error Message: " + error);
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
            string writeMetadataTagsVariable, string writeMetadataKeywordAddVariable, bool alwaysWrite, 
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

                string personalKeywordAddVariable = metadataToWrite.ReplaceVariables(writeMetadataKeywordAddVariable, allowedFileNameDateTimeFormats);

                string personalKeywordAddItems = metadataToWrite.VariablePersonalKeywords(personalKeywordAddVariable, allowedFileNameDateTimeFormats);

                tagsToWrite = metadataToWrite.ReplaceVariables(tagsToWrite, allowedFileNameDateTimeFormats, personalKeywordAddItems);
                if (!string.IsNullOrWhiteSpace(tagsToWrite)) exiftoolArguFileText += (string.IsNullOrWhiteSpace(exiftoolArguFileText) ? "" : "\r\n") + tagsToWrite;
            }
            return filesNeedToBeUpadted;
        }
        #endregion  

        #region WriteMetadata
        public static void WriteMetadata(List<Metadata> metadataListToWrite, List<Metadata> metadataListOriginal, List<string> allowedFileNameDateTimeFormats,
            string writeMetadataTagsVariable, string writeMetadataKeywordAddVariable, 
            out List<FileEntry> mediaFilesWithChangesWillBeUpdated, bool showCliWindow, ProcessPriorityClass processPriorityClass)
        {
            mediaFilesWithChangesWillBeUpdated = CreateExiftoolArguFileText(
                metadataListToWrite, metadataListOriginal, allowedFileNameDateTimeFormats, writeMetadataTagsVariable, writeMetadataKeywordAddVariable, 
                false, out string resultReplaceVariables);

            if (mediaFilesWithChangesWillBeUpdated.Count > 0) //Save if has anything to save
            {
                Logger.Debug("WriteMetadata: started");
                //Create directory, filename and remove old arg file
                string exiftoolArgFileFullpath = FileHandler.GetLocalApplicationDataPath("exiftool_arg.txt", true, null);

                using (StreamWriter sw = new StreamWriter(exiftoolArgFileFullpath, false, Encoding.UTF8))
                {
                    sw.WriteLine(resultReplaceVariables);
                }

                #region Exiftool Write
                String path = NativeMethods.GetFullPathOfFile("exiftool.exe");
                //-iptc:all -codedcharacterset=utf8 CodedCharacterSet UTF8  -charset iptc=utf8
                //-charset utf8 -charset iptc=utf8 -codedcharacterset=utf8 
                string arguments = "-@ \"" + NativeMethods.ShortFileName(exiftoolArgFileFullpath) + "\"";
                bool hasExiftoolErrorMessage = false;
                string exiftoolOutput = "";

                using (var process = new Process())
                {
                    string line;
                    process.StartInfo.FileName = path;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = !showCliWindow;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.StandardOutputEncoding = Encoding.UTF8;

                    process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            line = e.Data;
                            exiftoolOutput += line + "\r\n";
                            if (line.StartsWith("Error")) hasExiftoolErrorMessage = true;
                            Logger.Info("EXIFTOOL WRITE OUTPUT: " + line);
                        }
                    });

                    process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {                          
                            line = e.Data;
                            exiftoolOutput += line + "\r\n";
                            if (!line.StartsWith("Warning")) hasExiftoolErrorMessage = true;
                            Logger.Error("EXIFTOOL WRITE ERROR: " + line);
                        }
                    });


                    Logger.Debug("WriteMetadata: process.Start arguments: " + arguments);
                    Logger.Debug("WriteMetadata: process.Start file content:\r\n" + resultReplaceVariables);
                    bool result = process.Start();
                    try
                    {
                        process.PriorityClass = processPriorityClass;
                    } catch { }
                    // Asynchronously read the standard output of the spawned process.
                    // This raises OutputDataReceived events for each line of output.
                    process.BeginOutputReadLine();

                    process.WaitForExit(1000 * 60 * 5); //5 minutes
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
            out Metadata metadataUpdatedByUserCopy, out string errorMessage)
        {
            //Out parameter default
            errorMessage = "";
            metadataUpdatedByUserCopy = null;

            if (metadataWrittenByExiftoolWaitVerify.Count == 0) 
                return false;

            bool foundErrors = false;

            int verifyPosition = Metadata.FindFullFilenameInList(metadataWrittenByExiftoolWaitVerify, metadataRead.FileEntryBroker.FileFullPath);
            if (verifyPosition != -1)
            {
                ///Remove from list and add back to Read Exif once more
                if (metadataRead.FileEntryBroker.LastWriteDateTime > metadataWrittenByExiftoolWaitVerify[verifyPosition].FileDateModified)
                {
                    string fileErrorMessage = "File has been updated between writing and read back using exiftool.\r\n" +
                        "This can occure when OneDrive, GoogleDrive, Dropbox, iDrive, Box etc... change dates during syncing files.\r\n" +
                        "File modified before Exiftool: " + metadataRead.FileEntryBroker.LastWriteDateTime.ToString() + "\r\n" +
                        "File modified after  Exiftool: " + metadataWrittenByExiftoolWaitVerify[verifyPosition].FileDateModified.ToString();
                    errorMessage += (string.IsNullOrWhiteSpace(errorMessage) ? "" : "\r\n") + fileErrorMessage;
                    Logger.Warn("File with error: " + metadataRead.FileFullPath + "\r\n" + errorMessage);
                    foundErrors = true;
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
                errorMessage += (string.IsNullOrWhiteSpace(errorMessage) ? "" : "\r\n") + 
                    "Metadata errors:\r\n" + Metadata.GetErrors(metadataUpdatedByUserCopy, metadataRead);
                Logger.Warn("File with error: " + metadataUpdatedByUserCopy.FileFullPath + "\r\n" + errorMessage);

                foundErrors = true;
            }                
            
            return foundErrors;  
        }
        #endregion
    }


}
