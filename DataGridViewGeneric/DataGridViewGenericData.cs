using ColumnNamesAndWidth;
using Krypton.Toolkit;
using System.Collections.Generic;
using System.Threading;

namespace DataGridViewGeneric
{

    public class DataGridViewGenericData
    {
        #region IsAgregated / IsPopulated
        public bool IsPopulating { get; set; }        
        public bool IsPopulatingFile { get; set; } = false;
        public bool IsPopulatingImage { get; set; } = false;
        public bool IsAgregated { get; set; }
        public bool IsPopulationgCellSize { get; set; } = false;
        #endregion

        #region Undo / Redo
        public Stack<Dictionary<CellLocation, DataGridViewGenericCell>> UndoCellsStack { get; set; } = new Stack<Dictionary<CellLocation, DataGridViewGenericCell>>();
        public Stack<Dictionary<CellLocation, DataGridViewGenericCell>> RedoCellsStack { get; set; } = new Stack<Dictionary<CellLocation, DataGridViewGenericCell>>();
        #endregion

        #region Grid Info / Layout
        public string TopCellName { get; set; } = "";
        public string DataGridViewName { get; set; } = "";
        public DataGridViewSize CellSize { get; set; } = DataGridViewSize.Medium;
        public int CellHeight { get; set; } = 24;
        public KryptonCustomPaletteBase KryptonCustomPaletteBase { get; set; } = null;
        public List<ColumnNameAndWidth> ColumnNameAndWidthsLarge { get; set; }
        public List<ColumnNameAndWidth> ColumnNameAndWidthsMedium { get; set; }
        public List<ColumnNameAndWidth> ColumnNameAndWidthsSmall { get; set; }
        #endregion

        #region Favorite and Equal rows
        public List<FavoriteRow> FavoriteList { get; set; } = null;
        public bool ShowFavouriteColumns { get; set; } = false;
        public bool HideEqualColumns { get; set; } = false;
        #endregion
    }
}
