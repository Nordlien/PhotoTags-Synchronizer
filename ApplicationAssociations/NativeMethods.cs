using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ApplicationAssociations
{
    
    public static class NativeMethods    
    {
        // Define GetShortPathName API function.
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern uint GetShortPathName(string lpszLongPath, char[] lpszShortPath, int cchBuffer);

        // Return the short file name for a long file name.
        public static string ShortFileName(string long_name)
        {
            char[] name_chars = new char[1024];
            long length = GetShortPathName(
                long_name, name_chars,
                name_chars.Length);

            string short_name = new string(name_chars);
            return short_name.Substring(0, (int)length);
        }


        public static string GetFullPathOfFile(string fileName)
        {
            string fullFilePathToCheck = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), fileName);
            if (File.Exists(fullFilePathToCheck)) return fullFilePathToCheck;

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                fullFilePathToCheck = Path.Combine(path, fileName);
                if (File.Exists(fullFilePathToCheck)) return fullFilePathToCheck;
            }

            values = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
            foreach (var path in values.Split(Path.PathSeparator))
            {
                fullFilePathToCheck = Path.Combine(path, fileName);
                if (File.Exists(fullFilePathToCheck)) return fullFilePathToCheck;
            }
            return null;
        }

        // Return the long file name for a short file name.
        public static string LongFileName(string short_name)
        {
            return new FileInfo(short_name).FullName;
        }

        


    }
        
}
