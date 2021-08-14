using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public class ToolStripProfessionalRendererWithoutLines : ToolStripProfessionalRenderer
    {
        public ToolStripProfessionalRendererWithoutLines() : base() { }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (!(e.ToolStrip is ToolStrip)) base.OnRenderToolStripBorder(e);
        }
    }
    

}


