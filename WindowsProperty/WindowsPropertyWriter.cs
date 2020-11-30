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
                if (propertyWriter != null) propertyWriter.Dispose();
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
                if (propertyWriter != null) propertyWriter.Close();
            }
            catch { }
        }

        public void WriteKeywords(string tags)
        {
            try
            {
                if (propertyWriter!=null) propertyWriter.WriteProperty(SystemProperties.System.Keywords, tags);
            }
            catch { }
        }

        public void WriteNames(string names)
        {
            try
            {
                if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Photo.PeopleNames, names);
            }
            catch { }
        }

        public void WriteCategories(string categories)
        {
            try
            {
                if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Category, categories);
            }
            catch { }
        }
        

        public void WriteAlbum(string album)
        {
            try
            {
                if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Music.AlbumTitle, album);
            }
            catch { }
        }

        public void WriteSubtitle_Description(string description)
        {
            try
            {
                if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Media.Subtitle, description);
            }
            catch { }
        }
        public void WriteSubject_Description(string subject)
        {
            try
            {
                if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Subject, subject);
            }
            catch { }
        }

        public void WriteComment(string comment)
        {
            try
            {
                if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Comment, comment);
            }
            catch { }
        }

        public void WriteDateTaken(DateTime dateTime)
        {
            try
            {
                if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Photo.DateTaken, dateTime);
            }
            catch { }
        }

        public void WriteArtist_Author(string author)
        {
            try
            {
                if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Music.Artist, author);
            }
            catch { }
        }
      
        public void WriteRating(int rating)
        {
            try
            {
                if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Rating, rating);
            }
            catch { }
        }
    }

}
