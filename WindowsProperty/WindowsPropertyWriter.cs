using System;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace WindowsProperty
{

    public class WindowsPropertyWriter : IDisposable
    {
        private ShellFile shellFile;
        private ShellPropertyWriter propertyWriter;
        
        public WindowsPropertyWriter(string fullFileName)
        {
            shellFile = ShellFile.FromFilePath(fullFileName);
            propertyWriter = shellFile.Properties.GetPropertyWriter();
        }

        public void Dispose()
        {
            if (propertyWriter != null) propertyWriter.Dispose();
            propertyWriter = null;
            shellFile.Dispose();
            shellFile = null;
            //shellProperty = null;
        }

        public void Close()
        {
            if (propertyWriter != null) propertyWriter.Close();
        }

        public void WriteKeywords(string tags)
        {
            if (propertyWriter!=null) propertyWriter.WriteProperty(SystemProperties.System.Keywords, tags);
        }

        public void WriteNames(string names)
        {
            if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Photo.PeopleNames, names);
        }

        public void WriteCategories(string categories)
        {
            if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Category, categories);
        }


        public void WriteAlbum(string album)
        {
            if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Music.AlbumTitle, album);
        }

        public void WriteSubtitle_Description(string description)
        {
            if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Media.Subtitle, description);
        }
        public void WriteSubject_Description(string subject)
        {
            if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Subject, subject);
        }

        public void WriteComment(string comment)
        {
            if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Comment, comment);
        }

        public void WriteDateTaken(DateTime dateTime)
        {
            if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Photo.DateTaken, dateTime);
        }

        public void WriteArtist_Author(string author)
        {
            if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Music.Artist, author);
        }

        public void WriteRating(int rating)
        {
            if (propertyWriter != null) propertyWriter.WriteProperty(SystemProperties.System.Rating, rating);
        }
    }

}
