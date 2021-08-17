using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SqliteDatabase;
using DataGridViewGeneric;
using System.Diagnostics;
using Manina.Windows.Forms;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        /*
        private void FileSystemWatcherOnRenamed(object sender, RenamedEventArgs e)
        {
            if (!ImageAndMovieFileExtentionsUtility.IsMediaFormat(e.FullPath)) return;
           Logger.Trace(e.OldFullPath + " -->" + e.FullPath);
            throw new NotImplementedException();
        }

        private void FileSystemWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            if (!ImageAndMovieFileExtentionsUtility.IsMediaFormat(e.FullPath)) return;

            foreach (ImageListViewItem item in imageListView1.Items)
            {

                if (e.FullPath == Path.Combine(item.FileDirectory, item.FileFullPath))
                {
                    if (File.Exists(e.FullPath) && File.GetLastWriteTime(e.FullPath) == item.DateModified)
                    {
                        Logger.Warn("FileSystemWatcherOnChanged was not updated: " + e.FullPath + " " + e.ChangeType.ToString());
                        return; //Due to ExifTool updated it and system already knows
                    }

//DeleteMetadataBrokersFromDatabaseAndCache(item.FullFileName); //Do not delete, keep history

                    this.BeginInvoke(new Action(item.Update));

                    //item.Update();
                    Logger.Trace("FileSystemWatcherOnChanged: " + e.FullPath + " " + e.ChangeType.ToString());

                    FileEntry fileEntry = new FileEntry(
                    item.FileDirectory, item.FileFullPath,
                    item.DateAccessed);

                    AddQueueMetadataUpdatedFileEntry(new FileEntry(Path.Combine(item.FileDirectory, item.FileFullPath), item.DateModified));
                    //return;
                    throw new Exception("Add null ??"); // break;
                }
            }

            //GlobalData.isPopulatingImageListView = false;
        }

        private void FileSystemWatcherOnDeleted(object sender, FileSystemEventArgs e)
        {
            if (!ImageAndMovieFileExtentionsUtility.IsMediaFormat(e.FullPath)) return;

            Logger.Trace("FileSystemWatcherOnChanged: " + e.FullPath + " " + e.ChangeType.ToString());
            if (File.Exists(e.FullPath))
            {
                Logger.Warn("FileSystemWatcherOnChanged was not deleted: " + e.FullPath + " " + e.ChangeType.ToString());
                return; //Due to ExifTool delete old file
            }

            foreach (ImageListViewItem item in imageListView1.SelectedItems)
            {
                if (e.FullPath == Path.Combine(item.FileDirectory, item.FileFullPath))
                {
                    //imageListView1.SelectedItems.(item);

                    item.Selected = false;
                    break;
                }
            }

            foreach (ImageListViewItem item in imageListView1.Items)
            {
                if (e.FullPath == item.FileFullPath)
                {
                    //FileEntry fileEntry = new FileEntry(item.FileDirectory, item.FullFileName, item.DateModified);

                    if (!File.Exists(e.FullPath))
                    {
                        this.BeginInvoke(new Action<ImageListViewItem>(imageListView1.Items.RemoveItem), item);
                        //imageListView1.Items.Remove(item);
                        filesCutCopyPasteDrag.DeleteMetadataHirstory(e.FullPath);
                        Logger.Trace("FileSystemWatcherOnChanged was removed: " + e.FullPath + " " + e.ChangeType.ToString());

                    }
                    else
                    {
                        Logger.Warn("FileSystemWatcherOnChanged was NOT removed, still exists: " + e.FullPath + " " + e.ChangeType.ToString());
                    }
                    break;
                }
            }

        }

        private void FileSystemWatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            //if (GlobalData.IsPopulatingAnyGridView()) return;
            //GlobalData.isPopulatingImageListView = true;
            if (!ImageAndMovieFileExtentionsUtility.IsMediaFormat(e.FullPath)) return;

            Logger.Trace("FileSystemWatcherOnChanged: " + e.FullPath + " " + e.ChangeType.ToString());
            foreach (ImageListViewItem item in imageListView1.SelectedItems)
            {
                if (e.FullPath == Path.Combine(item.FileDirectory, item.FileFullPath) &&
                    (File.Exists(e.FullPath) && File.GetLastWriteTime(e.FullPath) == item.DateModified))
                {
                    Logger.Warn("FileSystemWatcherOnChanged was not created: " + e.FullPath + " " + e.ChangeType.ToString());
                    return; //Due to ExifTool updated it and system already knows
                }
            }

            Logger.Trace("FileSystemWatcherOnChanged was added: " + e.FullPath + " " + e.ChangeType.ToString());
            this.BeginInvoke(new Action<string>(imageListView1.Items.Add), e.FullPath);

            //imageListView1.Items.Add(Path.Combine(e.FullPath, e.Name));

            //FolderSelected_AddFilesImageListView(selectedFolder);
            ThreadCollectMetadataExiftool();
            ThreadCollectMetadataMicrosoftPhotos();
            ThreadCollectMetadataWindowsLiveGallery();

            //GlobalData.isPopulatingImageListView = false;
        }
        */
    }
}
