using PhotoTagsCommonComponets;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PhotoTagsCommonComponets;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ApplicationAssociations
{
    //https://stackoverflow.com/questions/12925748/iapplicationactivationmanageractivateapplication-in-c
    //ApplicationActivation.ActivateApplication("Microsoft.Windows.Photos_8wekyb3d8bbwe!App");            
    //ApplicationActivation.ActivateForFile("Microsoft.Windows.Photos_8wekyb3d8bbwe!App", "c:\\IMG_.jpg");


    #region enum ActivateOptions
    public enum ActivateOptions
    {
        None = 0x00000000,  // No flags set
        DesignMode = 0x00000001,  // The application is being activated for design mode, and thus will not be able to
                                  // to create an immersive window. Window creation must be done by design tools which
                                  // load the necessary components by communicating with a designer-specified service on
                                  // the site chain established on the activation manager.  The splash screen normally
                                  // shown when an application is activated will also not appear.  Most activations
                                  // will not use this flag.
        NoErrorUI = 0x00000002,  // Do not show an error dialog if the app fails to activate.                                
        NoSplashScreen = 0x00000004,  // Do not show the splash screen when activating the app.
    }
    #endregion

    #region enum CommonHRESULTValues
    public enum CommonHRESULTValues : uint
    {
        S_OK = 0x00000000, //Operation successful
        E_ABORT = 0x80004004, //Operation aborted
        E_ACCESSDENIED = 0x80070005, //General access denied error
        E_FAIL = 0x80004005, //Unspecified failure
        E_HANDLE = 0x80070006, //Handle that is not valid	
        E_INVALIDARG = 0x80070057, //One or more arguments are not valid	
        E_NOINTERFACE = 0x80004002, //No such interface supported 
        E_NOTIMPL = 0x80004001, //Not implemented	
        E_OUTOFMEMORY = 0x8007000E, //Failed to allocate necessary memory 
        E_POINTER = 0x80004003, //Pointer that is not valid
        E_UNEXPECTED = 0x8000FFFF //Unexpected failure	
    }
    #endregion

    #region interface IApplicationActivationManager
    [ComImport, Guid("2e941141-7f97-4756-ba1d-9decde894a3d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IApplicationActivationManager
    {
        // Activates the specified immersive application for the "Launch" contract, passing the provided arguments
        // string into the application.  Callers can obtain the process Id of the application instance fulfilling this contract.
        uint ActivateApplication([In] String appUserModelId, [In] String arguments, [In] ActivateOptions options, [Out] out UInt32 processId);
        uint ActivateForFile([In] String appUserModelId, [In][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] /*IShellItemArray* */ IShellItemArray itemArray, [In] String verb, [Out] out UInt32 processId);
        uint ActivateForProtocol([In] String appUserModelId, [In] IntPtr /* IShellItemArray* */itemArray, [Out] out UInt32 processId);
    }
    #endregion

    #region interface IShellItem
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    interface IShellItem
    {
    }
    #endregion

    #region interface IShellItemArray
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("b63ea76d-1f85-456f-a19c-48159efa858b")]
    interface IShellItemArray
    {
    }
    #endregion 

    #region class ApplicationActivationManager
    [ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")]//Application Activation Manager
    internal class ApplicationActivationManager : IApplicationActivationManager
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)/*, PreserveSig*/]
        public extern uint ActivateApplication([In] String appUserModelId, [In] String arguments, [In] ActivateOptions options, [Out] out UInt32 processId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public extern uint ActivateForFile([In] String appUserModelId, [In][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)]  /*IShellItemArray* */ IShellItemArray itemArray, [In] String verb, [Out] out UInt32 processId);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public extern uint ActivateForProtocol([In] String appUserModelId, [In] IntPtr /* IShellItemArray* */itemArray, [Out] out UInt32 processId);
    }
    #endregion 

    public class ApplicationActivation
    {
        #region DllImport
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void SHCreateItemFromParsingName(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            [In] IntPtr pbc,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid iIdIShellItem,
            [Out][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out IShellItem iShellItem);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void SHCreateShellItemArrayFromShellItem(
            [In][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] IShellItem psi,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid iIdIShellItem,
            [Out][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out IShellItemArray iShellItemArray);
        #endregion

        #region GetShell Item, ItemArray
        private static IShellItemArray GetShellItemArray(string sourceFile)
        {
            IShellItem item = GetShellItem(sourceFile);
            IShellItemArray array = GetShellItemArray(item);

            return array;
        }

        private static IShellItem GetShellItem(string sourceFile)
        {
            IShellItem iShellItem = null;
            Guid iIdIShellItem = new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe");
            SHCreateItemFromParsingName(sourceFile, IntPtr.Zero, iIdIShellItem, out iShellItem);

            return iShellItem;
        }

        private static IShellItemArray GetShellItemArray(IShellItem shellItem)
        {
            IShellItemArray iShellItemArray = null;
            Guid iIdIShellItemArray = new Guid("b63ea76d-1f85-456f-a19c-48159efa858b");
            SHCreateShellItemArrayFromShellItem(shellItem, iIdIShellItemArray, out iShellItemArray);

            return iShellItemArray;
        }
        #endregion 

        #region ActivateApplication(string appUserModelId, bool waitForExit = false)
        public static void ActivateApplication(string appUserModelId, bool waitForExit = false)
        {
            ApplicationActivationManager appActiveManager = new ApplicationActivationManager();            
            uint pid;
            CommonHRESULTValues result = (CommonHRESULTValues)appActiveManager.ActivateApplication(appUserModelId, null, ActivateOptions.None, out pid);

            if (result == CommonHRESULTValues.S_OK)
            {
                if (waitForExit)
                {
                    Process process = Process.GetProcessById((int)pid);
                    process.WaitForExit();
                }
            }
            else
            {
                throw new Exception("ActivateForFile failed error code: " + result);
            }
        }
        #endregion

        #region ActivateForFile(string appUserModelId, string filePullPath, string verb, bool waitForExit = false)
        public static void ActivateForFile(string appUserModelId, string filePullPath, string verb, bool waitForExit = false)
        {
            ApplicationActivationManager appActiveManager = new ApplicationActivationManager();//Class not registered
            IShellItemArray array = GetShellItemArray(filePullPath);

            uint pid;
            CommonHRESULTValues result = (CommonHRESULTValues)appActiveManager.ActivateForFile(appUserModelId, array, verb, out pid);
            if (result == CommonHRESULTValues.S_OK)
            {
                if (waitForExit)
                {
                    Process process = Process.GetProcessById((int)pid);
                    process.WaitForExit();
                }
            } else
            {
                throw new Exception("ActivateForFile failed error code: " + result);
            }
        }
        #endregion

        #region SplitCommandAndAgrument(string commandWithArguments, out string command, out string arguments)
        public static void SplitCommandAndAgrument(string commandWithArguments, out string command, out string arguments)
        {

            if (commandWithArguments.StartsWith("\""))
            {
                int commandEndIndex = commandWithArguments.IndexOf('\"', 1);
                if (commandEndIndex < 0)
                {
                    command = commandWithArguments;
                    arguments = "";
                }
                else
                {
                    command = commandWithArguments.Substring(1, commandEndIndex - 1);
                    int argumentStartIndex = commandEndIndex + 2;
                    arguments = commandWithArguments.Substring(argumentStartIndex, commandWithArguments.Length - argumentStartIndex);
                }
            }
            else
            {
                int commandEndIndex = commandWithArguments.IndexOf(' ', 0);
                if (commandEndIndex < 0)
                {
                    command = commandWithArguments;
                    arguments = "";
                }
                else
                {
                    command = commandWithArguments.Substring(0, commandEndIndex);
                    int argumentStartIndex = commandEndIndex + 1;
                    arguments = commandWithArguments.Substring(argumentStartIndex, commandWithArguments.Length - argumentStartIndex);
                }
            }
        }
        #endregion 

        #region ProcessRun(string commandWitharguments, string appUserModelId, string fileFullPath,  string verb, bool waitForExit)
        public static void ProcessRun(string commandWitharguments, string appUserModelId, string fileFullPath,  string verb, bool waitForExit)
        {
            if (!string.IsNullOrWhiteSpace(appUserModelId) && appUserModelId != "Chrome") ActivateForFile(appUserModelId, fileFullPath, verb, waitForExit);
            else
            {
                SplitCommandAndAgrument(commandWitharguments, out string command, out string arguments);
                if (string.IsNullOrWhiteSpace(arguments))
                {
                    arguments = "\"" + fileFullPath +"\"";
                } else
                {
                    arguments = arguments.Replace("--single-argument %1", "\"" + fileFullPath + "\""); //Chrome hack
                    arguments = arguments.Replace("\"%L\"", "\"" + fileFullPath + "\"");
                    arguments = arguments.Replace("%L", "\"" + fileFullPath + "\"");
                    arguments = arguments.Replace("\"%1\"", "\"" + fileFullPath + "\"");
                    arguments = arguments.Replace("%1", "\"" + fileFullPath + "\"");
                    
                    //Remove extra agruments
                    arguments = arguments.Replace("\"%2\"", "");
                    arguments = arguments.Replace("%2", "");                    
                    arguments = arguments.Replace("\"%3\"", "");
                    arguments = arguments.Replace("%3", "");
                    arguments = arguments.Replace("\"%4\"", "");
                    arguments = arguments.Replace("%4", "");
                    arguments = arguments.Replace("\"%5\"", "");
                    arguments = arguments.Replace("%5", "");
                    arguments = arguments.Replace("\"%6\"", "");
                    arguments = arguments.Replace("%6", "");
                    arguments = arguments.Replace("\"%7\"", "");
                    arguments = arguments.Replace("%7", "");
                    arguments = arguments.Replace("\"%8\"", "");
                    arguments = arguments.Replace("%8", "");
                    arguments = arguments.Replace("\"%9\"", "");
                    arguments = arguments.Replace("%9", "");
                    arguments = arguments.Replace("\"%10\"", "");
                    arguments = arguments.Replace("%10", "");
                    arguments = arguments.Replace("\"%11\"", "");
                    arguments = arguments.Replace("%11", "");
                    arguments = arguments.Replace("\"%12\"", "");
                    arguments = arguments.Replace("%12", "");
                    arguments = arguments.Replace("\"%13\"", "");
                    arguments = arguments.Replace("%13", "");
                    arguments = arguments.Replace("\"%14\"", "");
                    arguments = arguments.Replace("%14", "");
                    arguments = arguments.Replace("\"%15\"", "");
                    arguments = arguments.Replace("%15", "");
                }
                ProcessRun(command, arguments, waitForExit);
            }
        }
        #endregion

        #region ProcessRun(string command, string arguments, bool waitForExit)
        public static void ProcessRun(string command, string arguments, bool waitForExit)
        {
            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    RedirectStandardInput = false,
                    CreateNoWindow = false
                }
            })
            {
                bool result = process.Start();
                if (waitForExit)
                {
                    process.WaitForExit();
                    process.Close();
                    process.Dispose();
                }
            }
        }
        #endregion

        #region ProcessRun(string commandWithArguments, bool waitForExit)
        public static void ProcessRun(string commandWithArguments, bool waitForExit)
        {
            SplitCommandAndAgrument(commandWithArguments, out string command, out string arguments);
            ProcessRun(command, arguments, waitForExit);
        }
        #endregion

        #region ProcessRun(FormTerminalWindow formTerminalWindow, string commandWithArguments, bool waitForExit)
        private static PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow2 = null;
        private static bool printTerminalWindowCommand = false;
        private static string processTerminalWindowCommand = null; 
        public static void ProcessRun(FormTerminalWindow formTerminalWindow, string commandWithArguments, bool waitForExit)
        {
            SplitCommandAndAgrument(commandWithArguments, out string command, out string arguments);
            
            formTerminalWindow2 = formTerminalWindow;
            processTerminalWindowCommand = commandWithArguments;
            printTerminalWindowCommand = true;

            Process runProcess = new Process();
            formTerminalWindow.SetProcssToFollow(runProcess);
            

            //build.StartInfo.WorkingDirectory = @"dir";
            runProcess.StartInfo.Arguments = arguments;
            runProcess.StartInfo.FileName = command;
            

            runProcess.StartInfo.UseShellExecute = false;
            runProcess.StartInfo.RedirectStandardOutput = true;
            runProcess.StartInfo.RedirectStandardError = true;
            runProcess.StartInfo.CreateNoWindow = true;
            runProcess.ErrorDataReceived += RunProcess_ErrorDataReceived;
            runProcess.OutputDataReceived += RunProcess_OutputDataReceived;
            runProcess.Exited += RunProcess_Exited;
            
            runProcess.EnableRaisingEvents = true;

            runProcess.Start();
            
            
            runProcess.BeginOutputReadLine();
            runProcess.BeginErrorReadLine();
            runProcess.WaitForExit();
            while (!runProcess.HasExited) Thread.Sleep(100);
            
            formTerminalWindow2 = formTerminalWindow;
        }

 

        private static void RunProcess_Exited(object sender, EventArgs e)
        {
            if (formTerminalWindow2 != null && printTerminalWindowCommand) formTerminalWindow2.LogWarning(processTerminalWindowCommand + "\r\n");
            printTerminalWindowCommand = false;

            
        }

        private static void RunProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (formTerminalWindow2 != null  && printTerminalWindowCommand) formTerminalWindow2.LogWarning(processTerminalWindowCommand + "\r\n");
            printTerminalWindowCommand = false;
            if (e.Data != null && formTerminalWindow2 != null) formTerminalWindow2.LogInfo(e.Data + "\r\n");                
            //Process runProcess = (Process)sender;
            //if (formTerminalWindow2 != null && runProcess.HasExited) formTerminalWindow2.LogWarning((processTerminalWindowCommand == null ? "" : "Command ended: " + processTerminalWindowCommand + "\r\n") + "Exit code: " + +runProcess.ExitCode + "\r\n");
        }

        private static void RunProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (formTerminalWindow2 != null && printTerminalWindowCommand) formTerminalWindow2.LogWarning(processTerminalWindowCommand + "\r\n");
            printTerminalWindowCommand = false;
            if (e.Data != null && formTerminalWindow2 != null) formTerminalWindow2.LogError(e.Data + "\r\n");
            //Process runProcess = (Process)sender;
            //if (formTerminalWindow2 != null && runProcess.HasExited) formTerminalWindow2.LogWarning((processTerminalWindowCommand == null ? "" : "Command ended: " + processTerminalWindowCommand + "\r\n") + "Exit code: " + +runProcess.ExitCode + "\r\n");
        }
        #endregion

        #region ProcessRunOpenFile(string fileFullPath)
        public static void ProcessRunOpenFile(string fileFullPath)
        {
            Process.Start(fileFullPath);
        }
        #endregion 

        #region ProcessRunEditFile(string fileFullPath)
        public static void ProcessRunEditFile(string fileFullPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileFullPath);
            if (startInfo.Verbs.Contains("edit"))
            {
                startInfo.Verb = "edit";
                Process.Start(startInfo);
            } throw new Exception("The file: " + fileFullPath + " has no application associated with 'edit'.");
        }
        #endregion

        #region ShowFileInExplorer(string fileFullPath)
        public static void ShowFileInExplorer(string fileFullPath)
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "explorer.exe";
            proc.Arguments = "/select, \"" + fileFullPath + "\"";
            Process.Start(proc);

        }
        #endregion

        #region ShowFolderInEplorer (string folder)
        public static void ShowFolderInEplorer (string folder)
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "explorer.exe";
            proc.Arguments = folder;
            Process.Start(proc);
        }
        #endregion 

        #region ShowOpenWithDialog(string path)
        public static void ShowOpenWithDialog(string path)
        {
            var args = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shell32.dll");
            args += ",OpenAs_RunDLL " + path;
            Process.Start("rundll32.exe", args);
        }
        #endregion 


    }

}
