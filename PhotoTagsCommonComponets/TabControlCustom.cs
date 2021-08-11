using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace PhotoTagsSynchronizer
{
    public class TabControlCustom : TabControl
    {
        private const int TCM_ADJUSTRECT = 0x1328;

        protected override void WndProc(ref Message m)
        {
            //Hide the tab headers at run-time
            if (m.Msg == TCM_ADJUSTRECT)
            {

                RECT rect = (RECT)(m.GetLParam(typeof(RECT)));
                rect.Left = this.Left - this.Margin.Left;
                rect.Right = this.Right + this.Margin.Right-2;

                rect.Top = this.Top - this.Margin.Top+2;
                rect.Bottom = this.Bottom + this.Margin.Bottom-1;
                Marshal.StructureToPtr(rect, m.LParam, true);
                //m.Result = (IntPtr)1;
                //return;
            }
            //else
            // call the base class implementation
            base.WndProc(ref m);
        }

        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        public TabControlCustom()
        {
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
        }

        /*private Dictionary<TabPage, Color> TabColors = new Dictionary<TabPage, Color>();
        private void SetTabHeader(TabPage page, Color color)
        {
            TabColors[page] = color;
            tabControl1.Invalidate();
        }*/

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            //e.DrawBackground();
            using (Brush brush = new SolidBrush(this.TabPages[e.Index].BackColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
                SizeF sz = e.Graphics.MeasureString(this.TabPages[e.Index].Text, e.Font);
                e.Graphics.DrawString(
                    this.TabPages[e.Index].Text, e.Font, new SolidBrush(this.TabPages[e.Index].ForeColor), 
                    e.Bounds.Left + (e.Bounds.Width - sz.Width) / 2, e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1);

                Rectangle rect = e.Bounds;
                rect.Offset(0, 1);
                rect.Inflate(0, -1);
                e.Graphics.DrawRectangle(Pens.DarkGray, rect);
                e.DrawFocusRectangle();
            }
        }
    }
}
