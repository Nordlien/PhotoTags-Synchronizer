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

        public string FullFilePath { get => this.fullFilePath; set => this.fullFilePath = value; }
        public DateTime LastWriteDateTime { get => this.lastWriteDateTime; set => this.lastWriteDateTime = value; }

        public FileEntry(FileEntry fileEntry)
        {
            fullFilePath = fileEntry.fullFilePath;
            lastWriteDateTime = fileEntry.lastWriteDateTime;
        }

        public FileEntry(string fullFilePath, DateTime lastAccesDateTime)
        {
            this.fullFilePath = fullFilePath;
            this.lastWriteDateTime = lastAccesDateTime;
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
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(fullFilePath);
            hashCode = hashCode * -1521134295 + lastWriteDateTime.GetHashCode();
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
                this.fullFilePath == other.fullFilePath &&
                this.lastWriteDateTime == other.lastWriteDateTime;
        }

        public override bool Equals(object other)
        {
            return this.Equals(other as FileEntry);
        }

        public int CompareTo(FileEntry other)
        {
            int compare = FullFilePath.CompareTo(other.FullFilePath);
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

    }


}
