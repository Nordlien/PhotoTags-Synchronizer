using DataGridViewGeneric;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Thumbnails;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerCommon
    {
        #region Column handling - AddColumnSelectedFiles
        public static void AddColumnSelectedFiles(
            DataGridView dataGridView, ThumbnailDatabaseCache databaseAndCacheThumbnail, HashSet<FileEntry> imageListViewItems, 
            ReadWriteAccess readWriteAccessForColumn, ShowWhatColumns showWhatColumns, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault)
        {
            foreach (FileEntry imageListViewItem in imageListViewItems)
            {
                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.LastWriteDateTime, FileEntryVersion.CurrentVersionInDatabase);                
                Image thumbnail = null;
                if (databaseAndCacheThumbnail != null) thumbnail = databaseAndCacheThumbnail.ReadThumbnailFromCacheOnly(fileEntryAttribute.FileEntry);                
                DataGridViewHandler.AddColumnOrUpdateNew(dataGridView, fileEntryAttribute, thumbnail, null, readWriteAccessForColumn, showWhatColumns, dataGridViewGenericCellStatusDefault, out _);
            }
        }
        #endregion

    }
}
