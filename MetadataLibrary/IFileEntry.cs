using System;

namespace MetadataLibrary
{
    public interface IFileEntry
    {
        string FullFilePath { get; set; }
        string Directory { get; }
        string FileName { get; }
        DateTime LastWriteDateTime { get; set; }
    }
}