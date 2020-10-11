using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace WindowsProperty
{

    public class WindowsPropertyWriter
    {
        public void Write(string fullFileName, string tags)
        {
            try
            {
                var file = ShellFile.FromFilePath(fullFileName);

                var mediaTags = file.Properties.GetProperty(SystemProperties.System.Keywords);
                ShellPropertyWriter propertyWriter = file.Properties.GetPropertyWriter();
                propertyWriter.WriteProperty(SystemProperties.System.Keywords, tags);
                //propertyWriter.WriteProperty(SystemProperties.System.Music.AlbumTitle, album);
                //propertyWriter.WriteProperty(SystemProperties.System.Rating, Rating);
                //propertyWriter.WriteProperty(SystemProperties.System.Comment, entry.Description);

                propertyWriter.Close();
            } catch (Exception e)
            {
                
            }
            
        }

        public void WriteAlbum(string fullFileName, string album)
        {
            try
            {
                var file = ShellFile.FromFilePath(fullFileName);

                var mediaTags = file.Properties.GetProperty(SystemProperties.System.Keywords);
                ShellPropertyWriter propertyWriter = file.Properties.GetPropertyWriter();
                //propertyWriter.WriteProperty(SystemProperties.System.Keywords, tags);
                propertyWriter.WriteProperty(SystemProperties.System.Music.AlbumTitle, album);

                //propertyWriter.WriteProperty(SystemProperties.System.Rating, Rating);
                //propertyWriter.WriteProperty(SystemProperties.System.Comment, entry.Description);

                propertyWriter.Close();
            }
            catch (Exception e)
            {

            }

        }
    }

}
