using NLog;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ApplicationAssociations
{


    public class AssocQuery
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public string DocType { get; set; }
        public string Command { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_COMMAND, DocType); } }
        public string Executable { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_EXECUTABLE, DocType); } }
        public string FriendlyDocName { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_FRIENDLYDOCNAME, DocType); } }   //"MP4 File"
        public string FriendlyAppName { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_FRIENDLYAPPNAME, DocType); } } //"Movies & TV"
        public string NoOpen { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_NOOPEN, DocType); } }
        public string ShellNewValue { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_SHELLNEWVALUE, DocType); } }
        public string DdeCommand { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_DDECOMMAND, DocType); } }
        public string DdeIfExec { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_DDEIFEXEC, DocType); } }
        public string DdeApplication { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_DDEAPPLICATION, DocType); } }
        public string DdeTopic { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_DDETOPIC, DocType); } }
        public string InfoTip { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_INFOTIP, DocType); } }           //"prop:System.ItemType;System.Size;System.Media.Duration;System.OfflineAvailability"
        public string QuickTip { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_QUICKTIP, DocType); } }          //"prop:System.ItemTypeText;System.Size;System.DateModified"
        public string TileInfo { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_TILEINFO, DocType);   } }        //"prop:System.Media.Duration;System.Size"
        public string ContentType { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_CONTENTTYPE, DocType);  } }      //"video/mp4"
        public string DefaultIcon { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_DEFAULTICON, DocType);  } }      //"@{Microsoft.ZuneVideo_10.20092.14511.0_x64__8wekyb3d8bbwe?ms-resource://Microsoft.ZuneVideo/Files/Assets/FileExtension.png}"
        public string ShellExtension { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_SHELLEXTENSION, DocType); } }
        public string DropTarget { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_DROPTARGET, DocType); } }
        public string DelegateExecute { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_DELEGATEEXECUTE, DocType); } }   //"{4ED3A719-CEA8-4BD9-910D-E252F997AFC2}"
        public string SupportedUriProtocols { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_SUPPORTED_URI_PROTOCOLS, DocType); } }
        public string Progid { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_PROGID, DocType); } }            //"AppX6eg8h5sxqq90pv53845wmnbewywdqq5h"
        public string AppId { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_APPID, DocType); } }            //"Microsoft.ZuneVideo_8wekyb3d8bbwe!Microsoft.ZuneVideo"
        public string AppPublisher { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_APPPUBLISHER, DocType); } }     //"Microsoft Corporation"
        public string AppIconReference { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_APPICONREFERENCE, DocType); } } //"@{Microsoft.ZuneVideo_10.20092.14511.0_x64__8wekyb3d8bbwe?ms-resource://Microsoft.ZuneVideo/Files/Assets/AppList.png}"} }
        public string Max { get { return GetAssociation(AssocF.ASSOCF_VERIFY, AssocStr.ASSOCSTR_MAX, DocType); } } 

        public AssocQuery(string docType)
        {
            DocType = docType;            
        }

        [Flags]
        private enum AssocF : uint
        {
            ASSOCF_NONE = 0x00000000,
            ASSOCF_INIT_NOREMAPCLSID = 0x00000001,
            ASSOCF_INIT_BYEXENAME = 0x00000002,
            ASSOCF_OPEN_BYEXENAME = 0x00000002,
            ASSOCF_INIT_DEFAULTTOSTAR = 0x00000004,
            ASSOCF_INIT_DEFAULTTOFOLDER = 0x00000008,
            ASSOCF_NOUSERSETTINGS = 0x00000010,
            ASSOCF_NOTRUNCATE = 0x00000020,
            ASSOCF_VERIFY = 0x00000040,
            ASSOCF_REMAPRUNDLL = 0x00000080,
            ASSOCF_NOFIXUPS = 0x00000100,
            ASSOCF_IGNOREBASECLASS = 0x00000200,
            ASSOCF_INIT_IGNOREUNKNOWN = 0x00000400,
            ASSOCF_INIT_FIXED_PROGID = 0x00000800,
            ASSOCF_IS_PROTOCOL = 0x00001000,
            ASSOCF_INIT_FOR_FILE = 0x00002000
        }

        private enum AssocStr
        {
            ASSOCSTR_COMMAND = 1,
            ASSOCSTR_EXECUTABLE,
            ASSOCSTR_FRIENDLYDOCNAME,
            ASSOCSTR_FRIENDLYAPPNAME,
            ASSOCSTR_NOOPEN,
            ASSOCSTR_SHELLNEWVALUE,
            ASSOCSTR_DDECOMMAND,
            ASSOCSTR_DDEIFEXEC,
            ASSOCSTR_DDEAPPLICATION,
            ASSOCSTR_DDETOPIC,
            ASSOCSTR_INFOTIP,
            ASSOCSTR_QUICKTIP,
            ASSOCSTR_TILEINFO,
            ASSOCSTR_CONTENTTYPE,
            ASSOCSTR_DEFAULTICON,
            ASSOCSTR_SHELLEXTENSION,
            ASSOCSTR_DROPTARGET,
            ASSOCSTR_DELEGATEEXECUTE,
            ASSOCSTR_SUPPORTED_URI_PROTOCOLS,
            ASSOCSTR_PROGID,
            ASSOCSTR_APPID,
            ASSOCSTR_APPPUBLISHER,
            ASSOCSTR_APPICONREFERENCE,
            ASSOCSTR_MAX
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, ref uint pcchOut);
        /*
         * 
        AssocStr str
            File name extension
            A file name extension, such as .txt.

            CLSID
            A CLSID GUID in the standard "{GUID}" format.

            ProgID
            An application's ProgID, such as Word.Document.8.

            Executable name
            The name of an application's .exe file. The ASSOCF_OPEN_BYEXENAME flag must be set in flags.
        */
        private static string GetAssociation(AssocF assocF, AssocStr assocStr, string doctype, string pszExtra = null)
        {
            try
            {
                uint pcchOut = 0; // size of output buffer

                // First call is to get the required size of output buffer
                AssocQueryString(assocF, assocStr, doctype, pszExtra, null, ref pcchOut);
                if (pcchOut == 0) return String.Empty;

                var pszOut = new StringBuilder((int)pcchOut); // Allocate the output buffer                
                AssocQueryString(assocF, assocStr, doctype, pszExtra, pszOut, ref pcchOut);// Get the full pathname to the program in pszOut

                string doc = pszOut.ToString();
                return doc;
            }
            catch (Exception exception)
            {
                Logger.Warn(exception);
                return null;
            }
        }

        public static string GetAppId(string filenameExt, string verb)
        {
            return AssocQuery.GetAssociation(AssocF.ASSOCF_NONE, AssocStr.ASSOCSTR_APPID, filenameExt, verb);
        }

        public static string GetCommandProgId(string progId, string verb)
        {
            return AssocQuery.GetAssociation(AssocF.ASSOCF_NONE, AssocStr.ASSOCSTR_COMMAND, progId, verb);
        }

        public static string GetCommandExe(string exeName, string verb)
        {
            return AssocQuery.GetAssociation(AssocF.ASSOCF_OPEN_BYEXENAME, AssocStr.ASSOCSTR_COMMAND, exeName, verb);
        }

        
    }


}
