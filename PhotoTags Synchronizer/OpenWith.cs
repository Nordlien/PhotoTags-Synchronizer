using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessStart
{
    public class OpenWith : IDisposable
    {
        private bool mHaveUserChoice = false;
        private cApplicationData mProgUserChoice;
        private Dictionary<string, cApplicationData> mProgrammList;
        private bool _disposed = false;

        /// <summary>
        /// data for the Default User Choice Application for this extension
        /// </summary>
        public cApplicationData UserChoiceApplication
        {
            get { return mProgUserChoice; }
        }

        /// <summary>
        /// Collection of Applications for this extension
        /// </summary>
        public Dictionary<string, cApplicationData> Applicationlist
        {
            get { return mProgrammList; }
        }

        /// <summary>
        /// If a Default User Choice exists
        /// </summary>
        public bool HaveUserChoice
        {
            get { return mHaveUserChoice; }
        }

        public OpenWith(string strFileExtension)
        {
            if (!string.IsNullOrEmpty(strFileExtension))
            {
                if (!strFileExtension.StartsWith("."))
                {
                    strFileExtension = "." + strFileExtension;
                    //Log.Livelog.Log(string.Format("The Fileextension ({0}) dont start with '.' , '.' will be added to the extension", strFileExtension));
                }
                try
                {
                    mLoad(strFileExtension);
                }
                catch (Exception ex)
                {
                    //Log.Livelog.Log("Exception in 'OpenWith' constructor Err:'" + ex.ToString() + "' Extension:'" + strFileExtension + "'");
                }
            } else
            {
                throw new ArgumentNullException("strFileExtension", "OpenWith must initalized with a fileextension");
            }
        }

        private void mLoad(string strFileExtension)
        {

            using (RegistryKey rkReg = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + strFileExtension))
            {
                if (rkReg != null)
                {
                    //Check subkey for User choice Programm
                    using (RegistryKey rkUserChoice = rkReg.OpenSubKey("UserChoice"))
                    {
                        if (rkUserChoice != null)
                        {
                            mHaveUserChoice = true;
                            string tempUserChoiceName = (string)rkUserChoice.GetValue("ProgId");
                            tempUserChoiceName = tempUserChoiceName.Replace("Applications\\", "");

                            mProgUserChoice = new cApplicationData(tempUserChoiceName);
                            if (mProgUserChoice == null)
                            {
                                mHaveUserChoice = false;
                            }
                            rkUserChoice.Close();
                        }
                        else
                        {
                            mHaveUserChoice = false;
                        }
                    }

                    if (mProgrammList != null)
                    {
                        mProgrammList.Clear();
                    }
                    else
                    {
                        mProgrammList = new Dictionary<string, cApplicationData>();
                    }

                    //Check subkey for OpenWithList
                    using (RegistryKey rkOpenWithList = rkReg.OpenSubKey("OpenWithList"))
                    {
                        if (rkOpenWithList != null)
                        {
                            string[] strKeyArray = rkOpenWithList.GetValueNames();

                            foreach (string strKeyName in strKeyArray)
                            {
                                string tempValue = (string)rkOpenWithList.GetValue(strKeyName);
                                if (tempValue.Contains("."))
                                {
                                    cApplicationData appData = new cApplicationData(tempValue);

                                    if (appData != null && appData.Havefilelinks && !string.IsNullOrEmpty(appData.Productname))
                                    {
                                        //Log.Livelog.Log(string.Format("Add New Key to Programmlist Applicationkey:'{0}'  Productname:'{1}'", tempValue, appData.Productname));
                                        mProgrammList.Add(appData.Productname, appData);
                                    }
                                }
                            }
                            rkOpenWithList.Close();
                        }
                    }

                    //Check extension in HKEY_CLASSES_ROOT
                    using (RegistryKey rkExtensionClassRoot = Registry.ClassesRoot.OpenSubKey(strFileExtension + "\\OpenWithProgids"))
                    {
                        if (rkExtensionClassRoot != null)
                        {
                            string[] strKeyArray = rkExtensionClassRoot.GetValueNames();

                            foreach (string strKeyName in strKeyArray)
                            {
                                cApplicationData appData = new cApplicationData(strKeyName);

                                if (appData != null && appData.Havefilelinks && !string.IsNullOrEmpty(appData.Productname) && !mProgrammList.ContainsKey(appData.Productname))
                                {
                                    //Log.Livelog.Log(string.Format("Add New Key to Programmlist Applicationkey:'{0}' Productname:'{1}'", strKeyName, appData.Productname));
                                    mProgrammList.Add(appData.Productname, appData);
                                }
                            }
                            rkExtensionClassRoot.Close();
                        }
                    }

                    //Check Application Support File Extension
                    //HKEY_CLASSES_ROOT\Applications\msPaint.exe\SupportedTypes
                    using (RegistryKey rkApplic = Registry.ClassesRoot.OpenSubKey("Applications"))
                    {
                        if (rkApplic != null)
                        {
                            string[] strSubKeys = rkApplic.GetSubKeyNames();
                            if (strSubKeys != null)
                            {
                                //for each Application 
                                foreach (string mApp in strSubKeys)
                                {

                                    //Check for Subkey SupportedTypes
                                    RegistryKey rkSup = rkApplic.OpenSubKey(mApp + "\\SupportedTypes");
                                    if (rkSup != null)
                                    {
                                        string[] strNames = rkSup.GetValueNames();
                                        if (strNames.Contains<string>(strFileExtension))
                                        {
                                            //File Extension is Supported by this Application
                                            cApplicationData appData = new cApplicationData(mApp);

                                            if (appData != null && appData.Havefilelinks && !string.IsNullOrEmpty(appData.Productname) && !mProgrammList.ContainsKey(appData.Productname))
                                            {
                                                //Log.Livelog.Log(string.Format("Add New Key to Programmlist Applicationkey:'{0}' Productname:'{1}'", mApp, appData.Productname));
                                                mProgrammList.Add(appData.Productname, appData);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new TypeInitializationException("RegistryKey", new Exception(string.Format("No SubKey found for this File Extension, Extension:'{0}' ", strFileExtension)));
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
                    if (mProgUserChoice != null)
                    {
                        mProgUserChoice.Dispose();
                    }
                    if (mProgrammList != null)
                    {
                        foreach (cApplicationData item in mProgrammList.Values)
                        {
                            if (item != null)
                            {
                                item.Dispose();
                            }
                        }
                        mProgrammList.Clear();
                    }
                }
                _disposed = true;
            }
        }
    }
}
