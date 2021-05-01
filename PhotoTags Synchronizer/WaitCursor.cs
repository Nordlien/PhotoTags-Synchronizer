using System;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public class WaitCursor : IDisposable
    {
        public WaitCursor()
        {
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Application.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
        }

        #endregion
    }
}
