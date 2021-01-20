using MetadataLibrary;
using System.Collections.Generic;
using System.Drawing;

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
        public FileEntryAttribute FileEntryAttribute { get; set; } = null;
        public Image Thumbnail { get; set; } = null;
        public Metadata Metadata { get; set; } = null;
        public ReadWriteAccess ReadWriteAccess { get; set; } = ReadWriteAccess.DefaultReadOnly;
        public bool IsDirty { get; set; } = false;
        public bool HasFileBeenUpdatedGiveUserAwarning {get; set; } = false;

        public DataGridViewGenericColumn(FileEntryAttribute fileEntryAttribute, Image thumbnail, Metadata metadata, ReadWriteAccess readWriteAccess)
        {
            this.FileEntryAttribute = fileEntryAttribute;
            this.Metadata = metadata;
            this.ReadWriteAccess = readWriteAccess;
            this.Thumbnail = thumbnail;
        }

        /*
        public override bool Equals(object obj)
        {
            return obj is DataGridViewGenericColumn column &&
                EqualityComparer<FileEntryImage>.Default.Equals(FileEntryAttribute, column.FileEntryAttribute) &&
                EqualityComparer<Metadata>.Default.Equals(Metadata, column.Metadata);
        }*/

        /*
        public override int GetHashCode()
        {
            int hashCode = -1761064430;
            hashCode = hashCode * -1521134295 + EqualityComparer<FileEntryImage>.Default.GetHashCode(FileEntryAttribute);
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
        */
    }
}
