using System;

namespace MetadataLibrary
{
    public enum FileEntryVersion
    {
        NotAvailable,
        CurrentVersionInDatabase,
        Historical,
        Error,
        AutoCorrect,
        ExtractedNowFromExternalSource,
        ExtractedNowUsingExiftool,
        ExtractedNowUsingExiftoolWithError,
        ExtractedNowUsingExiftoolTimeout,
        ExtractedNowUsingExiftoolFileNotExist
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
                fileEntryVersion == FileEntryVersion.CurrentVersionInDatabase || fileEntryVersion == FileEntryVersion.AutoCorrect || 
                fileEntryVersion == FileEntryVersion.ExtractedNowUsingExiftool || fileEntryVersion == FileEntryVersion.ExtractedNowFromExternalSource;
        }
        #endregion

        #region IsErrorOrHistoricalVersion
        public static bool IsErrorOrHistoricalVersion(FileEntryVersion fileEntryVersion)
        {
            //fileEntryAttribute.FileEntryVersion != FileEntryVersion.AutoCorrect && fileEntryAttribute.FileEntryVersion != FileEntryVersion.Curren
            return fileEntryVersion == FileEntryVersion.Historical || fileEntryVersion == FileEntryVersion.Error;
        }
        #endregion

        #region IsReadOnlyType
        public static bool IsReadOnlyType(FileEntryVersion fileEntryVersion)
        {
            switch (fileEntryVersion)
            {
                case FileEntryVersion.AutoCorrect:
                case FileEntryVersion.CurrentVersionInDatabase:
                case FileEntryVersion.ExtractedNowFromExternalSource:
                case FileEntryVersion.ExtractedNowUsingExiftool:
                    return false;
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
                case FileEntryVersion.ExtractedNowFromExternalSource:
                case FileEntryVersion.ExtractedNowUsingExiftool:
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        case FileEntryVersion.ExtractedNowFromExternalSource: //is store in DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftool: //is store in DataGridView Column
                            //Both Extracted from source, newst version win
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundNewer;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostFoundOlder; //DEBUG, in case of queue get not in sequence

                            break;

                        case FileEntryVersion.AutoCorrect: //is store in DataGridView Column
                            return FileEntryVersionCompare.WonFoundNewer; //Extracted from source always win over AutoCorrect (No need to check dates, It's only exist one column, regardless of date)
                            
                        case FileEntryVersion.CurrentVersionInDatabase: //is store in DataGridView Column
                            return FileEntryVersionCompare.WonFoundNewer; //Extracted from source always win over Read from database (No need to check dates, It's only exist one column, regardless of date)

                        case FileEntryVersion.Historical: //is store in DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        case FileEntryVersion.NotAvailable:
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        default:
                            throw new NotImplementedException();
                    }

                    break;

                case FileEntryVersion.AutoCorrect:

                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {

                        case FileEntryVersion.ExtractedNowFromExternalSource: //is store in DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftool: //is store in DataGridView Column
                            //AutoCorrect, wins if newer
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundNewer;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostFoundOlder; //DEBUG, in case of queue get not in sequence

                            break;

                        case FileEntryVersion.AutoCorrect: //is store in DataGridView Column
                            //AutoCorrect, wins if newer
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundNewer;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostFoundOlder; //DEBUG, in case of queue get not in sequence
                            break;

                        case FileEntryVersion.CurrentVersionInDatabase: //is store in DataGridView Column
                            //AutoCorrect, always win over Read from database
                            return FileEntryVersionCompare.WonFoundNewer;

                        case FileEntryVersion.Historical: //is store in DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column
                                                     //Need continue the search
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        case FileEntryVersion.NotAvailable:
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case FileEntryVersion.CurrentVersionInDatabase:

                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {

                        case FileEntryVersion.ExtractedNowFromExternalSource: //is store in DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftool: //is store in DataGridView Column
                            return FileEntryVersionCompare.LostFoundOlder; 

                        case FileEntryVersion.AutoCorrect: //is store in DataGridView Column
                            return FileEntryVersionCompare.WonFoundNewer;

                        case FileEntryVersion.CurrentVersionInDatabase: //is store in DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundNewer;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.LostFoundOlder; //DEBUG, in case of queue get not in sequence
                            break;

                        case FileEntryVersion.Historical: //is store in DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column
                            //Need continue the search
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        case FileEntryVersion.NotAvailable:
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case FileEntryVersion.Error:
                case FileEntryVersion.Historical:
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        case FileEntryVersion.ExtractedNowFromExternalSource: //is store in DataGridView Column
                        case FileEntryVersion.ExtractedNowUsingExiftool: //is store in DataGridView Column
                        case FileEntryVersion.AutoCorrect: //is store in DataGridView Column
                        case FileEntryVersion.CurrentVersionInDatabase: //is store in DataGridView Column
                            return FileEntryVersionCompare.LostNoneEqualFound;

                        case FileEntryVersion.Historical: //is store in DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.WonFoundEqual;
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        case FileEntryVersion.NotAvailable:
                            return FileEntryVersionCompare.LostNoneEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                case FileEntryVersion.NotAvailable:
                    return FileEntryVersionCompare.LostNoneEqualFound;
                default:
                    throw new NotImplementedException();
            }

            return FileEntryVersionCompare.LostNoneEqualFound; //DEBUG - If arrived here, means not all cases handled with care
        }
        #endregion
    }
}