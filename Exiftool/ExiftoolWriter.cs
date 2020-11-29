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

namespace Exiftool
{
    public static class ExiftoolWriter
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static object SystemProperties { get; private set; }

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

        #region Files locked, wait unlock
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

        public static bool IsFileReadOnlyOrLockedByProcess(string fullFilePath)
        {
            FileStream fs = null;            
            try
            {
                // NOTE: This doesn't handle situations where file is opened for writing by another process but put into write shared mode, it will not throw an exception and won't show it as write locked
                fs = File.Open(fullFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None); // If we can't open file for reading and writing then it's locked by another process for writing
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

        public static bool IsFileThatNeedUpdatedLockedByProcess(List<Metadata> metadataListToWrite)
        {
            if (metadataListToWrite.Count == 0) return false;

            foreach (Metadata metadataToWrite in metadataListToWrite)
            {
                if (!File.Exists(metadataToWrite.FileFullPath)) return true; //In process rename
                if (IsFileLockedByProcess(metadataToWrite.FileFullPath)) return true; //In process OneDrive backup / update
            }

            return false;
        }

        public static void WaitLockedFilesToBecomeUnlocked(List<Metadata> metadataListToWrite)
        {
            int maxRetry = 30;
            bool areAnyFileLocked;
            do
            {
                areAnyFileLocked = ExiftoolWriter.IsFileThatNeedUpdatedLockedByProcess(metadataListToWrite);
                if (areAnyFileLocked) Thread.Sleep(1000);
                if (maxRetry-- < 0)
                    areAnyFileLocked = false;
            } while (areAnyFileLocked);
        }

        #endregion

        #region WriteMetadata
        public static List<Metadata> WriteMetadata(List<Metadata> metadataListToWrite, List<Metadata> metadataListOriginal, 
            List<string> allowedFileNameDateTimeFormats, string writeMetadataTags, string writeMetadataKeywordDelete, string writeMetadataKeywordAdd,
            string writeXtraAtomAlbum, bool writeXtraAtomAlbumVideo,
            string writeXtraAtomCategories, bool writeXtraAtomCategoriesVideo,
            string writeXtraAtomComment, bool writeXtraAtomCommentPicture, bool writeXtraAtomCommentVideo,
            string writeXtraAtomKeywords, bool writeXtraAtomKeywordsVideo,
            bool writeXtraAtomRatingPicture, bool writeXtraAtomRatingVideo,
            string writeXtraAtomSubject, bool writeXtraAtomSubjectPicture, bool wtraAtomSubjectVideo,
            string writeXtraAtomSubtitle, bool writeXtraAtomSubtitleVideo,
            string writeXtraAtomArtist, bool writeXtraAtomArtistVideo)
        {
            List<Metadata> metadataSaved = new List<Metadata>();

            if (metadataListToWrite.Count <= 0) return metadataSaved;
            if (metadataListToWrite.Count != metadataListOriginal.Count) return metadataSaved;
            int writeCount = metadataListToWrite.Count;

            //Create directory, filename and remove old arg file
            string exiftoolArgPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            if (!Directory.Exists(exiftoolArgPath)) Directory.CreateDirectory(exiftoolArgPath);
            string exiftoolArgFile = Path.Combine(exiftoolArgPath, "exiftool_arg.txt");
            if (File.Exists(exiftoolArgFile)) File.Delete(exiftoolArgFile);
            
            using (StreamWriter sw = new StreamWriter(exiftoolArgFile, false, Encoding.UTF8))
            {
                for (int updatedRecord = 0; updatedRecord < writeCount; updatedRecord++)
                {
                    Metadata metadataToWrite = metadataListToWrite[updatedRecord];
                    Metadata metadataOriginal = metadataListOriginal[updatedRecord];

                    if (metadataToWrite == metadataOriginal) continue; //No changes found in data, No data to write
                    metadataSaved.Add(metadataToWrite);
                    Logger.Info("Create EXIF updated argu file for: " + metadataToWrite.FileFullPath);

                    bool isVideoFormat = false;
                    bool isImageFormat = true;
                    if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(Path.Combine(metadataToWrite.FileDirectory, metadataToWrite.FileName)))
                    {
                        isVideoFormat = true;
                        isImageFormat = false;
                    }


                    #region Create Variable -XPKeywords={PersonalKeywordsList}
                    string personalKeywordsList = "";
                    foreach (KeywordTag keywordTag in metadataToWrite.PersonalKeywordTags)
                    {
                        if (personalKeywordsList.Length > 0) personalKeywordsList += ";";
                        personalKeywordsList += keywordTag;
                    }
                    #endregion 
                    
                    #region Create Variable -Categories={PersonalKeywordsXML}
                    string keywordCategories = "<Categories>";
                    foreach (KeywordTag tagHierarchy in metadataToWrite.PersonalKeywordTags)
                    {
                        string[] tagHierarchyList = tagHierarchy.Keyword.Split('/');
                        for (int tagNumber = 0; tagNumber < tagHierarchyList.Length; tagNumber++)
                        {
                            if (tagNumber == tagHierarchyList.Length - 1) keywordCategories += "<Category Assigned=\"1\">";
                            else keywordCategories += "<Category Assigned=\"0\">";

                            keywordCategories += tagHierarchyList[tagNumber];
                        }
                        for (int tagNumber = 0; tagNumber < tagHierarchyList.Length; tagNumber++) keywordCategories += "</Category>";
                    }
                    keywordCategories += "</Categories>";
                    #endregion

                    #region Remove duplicates Name and Area regions (Don't care about source)
                    List<RegionStructure> regionWriteListWithoutDuplicate = new List<RegionStructure>();
                    foreach (RegionStructure regionStructure in metadataToWrite.PersonalRegionList)
                        if (!regionStructure.DoesThisRectangleAndNameExistInList(regionWriteListWithoutDuplicate)) regionWriteListWithoutDuplicate.Add(regionStructure);
                    #endregion

                    #region Create Variable -ImageRegion= (IPTC region tags - ImageRegion)
                    #endregion

                    #region Create Variable -RegionInfoMP={PersonalRegionInfoMP} - Microsoft region tags 
                    string personalRegionInfoMP = "";                    
                    if (regionWriteListWithoutDuplicate.Count > 0)
                    {
                        bool needComma = false;
                        personalRegionInfoMP += "{Regions=[";
                        foreach (RegionStructure region in regionWriteListWithoutDuplicate)
                        {
                            RectangleF rectangleF = region.GetRegionInfoMPRectangleF(metadataToWrite.MediaSize);
                            if (needComma) personalRegionInfoMP += ",";
                            personalRegionInfoMP += "{PersonDisplayName=" + region.Name + ",Rectangle=" +
                                rectangleF.X.ToString(CultureInfo.InvariantCulture) + "|," +
                                rectangleF.Y.ToString(CultureInfo.InvariantCulture) + "|," +
                                rectangleF.Width.ToString(CultureInfo.InvariantCulture) + "|," +
                                rectangleF.Height.ToString(CultureInfo.InvariantCulture) + "}";
                            needComma = true;
                        }
                        personalRegionInfoMP += "]}";
                    }
                    #endregion

                    #region Create Variable -RegionInfo={PersonalRegionInfo} - MWG Regions Tags 
                    string personalRegionInfo = "";
                    if (regionWriteListWithoutDuplicate.Count > 0)
                    {
                        bool needComma = false;
                        personalRegionInfo += "{AppliedToDimensions={W=" + metadataToWrite.MediaWidth + 
                            ",H=" + metadataToWrite.MediaHeight + 
                            ",Unit=pixel}," + 
                            "RegionList=[";
                        foreach (RegionStructure region in regionWriteListWithoutDuplicate)
                        {
                            RectangleF rectangleF = region.GetRegionInfoRectangleF(metadataToWrite.MediaSize);
                            if (needComma) personalRegionInfo += ",";
                            personalRegionInfo += "{Area={W=" + rectangleF.Width.ToString(CultureInfo.InvariantCulture) +
                                ",H=" + rectangleF.Height.ToString(CultureInfo.InvariantCulture) +
                                ",X=" + rectangleF.X.ToString(CultureInfo.InvariantCulture) +
                                ",Y=" + rectangleF.Y.ToString(CultureInfo.InvariantCulture) +
                                ",Unit=normalized},Name=" + region.Name + "}";
                            needComma = true;
                        }
                        personalRegionInfo += "]}";
                    }

                    /*
                    if (isVideoFormat)
                    { 
                        WindowsProperty.WindowsPropertyWriter windowsPropertyWriter = new WindowsProperty.WindowsPropertyWriter();
                        string keywordTags = "";
                        foreach (KeywordTag keywordTag in metadataToWrite.PersonalKeywordTags)
                        {
                            if (keywordTags.Length > 0) keywordTags +=  ";";
                            keywordTags += keywordTag;
                        }

                        windowsPropertyWriter.Write(Path.Combine(metadataToWrite.FileDirectory, metadataToWrite.FileName), keywordTags);
                    }

                    if (isVideoFormat)
                    {
                        WindowsProperty.WindowsPropertyWriter windowsPropertyWriter2 = new WindowsProperty.WindowsPropertyWriter();
                        windowsPropertyWriter2.WriteAlbum(Path.Combine(metadataToWrite.FileDirectory, metadataToWrite.FileName), metadataToWrite.PersonalAlbum);
                    }

                    foreach (KeywordTag tag in metadataToWrite.PersonalKeywordTags)
                    {                        
                        sw.WriteLine("-Keywords+=" + tag.Keyword);
                        sw.WriteLine("-Subject+=" + tag.Keyword);
                        sw.WriteLine("-TagsList+=" + tag.Keyword);
                        sw.WriteLine("-CatalogSets+=" + tag.Keyword);
//if (isImageFormat) sw.WriteLine("-Category+=" + tag.Keyword);
                        //Warning: Sorry, ItemList is not writable
                        //sw.WriteLine("-LastKeywordXMP+=" + tag.Keyword);  //Warning: [minor] Fixed incorrect URI for xmlns:MicrosoftPhoto
                        //sw.WriteLine("-LastKeywordIPTC+=" + tag.Keyword); //Warning: [minor] Fixed incorrect URI for xmlns:MicrosoftPhoto                         
                    }*/

                    #endregion

                    #region Create Variable - Keyword delete
                    string personalKeywordDelete = "";
                    foreach (KeywordTag keywordTag in metadataOriginal.PersonalKeywordTags)
                    {
                        string personalItemDelete = metadataToWrite.ReplaceVariables(writeMetadataKeywordDelete, true, true, allowedFileNameDateTimeFormats,
                            personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, "", "");
                        personalItemDelete = metadataToWrite.ReplaceKeywordItemVariables(personalItemDelete, keywordTag.Keyword);

                        personalKeywordDelete += personalItemDelete + "\r\n";
                    }
                    #endregion

                    #region Create Variable - Keyword items - ***Loop of keyword items***
                    string personalKeywordAdd = ""; 
                    foreach (KeywordTag keywordTag in metadataToWrite.PersonalKeywordTags)
                    {
                        string keywordItemToWrite = metadataToWrite.ReplaceVariables(writeMetadataKeywordAdd, true, true, allowedFileNameDateTimeFormats,
                            personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, "", "");
                        keywordItemToWrite = metadataToWrite.ReplaceKeywordItemVariables(keywordItemToWrite, keywordTag.Keyword);

                        personalKeywordAdd += keywordItemToWrite + "\r\n";
                    }
                    #endregion

                    #region Replace Variable 
                    string tagsToWrite = metadataToWrite.RemoveLines(writeMetadataTags, metadataOriginal, false);
                    tagsToWrite = metadataToWrite.ReplaceVariables(tagsToWrite, true, true, allowedFileNameDateTimeFormats,
                        personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, personalKeywordDelete, personalKeywordAdd);
                    string writeXtraAtomAlbumReult = metadataToWrite.ReplaceVariables(writeXtraAtomAlbum, true, true, allowedFileNameDateTimeFormats,
                        personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, personalKeywordDelete, personalKeywordAdd);
                    string writeXtraAtomCategoriesResult = metadataToWrite.ReplaceVariables(writeXtraAtomCategories, true, true, allowedFileNameDateTimeFormats,
                        personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, personalKeywordDelete, personalKeywordAdd);
                    string writeXtraAtomCommentResult = metadataToWrite.ReplaceVariables(writeXtraAtomComment, true, true, allowedFileNameDateTimeFormats,
                        personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, personalKeywordDelete, personalKeywordAdd);
                    string writeXtraAtomKeywordsResult = metadataToWrite.ReplaceVariables(writeXtraAtomKeywords, true, true, allowedFileNameDateTimeFormats,
                        personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, personalKeywordDelete, personalKeywordAdd);
                    string writeXtraAtomSubjectResult = metadataToWrite.ReplaceVariables(writeXtraAtomSubject, true, true, allowedFileNameDateTimeFormats,
                        personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, personalKeywordDelete, personalKeywordAdd);
                    string writeXtraAtomSubtitleResult = metadataToWrite.ReplaceVariables(writeXtraAtomSubtitle, true, true, allowedFileNameDateTimeFormats,
                        personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, personalKeywordDelete, personalKeywordAdd);
                    #endregion

                    using (WindowsPropertyWriter windowsPropertyWriter = new WindowsPropertyWriter(metadataToWrite.FileFullPath))
                    {
                        if (isVideoFormat)
                        {
                            if (writeXtraAtomKeywordsVideo) windowsPropertyWriter.WriteKeywords(writeXtraAtomKeywordsResult);
                            if (writeXtraAtomCategoriesVideo) windowsPropertyWriter.WriteCategories(writeXtraAtomCategoriesResult);
                            if (writeXtraAtomAlbumVideo) windowsPropertyWriter.WriteAlbum(writeXtraAtomAlbumReult);

                            if (writeXtraAtomSubtitleVideo) windowsPropertyWriter.WriteSubtitle_Description(writeXtraAtomSubtitleResult);
                            if (writeXtraAtomArtistVideo) windowsPropertyWriter.WriteArtist_Author(metadataToWrite.PersonalAuthor);

                            if (wtraAtomSubjectVideo) windowsPropertyWriter.WriteSubject_Description(writeXtraAtomSubjectResult);
                            if (writeXtraAtomCommentVideo) windowsPropertyWriter.WriteComment(metadataToWrite.PersonalComments);
                            if (writeXtraAtomRatingVideo) windowsPropertyWriter.WriteRating((metadataToWrite.PersonalRatingPercent == null ? (int)0 : (int)metadataToWrite.PersonalRatingPercent));
                        }
                        else if (isImageFormat)
                        {
                            if (writeXtraAtomSubjectPicture) windowsPropertyWriter.WriteSubject_Description(writeXtraAtomSubjectResult);
                            if (writeXtraAtomCommentPicture) windowsPropertyWriter.WriteComment(metadataToWrite.PersonalComments);
                            if (writeXtraAtomRatingPicture) windowsPropertyWriter.WriteRating((metadataToWrite.PersonalRatingPercent == null ? (int)0 : (int)metadataToWrite.PersonalRatingPercent));
                        }
                        windowsPropertyWriter.Close();
                    }

                    sw.WriteLine(tagsToWrite);
                } 
            }


            #region Exiftool Write
            String path = NativeMethods.GetFullPathOfExeFile("exiftool.exe");
            string arguments = "-charset utf8 -charset iptc=utf8 -codedcharacterset=utf8 -m -@ \"" + NativeMethods.ShortFileName(exiftoolArgFile) + "\"";
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


                    //Extra 
                    RedirectStandardInput = true,
                    StandardOutputEncoding = Encoding.UTF8 //,

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

                while (!process.HasExited) System.Threading.Thread.Sleep(100);
     
                process.Close();
                process.Dispose();

            }
            if (hasExiftoolErrorMessage)  
                throw new Exception(exiftoolOutput);
                        #endregion

            return metadataSaved;
        }
#endregion

