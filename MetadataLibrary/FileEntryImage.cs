using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MetadataLibrary
{

    [Serializable]
    public class FileEntryImage : FileEntry, IEquatable<FileEntryImage>
    {
        private Image image;        
        public Image Image { get => image; set => image = value; }
        public bool AllowLoadFromCloud { get; set; } = false;

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

        public FileEntryImage(FileEntry fileEntry, Image image, bool allowLoadFromCloud) : this(fileEntry.FileFullPath, fileEntry.LastWriteDateTime, image)
        {
            AllowLoadFromCloud = allowLoadFromCloud;
        }

        public FileEntryImage(string fullFilePath, DateTime lastWriteDateTime) : base(fullFilePath, lastWriteDateTime)
        {

        }

        public FileEntryImage(string directory, string filename, DateTime lastWriteDateTime, Image image ) 
            : this(Path.Combine(directory, filename), lastWriteDateTime, image)
        {
        }

        public FileEntryImage(string fullFilePath, DateTime lastWriteDateTime, Image image ) : base(fullFilePath, lastWriteDateTime)
        {
            this.image = image;
        }

        public override int GetHashCode()
        {
            var hashCode = 982998765;
            hashCode = hashCode * -1254903653 + base.GetHashCode();
            //hashCode = hashCode * -1254903653 + EqualityComparer<Image>.Default.GetHashCode(image);
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
