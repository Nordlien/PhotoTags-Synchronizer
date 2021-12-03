using System;

namespace MetadataLibrary
{
    public class FileEntryAttribute : FileEntry, IEquatable<FileEntryAttribute>
    {
        public FileEntryVersion FileEntryVersion { get; set; } = FileEntryVersion.NotAvailable;

        public FileEntryBroker GetFileEntryBroker(MetadataBrokerType metadataBrokerType)
        {
            if (FileEntryVersion == FileEntryVersion.Error) return new FileEntryBroker((FileEntry)this, metadataBrokerType | MetadataBrokerType.ExifToolWriteError);
            return new FileEntryBroker((FileEntry)this, metadataBrokerType);
        }

        public FileEntry FileEntry { get => new FileEntry(fullFilePath, lastWriteDateTime); }

        public FileEntryAttribute(FileEntryAttribute fileEntryAttribute) : base(fileEntryAttribute.FileEntry)
        {
            FileEntryVersion = fileEntryAttribute.FileEntryVersion;            
        }

        public FileEntryAttribute(FileEntry fileEntry) : base(fileEntry)
        {
            FileEntryVersion = FileEntryVersion.NotAvailable;
        }

        public FileEntryAttribute(string fullFilePath, DateTime lastWriteDateTime, FileEntryVersion fileEntryVersion) : base(fullFilePath, lastWriteDateTime)
        {
            FileEntryVersion = fileEntryVersion;
        }

        public FileEntryAttribute(string directory, string filename, DateTime lastWriteDateTime, FileEntryVersion fileEntryVersion) : base(directory, filename, lastWriteDateTime)
        {
            FileEntryVersion = fileEntryVersion;
        }

        public FileEntryAttribute(FileEntry fileEntry, FileEntryVersion fileEntryVersion) : base(fileEntry)
        {
            FileEntryVersion = fileEntryVersion;
        }

        public override bool Equals(object other)
        {
            return this.Equals(other as FileEntryAttribute);
        }

        public bool Equals(FileEntryAttribute other)
        {
            if (other is null) return false; // If parameter is null, return false.
            if (Object.ReferenceEquals(this, other)) return true; // Optimization for a common success case.

            // Let base class check its own fields and do the run-time type comparison.
            return
                this.FileFullPath == other.FileFullPath &&
                this.FileEntryVersion == other.FileEntryVersion &&
                this.LastWriteDateTime == other.LastWriteDateTime;
        }

        public static bool operator ==(FileEntryAttribute left, FileEntryAttribute right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(FileEntryAttribute left, FileEntryAttribute right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            int hashCode = 881786474;
            hashCode = hashCode * 42156 + base.GetHashCode();
            hashCode = hashCode * 42156 + FileEntryVersion.GetHashCode();
            return hashCode;
        }
    }
}
