using System;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public class WaitCursor : IDisposable
    {
        private static int MouseDepth { get; set; } = 0;

        #region WaitCursor()
        public WaitCursor()
        {
            if (WaitCursor.MouseDepth++ == 0)
            {
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;

                System.Windows.Forms.Application.OpenForms[0].Refresh();
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (--WaitCursor.MouseDepth > 0) return;
            Application.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
        }
        #endregion
    }
}
