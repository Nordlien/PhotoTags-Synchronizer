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
        ExtractedNowFromMediaFile,
        ExtractedNowFromExternalSource,
    }

    public enum FileEntryVersionCompare
    {
        FoundEqual,
        FoundAndWon,
        FoundButLost,
        NotEqualFound,
        LostOverUserInput
    }

    public class FileEntryVersionHandler
    {
        public static bool IsCurrenOrUpdatedVersion(FileEntryVersion fileEntryVersion)
        {
            return 
                fileEntryVersion == FileEntryVersion.CurrentVersionInDatabase || fileEntryVersion == FileEntryVersion.AutoCorrect || 
                fileEntryVersion == FileEntryVersion.ExtractedNowFromMediaFile || fileEntryVersion == FileEntryVersion.ExtractedNowFromExternalSource;
        }

        public static bool IsErrorOrHistoricalVersion(FileEntryVersion fileEntryVersion)
        {
            //fileEntryAttribute.FileEntryVersion != FileEntryVersion.AutoCorrect && fileEntryAttribute.FileEntryVersion != FileEntryVersion.Curren
            return fileEntryVersion == FileEntryVersion.Historical || fileEntryVersion == FileEntryVersion.Error;
        }

        public static bool NeedUpdate(FileEntryVersionCompare fileEntryVersionCompare)
        {
            switch (fileEntryVersionCompare)
            {
                case FileEntryVersionCompare.FoundAndWon:
                    return true;
                case FileEntryVersionCompare.FoundEqual:
                    return true;
                case FileEntryVersionCompare.LostOverUserInput:
                    return false;
                case FileEntryVersionCompare.FoundButLost:
                    return false;
                case FileEntryVersionCompare.NotEqualFound:
                    return false;
                default:
                    throw new NotImplementedException();
            }
        }

        public static FileEntryVersionCompare CompareFileEntryAttribute(FileEntryAttribute fileEntryAttributeDataGridViewColumn, FileEntryAttribute fileEntryAttributeFromQueue)
        {
            if (fileEntryAttributeFromQueue.FileName != fileEntryAttributeDataGridViewColumn.FileName) 
                return FileEntryVersionCompare.NotEqualFound;

            switch (fileEntryAttributeFromQueue.FileEntryVersion)
            {
                case FileEntryVersion.ExtractedNowFromExternalSource:
                case FileEntryVersion.ExtractedNowFromMediaFile:
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        case FileEntryVersion.ExtractedNowFromExternalSource: //is store in DataGridView Column
                        case FileEntryVersion.ExtractedNowFromMediaFile: //is store in DataGridView Column
                            //Both Extracted from source, newst version win
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundAndWon;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundButLost; //DEBUG, in case of queue get not in sequence

                            break;

                        case FileEntryVersion.AutoCorrect: //is store in DataGridView Column
                            return FileEntryVersionCompare.FoundAndWon; //Extracted from source always win over AutoCorrect (No need to check dates, It's only exist one column, regardless of date)
                            
                        case FileEntryVersion.CurrentVersionInDatabase: //is store in DataGridView Column
                            return FileEntryVersionCompare.FoundButLost; //Extracted from source always win over Read from database (No need to check dates, It's only exist one column, regardless of date)

                        case FileEntryVersion.Historical: //is store in DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column
                            return FileEntryVersionCompare.NotEqualFound;
                        default:
                            throw new NotImplementedException();
                    }

                    break;

                case FileEntryVersion.AutoCorrect:

                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {

                        case FileEntryVersion.ExtractedNowFromExternalSource: //is store in DataGridView Column
                        case FileEntryVersion.ExtractedNowFromMediaFile: //is store in DataGridView Column
                            //AutoCorrect, wins if newer
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundAndWon;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundButLost; //DEBUG, in case of queue get not in sequence
                            break;

                        case FileEntryVersion.AutoCorrect: //is store in DataGridView Column
                            //AutoCorrect, wins if newer
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundAndWon;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundButLost; //DEBUG, in case of queue get not in sequence
                            break;

                        case FileEntryVersion.CurrentVersionInDatabase: //is store in DataGridView Column
                            //AutoCorrect, always win over Read from database
                            return FileEntryVersionCompare.FoundAndWon;

                        case FileEntryVersion.Historical: //is store in DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column
                                                     //Need continue the search
                            return FileEntryVersionCompare.NotEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case FileEntryVersion.CurrentVersionInDatabase:

                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {

                        case FileEntryVersion.ExtractedNowFromExternalSource: //is store in DataGridView Column
                        case FileEntryVersion.ExtractedNowFromMediaFile: //is store in DataGridView Column
                            return FileEntryVersionCompare.FoundButLost; 

                        case FileEntryVersion.AutoCorrect: //is store in DataGridView Column
                            return FileEntryVersionCompare.FoundAndWon;

                        case FileEntryVersion.CurrentVersionInDatabase: //is store in DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime > fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundAndWon;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundEqual;

                            if (fileEntryAttributeFromQueue.LastWriteDateTime < fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundButLost; //DEBUG, in case of queue get not in sequence
                            break;

                        case FileEntryVersion.Historical: //is store in DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column
                            //Need continue the search
                            return FileEntryVersionCompare.NotEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case FileEntryVersion.Error:
                case FileEntryVersion.Historical:
                    switch (fileEntryAttributeDataGridViewColumn.FileEntryVersion)
                    {
                        case FileEntryVersion.ExtractedNowFromExternalSource: //is store in DataGridView Column
                        case FileEntryVersion.ExtractedNowFromMediaFile: //is store in DataGridView Column
                        case FileEntryVersion.AutoCorrect: //is store in DataGridView Column
                        case FileEntryVersion.CurrentVersionInDatabase: //is store in DataGridView Column
                            return FileEntryVersionCompare.NotEqualFound;

                        case FileEntryVersion.Historical: //is store in DataGridView Column
                        case FileEntryVersion.Error: //is store in DataGridView Column
                            if (fileEntryAttributeFromQueue.LastWriteDateTime == fileEntryAttributeDataGridViewColumn.LastWriteDateTime)
                                return FileEntryVersionCompare.FoundEqual;
                            return FileEntryVersionCompare.NotEqualFound;
                        default:
                            throw new NotImplementedException();
                    }
                default:
                    throw new NotImplementedException();
            }

            return FileEntryVersionCompare.NotEqualFound; //DEBUG - If arrived here, means not all cases handled with care
        }
    }
}