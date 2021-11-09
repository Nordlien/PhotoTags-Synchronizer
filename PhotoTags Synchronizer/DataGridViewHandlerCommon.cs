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

                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.DateModified,
                    GlobalData.ListOfAutoCorrectFilesContains(imageListViewItem.FileFullPath) ? FileEntryVersion.AutoCorrect : FileEntryVersion.Current);
                
                Image thumbnail = null;
                if (databaseAndCacheThumbnail != null) thumbnail = databaseAndCacheThumbnail.ReadThumbnailFromCacheOnlyClone(fileEntryAttribute.FileEntry);
                
                Metadata metadata = null; //Force to updated DataGridView without content only columns, works faster this way
                //if (databaseAndCacheMetadataExiftool != null) metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool));
                
                DataGridViewHandler.AddColumnOrUpdateNew(dataGridView, fileEntryAttribute, thumbnail, metadata, readWriteAccessForColumn, showWhatColumns, dataGridViewGenericCellStatusDefault);
            }
        }
        #endregion

    }
}
