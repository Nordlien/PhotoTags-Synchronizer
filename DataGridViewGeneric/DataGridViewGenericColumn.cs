using MetadataLibrary;
using System;
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
        public Image thumbnailUnlock = null;
        public Image Thumbnail 
        {
            get 
            {
                lock (_ThumbnailLock) return thumbnailUnlock;
            }
            set 
            {
                lock (_ThumbnailLock) thumbnailUnlock = value;
            } 
        } 
        public readonly Object _ThumbnailLock = new Object();

        public Metadata Metadata { get; set; } = null;
        public ReadWriteAccess ReadWriteAccess { get; set; } = ReadWriteAccess.DefaultReadOnly;
        public bool IsDirty { get; set; } 
        public bool HasFileBeenUpdatedGiveUserAwarning {get; set; } = false;

        public bool IsPopulated { get; set; } = false;

        public DataGridViewGenericColumn(FileEntryAttribute fileEntryAttribute, Image thumbnail, Metadata metadata, ReadWriteAccess readWriteAccess)
        {
            this.FileEntryAttribute = fileEntryAttribute;
            this.Metadata = metadata;
            this.ReadWriteAccess = readWriteAccess;
            this.Thumbnail = thumbnail;
        }

    }
}
