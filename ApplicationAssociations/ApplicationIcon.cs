using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.IO;

namespace ApplicationAssociations
{
    public class ApplicationIcon
    {        
        [DllImport("shlwapi.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false, ThrowOnUnmappableChar = true)]
        private static extern int SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, int cchOutBuf, IntPtr ppvReserved);

        //HKEY_CLASSES_ROOT\Extensions\ContractId\Windows.Protocol\PackageId
        /// <summary>
        /// Example indirectString : @{Microsoft.Windows.Photos_2020.20110.11001.0_x64__8wekyb3d8bbwe?ms-resource://Microsoft.Windows.Photos/Files/Assets/PhotosMedTile.png}
        /// string icon = ExtractNormalPath(@"@{Microsoft.Windows.Photos_2020.20110.11001.0_x64__8wekyb3d8bbwe?ms-resource://Microsoft.Windows.Photos/Files/Assets/PhotosMedTile.png}");
        /// </summary>
        /// <param name="indirectString"></param>
        /// <returns>icon = "C:\\Program Files\\WindowsApps\\Microsoft.Windows.Photos_2020.20110.11001.0_neutral_split.scale-100_8wekyb3d8bbwe\\Assets\\PhotosMedTile.scale-100.png"</returns>
        public static string ExtractNormalIconPath(string indirectString)
        {
            StringBuilder outBuff = new StringBuilder(1024);
            int result = SHLoadIndirectString(indirectString, outBuff, outBuff.Capacity, IntPtr.Zero);
            return outBuff.ToString();
        }

        public static Icon GetApplicationIcon(string iconFullPath)
        {
            try
            {
                
                if (!string.IsNullOrEmpty(iconFullPath))
                {
                    string iconFullPathUpper = iconFullPath.ToUpper();
                    
                    if (iconFullPathUpper.EndsWith(".DLL") || iconFullPathUpper.EndsWith(".EXE") || iconFullPathUpper.EndsWith(".EXE,0") || iconFullPathUpper.EndsWith(".EXE,1") || 
                        iconFullPathUpper.EndsWith(".EXE,2") || iconFullPathUpper.EndsWith(".EXE,3") || iconFullPathUpper.EndsWith(".EXE,4") || iconFullPathUpper.EndsWith(".EXE,5"))
                    {
                        if (iconFullPathUpper.EndsWith(",0") || iconFullPathUpper.EndsWith(",1") || iconFullPathUpper.EndsWith(",2") ||
                            iconFullPathUpper.EndsWith(",3") || iconFullPathUpper.EndsWith(",4") || iconFullPathUpper.EndsWith(",5")) 
                            iconFullPath = iconFullPath.Substring(0, iconFullPath.Length - 2);

                        Icon icon = Icon.ExtractAssociatedIcon(iconFullPath);
                        if (icon != null) return icon;
                    } else  if (iconFullPath.StartsWith("@{") )
                    {
                        string filename = ExtractNormalIconPath(iconFullPath);
                        if (File.Exists(filename))
                        {
                            Bitmap bitmap = (Bitmap)Image.FromFile(filename);
                            Icon icon = Icon.FromHandle(bitmap.GetHicon());
                            if (icon != null) return icon;
                        }
                    } else
                    {
                        Bitmap bitmap = (Bitmap)Image.FromFile(iconFullPath);
                        Icon icon = Icon.FromHandle(bitmap.GetHicon());
                        if (icon != null) return icon;
                    }
                    
                }
            } catch  {}
            return ApplicationAssociations.Properties.Resources.IconNoIcon;
        }

    }
}
