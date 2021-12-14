using System;
using System.Collections.Generic;
using System.IO;


namespace MetadataLibrary
{

    [Serializable]
    public class FileEntry : IComparable<FileEntry>, IEquatable<FileEntry>, IFileEntry
    {
        protected string fullFilePath;
        protected DateTime lastWriteDateTime;

        public string FileFullPath { get => this.fullFilePath; set => this.fullFilePath = value; }
        public DateTime LastWriteDateTime { get => this.lastWriteDateTime; set => this.lastWriteDateTime = value; }

        public FileEntry(FileEntry fileEntry)
        {
            fullFilePath = fileEntry.fullFilePath;
            lastWriteDateTime = fileEntry.lastWriteDateTime;
        }

        public FileEntry(string fullFilePath, DateTime lastWriteDateTime)
        {
            this.fullFilePath = fullFilePath;
            this.lastWriteDateTime = lastWriteDateTime;
        }

        public FileEntry(string directory, string filename, DateTime lastAccesDateTime)
        {
            this.fullFilePath = Path.Combine(directory, filename);
            this.lastWriteDateTime = lastAccesDateTime;
        }

        public string Directory { get { return Path.GetDirectoryName(fullFilePath); } }

        public string FileName { get { return Path.GetFileName(fullFilePath); } }

        public override int GetHashCode()
        {
            var hashCode = -464453;
            hashCode = hashCode * -1521 + EqualityComparer<string>.Default.GetHashCode(fullFilePath.ToLower());
            hashCode = hashCode * -1521 + lastWriteDateTime.GetHashCode();
            return hashCode;
        }

        public bool Equals(FileEntry other)
        {
            if (other is null) return false; // If parameter is null, return false.
            if (Object.ReferenceEquals(this, other)) return true; // Optimization for a common success case.
            //if (this.GetType() != other.GetType()) return false; // If run-time types are not exactly the same, return false. Due to compare of FileEntryImage, FileEntryBroker

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return
                String.Compare(this.fullFilePath, other.fullFilePath, comparisonType: StringComparison.OrdinalIgnoreCase) == 0 &&
                this.lastWriteDateTime == other.lastWriteDateTime;
        }

        public override bool Equals(object other)
        {
            return this.Equals(other as FileEntry);
        }

        public int CompareTo(FileEntry other)
        {
            int compare = String.Compare(FileFullPath, other.FileFullPath, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (compare == 0)
            {
                if (this.LastWriteDateTime > other.LastWriteDateTime) compare = -1;
                if (this.LastWriteDateTime < other.LastWriteDateTime) compare = 1;
            }

            return compare;
        }

        public static bool operator ==(FileEntry left, FileEntry right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(FileEntry left, FileEntry right)
        {
            return !(left == right);
        }

        public static int FindIndex(List<FileEntry> fileEntries, FileEntry fileEntryToFind)
        {
            if (fileEntries == null) return -1;
            for (int index = 0; index < fileEntries.Count; index++)
            {
                if (fileEntries[index] == fileEntryToFind) return index;
            }
            return -1;
        }

        public static bool Contains(List<FileEntry> fileEntries, FileEntry fileEntryToFind)
        {
            return FindIndex(fileEntries, fileEntryToFind) > -1;
        }
    }
}
