using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SqliteDatabase;
using DataGridViewGeneric;
using Manina.Windows.Forms;
using MetadataLibrary;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        #region Update Status Bar
        private void ExiftoolReader_afterNewMediaFoundEvent(FileEntry fileEntry)
        {
            lock (queueMetadataExiftool)
            {
                queueMetadataExiftool.Remove(fileEntry);
            }
            UpdateStatusAction("EXIF data saved: " + fileEntry.FileName);
            UpdateStatusReadWriteStatus_NeedToBeUpated(); //Update number count also
        }

        private void timerActionStatusRemove_Tick(object sender, EventArgs e)
        {
            timerActionStatusRemove.Stop();
            toolStripStatusAction.Text = "Waiting actions...";
        }

        private void imageListView1_ThumbnailCaching(object sender, ItemEventArgs e)
        {
            UpdateStatusAction(string.Format("Cacheing image: {0}", e.Item.Text));
        }

        private void UpdateStatusAction(string text)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(UpdateStatusAction), text);
                return;
            }

            toolStripStatusAction.Text = text;
            timerActionStatusRemove.Stop(); //Restart
            timerActionStatusRemove.Start();
        }


        private void UpdateStatusReadWriteStatus_NeedToBeUpated()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(UpdateStatusReadWriteStatus_NeedToBeUpated));
                return;
            }

            toolStripStatusFilesAndSelected.Text = string.Format("Files: {0} Selected {1} ", imageListView1.Items.Count, imageListView1.SelectedItems.Count);

            int regionCount = 0;
            try
            {
                //It's not thread safe
                foreach (Metadata metadataRegionCount in queueThumbnailRegion) regionCount += metadataRegionCount.PersonalRegionList.Count;
            } catch { }

            toolStripStatusQueue.Text = string.Format("Exif:{0} Processing:{1} WLPG:{2} MP:{3} Thumbnails: {4} Regions: {5} Saving: {6} Verify: {7}",
                queueMetadataExiftool.Count,
                metaFileNotInDatabase.Count,
                queueMetadataWindowsLivePhotoGallery.Count,
                queueMetadataMicrosoftPhotos.Count,
                queueSaveThumbnails.Count,
                regionCount,
                queueSaveMetadataUpdatedByUser.Count,
                queueVerifyMetadata.Count);
        }
        #endregion
    }
}
