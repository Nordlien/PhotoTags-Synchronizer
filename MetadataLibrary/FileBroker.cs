using System;
using System.Collections.Generic;
using System.IO;


namespace MetadataLibrary
{
    public class FileBroker : IComparable<FileBroker>, IEquatable<FileBroker>
    {
        public string FullFilePath { get; set; }
        public MetadataBrokerType Broker { get; set; }

        public FileBroker(FileBroker fileEntry) : this (fileEntry.Broker, fileEntry.FullFilePath)
        {
            
        }

        public FileBroker(MetadataBrokerType broker, string fullFilePath) 
        {
            FullFilePath = fullFilePath;
            Broker = broker;
        }

        public string Directory { get { return Path.GetDirectoryName(FullFilePath); } }

        public string FileName { get { return Path.GetFileName(FullFilePath); } }

        public override int GetHashCode()
        {
            int hashCode = 836731445;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FullFilePath);
            hashCode = hashCode * -1521134295 + Broker.GetHashCode();
            return hashCode;
        }

        public bool Equals(FileBroker other)
        {
            if (other is null) return false; // If parameter is null, return false.
            if (Object.ReferenceEquals(this, other)) return true; // Optimization for a common success case.
            if (this.GetType() != other.GetType()) return false; // If run-time types are not exactly the same, return false. 

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return
                this.FullFilePath == other.FullFilePath &&
                this.Broker == other.Broker;
        }

        public override bool Equals(object other)
        {
            return this.Equals(other as FileEntry);
        }

        public int CompareTo(FileBroker other)
        {
            int compare = 0; FullFilePath.CompareTo(other.FullFilePath);
            if (this.Broker > other.Broker) compare = -1;
            if (this.Broker < other.Broker) compare = 1;
            if (compare == 0) compare = FullFilePath.CompareTo(other.FullFilePath);

            return compare;
        }

        public static bool operator ==(FileBroker left, FileBroker right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(FileBroker left, FileBroker right)
        {
            return !(left == right);
        }

        
        public static int FindIndex(List<FileBroker> fileEntries, FileBroker fileEntryToFind)
        {
            if (fileEntries == null) return -1;
            for (int index = 0; index < fileEntries.Count; index++)
            {
                if (fileEntries[index] == fileEntryToFind) return index;
            }
            return - 1;
        }

        public static bool Contains(List<FileBroker> fileEntries, FileBroker fileEntryToFind)
        {                        
            return FindIndex(fileEntries, fileEntryToFind) > -1;
        }

        
    }
}
