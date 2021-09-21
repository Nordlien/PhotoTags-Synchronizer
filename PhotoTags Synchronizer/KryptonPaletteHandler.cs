using System.IO;
using DataGridViewGeneric;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public static class KryptonPaletteHandler
    {
        public static string PaletteFilename = "";
        public static string PaletteName = "";
        public static bool UseDropShadow = true;

        #region 
        public static void SetPalette(KryptonForm kryptonForm, KryptonManager kryptonManager, IPalette newKryptonPalette, bool enableDropShadow)
        {
            KryptonPalette kryptonPalette = new KryptonPalette();
            kryptonPalette.Import(((KryptonPalette)newKryptonPalette).Export(false, true));
            kryptonManager.GlobalPalette = kryptonPalette;
            kryptonForm.UseDropShadow = enableDropShadow;
            switch (((KryptonPalette)newKryptonPalette).BasePaletteMode)
            {
                case PaletteMode.Office2007Black:
                case PaletteMode.Office2010Black:
                case PaletteMode.Office365Black:
                    //ColumnHeader - Normal - 0
                    //((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderNormal(null);
                    //((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderNormal(null);

                    //ColumnHeader - Warning - 1
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom1.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderWarning(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom1.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderWarning(null);

                    //ColumnHeader - Error - 2
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackCellError(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextCellError(null);

                    //ColumnHeader - Image - 3
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackCellImage(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextCellImage(null);

                    //Cell - Editable - (Normal / Favorite) - 0 
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellNormal(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellNormal(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Back.Color2 = DataGridViewHandler.ColorBackCellFavorite(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Content.Color2 = DataGridViewHandler.ColorTextCellFavorite(null);

                    //Cell - ReadOnly - 0 (Normal / Favorite)
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellReadOnly(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellReadOnly(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Back.Color2 = DataGridViewHandler.ColorBackCellFavoriteReadOnly(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Content.Color2 = DataGridViewHandler.ColorTextCellFavoriteReadOnly(null);

                    //Cell - Warning - 1

                    //Cell - Error - 2
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellError(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellError(null);

                    //Cell - RegionFace - 3
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellImage(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellImage(null);
                    break;
                case PaletteMode.Office2007Blue:
                case PaletteMode.Office2007Silver:
                case PaletteMode.Office2007White:
                case PaletteMode.Office2010Silver:
                case PaletteMode.Office2010White:
                case PaletteMode.Office2013:
                case PaletteMode.Office2013White:
                case PaletteMode.Office365Blue:
                case PaletteMode.Office365Silver:
                case PaletteMode.Office365White:
                case PaletteMode.ProfessionalOffice2003:
                case PaletteMode.ProfessionalSystem:
                case PaletteMode.SparkleBlue:
                case PaletteMode.SparkleOrange:
                case PaletteMode.SparklePurple:
                    //ColumnHeader - Normal - 0
                    //((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderNormal(null);
                    //((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderNormal(null);

                    //ColumnHeader - Warning - 1
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom1.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderWarning(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom1.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderWarning(null);

                    //ColumnHeader - Error - 2
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackCellError(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextCellError(null);

                    //ColumnHeader - Image - 3
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackCellImage(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextCellImage(null);

                    //Cell - Editable - (Normal / Favorite) - 0 
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellNormal(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellNormal(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Back.Color2 = DataGridViewHandler.ColorBackCellFavorite(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Content.Color2 = DataGridViewHandler.ColorTextCellFavorite(null);

                    //Cell - ReadOnly - 0 (Normal / Favorite)
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellReadOnly(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellReadOnly(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Back.Color2 = DataGridViewHandler.ColorBackCellFavoriteReadOnly(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Content.Color2 = DataGridViewHandler.ColorTextCellFavoriteReadOnly(null);

                    //Cell - Warning - 1

                    //Cell - Error - 2
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellError(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellError(null);

                    //Cell - RegionFace - 3
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellImage(null);
                    ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellImage(null);
                    break;
            }
            


        }
        #endregion

        public static KryptonPalette Load(string filename, string paletteName)
        {
            PaletteFilename = filename;
            PaletteName = paletteName;

            KryptonPalette kryptonPalette = new KryptonPalette();
            kryptonPalette.BasePaletteMode = PaletteMode.Office365Black;

            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Black)) kryptonPalette.BasePaletteMode = PaletteMode.Office2007Black;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Blue)) kryptonPalette.BasePaletteMode = PaletteMode.Office2007Blue;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Silver)) kryptonPalette.BasePaletteMode = PaletteMode.Office2007Silver;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007White)) kryptonPalette.BasePaletteMode = PaletteMode.Office2007White;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Black)) kryptonPalette.BasePaletteMode = PaletteMode.Office2010Black;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Blue)) kryptonPalette.BasePaletteMode = PaletteMode.Office2010Blue;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Silver)) kryptonPalette.BasePaletteMode = PaletteMode.Office2010Silver;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010White)) kryptonPalette.BasePaletteMode = PaletteMode.Office2010White;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2013)) kryptonPalette.BasePaletteMode = PaletteMode.Office2013;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2013White)) kryptonPalette.BasePaletteMode = PaletteMode.Office2013White;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Black)) kryptonPalette.BasePaletteMode = PaletteMode.Office365Black;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Blue)) kryptonPalette.BasePaletteMode = PaletteMode.Office365Blue;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Silver)) kryptonPalette.BasePaletteMode = PaletteMode.Office365Silver;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365White)) kryptonPalette.BasePaletteMode = PaletteMode.Office365White;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.ProfessionalOffice2003)) kryptonPalette.BasePaletteMode = PaletteMode.ProfessionalOffice2003;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.ProfessionalSystem)) kryptonPalette.BasePaletteMode = PaletteMode.ProfessionalSystem;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparkleBlue)) kryptonPalette.BasePaletteMode = PaletteMode.SparkleBlue;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparkleOrange)) kryptonPalette.BasePaletteMode = PaletteMode.SparkleOrange;
            if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparklePurple)) kryptonPalette.BasePaletteMode = PaletteMode.SparklePurple;
            //if (paletteName == "Custom") kryptonPalette.BasePaletteMode = PaletteMode.Custom;
            //if (paletteName == "Global") kryptonPalette.BasePaletteMode = PaletteMode.Global;

            try
            {
                if (File.Exists(filename)) kryptonPalette.Import(filename, true);
            } catch
            {

            }
            return kryptonPalette;
        }
    }




}


    
