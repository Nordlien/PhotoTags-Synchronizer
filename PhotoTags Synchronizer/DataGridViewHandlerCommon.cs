using DataGridViewGeneric;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerCommon
    {
        #region Column handling - AddColumnSelectedFiles
        public static void AddColumnSelectedFiles(
            DataGridView dataGridView, MetadataDatabaseCache DatabaseAndCacheMetadataExiftool, ImageListViewSelectedItemCollection imageListViewItems, bool useCurrentFileLastWrittenDate,
            ReadWriteAccess readWriteAccessForColumn, ShowWhatColumns showWhatColumns, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewItems)
            {
                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.DateModified, FileEntryVersion.Current);
                Image thumbnail = imageListViewItem.ThumbnailImage == null ? null : new Bitmap((Image)imageListViewItem.ThumbnailImage);
                Metadata metadata = null;
                //if (DatabaseAndCacheMetadataExiftool != null) metadata = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool));
                DataGridViewHandler.AddColumnOrUpdateNew(dataGridView, fileEntryAttribute, thumbnail, metadata, readWriteAccessForColumn, showWhatColumns, dataGridViewGenericCellStatusDefault);
            }
        }
        #endregion
        /*
        public static void AddVisibleFiles(List<FileEntryAttribute> allFileEntryAttributeDateVersions, List<FileEntryAttribute> fileEntryAttributeDateVersions, ShowWhatColumns showWhatColumns)
        {
            bool showErrorColumns = (showWhatColumns & ShowWhatColumns.ErrorColumns) > 0;
            bool showHirstoryColumns = (showWhatColumns & ShowWhatColumns.HistoryColumns) > 0;
            foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributeDateVersions)
            {
                switch (fileEntryAttribute.FileEntryVersion)
                {
                    case FileEntryVersion.Historical:
                        if (!showHirstoryColumns) allFileEntryAttributeDateVersions.Add(new FileEntryAttribute(fileEntryAttribute.FileEntry,FileEntryVersion.Remove));
                        else allFileEntryAttributeDateVersions.Add(fileEntryAttribute);
                        break;
                    case FileEntryVersion.Error:
                        if (!showErrorColumns) allFileEntryAttributeDateVersions.Add(new FileEntryAttribute(fileEntryAttribute.FileEntry, FileEntryVersion.Remove));
                        else allFileEntryAttributeDateVersions.Add(fileEntryAttribute);
                        break;
                    case FileEntryVersion.Current:
                        allFileEntryAttributeDateVersions.Add(fileEntryAttribute);
                        break;

                    case FileEntryVersion.NotAvailable:
                    default:
                        throw new Exception("Not implmented");
                }
                
            }
        }
        */
    }
}
