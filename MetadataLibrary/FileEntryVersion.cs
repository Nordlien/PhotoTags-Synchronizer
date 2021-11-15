namespace MetadataLibrary
{
    public enum FileEntryVersion
    {
        NotAvailable,
        CurrentVersionInDatabase,
        Historical,
        Error,
        AutoCorrect,
        ExtractedNowFromMediaFile
    }

    public class FileEntryVersionHandler
    {
        public static bool IsCurrenOrUpdatedVersion(FileEntryVersion fileEntryVersion)
        {
            return fileEntryVersion == FileEntryVersion.CurrentVersionInDatabase || fileEntryVersion == FileEntryVersion.AutoCorrect || fileEntryVersion == FileEntryVersion.ExtractedNowFromMediaFile;
        }

        public static bool IsErrorOrHistoricalVersion(FileEntryVersion fileEntryVersion)
        {
            //fileEntryAttribute.FileEntryVersion != FileEntryVersion.AutoCorrect && fileEntryAttribute.FileEntryVersion != FileEntryVersion.Curren
            return fileEntryVersion == FileEntryVersion.Historical || fileEntryVersion == FileEntryVersion.Error;
        }
    }
}