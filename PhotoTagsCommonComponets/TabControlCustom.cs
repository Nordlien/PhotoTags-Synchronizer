using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;

namespace PhotoTagsSynchronizer
{
    
    // Edit completing Moses comment
    public class Win32
    {
        public const int WM_MOUSEFIRST = 0x0200;
        public const int WM_MOUSELEAVE = 0x02A3;
        public const int WM_MOUSEHOVER = 0x02A1;
        public const int WM_PAINT = 0x000F;
        [DllImport("user32")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprc, IntPtr hrgn, uint flags);
    }

    #region ControlHandlerCommon
    public class ControlHandlerCommon
    {
        public const int WS_EX_COMPOSITED = 0x02000000;

        public const int TCN_FIRST = 0 - 550;
        public const int TCN_SELCHANGING = (TCN_FIRST - 2);

        public const int WM_USER = 0x400;
        public const int WM_NOTIFY = 0x4E;
        public const int WM_REFLECT = WM_USER + 0x1C00;
        public const int WM_PAINT = 0xF;

        /*
        enum ControlStylesEnum
        {
            AllPaintingInWmPaint = 8192, //If true, the control ignores the window message WM_ERASEBKGND to reduce flicker.This style should only be applied if the UserPaint bit is set to true.
            CacheText = 16384, //If true, the control keeps a copy of the text rather than getting it from the Handle each time it is needed.This style defaults to false. This behavior improves performance, but makes it difficult to keep the text synchronized.
            ContainerControl = 1, //If true, the control is a container-like control.
            DoubleBuffer = 65536, //If true, drawing is performed in a buffer, and after it completes, the result is output to the screen. Double-buffering prevents flicker caused by the redrawing of the control. If you set DoubleBuffer to true, you should also set UserPaint and AllPaintingInWmPaint to true.
            EnableNotifyMessage = 32768, //If true, the OnNotifyMessage(Message) method is called for every message sent to the control's WndProc(Message). This style defaults to false. EnableNotifyMessage does not work in partial trust.
            FixedHeight = 64, //If true, the control has a fixed height when auto - scaled.For example, if a layout operation attempts to rescale the control to accommodate a new Font, the control's Height remains unchanged.
            FixedWidth = 32, //If true, the control has a fixed width when auto - scaled.For example, if a layout operation attempts to rescale the control to accommodate a new Font, the control's Width remains unchanged.
            Opaque = 4, //If true, the control is drawn opaque and the background is not painted.
            OptimizedDoubleBuffer = 131072, //If true, the control is first drawn to a buffer rather than directly to the screen, which can reduce flicker.If you set this property to true, you should also set the AllPaintingInWmPaint to true.
            ResizeRedraw = 16, //If true, the control is redrawn when it is resized.
            Selectable = 512, //If true, the control can receive focus.
            StandardClick = 256, //If true, the control implements the standard Click behavior.
            StandardDoubleClick = 4096, //If true, the control implements the standard DoubleClick behavior.This style is ignored if the StandardClick bit is not set to true.
            SupportsTransparentBackColor = 2048, //If true, the control accepts a BackColor with an alpha component of less than 255 to simulate transparency.Transparency will be simulated only if the UserPaint bit is set to true and the parent control is derived from Control.
            UserMouse = 1024, //If true, the control does its own mouse processing, and mouse events are not handled by the operating system.
            UserPaint = 2, //If true, the control paints itself rather than the operating system doing so.If false, the Paint event is not raised. This style only applies to classes derived from Control.
            UseTextForAccessibility = 262144, //Specifies that the value of the control's Text property, if set, determines the control's default Active Accessibility name and shortcut key.
        }*/

        public static ControlStyles ControlStyleCommon =
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer |
            //ControlStyles.CacheText |
            //ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.FixedHeight |
            //ControlStyles.FixedWidth |
            //ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint;
    }
    #endregion

    #region ComboBoxCustom
    public class ComboBoxCustom2 : ComboBox
    {
        /*
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Console.WriteLine("Text:" + this.Text);
            Console.WriteLine("Text:" + base.Text);
            Console.WriteLine("Rect:" + e.ClipRectangle.ToString());

            base.OnPaintBackground(e);
            using (var brush = new SolidBrush(Color.Yellow))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
                e.Graphics.DrawRectangle(Pens.DarkGray, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
            }
            base.OnPaintBackground(e);
        }
        */

