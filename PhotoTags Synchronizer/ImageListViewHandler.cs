using Manina.Windows.Forms;
using MetadataLibrary;
using System.Collections.Generic;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{
    public static class ImageListViewHandler
    {
        private static HashSet<FileEntry> imageListViewFileEntriesCache = null;
        private static HashSet<FileEntry> imageListViewSelectedFileEntriesCache = null;
        private static HashSet<string> imageListViewSelectedFilesCache = null;

        #region ImageListView - ClearAllAndCaches
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

        #region ImageListView - ClearThumbnailCache
        public static void ClearThumbnailCache(ImageListView imageListeView)
        {
            imageListeView.ClearThumbnailCache();
        }
        #endregion

        #region ImageListView - ClearCacheFileEntries
        public static void ClearCacheFileEntries(ImageListView imageListView)
        {
            imageListViewFileEntriesCache = null;
        }
        #endregion

        #region ImageListView - ClearCacheFileEntriesSelectedItems
        public static void ClearCacheFileEntriesSelectedItems(ImageListView imageListView)
        {
            imageListViewSelectedFileEntriesCache = null;
            imageListViewSelectedFilesCache = null;
        }
        #endregion

        #region ImageListView - HashSet<FileEntry> - GetFileEntriesAllItemsCache
        public static HashSet<FileEntry> GetFileEntriesAllItemsCache(ImageListView imageListView)
        {
            if (imageListViewFileEntriesCache == null)
            {
                imageListViewFileEntriesCache = new HashSet<FileEntry>();
                //Read to cache
                foreach (ImageListViewItem imageListViewItem in imageListView.Items)
                {
                    imageListViewFileEntriesCache.Add(new FileEntry(imageListViewItem.FileFullPath, imageListViewItem.DateModified));
                }
            }
            return imageListViewFileEntriesCache;
        }
        #endregion

        #region ImageListView - HashSet<FileEntry> - GetFileEntriesSelectedItemsCache
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


                //LoadingItemsImageListView(0, queueCount);

                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();
                foreach (ImageListViewItem imageListViewItem in imageListView.SelectedItems)
                {
                    //LoadingItemsImageListView(--queueSize, queueCount);
                    FileEntry fileEntry = new FileEntry(imageListViewItem.FileFullPath, imageListViewItem.DateModified);
                    if (!fileEntries.Contains(fileEntry)) fileEntries.Add(fileEntry);
                    if (!fullFilePaths.Contains(fileEntry.FileFullPath)) fullFilePaths.Add(fileEntry.FileFullPath);

                    //if (stopwatch.ElapsedMilliseconds > 200)
                    //{
                    //    kryptonWorkspaceCellMediaFiles.Refresh();
                    //    stopwatch.Restart();
                    //}
                }
                //LoadingItemsImageListView(0, 0);

                imageListViewSelectedFileEntriesCache = fileEntries;
                imageListViewSelectedFilesCache = fullFilePaths;
            }
            catch { }
            return fileEntries;
        }
        #endregion

        #region ImageListView - FileEntry - GetFileEntryFromSelectedFilesCached
        public static FileEntry GetFileEntryFromSelectedFilesCached(ImageListView imageListView, string fullFileName)
        {
            foreach (FileEntry fileEntry in GetFileEntriesSelectedItemsCache(imageListView, true))
            {
                if (fileEntry.FileFullPath == fullFileName) return fileEntry;
            }
            return null;
        }
        #endregion

        #region ImageListView - SetFileEntriesNewCache
        public static void SetFileEntriesNewCache(ImageListView imageListView, HashSet<FileEntry> newFileList)
        {
            imageListViewFileEntriesCache = new HashSet<FileEntry>(newFileList);
        }
        #endregion


        #region ImageListView - AddItem
        public static void ImageListViewAddItem(ImageListView imageListView, string fullFilename)
        {
            imageListView.Items.Add(fullFilename);
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
                if (item.FileFullPath == fullFilename)
                {
                    foundItem = item;
                    break;
                }
            }
            return foundItem;
        }
        #endregion

        #region ImageListView - SetItemDirty
        public static void SetItemDirty(ImageListView imageListView, string fullfilename)
        {
            ImageListViewItem imageListViewItem = FindItem(imageListView.Items, fullfilename);
            if (imageListViewItem != null) imageListViewItem.Dirty();
        }
        #endregion

        #region ImageListView - Enable
        public static void Enable(ImageListView imageListView, bool enabled)
        {
            //imageListView.Enabled = enabled;
        }
        #endregion
    }
}
