using System;

namespace MetadataLibrary
{
    public enum FileEntryVersion
    {
        NotAvailable,
        CurrentVersionInDatabase,
        Historical,
        Error,
        MetadataToSave,
        CompatibilityFixedAndAutoUpdated,
        ExtractedNowUsingExiftoolWithError,
        ExtractedNowUsingExiftoolTimeout,
        ExtractedNowUsingExiftoolFileNotExist,
        ExtractedNowUsingExiftool,
        ExtractedNowUsingReadMediaFile,
        ExtractedNowUsingWindowsLivePhotoGallery,
        ExtractedNowUsingMicrosoftPhotos,
        ExtractedNowUsingWebScraping
    }

    public enum FileEntryVersionCompare
    {
        WonFoundEqual,
        WonFoundNewer,
        LostFoundOlder,
        LostNoneEqualFound,
        LostOverUserInput,
        WonColumnCreatedHistoricalOrError,
        WonColumnCredtedCurrent
    }

    public class FileEntryVersionHandler
    {
        #region IsCurrenOrUpdatedVersion
        public static bool IsCurrenOrUpdatedVersion(FileEntryVersion fileEntryVersion)
        {
            return 
                fileEntryVersion == FileEntryVersion.CurrentVersionInDatabase || 
                fileEntryVersion == FileEntryVersion.CompatibilityFixedAndAutoUpdated ||
                fileEntryVersion == FileEntryVersion.MetadataToSave ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingExiftool || 
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingReadMediaFile ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingMicrosoftPhotos ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingWebScraping;
        }
        #endregion

        #region IsErrorOrHistoricalVersion
        public static bool IsErrorOrHistoricalVersion(FileEntryVersion fileEntryVersion)
        {
            //fileEntryAttribute.FileEntryVersion != FileEntryVersion.AutoCorrect && fileEntryAttribute.FileEntryVersion != FileEntryVersion.Curren
            return fileEntryVersion == FileEntryVersion.Historical || fileEntryVersion == FileEntryVersion.Error;
        }
        #endregion

