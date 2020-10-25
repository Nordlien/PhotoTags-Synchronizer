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

        private NamedPipeClient<PipeMessageCommand> client = new NamedPipeClient<PipeMessageCommand>("SqlCeDatabase32Pipe");
        private MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGalleryPipe;
        private Process process;
        private string errorMessage = "";
        private bool errorOccurred = false;
        private readonly AutoResetEvent waitEventServerStarted = new AutoResetEvent(false);
        private readonly AutoResetEvent waitEventPipeCommandReturn = new AutoResetEvent(false);

        public void Connect(MetadataDatabaseCache metadataDatabaseCache)
        {
            this.databaseAndCacheMetadataWindowsLivePhotoGalleryPipe = metadataDatabaseCache;
        }

        public WindowsLivePhotoGalleryDatabasePipe ()
        {
            client.ServerMessage += Client_ServerMessage;
            client.Error += Client_Error;
            client.Start(); 
        }

        private void Client_Error(Exception exception)
        {
            throw new NotImplementedException();
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.Error("MWLG error:  " + e.Data);
            errorMessage = e.Data;
            errorOccurred = true;
            waitEventServerStarted.Set();            
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.Info("MWLG output: " + e.Data);
            if (e.Data == "Server up and running...")
            {
                errorOccurred = false;
                waitEventServerStarted.Set();
            }
        }


        private Metadata metadataReadFromPipe;
        private void Client_ServerMessage(NamedPipeConnection<PipeMessageCommand, PipeMessageCommand> connection, PipeMessageCommand message)
        {
            Logger.Trace("Microsoft Windows Live Gallery Pipe message" + message.Message + " Command:" + message.Command);
            if (message.Metadata == null) 
                Logger.Trace("Microsoft Windows Live Gallery Pipe: metadata not found");
            else 
                Logger.Trace("Microsoft Windows Live Gallery Pipe found: " + Path.Combine(message.Metadata.FileFullPath, message.Metadata.FileName));
            metadataReadFromPipe = message.Metadata;
            if (message.Command == "File") 
                waitEventPipeCommandReturn.Set();
        }

        private bool messageShown = false;

        private void StartServer()
        {
            bool isServerRunning = true;
            Mutex mutex = new System.Threading.Mutex(false, "WindowsLivePhotoGalleryServer");
            try
            {
                //Allow only one server to run
                if (mutex.WaitOne(0, false)) isServerRunning = false;
                else isServerRunning = true;
            }
            finally
            {
                if (mutex != null) mutex.Close();
            }

            if (!isServerRunning)
            {
                string windowsLiveGalleryServerfileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pipe\\WindowsLivePhotoGalleryServer.exe");
                
                if (File.Exists(windowsLiveGalleryServerfileName))
                {                
                    try
                    {
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

                        process = new Process
                        {
                            StartInfo = startInfo
                        };
                        process.OutputDataReceived += Process_OutputDataReceived;
                        process.ErrorDataReceived += Process_ErrorDataReceived;
                        
                        if (process.Start())
                        {
                            process.BeginErrorReadLine();
                            process.BeginOutputReadLine();

                            if (!waitEventServerStarted.WaitOne(60000))
                            {
                                errorOccurred = true;
                                errorMessage += "Waiting for server to start timeouted" + "\r\n";
                                Logger.Error(errorMessage);
                            }

                            if (!errorOccurred) client.WaitForConnection(60000);
                        }
                        else
                        {
                            errorOccurred = true;
                            errorMessage += "Can't start the process, process staring failed, exception unknown:" + windowsLiveGalleryServerfileName + "\r\n";
                            Logger.Error(errorMessage);
                        }
                    } catch (Exception e)
                    {
                        errorOccurred = true;
                        errorMessage += "Faild starting the process, with exception message: " + e.Message + "\r\n";
                        Logger.Error(errorMessage);
                    }
                } 
                else
                {
                    errorOccurred = true;
                    errorMessage += "File not found, can't start the process:" + windowsLiveGalleryServerfileName + "\r\n";
                    Logger.Error(errorMessage);
                }

                if (errorOccurred)
                {
                    client = null;
                    if (!messageShown)
                    {
                        
                        MessageBox.Show("Error from WindowsLiveGallery background process:\r\n" + errorMessage);
                        messageShown = true;
                        errorMessage = "";
                    }
                }
            }
        }

        
        public Metadata Read(MetadataBrokerTypes broker, string fullFilePath)
        {
            Stopwatch stopWatch = new Stopwatch();
            
            stopWatch.Restart();
            StartServer();


            if (client != null)
            {
                metadataReadFromPipe = null;

                PipeMessageCommand pipeMessageCommand = new PipeMessageCommand();
                pipeMessageCommand.FullFileName = fullFilePath;
                pipeMessageCommand.Command = "File";
                pipeMessageCommand.Message = "File:" + fullFilePath;

                waitEventPipeCommandReturn.Reset(); //Clear in case of timeout
                client.PushMessage(pipeMessageCommand);
                stopWatch.Stop();
                if (stopWatch.ElapsedMilliseconds > 10) Logger.Info("Push file request {0}ms...", stopWatch.ElapsedMilliseconds);

                stopWatch.Restart();
                if (waitEventPipeCommandReturn.WaitOne(300000)) 
                    return metadataReadFromPipe;
                else 
                    Logger.Info("Wait message timeout... {0}", stopWatch.ElapsedMilliseconds);
            }
            return null;
        }

    }
}
