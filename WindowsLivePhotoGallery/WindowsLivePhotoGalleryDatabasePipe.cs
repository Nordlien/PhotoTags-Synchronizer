using NamedPipeWrapper;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using MetadataLibrary;
using PipeMessage;
using System.Windows.Forms;

namespace WindowsLivePhotoGallery
{
    public class WindowsLivePhotoGalleryDatabasePipe : ImetadataReader
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private NamedPipeClient<PipeMessageCommand> pipeClient; // = new NamedPipeClient<PipeMessageCommand>("SqlCeDatabase32Pipe");
        private MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGalleryPipe;
        private Metadata metadataReadFromPipe;

        private Process process;

        private static readonly Object _ErrorHandlingLock = new Object();
        private static readonly Object _PipeClientLock = new Object();
        
        private string globalErrorMessageHandler = "";
        
        private bool consoleProcessErrorOccurred = false;
        private bool consoleProcessDisconnected = false;
        private bool consoleProcessConnecting = false;

        private bool pipeClientProcessErrorOccurred = false;
        private bool pipeClientDiconnected = false;

        private readonly AutoResetEvent consoleProcessWaitEventStarted = new AutoResetEvent(false);
        private readonly AutoResetEvent consoleProcessWaitEventServerExited = new AutoResetEvent(false);
        private readonly AutoResetEvent pipeClientEventWaitPipeCommandReturn = new AutoResetEvent(false);


        #region Constructor
        public WindowsLivePhotoGalleryDatabasePipe()
        {
            //ReconnectPipe();
        }
        #endregion

        #region ConnectDatabase
        public void ConnectDatabase(MetadataDatabaseCache metadataDatabaseCache)
        {
            this.databaseAndCacheMetadataWindowsLivePhotoGalleryPipe = metadataDatabaseCache;
        }
        #endregion

        #region PipeClient

        #region PipeClient Error
        private void PipeClient_Error(Exception exception)
        {
            lock (_ErrorHandlingLock)
            {
                pipeClientProcessErrorOccurred = true;
                globalErrorMessageHandler += (globalErrorMessageHandler == "" ? "" : "\r\n") + "[Windows Live Photo Gallery | Pipe Client] Error: " + exception.Message;
            }
            Logger.Error(globalErrorMessageHandler);
            pipeClientEventWaitPipeCommandReturn.Set();
        }
        #endregion

        #region PipeClient_ServerMessage
        private void PipeClient_ServerMessage(NamedPipeConnection<PipeMessageCommand, PipeMessageCommand> connection, PipeMessageCommand message)
        {
            Logger.Trace("[Windows Live Photo Gallery | Pipe Client] ServerMessage - Id: {" + connection.Id + "}, " + 
                "Metadata: " + (message.Metadata == null ? "NULL" : "Found") + " " +
                "Command: " + message.Command + " " + 
                "Message: " + message.Message );

            metadataReadFromPipe = message.Metadata;
            if (message.Command == "File") pipeClientEventWaitPipeCommandReturn.Set();
        }
        #endregion

        #region PipeClient_Start_WithoutConnect()
        public void PipeClient_Start_WithoutConnect()
        {
            Logger.Trace("[Windows Live Photo Gallery | Pipe Client] ReconnectPipe");
            try
            {
                if (pipeClient == null)
                {
                    lock (_PipeClientLock)
                    {
                        pipeClient = new NamedPipeClient<PipeMessageCommand>("SqlCeDatabase32Pipe");
                        pipeClient.ServerMessage -= PipeClient_ServerMessage;
                        pipeClient.ServerMessage += PipeClient_ServerMessage;
                        pipeClient.Error -= PipeClient_Error;
                        pipeClient.Error += PipeClient_Error;
                        pipeClient.Disconnected -= PipeClient_Disconnected;
                        pipeClient.Disconnected += PipeClient_Disconnected;
                    }
                    
                    pipeClient.Start(TimeSpan.FromSeconds(30));
                    
                    lock (_ErrorHandlingLock)
                    {
                        pipeClientProcessErrorOccurred = false;
                        pipeClientDiconnected = false;
                    }
                }
            } 
            catch (Exception ex)
            {
                lock (_ErrorHandlingLock)
                {
                    pipeClientProcessErrorOccurred = true;
                    pipeClientDiconnected = false; 
                    globalErrorMessageHandler += (globalErrorMessageHandler == "" ? "" : "\r\n") + "[Windows Live Photo Gallery | Pipe Client] Reconnect error: " + ex.Message;
                }
                Logger.Error(globalErrorMessageHandler);
            }
        }
        #endregion

