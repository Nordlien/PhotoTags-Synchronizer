using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MetadataLibrary
{
    public enum FileEntryVersion
    {
        NotAvailable,
        Current,
        Historical,
        Error
    }

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

        public FileEntryAttribute(string fullFilePath, DateTime lastAccesDateTime, FileEntryVersion fileEntryVersion) : base(fullFilePath, lastAccesDateTime)
        {
            FileEntryVersion = fileEntryVersion;
        }

        public FileEntryAttribute(string directory, string filename, DateTime lastAccesDateTime, FileEntryVersion fileEntryVersion) : base(directory, filename, lastAccesDateTime)
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
                (
                (this.FileEntryVersion == FileEntryVersion.Current && this.FileEntryVersion == other.FileEntryVersion) || //Don't care about updated LastWrittenTime when current version
                (this.FileEntryVersion != FileEntryVersion.Current && this.FileEntryVersion == other.FileEntryVersion && this.LastWriteDateTime == other.LastWriteDateTime)
                );
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
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + FileEntryVersion.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<FileEntry>.Default.GetHashCode(FileEntry);
            return hashCode;
        }
    }

    [Serializable]
    public class FileEntryImage : FileEntry, IEquatable<FileEntryImage>
    {
        private Image image;
        //private string size;
        public Image Image { get => image; set => image = value; }

        public FileEntry FileEntry { get => new FileEntry(fullFilePath, lastWriteDateTime); }

        public FileEntryImage(FileEntryImage fileEntryImage) : base (fileEntryImage.FileEntry)
        {
            Image = fileEntryImage.image == null ? null : new Bitmap(fileEntryImage.image);
        }

        public FileEntryImage(FileEntry fileEntry) : base(fileEntry.FileFullPath, fileEntry.LastWriteDateTime)
        {

        }

        public FileEntryImage(FileEntry fileEntry, Image image) : this(fileEntry.FileFullPath, fileEntry.LastWriteDateTime, image)
        {

        }

        public FileEntryImage(string fullFilePath, DateTime lastAccesDateTime) : base(fullFilePath, lastAccesDateTime)
        {

        }

        public FileEntryImage(string directory, string filename, DateTime lastAccesDateTime, Image image ) 
            : this(Path.Combine(directory, filename), lastAccesDateTime, image)
        {
        }

        public FileEntryImage(string fullFilePath, DateTime lastAccesDateTime, Image image ) : base(fullFilePath, lastAccesDateTime)
        {
            this.image = image;
        }

        public override int GetHashCode()
        {
            var hashCode = -662501232;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Image>.Default.GetHashCode(image);
            return hashCode;
        }

        public override bool Equals(object other)
        {
            return this.Equals(other as FileEntryImage);
        }


        public bool Equals(FileEntryImage other)
        {
            if (other is null) return false; // If parameter is null, return false.
            if (Object.ReferenceEquals(this, other)) return true; // Optimization for a common success case.

            // Let base class check its own fields and do the run-time type comparison.
            return base.Equals((FileEntry)other);
        }

        public static bool operator ==(FileEntryImage left, FileEntryImage right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(FileEntryImage left, FileEntryImage right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