        #region Container components
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        #endregion

        #region Dispose components
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion 
        
        #region Initialize components - Component Designer generated code
        /// <summary> 
        /// Required method for Designer support
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        protected override void InitLayout()
        {
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            //base.InitLayout();
            //DropDownStyle = ComboBoxStyle.DropDownList; //Works only in DropDownList          
        }

        #region Constructor 
        public ComboBoxCustom2()
        {
            InitializeComponent(); // This call is required by the Windows.Forms Form Designer.
            BorderColor = Color.Black;           
            SetStyle(ControlHandlerCommon.ControlStyleCommon, true);
            SetStyle(ControlStyles.Opaque, false); //If true, the control is drawn opaque and the background is not painted.
            //SetStyle(ControlStyles.Selectable, false); //to recive focuse if true
        }
        #endregion

        /*
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= ControlHandlerCommon.WS_EX_COMPOSITED;
                return handleParam;
            }
        }
        */

        #region OnResize
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
        #endregion 

        #region OnParentBackColorChanged
        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }
        #endregion

        #region OnSelectedIndexChanged
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }
        #endregion
        

        /*#region BorderColor Manipulation
        private Color borderColor = Color.Empty;
        [Browsable(true), Description("The background color used to display text and graphics in a control.")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                Invalidate();
                //base.OnBackColorChanged(EventArgs.Empty); //Let the Tabpages know that the border color has changed.
            }
        }
        #endregion*/

        private const int WM_PAINT = 0xF;
        private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
        Color borderColor = Color.Blue;
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        protected override void WndProc(ref Message m)
        {
            /*if (m.Msg == Win32.WM_MOUSEFIRST || m.Msg == Win32.WM_MOUSELEAVE || m.Msg == Win32.WM_MOUSEHOVER) //0x0200, 0x02A3, 0x02A1
            {
                m.Result = (IntPtr)1;
                return;
            }*/
            /*else base.WndProc(ref m); */
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT && DropDownStyle != ComboBoxStyle.Simple)
            {
                
                using (var g = Graphics.FromHwnd(Handle))
                {
                    var adjustMent = 0;
                    if (FlatStyle == FlatStyle.Popup ||
                       (FlatStyle == FlatStyle.Flat &&
                       DropDownStyle == ComboBoxStyle.DropDownList))
                        adjustMent = 1;

                    var innerBorderWisth = 3;
                    var innerBorderColor = BackColor;
                    if (DropDownStyle == ComboBoxStyle.DropDownList &&
                        (FlatStyle == FlatStyle.System || FlatStyle == FlatStyle.Standard))
                        innerBorderColor = Color.FromArgb(0xCCCCCC);
                    
                    if (DropDownStyle == ComboBoxStyle.DropDown && !Enabled)
                        innerBorderColor = SystemColors.Control;

                    if (DropDownStyle == ComboBoxStyle.DropDown || Enabled == false)
                    {
                        using (var p = new Pen(innerBorderColor, innerBorderWisth))
                        {
                            p.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                            g.DrawRectangle(p, 1, 1,
                                Width - buttonWidth - adjustMent - 1, Height - 1);
                        }
                    }
                    using (var p = new Pen(BorderColor))
                    {
                        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                        g.DrawLine(p, Width - buttonWidth - adjustMent,
                            0, Width - buttonWidth - adjustMent, Height);
                    }
                }
                //m.Result = new IntPtr(1);
                //return;
            }
            
        }

        /*
        #region WndProc
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            
            if (m.Msg == ControlHandlerCommon.WM_PAINT)
            {
                using (var g = Graphics.FromHwnd(Handle))
                {
                    // Uncomment this if you don't want the "highlight border".
                    using (var p = new Pen(this.BorderColor, 1))
                    {
                        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                    }
                    /*
                    using (var p = new Pen(this.BorderColor, 2))
                    {
                        g.DrawRectangle(p, 2, 2, Width - buttonWidth - 4, Height - 4);
                    }
                    * /
                }
            }
        }
        #endregion
        */
    }

    [ToolboxBitmap(typeof(ComboBox))]
    public class ComboBoxCustom : ComboBox
    {
        public ComboBoxCustom()
        {
            //this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            //this.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            //this.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DrawItem += ComboBoxEx_DrawItem;
            this.DropDownBorderColor = Color.Red;
            this.DropDownBackColor = Color.Yellow;
        }

        [Category("Appearance")]
        [Description("The border color of the drop down list")]
        [DefaultValue(typeof(Color), "Red")]
        public Color DropDownBorderColor { get; set; }

        [Category("Appearance")]
        [Description("The background color of the drop down list")]
        [DefaultValue(typeof(Color), "Yellow")]
        public Color DropDownBackColor { get; set; }

        Color borderColor = Color.Blue;
        [Category("Appearance")]
        [Description("The background color of the drop down list")]
        [DefaultValue(typeof(Color), "Yellow")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        private void ComboBoxEx_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
                return;

            // Draw the background of the item
            if (
                ((e.State & DrawItemState.Focus) == DrawItemState.Focus) ||
                ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ||
                ((e.State & DrawItemState.HotLight) == DrawItemState.HotLight)
               )
            {
                e.DrawBackground();
            }
            else
            {
                using (Brush backgroundBrush = new SolidBrush(DropDownBackColor))
                {
                    e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                }
            }

            //Draw item text
            e.Graphics.DrawString(Items[e.Index].ToString(), this.Font, Brushes.Black,
              new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

            // Draw the focus rectangle if the mouse hovers over an item
            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();

            //Draw the border around the whole DropDown area
            using (Pen borderPen = new Pen(DropDownBorderColor, 1))
            {
                Point start;
                Point end;

                if (e.Index == 0)
                {
                    //Draw top border
                    start = new Point(e.Bounds.Left, e.Bounds.Top);
                    end = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top);
                    e.Graphics.DrawLine(borderPen, start, end);
                }

                //Draw left border
                start = new Point(e.Bounds.Left, e.Bounds.Top);
                end = new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height - 1);
                e.Graphics.DrawLine(borderPen, start, end);

                //Draw Right border
                start = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top);
                end = new Point(e.Bounds.Left + e.Bounds.Width - 1,
                                e.Bounds.Top + e.Bounds.Height - 1);
                e.Graphics.DrawLine(borderPen, start, end);

                if (e.Index == Items.Count - 1)
                {
                    //Draw bottom border
                    start = new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height - 1);
                    end = new Point(e.Bounds.Left + e.Bounds.Width - 1,
                                    e.Bounds.Top + e.Bounds.Height - 1);
                    e.Graphics.DrawLine(borderPen, start, end);
                }
            }
        }
    }
    #endregion 
    public class TextBoxCustom : TextBox
    {

    } 

    #region class TabControlCustom
    /// <summary>
    /// Summary description for TabControl.
    /// </summary>
    public class TabControlCustom : System.Windows.Forms.TabControl
    {
        #region Container
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        #endregion 

        #region Dispose
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion 

        #region InitializeComponent - Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region Constructor - TabControlCustom
        public TabControlCustom()
        {
            InitializeComponent(); // This call is required by the Windows.Forms Form Designer.
            SetStyle(ControlHandlerCommon.ControlStyleCommon, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }
        #endregion 

        #region Interop
        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr HWND;
            public uint idFrom;
            public int code;
            public override String ToString()
            {
                return String.Format("Hwnd: {0}, ControlID: {1}, Code: {2}", HWND, idFrom, code);
            }
        }

        
        #endregion

        #region OnParentBackColorChanged
        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }
        #endregion

        #region OnSelectedIndexChanged
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }
        #endregion

        #region OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(BackColor);
            Rectangle r = ClientRectangle;
            if (TabCount <= 0) return;

            //Draw a custom background for Transparent TabPages
            r = SelectedTab.Bounds;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Font DrawFont = new Font(Font.FontFamily, 24, FontStyle.Regular, GraphicsUnit.Pixel);
            ControlPaint.DrawStringDisabled(e.Graphics, "", DrawFont, BackColor, (RectangleF)r, sf);
            DrawFont.Dispose();

            //Draw a border around TabPage
            r.Inflate(3, 3);
            TabPage tp = TabPages[SelectedIndex];
            SolidBrush PaintBrush = new SolidBrush(tp.BackColor);
            e.Graphics.FillRectangle(PaintBrush, r);
            //ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, ButtonBorderStyle.Outset); //Don't paint the border
            
            //Draw the Tabs
            for (int index = 0; index <= TabCount - 1; index++)
            {
                tp = TabPages[index];
                r = GetTabRect(index);
                ButtonBorderStyle bs = ButtonBorderStyle.Outset;
                if (index == SelectedIndex) bs = ButtonBorderStyle.Inset;
                PaintBrush.Color = tp.BackColor;
                e.Graphics.FillRectangle(PaintBrush, r);
                ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, bs);
                PaintBrush.Color = tp.ForeColor;

                //Set up rotation for left and right aligned tabs
                if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right)
                {
                    float RotateAngle = 90;
                    if (Alignment == TabAlignment.Left) RotateAngle = 270;
                    PointF cp = new PointF(r.Left + (r.Width >> 1), r.Top + (r.Height >> 1));
                    e.Graphics.TranslateTransform(cp.X, cp.Y);
                    e.Graphics.RotateTransform(RotateAngle);
                    r = new Rectangle(-(r.Height >> 1), -(r.Width >> 1), r.Height, r.Width);
                }
                //Draw the Tab Text
                if (tp.Enabled) e.Graphics.DrawString(tp.Text, Font, PaintBrush, (RectangleF)r, sf);
                else ControlPaint.DrawStringDisabled(e.Graphics, tp.Text, Font, tp.BackColor, (RectangleF)r, sf);

                e.Graphics.ResetTransform();
            }

            PaintBrush.Dispose();
        }
        #endregion 

        #region BackColor Manipulation
        //As well as exposing the property to the Designer we want it to behave just like any other 
        //controls BackColor property so we need some clever manipulation.
        private Color m_Backcolor = Color.Empty;
        [Browsable(true), Description("The background color used to display text and graphics in a control.")]
        public override Color BackColor
        {
            get
            {
                if (m_Backcolor.Equals(Color.Empty))
                {
                    if (Parent == null) return Control.DefaultBackColor;
                    else return Parent.BackColor;
                }
                return m_Backcolor;
            }
            set
            {
                if (m_Backcolor.Equals(value)) return;
                m_Backcolor = value;
                Invalidate();
                //Let the Tabpages know that the backcolor has changed.
                base.OnBackColorChanged(EventArgs.Empty);
            }
        }
        public bool ShouldSerializeBackColor()
        {
            return !m_Backcolor.Equals(Color.Empty);
        }
        public override void ResetBackColor()
        {
            m_Backcolor = Color.Empty;
            Invalidate();
        }
        #endregion

        #region event SelectedTabPageChangeEventHandler SelectedIndexChanging
        [Description("Occurs as a tab is being changed.")]
        public event SelectedTabPageChangeEventHandler SelectedIndexChanging;
        #endregion

        #region WndProc
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (ControlHandlerCommon.WM_REFLECT + ControlHandlerCommon.WM_NOTIFY))
            {
                NMHDR hdr = (NMHDR)(Marshal.PtrToStructure(m.LParam, typeof(NMHDR)));
                if (hdr.code == ControlHandlerCommon.TCN_SELCHANGING)
                {
                    TabPage tp = TestTab(PointToClient(Cursor.Position));
                    if (tp != null)
                    {
                        TabPageChangeEventArgs e = new TabPageChangeEventArgs(SelectedTab, tp);
                        if (SelectedIndexChanging != null) SelectedIndexChanging(this, e);
                        if (e.Cancel || tp.Enabled == false)
                        {
                            m.Result = new IntPtr(1);
                            return;
                        }
                    }
                }
            }
            base.WndProc(ref m);
        }        
        #endregion

        #region TabPage TestTab(Point pt)
        private TabPage TestTab(Point pt)
        {
            for (int index = 0; index <= TabCount - 1; index++)
            {
                if (GetTabRect(index).Contains(pt.X, pt.Y)) return TabPages[index];
            }
            return null;
        }
        #endregion
    }
    #endregion

    #region TabPageChangeEventArgs
    public class TabPageChangeEventArgs : EventArgs
    {
        private TabPage _Selected = null;
        private TabPage _PreSelected = null;
        public bool Cancel = false;

        public TabPage CurrentTab
        {
            get { return _Selected; }
        }


        public TabPage NextTab
        {
            get { return _PreSelected; }
        }

        public TabPageChangeEventArgs(TabPage CurrentTab, TabPage NextTab)
        {
            _Selected = CurrentTab;
            _PreSelected = NextTab;
        }
    }

    public delegate void SelectedTabPageChangeEventHandler(Object sender, TabPageChangeEventArgs e);
    #endregion

}


