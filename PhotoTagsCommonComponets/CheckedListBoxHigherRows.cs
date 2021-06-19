using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public sealed class CheckedListBoxHigherRows : CheckedListBox
    {
        public CheckedListBoxHigherRows()
        {
            ItemHeight = 26;
        }
        public override int ItemHeight { get; set; }
    }
}