                        #region Verify HasWriteMetadataErrors
        public static bool HasWriteMetadataErrors(Metadata metadataRead, 
            List<Metadata> metadataWrittenByExiftoolWaitVerify,       /* Data was got to by  after saved */ 
            List<Metadata> metadataSaveExiftoolParameter,   /* Data sent to exiftool for saving */ 
            out Metadata metadataUpdatedByUserCopy, out string message)
        {
            //Out parameter default
            message = "";
            metadataUpdatedByUserCopy = null;

            bool foundErrors = false;

            int verifyPosition = Metadata.FindFileEntryInList(metadataWrittenByExiftoolWaitVerify, metadataRead.FileEntryBroker);
            if (verifyPosition == -1) 
                return false; //No need for verify, the metadata was only read, most likly first time read (without save, read and verify)

            //if (Metadata.FindFileEntryInList(metadataSaveExiftoolParameter, metadataRead.FileEntryBroker) != -1) 
            //    return false; //A new version waiting to be saves exists, not need to verify before saved

            metadataUpdatedByUserCopy = new Metadata(metadataWrittenByExiftoolWaitVerify[verifyPosition]); //Copy data to verify
            metadataWrittenByExiftoolWaitVerify.RemoveAt(verifyPosition);

            metadataUpdatedByUserCopy.FileDateModified = metadataRead.FileDateModified;   //After save, this was updated
            metadataUpdatedByUserCopy.FileLastAccessed = metadataRead.FileLastAccessed;   //This has changed, do not care
            metadataUpdatedByUserCopy.FileSize = metadataRead.FileSize;                   //This has changed, do not care
            metadataUpdatedByUserCopy.Errors = metadataRead.Errors;                       //This has changed, do not care, Hopefully this is gone
            metadataUpdatedByUserCopy.Broker = metadataRead.Broker;                       //This has changed, do not care

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
