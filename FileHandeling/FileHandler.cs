using Krypton.Toolkit;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Security.Principal;

namespace FileHandeling
{
    

    /// <summary>
    /// Simplifies the creation of folders in the CommonApplicationData folder
    /// and setting of permissions for all users.
    /// </summary>
    public class CommonApplicationData {
        private string applicationFolder;
        private string companyFolder;
        private static readonly string directory =
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        /// <summary>
        /// Creates a new instance of this class creating the specified company and application folders
        /// if they don't already exist and optionally allows write/modify to all users.
        /// </summary>
        /// <param name="companyFolder">The name of the company's folder (normally the company name).</param>
        /// <param name="applicationFolder">The name of the application's folder (normally the application name).</param>
        /// <remarks>If the application folder already exists then permissions if requested are NOT altered.</remarks>
        public CommonApplicationData(string companyFolder, string applicationFolder)
            : this(companyFolder, applicationFolder, false) { }
        /// <summary>
        /// Creates a new instance of this class creating the specified company and application folders
        /// if they don't already exist and optionally allows write/modify to all users.
        /// </summary>
        /// <param name="companyFolder">The name of the company's folder (normally the company name).</param>
        /// <param name="applicationFolder">The name of the application's folder (normally the application name).</param>
        /// <param name="allUsers">true to allow write/modify to all users; otherwise, false.</param>
        /// <remarks>If the application folder already exists then permissions if requested are NOT altered.</remarks>
        public CommonApplicationData(string companyFolder, string applicationFolder, bool allUsers) {
            this.applicationFolder = applicationFolder;
            this.companyFolder = companyFolder;
            CreateFolders(allUsers);
        }

        /// <summary>
        /// Gets the path of the application's data folder.
        /// </summary>
        public string ApplicationFolderPath {
            get { return Path.Combine(CompanyFolderPath, applicationFolder); }
        }
        /// <summary>
        /// Gets the path of the company's data folder.
        /// </summary>
        public string CompanyFolderPath {
            get { return Path.Combine(directory, companyFolder); }
        }

