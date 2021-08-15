using System.Windows.Forms;

namespace PhotoTagsCommonComponets
{
    public class TreeViewWithoutDoubleClick : TreeView
	{
		protected override void WndProc(ref Message m)
		{
			//Make double click as single click
			if (m.Msg == 0x203) m.Msg = 0x0201;
			base.WndProc(ref m);

			// Suppress WM_LBUTTONDBLCLK
			//if (m.Msg == 0x0203 && this.CheckBoxes)
			//{
			//    var localPos = this.PointToClient(Cursor.Position);
			//    var hitTestInfo = this.HitTest(localPos);
			//    if (hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
			//    {
			//        m.Msg = 0x0201;
			//    }
			//}
			//base.WndProc(ref m);
		}
	}
}
