using System;
using System.Collections.Generic;
using System.Drawing;


namespace MetadataLibrary
{

    /*

    [Serializable]
    public class FileEntryColumn_RemoveMe : FileEntry, IEquatable<FileEntryColumn>
    {
        private Image image;

        public Image Image { get => image; set => image = value; }
        //public int ColumnIndex { get => columnIndex; set => columnIndex = value; }
        public FileEntry FileEntry { get => new FileEntry(fullFilePath, lastAccessDateTime); }

        public FileEntryColumn(string fullFilePath, DateTime lastAccesDateTime) : this(fullFilePath, lastAccesDateTime, null)
        {
            //this.fullFilePath = fullFilePath;                 //this()
            //this.lastAccessDateTime = lastAccesDateTime;      //this()
        }

        public FileEntryColumn(string fullFilePath, DateTime lastAccesDateTime, Image image /*, int columnIndex/) : base (fullFilePath, lastAccesDateTime) 
        {
            //this.fullFilePath = fullFilePath;             //base()
            //this.lastAccessDateTime = lastAccesDateTime;  //base()
            this.image = image;
            //this.columnIndex = columnIndex;
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
            return this.Equals(other as FileEntryColumn);
        }


        public bool Equals(FileEntryColumn other)
        {
            if (other is null) return false; // If parameter is null, return false.
            if (Object.ReferenceEquals(this, other)) return true; // Optimization for a common success case.

            // Check properties that this class declares.
            // Let base class check its own fields and do the run-time type comparison.
            return base.Equals((FileEntry)other);
        }

        public static bool operator ==(FileEntryColumn left, FileEntryColumn right)
        {
            return EqualityComparer<FileEntryColumn>.Default.Equals(left, right);
        }

        public static bool operator !=(FileEntryColumn left, FileEntryColumn right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
        */
    
}
