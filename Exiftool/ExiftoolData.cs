using System;
using System.Collections.Generic;

namespace Exiftool
{

    public class ExiftoolData
    {
        public string FileName { get; set; }
        public string FileDirectory { get; set; }
        public DateTime FileDateModified { get; set; }
        public string Region { get; set; }
        public string Command { get; set; }
        public string Parameter { get; set; }
        public string FullFilePath { get { return System.IO.Path.Combine(FileDirectory, FileName); } }
        public bool HasValueSet { get; set; }
        private object tristateValue;

        public object TristateValue
        {
            get { return tristateValue; }
            set
            {
                tristateValue = value;
                HasValueSet = true;
            }
        }

        public ExiftoolData()
        {
            this.FileName = null;
            this.FileDirectory = null;
            this.FileDateModified = DateTime.MinValue;
            this.Region = null;
            this.Command = null;
            this.Parameter = null;
            this.TristateValue = null;
            this.HasValueSet = false; 
        }

        public ExiftoolData(ExiftoolData exifToolData)
        {
            this.FileName = exifToolData.FileName;
            this.FileDirectory = exifToolData.FileDirectory;
            this.FileDateModified = exifToolData.FileDateModified;
            this.Region = exifToolData.Region;
            this.Command = exifToolData.Command;
            this.Parameter = exifToolData.Parameter;
            this.TristateValue = exifToolData.TristateValue;
            this.HasValueSet = exifToolData.HasValueSet;
        }

        public ExiftoolData(ExiftoolData exifToolData, object newTristateValue, bool hasValueSet)
        {
            this.FileName = exifToolData.FileName;
            this.FileDirectory = exifToolData.FileDirectory;
            this.FileDateModified = exifToolData.FileDateModified;
            this.Region = exifToolData.Region;
            this.Command = exifToolData.Command;
            this.Parameter = exifToolData.Parameter;
            this.TristateValue = newTristateValue;
            this.HasValueSet = hasValueSet;
        }

        public ExiftoolData(string fileName, string fileDirectory, DateTime fileDateModified, string region, string command, string parameter, object tristateValue)
        {
            this.FileName = fileName;
            this.FileDirectory = fileDirectory;
            this.FileDateModified = fileDateModified;
            this.Region = region;
            this.Command = command;
            this.Parameter = parameter;
            this.TristateValue = tristateValue;
        }

        public override bool Equals(object obj)
        {
            return obj is ExiftoolData data &&
                FileName == data.FileName &&
                FileDirectory == data.FileDirectory &&
                FileDateModified == data.FileDateModified &&
                Region == data.Region &&
                Command == data.Command;
        }

        public override int GetHashCode()
        {
            int hashCode = -1041223139;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FileName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FileDirectory);
            hashCode = hashCode * -1521134295 + FileDateModified.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Region);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Command);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Parameter);
            return hashCode;
        }

        public static bool operator ==(ExiftoolData left, ExiftoolData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExiftoolData left, ExiftoolData right)
        {
            return !(left == right);
        }
    }

}
