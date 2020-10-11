using MetadataLibrary;
using System.Collections.Generic;

namespace DataGridViewGeneric
{
    /// <summary>
    /// 
    /// </summary>
    public enum ReadWriteAccess
    {
        /// <summary>
        /// All celles in this column will be read only, example: when showing historical data, when show stored when updated metadata on file failed
        /// </summary>
        ForceCellToReadOnly,  
        /// <summary>
        /// 
        /// </summary>
        AllowCellReadAndWrite,
        /// <summary>
        /// This only used in Windows Poperties Form, TODO: I guess this can be removed and use AllowCellReadAndWrite instead
        /// </summary>
        DefaultReadOnly
    }

    public class DataGridViewGenericColumn
    {
        public FileEntryImage FileEntryImage { get; set; }
        public Metadata Metadata { get; set; }
        public ReadWriteAccess ReadWriteAccess { get; set; }

        public DataGridViewGenericColumn(FileEntryImage fileEntryImage, Metadata metadata, ReadWriteAccess readWriteAccess)
        {
            this.FileEntryImage = fileEntryImage;
            this.Metadata = metadata;
            this.ReadWriteAccess = readWriteAccess;
        }

        public override bool Equals(object obj)
        {
            return obj is DataGridViewGenericColumn column &&
                EqualityComparer<FileEntryImage>.Default.Equals(FileEntryImage, column.FileEntryImage) &&
                EqualityComparer<Metadata>.Default.Equals(Metadata, column.Metadata);
        }

        public override int GetHashCode()
        {
            int hashCode = -1761064430;
            hashCode = hashCode * -1521134295 + EqualityComparer<FileEntryImage>.Default.GetHashCode(FileEntryImage);
            hashCode = hashCode * -1521134295 + EqualityComparer<Metadata>.Default.GetHashCode(Metadata);
            return hashCode;
        }

        public static bool operator ==(DataGridViewGenericColumn left, DataGridViewGenericColumn right)
        {
            return EqualityComparer<DataGridViewGenericColumn>.Default.Equals(left, right);
        }

        public static bool operator !=(DataGridViewGenericColumn left, DataGridViewGenericColumn right)
        {
            return !(left == right);
        }
    }
}
