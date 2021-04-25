using System;
using System.Drawing;

namespace PhotoTagsSynchronizer
{
    public class RegionSelectedEventArgs : EventArgs
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public RectangleF Selection { get; set; }
        public Size ImageSize { get; set; }
    }
}
