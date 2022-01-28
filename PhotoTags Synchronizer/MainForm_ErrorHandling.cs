using DataGridViewGeneric;
using Exiftool;
using FileDateTime;
using FileHandeling;
using Krypton.Toolkit;
using Manina.Windows.Forms;
using MetadataLibrary;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thumbnails;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region Error Message handling
        private static string listOfErrors = "";
        private static bool hasWriteAndVerifyMetadataErrors = false;

        const string AddErrorFileSystemRegion = "FileSystem";
        const string AddErrorFileSystemCopy = "Copy";
        const string AddErrorFileSystemMove = "Move";
        const string AddErrorFileSystemRead = "Read";
        const string AddErrorFileSystemCopyFolder = "Copy Folder";
        const string AddErrorFileSystemMoveFolder = "Move Folder";
        const string AddErrorFileSystemCreateFolder = "Create Folder";
        const string AddErrorFileSystemDeleteFolder = "Delete Folder";

        const string AddErrorPropertiesRegion = "Properties";
        const string AddErrorPropertiesCommandWrite = "Write";
        const string AddErrorPropertiesParameterWrite = "Write";
        const string AddErrorExiftooRegion = "Exiftool";
        const string AddErrorExiftooCommandVerify = "Verify";
        const string AddErrorExiftooParameterVerify = "Verify";
        const string AddErrorExiftooCommandWrite = "Write";
        const string AddErrorExiftooParameterWrite = "Write";
        const string AddErrorExiftooCommandRead = "Read";
        const string AddErrorExiftooParameterRead = "Read";
        const string AddErrorParameterNone = "Error";

        

        public void AddError(
            string fileDirectory, string fileName, DateTime fileDateModified,
            string region, string command, string oldValue, string newValue,
            string warning)
        {
            AddError(fileDirectory, fileName, fileDateModified,
            region, command, oldValue,
            region, command, newValue,
            warning, true);
        }

        public void AddError(string fileDirectory, string region, string command, string oldValue, string newValue, string warning)
        {
            DateTime dateTimeLastWrittenDate = DateTime.Now;
            try
            {
                dateTimeLastWrittenDate = Directory.GetLastWriteTime(fileDirectory);
            }
            catch { }

            AddError(fileDirectory, "", dateTimeLastWrittenDate,
            region, command, oldValue,
            region, command, newValue,
            warning, false);
        }


        public void AddError(
            string fileDirectory, string fileName, DateTime fileDateModified,
            string oldRegion, string oldCommand, string oldParameter,
            string newRegion, string newCommand, string newParameter,
            string warning, bool writeToDatabase)
        {
            if (writeToDatabase)
            {
                ExiftoolData exiftoolDataOld = new ExiftoolData(fileName, fileDirectory, fileDateModified, oldRegion, oldCommand, oldParameter, null);
                ExiftoolData exiftoolDataNew = new ExiftoolData(fileName, fileDirectory, fileDateModified, newRegion, newCommand, newParameter, null);
                databaseExiftoolWarning.Write(exiftoolDataOld, exiftoolDataNew, warning);
            }

            string fullFilePath = Path.Combine(fileDirectory, fileName);
            lock (queueErrorQueueLock)
            {
                if (!queueErrorQueue.ContainsKey(fullFilePath)) queueErrorQueue.Add(fullFilePath, warning);
            }

            listOfErrors += warning + "\r\n------\r\n\r\n";
            hasWriteAndVerifyMetadataErrors = true;
            UpdateStatusAction("Saving metadata has errors...");
        }

        public void RemoveError(string fullFilePath)
        {
            lock (queueErrorQueueLock)
            {
                if (queueErrorQueue.ContainsKey(fullFilePath)) queueErrorQueue.Remove(fullFilePath);
            }
        }

        private static FormMessageBox formMessageBoxWarnings = null;
        private void timerShowErrorMessage_Tick(object sender, EventArgs e)
        {
            timerShowErrorMessage.Stop();
            if (hasWriteAndVerifyMetadataErrors)
            {
                string errors = listOfErrors;
                listOfErrors = "";
                hasWriteAndVerifyMetadataErrors = false;

                //MessageBox.Show(errors, "Warning or Errors has occured!", MessageBoxButtons.OK);
                if (formMessageBoxWarnings == null || formMessageBoxWarnings.IsDisposed) formMessageBoxWarnings = new FormMessageBox("Warning", errors);
                else formMessageBoxWarnings.AppendMessage(errors);
                formMessageBoxWarnings.Owner = this;
                formMessageBoxWarnings.Show();
            }
            try
            {
                timerShowErrorMessage.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "timerShowErrorMessage.Start failed.");
            }
        }
        #endregion

    }
}
