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
            DataGridView dataGridView, MetadataDatabaseCache databaseAndCacheMetadataExiftool, ThumbnailDatabaseCache databaseAndCacheThumbnail, 
            HashSet<FileEntry> imageListViewItems, bool useCurrentFileLastWrittenDate,
            ReadWriteAccess readWriteAccessForColumn, ShowWhatColumns showWhatColumns, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault)
        {
            foreach (FileEntry imageListViewItem in imageListViewItems)
            {
                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.LastWriteDateTime,
                    GlobalData.ListOfAutoCorrectFilesContains(imageListViewItem.FileFullPath) ? FileEntryVersion.AutoCorrect : FileEntryVersion.CurrentVersionInDatabase);                
                Image thumbnail = null;
                if (databaseAndCacheThumbnail != null) thumbnail = databaseAndCacheThumbnail.ReadThumbnailFromCacheOnl(fileEntryAttribute.FileEntry);                
                DataGridViewHandler.AddColumnOrUpdateNew(dataGridView, fileEntryAttribute, thumbnail, null, readWriteAccessForColumn, showWhatColumns, dataGridViewGenericCellStatusDefault, out _);
            }
        }
        #endregion

    }
}
