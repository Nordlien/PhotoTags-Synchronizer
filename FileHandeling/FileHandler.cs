using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHandeling
{
    public static class FileHandler
    {
        //private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region Files locked, wait unlock, in cloud

        #region IsFileInCloud
        public static bool IsFileInCloud(string fullFileName)
        {
            //FILE_ATTRIBUTE_RECALL_ON_DATA_ACCESS - 0x400000 - When this attribute is set, it means that the file or directory is not fully present locally. For a file that means that not all of its data is on local storage (e.g. it may be sparse with some data still in remote storage). For a directory it means that some of the directory contents are being virtualized from another location. Reading the file / enumerating the directory will be more expensive than normal, e.g. it will cause at least some of the file/directory content to be fetched from a remote store. Only kernel-mode callers can set this bit.

            try
            {
                FileAttributes fileAttributes = File.GetAttributes(fullFileName);
                if ((((int)fileAttributes) & 0x000400000) == 0x000400000) return true;
            }
            catch { return true; }
            return false;
        }
        #endregion 

        #region IsFileVirual
        public static bool IsFileVirtual(string fullFileName)
        {
            //FILE_ATTRIBUTE_VIRTUAL - 500000 - This value is reserved for system use.
            try
            {
                FileAttributes fileAttributes = File.GetAttributes(fullFileName);
                if ((((int)fileAttributes) & 0x000500000) == 0x000500000) return true;
            }
            catch { return true; }
            return false;
        }
        #endregion 

        #region IsFileReadOnly
        public static bool IsFileReadOnly(string fileFullPath)
        {
            bool isReadOnly;
            try
            {
                isReadOnly = new FileInfo(fileFullPath).IsReadOnly;
            }
            catch
            {
                isReadOnly = true;
            }
            return isReadOnly;
        }
        #endregion

        public static string FileLockedByProcess { get; set; }

        #region IsFileLockedByProcess
        public static bool IsFileLockedByProcess(string fullFilePath)
        {
            if (!File.Exists(fullFilePath)) return false;

            FileStream fs = null;
            try
            {
                // NOTE: This doesn't handle situations where file is opened for writing by another process but put into write shared mode, it will not throw an exception and won't show it as write locked
                fs = File.Open(fullFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None); // If we can't open file for reading and writing then it's locked by another process for writing
            }
            catch (UnauthorizedAccessException) // https://msdn.microsoft.com/en-us/library/y973b725(v=vs.110).aspx
            {
                // This is because the file is Read-Only and we tried to open in ReadWrite mode, now try to open in Read only mode
                try
                {
                    fs = File.Open(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (Exception)
                {
                    FileLockedByProcess = fullFilePath;
                    return true; // This file has been locked, we can't even open it to read
                }
            }
            catch (Exception)
            {
                FileLockedByProcess = fullFilePath;
                return true; // This file has been locked
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            FileLockedByProcess = "";
            return false;
        }
        #endregion

        #region IsFileThatNeedUpdatedLockedByProcess
        public static bool IsFileThatNeedUpdatedLockedByProcess(List<Metadata> fileEntriesToCheck)
        {
            if (fileEntriesToCheck.Count == 0) return false;

            foreach (Metadata fileEntryToCheck in fileEntriesToCheck)
            {
                if (!File.Exists(fileEntryToCheck.FileFullPath)) return false; //In process rename
                if (IsFileReadOnly(fileEntryToCheck.FileFullPath)) return false; //No need to wait, Attribute is set to read only
                if (IsFileLockedByProcess(fileEntryToCheck.FileFullPath)) return true; //In process OneDrive backup / update
            }
            return false;
        }
        #endregion 

        #region WaitLockedFilesToBecomeUnlocked

        public static void WaitLockedFilesToBecomeUnlocked(List<Metadata> fileEntriesToCheck)
        {
            int maxRetry = 30;
            bool areAnyFileLocked;
            do
            {
                areAnyFileLocked = IsFileThatNeedUpdatedLockedByProcess(fileEntriesToCheck);
                if (areAnyFileLocked) Task.Delay(3000).Wait();
                if (maxRetry-- < 0)
                {
                    if (MessageBox.Show(
                        "Other applications can lock files temporary e.g.Google Drive and OneDrive.\r\n" +
                        "Please turn of file sync temporary to avoid this issue, and to avoid duplicate sync files\r\n" +
                        "Will you retry waiting for file to be unlocked\r\n" + FileLockedByProcess,
                        "File(s) are locked by another applications", MessageBoxButtons.RetryCancel) == DialogResult.Retry) maxRetry = 15;
                    else areAnyFileLocked = false;
                }
            } while (areAnyFileLocked);
        }
        #endregion

        #region WaitLockedFilesToBecomeUnlocked
        /// <summary>
        /// Check if file is unlocked, if not wait. On timeout ask if wait more
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns>true - if unlocked and exist</returns>
        public static bool WaitLockedFileToBecomeUnlocked(string fileFullPath)
        {
            if (!File.Exists(fileFullPath)) return false; //Not locked, file doesn't exist

            /*List<Metadata> fileEntriesToCheck = new List<Metadata>();
            Metadata metadata = new Metadata(MetadataBrokerType.Empty);
            metadata.FileDirectory = Path.GetDirectoryName(fileFullPath);
            metadata.FileName = Path.GetFileName(fileFullPath);
            fileEntriesToCheck.Add(metadata);*/

            int maxRetry = 30;
            bool areAnyFileLocked;
            do
            {
                areAnyFileLocked = IsFileLockedByProcess(fileFullPath);

                if (areAnyFileLocked) Task.Delay(3000).Wait(); 
                if (maxRetry-- < 0)
                {
                    if (MessageBox.Show(
                        "Other applications can lock files temporary e.g.Google Drive and OneDrive.\r\n" +
                        "Please turn of file sync temporary to avoid this issue, and to avoid duplicate sync files\r\n" +
                        "Will you retry waiting for file to be unlocked\r\n" + FileLockedByProcess,
                        "File(s) are locked by another applications", MessageBoxButtons.RetryCancel) == DialogResult.Retry) maxRetry = 15;
                    else
                    {
                        areAnyFileLocked = false;
                        return false; //False, file still locked 
                    }
                }
            } while (areAnyFileLocked);
            return true; //True, file exist and unlocked 
        }
        #endregion  

        #endregion

    }
}
