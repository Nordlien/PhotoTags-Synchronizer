using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System;

namespace PhotoTagsCommonComponets
{
    public partial class ComboBoxCustom2 : ComboBox
    {
        public enum PenStyles
        {
            PS_SOLID = 0,
            PS_DASH = 1,
            PS_DOT = 2,
            PS_DASHDOT = 3,
            PS_DASHDOTDOT = 4
        }

        public enum ComboBoxButtonState
        {
            STATE_SYSTEM_NONE = 0,
            STATE_SYSTEM_INVISIBLE = 0x00008000,
            STATE_SYSTEM_PRESSED = 0x00000008
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COMBOBOXINFO
        {
            public Int32 cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public ComboBoxButtonState buttonState;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is RECT))
                {
                    return false;
                }
                return this.Equals((RECT)obj);
            }

            public bool Equals(RECT value)
            {
                return this.Left == value.Left &&
                       this.Top == value.Top &&
                       this.Right == value.Right &&
                       this.Bottom == value.Bottom;
            }

            public int Height
            {
                get
                {
                    return Bottom - Top + 1;
                }
            }

            public int Width
            {
                get
                {
                    return Right - Left + 1;
                }
            }

            public Size Size { get { return new Size(Width, Height); } }

            public Point Location { get { return new Point(Left, Top); } }

