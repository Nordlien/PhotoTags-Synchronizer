using System;

namespace MetadataLibrary
{
    public interface IFileEntry
    {
        string FileFullPath { get; set; }
        string Directory { get; }
        string FileName { get; }
        DateTime LastWriteDateTime { get; set; }
    }
}