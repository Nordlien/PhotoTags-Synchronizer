using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ApplicationAssociations
{
    public class ApplicationInstalled
    {
        //[DllImport("ole32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        //static extern string ProgIDFromCLSID([In()] ref Guid clsid);

        public static SortedList<string, ApplicationData> ListInstalledApps2()
        {
            SortedList<string, ApplicationData> applicationDatas = new SortedList<string, ApplicationData>();

            List<string> progIds = ProtocolAssociationsRegistry.GetAllProgs();

            foreach (string progId in progIds)
            {
                var verbs = ProtocolAssociationsRegistry.GetVerbsByProgId(progId);

                if (verbs != null && verbs.Length > 0)
                {
                    ApplicationData applicationData = new ApplicationData();
                    applicationData.ProgId = progId;

                    AssocQuery assocQuery = new AssocQuery(progId);
                    applicationData.ApplicationId = assocQuery.AppId;
                    applicationData.Command = assocQuery.Command;
                    applicationData.FriendlyAppName = assocQuery.FriendlyAppName;
                    applicationData.AppIconReference = assocQuery.AppIconReference;

                    //AddApplicationData(extention, applicationData);

                    if (verbs.Length > 0)
                    {
                        foreach (string verb in verbs)
                        {
                            string command = "";
                            if (!string.IsNullOrEmpty(applicationData.Command))
                            {
                                command = AssocQuery.GetCommandProgId(progId, verb);
                            }
                            applicationData.AddVerb(verb, command);
                        }

                        if (!applicationDatas.Keys.Contains(applicationData.FriendlyAppName)) applicationDatas.Add(applicationData.FriendlyAppName, applicationData);
                    }
                }
            }
            return applicationDatas;
        }

        public static SortedList<string, ApplicationData> ListInstalledApps()
        {
            SortedList<string, ApplicationData> applicationDatas = new SortedList<string, ApplicationData>();

            // GUID taken from https://docs.microsoft.com/en-us/windows/win32/shell/knownfolderid
            var FOLDERID_AppsFolder = new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}");
            ShellObject appsFolder = (ShellObject)KnownFolderHelper.FromKnownFolderId(FOLDERID_AppsFolder);

            foreach (var app in (IKnownFolder)appsFolder)
            {
                ApplicationData applicationData = new ApplicationData();
                applicationData.FriendlyAppName = app.Name;      // The friendly app name
                applicationData.ApplicationId = app.ParsingName; // app.Properties.System.AppUserModel.ID
                applicationData.ProgId = AssocQuery.GetAppId(app.ParsingName, "open");
                applicationData.Icon = app.Thumbnail.Icon; // You can even get the Jumbo icon in one shot - ImageSource icon = app.Thumbnail.ExtraLargeBitmapSource;


                //var a = AssocQuery.GetAppId(app.ParsingName, "open");
                //var b = AssocQuery.GetCommandProgId(app.ParsingName, "open");
                //var c = AssocQuery.GetCommandExe(app.ParsingName, "open");

                //ProgIDFromCLSID:
                //Guid g = new Guid("88D969C0-F192-11D4-A65F-0040963251E5"); // MSXML 4.0 DOM.
                //string progId = ProgIDFromCLSID(ref g);

                //Debug.Assert(progId == "Msxml2.DOMDocument.4.0");


                //Microsoft.SkyDrive.Desktop
                AssocQuery assocQuery = new AssocQuery(applicationData.ApplicationId);
                var verbs = ProtocolAssociationsRegistry.GetVerbsByProgId(app.Name);
                
                foreach (string verb in verbs)
                {
                    string command = "";
                    //if (!string.IsNullOrEmpty(applicationData.Command)) { command = AssocQuery.GetCommandProgId(progId, verb); }
                    applicationData.AddVerb(verb, command);
                }

                if (!applicationDatas.Keys.Contains(applicationData.FriendlyAppName))
                    applicationDatas.Add(applicationData.FriendlyAppName, applicationData);

            }

            return applicationDatas;
        }
    }
}
