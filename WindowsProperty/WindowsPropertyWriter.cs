using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace WindowsProperty
{

    public class WindowsPropertyWriter : IDisposable
    {
        private ShellFile shellFile;
        private ShellPropertyWriter propertyWriter;
        //private IShellProperty shellProperty;

        public WindowsPropertyWriter(string fullFileName)
        {
            try
            {
                shellFile = ShellFile.FromFilePath(fullFileName);
                propertyWriter = shellFile.Properties.GetPropertyWriter();
            }
            catch { }
        }
        public void Dispose()
        {
            try 
            { 
                propertyWriter.Dispose();
                propertyWriter = null;
                shellFile.Dispose();
                shellFile = null;
                //shellProperty = null;
            }
            catch { }        
        }

        public void Close()
        {
            try
            {
                propertyWriter.Close();
            }
            catch { }
        }

        public void WriteKeywords(string tags)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Keywords, tags);


                propertyWriter.WriteProperty(SystemProperties.System.Photo.PeopleNames, tags);
            }
            catch { }
        }

        public void WriteNames(string names)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Photo.PeopleNames, names);
            }
            catch { }
        }

        public void WriteCategories(string categories)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Category, categories);
            }
            catch { }
        }
        

        public void WriteAlbum(string album)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Music.AlbumTitle, album);
            }
            catch { }
        }

        public void WriteSubtitle_Description(string description)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Media.Subtitle, description);
            }
            catch { }
        }
        public void WriteSubject_Description(string subject)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Subject, subject);
            }
            catch { }
        }

        public void WriteComment(string comment)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Comment, comment);
            }
            catch { }
        }

        public void WriteDateTaken(DateTime dateTime)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Photo.DateTaken, dateTime);
            }
            catch { }
        }

        public void WriteArtist_Author(string author)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Music.Artist, author);
            }
            catch { }
        }
      
        public void WriteRating(int rating)
        {
            try
            {
                propertyWriter.WriteProperty(SystemProperties.System.Rating, rating);
            }
            catch { }
        }
    }

}