        private void CreateFolders(bool allUsers) {
            DirectoryInfo directoryInfo;
            DirectorySecurity directorySecurity;
            AccessRule rule;
            SecurityIdentifier securityIdentifier = new SecurityIdentifier
                (WellKnownSidType.BuiltinUsersSid, null);
            if (!Directory.Exists(CompanyFolderPath)) {
                directoryInfo = Directory.CreateDirectory(CompanyFolderPath);
                bool modified;
                directorySecurity = directoryInfo.GetAccessControl();
                rule = new FileSystemAccessRule(
                        securityIdentifier,
                        FileSystemRights.Write |
                        FileSystemRights.ReadAndExecute |
                        FileSystemRights.Modify,
                        AccessControlType.Allow);
                directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, out modified);
                directoryInfo.SetAccessControl(directorySecurity);
            }
            if (!Directory.Exists(ApplicationFolderPath)) {
                directoryInfo = Directory.CreateDirectory(ApplicationFolderPath);
                if (allUsers) {
                    bool modified;
                    directorySecurity = directoryInfo.GetAccessControl();
                    rule = new FileSystemAccessRule(
                        securityIdentifier,
                        FileSystemRights.Write |
                        FileSystemRights.ReadAndExecute |
                        FileSystemRights.Modify,
                        InheritanceFlags.ContainerInherit |
                        InheritanceFlags.ObjectInherit,
                        PropagationFlags.InheritOnly,
                        AccessControlType.Allow);
                    directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, out modified);
                    directoryInfo.SetAccessControl(directorySecurity);
                }
            }
        }
        /// <summary>
        /// Returns the path of the application's data folder.
        /// </summary>
        /// <returns>The path of the application's data folder.</returns>
        public override string ToString() {
            return ApplicationFolderPath;
        }
    }

    public static class FileHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static DateTime MinimumFileSystemDateTime = new DateTime(1601, 1, 1, 1, 1, 1);
        public static int GetFileLockedStatusTimeout { get; set; } = 500;
        public static int WaitFileGetUnlockedTimeout { get; set; } = 1000;

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
            if (waitAndRetry && currentLastWrittenDateTime <= FileHandler.MinimumFileSystemDateTime)
            {
                int retryCount = 5;
                do
                {
                    Thread.Sleep(25);
                    currentLastWrittenDateTime = File.GetLastWriteTime(fullFileName);
                } while (retryCount-- > 0 && currentLastWrittenDateTime <= FileHandler.MinimumFileSystemDateTime);
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

        #region DoesFileExist
        private struct FileExistsCache
        {
            public DateTime ValidUntil;
            public bool Exists;
        }

        private static Dictionary<string, FileExistsCache> fileExistsListCache = new Dictionary<string, FileExistsCache>();
        private static readonly Object fileExistsListCacheLock = new Object();
        public static bool DoesFileExists(string fullFileName, bool allowUseCache = true)
        {
            bool fileExists = false;

            lock (fileExistsListCacheLock)
            {
                if (allowUseCache) 
                {
                    if (fileExistsListCache.TryGetValue(fullFileName, out FileExistsCache fileExistsCacheCheck))
                    {
                        if (fileExistsCacheCheck.ValidUntil > DateTime.Now)
                        {
                            fileExists = fileExistsCacheCheck.Exists;
                        }
                        else
                        {
                            fileExistsListCache.Remove(fullFileName); //Timeout / create new
                            fileExists = File.Exists(fullFileName);
                        }
                    } else fileExists = File.Exists(fullFileName);
                }
                else fileExists = File.Exists(fullFileName);

                if (!fileExistsListCache.ContainsKey(fullFileName))
                {
                    FileExistsCache fileExistsCache = new FileExistsCache();
                    fileExistsCache.ValidUntil = DateTime.Now.AddMilliseconds(200);
                    fileExistsCache.Exists = fileExists;
                    fileExistsListCache.Add(fullFileName, fileExistsCache);
                }
                
            }
            return fileExists;
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
                fileStatus.FileExists = DoesFileExists(fullFileName);
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

                if (fileStatus.FileExists) fileStatus.LastWrittenDateTime = GetLastWriteTime(fullFileName, waitAndRetry: false);
                else fileStatus.LastWrittenDateTime = DateTime.MinValue;

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

            }
            catch (Exception ex)
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
                    status = status + "\r\nLast Write Time: " + fileStatus.LastWrittenDateTime.ToString();
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
                if (DoesFileExists(fullFileName)) fileAttributes = File.GetAttributes(fullFileName);
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
                if (DoesFileExists(fullFileName)) fileAttributes = File.GetAttributes(fullFileName);
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
        public static Dictionary<string, DateTime> CloundFileTouchedAndWhen = new Dictionary<string, DateTime>();
        public static object CloundFileTouchedAndWhenLock = new object();

        public static Dictionary<string, DateTime> CloundFileTouchedFailedAndWhen { get; set; } = new Dictionary<string, DateTime>();
        public static object CloundFileTouchedFailedAndWhenLock = new object();

        private static int TouchWaitDownloadingTimeoutFailed = 1000 * 60 * 2;

        #region ClearOfflineFileTouched
        public static void ClearOfflineFileTouched()
        {
            lock (CloundFileTouchedAndWhenLock)
            {
                CloundFileTouchedAndWhen.Clear();
            }
        }
        #endregion

        #region ClearOfflineFileTouchedFailed
        public static void ClearOfflineFileTouchedFailed()
        {
            lock (CloundFileTouchedFailedAndWhenLock)
            {
                CloundFileTouchedFailedAndWhen.Clear();
            }
        }
        #endregion

        #region RemoveOfflineFileTouched
        public static void RemoveOfflineFileTouched(string fullFileName)
        {
            lock (CloundFileTouchedAndWhenLock)
            {
                if (CloundFileTouchedAndWhen.ContainsKey(fullFileName)) CloundFileTouchedAndWhen.Remove(fullFileName);
            }
        }
        #endregion

        #region RemoveOfflineFileTouchedFailed
        public static void RemoveOfflineFileTouchedFailed(string fullFileName)
        {
            lock (CloundFileTouchedFailedAndWhenLock)
            {
                if (CloundFileTouchedFailedAndWhen.ContainsKey(fullFileName)) CloundFileTouchedFailedAndWhen.Remove(fullFileName);
            }
        }
        #endregion

        #region IsOfflineFileTouchedAndWithoutTimeout
        public static bool IsOfflineFileTouched(string fullFileName)
        {
            lock (CloundFileTouchedAndWhenLock)
            {
                return CloundFileTouchedAndWhen.ContainsKey(fullFileName);
            }
        }
        #endregion

        #region IsOfflineFileTouchedAndFailedWithoutTimedOut
        public static bool DidTouchedFileTimeoutDuringDownload(string fullFileName)
        {
            bool didTounchTimeout = false;
            lock (CloundFileTouchedFailedAndWhenLock)
            {
                if (CloundFileTouchedFailedAndWhen.ContainsKey(fullFileName))
                {
                    if ((DateTime.Now > CloundFileTouchedFailedAndWhen[fullFileName]))
                        didTounchTimeout = true;
                }
            }
            return didTounchTimeout;
        }
        #endregion

        #region Touch Offline File 
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
                lock (CloundFileTouchedFailedAndWhenLock)
                {
                    if (!CloundFileTouchedFailedAndWhen.ContainsKey(fullFileName)) CloundFileTouchedFailedAndWhen.Add(fullFileName, DateTime.Now);
                    else CloundFileTouchedFailedAndWhen[fullFileName] = DateTime.Now;
                }
            }
        }
        #endregion

        #region Touch Offline File To Get File Online
        public static void TouchOfflineFileToGetFileOnline(string fullFileName)
        {
            if (IsOfflineFileTouched(fullFileName)) return;

            #region Touch the file and log when
            FileStatus fileStatus = GetFileStatus(fullFileName, checkLockedStatus: false);
            if (fileStatus.IsInCloudOrVirtualOrOffline)
            {
                lock (CloundFileTouchedAndWhenLock)
                {
                    if (!CloundFileTouchedAndWhen.ContainsKey(fullFileName)) CloundFileTouchedAndWhen.Add(fullFileName, DateTime.Now);
                    if (!CloundFileTouchedFailedAndWhen.ContainsKey(fullFileName)) CloundFileTouchedFailedAndWhen.Add(fullFileName, DateTime.Now.AddMilliseconds(TouchWaitDownloadingTimeoutFailed));
                    else CloundFileTouchedFailedAndWhen[fullFileName] = DateTime.Now.AddMilliseconds(TouchWaitDownloadingTimeoutFailed);
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
            catch
            {
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
            }
            catch (Exception ex)
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


        #region CreateFolders
        private static void CreateFoldersWithUserAccessRights(string ApplicationFolderPath, bool allUsers = true) {
            DirectoryInfo directoryInfo;
            DirectorySecurity directorySecurity;
            AccessRule rule;
            SecurityIdentifier securityIdentifier = new SecurityIdentifier
                (WellKnownSidType.BuiltinUsersSid, null);
            
            if (!Directory.Exists(ApplicationFolderPath)) {
                directoryInfo = Directory.CreateDirectory(ApplicationFolderPath);
                if (allUsers) {
                    bool modified;
                    directorySecurity = directoryInfo.GetAccessControl();
                    rule = new FileSystemAccessRule(
                        securityIdentifier,
                        FileSystemRights.Write |
                        FileSystemRights.ReadAndExecute |
                        FileSystemRights.Modify,
                        InheritanceFlags.ContainerInherit |
                        InheritanceFlags.ObjectInherit,
                        PropagationFlags.InheritOnly,
                        AccessControlType.Allow);
                    directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, out modified);
                    directoryInfo.SetAccessControl(directorySecurity);
                }
            }
        }
        #endregion

        #region SetLocalApplicationDataPath
        private static string applicationDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
        public static void SetLocalApplicationDataPath(string newLocalApplicationDataPath) 
        {
            if (Directory.Exists(newLocalApplicationDataPath)) 
            {
                applicationDataPath = newLocalApplicationDataPath;
            }
            else 
            {
                applicationDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            }
        }
        #endregion

        #region GetLocalApplicationDataPath
        public static string GetLocalApplicationDataPath(string tempfilename, bool deleteOldTempFile = false)
        {
            //Create directory, filename and remove old arg file

            string exiftoolArgFileDirecory = applicationDataPath;
            if (!Directory.Exists(exiftoolArgFileDirecory)) CreateFoldersWithUserAccessRights(exiftoolArgFileDirecory);

            string exiftoolArgFileFullPath = Path.Combine(exiftoolArgFileDirecory, tempfilename);
            try
            {
                if (deleteOldTempFile && File.Exists(exiftoolArgFileFullPath)) FileHandler.Delete(exiftoolArgFileFullPath, false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            return exiftoolArgFileFullPath;
        }

        //public static string GetLocalApplicationDataTempPath(string tempfilename)
        //{
        //    //Create directory, filename and remove old arg file

        //    string tempFileDirecory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
        //    if (!Directory.Exists(tempFileDirecory)) Directory.CreateDirectory(tempFileDirecory);
        //    string tempFileFullPath = Path.Combine(tempFileDirecory, tempfilename);
            
        //    return tempFileFullPath;
        //}
        #endregion

        #region CombineApplicationPathWithFilename
        public static string CombineApplicationPathWithFilename(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
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

        #region Delete
        public static void Delete(string path, bool moveToRecycleBin)
        {
            if (moveToRecycleBin) FileOperationAPIWrapper.MoveToRecycleBin(path);
            else File.Delete(path);
        }

        public static void DirectoryDelete(string path, bool moveToRecycleBin)
        {
            if (moveToRecycleBin) FileOperationAPIWrapper.MoveToRecycleBin(path);
            else Directory.Delete(path);
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }
        #endregion

        #region FilenameMatchesPattern
        /// <summary>
        ///   Checks if name matches pattern with '?' and '*' wildcards.
        /// </summary>
        /// <param name="filename">
        ///   Name to match.
        /// </param>
        /// <param name="pattern">
        ///   Pattern to match to.
        /// </param>
        /// <returns>
        ///   <c>true</c> if name matches pattern, otherwise <c>false</c>.
        /// </returns>
        public static bool FilenameMatchesPattern(string filename, string pattern)
        {
            // prepare the pattern to the form appropriate for Regex class
            StringBuilder sb = new StringBuilder(pattern);
            // remove superflous occurences of  "?*" and "*?"
            while (sb.ToString().IndexOf("?*") != -1)
            {
                sb.Replace("?*", "*");
            }
            while (sb.ToString().IndexOf("*?") != -1)
            {
                sb.Replace("*?", "*");
            }
            // remove superflous occurences of asterisk '*'
            while (sb.ToString().IndexOf("**") != -1)
            {
                sb.Replace("**", "*");
            }
            // if only asterisk '*' is left, the mask is ".*"
            if (sb.ToString().Equals("*"))
                pattern = ".*";
            else
            {
                // replace '.' with "\."
                sb.Replace(".", "\\.");
                // replaces all occurrences of '*' with ".*" 
                sb.Replace("*", ".*");
                // replaces all occurrences of '?' with '.*' 
                sb.Replace("?", ".");
                // add "\b" to the beginning and end of the pattern
                sb.Insert(0, "\\b");
                sb.Append("\\b");
                pattern = sb.ToString();
            }
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(filename);
        }
        #endregion
    }
}
