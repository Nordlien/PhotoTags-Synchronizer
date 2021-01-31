using DataGridViewGeneric;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
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
            DataGridView dataGridView, MetadataDatabaseCache databaseAndCacheMetadataExiftool, ThumbnailDatabaseCache databaseAndCacheThumbnail, ImageListViewSelectedItemCollection imageListViewItems, bool useCurrentFileLastWrittenDate,
            ReadWriteAccess readWriteAccessForColumn, ShowWhatColumns showWhatColumns, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewItems)
            {
                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.DateModified, FileEntryVersion.Current);
                
                Image thumbnail = null;
                if (databaseAndCacheThumbnail != null) thumbnail = databaseAndCacheThumbnail.ReadThumbnailFromCacheOnlyClone(fileEntryAttribute.FileEntry);
                //imageListViewItem.ThumbnailImage == null ? null : new Bitmap((Image)imageListViewItem.ThumbnailImage);
                
                Metadata metadata = null;
                //if (databaseAndCacheMetadataExiftool != null) metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool));
                
                DataGridViewHandler.AddColumnOrUpdateNew(dataGridView, fileEntryAttribute, thumbnail, metadata, readWriteAccessForColumn, showWhatColumns, dataGridViewGenericCellStatusDefault);
            }
        }
        #endregion

    }
}
