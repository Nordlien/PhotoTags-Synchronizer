using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileDateTime
{
    public class FileDateTimeReader
    {
        private List<string> allowedFormats;

        public FileDateTimeReader(List<string> allowedFormatList)
        {

            this.allowedFormats = allowedFormatList.OrderByDescending(x => x.Length).ToList();
        }

        public FileDateTimeReader(string allowedFormatsString) : this(
                new List<string>(
                allowedFormatsString.Split(new string[] {
                Environment.NewLine, CultureInfo.CurrentCulture.TextInfo.ListSeparator, CultureInfo.InvariantCulture.TextInfo.ListSeparator, "\t"},
                StringSplitOptions.RemoveEmptyEntries)))
        {
        }

        private int MaxLength()
        {
            int maxLength = 0;
            foreach (string format in allowedFormats)
            {
                if (format.Length > maxLength) maxLength = format.Length;
            }
            return maxLength;
        }

        private int MinLength()
        {
            int minLength = int.MaxValue;
            foreach (string format in allowedFormats)
            {
                if (format.Length < minLength) minLength = format.Length;
            }
            return minLength;
        }

        private bool FoundDateTime(string filename, ref int position, out int length, out DateTime? dateTimeFound)
        {
            dateTimeFound = null;

            int minLength = MinLength();
            length = 0;
            if (string.IsNullOrEmpty(filename) || filename.Length < minLength) return false;
            while (position + minLength < filename.Length)
            {
                foreach (string format in allowedFormats)
                {
                    if (position + format.Length <= filename.Length)
                    {
                        string filenameSubstring = filename.Substring(position, format.Length);

                        //DateTime dateTime;
                        if (DateTime.TryParseExact(filenameSubstring, format, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime result))
                        {
                            dateTimeFound = result;
                            length = format.Length;
                            return true;
                        }
                    }
                }
                position++;
            }
            return false;
        }

        public string RemoveAllDateTimes(string filename, out List<string> whatWasRemoved, out List<DateTime> dateTimesFound)
        {
            DateTime? dateTimeFound;

            string filenameCopy = filename;
            int position = 0;
            int length;
            whatWasRemoved = new List<string>();
            dateTimesFound = new List<DateTime>();

            while (FoundDateTime(filenameCopy, ref position, out length, out dateTimeFound))
            {
                whatWasRemoved.Add(filenameCopy.Substring(position, length));
                if (dateTimeFound!=null) dateTimesFound.Add((DateTime)dateTimeFound);
                filenameCopy = filenameCopy.Remove(position, length);
            }
            return filenameCopy;
        }

        public string RemoveAllDateTimes(string filename)
        {
            //List<string> whatWasRemoved;
            return RemoveAllDateTimes(filename, out _, out _);
        }

        public List<string> ListAllDateTimesFound(string filename)
        {
            List<string> whatWasRemoved;
            _ = RemoveAllDateTimes(filename, out whatWasRemoved, out _);
            return whatWasRemoved;
        }

        public List<DateTime> ListAllDateTimes(string filename)
        {
            List<DateTime> listOfDateTimes;
            _ = RemoveAllDateTimes(filename, out _, out listOfDateTimes);            
            return listOfDateTimes;
        }
    }
}
