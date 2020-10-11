using System;
using MetadataLibrary;

namespace PipeMessage
{
    [Serializable]
    public class PipeMessageCommand
    {
        private string command;
        private string fullFileName;
        private string message;
        private Metadata metadata;

        public string Command { get => command; set => command = value; }
        public string FullFileName { get => fullFileName; set => fullFileName = value; }
        public string Message { get => message; set => message = value; }
        public Metadata Metadata { get => metadata; set => metadata = value; }
    }
}
