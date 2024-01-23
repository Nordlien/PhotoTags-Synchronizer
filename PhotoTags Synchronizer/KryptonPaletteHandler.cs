using System;
using System.IO;
using System.Windows.Forms;
using DataGridViewGeneric;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public static class KryptonCustomPaletteBaseHandler
    {
        public static string PaletteFilename = "";
        public static string PaletteName = "";
        public static bool UseDropShadow = true;
        public static bool IsSystemPalette = true;

        #region SetPalette
        public static void SetPalette(KryptonForm kryptonForm, KryptonManager kryptonManager, PaletteBase  newKryptonCustomPaletteBase, bool isSystemPalette, bool enableDropShadow)
        {
            KryptonCustomPaletteBase KryptonCustomPaletteBase = new KryptonCustomPaletteBase();

            KryptonCustomPaletteBase.Import(((KryptonCustomPaletteBase)newKryptonCustomPaletteBase).Export(false, true)); //Make a copy
            kryptonManager.GlobalPalette = KryptonCustomPaletteBase;
            if (isSystemPalette)
            {
                System.Drawing.Font defaultFont = new System.Drawing.Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Regular);
                switch (((KryptonCustomPaletteBase)newKryptonCustomPaletteBase).BasePaletteMode)
                {
                    case PaletteMode.Office2007Black:
                    case PaletteMode.Office2010Black:
                    case PaletteMode.Microsoft365Black:
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).Common.StateCommon.Content.ShortText.Font = defaultFont;
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).Common.StateCommon.Content.LongText.Font = defaultFont;
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.DataCell.Content.Font = defaultFont;
                        //ColumnHeader - Normal - 0
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderNormal(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderNormal(null);

                        //ColumnHeader - Warning - 1
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom1.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderWarning(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom1.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderWarning(null);

                        //ColumnHeader - Error - 2
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackCellError(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextCellError(null);

                        //ColumnHeader - Image - 3
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackCellImage(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextCellImage(null);

                        //Cell - Editable - (Normal / Favorite) - 0 
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellNormal(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellNormal(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Back.Color2 = DataGridViewHandler.ColorBackCellFavorite(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Content.Color2 = DataGridViewHandler.ColorTextCellFavorite(null);

                        //Cell - ReadOnly - 0 (Normal / Favorite)
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellReadOnly(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellReadOnly(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Back.Color2 = DataGridViewHandler.ColorBackCellFavoriteReadOnly(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Content.Color2 = DataGridViewHandler.ColorTextCellFavoriteReadOnly(null);

                        //Cell - Warning - 1

                        //Cell - Error - 2
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellError(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellError(null);

                        //Cell - RegionFace - 3
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellImage(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellImage(null);
                        break;
                    case PaletteMode.Office2007Blue:
                    case PaletteMode.Office2007Silver:
                    case PaletteMode.Office2007White:
                    case PaletteMode.Office2010Blue:
                    case PaletteMode.Office2010Silver:
                    case PaletteMode.Office2010White:
                    case PaletteMode.Office2013White:
                    case PaletteMode.Microsoft365Blue:
                    case PaletteMode.Microsoft365Silver:
                    case PaletteMode.Microsoft365White:
                    case PaletteMode.ProfessionalOffice2003:
                    case PaletteMode.ProfessionalSystem:
                    case PaletteMode.SparkleBlue:
                    case PaletteMode.SparkleOrange:
                    case PaletteMode.SparklePurple:
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).Common.StateCommon.Content.ShortText.Font = defaultFont;
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).Common.StateCommon.Content.LongText.Font = defaultFont;
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.DataCell.Content.Font = defaultFont;

                        //ColumnHeader - Normal - 0
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderNormal(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderNormal(null);

                        //ColumnHeader - Warning - 1
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom1.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackHeaderWarning(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom1.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextHeaderWarning(null);

                        //ColumnHeader - Error - 2
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackCellError(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextCellError(null);

                        //ColumnHeader - Image - 3
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateCommon.HeaderColumn.Back.Color1 = DataGridViewHandler.ColorBackCellImage(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateCommon.HeaderColumn.Content.Color1 = DataGridViewHandler.ColorTextCellImage(null);

                        //Cell - Editable - (Normal / Favorite) - 0 
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellNormal(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellNormal(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Back.Color2 = DataGridViewHandler.ColorBackCellFavorite(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateNormal.DataCell.Content.Color2 = DataGridViewHandler.ColorTextCellFavorite(null);

                        //Cell - ReadOnly - 0 (Normal / Favorite)
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellReadOnly(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellReadOnly(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Back.Color2 = DataGridViewHandler.ColorBackCellFavoriteReadOnly(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCommon.StateDisabled.DataCell.Content.Color2 = DataGridViewHandler.ColorTextCellFavoriteReadOnly(null);

                        //Cell - Warning - 1

                        //Cell - Error - 2
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellError(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom2.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellError(null);

                        //Cell - RegionFace - 3
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateNormal.DataCell.Back.Color1 = DataGridViewHandler.ColorBackCellImage(null);
                        ((KryptonCustomPaletteBase)kryptonManager.GlobalPalette).GridStyles.GridCustom3.StateNormal.DataCell.Content.Color1 = DataGridViewHandler.ColorTextCellImage(null);
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

        #region KryptonCustomPaletteBase Load
        public static KryptonCustomPaletteBase Load(string filename, string paletteName)
        {
            PaletteFilename = filename;
            PaletteName = paletteName;

            KryptonCustomPaletteBase KryptonCustomPaletteBase = new KryptonCustomPaletteBase();
            IsSystemPalette = true;
            try
            {
                if (File.Exists(filename))
                {
                    if (!string.IsNullOrWhiteSpace(KryptonCustomPaletteBase.Import(filename, true))) IsSystemPalette = false;                       
                } else IsSystemPalette = true;
            } catch
            {
                IsSystemPalette = true;
            }

            if (IsSystemPalette)
            {
                if (string.IsNullOrWhiteSpace(paletteName)) paletteName = ThemeManager.ReturnPaletteModeAsString(PaletteMode.Microsoft365Blue);
                if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Black)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Office2007Black;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Blue)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Office2007Blue;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Silver)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Office2007Silver;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007White)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Office2007White;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Black)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Office2010Black;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Blue)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Office2010Blue;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Silver)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Office2010Silver;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010White)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Office2010White;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2013White)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Office2013White;
                else if (
                    paletteName == "Office 365 - Black" ||
                    paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Microsoft365Black)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Microsoft365Black;
                else if (
                    paletteName == "Office 365 - Blue" ||
                    paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Microsoft365Blue)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Microsoft365Blue;
                else if (
                    paletteName == "Office 365 - Silver" ||
                    paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Microsoft365Silver)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Microsoft365Silver;
                else if (
                    paletteName == "Office 365 - White" ||
                    paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.Microsoft365White)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Microsoft365White;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.ProfessionalOffice2003)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.ProfessionalOffice2003;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.ProfessionalSystem)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.ProfessionalSystem;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparkleBlue)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.SparkleBlue;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparkleOrange)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.SparkleOrange;
                else if (paletteName == ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparklePurple)) KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.SparklePurple;
                else KryptonCustomPaletteBase.BasePaletteMode = PaletteMode.Microsoft365Blue;
            }
            return KryptonCustomPaletteBase;
        }
        #endregion

        

        #region 
        public static void SetDataGridViewPalette(KryptonManager kryptonManager, DataGridView dataGridView)
        {
            DataGridViewGenericData dataGridViewGenericData = DataGridViewHandler.GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData != null) dataGridViewGenericData.KryptonCustomPaletteBase = (KryptonCustomPaletteBase)kryptonManager.GlobalPalette;
        }

        public static void SetImageListViewPalettes(KryptonManager kryptonManager, Manina.Windows.Forms.ImageListView imageListView)
        {
            KryptonCustomPaletteBase palette = (KryptonCustomPaletteBase)kryptonManager.GlobalPalette;

            // Get the two colors and angle used to draw the control background
            imageListView.BackColor = palette.GetBackColor1(PaletteBackStyle.ControlClient, imageListView.Enabled ? PaletteState.Normal : PaletteState.Disabled);
            imageListView.ForeColor = palette.GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
            imageListView.Font = palette.GetContentShortTextNewFont(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
            imageListView.HeaderFont = palette.GetContentLongTextNewFont(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);            
        }
        #endregion 
    }




}


    
