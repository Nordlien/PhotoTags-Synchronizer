using System;
using System.IO;

namespace MetadataLibrary
{
    [Serializable]
    public class FileEntryBroker : FileEntry, IEquatable<FileEntryBroker>
    {
        private MetadataBrokerTypes broker;
       
        public MetadataBrokerTypes Broker { get => broker; set => broker = value; }

        public FileEntryBroker(FileEntry fileEntry, MetadataBrokerTypes broker) : base(fileEntry)
        {
            this.broker = broker;
            this.fullFilePath = fileEntry.FullFilePath;
            this.lastWriteDateTime = fileEntry.LastWriteDateTime;
        }

        public FileEntryBroker(string fileDirectory, string fileName, DateTime lastAccessDateTime, MetadataBrokerTypes broker) 
            : base(Path.Combine (fileDirectory, fileName), lastAccessDateTime)
        {
            this.broker = broker;

        }
        public FileEntryBroker(string fullFilePath, DateTime lastAccessDateTime, MetadataBrokerTypes broker) : base(fullFilePath, lastAccessDateTime)
        {
            this.broker = broker;
        }

        public override int GetHashCode()
        {
            var hashCode = -1180706998;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + broker.GetHashCode();
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

        

    }


}
