using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace ProcessStart
{
    public class cApplicationData : IDisposable
    {
        private string mAppRegName;
        private string mAppFilelink;
        private string mAppCompany;
        private string mAppProductname;
        private Icon mAppIcon;
        private FileInfo mAppFileInfo;
        private FileVersionInfo mAppFileVersionInfo;
        private Filelink mOpenFileLink;
        private Filelink mEditFileLink;
        private Filelink mPlayFileLink;
        private Filelink mPrintFileLink;
        private Filelink mPreviewFileLink;
        private bool mHaveFilelinks = false;
        private bool _disposed = false;

        #region Property

        /// <summary>
        /// Application Icon
        /// </summary>
        public Icon ApplicationIcon
        {
            get { return mAppIcon; }
        }

        /// <summary>
        /// If any filelinks (Fullfilename) exists
        /// </summary>
        public bool Havefilelinks 
        {
            get { return mHaveFilelinks; } 
        }

        /// <summary>
        /// Name in the Regestry
        /// </summary>
        public string RegistryName
        {
            get { return mAppRegName; }
        }

        /// <summary>
        /// Filelink (Fullfilename)
        /// </summary>
        public string Filenamelink
        {
            get { return mAppFilelink; }
        }

        /// <summary>
        /// Companyname of the Application
        /// </summary>
        public string Company
        {
            get { return mAppCompany; }
        }

        /// <summary>
        /// Productname of the Application
        /// </summary>
        public string Productname
        {
            get { return mAppProductname; }
        }

        /// <summary>
        /// IO.FileInfo for the Application
        /// </summary>
        public FileInfo FileInfo
        {
            get { return mAppFileInfo; }
        }

        /// <summary>
        /// Diagnostics.FileVersionInfo for the Application
        /// </summary>
        public FileVersionInfo FileVersionInfo
        {
            get { return mAppFileVersionInfo; }
        }

        /// <summary>
        /// filelink for Open a File with the Application
        /// </summary>
        public Filelink OpenFilenameLink
        {
            get { return mOpenFileLink; }
        }

        /// <summary>
        /// Filelink for Edit a File with the Application
        /// </summary>
        public Filelink EditFilenameLink
        {
            get { return mEditFileLink; }
        }

        /// <summary>
        /// Filelink for Play a File with the Application
        /// </summary>
        public Filelink PlayFilenameLink
        {
            get { return mPlayFileLink; }
        }

        /// <summary>
        /// Filelink for Print a File with the Application
        /// </summary>
        public Filelink PrintFilenameLink
        {
            get { return mPrintFileLink; }
        }

        /// <summary>
        /// Filelink for Preview a File with the Application
        /// </summary>
        public Filelink PreviewFilenameLink
        {
            get { return mPreviewFileLink; }
        }
        #endregion

        public cApplicationData(string strAppRegName)
        {
            InitApplicationData(strAppRegName);
        }

        public override string ToString()
        {
            return "Registryname:'" + mAppRegName + "', " + Environment.NewLine + "Filelink:'" + mAppFilelink + "', " +
                Environment.NewLine + "Company:'" + mAppCompany + "', " + Environment.NewLine + "Productname:'" + mAppProductname + "'";
        }

        /// <summary>
        /// Intern constructor 
        /// </summary>
        /// <param name="strAppRegName"></param>
        private void InitApplicationData(string strAppRegName)
        {
            if (!string.IsNullOrEmpty(strAppRegName))
            {
                mAppRegName = strAppRegName;

                mGetApplicationFileLinks();

                GetApplicationIcon();
            }
            else
            {
                throw new ArgumentNullException("strAppRegName", "No Application Registryname");
            }
        }

        private void mGetApplicationFileLinks()
        {
            //get app path from CurrentVersion
            using (RegistryKey rkAppPaths = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\" + mAppRegName))
            {
                if (rkAppPaths != null)
                {
                    string[] strKeyName = rkAppPaths.GetValueNames();
                    if (strKeyName != null)
                    {
                        foreach (string mkey in strKeyName)
                        {
                            if (string.IsNullOrEmpty(mkey))
                            {
                                string strFilelink = (string)rkAppPaths.GetValue(mkey);

                                Filelink fl = NormalizeToFilelink(strFilelink);

                                if (fl != null)
                                {
                                    mOpenFileLink = fl;
                                    mHaveFilelinks = true;

                                    GetApplicationFileInfo(fl.Filelocation);
                                    fl = null;
                                }
                                else
                                {
                                    //Log.Livelog.Log(string.Format("No Filelink Class for '{0}'", strFilelink));
                                }
                            }
                        }
                    }
                }
                else
                {
                    string strSubkeyRoot = "Applications\\";

                    //If not in CurrentVersion then Check there
                    RegistryKey rkAppShell = Registry.ClassesRoot.OpenSubKey(strSubkeyRoot + mAppRegName + "\\shell");
                    if (rkAppShell == null)
                    {
                        //try without Subkey
                        if (!string.IsNullOrEmpty(strSubkeyRoot))
                        {
                            rkAppShell = Registry.ClassesRoot.OpenSubKey(mAppRegName + "\\shell");
                        }
                    }

                    if (rkAppShell != null)
                    {
                        string[] strShellVerbs = rkAppShell.GetSubKeyNames();
                        if (strShellVerbs != null)
                        {
                            foreach (string strKeyName in strShellVerbs)
                            {
                                using (RegistryKey rkAppCommand = rkAppShell.OpenSubKey(strKeyName + "\\command"))
                                {
                                    if (rkAppCommand != null)
                                    {
                                        string[] strComNames = rkAppCommand.GetValueNames();
                                        if (strComNames != null)
                                        {
                                            if (strComNames[0] != "DelegateExecute")
                                            {
                                                string strFilelink = (string)rkAppCommand.GetValue(strComNames[0]);

                                                Filelink fl = NormalizeToFilelink(strFilelink);

                                                if (fl != null)
                                                {
                                                    rkAppCommand.Close();

                                                    SetApplicationFilelink(fl, strKeyName);

                                                    GetApplicationFileInfo(fl.Filelocation);
                                                    fl = null;
                                                }
                                                else
                                                {
                                                    //Log.Livelog.Log(string.Format("No Filelink Class for '{0}'", strFilelink));
                                                }
                                            }
                                            else
                                            {
                                                //Log.Livelog.Log(string.Format("No DelegateExecute Implemented Application:'{0}'", mAppRegName));
                                            }
                                        }
                                        else
                                        {
                                            rkAppCommand.Close();
                                            //Log.Livelog.Log(string.Format("No keys in \\command for Keyname: '{0}'", strKeyName));
                                        }
                                    }
                                    else
                                    {
                                        //Log.Livelog.Log(string.Format("No Command for Subkey:{0} ", strKeyName));
                                    }
                                }
                            }
                        }
                        else
                        {
                            rkAppShell.Close();
                            rkAppShell = null;
                            //Log.Livelog.Log(string.Format("No Shell Verbs found for this Application Name:'{0}'", mAppRegName));
                        }
                    }
                    else
                    {
                        //Log.Livelog.Log(string.Format("Application Name not found in Registry Name:'{0}'", mAppRegName));
                    }
                }
            }
        }

        /// <summary>
        /// set the Filelink to the correct Keyname
        /// </summary>
        /// <param name="fl">Filelink for the Application</param>
        /// <param name="strKeyName">shell/command Keyname</param>
        /// <returns></returns>
        private bool SetApplicationFilelink(Filelink fl, string strKeyName)
        {
            if (fl != null)
            {
                switch (strKeyName.ToLower())
                {
                    case "open":
                        if (mOpenFileLink == null)
                        {
                            mOpenFileLink = fl;
                            mHaveFilelinks = true;
                        }
                        else
                        {
                            //Log.Livelog.Log(string.Format("Key 'open' already have a Filelink, Filelink String:'{0}'", mOpenFileLink.ToString()));
                        }
                        break;
                    case "edit":
                        if (mEditFileLink == null)
                        {
                            mEditFileLink = fl;
                            mHaveFilelinks = true;
                        }
                        else
                        {
                            //Log.Livelog.Log(string.Format("Key 'edit' already have a Filelink, Filelink String:'{0}'", mEditFileLink.ToString()));
                        }
                        break;
                    case "play":
                        if (mPlayFileLink == null)
                        {
                            mPlayFileLink = fl;
                            mHaveFilelinks = true;
                        }
                        else
                        {
                            //Log.Livelog.Log(string.Format("Key 'play' already have a Filelink, Filelink String:'{0}'", mPlayFileLink.ToString()));
                        }
                        break;
                    case "print":
                    case "printto":
                        if (mPrintFileLink == null)
                        {
                            mPrintFileLink = fl;
                            mHaveFilelinks = true;
                        }
                        else
                        {
                            //Log.Livelog.Log(string.Format("Key 'print' already have a Filelink, Filelink String:'{0}'", mPrintFileLink.ToString()));
                        }
                        break;
                    case "preview":
                        if (mPreviewFileLink == null)
                        {
                            mPreviewFileLink = fl;
                            mHaveFilelinks = true;
                        }
                        else
                        {
                            //Log.Livelog.Log(string.Format("Key 'preview' already have a Filelink, Filelink String:'{0}'", mPreviewFileLink.ToString()));
                        }
                        break;
                    default:
                        //Log.Livelog.Log("Unkown Key name :'" + strKeyName + "'");
                        if (mOpenFileLink == null)
                        {
                            mOpenFileLink = fl;
                            mHaveFilelinks = true;
                        }
                        else
                        {
                            //Log.Livelog.Log(string.Format("Key 'open' (used for Unkown Key) already have a Filelink, Filelink String:'{0}'", mOpenFileLink.ToString()));
                        }
                        break;
                }

                return true;
            } else
            {
                //Log.Livelog.Log("Filelink is Empty");
                return false;
            }
        }

        /// <summary>
        /// get the IO.FileInfo and Diagnostics.FileVersionInfo
        /// </summary>
        /// <param name="strFilelink"></param>
        private void GetApplicationFileInfo(string strFilelink)
        {
            try
            {
                if ((string.IsNullOrEmpty(mAppFilelink)) && !string.IsNullOrEmpty(strFilelink))
                {
                    if (File.Exists(strFilelink))
                    {
                        FileInfo fi = new FileInfo(strFilelink);
                        if (fi != null)
                        {
                            mAppFileInfo = fi;
                            mAppFilelink = fi.FullName;
                            fi = null;
                        } else
                        {
                            //Log.Livelog.Log(string.Format("No FileInfo for Application at Filelink:'{0}'", strFilelink));
                        }
                        if (mAppFileVersionInfo == null)
                        {
                            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(strFilelink);
                            if (fvi != null)
                            {
                                mAppFileVersionInfo = fvi;
                                mAppProductname = fvi.FileDescription;
                                mAppCompany = fvi.CompanyName;
                                fvi = null;
                            } else
                            {
                                //Log.Livelog.Log(string.Format("No FileVersionInfo for Application at Filelink:'{0}'", strFilelink));
                            }
                        }
                    }
                    else
                    {
                        //Log.Livelog.Log(string.Format("GetApplicationFileInfo cant find File at Path:'{0}' ", strFilelink));
                    }
                }
            } catch (Exception ex)
            {
                //Log.Livelog.Log(string.Format("GetApplicationFileInfo Filelink:'{0}' Error:'{1}' ", strFilelink, ex.Message));
            }
        }

        /// <summary>
        /// Splites the Filepath an the Parameters 
        /// </summary>
        /// <param name="strFilelink">Filelink form Registry</param>
        /// <returns></returns>
        private Filelink NormalizeToFilelink(string strFilelink)
        {
            if (!string.IsNullOrEmpty(strFilelink))
            {
                Filelink fLink = new Filelink();

                strFilelink = strFilelink.Replace("\"", "");

                string[] strArray = strFilelink.Split(new string[] { @"\" }, StringSplitOptions.None);

                if (strArray != null)
                {
                    string mfilename = strArray[strArray.Length - 1];

                    string[] strFilArray = mfilename.Split(new string[] { " " }, StringSplitOptions.None);

                    if (strFilArray != null)
                    {
                        int iFileEnd = 0;
                        for (iFileEnd = (strFilArray.Length - 1); iFileEnd > 0; iFileEnd--)
                        {
                            if (strFilArray[iFileEnd].Contains("."))
                            {
                                break;
                            }
                        }

                        string fName = "";
                        string fParams = "";
                        for (int i = 0; i < strFilArray.Length; i++)
                        {
                            if (i <= iFileEnd )
                            {
                                //filename
                                fName += strFilArray[i];
                            } else
                            {
                                //Params
                                fParams += strFilArray[i];
                            }
                        }

                        if (fName.EndsWith(","))
                        {
                            fName = fName.Remove(fName.Length - 1);
                            fParams = "," + fParams;
                        }

                        //add location to the filename
                        //max entrie form this array is filename with params
                        for (int i = 0; i < (strArray.Length - 1); i++)
                        {
                            if ((strArray[i].Contains(".exe") || strArray[i].Contains(".dll")) && strArray[i].Contains(" "))
                            {
                                // PreParam String
                                string[] strTemp = strArray[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                if (strTemp.Length == 2)
                                {
                                    fLink.PreParams = fLink.Filelocation + strTemp[0];
                                    fLink.Filelocation = strTemp[1] + @"\";
                                }
                            }
                            else
                            {
                                fLink.Filelocation += strArray[i] + @"\";
                            }
                        }

                        fLink.Filelocation += fName;
                        fLink.Params = fParams;
                    }
                }
                return fLink;
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the Application Icon from the File
        /// </summary>
        private void GetApplicationIcon()
        {
            if (!string.IsNullOrEmpty(mAppFilelink))
            {
                System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(mAppFilelink);
                if (ico != null)
                {
                    mAppIcon = ico;
                }
                else
                {
                    //Log.Livelog.Log(string.Format("No Icon for Application at Filelink:'{0}'", mAppFilelink));
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (mAppIcon != null)
                    {
                        mAppIcon.Dispose();
                    }
                }
                _disposed = true;
            }
        }
    }

    public class Filelink
    {

        private string myFilelocation;
        private string myParams;
        private string myPreParams;

        public string PreParams
        {
            get { return myPreParams; }
            set { myPreParams = value; }
        }

        public string Params
        {
            get { return myParams; }
            set { myParams = value; }
        }
        
        public string Filelocation
        {
            get { return myFilelocation; }
            set { myFilelocation = value; }
        }

        public override string ToString()
        {
            return string.Format("PreParams:'{0}', Filelocation:'{1}', Params:'{2}'", myPreParams, myFilelocation, myParams);
        }
    }
}
