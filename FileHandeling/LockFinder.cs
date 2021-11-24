using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

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

}