        #region ConvertCurrentErrorHistorical
        public static FileEntryVersion ConvertCurrentErrorHistorical(FileEntryVersion fileEntryVersion)
        {
            switch (fileEntryVersion)
            {
                case FileEntryVersion.MetadataToSave:
                case FileEntryVersion.CompatibilityFixedAndAutoUpdated:
                case FileEntryVersion.CurrentVersionInDatabase:
                case FileEntryVersion.ExtractedNowUsingExiftool:
                case FileEntryVersion.ExtractedNowUsingReadMediaFile:
                case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:
                case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:
                case FileEntryVersion.ExtractedNowUsingWebScraping:
                    return FileEntryVersion.CurrentVersionInDatabase;

                case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:
                case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:
                case FileEntryVersion.ExtractedNowUsingExiftoolWithError:
                case FileEntryVersion.Error:
                    return FileEntryVersion.Error;
                case FileEntryVersion.Historical:
                    return FileEntryVersion.Historical;
                case FileEntryVersion.NotAvailable:
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region ToStringShort
        public static string ToStringShort(FileEntryVersion fileEntryVersion)
        {
            switch (fileEntryVersion)
            {
                case FileEntryVersion.MetadataToSave:
                    return "Save and Compatibility checked";
                case FileEntryVersion.CompatibilityFixedAndAutoUpdated:
                    return "AutoCorrected";
                case FileEntryVersion.CurrentVersionInDatabase:
                    return "Current";
                case FileEntryVersion.ExtractedNowUsingExiftool:
                    return "Exiftool extracted";
                case FileEntryVersion.ExtractedNowUsingReadMediaFile:
                    return "Extracted Poster";
                case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:
                    return "Extracted Windows Live Photo Gallery";
                case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:
                    return "Extracted Microsoft Photos";
                case FileEntryVersion.ExtractedNowUsingWebScraping:
                    return "Extracted WebScraping";
                case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:
                    return "Exiftool timeout";
                case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:
                    return "File not found";
                case FileEntryVersion.ExtractedNowUsingExiftoolWithError:
                    return "Exiftool with Error";
                case FileEntryVersion.Error:
                    return "Error";
                case FileEntryVersion.Historical:
                    return "Historical";
                case FileEntryVersion.NotAvailable:
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region IsReadOnlyType
        public static bool IsReadOnlyType(FileEntryVersion fileEntryVersion)
        {
            switch (fileEntryVersion)
            {
                case FileEntryVersion.MetadataToSave:
                case FileEntryVersion.CompatibilityFixedAndAutoUpdated:
                case FileEntryVersion.CurrentVersionInDatabase:
                case FileEntryVersion.ExtractedNowUsingExiftool:
                case FileEntryVersion.ExtractedNowUsingReadMediaFile:
                case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:
                case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:
                case FileEntryVersion.ExtractedNowUsingWebScraping:
                    return false;
                case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:
                case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:
                case FileEntryVersion.ExtractedNowUsingExiftoolWithError:
                case FileEntryVersion.Error:
                case FileEntryVersion.Historical:
                case FileEntryVersion.NotAvailable:
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region NeedUpdate
        public static bool NeedUpdate(FileEntryVersionCompare fileEntryVersionCompare)
        {
            switch (fileEntryVersionCompare)
            {
                case FileEntryVersionCompare.WonFoundNewer:
                case FileEntryVersionCompare.WonFoundEqual:
                case FileEntryVersionCompare.WonColumnCreatedHistoricalOrError:
                case FileEntryVersionCompare.WonColumnCredtedCurrent:
                    return true;
                case FileEntryVersionCompare.LostOverUserInput:
                case FileEntryVersionCompare.LostFoundOlder:
                case FileEntryVersionCompare.LostNoneEqualFound:
                    return false;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region CompareFileEntryAttribute
        public static FileEntryVersionCompare CompareFileEntryAttribute(FileEntryAttribute fileEntryAttributeDataGridViewColumn, FileEntryAttribute fileEntryAttributeFromQueue)
        {
            if (String.Compare(fileEntryAttributeFromQueue.FileFullPath, fileEntryAttributeDataGridViewColumn.FileFullPath, 
                comparisonType: StringComparison.OrdinalIgnoreCase) != 0) return FileEntryVersionCompare.LostNoneEqualFound;

            switch (fileEntryAttributeFromQueue.FileEntryVersion)
            {
                case FileEntryVersion.ExtractedNowUsingExiftool:                        //From queue
                case FileEntryVersion.ExtractedNowUsingReadMediaFile:                   //From queue
                case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:         //From queue
                case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:                 //From queue
                case FileEntryVersion.ExtractedNowUsingWebScraping:                     //From queue
                    #region
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            //Both Extracted from source, newst version win
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundNewer;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostFoundOlder; //DEBUG, ThreadSaveToDatabaseRegionAndThumbnail when updated old regions

                            break;
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            return FileEntryVersionCompare.WonFoundNewer; //Extracted from source always win over AutoCorrect (No need to check dates, It's only exist one column, regardless of date)
                            
                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            return FileEntryVersionCompare.WonFoundNewer; //Extracted from source always win over Read from database (No need to check dates, It's only exist one column, regardless of date)

                        case FileEntryVersion.Historical:                               //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                        case FileEntryVersion.Error:                                    //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound;          
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                    #endregion
                    break;

                case FileEntryVersion.MetadataToSave:                                   //From queue
                case FileEntryVersion.CompatibilityFixedAndAutoUpdated:                 //From queue
                    #region
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            //AutoCorrect, wins if newer
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundNewer;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostFoundOlder; //DEBUG, ThreadSaveToDatabaseRegionAndThumbnail when updated old regions

                            break;
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            //AutoCorrect, wins if newer
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundNewer;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostFoundOlder; //DEBUG, ThreadSaveToDatabaseRegionAndThumbnail when updated old regions
                            break;

                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            //AutoCorrect, always win over Read from database
                            return FileEntryVersionCompare.WonFoundNewer;

                        case FileEntryVersion.Historical:                               //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column
                                                     //Need continue the search
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                    #endregion
                    break;
                case FileEntryVersion.CurrentVersionInDatabase:                         //From queue
                    #region 
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            return FileEntryVersionCompare.LostFoundOlder;              //DataGridView Column

                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            return FileEntryVersionCompare.WonFoundNewer;               //DataGridView Column

                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundNewer;           

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostFoundOlder; //DEBUG, in case of queue get not in sequence
                            break;

                        case FileEntryVersion.Historical:                               //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                        case FileEntryVersion.Error:                                    //DataGridView Column
                            //Need continue the search
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                    #endregion
                    break;
                case FileEntryVersion.Error:                                            //From queue
                case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:                 //From queue
                case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:            //From queue
                case FileEntryVersion.ExtractedNowUsingExiftoolWithError:               //From queue
                case FileEntryVersion.Historical:                                       //From queue
                    #region 
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound;

                        case FileEntryVersion.Historical:                               //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                        case FileEntryVersion.Error:                                    //DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                    #endregion
                case FileEntryVersion.NotAvailable:                                     //From queue
                    return FileEntryVersionCompare.LostNoneEqualFound;
                default:
                    throw new NotImplementedException();
            }

            return FileEntryVersionCompare.LostNoneEqualFound; //DEBUG - If arrived here, means not all cases handled with care
        }
        #endregion
    }
}