            // Handy method for converting to a System.Drawing.Rectangle
            public System.Drawing.Rectangle ToRectangle()
            {
                return System.Drawing.Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }

            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }

            public void Inflate(int width, int height)
            {
                this.Left -= width;
                this.Top -= height;
                this.Right += width;
                this.Bottom += height;
            }

            public override int GetHashCode()
            {
                return Left ^ ((Top << 13) | (Top >> 0x13))
                    ^ ((Width << 0x1a) | (Width >> 6))
                    ^ ((Height << 7) | (Height >> 0x19));
            }

            public static implicit operator Rectangle(RECT rect)
            {
                return System.Drawing.Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            public static implicit operator RECT(Rectangle rect)
            {
                return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }

        public ComboBoxCustom2()
        {
            //InitializeComponent();

            // Timer to check when the dropdown is fully visible
            //_dropDownCheck.Interval = 100;
            //_dropDownCheck.Tick += new EventHandler(dropDownCheck_Tick);
        }

        /// <summary>
        /// Override window messages
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // Filter window messages
            switch (m.Msg)
            {
                // Draw a custom color border around the drop down list cintaining popup
                case WM_CTLCOLORLISTBOX:
                    base.WndProc(ref m);
                    DrawNativeBorder(m.LParam);
                    break;

                default: base.WndProc(ref m); break;
            }
        }

        public const int WM_CTLCOLORLISTBOX = 0x0134;
        private Timer _dropDownCheck = new Timer();      // Timer that checks when the drop down is fully visible

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(IntPtr hWnd, ref COMBOBOXINFO pcbi);

        [DllImport("gdi32.dll")]
        public static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(PenStyles enPenStyle, int nWidth, int crColor);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern void Rectangle(IntPtr hdc, int X1, int Y1, int X2, int Y2);

        public static int RGB(int R, int G, int B)
        {
            return (R | (G << 8) | (B << 16));
        }

        /// <summary>
        /// On drop down
        /// </summary>
        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            // Start checking for the dropdown visibility
            _dropDownCheck.Start();
        }

        /// <summary>
        /// Checks when the drop down is fully visible
        /// </summary>
        private void dropDownCheck_Tick(object sender, EventArgs e)
        {
            // If the drop down has been fully dropped
            if (DroppedDown)
            {
                // Stop the time, send a listbox update
                _dropDownCheck.Stop();
                Message m = GetControlListBoxMessage(this.Handle);
                WndProc(ref m);
            }
        }

        /// <summary>
        /// Non client area border drawing
        /// </summary>
        /// <param name="m">The window message to process</param>
        /// <param name="handle">The handle to the control</param>
        public void DrawNativeBorder(IntPtr handle)
        {
            // Define the windows frame rectangle of the control
            RECT controlRect;
            GetWindowRect(handle, out controlRect);
            controlRect.Right -= controlRect.Left; controlRect.Bottom -= controlRect.Top;
            controlRect.Top = controlRect.Left = 0;

            // Get the device context of the control
            IntPtr dc = GetWindowDC(handle);

            // Define the client area inside the control rect
            RECT clientRect = controlRect;
            clientRect.Left += 1;
            clientRect.Top += 1;
            clientRect.Right -= 1;
            clientRect.Bottom -= 1;
            ExcludeClipRect(dc, clientRect.Left, clientRect.Top, clientRect.Right, clientRect.Bottom);

            // Create a pen and select it
            Color borderColor = Color.Magenta;
            IntPtr border = CreatePen(PenStyles.PS_SOLID, 1, RGB(borderColor.R, borderColor.G, borderColor.B));

            // Draw the border rectangle
            IntPtr borderPen = SelectObject(dc, border);
            Rectangle(dc, controlRect.Left, controlRect.Top, controlRect.Right, controlRect.Bottom);
            SelectObject(dc, borderPen);
            DeleteObject(border);

            // Release the device context
            ReleaseDC(handle, dc);
            SetFocus(handle);
        }

        /// <summary>
        /// Creates a default WM_CTLCOLORLISTBOX message
        /// </summary>
        /// <param name="handle">The drop down handle</param>
        /// <returns>A WM_CTLCOLORLISTBOX message</returns>
        public Message GetControlListBoxMessage(IntPtr handle)
        {
            // Force non-client redraw for focus border
            Message m = new Message();
            m.HWnd = handle;
            m.LParam = GetListHandle(handle);
            m.WParam = IntPtr.Zero;
            m.Msg = WM_CTLCOLORLISTBOX;
            m.Result = IntPtr.Zero;
            return m;
        }

        /// <summary>
        /// Gets the list control of a combo box
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static IntPtr GetListHandle(IntPtr handle)
        {
            COMBOBOXINFO info;
            info = new COMBOBOXINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            return GetComboBoxInfo(handle, ref info) ? info.hwndList : IntPtr.Zero;
        }
    }
    public class ComboBoxCustom : ComboBox
    {
        
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        public ComboBoxCustom()
        {
            InitializeComponent();
            //SetStyle(ControlHandlerCommon.ControlStyleCommon, true);
            //DrawMode = DrawMode.OwnerDrawFixed;
            //DropDownStyle = ComboBoxStyle.DropDown;
            //FlatStyle = FlatStyle.Flat;
        }

        private Color borderColor = Color.Gray;
        [DefaultValue(typeof(Color), "Gray")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    Invalidate();
                }
            }
        }
        private Color buttonColor = Color.LightGray;
        [DefaultValue(typeof(Color), "LightGray")]
        public Color ButtonColor
        {
            get { return buttonColor; }
            set
            {
                if (buttonColor != value)
                {
                    buttonColor = value;
                    Invalidate();
                }
            }
        }
        protected override void WndProc(ref Message m)
        {
            /*
            if (m.Msg == WM_PAINT && DropDownStyle != ComboBoxStyle.Simple)
            {
                var clientRect = ClientRectangle;
                var dropDownButtonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
                var outerBorder = new Rectangle(clientRect.Location,
                    new Size(clientRect.Width - 1, clientRect.Height - 1));
                var innerBorder = new Rectangle(outerBorder.X + 1, outerBorder.Y + 1,
                    outerBorder.Width - dropDownButtonWidth - 2, outerBorder.Height - 2);
                var innerInnerBorder = new Rectangle(innerBorder.X + 1, innerBorder.Y + 1,
                    innerBorder.Width - 2, innerBorder.Height - 2);
                var dropDownRect = new Rectangle(innerBorder.Right + 1, innerBorder.Y,
                    dropDownButtonWidth, innerBorder.Height + 1);
                if (RightToLeft == RightToLeft.Yes)
                {
                    innerBorder.X = clientRect.Width - innerBorder.Right;
                    innerInnerBorder.X = clientRect.Width - innerInnerBorder.Right;
                    dropDownRect.X = clientRect.Width - dropDownRect.Right;
                    dropDownRect.Width += 1;
                }
                var innerBorderColor = Enabled ? BackColor : SystemColors.Control;
                var outerBorderColor = Enabled ? BorderColor : SystemColors.ControlDark;
                var buttonColor = Enabled ? ButtonColor : SystemColors.Control;
                var middle = new Point(dropDownRect.Left + dropDownRect.Width / 2,
                    dropDownRect.Top + dropDownRect.Height / 2);
                var arrow = new Point[]
                {
                new Point(middle.X - 3, middle.Y - 2),
                new Point(middle.X + 4, middle.Y - 2),
                new Point(middle.X, middle.Y + 2)
                };
                var ps = new PAINTSTRUCT();
                bool shoulEndPaint = false;
                IntPtr dc;
                if (m.WParam == IntPtr.Zero)
                {
                    dc = BeginPaint(Handle, ref ps);
                    m.WParam = dc;
                    shoulEndPaint = true;
                }
                else
                {
                    dc = m.WParam;
                }
                var rgn = CreateRectRgn(innerInnerBorder.Left, innerInnerBorder.Top,
                    innerInnerBorder.Right, innerInnerBorder.Bottom);
                SelectClipRgn(dc, rgn);
                DefWndProc(ref m);
                DeleteObject(rgn);
                rgn = CreateRectRgn(clientRect.Left, clientRect.Top,
                    clientRect.Right, clientRect.Bottom);
                SelectClipRgn(dc, rgn);
                using (var g = Graphics.FromHdc(dc))
                {
                    using (var b = new SolidBrush(buttonColor))
                    {
                        g.FillRectangle(b, dropDownRect);
                    }
                    using (var b = new SolidBrush(outerBorderColor))
                    {
                        g.FillPolygon(b, arrow);
                    }
                    using (var p = new Pen(innerBorderColor))
                    {
                        g.DrawRectangle(p, innerBorder);
                        g.DrawRectangle(p, innerInnerBorder);
                    }
                    using (var p = new Pen(outerBorderColor))
                    {
                        g.DrawRectangle(p, outerBorder);
                    }
                }
                if (shoulEndPaint)
                    EndPaint(Handle, ref ps);
                DeleteObject(rgn);

                Refresh();
                m.Result = new IntPtr(1);
                return;
            }
            else*/
                base.WndProc(ref m);
        }

        private const int WM_PAINT = 0xF;
        // The WM_SIZE message is sent to a window after its size has changed.
        private const int WM_SIZE = 0x0005; 
        // The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately. 
        private const int WM_ACTIVATE = 0x0006;
        // The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
        private const int WM_SHOWWINDOW = 0x0018;
        // The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
        private const int WM_ACTIVATEAPP = 0x001C;
        private const int WM_GETMINMAXINFO = 0x0024;
        // The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
        private const int WM_WINDOWPOSCHANGING = 0x0046;
        // The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
        private const int WM_WINDOWPOSCHANGED = 0x0047;
        // The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles
        private const int WM_STYLECHANGED = 0x007D;
        // The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
        private const int WM_NCCALCSIZE = 0x0083;
        // The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
        private const int WM_NCHITTEST = 0x0084;
        // The WM_NCPAINT message is sent to a window when its frame must be painted. 
        private const int WM_NCPAINT = 0x0085;
        // The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
        private const int WM_NCACTIVATE = 0x0086;
        // The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        private const int WM_NCMOUSEMOVE = 0x00A0;
        // The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        // The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        private const int WM_NCLBUTTONUP = 0x00A2;
        // A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.
        private const int WM_SYSCOMMAND = 0x0112;
        // The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        private const int WM_LBUTTONDOWN = 0x0201;
        // An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window. 
        private const int WM_MDIACTIVATE = 0x0222;
        // The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
        private const int WM_MOUSELEAVE = 0x02A3;
        // The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
        private const int WM_NCMOUSELEAVE = 0x02A2;
        private const int WM_MOUSEHOVER = 0x2A1;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int L, T, R, B;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public int rcPaint_left;
            public int rcPaint_top;
            public int rcPaint_right;
            public int rcPaint_bottom;
            public bool fRestore;
            public bool fIncUpdate;
            public int reserved1;
            public int reserved2;
            public int reserved3;
            public int reserved4;
            public int reserved5;
            public int reserved6;
            public int reserved7;
            public int reserved8;
        }
        [DllImport("user32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hWnd,
            [In, Out] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

        [DllImport("user32.dll")]
        public static extern int GetUpdateRgn(IntPtr hwnd, IntPtr hrgn, bool fErase);
        public enum RegionFlags
        {
            ERROR = 0,
            NULLREGION = 1,
            SIMPLEREGION = 2,
            COMPLEXREGION = 3,
        }
        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);
    }

    #region ComboBoxCustom
    [ToolboxBitmap(typeof(ComboBox))]
    public class ComboBoxCustom3 : ComboBox
    {
        [DefaultValue(50)]
        public int ColorFieldWidth { get; set; } = 50;

        [DefaultValue(4)]
        public int VerticalItemPadding { get; set; } = 4;

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        public ComboBoxCustom3()
        {
            InitializeComponent();

            SetStyle(ControlHandlerCommon.ControlStyleCommon, true);
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DrawItem += ComboBoxCustom_DrawItem;
            //this.DropDownBorderColor = Color.Red;
            //this.DropDownBackColor = Color.Yellow;

        }


        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);

            if (e.Index > -1)
            {
                Size textSize = e.Graphics.MeasureString(this.Items[e.Index].ToString(), this.Font).ToSize();
                e.ItemHeight = textSize.Height + this.VerticalItemPadding;
                e.ItemWidth = textSize.Width + this.ColorFieldWidth;
            }
        }



        #region Appearance Properties
        private Color dropDownBorderColor = Color.Black;
        [Category("Appearance")]
        [Description("The border color of the drop down list")]
        [DefaultValue(typeof(Color), "Black")]
        public Color DropDownBorderColor
        {
            get { return dropDownBorderColor; }
            set { dropDownBorderColor = value; Invalidate(); }
        }

        private Color dropDownBackColor = SystemColors.Window;
        [Category("Appearance")]
        [Description("The background color of the drop down list")]
        [DefaultValue(typeof(Color), "Window")]
        public Color DropDownBackColor {
            get { return dropDownBackColor; }
            set { dropDownBackColor = value; Invalidate(); }
        }

        Color borderColor = Color.Black;
        [Category("Appearance")]
        [Description("The background color of the combobox")]
        [DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }
        #endregion

        #region ComboBoxCustom_DrawItem
        private void ComboBoxCustom_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit) return;

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
            e.Graphics.DrawString(Items[e.Index].ToString(), this.Font, new SolidBrush(this.ForeColor), new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

            // Draw the focus rectangle if the mouse hovers over an item
            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus) e.DrawFocusRectangle();

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
                end = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top + e.Bounds.Height - 1);
                e.Graphics.DrawLine(borderPen, start, end);

                if (e.Index == Items.Count - 1)
                {
                    //Draw bottom border
                    start = new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height - 1);
                    end = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top + e.Bounds.Height - 1);
                    e.Graphics.DrawLine(borderPen, start, end);
                }
            }
        }
        #endregion 

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            OnPaintBackground(e);
            OnPaintComboButtonBackground(e);
        }

        #region Virtual Events
        protected virtual void OnPaintComboButtonBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            Rectangle rc = ClientRectangle;
            g.FillRectangle(new SolidBrush(Color.Red), rc.Right - 17, rc.Y + 2, 15, rc.Height - 4);
        }
        #endregion

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Fill the Background
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            // Draw DateTime text
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), 5, 2);

            // Draw Border
            e.Graphics.DrawRectangle(Pens.Red, new Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1));
        }


    }
    #endregion

}


