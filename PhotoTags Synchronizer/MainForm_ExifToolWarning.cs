using System.Windows.Forms;
using DataGridViewGeneric;
using System.Collections.Generic;
using MetadataPriorityLibrary;
using System.Linq;
using System;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        public void PopulateExiftoolWarningToolStripMenuItems()
        {
            toolStripMenuItemtoolExiftoolWarningAssignCompositeTag.DropDownItems.Clear();
            SortedDictionary<string, string> listAllTags = new CompositeTags().ListAllTags();
            foreach (KeyValuePair<string, string> tag in listAllTags.OrderBy(key => key.Value))
            {
                ToolStripMenuItem newTagItem = new ToolStripMenuItem();
                newTagItem.Name = tag.Value;
                newTagItem.Size = new System.Drawing.Size(224, 26);
                newTagItem.Text = tag.Value;
                
                toolStripMenuItemtoolExiftoolWarningAssignCompositeTag.DropDownItems.Add(newTagItem);

                switch (tag.Value)
                {
                    case CompositeTags.NotDefined:
                        newTagItem.Tag = new MetadataPriorityValues(tag.Value, 25);
                        newTagItem.Click += new System.EventHandler(this.exiftoolWarningCompositeTagsPrioityToolStripMenuItem_Click);
                        break;
                    case CompositeTags.Ignore:
                        newTagItem.Tag = new MetadataPriorityValues(tag.Value, 25);
                        newTagItem.Click += new System.EventHandler(this.exiftoolWarningCompositeTagsPrioityToolStripMenuItem_Click);
                        break;
                    default:
                        ToolStripMenuItem newTagSubItem1 = new ToolStripMenuItem();
                        newTagSubItem1.Name = "Prioity low - 25";
                        newTagSubItem1.Size = new System.Drawing.Size(224, 26);
                        newTagSubItem1.Text = "Prioity low - 25";
                        newTagSubItem1.Tag = new MetadataPriorityValues(tag.Value, 25);
                        newTagSubItem1.Click += new System.EventHandler(this.exiftoolWarningCompositeTagsPrioityToolStripMenuItem_Click);
                        newTagItem.DropDownItems.Add(newTagSubItem1);

                        ToolStripMenuItem newTagSubItem2 = new ToolStripMenuItem();
                        newTagSubItem2.Name = "Prioity medium low - 50";
                        newTagSubItem2.Size = new System.Drawing.Size(224, 26);
                        newTagSubItem2.Text = "Prioity medium low - 50";
                        newTagSubItem2.Tag = new MetadataPriorityValues(tag.Value, 50);
                        newTagSubItem2.Click += new System.EventHandler(this.exiftoolWarningCompositeTagsPrioityToolStripMenuItem_Click);
                        newTagItem.DropDownItems.Add(newTagSubItem2);

                        ToolStripMenuItem newTagSubItem3 = new ToolStripMenuItem();
                        newTagSubItem3.Name = "Prioity normal - 100";
                        newTagSubItem3.Size = new System.Drawing.Size(224, 26);
                        newTagSubItem3.Text = "Prioity normal - 100";
                        newTagSubItem3.Tag = new MetadataPriorityValues(tag.Value, 100);
                        newTagSubItem3.Click += new System.EventHandler(this.exiftoolWarningCompositeTagsPrioityToolStripMenuItem_Click);
                        newTagItem.DropDownItems.Add(newTagSubItem3);

                        ToolStripMenuItem newTagSubItem4 = new ToolStripMenuItem();
                        newTagSubItem4.Name = "Prioity medium high - 150";
                        newTagSubItem4.Size = new System.Drawing.Size(224, 26);
                        newTagSubItem4.Text = "Prioity medium high - 150";
                        newTagSubItem4.Tag = new MetadataPriorityValues(tag.Value, 150);
                        newTagSubItem4.Click += new System.EventHandler(this.exiftoolWarningCompositeTagsPrioityToolStripMenuItem_Click);
                        newTagItem.DropDownItems.Add(newTagSubItem4);

                        ToolStripMenuItem newTagSubItem5 = new ToolStripMenuItem();
                        newTagSubItem5.Name = "Prioity high - 200";
                        newTagSubItem5.Size = new System.Drawing.Size(224, 26);
                        newTagSubItem5.Text = "Prioity high - 200";
                        newTagSubItem5.Tag = new MetadataPriorityValues(tag.Value, 200);
                        newTagSubItem5.Click += new System.EventHandler(this.exiftoolWarningCompositeTagsPrioityToolStripMenuItem_Click);
                        newTagItem.DropDownItems.Add(newTagSubItem5);
                        break;
                }
            }
        }

        private void exiftoolWarningCompositeTagsPrioityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            MetadataPriorityValues metadataPriorityValues = (MetadataPriorityValues)(((ToolStripMenuItem)sender).Tag);

            List<int> rows = DataGridViewHandler.GetRowSelected(dataGridView);
            foreach (int rowIndex in rows)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader && dataGridViewGenericRow.MetadataPriorityKey != null)
                {
                    exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary[dataGridViewGenericRow.MetadataPriorityKey] = metadataPriorityValues;

                    MetadataPriorityGroup metadataPriorityGroup = new MetadataPriorityGroup(
                        dataGridViewGenericRow.MetadataPriorityKey,
                        exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary[dataGridViewGenericRow.MetadataPriorityKey]);

                    bool priorityKeyExisit = exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary.ContainsKey(dataGridViewGenericRow.MetadataPriorityKey);
                    if (exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary[dataGridViewGenericRow.MetadataPriorityKey].Composite == CompositeTags.NotDefined) priorityKeyExisit = false;

                    if (priorityKeyExisit)
                        DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, metadataPriorityGroup.ToString());
                    else
                        DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, "");

                    DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, rowIndex,
                        new DataGridViewGenericRow(
                            dataGridViewGenericRow.HeaderName,
                            dataGridViewGenericRow.RowName,
                            dataGridViewGenericRow.IsMultiLine,
                            dataGridViewGenericRow.MetadataPriorityKey));
                    DataGridViewHandler.SetRowFavoriteFlag(dataGridView, rowIndex);
                    DataGridViewHandler.SetCellBackGroundColorForRow(dataGridView, rowIndex);
                }
            }
            DataGridViewHandler.Refresh(dataGridView);
            exiftoolReader.MetadataReadPrioity.WriteAlways();
        }

        private void dataGridViewExifToolWarning_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }

        private void toolStripMenuItemShowPosterWindowWarnings_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            OpenRegionSelector();
            RegionSelectorLoadAndSelect(dataGridView);
        }

        private void dataGridViewExifToolWarning_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (e.RowIndex == -1) RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }

    }


    
}
