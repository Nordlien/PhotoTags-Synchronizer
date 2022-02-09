using System;
using System.IO;
using System.Windows.Forms;
using DataGridViewGeneric;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public static class KryptonPaletteHandler
    {
        public static string PaletteFilename = "";
        public static string PaletteName = "";
        public static bool UseDropShadow = true;
        public static bool IsSystemPalette = true;

        #region SetPalette
        public static void SetPalette(KryptonForm kryptonForm, KryptonManager kryptonManager, IPalette newKryptonPalette, bool isSystemPalette, bool enableDropShadow)
        {
            KryptonPalette kryptonPalette = new KryptonPalette();

            kryptonPalette.Import(((KryptonPalette)newKryptonPalette).Export(false, true)); //Make a copy
            kryptonManager.GlobalPalette = kryptonPalette;
            kryptonForm.UseDropShadow = enableDropShadow;
            if (isSystemPalette)
            {
                switch (((KryptonPalette)newKryptonPalette).BasePaletteMode)
                {
                    case PaletteMode.Office2007Black:
                    case PaletteMode.Office2010Black:
                    case PaletteMode.Office365Black:
                        //ColumnHeader - Normal - 0
                        ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderNormal(null);
                        ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderNormal(null);

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
                    case PaletteMode.Office2010Blue:
                    case PaletteMode.Office2010Silver:
                    case PaletteMode.Office2010White:
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
                        ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderNormal(null);
                        ((KryptonPalette)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderNormal(null);

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
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion

        public static void Save(string filename)
        {
            PaletteFilename = filename;
            IsSystemPalette = false;
        }

        #region KryptonPalette Load
        public static KryptonPalette Load(string filename, string paletteName)
        {
            PaletteFilename = filename;
            PaletteName = paletteName;

            KryptonPalette kryptonPalette = new KryptonPalette();
            IsSystemPalette = true;
            try
            {
                if (File.Exists(filename))
                {
                    if (!string.IsNullOrWhiteSpace(kryptonPalette.Import(filename, true))) IsSystemPalette = false;                       
                } else IsSystemPalette = true;
            } catch
            {
                IsSystemPalette = true;
            }

            if (IsSystemPalette)
            {
                if (string.IsNullOrWhiteSpace(paletteName)) paletteName = ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Blue);
                if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Black)) kryptonPalette.BasePaletteMode = PaletteMode.Office2007Black;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Blue)) kryptonPalette.BasePaletteMode = PaletteMode.Office2007Blue;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Silver)) kryptonPalette.BasePaletteMode = PaletteMode.Office2007Silver;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007White)) kryptonPalette.BasePaletteMode = PaletteMode.Office2007White;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Black)) kryptonPalette.BasePaletteMode = PaletteMode.Office2010Black;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Blue)) kryptonPalette.BasePaletteMode = PaletteMode.Office2010Blue;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Silver)) kryptonPalette.BasePaletteMode = PaletteMode.Office2010Silver;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010White)) kryptonPalette.BasePaletteMode = PaletteMode.Office2010White;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2013White)) kryptonPalette.BasePaletteMode = PaletteMode.Office2013White;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Black)) kryptonPalette.BasePaletteMode = PaletteMode.Office365Black;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Blue)) kryptonPalette.BasePaletteMode = PaletteMode.Office365Blue;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Silver)) kryptonPalette.BasePaletteMode = PaletteMode.Office365Silver;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365White)) kryptonPalette.BasePaletteMode = PaletteMode.Office365White;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.ProfessionalOffice2003)) kryptonPalette.BasePaletteMode = PaletteMode.ProfessionalOffice2003;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.ProfessionalSystem)) kryptonPalette.BasePaletteMode = PaletteMode.ProfessionalSystem;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparkleBlue)) kryptonPalette.BasePaletteMode = PaletteMode.SparkleBlue;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparkleOrange)) kryptonPalette.BasePaletteMode = PaletteMode.SparkleOrange;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparklePurple)) kryptonPalette.BasePaletteMode = PaletteMode.SparklePurple;
                else throw new NotImplementedException();
            }
            return kryptonPalette;
        }
        #endregion

        

        #region 
        public static void SetDataGridViewPalette(KryptonManager kryptonManager, DataGridView dataGridView)
        {
            DataGridViewGenericData dataGridViewGenericData = DataGridViewHandler.GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData != null) dataGridViewGenericData.KryptonPalette = (KryptonPalette)kryptonManager.GlobalPalette;
        }

        public static void SetImageListViewPalettes(KryptonManager kryptonManager, Manina.Windows.Forms.ImageListView imageListView)
        {
            KryptonPalette palette = (KryptonPalette)kryptonManager.GlobalPalette;

            // Get the two colors and angle used to draw the control background
            imageListView.BackColor = palette.GetBackColor1(PaletteBackStyle.ControlClient, imageListView.Enabled ? PaletteState.Normal : PaletteState.Disabled);
            imageListView.ForeColor = palette.GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
            imageListView.Font = palette.GetContentShortTextNewFont(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
            imageListView.HeaderFont = palette.GetContentLongTextNewFont(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);            
        }
        #endregion 
    }




}


    
