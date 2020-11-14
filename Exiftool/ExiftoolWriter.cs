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

/*
https://exiftool.org/forum/index.php?topic=7178.0
I did some testing to try and figure out the priority of which metadata Google Photos uses as the date for images.  Here are the results from highest priority to lowest.

IPTC:DateCreated + IPTC:TimeCreated
EXIF:DateTimeOriginal
XMP:DateTimeOriginal
EXIF:CreateDate
XMP:CreateDate
EXIF:ModifyDate
XMP:ModifyDate
GPS:GPSDateStamp + GPS:GPSTimeStamp + TimeZone Modifier (-6 hours in my test)
System:FileModifyDate +Timezone modifier.  This one was odd, as it was different than the gps timezone modifier.  My computer's current timezone is -7 hours, but Google Photos set it to -4 hours and adjusted the time.  So, the file was set to 12:20 and changed it to 15:20.

Personally, I would suggest making sure that EXIF:DateTimeOriginal is set properly as that is a better standard in my opinion.   IPTC:DateCreated + IPTC:TimeCreated are not set as often in most pictures straight out of a camera in my experience.

Edit:  Checking for other metadata.  Google Photos seems to ignore everything else except a description.  It only checks IPTC:Caption-Abstract and XMP:Description, in that order.

----
https://exiftool.org/forum/index.php/topic,6959.0.html 
Lightroom Metadata	Actual Tags Written
Capture Time	EXIF:DateTimeOriginal
IPTC:DateCreated
IPTC:TimeCreated
XMP:DateCreated
Persons Shown	XMP:PersonInImage
Title	XMP:Title
IPTC:ObjectName
Label	XMP:Label
Rating	XMP:Rating
Caption	EXIF:ImageDescription
IPTC:Caption-Abstract
XMP:Description
Note: If there are multiple lines, Lightroom will use Carriage Returns in Caption-Abstract, but Line Feeds in ImageDescription and Description
User Comment	EXIF:UserComment
GPS Data
GPS
Altitude
Writes to EXIF GPS block.  Will erase XMP GPS data if it exists.
GPSLatitudeRef
GPSLatitude
GPSLongitudeRef
GPSLongitude
GPSAltitude
GPSTimeStamp
GPSImgDirectionRef
GPSImgDirection
GPSMapDatum
Contact->Creator	EXIF:Artist
IPTC:By-line
XMP:Creator


f you have to write multiple regions, you can just add them on, but you must keep names and the matching dimensions in the same order. For example

exiftool 
-RegionPersonDisplayName=findus_l 
-RegionRectangle="0.48, 0.418, 0.059333, 0.089" 
-RegionPersonDisplayName="John Smith" 
-RegionRectangle="0.37645533, 0.04499886, 0.35111009, 0.26633097" 
/path/to/files


These commands would overwrite any existing region data. If you are adding new names without overwriting, you would change the equal signs to PlusEqual +=.
*/


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
        public static void WriteMetadata(List<Metadata> metadataListToWrite, List<Metadata> metadataListOriginal, int writeCount,
            List<string> allowedFileNameDateTimeFormats,
            string writeMetadataTags, string writeMetadataKeywordItems, bool writeAlbumProperties, bool writeKeywordProperties)
        {
            if (writeCount == 0) return;
            if (metadataListToWrite.Count != metadataListOriginal.Count) return;

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
                    Logger.Info("Create EXIF updated argu file for: " + metadataToWrite.FileFullPath);

                    bool isVideoFormat = false;
                    bool isImageFormat = true;
                    if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(Path.Combine(metadataToWrite.FileDirectory, metadataToWrite.FileName)))
                    {
                        isVideoFormat = true;
                        isImageFormat = false;
                    }

                    if (isVideoFormat && writeAlbumProperties)
                    {
                        WindowsProperty.WindowsPropertyWriter windowsPropertyWriter = new WindowsProperty.WindowsPropertyWriter();
                        string keywordTags = "";
                        foreach (KeywordTag keywordTag in metadataToWrite.PersonalKeywordTags)
                        {
                            if (keywordTags.Length > 0) keywordTags += ";";
                            keywordTags += keywordTag;
                        }

                        windowsPropertyWriter.Write(Path.Combine(metadataToWrite.FileDirectory, metadataToWrite.FileName), keywordTags);
                    }

                    if (isVideoFormat && writeKeywordProperties)
                    {
                        WindowsProperty.WindowsPropertyWriter windowsPropertyWriter2 = new WindowsProperty.WindowsPropertyWriter();
                        windowsPropertyWriter2.WriteAlbum(Path.Combine(metadataToWrite.FileDirectory, metadataToWrite.FileName), metadataToWrite.PersonalAlbum);
                    }


                    //-Categories={PersonalKeywordsXML}
                    #region Write Keyword XML string 
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

                    //-XPKeywords={PersonalKeywordsList}
                    #region Write Keywords String
                    string personalKeywordsList = "";
                    if (isImageFormat)
                    {
                        //Write 'Windows XP Explorer XPkeywords' in one line. Windows Explorer use ; as seperator
                        String keywords = "";
                        foreach (KeywordTag tag in metadataToWrite.PersonalKeywordTags)
                        {
                            if (!string.IsNullOrWhiteSpace(keywords))
                                keywords += "\t" + tag.Keyword;
                            else
                                keywords = tag.Keyword;
                        }
                        personalKeywordsList = keywords.Replace("\t", ";");
                    }
                    #endregion

                    //-ImageRegion=
                    #region IPTC region tags - ImageRegion
                    #endregion

                    //-RegionInfoMP={PersonalRegionInfoMP}
                    #region Microsoft region tags - RegionInfoMP 
                    string personalRegionInfoMP = "";                    
                    if (metadataToWrite.PersonalRegionList.Count > 0)
                    {
                        bool needComma = false;
                        personalRegionInfoMP += "{Regions=[";
                        foreach (RegionStructure region in metadataToWrite.PersonalRegionList)
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

                    //-RegionInfo={PersonalRegionInfo}
                    #region MWG Regions Tags - RegionInfo
                    string personalRegionInfo = "";
                    if (metadataToWrite.PersonalRegionList.Count > 0)
                    {
                        bool needComma = false;
                        personalRegionInfo += "-RegionInfo={AppliedToDimensions={W=" + metadataToWrite.MediaWidth + 
                            ",H=" + metadataToWrite.MediaHeight + 
                            ",Unit=pixel}," + 
                            "RegionList=[";
                        foreach (RegionStructure region in metadataToWrite.PersonalRegionList)
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


                    Logger.Debug("---MIMETYPE----:" + metadataToWrite.FileMimeType);
                    

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
                    }

                    #endregion

                    /*
                    -Categories={PersonalKeywordsXML}
                    -XPKeywords={PersonalKeywordsList}
                    -ImageRegion=
                    -RegionInfoMP={PersonalRegionInfoMP}
                    -RegionInfo={PersonalRegionInfo}
                    */

                    /*
                    string writeMetadataTags = Properties.Settings.Default.WriteMetadataTags;
                    string writeMetadataKeywordItems = Properties.Settings.Default.WriteMetadataKeywordItems;
                    bool writeAlbumProperties = Properties.Settings.Default.WriteMetadataPropertiesVideoAlbum;
                    bool writeKeywordProperties = Properties.Settings.Default.WriteMetadataPropertiesVideoKeywords;
                    */

                    string tagsToWrite = metadataToWrite.ReplaceVariables(writeMetadataTags, true, true, allowedFileNameDateTimeFormats, 
                        personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories);

                    sw.WriteLine(tagsToWrite);


                    #region Write Keyword ***List***

                    foreach (KeywordTag keywordTag in metadataOriginal.PersonalKeywordTags)
                    {
                        string keywordItemToWrite = metadataToWrite.ReplaceVariables(writeMetadataKeywordItems, true, true, allowedFileNameDateTimeFormats,
                            personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories);
                        keywordItemToWrite = metadataToWrite.ReplaceVariables(keywordItemToWrite, keywordTag.Keyword);

                        sw.WriteLine(keywordItemToWrite);                        
                    }

                    #endregion

                    sw.WriteLine(metadataToWrite.FileFullPath);
                    sw.WriteLine("-execute");
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

        }
        #endregion

        #region Verify HasWriteMetadataErrors
        public static bool HasWriteMetadataErrors(Metadata metadataRead, List<Metadata> metadataWaitingVerify, List<Metadata> metadataWaitingSaving, out Metadata metadataUpdatedByUserCopy, out string message)
        {
            //Out parameter default
            message = "";
            metadataUpdatedByUserCopy = null;

            bool foundErrors = false;

            int verifyPosition = Metadata.FindMetadataInList(metadataWaitingVerify, metadataRead);
            if (verifyPosition == -1) return false; //continue; //No need for verify

            if (Metadata.FindMetadataInList(metadataWaitingSaving, metadataRead) != -1) return false; //continue; //A new version waiting to be saves exists, not need to verify before saved

            metadataUpdatedByUserCopy = new Metadata(metadataWaitingVerify[verifyPosition]); //Copy data to verify
            metadataWaitingVerify.RemoveAt(verifyPosition);

            metadataUpdatedByUserCopy.FileDateModified = metadataRead.FileDateModified;   //This has changed, do not care
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
