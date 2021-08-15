using System.Windows.Forms;

namespace PhotoTagsCommonComponets
{
    #region ControlHandlerCommon
    public class ControlHandlerCommon
    {
        public const int WS_EX_COMPOSITED = 0x02000000;

        public const int TCN_FIRST = 0 - 550;
        public const int TCN_SELCHANGING = (TCN_FIRST - 2);

        public const int WM_NOTIFY = 0x4E;
        public const int WM_REFLECT = WM_USER + 0x1C00;
        public const int WM_PAINT = 0xF;
        public const int WM_MOUSEFIRST = 0x0200;
        public const int WM_MOUSELEAVE = 0x02A3;
        public const int WM_MOUSEHOVER = 0x02A1;
        public const int WM_USER = 0x400;
        
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

}