        #region PipeClient_Disconnected
        private void PipeClient_Disconnected(NamedPipeConnection<PipeMessageCommand, PipeMessageCommand> connection)
        {
            Logger.Trace("[Windows Live Photo Gallery | Pipe Client] Disconnected - Id: {" + connection.Id + "} Disconnect Pipe");
            lock (_PipeClientLock)
            {
                pipeClientDiconnected = true;
            }
        }
        #endregion

        #region PipeClient ForceDisconnectAfterError
        private void PipeClientForceDisconnectAfterError(ref bool _pipeClientProcessErrorOccurred, ref bool _pipeClientDiconnected, ref bool _consoleProcessDisconnected)
        {
            #region Error handling message
            if (_pipeClientProcessErrorOccurred || _pipeClientDiconnected || _consoleProcessDisconnected)
            {
                //Don't disconnect on "consoleProcessErrorOccurred" Console Errors, Error from console can be data error, database error, etc..
                try
                {
                    if (pipeClient != null)
                    {
                        Logger.Trace("[Windows Live Photo Gallery | Pipe Client] Force Disconnect After Error");
                        pipeClient.Stop();
                        pipeClient.WaitForDisconnection(5000);
                    }
                }
                finally
                {
                    lock (_PipeClientLock)
                    {
                        pipeClient = null;
                        pipeClientDiconnected = true;
                        Logger.Trace("[Windows Live Photo Gallery | Pipe Client] Force Disconnect After Error - null");
                    }                    
                }
            }
            #endregion
        }
        #endregion

        #endregion 

        #region ConsoleProcess

