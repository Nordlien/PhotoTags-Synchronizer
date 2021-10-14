using Krypton.Toolkit;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHandeling
{


    #region LockFinder
    public class LockFinder
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region LockFinder - FindLockProcesses
        public List<Process> FindLockProcesses(string path)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            uint handle;
            string key = Guid.NewGuid().ToString();
            List<Process> processes = new List<Process>();
            
            int res = RmStartSession(out handle, 0, key);
            Logger.Debug("RmStartSession: " + stopwatch.ElapsedMilliseconds);
            if (res != 0) throw new Exception("Could not begin restart session. Unable to determine file locker.");


            try
            {
                const int ERROR_MORE_DATA = 234;
                uint pnProcInfoNeeded = 0, pnProcInfo = 0,
                    lpdwRebootReasons = RmRebootReasonNone;
                string[] resources = new string[] { path };

                res = RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);
                Logger.Debug("RmRegisterResources: " + stopwatch.ElapsedMilliseconds);
                if (res != 0) throw new Exception("Could not register resource.");

                res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);
                Logger.Debug("RmGetList: " + stopwatch.ElapsedMilliseconds);
                if (res == ERROR_MORE_DATA)
                {
                    RM_PROCESS_INFO[] processInfo = new RM_PROCESS_INFO[pnProcInfoNeeded];
                    pnProcInfo = pnProcInfoNeeded;
                    // Get the list.
                    res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
                    if (res == 0)
                    {
                        processes = new List<Process>((int)pnProcInfo);
                        for (int i = 0; i < pnProcInfo; i++)
                        {
                            try
                            {
                                processes.Add(Process.GetProcessById(processInfo[i].
                                    Process.dwProcessId));
                            }
                            catch (ArgumentException) { }
                        }
                    }
                    else
                    {
                        throw new Exception("Could not list processes locking resource");
                    }
                }
                else if (res != 0)
                {
                    throw new Exception("Could not list processes locking resource. Failed to get size of result.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                RmEndSession(handle);
            }

            if (stopwatch.ElapsedMilliseconds > 1000)
                Logger.Debug("FindLockProcesses: " + stopwatch.ElapsedMilliseconds);
            return processes;
        }
        #endregion 

        #region LockFinder - Const
        private const int RmRebootReasonNone = 0;
        private const int CCH_RM_MAX_APP_NAME = 255;
        private const int CCH_RM_MAX_SVC_NAME = 63;
        
        [StructLayout(LayoutKind.Sequential)]
        struct RM_UNIQUE_PROCESS
        {
            public int dwProcessId;
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded,
                                    ref uint pnProcInfo, [In, Out] RM_PROCESS_INFO[] rgAffectedApps,
                                    ref uint lpdwRebootReasons);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct RM_PROCESS_INFO
        {
            public RM_UNIQUE_PROCESS Process;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            public string strAppName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            public string strServiceShortName;
            public RM_APP_TYPE ApplicationType;
            public uint AppStatus;
            public uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }

        enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int RmRegisterResources(uint pSessionHandle,
                                            UInt32 nFiles, string[] rgsFilenames,
                                            UInt32 nApplications,
                                            [In] RM_UNIQUE_PROCESS[] rgApplications,
                                            UInt32 nServices, string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags,
                                        string strSessionKey);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int RmEndSession(uint pSessionHandle);
        #endregion 

    }
    #endregion

    public static class FileHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static int GetFileLockedStatusTimeout { get; set; } = 500;
        public static int WaitFileGetUnlockedTimeout { get; set; } = 1000;
        public static int WaitTimeBetweenCheckFileIsUnlocked { get; set; } = 500;
        public static int RetryCheckFileIsUnlocked { get; set; } = 30;

        #region IsFileInCloud
        public static bool IsFileInCloud(string fullFileName)
        {
            //FILE_ATTRIBUTE_RECALL_ON_DATA_ACCESS - 0x400000 - When this attribute is set, it means that the file or directory is not fully present locally. For a file that means that not all of its data is on local storage (e.g. it may be sparse with some data still in remote storage). For a directory it means that some of the directory contents are being virtualized from another location. Reading the file / enumerating the directory will be more expensive than normal, e.g. it will cause at least some of the file/directory content to be fetched from a remote store. Only kernel-mode callers can set this bit.

            try
            {
                FileAttributes fileAttributes = File.GetAttributes(fullFileName);
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

        public static bool IsFileLockedForRead(string fullFilePath)
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
        public static bool IsFileLockedByProcess(string fullFilePath, int millisecondsTimeout)
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
            if (!task.Wait(millisecondsTimeout)) result = false;
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

        public static bool IsFileLockedByProcess(string fullFilePath)
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

            if (stopwatch.ElapsedMilliseconds > 1000)
                Logger.Debug("IsFileLockedByProcess: " + stopwatch.ElapsedMilliseconds);
            return false;
        }
        #endregion

        #region IsFileThatNeedUpdatedLockedByProcess
        public static bool IsFileThatNeedUpdatedLockedByProcess(List<Metadata> fileEntriesToCheck, bool needWriteAccess)
        {
            if (fileEntriesToCheck.Count == 0) return false;

            foreach (Metadata fileEntryToCheck in fileEntriesToCheck)
            {
                if (!File.Exists(fileEntryToCheck.FileFullPath)) return false; //In process rename
                if (needWriteAccess)
                {
                    if (IsFileReadOnly(fileEntryToCheck.FileFullPath)) return false; //No need to wait, Attribute is set to read only
                    if (IsFileLockedByProcess(fileEntryToCheck.FileFullPath, WaitFileGetUnlockedTimeout)) return true; //In process OneDrive backup / update
                } else
                {
                    if (IsFileLockedForRead(fileEntryToCheck.FileFullPath, WaitFileGetUnlockedTimeout)) return true;
                }
            }
            return false;
        }
        #endregion 

        #region WaitLockedFilesToBecomeUnlocked
        static FormWaitLockedFile formWaitLockedFile = new FormWaitLockedFile();

        public static bool WaitLockedFilesToBecomeUnlocked(List<Metadata> fileEntriesToCheck, bool needWriteAccess, Form form)
        {
            int waitBeforeShowRefreshMessage = RetryCheckFileIsUnlocked;
            bool areAnyFileLocked;
            do
            {
                areAnyFileLocked = IsFileThatNeedUpdatedLockedByProcess(fileEntriesToCheck, needWriteAccess);
                
                if (areAnyFileLocked) Task.Delay(WaitTimeBetweenCheckFileIsUnlocked).Wait();

                if (formWaitLockedFile != null && !formWaitLockedFile.IsFormVisible)
                {
                    if (formWaitLockedFile.IgnoredClicked)
                    {
                        FileLockedByProcess = "";
                        return false; //False, file still locked
                    }
                }

                if (areAnyFileLocked && waitBeforeShowRefreshMessage-- < 0)
                {
                    try
                    {
                        if (formWaitLockedFile == null || formWaitLockedFile.IsDisposed) formWaitLockedFile = new FormWaitLockedFile();
                        if (form != null) formWaitLockedFile.Owner = form;

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
                            if (IsFileLockedByProcess(metadata.FileFullPath, FileHandler.GetFileLockedStatusTimeout)) statusOnFiles += "- File is Locked by an other application\r\n";
                            if (IsFileReadOnly(metadata.FileFullPath)) statusOnFiles += "- File is read only\r\n";
                            if (IsFileVirtual(metadata.FileFullPath)) statusOnFiles += "- File is virtual\r\n";
                            if (IsFileInCloud(metadata.FileFullPath)) statusOnFiles += "- File is in cloud\r\n";
                        }
                        formWaitLockedFile.AddFiles(listOfFiles);

                        _ = form.BeginInvoke(new Action<string>(formWaitLockedFile.SetTextboxFiles), statusOnFiles);
                        if (!formWaitLockedFile.IsFormVisible) _ = form.BeginInvoke(new Action(formWaitLockedFile.Show)); 

                        if (formWaitLockedFile.IgnoredClicked) return false; //False, file still locked 
                        waitBeforeShowRefreshMessage = RetryCheckFileIsUnlocked;
                    }
                    catch { 
                    }
                }
            } while (areAnyFileLocked);
            try
            {
                if (formWaitLockedFile != null) formWaitLockedFile.Close();
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
        public static bool WaitLockedFileToBecomeUnlocked(string fileFullPath, bool needWriteAccess, Form form)
        {
            if (!File.Exists(fileFullPath)) return false; //Not locked, file doesn't exist

            List<Metadata> fileEntriesToCheck = new List<Metadata>();
            Metadata metadata = new Metadata(MetadataBrokerType.Empty);
            metadata.FileDirectory = Path.GetDirectoryName(fileFullPath);
            metadata.FileName = Path.GetFileName(fileFullPath);
            fileEntriesToCheck.Add(metadata);

            return WaitLockedFilesToBecomeUnlocked(fileEntriesToCheck, needWriteAccess, form);

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
        public static string GetLocalApplicationDataPath(string tempfilename, bool deleteOldTempFile)
        {
            //Create directory, filename and remove old arg file
            string exiftoolArgFileDirecory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            if (!Directory.Exists(exiftoolArgFileDirecory)) Directory.CreateDirectory(exiftoolArgFileDirecory);
            string exiftoolArgFileFullPath = Path.Combine(exiftoolArgFileDirecory, tempfilename);
            if (deleteOldTempFile && File.Exists(exiftoolArgFileFullPath)) File.Delete(exiftoolArgFileFullPath);
            return exiftoolArgFileFullPath;
        }
        #endregion

        public static string CombineApplicationPathWithFilename(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        #region FixOneDriveIssues 
        public static bool FixOneDriveIssues(HashSet<FileEntry> fileEntries, Form form, bool fixError = false, bool letNewstFileWin = true)
        {
            string machineName = "-" + Environment.MachineName;
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
                        if (letNewstFileWin)
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
                                        KryptonMessageBox.Show(ex.Message + "\r\nWas trying to replace\r\n" + fileEntryWithoutMachineName.FileFullPath + "\r\n with\r\n" + fileEntryMaybeHasMachineName.FileFullPath, "Was not able to remove dubpliacted file.");
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
                                        KryptonMessageBox.Show(ex.Message + "\r\n" + fileEntryMaybeHasMachineName.FileFullPath, "Was not able to remove dubpliacted file.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                                KryptonMessageBox.Show(ex.Message + "\r\n" + fileEntryWithoutMachineName.FileFullPath + "\r\n" + fileEntryMaybeHasMachineName.FileFullPath, "Was not able to remove the oldest of dubpliacted file.");
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
                                KryptonMessageBox.Show(ex.Message + "\r\n" + fileEntryMaybeHasMachineName.FileFullPath, "Was not able to remove dubpliacted file.");
                            }
                        }
                    }
                }
                
            }

            return false;
        }
        #endregion 
    }
}
