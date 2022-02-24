using Krypton.Toolkit;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHandeling
{

    public static class FileHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static int GetFileLockedStatusTimeout { get; set; } = 500;
        public static int WaitFileGetUnlockedTimeout { get; set; } = 1000;
        //public static int WaitTimeBetweenCheckFileIsUnlocked { get; set; } = 500;
        //public static int WaitNumberOfRetryBeforeShowMessage { get; set; } = 3;

        #region GetCreationTime
        public static DateTime GetCreationTime(string fullFileName)
        {
            DateTime currentCreationTime = File.GetCreationTime(fullFileName);
            if (currentCreationTime < new DateTime(1700, 1, 1, 1, 1, 1))
            {
                Thread.Sleep(1000);
                currentCreationTime = File.GetCreationTime(fullFileName);
                //DEBUG
            }
            return currentCreationTime;
        }
        #endregion

        #region GetLastWriteTime
        public static DateTime GetLastWriteTime(string fullFileName, bool waitAndRetry = true)
        {
            DateTime currentLastWrittenDateTime = File.GetLastWriteTime(fullFileName);
            if (waitAndRetry && currentLastWrittenDateTime < new DateTime(1700, 1, 1, 1, 1, 1))
            {
                Thread.Sleep(1000);
                currentLastWrittenDateTime = File.GetLastWriteTime(fullFileName);
                //DEBUG
            }
            return currentLastWrittenDateTime;
        }
        #endregion

        #region GetFileStatusText
        public static string GetFileStatusText(string fullFileName,
            bool fileInaccessibleOrError = false,
            string fileErrorMessage = null,
            ExiftoolProcessStatus exiftoolProcessStatus = ExiftoolProcessStatus.DoNotUpdate,
            bool checkLockedStatus = false,
            int checkLockStatusTimeout = 100,
            bool showLockedByProcess = false)
        {
            FileStatus fileStatus = FileHandler.GetFileStatus(fullFileName,
                hasErrorOccured: fileInaccessibleOrError,
                errorMessage: fileErrorMessage,
                exiftoolProcessStatus: exiftoolProcessStatus,
                checkLockedStatus: checkLockedStatus,
                checkLockStatusTimeout: checkLockStatusTimeout);

            string fileStatusText = ConvertFileStatusToText(fileStatus, fullFileName);
            if (showLockedByProcess && fileStatus.HasAnyLocks) fileStatusText += "\r\n" + GetLockedByText(fullFileName);
            
            return fileStatusText;
        }
        #endregion

        #region GetItemFileStatus
        public static FileStatus GetFileStatus(string fullFileName, 
            bool hasErrorOccured = false,
            string errorMessage = null,
            ExiftoolProcessStatus exiftoolProcessStatus = ExiftoolProcessStatus.DoNotUpdate, 
            bool checkLockedStatus = false, 
            int checkLockStatusTimeout = 100)
        {
            FileStatus fileStatus = new FileStatus();
            try
            {
                #region File - Exists, Dirty or has Error
                fileStatus.IsDirty = false;
                fileStatus.FileExists = File.Exists(fullFileName);
                FileInfo fileInfo = null;
                if (fileStatus.FileExists) fileInfo = new FileInfo(fullFileName);
                fileStatus.HasErrorOccured = hasErrorOccured;
                fileStatus.FileErrorMessage = errorMessage;
                #endregion

                #region File - Location on Device or in Cloud
                fileStatus.IsInCloud = fileStatus.FileExists && IsFileInCloud(fullFileName);
                fileStatus.IsVirtual = fileStatus.FileExists && IsFileVirtual(fullFileName);
                fileStatus.IsOffline = fileStatus.FileExists && (fileInfo == null ? false : (fileInfo.Attributes & FileAttributes.Offline) == FileAttributes.Offline);
                #endregion

                #region File - Locks and Access rights
                fileStatus.IsReadOnly = 
                    fileStatus.FileExists && (fileInfo == null ? false : (fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly); 
                fileStatus.IsFileLockedReadAndWrite = fileStatus.IsInCloudOrVirtualOrOffline ||
                    (!fileStatus.IsInCloudOrVirtualOrOffline && fileStatus.FileExists && checkLockedStatus && IsFileLockedForReadAndWriteCached(fullFileName, checkLockStatusTimeout));
                fileStatus.IsFileLockedForRead = fileStatus.IsFileLockedReadAndWrite || fileStatus.IsInCloudOrVirtualOrOffline ||
                    (!fileStatus.IsInCloudOrVirtualOrOffline && fileStatus.FileExists && checkLockedStatus && IsFileLockedForRead(fullFileName, checkLockStatusTimeout));
                #endregion

                #region Exiftool - Where in Processes
                if (exiftoolProcessStatus != ExiftoolProcessStatus.DoNotUpdate) fileStatus.ExiftoolProcessStatus = exiftoolProcessStatus;
                #endregion

            } catch (Exception ex)
            {
                #region File - Exists, Dirty or has Error
                fileStatus.IsDirty = false;
                fileStatus.FileExists = false;
                fileStatus.HasErrorOccured = true;
                fileStatus.FileErrorMessage = ex.Message;
                #endregion

                #region File - Location on Device or in Cloud
                fileStatus.IsInCloud = false;
                fileStatus.IsVirtual = false;
                fileStatus.IsOffline = false;
                #endregion

                #region File - Locks and Access rights
                fileStatus.IsReadOnly = true;
                fileStatus.IsFileLockedReadAndWrite = false;
                fileStatus.IsFileLockedForRead = false;
                #endregion

                #region Exiftool - Where in Processes
                fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError;
                #endregion
            }
            return fileStatus;
        }
        #endregion

        #region ConvertFileStatusToText
        public static string ConvertFileStatusToText(FileStatus fileStatus, string fullFileName = null)
        {
            string status = fileStatus.ToString();

            if (fileStatus.FileExists && fullFileName != null)
            {
                try
                {
                    status = status + "\r\nFile Attributes: " + File.GetAttributes(fullFileName).ToString();
                }
                catch { }
                try
                {
                    status = status + "\r\nCreation Time: " + File.GetCreationTime(fullFileName).ToString();
                }
                catch { }
                try
                {
                    status = status + "\r\nLast Write Time: " + FileHandler.GetLastWriteTime(fullFileName).ToString();
                }
                catch { }
            }
            return status;
        }

        #endregion

        #region GetLockedByText
        public static string GetLockedByText(string fullFileName)
        {
            string lockedByProcessesText = "";
            List<Process> processes = FileHandler.FindLockProcesses(fullFileName);
            if (processes.Count == 0) lockedByProcessesText += "  Locked by process: Nothing found\r\n";
            else
            {
                foreach (Process process in processes) lockedByProcessesText += "  Locked by process: " + process + "\r\n";
            }
            return lockedByProcessesText;
        }
        #endregion 

        #region IsFileInCloud
        public static bool IsFileInCloud(string fullFileName)
        {
            //FILE_ATTRIBUTE_RECALL_ON_DATA_ACCESS - 0x400000 - When this attribute is set, it means that the file or directory is not fully present locally. 
            //For a file that means that not all of its data is on local storage (e.g. it may be sparse with some data still in remote storage). 
            //For a directory it means that some of the directory contents are being virtualized from another location. 
            //Reading the file / enumerating the directory will be more expensive than normal, e.g. it will cause at least some of the file/directory
            //content to be fetched from a remote store. Only kernel-mode callers can set this bit.

            try
            {
                FileAttributes fileAttributes = (FileAttributes)0;
                if (File.Exists(fullFileName)) fileAttributes = File.GetAttributes(fullFileName);
                if ((((int)fileAttributes) & 0x000400000) == 0x000400000) return true;
            }
            catch 
            { 
                return false; 
            }
            return false;
        }
        #endregion 

        #region IsFileVirual
        public static bool IsFileVirtual(string fullFileName)
        {
            //FILE_ATTRIBUTE_VIRTUAL - 500000 - This value is reserved for system use.
            try
            {
                FileAttributes fileAttributes = (FileAttributes)0;
                if (File.Exists(fullFileName)) fileAttributes = File.GetAttributes(fullFileName);
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
                if (File.Exists(fileFullPath)) isReadOnly = new FileInfo(fileFullPath).IsReadOnly;
                else isReadOnly = true;
            }
            catch
            {
                isReadOnly = true;
            }
            return isReadOnly;
        }
        #endregion

        #region IsStillInCloudAfterTouchFileActivateReadFromCloud
        private static Dictionary<string, DateTime> cloundFileTouchedAndWhen = new Dictionary<string, DateTime>();
        private static object cloundFileTouchedAndWhenLock = new object();

        private static Dictionary<string, DateTime> cloundFileTouchedFailedAndWhen = new Dictionary<string, DateTime>();
        private static object cloundFileTouchedFailedAndWhenLock = new object();

        private static int TouchTimeout = 1000 * 20;
        private static int TouchTimeoutFailed = 1000 * 60 * 2;

        #region RemoveOfflineFileTouched
        public static void RemoveOfflineFileTouched(string fullFileName)
        {
            lock (cloundFileTouchedAndWhenLock)
            {
                if (cloundFileTouchedAndWhen.ContainsKey(fullFileName)) cloundFileTouchedAndWhen.Remove(fullFileName);
            }
        }

        public static void RemoveOldOfflineFileTouched(string fullFileName)
        {
            lock (cloundFileTouchedAndWhenLock)
            {
                if (cloundFileTouchedAndWhen.ContainsKey(fullFileName))
                {
                    if ((DateTime.Now - cloundFileTouchedAndWhen[fullFileName]).TotalMilliseconds > TouchTimeout) //Remove when timeout
                        cloundFileTouchedAndWhen.Remove(fullFileName);
                }
            }
        }
        #endregion

        #region RemoveOfflineFileTouchedFailed
        public static void RemoveOfflineFileTouchedFailed(string fullFileName)
        {
            lock (cloundFileTouchedFailedAndWhenLock)
            {
                if (cloundFileTouchedFailedAndWhen.ContainsKey(fullFileName)) cloundFileTouchedFailedAndWhen.Remove(fullFileName);
            }
        }

        public static void RemoveOldOfflineFileTouchedFailed(string fullFileName)
        {
            lock (cloundFileTouchedFailedAndWhenLock)
            {
                if (cloundFileTouchedFailedAndWhen.ContainsKey(fullFileName))
                {
                    if ((DateTime.Now - cloundFileTouchedFailedAndWhen[fullFileName]).TotalMilliseconds > TouchTimeoutFailed) //Remove when timeout
                        cloundFileTouchedFailedAndWhen.Remove(fullFileName);
                }
            }
        }
        #endregion

        #region IsOfflineFileTouchedAndWithoutTimeout
        public static bool IsOfflineFileTouchedAndWithoutTimeout(string fullFileName)
        {
            bool isTounch = false;
            lock (cloundFileTouchedAndWhenLock)
            {
                if (cloundFileTouchedAndWhen.ContainsKey(fullFileName))
                {
                    if ((DateTime.Now - cloundFileTouchedAndWhen[fullFileName]).TotalMilliseconds < TouchTimeout) 
                        isTounch = true;
                }
            }
            return isTounch;
        }
        #endregion

        #region IsOfflineFileTouchedAndFailedWithoutTimedOut
        public static bool IsOfflineFileTouchedAndFailedWithoutTimedOut(string fullFileName)
        {
            bool isTounch = false;
            lock (cloundFileTouchedFailedAndWhenLock)
            {
                if (cloundFileTouchedFailedAndWhen.ContainsKey(fullFileName))
                {
                    if ((DateTime.Now - cloundFileTouchedFailedAndWhen[fullFileName]).TotalMilliseconds < TouchTimeoutFailed) 
                        isTounch = true;
                }
            }
            return isTounch;
        }
        #endregion

        #region Touch Offline File - Log errors 
        private static void TouchOfflineFileProcess(string fullFileName)
        {
            try
            {
                byte[] buffer = new byte[512];
                using (FileStream fs = new FileStream(fullFileName, FileMode.Open, FileAccess.Read))
                {
                    var bytes_read = fs.Read(buffer, 0, buffer.Length); //Get OneDrive to start download the file
                    fs.Close();
                }
            }
            catch
            {
                lock (cloundFileTouchedFailedAndWhenLock)
                {
                    if (!cloundFileTouchedFailedAndWhen.ContainsKey(fullFileName)) 
                        cloundFileTouchedFailedAndWhen.Add(fullFileName, DateTime.Now);
                }
            }
        }
        #endregion

        #region Touch Offline File To Get File Online
        public static void TouchOfflineFileToGetFileOnline(string fullFileName)
        {
            if (IsOfflineFileTouchedAndFailedWithoutTimedOut(fullFileName)) 
                return;
            RemoveOldOfflineFileTouchedFailed(fullFileName);

            if (IsOfflineFileTouchedAndWithoutTimeout(fullFileName)) 
                return;
            RemoveOldOfflineFileTouched(fullFileName);

            #region Touch the file and log when
            FileStatus fileStatus = GetFileStatus(fullFileName, checkLockedStatus: false);
            if (fileStatus.IsInCloudOrVirtualOrOffline)
            {
                lock (cloundFileTouchedAndWhenLock)
                {
                    if (!cloundFileTouchedAndWhen.ContainsKey(fullFileName)) cloundFileTouchedAndWhen.Add(fullFileName, DateTime.Now);
                }

                Task task = Task.Run(() =>
                {
                    TouchOfflineFileProcess(fullFileName);
                });
                task.Wait(1000);
            }
            #endregion
        }
        #endregion

        #endregion

        public static string FileLockedByProcess { get; set; }

        #region IsFileLockedForRead
        private static List<string> inProcessIsFileLockedForRead = new List<string>();
        public static bool IsFileLockedForRead(string fullFilePath, int millisecondsTimeout)
        {
            bool result = false;
            try
            {
                if (inProcessIsFileLockedForRead.Contains(fullFilePath)) return true;
                inProcessIsFileLockedForRead.Add(fullFilePath);
            }
            catch
            {                
                try
                {
                    inProcessIsFileLockedForRead.Clear();
                }
                catch { }
            }
            Task task = Task.Run(() =>
            {
                result = IsFileLockedForRead(fullFilePath);
            });
            if (!task.Wait(millisecondsTimeout)) result = false;
            try
            {
                inProcessIsFileLockedForRead.Remove(fullFilePath);
            }
            catch {
                try
                {
                    inProcessIsFileLockedForRead.Clear();
                }
                catch { }
            }
            return result;
        }

        private static bool IsFileLockedForRead(string fullFilePath)
        {
            if (!File.Exists(fullFilePath)) return false;

            bool isLockedForRead = true;
            try
            {
                using (var fs = File.Open(fullFilePath, FileMode.Open))
                {
                    isLockedForRead = !fs.CanRead;
                    var canWrite = fs.CanWrite;
                }
            } catch (Exception ex)
            {
                Logger.Debug(ex, "IsFileLockedForRead");
                isLockedForRead = true;
            }
            return isLockedForRead;
        }
        #endregion  

        #region IsFileLockedByProcess - Cache
        private static List<string> inProcessIsFileLockedByProcess = new List<string>();
        private static object inProcessIsFileLockedByProcessLock = new object();
        public static bool IsFileLockedForReadAndWriteCached(string fullFilePath, int millisecondsTimeout)
        {   
            bool result = false;
            try
            {
                lock (inProcessIsFileLockedByProcessLock)
                {
                    if (inProcessIsFileLockedByProcess.Contains(fullFilePath)) return true;
                    inProcessIsFileLockedByProcess.Add(fullFilePath);
                }
            }
            catch
            {
                try
                {
                    lock (inProcessIsFileLockedByProcessLock) inProcessIsFileLockedByProcess.Clear();
                }
                catch { }
            }

            Task task = Task.Run(() =>
            {
                result = IsFileLockedForReadOrWrite(fullFilePath);
            });
            if (!task.Wait(millisecondsTimeout)) result = true;
            // task.Wait(millisecondsTimeout);
            try
            {
                lock (inProcessIsFileLockedByProcessLock)
                {
                    if (inProcessIsFileLockedByProcess.Contains(fullFilePath))
                        inProcessIsFileLockedByProcess.Remove(fullFilePath);
                }
            }
            catch
            {
                try
                {
                    lock (inProcessIsFileLockedByProcessLock) inProcessIsFileLockedByProcess.Clear();
                }
                catch
                {
                }
            }
            return result;
        }

        private static bool IsFileLockedForReadOrWrite(string fullFilePath)
        {
            if (!File.Exists(fullFilePath)) return false;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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
        public static bool IsFileThatNeedUpdatedLockedByProcess(List<Metadata> fileEntriesToCheck, bool needWriteAccess)
        {
            if (fileEntriesToCheck.Count == 0) return false;
            foreach (Metadata fileEntryToCheck in fileEntriesToCheck)
            {
                if (File.Exists(fileEntryToCheck.FileFullPath))
                {
                    if (needWriteAccess)
                    {
                        if (IsFileLockedForReadAndWriteCached(fileEntryToCheck.FileFullPath, WaitFileGetUnlockedTimeout)) return true; //In process OneDrive backup / update
                    }
                    else
                    {
                        if (IsFileLockedForRead(fileEntryToCheck.FileFullPath, WaitFileGetUnlockedTimeout)) return true;
                    }
                }
            }
            return false;
        }
        #endregion 

        #region FindLockProcesses
        public static List<Process> FindLockProcesses(string path)
        {
            LockFinder lockFinder = new LockFinder();
            return lockFinder.FindLockProcesses(path);
        }
        #endregion

        #region GetLocalApplicationDataPath
        public static string GetLocalApplicationDataPath(string tempfilename, bool deleteOldTempFile, Form formOwner = null)
        {
            //Create directory, filename and remove old arg file

            string exiftoolArgFileDirecory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            if (!Directory.Exists(exiftoolArgFileDirecory)) Directory.CreateDirectory(exiftoolArgFileDirecory);
            string exiftoolArgFileFullPath = Path.Combine(exiftoolArgFileDirecory, tempfilename);
            try
            {
                if (deleteOldTempFile && File.Exists(exiftoolArgFileFullPath)) File.Delete(exiftoolArgFileFullPath);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            return exiftoolArgFileFullPath;
        }
        #endregion

        #region CombineApplicationPathWithFilename
        public static string CombineApplicationPathWithFilename(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }
        #endregion

        #region FixOneDriveIssues 
        public static bool FixOneDriveIssues(HashSet<FileEntry> fileEntries, Form form, List<string> listOfNetworkNames, bool fixError = false, bool letNewestFileWin = true)
        {

            foreach (string networkName in listOfNetworkNames)
            {
                string machineName = "-" + networkName;
                int machineNameLength = machineName.Length;

                foreach (FileEntry fileEntryMaybeHasMachineName in fileEntries)
                {
                    bool machineNameFound = false;

                    string filenameWithoutExtension = Path.GetFileNameWithoutExtension(fileEntryMaybeHasMachineName.FileName);
                    int indexOfMachineName = filenameWithoutExtension.IndexOf(machineName, StringComparison.OrdinalIgnoreCase);
                    if (indexOfMachineName >= 0)
                    {
                        int charsBehindMachineName = filenameWithoutExtension.Length - indexOfMachineName - machineNameLength;

                        if (charsBehindMachineName == 0) machineNameFound = true;
                        else if (charsBehindMachineName == 1) machineNameFound = false;
                        else if (charsBehindMachineName == 2)
                        {
                            if (filenameWithoutExtension[indexOfMachineName + machineNameLength] == '-' &&
                                char.IsDigit(filenameWithoutExtension[indexOfMachineName + machineNameLength + 1])) machineNameFound = true; //numberExtraCharBehind = 2;

                        }
                        else if (charsBehindMachineName == 3)
                        {
                            if (fileEntryMaybeHasMachineName.FileFullPath[indexOfMachineName + machineNameLength] == '-' &&
                                char.IsDigit(filenameWithoutExtension[indexOfMachineName + machineNameLength + 1]) &&
                                char.IsDigit(filenameWithoutExtension[indexOfMachineName + machineNameLength + 2])) machineNameFound = true; //numberExtraCharBehind = 3;
                        }
                        else
                        {
                            if (filenameWithoutExtension.IndexOf(machineName, indexOfMachineName, StringComparison.OrdinalIgnoreCase) != -1)
                                machineNameFound = true;
                            else
                                machineNameFound = false;
                        }

                        
                        
                        //string filenameWithoutMachineName = fileEntryMaybeHasMachineName
                        string pathWithoutMachineName = filenameWithoutExtension.Substring(0, indexOfMachineName);
                        FileEntry fileEntryWithoutMachineName = new FileEntry(
                            Path.Combine(Path.GetDirectoryName(fileEntryMaybeHasMachineName.FileFullPath),
                            pathWithoutMachineName + Path.GetExtension(fileEntryMaybeHasMachineName.FileFullPath)), 
                            fileEntryMaybeHasMachineName.LastWriteDateTime);

                        if (machineNameFound && !fixError)
                        {
                            return fileEntries.Contains(fileEntryWithoutMachineName);
                        }

                        if (fixError)
                        {
                            if (fileEntries.Contains(fileEntryWithoutMachineName))
                            {
                                if (letNewestFileWin)
                                {
                                    try
                                    {
                                        DateTime dateTimeWithoutMachineName = fileEntryWithoutMachineName.LastWriteDateTime;
                                        DateTime dateTimeHasMachineName = fileEntryMaybeHasMachineName.LastWriteDateTime;

                                        if (dateTimeHasMachineName > dateTimeWithoutMachineName)
                                        {
                                            try
                                            {
                                                File.Delete(fileEntryWithoutMachineName.FileFullPath);
                                                File.Move(fileEntryMaybeHasMachineName.FileFullPath, fileEntryWithoutMachineName.FileFullPath);
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.Error(ex);
                                                KryptonMessageBox.Show(ex.Message + "\r\nWas trying to replace\r\n" + fileEntryWithoutMachineName.FileFullPath + "\r\n with\r\n" + fileEntryMaybeHasMachineName.FileFullPath,
                                                    "Was not able to remove duplicated file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                File.Delete(fileEntryMaybeHasMachineName.FileFullPath);
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.Error(ex);
                                                KryptonMessageBox.Show(ex.Message + "\r\n" + fileEntryMaybeHasMachineName.FileFullPath,
                                                    "Was not able to remove dubpliacted file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex);
                                        KryptonMessageBox.Show(ex.Message + "\r\n" + fileEntryWithoutMachineName.FileFullPath + "\r\n" + fileEntryMaybeHasMachineName.FileFullPath,
                                            "Was not able to remove the oldest of dubpliacted file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        File.Delete(fileEntryMaybeHasMachineName.FileFullPath);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex);
                                        KryptonMessageBox.Show(ex.Message + "\r\n" + fileEntryMaybeHasMachineName.FileFullPath,
                                            "Was not able to remove dubpliacted file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            return false;
        }
        #endregion

        #region TrimFolderName
        public static string TrimFolderName(string path, string replace, string replaceWith)
        {
            while (path.Contains(replace)) path = path.Replace(replace, replaceWith);
            return path;
        }
        public static string TrimFolderName(string path)
        {      
            return TrimFolderName(path, " \\", "\\"); //https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.createdirectory?view=net-5.0
        }
        #endregion

        #region CombinePathAndName
        public static string CombinePathAndName(string folder, string filename)
        {
            string path = Path.GetFullPath(Path.Combine(folder.TrimEnd(), filename));   //Trailing spaces are removed from the end of the path parameter before creating the directory.            
            return path;
        }
        #endregion
    }
}
