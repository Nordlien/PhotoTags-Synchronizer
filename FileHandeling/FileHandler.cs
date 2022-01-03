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
        public static int WaitTimeBetweenCheckFileIsUnlocked { get; set; } = 500;
        public static int WaitNumberOfRetryBeforeShowMessage { get; set; } = 3;

        public static Form MainForm { get; set; } = null;

        #region IsStillInCloudAfterTouchFileActivateReadFromCloud
        public static bool IsStillInCloudAfterTouchFileActivateReadFromCloud(string fileFullPath)
        {
            try
            {
                byte[] buffer = new byte[512];
                using (FileStream fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read))
                {
                    var bytes_read = fs.Read(buffer, 0, buffer.Length); //Get OneDrive to start download the file
                    fs.Close();
                }
                return false;
            }
            catch { }
            return true;
        }
        #endregion

        #region GetItemFileStatus
        public static ItemFileStatus GetItemFileStatus(string fullFileName, bool checkLockedStatus = false, bool isDownloadingFromCloud = false, bool isExiftoolRunning = false)
        {
            ItemFileStatus itemFileStatus = new ItemFileStatus();
            try
            {
                #region Exists
                itemFileStatus.IsDirty = false;
                itemFileStatus.FileExists = File.Exists(fullFileName);
                FileInfo fileInfo = null;
                if (itemFileStatus.FileExists) fileInfo = new FileInfo(fullFileName);
                itemFileStatus.FailedToAccessInfo = false;
                #endregion

                #region Located
                itemFileStatus.IsInCloud = itemFileStatus.FileExists && IsFileInCloud(fullFileName);
                itemFileStatus.IsVirtual = itemFileStatus.FileExists && IsFileVirtual(fullFileName);
                itemFileStatus.IsOffline = fileInfo.Attributes == System.IO.FileAttributes.Offline;
                #endregion

                #region Access
                itemFileStatus.IsReadOnly = itemFileStatus.FileExists && fileInfo.Attributes == System.IO.FileAttributes.ReadOnly; // IsFileReadOnly(fullFileName);
                itemFileStatus.IsFileLockedReadAndWrite = itemFileStatus.IsInCloudOrVirtualOrOffline ||
                    (!itemFileStatus.IsInCloudOrVirtualOrOffline && itemFileStatus.FileExists && checkLockedStatus && IsFileLockedForReadAndWrite(fullFileName, 100));
                itemFileStatus.IsFileLockedForRead = itemFileStatus.IsInCloudOrVirtualOrOffline ||
                    (!itemFileStatus.IsInCloudOrVirtualOrOffline && itemFileStatus.FileExists && checkLockedStatus && IsFileLockedForRead(fullFileName, 100));
                #endregion

                #region Processes
                itemFileStatus.IsDownloadingFromCloud = isDownloadingFromCloud;
                itemFileStatus.IsExiftoolRunning = isExiftoolRunning;
                #endregion

            } catch
            {
                #region Exists
                itemFileStatus.IsDirty = false;
                itemFileStatus.FileExists = false;
                itemFileStatus.FailedToAccessInfo = true;
                #endregion

                #region Access
                itemFileStatus.IsReadOnly = true;
                itemFileStatus.IsFileLockedReadAndWrite = false;
                itemFileStatus.IsFileLockedForRead = false;
                #endregion

                #region Located
                itemFileStatus.IsInCloud = false;
                itemFileStatus.IsVirtual = false;
                itemFileStatus.IsOffline = false;
                #endregion

                #region Processes
                itemFileStatus.IsDownloadingFromCloud = isDownloadingFromCloud;
                itemFileStatus.IsExiftoolRunning = isExiftoolRunning;
                #endregion
            }
            return itemFileStatus;
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

        #region FileStatusText
        public static string FileStatusText(string fullFileName)
        {
            string status = "";
            if (!File.Exists(fullFileName)) status = "File not exists";
            else
            { 
                status =  
                    (IsFileInCloud(fullFileName) ? "File is in cloud" : "File is not in cloud") + "\r\n" +
                    (IsFileLockedForReadAndWrite(fullFileName, FileHandler.GetFileLockedStatusTimeout) ? "File is locked by process" : "File is not locked by process") + "\r\n" +
                    (IsFileLockedForRead(fullFileName) ? "File is locked for reading" : "File is not locked for reading") + "\r\n" +
                    (IsFileVirtual(fullFileName) ? "File is virtual" : "File is not virtual");
                try
                {
                    status = status + "\r\nAttribute: " + File.GetAttributes(fullFileName).ToString();
                } catch { }
                try
                {
                    status = status + "\r\nCreation Time: " + File.GetCreationTime(fullFileName).ToString();
                } catch { }
                try
                {
                    status = status + "\r\nLast Write Time: " + File.GetLastWriteTime(fullFileName).ToString();
                } catch { }
            }
            return status;
        }
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

        #region IsFileLockedByProcess
        private static List<string> inProcessIsFileLockedByProcess = new List<string>();
        public static bool IsFileLockedForReadAndWrite(string fullFilePath, int millisecondsTimeout)
        {   
            bool result = false;
            try
            {
                if (inProcessIsFileLockedByProcess.Contains(fullFilePath)) return true;
                inProcessIsFileLockedByProcess.Add(fullFilePath);
            }
            catch
            {
                try
                {
                    inProcessIsFileLockedByProcess.Clear();
                }
                catch { }
            }

            Task task = Task.Run(() =>
            {
                result = IsFileLockedByProcess(fullFilePath);
            });
            if (!task.Wait(millisecondsTimeout)) result = true; 
            // task.Wait(millisecondsTimeout);
            try
            {
                inProcessIsFileLockedByProcess.Remove(fullFilePath);
            }
            catch
            {
                try
                {
                    inProcessIsFileLockedByProcess.Clear();
                }
                catch { }
            }
            return result;
        }

        private static bool IsFileLockedByProcess(string fullFilePath)
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

        #region 

        #endregion

        #region IsFileThatNeedUpdatedLockedByProcess
        public static bool IsFileThatNeedUpdatedLockedByProcess(List<Metadata> fileEntriesToCheck, bool needWriteAccess)
        {
            if (fileEntriesToCheck.Count == 0) return false;
            foreach (Metadata fileEntryToCheck in fileEntriesToCheck)
            {
                //if (!File.Exists(fileEntryToCheck.FileFullPath)) return false; //In process rename
                //if (File.Exists(fileEntryToCheck.FileFullPath) && IsFileLockedForRead(fileEntryToCheck.FileFullPath, WaitFileGetUnlockedTimeout)) return true;

                if (File.Exists(fileEntryToCheck.FileFullPath))
                {
                    if (needWriteAccess)
                    {
                        //if (IsFileReadOnly(fileEntryToCheck.FileFullPath)) return true; //No need to wait, Attribute is set to read only
                        //if (IsFileReadOnly(fileEntryToCheck.FileFullPath)) return false; //No need to wait, Attribute is set to read only
                        if (IsFileLockedForReadAndWrite(fileEntryToCheck.FileFullPath, WaitFileGetUnlockedTimeout)) return true; //In process OneDrive backup / update
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

        #region WaitLockedFilesToBecomeUnlocked
        static FormWaitLockedFile formWaitLockedFile = new FormWaitLockedFile();
        public static bool WaitLockedFilesToBecomeUnlocked(List<Metadata> fileEntriesToCheck, bool needWriteAccess, Form formOwner)
        {
            int waitBeforeShowRefreshMessage = WaitNumberOfRetryBeforeShowMessage;
            
            bool areAnyFileLocked;
            do
            {
                areAnyFileLocked = IsFileThatNeedUpdatedLockedByProcess(fileEntriesToCheck, needWriteAccess);
                
                if (areAnyFileLocked) 
                    Task.Delay(WaitTimeBetweenCheckFileIsUnlocked).Wait();

                if (formWaitLockedFile != null && !formWaitLockedFile.IsFormVisible)
                {
                    if (formWaitLockedFile.IgnoredClicked)
                    {
                        FileLockedByProcess = "";
                        return false; //False, file still locked
                    }
                }

                if (areAnyFileLocked && --waitBeforeShowRefreshMessage < 0)
                {
                    try
                    {
                        if (formWaitLockedFile == null || formWaitLockedFile.IsDisposed) formWaitLockedFile = new FormWaitLockedFile();
                        if (formOwner != null) formWaitLockedFile.Owner = formOwner;

                        List<string> listOfFiles = new List<string>();
                        string statusOnFiles = "";
                        foreach (Metadata metadata in fileEntriesToCheck)
                        {
                            listOfFiles.Add(metadata.FileFullPath);
                            statusOnFiles += metadata.FileFullPath + "\r\n";
                            try
                            {
                                if (!File.Exists(metadata.FileFullPath)) statusOnFiles += "- File doesn't exist\r\n";
                            }
                            catch { }
                            if (IsFileLockedForReadAndWrite(metadata.FileFullPath, FileHandler.GetFileLockedStatusTimeout)) statusOnFiles += "- File is Locked by an other application\r\n";
                            if (IsFileReadOnly(metadata.FileFullPath)) statusOnFiles += "- File is read only\r\n";
                            if (IsFileVirtual(metadata.FileFullPath)) statusOnFiles += "- File is virtual\r\n";
                            if (IsFileInCloud(metadata.FileFullPath)) statusOnFiles += "- File is in cloud\r\n";
                        }
                        formWaitLockedFile.AddFiles(listOfFiles);

                        if (formOwner != null)
                        {
                            _ = formOwner.BeginInvoke(new Action<string>(formWaitLockedFile.SetTextboxFiles), statusOnFiles);
                            if (!formWaitLockedFile.IsFormVisible) _ = formOwner.BeginInvoke(new Action(formWaitLockedFile.Show));
                        }
                        else
                        {
                            if (formWaitLockedFile != null && !formWaitLockedFile.IsHandleCreated) formWaitLockedFile.Show();
                            
                            if (formWaitLockedFile != null && formWaitLockedFile.IsHandleCreated)
                            {
                                _ = formWaitLockedFile.BeginInvoke(new Action<string>(formWaitLockedFile.SetTextboxFiles), statusOnFiles);
                                if (!formWaitLockedFile.IsFormVisible) _ = formWaitLockedFile.BeginInvoke(new Action(formWaitLockedFile.Show));
                            }
                        }

                        if (formWaitLockedFile.IgnoredClicked) return false; //False, file still locked 
                        waitBeforeShowRefreshMessage = WaitNumberOfRetryBeforeShowMessage;
                    }
                    catch { 
                    }
                }
            } while (areAnyFileLocked);

            try
            {
                if (formWaitLockedFile != null && formWaitLockedFile.IsHandleCreated)
                {
                    if (formOwner != null) _ = formOwner.BeginInvoke(new Action(formWaitLockedFile.Close));
                    else _ = formWaitLockedFile.BeginInvoke(new Action(formWaitLockedFile.Close));
                    //formWaitLockedFile.Close();
                }
            }
            catch { }
            return true; //True, file exist and unlocked, or ignored 
        }
        #endregion

        #region WaitLockedFilesToBecomeUnlocked
        /// <summary>
        /// Check if file is unlocked, if not wait. On timeout ask if wait more
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns>true - if unlocked and exist</returns>
        public static bool WaitLockedFileToBecomeUnlocked(string fileFullPath, bool needWriteAccess, Form formOwner)
        {
            if (!File.Exists(fileFullPath)) return false; //Not locked, file doesn't exist

            List<Metadata> fileEntriesToCheck = new List<Metadata>();
            Metadata metadata = new Metadata(MetadataBrokerType.Empty);
            metadata.FileDirectory = Path.GetDirectoryName(fileFullPath);
            metadata.FileName = Path.GetFileName(fileFullPath);
            fileEntriesToCheck.Add(metadata);

            return WaitLockedFilesToBecomeUnlocked(fileEntriesToCheck, needWriteAccess, formOwner);

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
                WaitLockedFileToBecomeUnlocked(exiftoolArgFileFullPath, true, formOwner);
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

                    int indexOfMachineName = fileEntryMaybeHasMachineName.FileFullPath.IndexOf(machineName, StringComparison.OrdinalIgnoreCase);
                    if (indexOfMachineName >= 0)
                    {
                        int charsBehindMachineName = fileEntryMaybeHasMachineName.FileFullPath.Length - indexOfMachineName + machineNameLength;

                        switch (charsBehindMachineName) //Validate position and extras chars behind machinename
                        {
                            case 0: //Ok
                                machineNameFound = true;
                                break;
                            case 1: //
                                machineNameFound = false;
                                break;
                            case 2: //Somthing more behine -MachineName-x
                                if (fileEntryMaybeHasMachineName.FileFullPath[indexOfMachineName + machineNameLength] == '-' &&
                                char.IsDigit(fileEntryMaybeHasMachineName.FileFullPath[indexOfMachineName + machineNameLength + 1])) machineNameFound = true; //numberExtraCharBehind = 2;
                                break;
                            case 3: //Somthing more behine -MachineName-xx
                                if (fileEntryMaybeHasMachineName.FileFullPath[indexOfMachineName + machineNameLength] == '-' &&
                                char.IsDigit(fileEntryMaybeHasMachineName.FileFullPath[indexOfMachineName + machineNameLength + 1]) &&
                                char.IsDigit(fileEntryMaybeHasMachineName.FileFullPath[indexOfMachineName + machineNameLength + 2])) machineNameFound = true; //numberExtraCharBehind = 3;
                                break;
                            default: //not ok
                                machineNameFound = false;
                                break;
                        }

                        if (machineNameFound && !fixError) return true;
                        string pathWithoutMachineName = fileEntryMaybeHasMachineName.FileFullPath.Substring(0, indexOfMachineName);
                        FileEntry fileEntryWithoutMachineName = new FileEntry(fileEntryMaybeHasMachineName.FileFullPath, fileEntryMaybeHasMachineName.LastWriteDateTime);

                        if (fileEntries.Contains(fileEntryWithoutMachineName))
                        {
                            if (letNewestFileWin)
                            {
                                try
                                {
                                    DateTime dateTimeWithoutMachineName = fileEntryWithoutMachineName.LastWriteDateTime;
                                    DateTime dateTimeHasMachineName = fileEntryMaybeHasMachineName.LastWriteDateTime;


                                    WaitLockedFileToBecomeUnlocked(fileEntryMaybeHasMachineName.FileFullPath, true, form);
                                    if (dateTimeHasMachineName > dateTimeWithoutMachineName)
                                    {
                                        try
                                        {
                                            WaitLockedFileToBecomeUnlocked(fileEntryWithoutMachineName.FileFullPath, true, form);
                                            File.Delete(fileEntryWithoutMachineName.FileFullPath);
                                            File.Move(fileEntryMaybeHasMachineName.FileFullPath, fileEntryWithoutMachineName.FileFullPath);
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Error(ex);
                                            KryptonMessageBox.Show(ex.Message + "\r\nWas trying to replace\r\n" + fileEntryWithoutMachineName.FileFullPath + "\r\n with\r\n" + fileEntryMaybeHasMachineName.FileFullPath, 
                                                "Was not able to remove dubpliacted file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                                    WaitLockedFileToBecomeUnlocked(fileEntryMaybeHasMachineName.FileFullPath, true, form);
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
