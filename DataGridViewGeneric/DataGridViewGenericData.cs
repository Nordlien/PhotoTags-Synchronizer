using Krypton.Toolkit;
using System.Collections.Generic;

namespace DataGridViewGeneric
{

    public class DataGridViewGenericData
    {
        public Stack<Dictionary<CellLocation, DataGridViewGenericCell>> UndoCellsStack { get; set; } = new Stack<Dictionary<CellLocation, DataGridViewGenericCell>>();
        public Stack<Dictionary<CellLocation, DataGridViewGenericCell>> RedoCellsStack { get; set; } = new Stack<Dictionary<CellLocation, DataGridViewGenericCell>>();
        public List<FavoriteRow> FavoriteList { get; set; } = null;
        public bool IsPopulating { get; set; } = false;
        public bool IsPopulatingFile { get; set; } = false;
        public bool IsPopulatingImage { get; set; } = false;
        public bool IsAgregated { get; set; } = false;        
        public string TopCellName { get; set; } = "";
        public string DataGridViewName { get; set; } = "";
        public DataGridViewSize CellSize { get; set; } = DataGridViewSize.Medium;

        public bool ShowFavouriteColumns { get; set; } = false;
        public bool HideEqualColumns { get; set; } = false;

        public KryptonPalette KryptonPalette { get; set; } = null;
    }
}
