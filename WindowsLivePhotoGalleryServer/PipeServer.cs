using NamedPipeWrapper;
using System;
using System.Threading;
using MetadataLibrary;
using System.IO;
using System.Diagnostics;
using PipeMessage;

namespace WindowsLivePhotoGalleryServer
{
    public class PipeServer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private bool KeepRunning = true;
        private WindowsLivePhotoGalleryDatabaseReader databaseWindowsLivePhotGallery;

        public PipeServer(string pipeName)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            string destinationFile = Path.Combine(databasePath, "Pictures.pd6");
            string sourceFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows Live Photo Gallery\\Pictures.pd6");

            try
            {
                if (!Directory.Exists(databasePath))
                {
                    Directory.CreateDirectory(databasePath);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Failed create direcotry for storing copy of the database: " + e.Message);
                databaseWindowsLivePhotGallery = null;
                return;
            }

            try
            {
                if (!File.Exists(destinationFile) || (File.GetLastWriteTime(sourceFile) >= File.GetLastWriteTime(destinationFile).AddSeconds(3600))) //Copy new only every hour
                    File.Copy(sourceFile, destinationFile, true);
                Logger.Info("Copy the databasebase file");
                Logger.Info("Copy from: " + sourceFile);
                Logger.Info("Copy to:   " + destinationFile);
            }
            catch (IOException iox)
            {
                Logger.Error("Copy the database failed: " + iox.Message);
                Console.Error.WriteLine("Copy the database failed: " + iox.Message); //Write Error message back to client
                return;
            }


            try
            {
                databaseWindowsLivePhotGallery = new WindowsLivePhotoGalleryDatabaseReader();
                databaseWindowsLivePhotGallery.Connect(destinationFile);
                Logger.Info("Windows Live Photo Gallery connected: " + destinationFile);
            }
            catch (Exception e)
            {
                Logger.Error("Windows Live Photo Gallery warning: " + e.Message);
                Console.Error.WriteLine("Windows Live Photo Gallery warning: " + e.Message); //Write Error message back to client
                databaseWindowsLivePhotGallery = null;
                return;
            }

            try
            {
                Logger.Info("Cache Database connected...");
            }
            catch (Exception e)
            {
                Logger.Error("Failed connect cache database: " + e.Message);
                Console.Error.WriteLine("Failed connect cache database: " + e.Message); //Write Error message back to client
                return;
            }

            var server = new NamedPipeServer<PipeMessageCommand>(pipeName);
            server.ClientConnected += OnClientConnected;
            server.ClientDisconnected += OnClientDisconnected;
            server.ClientMessage += OnClientMessage;
            server.Error += OnError;
            server.Start();
            
            Console.WriteLine("Server up and running..."); //NB: Write message back to client. Client wait for this signal... If change, also change in client code

            Logger.Info("Server up and running...");
            Logger.Info("Waiting client connection...");
            

            while (KeepRunning)
            {
                Thread.Sleep(10);
            }
            server.Stop();
        }

        private void OnClientConnected(NamedPipeConnection<PipeMessageCommand, PipeMessageCommand> connection)
        {
            Logger.Info("Client {0} is now connected!", connection.Id);
            
            PipeMessageCommand pipeMessageCommand = new PipeMessageCommand();
            pipeMessageCommand.FullFileName = "";
            pipeMessageCommand.Command = "Hello";
            pipeMessageCommand.Message = "Hello";
            connection.PushMessage(pipeMessageCommand);
            
        }

        private void OnClientDisconnected(NamedPipeConnection<PipeMessageCommand, PipeMessageCommand> connection)
        {
            Logger.Info("Client {0} disconnected", connection.Id);
            KeepRunning = false;
        }

        Object lockObject = new Object();
        private void OnClientMessage(NamedPipeConnection<PipeMessageCommand, PipeMessageCommand> connection, PipeMessageCommand message)
        {
            if (message.Command == "Quit!") KeepRunning = false;
            if (message.Command.StartsWith("File"))
            {
                string fullFilePath = message.FullFileName;
                Logger.Info("Client {0} Proccessing file: {1}", connection.Id, fullFilePath);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Metadata metadata = databaseWindowsLivePhotGallery.Read(MetadataBrokerType.WindowsLivePhotoGallery, fullFilePath);
                
                if (metadata == null)
                    Logger.Info("Client ID: {0}, Proccessing file: {1}, Time Elapsed Milliseconds: {2}, Metadata NOT found ", connection.Id, fullFilePath, stopwatch.ElapsedMilliseconds.ToString());
                else
                    Logger.Info("Client ID: {0}, Proccessing file: {1}, Time Elapsed Milliseconds: {2}, Metadata found ", connection.Id, fullFilePath, stopwatch.ElapsedMilliseconds.ToString());

                PipeMessageCommand pipeMessageCommand = new PipeMessageCommand();
                pipeMessageCommand.FullFileName = fullFilePath;
                pipeMessageCommand.Command = "File";
                pipeMessageCommand.Message = "File:" + fullFilePath;
                pipeMessageCommand.Metadata = metadata;
                connection.PushMessage(pipeMessageCommand);                
            } else
            {
                Logger.Info("Unknown command: " + message.Command);
            }
        }

        private void OnError(Exception exception)
        {
            Logger.Error("ERROR: {0}", exception.Message);
            KeepRunning = false;
        }
    }
}
