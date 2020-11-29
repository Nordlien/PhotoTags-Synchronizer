using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SqliteDatabase;
using DataGridViewGeneric;
using Manina.Windows.Forms;
using MetadataLibrary;
using System.Collections.Generic;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {

        #region UpdateStatusAction - Remove (+Timer stop)
        private void timerActionStatusRemove_Tick(object sender, EventArgs e)
        {
            timerActionStatusRemove.Stop();
            toolStripStatusAction.Text = "Waiting actions...";
        }
        #endregion

        #region UpdateStatusAction - (+Timer start)
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
        #endregion 

        #region UpdatedStatusAction - Trigger by imageListView1_ThumbnailCaching
        private void imageListView1_ThumbnailCaching(object sender, ItemEventArgs e)
        {
            UpdateStatusAction(string.Format("Cacheing image: {0}", e.Item.Text));
        }
        #endregion

        #region UpdateStatusAction & UpdateStatusAllQueueStatus - Trigger by ExiftoolReader_afterNewMediaFoundEvent
        private void ExiftoolReader_afterNewMediaFoundEvent(FileEntry fileEntry)
        {
            lock (commonQueueReadMetadataFromExiftool)
            {
                commonQueueReadMetadataFromExiftool.Remove(fileEntry);
            }
            UpdateStatusAction("Exiftool read and cached: " + fileEntry.FileName);
            UpdateStatusAllQueueStatus(); //Update number count also
        }
        #endregion

        #region UpdateStatusAllQueueStatus
        private void UpdateStatusAllQueueStatus()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(UpdateStatusAllQueueStatus));
                return;
            }

            toolStripStatusFilesAndSelected.Text = string.Format("Files: {0} Selected {1} ", imageListView1.Items.Count, imageListView1.SelectedItems.Count);

            int regionCount = 0;
            try
            {
                //It's not thread safe
                foreach (Metadata metadataRegionCount in commonQueueReadPosterAndSaveFaceThumbnails) regionCount += metadataRegionCount.PersonalRegionList.Count;
            } catch { }

            toolStripStatusQueue.Text = string.Format("Exif:{0} Processing:{1} WLPG:{2} MP:{3} Thumbnails: {4} Regions: {5} Saving: {6} Verify: {7}",
                commonQueueReadMetadataFromExiftool.Count,
                mediaFilesNotInDatabase.Count,
                commonQueueReadMetadataFromWindowsLivePhotoGallery.Count,
                commonQueueReadMetadataFromMicrosoftPhotos.Count,
                commonQueueSaveThumbnailToDatabase.Count,
                regionCount,
                CountSaveQueue(), //queueSaveMetadataUpdatedByUser.Count,
                commonQueueMetadataWrittenByExiftoolReadyToVerify.Count);
        }
        #endregion

        #region UpdateExiftoolSaveStatus - Trigger by Timer Tick
        private void timerShowExiftoolSaveProgress_Tick(object sender, EventArgs e)
        {
            UpdateExiftoolSaveStatus();
        }
        #endregion

        #region UpdateExiftoolSaveStatus - Show Exiftool write progress
        private void UpdateExiftoolSaveStatus()
        {
            try
            {
                lock (fileSaveSize)
                {
                    if (fileSaveSize.Count == 0) return;
                    foreach (KeyValuePair<string, long> keyValuePair in fileSaveSize)
                    {
                        string tempFile = keyValuePair.Key + "_exiftool_tmp";
                        if (File.Exists(tempFile))
                        {
                            long tempFileSize = new FileInfo(tempFile).Length;
                            if (keyValuePair.Value != tempFileSize) UpdateStatusAction("Exiftool written " + tempFileSize + " bytes on " + Path.GetFileName(keyValuePair.Key));
                            fileSaveSize[keyValuePair.Key] = tempFileSize;
                        }
                    }
                }
            }
            catch { }
        }
        #endregion 

        #region UpdateExiftoolSaveStatus - Helper functions
        Dictionary<string, long> fileSaveSize = new Dictionary<string, long>();
        private void ShowExiftoolSaveProgressClear()
        {
            lock (fileSaveSize)
            {
                fileSaveSize.Clear();
            }
            //timerShowExiftoolSaveProgress.Start();
            //Application.DoEvents();
        }

        private void ShowExiftoolSaveProgressStop()
        {
            //fileSaveSizefileSaveSize.Clear();
            //timerShowExiftoolSaveProgress.Stop();
        }

        private int FileSaveSizeCount()
        {
            lock (fileSaveSize) return fileSaveSize.Count;
        }

        private void AddWatcherShowExiftoolSaveProcessQueue(string fullFileName)
        {
            lock (fileSaveSize)
            {
                if (!fileSaveSize.ContainsKey(fullFileName)) fileSaveSize.Add(fullFileName, 0);
            }
        }

        private int CountSaveQueue()
        {
            int countToSave = commonQueueSaveMetadataUpdatedByUser.Count;
            try
            {
                lock (fileSaveSize)
                {
                    if (CommonQueueSaveMetadataUpdatedByUserCount() == 0 || fileSaveSize.Count == 0) return countToSave;
                    countToSave = commonQueueSaveMetadataUpdatedByUser.Count; //In the queue
                    foreach (KeyValuePair<string, long> keyValuePair in fileSaveSize) if (keyValuePair.Value == 0) countToSave++; //In progress with Exiftool
                    if (countToSave >= 0) countToSave++; //Something is in progress saving
                }
            }
            catch { } //It's not thread safe, if error, don't care
            return countToSave;
        }
        #endregion 

        

        
    }
}
