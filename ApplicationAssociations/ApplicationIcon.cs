using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

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
                    if (iconFullPath.ToUpper().EndsWith(".EXE") || iconFullPath.ToUpper().EndsWith(".DLL"))
                    {
                        Icon icon = Icon.ExtractAssociatedIcon(iconFullPath);
                        if (icon != null) return icon;
                    } else  if (iconFullPath.StartsWith("@{") )
                    {
                        string filename = ExtractNormalIconPath(iconFullPath);
                        Bitmap bitmap = (Bitmap)Image.FromFile(filename);
                        Icon icon = Icon.FromHandle(bitmap.GetHicon());
                        if (icon != null) return icon;
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
