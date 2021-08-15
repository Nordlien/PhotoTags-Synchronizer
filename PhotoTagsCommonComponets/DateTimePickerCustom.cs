using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PhotoTagsCommonComponets
{
    public class DateTimePickerCustom : DateTimePicker
    {
        private Image calendarImage;
        private Image calendarImageWhite;
        public DateTimePickerCustom()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            calendarImage = PhotoTagsCommonComponets.Properties.Resources.Calendar;
            calendarImageWhite = PhotoTagsCommonComponets.Properties.Resources.CalendarWhite;
        }

        private Color borderColor = Color.DeepSkyBlue;
        [DefaultValue(typeof(Color), "RoyalBlue")]
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
        UpDownRenderer updownRenderer;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT)
            {
                var info = new DATETIMEPICKERINFO();
                info.cbSize = Marshal.SizeOf(info);
                SendMessage(Handle, DTM_GETDATETIMEPICKERINFO, IntPtr.Zero, ref info);

                var clientRect = new Rectangle(0, 0, Width, Height);
                var dropDownButtonRect = new Rectangle(info.rcButton.L, info.rcButton.T,
                   info.rcButton.R - info.rcButton.L, clientRect.Height);
                var dropDownRect = dropDownButtonRect;
                var imageRect = Rectangle.Empty;
                if (info.rcButton.R - info.rcButton.L > SystemInformation.HorizontalScrollBarArrowWidth)
                {
                    var w = dropDownButtonRect.Width / 2;
                    imageRect = dropDownButtonRect;
                    imageRect.Width = w;

                    dropDownRect.X += w;
                    dropDownRect.Width = w;
                }
                var checkBoxRect = new Rectangle(info.rcCheck.L, info.rcCheck.T,
                   info.rcCheck.R - info.rcCheck.L, clientRect.Height);
                var innerRect = new Rectangle(checkBoxRect.Right + 1, 1,
                    clientRect.Width - dropDownButtonRect.Width - checkBoxRect.Width - (ShowCheckBox ? 3 : 1),
                    clientRect.Height - 2);
                if (RightToLeft == RightToLeft.Yes && RightToLeftLayout == true)
                {
                    dropDownButtonRect.X = clientRect.Width - dropDownButtonRect.Right;
                    dropDownButtonRect.Width += 1;

                    innerRect.X -= clientRect.Width - innerRect.X;
                    innerRect.Width += 1;

                    imageRect.X = clientRect.Width - imageRect.Right;

                    dropDownRect.X = clientRect.Width - dropDownRect.Right;
                    dropDownRect.Width += 1;
                }

                var middle = new Point(dropDownRect.Left + dropDownRect.Width / 2,
                    dropDownRect.Top + dropDownRect.Height / 2);
                var arrow = new Point[]
                {
                        new Point(middle.X - 3, middle.Y - 2),
                        new Point(middle.X + 4, middle.Y - 2),
                        new Point(middle.X, middle.Y + 2)
                };

                var borderColor = Enabled ? BorderColor : Color.LightGray;


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

                var rgn = CreateRectRgn(innerRect.Left, innerRect.Top, innerRect.Right, innerRect.Bottom);
                SelectClipRgn(dc, rgn);
                DefWndProc(ref m);
                DeleteObject(rgn);
                rgn = CreateRectRgn(clientRect.Left, clientRect.Top, clientRect.Right, clientRect.Bottom);
                SelectClipRgn(dc, rgn);

                using (var g = Graphics.FromHdc(dc))
                {
                    if (ShowCheckBox)
                    {
                        var r = Rectangle.FromLTRB(info.rcCheck.L, info.rcCheck.T,
                            info.rcCheck.R, info.rcCheck.B);
                        if (Checked)
                            DrawFrameControl(dc, ref info.rcCheck, DFC_MENU, DFCS_MENUCHECK);
                        else
                            g.FillRectangle(SystemBrushes.Window, r);
                        if (!Enabled)
                        {
                            using (var b = new SolidBrush(Color.FromArgb(200, SystemColors.Control)))
                                g.FillRectangle(b, r);
                        }
                        var r2 = r;
                        r2.Width -= 1; r2.Height -= 1;
                        g.DrawRectangle(Enabled ? SystemPens.WindowText : SystemPens.GrayText, r2);

                        r2.Inflate(1, 1);
                        g.DrawRectangle(Pens.White, r2);
                    }

                    if (ShowUpDown)
                    {
                        if (updownRenderer == null && info.hwndUD != IntPtr.Zero)
                            updownRenderer = new UpDownRenderer(info.hwndUD);
                        updownRenderer.ButtonColor = this.BorderColor;
                        updownRenderer.ArrowColor = Color.Black;
                    }
                    else
                    {
                        var buttonColor = Enabled ? BorderColor : Color.LightGray;
                        var arrorColor = Color.Black;
                        var imageToRender = calendarImage;
                        if (dropDownButtonRect.Contains(PointToClient(Cursor.Position)))
                        {
                            arrorColor = Color.White;
                            imageToRender = calendarImageWhite;
                        }
                        using (var brush = new SolidBrush(buttonColor))
                            g.FillRectangle(brush, dropDownButtonRect);
                        if (imageRect != Rectangle.Empty)
                            g.DrawImage(imageToRender,
                                imageRect.Left + ((imageRect.Width - imageToRender.Width) / 2),
                                imageRect.Top + ((imageRect.Height - imageToRender.Height) / 2),
                                calendarImage.Width,
                                calendarImage.Height);
                        using (var brush = new SolidBrush(arrorColor))
                            g.FillPolygon(brush, arrow);
                    }
                    using (var pen = new Pen(borderColor))
                        g.DrawRectangle(pen, 0, 0,
                            ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                }
                if (shoulEndPaint)
                    EndPaint(Handle, ref ps);
                DeleteObject(rgn);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private class UpDownRenderer : NativeWindow
        {
            public UpDownRenderer(IntPtr hwnd)
            {
                this.AssignHandle(hwnd);
                SetWindowTheme(Handle, string.Empty, string.Empty);
            }
            private bool Enabled => IsWindowEnabled(Handle);
            private Point PointToClient(Point p)
            {
                var pt = new POINT() { X = p.X, Y = p.Y };
                ScreenToClient(Handle, ref pt);
                return new Point(pt.X, pt.Y);
            }
            private Rectangle ClientRectangle
            {
                get
                {
                    var r = new RECT();
                    GetWindowRect(Handle, ref r);
                    return new Rectangle(0, 0, r.R - r.L, r.B - r.T);
                }
            }
            public Color ButtonColor { get; set; }
            public Color ArrowColor { get; set; }
            private Point[] GetDownArrow(Rectangle r)
            {
                var middle = new Point(r.Left + r.Width / 2, r.Top + r.Height / 2);
                return new Point[]
                {
                new Point(middle.X - 3, middle.Y - 1),
                new Point(middle.X + 4, middle.Y - 1),
                new Point(middle.X, middle.Y + 3)
                };
            }
            private Point[] GetUpArrow(Rectangle r)
            {
                var middle = new Point(r.Left + r.Width / 2, r.Top + r.Height / 2);
                return new Point[]
                {
                new Point(middle.X - 4, middle.Y + 2),
                new Point(middle.X + 4, middle.Y + 2),
                new Point(middle.X, middle.Y - 3)
                };
            }
            protected override void WndProc(ref Message m)
            {
                var enabled = Enabled;
                var clientRectangle = ClientRectangle;
                var backColor = enabled ? ButtonColor : Color.LightGray;
                if (m.Msg == WM_MOUSEMOVE)
                {
                    var r = clientRectangle;
                    var rect = new RECT() { L = r.Left, T = r.Top, R = r.Right, B = r.Bottom };
                    InvalidateRect(Handle, ref rect, false);
                }
                if (m.Msg == WM_PAINT)
                {
                    var s = new PAINTSTRUCT();
                    BeginPaint(Handle, ref s);

                    using (var g = Graphics.FromHdc(s.hdc))
                    {
                        var context = BufferedGraphicsManager.Current;
                        using (var bg = context.Allocate(g, clientRectangle))
                        {
                            var gg = bg.Graphics;
                            var r1 = new Rectangle(0, 0,
                                clientRectangle.Width, clientRectangle.Height / 2);
                            var r2 = new Rectangle(0, clientRectangle.Height / 2,
                                clientRectangle.Width, clientRectangle.Height / 2 + 1);
                            using (var backBrush = new SolidBrush(backColor))
                            {
                                gg.FillRectangle(backBrush, r1);
                                gg.FillRectangle(backBrush, r2);
                            }
                            gg.DrawLine(Pens.White, r1.Left, r1.Bottom, r1.Right, r1.Bottom);
                            gg.DrawLine(Pens.White, r2.Left, r2.Top, r2.Right, r2.Top);

                            if (r1.Contains(PointToClient(Cursor.Position)))
                                gg.FillPolygon(Brushes.White, GetUpArrow(r1));
                            else
                                gg.FillPolygon(Brushes.Black, GetUpArrow(r1));

                            if (r2.Contains(PointToClient(Cursor.Position)))
                                gg.FillPolygon(Brushes.White, GetDownArrow(r2));
                            else
                                gg.FillPolygon(Brushes.Black, GetDownArrow(r2));
                            bg.Render(g);
                        }
                    }
                    m.Result = (IntPtr)1;
                    base.WndProc(ref m);
                    EndPaint(Handle, ref s);
                }
                else
                    base.WndProc(ref m);
            }
        }

        const int WM_PAINT = 0xF;
        const int DTM_FIRST = 0x1000;
        const int WM_ERASEBKGND = 0x14;
        const int DTM_GETDATETIMEPICKERINFO = DTM_FIRST + 14;
        const int WM_MOUSEMOVE = 0x0200;


        const int DFC_MENU = 2;
        const int DFCS_MENUCHECK = 0x0001;
        [DllImport("user32.dll")]
        static extern bool DrawFrameControl(IntPtr hDC, ref RECT rect, int type, int state);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg,
            IntPtr wParam, ref DATETIMEPICKERINFO info);
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg,
        IntPtr wParam, IntPtr info);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int L, T, R, B;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int X, Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DATETIMEPICKERINFO
        {
            public int cbSize;
            public RECT rcCheck;
            public int stateCheck;
            public RECT rcButton;
            public int stateButton;
            public IntPtr hwndEdit;
            public IntPtr hwndUD;
            public IntPtr hwndDropDown;
        }
        [DllImport("user32.dll")]
        private static extern bool IsWindowEnabled(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);
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

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hWnd, [In, Out] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

        [DllImport("user32.dll")]
        static extern int ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        static extern int InvalidateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);
    }
}
