using System;


namespace Exiftool
{
    public struct ExiftoolWarningData
    {
        public ExiftoolData OldExiftoolData { get; set; }
        public ExiftoolData NewExiftoolData { get; set; }
        public string WarningMessage { get; set; }
    }



}
