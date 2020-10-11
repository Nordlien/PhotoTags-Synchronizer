using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;


namespace WindowsLivePhotoGalleryServer
{

    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Mutex mutex = new System.Threading.Mutex(false, "WindowsLivePhotoGalleryServer");
            try
            {
                if (mutex.WaitOne(0, false)) //Allow only one server to run
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    Logger.Info("WindowsLivePhotoGalleryServer " + fvi.ProductVersion);
                    Logger.Info("PhotoTags Synchronizer 32bit background process...");
                    Logger.Info("---------------------------------------------");
                    _ = new PipeServer("SqlCeDatabase32Pipe");
                }
            }
            finally
            {
                if (mutex != null)
                {
                    mutex.Close();
                }
            }


        }
    }
}

