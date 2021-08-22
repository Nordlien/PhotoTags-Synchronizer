using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationAssociations
{
    public class ProtocolAssociationsRegistry
    {
        #region GetProgIdExtentionAssociations
        //--------------------------------------------------------------------------------
        //https://stackoverflow.com/questions/11981337/how-to-get-application-name-to-be-displayed-in-open-with-list
        public static List<string> GetProgIdExtentionAssociations(string ext)
        {
            List<string> progIds = new List<string>();

            string baseKey = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\" + ext;

            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(baseKey + @"\OpenWithList"))
            {
                if (rk != null)
                {
                    string mruList = (string)rk.GetValue("MRUList");
                    if (mruList != null)
                    {
                        foreach (char c in mruList.ToString())
                        {
                            string str = rk.GetValue(c.ToString()).ToString();
                            if (!progIds.Contains(str))
                            {
                                progIds.Add(str);
                            }
                        }
                    }
                }
            }

            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(baseKey + @"\OpenWithProgids"))
            {
                if (rk != null)
                {
                    foreach (string item in rk.GetValueNames())
                        progIds.Add(item);
                }
            }

            using (RegistryKey rk = Registry.ClassesRoot.OpenSubKey(ext + @"\OpenWithList"))
            {
                if (rk != null)
                {
                    foreach (var item in rk.GetSubKeyNames())
                    {
                        if (!progIds.Contains(item))
                        {
                            progIds.Add(item.ToString());
                        }
                    }
                }
            }
            using (RegistryKey rk = Registry.ClassesRoot.OpenSubKey(ext + @"\OpenWithProgids"))
            {
                if (rk != null)
                {
                    foreach (string item in rk.GetValueNames())
                    {
                        if (!progIds.Contains(item))
                        {
                            progIds.Add(item);

                        }
                    }
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
                                if (strNames.Contains<string>(ext))
                                {
                                    progIds.Add(mApp);
                                }
                            }
                        }
                    }
                }
            }
            return progIds;
        }
        #endregion 

        public static string[] GetVerbsByProgId(string progId)
        {
            var verbs = new List<string>();

            if (!string.IsNullOrEmpty(progId))
            {
                /*using (RegistryKey key = Registry.ClassesRoot.OpenSubKey("Applications\\" + progId + "\\shell")) //Contains to much crap as Notepad.exe for jpeg
                {
                    if (key != null)
                    {
                        var names = key.GetSubKeyNames();
                        verbs.AddRange(names.Where(name => string.Compare(name, "new", StringComparison.OrdinalIgnoreCase) != 0));
                    }
                }*/
                
                using (var key = Registry.ClassesRoot.OpenSubKey(progId + "\\shell"))
                {
                    if (key != null)
                    {
                        var names = key.GetSubKeyNames();
                        verbs.AddRange(names.Where(name => string.Compare(name, "new", StringComparison.OrdinalIgnoreCase) != 0));
                    }
                }
            }

            return verbs.ToArray();
        }

        private string GetApplicationPath(string mAppRegName)
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
                            if (string.IsNullOrEmpty(mkey)) return (string)rkAppPaths.GetValue(mkey);                            
                        }
                    }
                }
            }
            return null;
        }

        

        
    }
}
