using Manina.Windows.Forms;
using MetadataLibrary;
using System.Collections.Generic;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{
    public static class ImageListViewHandler
    {
        private static HashSet<FileEntry> imageListViewSelectedFileEntriesCache = null;
        private static HashSet<string> imageListViewSelectedFilesCache = null;

        #region ImageListView - Cache - ClearCacheFileEntries
        public static void ClearCacheFileEntries(ImageListView imageListView)
        {
            //imageListViewFileEntriesCache = null;
        }
        #endregion

        #region ImageListView - Cache - ClearAllAndCaches
        public static void ClearAllAndCaches(ImageListView imageListeView)
        {
            imageListeView.ClearSelection();
            imageListeView.Items.Clear();
            imageListeView.ClearThumbnailCache();
            imageListeView.Refresh();

            ClearCacheFileEntries(imageListeView);
            ClearCacheFileEntriesSelectedItems(imageListeView);
        }
        #endregion

        #region ImageListView - Cache - ClearThumbnailCache
        public static void ClearThumbnailCache(ImageListView imageListeView)
        {
            imageListeView.ClearThumbnailCache();
        }
        #endregion

        #region ImageListView - Cache - ClearCacheFileEntriesSelectedItems
        public static void ClearCacheFileEntriesSelectedItems(ImageListView imageListView)
        {
            imageListViewSelectedFileEntriesCache = null;
            imageListViewSelectedFilesCache = null;
        }
        #endregion

        #region ImageListView - GetFileEntriesItems
        public static HashSet<FileEntry> GetFileEntriesItems(ImageListView imageListView)
        {
            HashSet<FileEntry> fileEntries = new HashSet<FileEntry>();
            try
            {
                foreach (ImageListViewItem imageListViewItem in imageListView.Items)
                {
                    FileEntry fileEntry = new FileEntry(imageListViewItem.FileFullPath, imageListViewItem.DateModified);
                    if (!fileEntries.Contains(fileEntry)) fileEntries.Add(fileEntry);                    
                }
            }
            catch { }
            return fileEntries;
        }
        #endregion

        #region ImageListView - Cache - HashSet<FileEntry> - GetFileEntriesSelectedItemsCache
        public static HashSet<FileEntry> GetFileEntriesSelectedItemsCache(ImageListView imageListView, bool allowUseCache)
        {
            if (!allowUseCache) imageListViewSelectedFileEntriesCache = null;
            if (imageListViewSelectedFileEntriesCache != null) return imageListViewSelectedFileEntriesCache;

            HashSet<FileEntry> fileEntries = new HashSet<FileEntry>();
            HashSet<string> fullFilePaths = new HashSet<string>();
            try
            {
                int queueCount = imageListView.SelectedItems.Count;
                int queueSize = queueCount;

                foreach (ImageListViewItem imageListViewItem in imageListView.SelectedItems)
                {
                    FileEntry fileEntry = new FileEntry(imageListViewItem.FileFullPath, imageListViewItem.DateModified);
                    if (!fileEntries.Contains(fileEntry)) fileEntries.Add(fileEntry);
                    if (!fullFilePaths.Contains(fileEntry.FileFullPath)) fullFilePaths.Add(fileEntry.FileFullPath);
                }

                imageListViewSelectedFileEntriesCache = new HashSet<FileEntry>(fileEntries);
                imageListViewSelectedFilesCache = new HashSet<string>(fullFilePaths);
            }
            catch { }
            return fileEntries;
        }
        #endregion

        #region ImageListView - Cache - HashSet<string> - GetFilesSelectedItemsCache
        public static HashSet<string> GetFilesSelectedItemsCache(ImageListView imageListView)
        {
            if (imageListViewSelectedFilesCache == null) GetFileEntriesSelectedItemsCache(imageListView, false);
            return imageListViewSelectedFilesCache;
        }
        #endregion

        #region ImageListView - Cache - DoesExistInSelectedFiles
        public static bool DoesExistInSelectedFiles(ImageListView imageListView, string fullFilename)
        {
            if (imageListViewSelectedFileEntriesCache == null) GetFileEntriesSelectedItemsCache(imageListView, false);
            return imageListViewSelectedFilesCache.Contains(fullFilename);
        }
        #endregion 

        #region ImageListView - AddItem
        public static void ImageListViewAddItem(ImageListView imageListView, string fullFileName, ref bool hasTriggerLoadAllMetadataActions, ref HashSet<string> keepTrackOfLoadedMetadata)
        {
            hasTriggerLoadAllMetadataActions = false;
            keepTrackOfLoadedMetadata.Add(fullFileName);
            imageListView.Items.Add(fullFileName);
            ClearCacheFileEntries(imageListView);
        }
        #endregion

        #region ImageListView - RemoveItem
        public static void ImageListViewRemoveItem(ImageListView imageListView, ImageListViewItem foundItem)
        {
            imageListView.Items.Remove(foundItem);
            ClearCacheFileEntries(imageListView);
        }
        #endregion

        #region ImageListView - FindItem
        public static ImageListViewItem FindItem(ImageListViewItemCollection imageListViewItemCollection, string fullFilename)
        {
            ImageListViewItem foundItem = null;

            foreach (ImageListViewItem item in imageListViewItemCollection)
            {
                if (FilesCutCopyPasteDrag.IsFilenameEqual(item.FileFullPath, fullFilename))
                {
                    foundItem = item;
                    break;
                }
            }
            return foundItem;
        }
        #endregion

        #region ImageListView - Enable
        public static void Enable(ImageListView imageListView, bool enabled)
        {
            //imageListView.Enabled = enabled;
        }
        #endregion

        public static void SuspendLayout(ImageListView imageListView)
        {
            imageListView.SuspendLayout();
        }

        public static void ResumeLayout(ImageListView imageListView)
        {
            imageListView.ResumeLayout();
        }
    }
}