        #region ConsoleProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        private void ConsoleProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                lock (_ErrorHandlingLock)
                {
                    consoleProcessErrorOccurred = true;
                    globalErrorMessageHandler += (globalErrorMessageHandler == "" ? "" : "\r\n") + "[Windows Live Photo Gallery | Console Process] Error: " + e.Data;
                }
                Logger.Error(globalErrorMessageHandler);                
            }
            consoleProcessWaitEventStarted.Set(); 
        }
        #endregion

        #region ConsoleProcess_OutputDataReceived
        private void ConsoleProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.Trace("[Windows Live Photo Gallery | Console Process] Data received: " + e.Data);
            if (e.Data == "Server up and running...") consoleProcessWaitEventStarted.Set();
            else if (e.Data == "Server disconnecting...") 
            {
                lock (_ErrorHandlingLock) { consoleProcessDisconnected = true; }
                PipeClientForceDisconnectAfterError(ref pipeClientProcessErrorOccurred, ref pipeClientDiconnected, ref consoleProcessDisconnected);
                consoleProcessWaitEventServerExited.Set();
            }
        }
        #endregion

        #region ConsoleProcess_Exited
        private void ConsoleProcess_Exited(object sender, EventArgs e)
        {
            Logger.Trace("[Windows Live Photo Gallery | Console Process] Exited");
            lock (_ErrorHandlingLock) { consoleProcessDisconnected = true; }
            PipeClientForceDisconnectAfterError(ref pipeClientProcessErrorOccurred, ref pipeClientDiconnected, ref consoleProcessDisconnected);
            consoleProcessWaitEventServerExited.Set();
        }
        #endregion 

        #region ConsoleProcess - IsConsoleServerRunning
        private bool IsConsoleProcessRunning()
        {
            bool isConsoleServerRunning;
            #region Mutex avoid start twice
            Mutex mutex = new System.Threading.Mutex(false, "WindowsLivePhotoGalleryServer");
            try
            {
                //Allow only one server to run
                if (mutex.WaitOne(0, false)) isConsoleServerRunning = false;
                else isConsoleServerRunning = true;
            }
            finally
            {
                if (mutex != null) mutex.Close();
            }
            #endregion
            Logger.Trace("[Windows Live Photo Gallery | Console Process] IsConsoleServerRunning: " + isConsoleServerRunning);
            return isConsoleServerRunning;
        }
        #endregion 

        #region ConsoleProcess - Start
        private void ConsoleProcessStart()
        {
            bool isServerAlreadyRunning = IsConsoleProcessRunning();

            if (!isServerAlreadyRunning)
            {
                consoleProcessConnecting = true;
                string windowsLiveGalleryServerfileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pipe\\WindowsLivePhotoGalleryServer.exe");

                if (File.Exists(windowsLiveGalleryServerfileName))
                {
                    try
                    {
                        consoleProcessWaitEventStarted.Reset();

                        #region Create ProcessStartInfo
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = windowsLiveGalleryServerfileName,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = "",
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true
                        };

                        process = new Process { StartInfo = startInfo };
                        process.OutputDataReceived -= ConsoleProcess_OutputDataReceived;
                        process.OutputDataReceived += ConsoleProcess_OutputDataReceived;
                        process.ErrorDataReceived -= ConsoleProcess_ErrorDataReceived;
                        process.ErrorDataReceived += ConsoleProcess_ErrorDataReceived;
                        process.Exited -= ConsoleProcess_Exited;
                        process.Exited += ConsoleProcess_Exited;
                        #endregion

                        Stopwatch stopwatch = new Stopwatch();

                        #region process.Start()
                        stopwatch.Restart();
                        if (process.Start()) Logger.Trace("[Windows Live Photo Gallery | Console Process] Started. " + stopwatch.ElapsedMilliseconds + " ms.");
                        else Logger.Trace("[Windows Live Photo Gallery | Console Process] Existing process reused. " + stopwatch.ElapsedMilliseconds + " ms.");

                        lock (_ErrorHandlingLock)
                        {
                            consoleProcessDisconnected = false;
                            consoleProcessErrorOccurred = false;
                        }

                        process.BeginErrorReadLine();
                        process.BeginOutputReadLine();
                        #endregion

                        #region Wait hello
                        stopwatch.Restart();
                        if (!consoleProcessWaitEventStarted.WaitOne(10000))
                        {
                            lock (_ErrorHandlingLock)
                            {
                                consoleProcessDisconnected = false;
                                consoleProcessErrorOccurred = true;
                            }

                            globalErrorMessageHandler += (globalErrorMessageHandler == "" ? "" : "\r\n") + "[Windows Live Photo Gallery | Console Process] Wait console process 'hello' responed. " + stopwatch.ElapsedMilliseconds + " ms.";
                            Logger.Error(globalErrorMessageHandler);
                        }
                        else Logger.Trace("[Windows Live Photo Gallery | Console Process] Wait console process 'hello' responed. " + stopwatch.ElapsedMilliseconds + " ms.");
                        #endregion

                        #region PipeClient Connect
                        if (!consoleProcessErrorOccurred) //Wait Hello Timeout
                        {
                            stopwatch.Start();
                            pipeClient.WaitForConnection(5000);
                            Logger.Trace("[Windows Live Photo Gallery | Console Process] Waited for Pipe Client connection: " + stopwatch.ElapsedMilliseconds + "ms.");
                        }
                        #endregion 
                    }
                    catch (Exception e)
                    {
                        #region Error handling
                        lock (_ErrorHandlingLock)
                        {
                            consoleProcessErrorOccurred = true;
                            globalErrorMessageHandler += (globalErrorMessageHandler == "" ? "" : "\r\n") + "Faild starting the process " + windowsLiveGalleryServerfileName + ", with exception message: " + e.Message;
                        }
                        Logger.Error(globalErrorMessageHandler);
                        #endregion 
                    }
                }
                else
                {
                    #region Error handling
                    lock (_ErrorHandlingLock)
                    {
                        consoleProcessErrorOccurred = true;
                        globalErrorMessageHandler += (globalErrorMessageHandler == "" ? "" : "\r\n") + "File not found, can't start the process:" + windowsLiveGalleryServerfileName;
                    }
                    Logger.Error(globalErrorMessageHandler);
                    #endregion
                }
                consoleProcessConnecting = false;

            }
        }

        
        #endregion

        #endregion

        #region ShowErrorMessageAskForRetry
        private bool ShowErrorMessageAskForRetry(string errorMessage)
        {
            bool retryConnect = false;
            
            if (MessageBox.Show(
                "Error from Windows Live Photo Gallery background process:\r\n" + errorMessage + "\r\n\r\n" +
                "Do you want try reconnect?",
                "Error from Windows Live Photo Gallery background process",
                MessageBoxButtons.RetryCancel) == DialogResult.Retry)
            {
                retryConnect = true;
            }
            else
            {
                retryConnect = false;
            }

            return retryConnect;
        }
        #endregion

        #region ErrorMessageReset()
        private void ErrorMessageReset()
        {
            globalErrorMessageHandler = "";
            
            consoleProcessErrorOccurred = false;
            consoleProcessDisconnected = false;

            pipeClientProcessErrorOccurred = false;
            pipeClientDiconnected = false;
        }
        #endregion

        #region Metadata Read
        private bool errorHasOccurdDoNotReconnect = false; 
        public Metadata Read(MetadataBrokerType broker, string fullFilePath)
        {
            if (errorHasOccurdDoNotReconnect) return null;
            
            Stopwatch stopWatch = new Stopwatch();
            
            ErrorMessageReset();

            bool retryConnect = false;
            do
            {
                PipeClientForceDisconnectAfterError(ref pipeClientProcessErrorOccurred, ref pipeClientDiconnected, ref consoleProcessDisconnected);

                #region Start PipeClient, if "HANGING" Console process exist, kill that first
                if (pipeClient == null)
                {
                    //Chech if Console Procss running after crash
                    if (IsConsoleProcessRunning())
                    {
                        try
                        {   
                            consoleProcessWaitEventServerExited.Reset();

                            if (process != null) process.CloseMainWindow();
                            else Logger.Warn("[Windows Live Photo Gallery | Console Process] Console Server running, but lost process connection with it. Can't close it.");
                            consoleProcessWaitEventServerExited.WaitOne(2000);
                        }
                        catch (Exception ex)
                        {
                            Logger.Warn("[Windows Live Photo Gallery | Console Process] Close process was not preformed. " + ex.Message);
                        }
                    }

                    if (IsConsoleProcessRunning())
                    {
                        try
                        {
                            if (process != null) process.Kill();
                            else Logger.Warn("[Windows Live Photo Gallery | Console Process] Console Server running, but lost process connection with it. Can't kill it.");
                        }
                        catch (Exception ex)
                        {
                            Logger.Warn("[Windows Live Photo Gallery | Console Process] Kill process was not preformed. " + ex.Message);
                        }
                    }

                    PipeClient_Start_WithoutConnect();
                }
                #endregion

                if (!pipeClientDiconnected || !pipeClientProcessErrorOccurred || pipeClient != null) ConsoleProcessStart();

                #region PipeMessageCommand
                if (!pipeClientProcessErrorOccurred && !consoleProcessErrorOccurred)
                {
                    metadataReadFromPipe = null;

                    stopWatch.Restart();
                    PipeMessageCommand pipeMessageCommand = new PipeMessageCommand();
                    pipeMessageCommand.FullFileName = fullFilePath;
                    pipeMessageCommand.Command = "File";
                    pipeMessageCommand.Message = "File:" + fullFilePath;

                    pipeClientEventWaitPipeCommandReturn.Reset(); //Clear in case of timeout
                    pipeClient.PushMessage(pipeMessageCommand);
                    Logger.Trace("[Windows Live Photo Gallery | Pipe Client] Push message: Request File {0}ms...", stopWatch.ElapsedMilliseconds);

                    stopWatch.Restart();

                    #region Retry timeout
                    bool retryWait;
                    do
                    {
                        retryWait = false;
                        if (pipeClientEventWaitPipeCommandReturn.WaitOne(20000))
                        {
                            Logger.Trace("[Windows Live Photo Gallery | Console Process] Push message: Wait answer {0}ms...", stopWatch.ElapsedMilliseconds);
                            return metadataReadFromPipe;
                        }
                        else
                        {
                            Logger.Trace("[Windows Live Photo Gallery | Console Process] Push message: Wait answer failed {0}ms...", stopWatch.ElapsedMilliseconds);
                            lock (_ErrorHandlingLock)
                            {
                                pipeClientProcessErrorOccurred = true;
                                globalErrorMessageHandler += (globalErrorMessageHandler == "" ? "" : "\r\n") + "[Windows Live Photo Gallery | Pipe Client] No response from server. Wait message timeout...";
                            }
                            Logger.Error(globalErrorMessageHandler);

                            switch (MessageBox.Show(
                                "Retry - and wait more.\r\n" +
                                "Ignor - and contine trying.\r\n" +
                                "Abort - and stop loading data from Windows Live Photo Gallery", 
                                "Waiting answer from server timeouted...", MessageBoxButtons.AbortRetryIgnore))
                            {
                                case DialogResult.Retry:
                                    retryWait = true;
                                    break;
                                case DialogResult.Ignore:
                                    retryWait = false;
                                    break;
                                case DialogResult.Abort:
                                    errorHasOccurdDoNotReconnect = true;
                                    return null;                                    
                            }

                        }
                    } while (retryWait);
                    #endregion 

                }
                #endregion

                #region MessageBox - retry
                bool errorOccured;
                string errorMessage;
                
                lock (_ErrorHandlingLock)
                {
                    errorOccured = (pipeClientProcessErrorOccurred || consoleProcessErrorOccurred || !string.IsNullOrWhiteSpace(globalErrorMessageHandler));
                    errorMessage = globalErrorMessageHandler;
                    globalErrorMessageHandler = "";
                }

                if (errorOccured)
                {
                    retryConnect = ShowErrorMessageAskForRetry(errorMessage);
                    
                } else retryConnect = false;
                #endregion 

            } while (retryConnect);
            return null;
        }
        #endregion 
    }
}
