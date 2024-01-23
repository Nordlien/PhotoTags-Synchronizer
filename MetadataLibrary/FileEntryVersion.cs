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
        Won_Update_Satuts_Metdata_DataGridView,
        Update_Status_FileNotFound,
        Update_Status_DataGridView_LostOverUserData,

        CreateColumnHistoricalOrError_CreateColumn,
        CreateColumnCurrent_CreateColumn,

        Update_Status_Metadata_WriteFailed,
        LostOverUserInput_Update_Status,
        LostWasOlder_Updated_Nothing,
        LostNoneEqualFound_ContinueSearch_Update_Nothing
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
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingExiftoolWithError ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingExiftoolTimeout ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist ||
                
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingReadMediaFile ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingMicrosoftPhotos ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingWebScraping;
        }
        #endregion

        #region IsCurrenFileVersion
        public static bool IsCurrenFileVersion(FileEntryVersion fileEntryVersion)
        {
            return
                fileEntryVersion == FileEntryVersion.CurrentVersionInDatabase ||
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingExiftool;
        }
        #endregion

        #region IsErrorOrHistoricalVersion
        public static bool IsErrorOrHistoricalVersion(FileEntryVersion fileEntryVersion)
        {
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
                case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:
                case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:
                case FileEntryVersion.ExtractedNowUsingExiftoolWithError:

                case FileEntryVersion.ExtractedNowUsingReadMediaFile:
                case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:
                case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:
                case FileEntryVersion.ExtractedNowUsingWebScraping:                
                    return FileEntryVersion.CurrentVersionInDatabase;

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
                    return "Saving and Compatibility checked";
                case FileEntryVersion.CompatibilityFixedAndAutoUpdated:
                    return "AutoCorrected";
                case FileEntryVersion.CurrentVersionInDatabase:
                    return "Current";
                case FileEntryVersion.ExtractedNowUsingExiftool:
                    return "Exiftool extracted";
                case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:
                    return "Exiftool timeout";
                case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:
                    return "File not found";
                case FileEntryVersion.ExtractedNowUsingExiftoolWithError:
                    return "Exiftool with Error";
                case FileEntryVersion.ExtractedNowUsingReadMediaFile:
                    return "Extracted Poster";
                case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:
                    return "Extracted Windows Live Photo Gallery";
                case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:
                    return "Extracted Microsoft Photos";
                case FileEntryVersion.ExtractedNowUsingWebScraping:
                    return "Extracted WebScraping";                
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

        #region IsReadOnlyColumnType
        public static bool IsReadOnlyColumnType(FileEntryVersion fileEntryVersion)
        {
            switch (fileEntryVersion)
            {
                case FileEntryVersion.MetadataToSave:
                case FileEntryVersion.CompatibilityFixedAndAutoUpdated:
                case FileEntryVersion.CurrentVersionInDatabase:
                
                case FileEntryVersion.ExtractedNowUsingExiftool:
                case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:
                case FileEntryVersion.ExtractedNowUsingExiftoolWithError:

                case FileEntryVersion.ExtractedNowUsingReadMediaFile:
                case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:
                case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:
                case FileEntryVersion.ExtractedNowUsingWebScraping:
                    return false;

                case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:
                case FileEntryVersion.Error:
                case FileEntryVersion.Historical:
                case FileEntryVersion.NotAvailable:
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DoesCellsNeedUpdate
        public static bool DoesCellsNeedUpdate(FileEntryVersionCompare fileEntryVersionCompare)
        {
            switch (fileEntryVersionCompare)
            {
                case FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView:
                case FileEntryVersionCompare.Update_Status_DataGridView_LostOverUserData:                
                    return true;

                case FileEntryVersionCompare.CreateColumnHistoricalOrError_CreateColumn:
                case FileEntryVersionCompare.CreateColumnCurrent_CreateColumn:
                
                    return true;

                case FileEntryVersionCompare.Update_Status_FileNotFound:
                case FileEntryVersionCompare.Update_Status_Metadata_WriteFailed:
                case FileEntryVersionCompare.LostOverUserInput_Update_Status:
                case FileEntryVersionCompare.LostWasOlder_Updated_Nothing:
                case FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing:
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
                comparisonType: StringComparison.OrdinalIgnoreCase) != 0) return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;

            switch (fileEntryAttributeFromQueue.FileEntryVersion)
            {
                case FileEntryVersion.CurrentVersionInDatabase:                         //From queue
                    #region Queue from From CurrentVersionInDatabase
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        #region Queue: CurrentVersionInDatabase - DataGridView: CurrentVersionInDatabase - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing; //Continue search, can be missing data from past, eg. ThreadSaveToDatabaseRegionAndThumbnail when updated old regions
                            break;
                        #endregion

                        #region Queue: CurrentVersionInDatabase - DataGridView: Failed - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_FileNotFound;
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                            return FileEntryVersionCompare.LostWasOlder_Updated_Nothing;
                        #endregion

                        #region Queue: CurrentVersionInDatabase - DataGridView: Extracted - Result: LostWasOlder_Updated_Nothing???
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            return FileEntryVersionCompare.LostWasOlder_Updated_Nothing;
                        #endregion

                        #region Queue: CurrentVersionInDatabase - DataGridView: ToSave - Result: Won was newer
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;
                        #endregion

                        #region Queue: CurrentVersionInDatabase - DataGridView: Historical/Error - Result: Continue Search
                        case FileEntryVersion.Historical:                               //DataGridView Column
                        case FileEntryVersion.Error:                                    //DataGridView Column
                            //Need continue the search
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region NotAvailable
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        default:
                            throw new NotImplementedException();
                            #endregion
                    }
                    #endregion
                    break;

                case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:            //From queue
                    #region Queue From File not Exist
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        #region Queue: File not Exist - DataGridView: CurrentVersion - Result: WonWasEqual_Update_Status_Metadata_WriteFailed
                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_FileNotFound;
                        #endregion

                        #region Queue: File not Exist - DataGridView: Failed - Result: Update_Status_Metadata_WriteFailed
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_FileNotFound;
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column                        
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_FileNotFound;
                        #endregion

                        #region Queue: File not Exist - DataGridView: Extracted - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_FileNotFound;
                        #endregion

                        #region Queue: File not Exist - DataGridView: ToSave - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_FileNotFound;
                        #endregion

                        #region Queue: File not Exist - DataGridView: Historical/Error - Result: Continue Search
                        case FileEntryVersion.Historical:                               //DataGridView Column
                        case FileEntryVersion.Error:                                    //DataGridView Column
                            //Need continue the search
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region NotAvailable
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        default:
                            throw new NotImplementedException();
                            #endregion
                    }
                    #endregion

                case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:                 //From queue                
                case FileEntryVersion.ExtractedNowUsingExiftoolWithError:               //From queue
                    #region Queue From Failed
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        #region Queue: Failed - DataGridView: CurrentVersion - Result: WonWasEqual_Update_Status_Metadata_WriteFailed
                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_Metadata_WriteFailed;
                        #endregion

                        #region Queue: Failed - DataGridView: Failed - Result: Update_Status_Metadata_WriteFailed
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_FileNotFound;
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column                        
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_Metadata_WriteFailed;
                        #endregion

                        #region Queue: Failed - DataGridView: Extracted - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Update_Status_Metadata_WriteFailed;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Update_Status_Metadata_WriteFailed;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                            break;
                        #endregion

                        #region Queue: Failed - DataGridView: ToSave - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_Metadata_WriteFailed;
                        #endregion

                        #region Queue: Failed - DataGridView: Historical/Error - Result: Continue Search
                        case FileEntryVersion.Historical:                               //DataGridView Column
                        case FileEntryVersion.Error:                                    //DataGridView Column
                            //Need continue the search
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region NotAvailable
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        default:
                            throw new NotImplementedException();
                            #endregion
                    }
                    #endregion
                    break;

                case FileEntryVersion.ExtractedNowUsingExiftool:                        //From queue
                case FileEntryVersion.ExtractedNowUsingReadMediaFile:                   //From queue
                case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:         //From queue
                case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:                 //From queue
                case FileEntryVersion.ExtractedNowUsingWebScraping:                     //From queue
                    #region Queue from Extracted
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        #region Queue: Extracted - DataGridView: CurrentVersion - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing; //Continue search, can be missing data from past, eg. ThreadSaveToDatabaseRegionAndThumbnail when updated old regions
                            break;
                        #endregion

                        #region Queue: Extracted - DataGridView: Failed - Result: Update_Status_Metadata_WriteFailed
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_FileNotFound;
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_Metadata_WriteFailed;
                        #endregion

                        #region Queue: Extracted - DataGridView: Extracted - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            //Both Extracted from source, newest version win
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing; //Continue search, can be missing data from past, eg. ThreadSaveToDatabaseRegionAndThumbnail when updated old regions
                            break;
                        #endregion

                        #region Queue: Extracted - DataGridView: ToSave - Result: WonWasNewer_Update_Status_Metdata_DataGridView
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView; //Extracted from source always win over AutoCorrect (No need to check dates, It's only exist one column, regardless of date)
                        #endregion

                        #region Queue: Extracted - DataGridView: Historical/Error - Result: Continue search
                        case FileEntryVersion.Historical:                               //DataGridView Column                        
                        case FileEntryVersion.Error:                                    //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region NotAvailable
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        default:
                            throw new NotImplementedException();
                        #endregion
                    }
                    #endregion
                    break;

                case FileEntryVersion.MetadataToSave:                                   //From queue
                case FileEntryVersion.CompatibilityFixedAndAutoUpdated:                 //From queue
                    #region Queue from Saving
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        #region Queue: ToSave - DataGridView: CurrentVersion - Result: WonWasNewer_Update_Status_Metdata_DataGridView
                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            //AutoCorrect, always win over Read from database
                            return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;
                        #endregion

                        #region Queue: ToSave - DataGridView: Failed - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                            return FileEntryVersionCompare.Update_Status_FileNotFound;
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                            return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;
                        #endregion

                        #region Queue: ToSave - DataGridView: Extracted - Result: Depends on LastWriteDateTime 
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing; //Continue search, can be missing data from past, eg. ThreadSaveToDatabaseRegionAndThumbnail when updated old regions
                            break;
                        #endregion

                        #region Queue: ToSave - DataGridView: ToSave - Result: Depends on LastWriteDateTime (Last save wins)
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing; //Continue search, can be missing data from past, eg. ThreadSaveToDatabaseRegionAndThumbnail when updated old regions
                            break;
                        #endregion

                        #region Queue: ToSave - DataGridView: Historical/Error - Result: Continue Search
                        case FileEntryVersion.Historical:                               //DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column, Need continue the search
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region NotAvailable
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        default:
                            throw new NotImplementedException();
                        #endregion
                    }
                    #endregion
                    break;
                
                case FileEntryVersion.Error:                                            //From queue
                    #region Queue from Error
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        #region Queue: Error - DataGridView: CurrenyVersion - Result: Continue Search
                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region Queue: Error - DataGridView: Failed - Result: Continue Search
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region Queue: Error - DataGridView: Extracted - Result: Continue Search
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region Queue: Error - DataGridView: ToSave - Result: Continue Search
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region Queue: Error - DataGridView: Historical - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.Historical:                               //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        case FileEntryVersion.Error:                                    //DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region NotAvailable
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        default:
                            throw new NotImplementedException();
                        #endregion
                    }
                    #endregion

                case FileEntryVersion.Historical:                                       //From queue
                    #region Queue from Historical
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        #region Queue: Historical - DataGridView: CurrentVersion - Result: Continue Search
                        case FileEntryVersion.CurrentVersionInDatabase:                 //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region Queue: Historical - DataGridView: Failed - Result: Continue Search
                        case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region Queue: Historical - DataGridView: Extracted - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.ExtractedNowUsingExiftool:                //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingReadMediaFile:           //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery: //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:         //DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingWebScraping:             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region Queue: Historical - DataGridView: ToSave - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.MetadataToSave:                           //DataGridView Column
                        case FileEntryVersion.CompatibilityFixedAndAutoUpdated:         //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion 

                        #region Queue: Historical - DataGridView: Historical - Result: Depends on LastWriteDateTime
                        case FileEntryVersion.Historical:                               //DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime) 
                                return FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        case FileEntryVersion.Error:                                    //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        #endregion

                        #region NotAvailable
                        case FileEntryVersion.NotAvailable:                             //DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                        default:
                            throw new NotImplementedException();
                        #endregion
                    }
                    #endregion

                case FileEntryVersion.NotAvailable:                                     //From queue
                    return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
                default:
                    throw new NotImplementedException();
            }

            return FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing; //DEBUG - If arrived here, means not all cases handled with care
        }
        #endregion
    }
}