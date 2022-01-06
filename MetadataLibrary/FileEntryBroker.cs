using System;
using System.Collections.Generic;
using System.IO;

namespace MetadataLibrary
{
    

    [Serializable]
    public class FileEntryBroker : FileEntry, IEquatable<FileEntryBroker>
    {
        private MetadataBrokerType broker;
       
        public MetadataBrokerType Broker { get => broker; set => broker = value; }

        public FileEntryBroker(FileEntryBroker fileEntryBroker) : this(fileEntryBroker.FileFullPath, fileEntryBroker.LastWriteDateTime, fileEntryBroker.Broker)
        {
        }

        public FileEntryBroker(FileEntry fileEntry, MetadataBrokerType broker) : base(fileEntry)
        {
            this.broker = broker;
        }

        public FileEntryBroker(string fileDirectory, string fileName, DateTime lastAccessDateTime, MetadataBrokerType broker) 
            : base(Path.Combine (fileDirectory, fileName), lastAccessDateTime)
        {
            if (broker == MetadataBrokerType.WebScraping) fileDirectory = MetadataDatabaseCache.WebScapingFolderName;
            this.broker = broker;
        }

        public FileEntryBroker(string fullFilePath, DateTime lastWriteDateTime, MetadataBrokerType broker) : base(fullFilePath, lastWriteDateTime)
        {
            this.broker = broker;
        }

        public override int GetHashCode()
        {
            var hashCode = 65765335;
            hashCode = hashCode * -424254 + base.GetHashCode();
            hashCode = hashCode * -424254 + broker.GetHashCode();
            return hashCode;
        }


        public override bool Equals(object other)
        {
            return this.Equals(other as FileEntryBroker);
        }

        public bool Equals(FileEntryBroker other)
        {
            if (other is null) return false; // If parameter is null, return false.
            if (Object.ReferenceEquals(this, other)) return true; // Optimization for a common success case.
            //if (this.GetType() != other.GetType()) return false; // If run-time types are not exactly the same, return false.

            // Check properties that this class declares.
            if (broker == other.broker) 
            {
                // Let base class check its own fields and do the run-time type comparison.
                return base.Equals((FileEntry)other);
            }
            else return false;
        }

        public static bool operator ==(FileEntryBroker left, FileEntryBroker right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(FileEntryBroker left, FileEntryBroker right)
        {
            return !(left == right);
        }

        public static DateTime? FindNewestDate(List<FileEntryBroker> fileVersionDates)
        {
            if (fileVersionDates.Count == 0) return null;

            DateTime newestDate = fileVersionDates[0].LastWriteDateTime;
            foreach (FileEntryBroker fileEntryFindNewest in fileVersionDates)
            {
                if (//fileEntryFindNewest.Broker == MetadataBrokerTypes.ExifTool && 
                    fileEntryFindNewest.LastWriteDateTime > newestDate )
                    newestDate = fileEntryFindNewest.LastWriteDateTime;
            }
            return newestDate;
        }

        public static DateTime? FindNewestDate(List<FileEntryAttribute> fileVersionDates)
        {
            if (fileVersionDates.Count == 0) return null;

            DateTime newestDate = fileVersionDates[0].LastWriteDateTime;
            foreach (FileEntryAttribute fileEntryFindNewest in fileVersionDates)
            {
                if (//fileEntryFindNewest.Broker == MetadataBrokerTypes.ExifTool &&
                    fileEntryFindNewest.LastWriteDateTime > newestDate)
                    newestDate = fileEntryFindNewest.LastWriteDateTime;
            }
            return newestDate;
        }

        public static DateTime? FindOldestDate(List<FileEntryBroker> fileVersionDates)
        {
            if (fileVersionDates.Count == 0) return null;

            DateTime oldestDate = fileVersionDates[0].LastWriteDateTime;
            foreach (FileEntryBroker fileEntryBrokerFindNewest in fileVersionDates)
            {
                if (fileEntryBrokerFindNewest.Broker == MetadataBrokerType.ExifTool &&
                    fileEntryBrokerFindNewest.LastWriteDateTime < oldestDate)
                    oldestDate = fileEntryBrokerFindNewest.LastWriteDateTime;
            }
            return oldestDate;
        }

    }


}
