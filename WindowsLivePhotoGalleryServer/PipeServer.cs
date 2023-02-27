using NamedPipeWrapper;
using System;
using MetadataLibrary;
using System.IO;
using System.Diagnostics;
using PipeMessage;
using System.Threading.Tasks;

namespace WindowsLivePhotoGalleryServer
{
    public class PipeServer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private bool KeepRunning = true;
        private Stopwatch stopwatchLastCommand = new Stopwatch();
        private WindowsLivePhotoGalleryDatabaseReader databaseWindowsLivePhotGallery;

        #region PipeServer
        public PipeServer(string pipeName)
        {

            stopwatchLastCommand.Start();
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            string destinationFile = Path.Combine(databasePath, "Pictures.pd6");
            string sourceFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows Live Photo Gallery\\Pictures.pd6");

            #region Create folder
            try
            {
                if (!Directory.Exists(databasePath)) Directory.CreateDirectory(databasePath);                
            }
            catch (Exception e)
            {
                WriteError("Failed create folder for storing copy of the database: " + e.Message);
                databaseWindowsLivePhotGallery = null;
                return;
            }
            #endregion 

            if (!File.Exists(sourceFile)) return;

            #region Copy Windows Live Photo Gallery database
            try
            {
                if (!File.Exists(destinationFile) || (File.GetLastWriteTime(sourceFile) >= File.GetLastWriteTime(destinationFile).AddSeconds(3600)))
                {
                    //Copy new only every hour
                    File.Copy(sourceFile, destinationFile, true);
                    WriteResponseLine("Copy the databasebase file");    //Write message back to client
                    WriteResponseLine("Copy from: " + sourceFile);      //Write message back to client
                    WriteResponseLine("Copy to:   " + destinationFile); //Write message back to client
                }
            }
            catch (IOException iox)
            {
                WriteError("Copy the database failed: " + iox.Message);
                return;
            }
            #endregion 

            #region Connect to database
            try
            {
                databaseWindowsLivePhotGallery = new WindowsLivePhotoGalleryDatabaseReader();
                databaseWindowsLivePhotGallery.Connect(destinationFile);
                WriteResponseLine("Windows Live Photo Gallery connected: " + destinationFile); //Write message back to client
            }
            catch (Exception e)
            {
                WriteError("Windows Live Photo Gallery connect to database failed: " + e.Message);
                databaseWindowsLivePhotGallery = null;
                return;
            }
            WriteResponseLine("Cache Database connected...");//Write message back to client
            #endregion

            #region Server Start
            var server = new NamedPipeServer<PipeMessageCommand>(pipeName);
            server.ClientConnected += OnClientConnected;
            server.ClientDisconnected += OnClientDisconnected;
            server.ClientMessage += OnClientMessage;
            server.Error += OnError;
            server.Start();

            WriteResponseLine("Server up and running...");
            WriteResponseLine("Waiting client connection...");
            #endregion 

            #region Server loop
            stopwatchLastCommand.Start();
            while (KeepRunning)
            {
                Task.Delay(10).Wait();
                if (stopwatchLastCommand.ElapsedMilliseconds > 3600000) 
                {
                    WriteResponseLine("Server didn't get any request, quiting...");
                    WriteResponseLine("Server disconnecting...");
                    KeepRunning = false;
                } 
            }
            #endregion 

            server.Stop();
        }
        #endregion

        #region OnClientConnected - Hello
        private void OnClientConnected(NamedPipeConnection<PipeMessageCommand, PipeMessageCommand> connection)
        {
            WriteResponseLine("Client {" + connection.Id +"} is now connected!");
            
            PipeMessageCommand pipeMessageCommand = new PipeMessageCommand();
            pipeMessageCommand.FullFileName = "";
            pipeMessageCommand.Command = "Hello";
            pipeMessageCommand.Message = "Hello";
            connection.PushMessage(pipeMessageCommand);
        }
        #endregion 

        #region OnClientDisconnected
        private void OnClientDisconnected(NamedPipeConnection<PipeMessageCommand, PipeMessageCommand> connection)
        {
            WriteResponseLine("Client {" + connection.Id + "} disconnected");
            KeepRunning = false;
        }
        #endregion 

        #region OnClientMessage
        private void OnClientMessage(NamedPipeConnection<PipeMessageCommand, PipeMessageCommand> connection, PipeMessageCommand message)
        {
            stopwatchLastCommand.Restart();

            if (message.Command == "Quit!") KeepRunning = false;
            if (message.Command.StartsWith("File"))
            {            
                string fullFilePath = message.FullFileName;
                WriteResponseLine("Client {" + connection.Id + "} Proccessing file: {" + fullFilePath + "}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Metadata metadata = databaseWindowsLivePhotGallery.Read(MetadataBrokerType.WindowsLivePhotoGallery, fullFilePath);

                WriteResponseLine(
                    "Client ID: {" + connection.Id + "}, " +
                    (metadata == null ? "File not found" : "Data found") + " " +
                    "File: {" + fullFilePath + "}, " +
                    "Time Elapsed Milliseconds: {" + stopwatch.ElapsedMilliseconds.ToString() + "}" );

                PipeMessageCommand pipeMessageCommand = new PipeMessageCommand();
                pipeMessageCommand.FullFileName = fullFilePath;
                pipeMessageCommand.Command = "File";
                pipeMessageCommand.Message = "File:" + fullFilePath;
                pipeMessageCommand.Metadata = metadata;
                connection.PushMessage(pipeMessageCommand);
            } 
            else 
            {
                WriteResponseLine("Unknown command: " + message.Command);
            }
        }
        #endregion 

        #region OnError
        private void OnError(Exception exception)
        {
            WriteError("ERROR: " + exception.Message);
            KeepRunning = false;
        }
        #endregion

        #region WriteResponseLine
        private void WriteResponseLine(string message)
        {
            Console.WriteLine(message);
        }
        #endregion

        #region WriteError
        private void WriteError(string message)
        {
            Logger.Error(message);
            Console.Error.WriteLine(message); //Write Error message back to client
        }
        #endregion
    }
}
