using DataGridViewGeneric;
using MetadataLibrary;
using MetadataPriorityLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public partial class Config : Form
    {
        public MetadataReadPrioity MetadataReadPrioity
        {
            get;
            set;
        } //= new MetadataReadPrioity();

        private Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionaryCopy = new Dictionary<MetadataPriorityKey, MetadataPriorityValues>();
        private AutoCorrect autoCorrect = new AutoCorrect();

        public Config()
        {
            InitializeComponent();
            textBoxConfigFilenameDateFormats.Text = Properties.Settings.Default.RenameDateFormats;

            SortedDictionary<string, string> listAllTags = new CompositeTags().ListAllTags();
            foreach (KeyValuePair<string, string> tag in listAllTags.OrderBy(key => key.Value))
            {
                ToolStripMenuItem newTagItem = new ToolStripMenuItem();
                newTagItem.Name = tag.Value;
                newTagItem.Size = new System.Drawing.Size(224, 26);
                newTagItem.Text = tag.Value;
                newTagItem.Click += new System.EventHandler(this.ToolStripMenuItemMoveAndAssign_Click);
                this.toolStripMenuItemMetadataReadMove.DropDownItems.Add(newTagItem);
            }

            

        }

        #region AutoCorrect
        private void PopulateAutoCorrectListOrder(ImageListViewOrder imageListViewOrder, List<MetadataBrokerTypes> listPriority)
        {
            ListViewItem listViewItem;

            imageListViewOrder.Items.Clear();
            foreach (MetadataBrokerTypes metadataBroker in listPriority)
            {
                switch (metadataBroker)
                {
                    case MetadataBrokerTypes.ExifTool:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Exiftool";
                        listViewItem.Tag = MetadataBrokerTypes.ExifTool;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerTypes.MicrosoftPhotos:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "MicrosoftPhotos";
                        listViewItem.Tag = MetadataBrokerTypes.MicrosoftPhotos;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerTypes.WindowsLivePhotoGallery:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Windows Live Photo Gallery";
                        listViewItem.Tag = MetadataBrokerTypes.WindowsLivePhotoGallery;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerTypes.FileSystem:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Subfolder name";
                        listViewItem.Tag = MetadataBrokerTypes.FileSystem;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                }
            }
        }

        private void PopulateAutoCorrectDateTakenPriority(ImageListViewOrder imageListViewOrder, List<DateTimeSources> listPriority)
        {
            ListViewItem listViewItem;

            imageListViewOrder.Items.Clear();
            foreach (DateTimeSources dateTimeSource in listPriority)
            {
                switch (dateTimeSource)
                {
                    case DateTimeSources.DateTaken:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Date&Time Taken";
                        listViewItem.Tag = DateTimeSources.DateTaken;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case DateTimeSources.GPSDateAndTime:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "GPS UTC DateTime";
                        listViewItem.Tag = DateTimeSources.GPSDateAndTime;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case DateTimeSources.FirstDateFoundInFilename:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "First Date&Time found in Filename";
                        listViewItem.Tag = DateTimeSources.FirstDateFoundInFilename;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case DateTimeSources.LastDateFoundInFilename:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Last Date&Time found in Filename";
                        listViewItem.Tag = DateTimeSources.LastDateFoundInFilename;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                }
            }
        }

        private void PopulateAutoCorrectPoperties()
        {
            #region Date Taken
            if (autoCorrect.DateTakenPriority == null || autoCorrect.DateTakenPriority.Count == 0)
            {
                autoCorrect.DateTakenPriority.Add(DateTimeSources.DateTaken);
                autoCorrect.DateTakenPriority.Add(DateTimeSources.GPSDateAndTime);
                autoCorrect.DateTakenPriority.Add(DateTimeSources.FirstDateFoundInFilename);
                autoCorrect.DateTakenPriority.Add(DateTimeSources.LastDateFoundInFilename);
            }

            PopulateAutoCorrectDateTakenPriority(imageListViewOrderDateTaken, autoCorrect.DateTakenPriority);

            if (autoCorrect.UpdateDateTaken)
            {
                if (autoCorrect.UpdateDateTakenWithFirstInPrioity)
                    radioButtonDateTakenUseFirst.Checked = true;
                else
                    radioButtonDateTakenChangeWhenEmpty.Checked = true;
            }
            else radioButtonDateTakenDoNotChange.Checked = true;

            #endregion

            #region Title            
            if (autoCorrect.TitlePriority == null || autoCorrect.TitlePriority.Count == 0)
            {
                autoCorrect.TitlePriority.Add(MetadataBrokerTypes.ExifTool);
                autoCorrect.TitlePriority.Add(MetadataBrokerTypes.MicrosoftPhotos);
                autoCorrect.TitlePriority.Add(MetadataBrokerTypes.WindowsLivePhotoGallery);
            }

            PopulateAutoCorrectListOrder(imageListViewOrderTitle, autoCorrect.TitlePriority);

            if (autoCorrect.UpdateTitle)            
            {
                if (autoCorrect.UpdateTitleWithFirstInPrioity)
                    radioButtonTitleUseFirst.Checked = true;
                else
                    radioButtonTitleChangeWhenEmpty.Checked = true;
            }
            else radioButtonTitleDoNotChange.Checked = true;

            #endregion

            #region Album
            if (autoCorrect.AlbumPriority == null || autoCorrect.AlbumPriority.Count == 0)
            {
                autoCorrect.AlbumPriority.Add(MetadataBrokerTypes.ExifTool);
                autoCorrect.AlbumPriority.Add(MetadataBrokerTypes.MicrosoftPhotos);
                autoCorrect.AlbumPriority.Add(MetadataBrokerTypes.FileSystem);
            }
            PopulateAutoCorrectListOrder(imageListViewOrderAlbum, autoCorrect.AlbumPriority);

            if (autoCorrect.UpdateAlbum)
            {
                if (autoCorrect.UpdateAlbumWithFirstInPrioity)
                    radioButtonAlbumUseFirst.Checked = true;
                else
                    radioButtonAlbumChangeWhenEmpty.Checked = true;
            }
            else radioButtonAlbumDoNotChange.Checked = true;

            #endregion

            #region Keywords
            checkBoxKeywordsAddMicrosoftPhotos.Checked = autoCorrect.UseKeywordsFromMicrosoftPhotos;
            checkBoxKeywordsAddWindowsMediaPhotoGallery.Checked = autoCorrect.UseKeywordsFromWindowsLivePhotoGallery;
            comboBoxKeywordsAiConfidence.SelectedIndex = 9 - (int)(autoCorrect.KeywordTagConfidenceLevel * 10);

            checkBoxKeywordBackupDateTakenAfter.Checked = autoCorrect.BackupDateTakenAfterUpdate;
            checkBoxKeywordBackupDateTakenBefore.Checked = autoCorrect.BackupDateTakenBeforeUpdate;
            checkBoxKeywordBackupGPSDateTimeUTCAfter.Checked = autoCorrect.BackupGPGDateTimeUTCAfterUpdate;
            checkBoxKeywordBackupGPSDateTimeUTCBefore.Checked = autoCorrect.BackupGPGDateTimeUTCBeforeUpdate;
            checkBoxKeywordBackupLocationCity.Checked = autoCorrect.BackupLocationCity;
            checkBoxKeywordBackupLocationCountry.Checked = autoCorrect.BackupLocationCountry;
            checkBoxKeywordBackupLocationName.Checked = autoCorrect.BackupLocationName;
            checkBoxKeywordBackupLocationState.Checked = autoCorrect.BackupLocationState;
            checkBoxKeywordBackupRegionFaceNames.Checked = autoCorrect.BackupRegionFaceNames;
            #endregion

            #region Region Faces
            checkBoxFaceRegionAddMicrosoftPhotos.Checked = autoCorrect.UseFaceRegionFromMicrosoftPhotos;
            checkBoxFaceRegionAddWindowsMediaPhotoGallery.Checked = autoCorrect.UseFaceRegionFromWindowsLivePhotoGallery;
            #endregion

            #region Author
            if (!autoCorrect.UpdateAuthor) radioButtonAuthorDoNotChange.Checked = true;
            else if (autoCorrect.UpdateAuthorOnlyWhenEmpty) radioButtonAuthorChangeWhenEmpty.Checked = true;
            else radioButtonAuthorAlwaysChange.Checked = true;                                                
            #endregion

            #region Location            
            if (!autoCorrect.UpdateLocation) radioButtonLocationNameDoNotChange.Checked = true;            
            else if (autoCorrect.UpdateLocationOnlyWhenEmpty) radioButtonLocationNameChangeWhenEmpty.Checked = true;            
            else radioButtonLocationNameChangeAlways.Checked = true;

            checkBoxUpdateLocationName.Checked = autoCorrect.UpdateLocationName;
            checkBoxUpdateLocationCity.Checked = autoCorrect.UpdateLocationCity;
            checkBoxUpdateLocationState.Checked = autoCorrect.UpdateLocationState;
            checkBoxUpdateLocationCountry.Checked = autoCorrect.UpdateLocationCountry;
            #endregion
        }

        private void GetAutoCorrectPoperties()
        {
            #region DateTaken
            autoCorrect.DateTakenPriority.Clear();
            foreach (ListViewItem item in imageListViewOrderDateTaken.Items)
            {
                autoCorrect.DateTakenPriority.Add((DateTimeSources)item.Tag);
            }

            if (radioButtonDateTakenDoNotChange.Checked)
            {
                autoCorrect.UpdateDateTaken = false;
                autoCorrect.UpdateDateTakenWithFirstInPrioity = false;
            }
            else
            {
                autoCorrect.UpdateTitle = true;

                if (radioButtonDateTakenUseFirst.Checked)
                    autoCorrect.UpdateDateTakenWithFirstInPrioity = true;
                else
                    autoCorrect.UpdateDateTakenWithFirstInPrioity = false;
            }
            #endregion 

            #region Title
            autoCorrect.TitlePriority.Clear();
            foreach (ListViewItem item in imageListViewOrderTitle.Items)
            {
                autoCorrect.TitlePriority.Add((MetadataBrokerTypes)item.Tag);
            }

            if (radioButtonTitleDoNotChange.Checked)
            {
                autoCorrect.UpdateTitle = false;
                autoCorrect.UpdateTitleWithFirstInPrioity = false;
            }
            else
            {
                autoCorrect.UpdateTitle = true;

                if (radioButtonTitleUseFirst.Checked)
                    autoCorrect.UpdateTitleWithFirstInPrioity = true;
                else
                    autoCorrect.UpdateTitleWithFirstInPrioity = false;
            }
            #endregion

            #region Album
            autoCorrect.AlbumPriority.Clear();
            foreach (ListViewItem item in imageListViewOrderAlbum.Items)
            {
                autoCorrect.AlbumPriority.Add((MetadataBrokerTypes)item.Tag);
            }

            if (radioButtonAlbumDoNotChange.Checked)
            {
                autoCorrect.UpdateAlbum = false;
                autoCorrect.UpdateAlbumWithFirstInPrioity = false;
            }
            else
            {
                autoCorrect.UpdateAlbum = true;

                if (radioButtonTitleUseFirst.Checked)
                    autoCorrect.UpdateAlbumWithFirstInPrioity = true;
                else
                    autoCorrect.UpdateAlbumWithFirstInPrioity = false;
            }

            autoCorrect.KeywordTagConfidenceLevel = (90 - comboBoxKeywordsAiConfidence.SelectedIndex * 10) / 100.0;            
            #endregion

            #region Keywords
            autoCorrect.UseKeywordsFromMicrosoftPhotos = checkBoxKeywordsAddMicrosoftPhotos.Checked;
            autoCorrect.UseKeywordsFromWindowsLivePhotoGallery = checkBoxKeywordsAddWindowsMediaPhotoGallery.Checked;

            autoCorrect.BackupDateTakenAfterUpdate = checkBoxKeywordBackupDateTakenAfter.Checked;
            autoCorrect.BackupDateTakenBeforeUpdate = checkBoxKeywordBackupDateTakenBefore.Checked;
            autoCorrect.BackupGPGDateTimeUTCAfterUpdate = checkBoxKeywordBackupGPSDateTimeUTCAfter.Checked;
            autoCorrect.BackupGPGDateTimeUTCBeforeUpdate = checkBoxKeywordBackupGPSDateTimeUTCBefore.Checked;
            autoCorrect.BackupLocationCity = checkBoxKeywordBackupLocationCity.Checked;
            autoCorrect.BackupLocationCountry = checkBoxKeywordBackupLocationCountry.Checked;
            autoCorrect.BackupLocationName = checkBoxKeywordBackupLocationName.Checked;
            autoCorrect.BackupLocationState = checkBoxKeywordBackupLocationState.Checked;
            autoCorrect.BackupRegionFaceNames = checkBoxKeywordBackupRegionFaceNames.Checked;
            #endregion

            #region Region Faces
            autoCorrect.UseFaceRegionFromMicrosoftPhotos = checkBoxFaceRegionAddMicrosoftPhotos.Checked;
            autoCorrect.UseFaceRegionFromWindowsLivePhotoGallery = checkBoxFaceRegionAddWindowsMediaPhotoGallery.Checked;
            #endregion

            #region Author
            if (radioButtonAuthorDoNotChange.Checked)
            {
                autoCorrect.UpdateAuthor = false;
                autoCorrect.UpdateAuthorOnlyWhenEmpty = false;
            }
            else
            {
                autoCorrect.UpdateAuthor = true;

                if (radioButtonAuthorChangeWhenEmpty.Checked)
                    autoCorrect.UpdateAuthorOnlyWhenEmpty = true;
                else
                    autoCorrect.UpdateAuthorOnlyWhenEmpty = false;
            }
            #endregion

            #region Location            
            if (radioButtonLocationNameDoNotChange.Checked)
            {
                autoCorrect.UpdateLocation = false;
                autoCorrect.UpdateLocationOnlyWhenEmpty = false;
            }
            else
            {
                autoCorrect.UpdateLocation = true;
                
                if (radioButtonLocationNameChangeWhenEmpty.Checked)
                    autoCorrect.UpdateLocationOnlyWhenEmpty = true;
                else
                    autoCorrect.UpdateLocationOnlyWhenEmpty = false;
            }

            autoCorrect.UpdateLocationName = checkBoxUpdateLocationName.Checked;
            autoCorrect.UpdateLocationCity = checkBoxUpdateLocationCity.Checked;
            autoCorrect.UpdateLocationState = checkBoxUpdateLocationState.Checked;
            autoCorrect.UpdateLocationCountry = checkBoxUpdateLocationCountry.Checked;
            #endregion
        }
        #endregion

        #region All tabs - Init - Save - Close
        public void Init()
        {
            CopyMetadataReadPrioity(MetadataReadPrioity.MetadataPrioityDictionary, metadataPrioityDictionaryCopy);
            PopulateMetadataRead();
            autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
            PopulateAutoCorrectPoperties();
        }

        private void buttonConfigSave_Click(object sender, EventArgs e)
        {
            GetAutoCorrectPoperties();
            Properties.Settings.Default.AutoCorrect = autoCorrect.SerializeThis();
            
            Properties.Settings.Default.RenameDateFormats = textBoxConfigFilenameDateFormats.Text;
            Properties.Settings.Default.Save();

            MetadataReadPrioity.MetadataPrioityDictionary = metadataPrioityDictionaryCopy;
            MetadataReadPrioity.WriteAlways();
            this.Close();
        }

        private void buttonConfigCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion 


        public void CopyMetadataReadPrioity(Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionarySource,
            Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionaryDestination)
        {
            metadataPrioityDictionaryCopy = new Dictionary<MetadataPriorityKey, MetadataPriorityValues>();
            foreach (KeyValuePair<MetadataPriorityKey, MetadataPriorityValues> keyValuePair in metadataPrioityDictionarySource)
            {
                metadataPrioityDictionaryCopy.Add(new MetadataPriorityKey(keyValuePair.Key), new MetadataPriorityValues(keyValuePair.Value));
            }
        }

        #region Config Read Metadata - Populate

        public void PopulateMetadataRead()
        {
            PopulateMetadataRead(dataGridViewMetadataReadPriority);
        }

        private void PopulateMetadataRead(DataGridView dataGridView)
        {
            isCellValueUpdating = true;
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridView, "Name", "Tags", DataGridViewSize.ConfigSize);
            DataGridViewHandler.Clear(dataGridView, DataGridViewSize.ConfigSize);
            //contextMenuStripMetadataRead contextMenuStripMetadataRead

            DateTime dateTimeEditable = DateTime.Now;

            int columnIndex1 = DataGridViewHandler.AddColumnOrUpdate(dataGridView,
                new FileEntryImage("Priority", dateTimeEditable), //Heading
                    null, dateTimeEditable,
                    ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Off, true));

            List<string> compositeList = new List<string>();

            List<MetadataPriorityGroup> metadataPrioityGroupSortedList = new List<MetadataPriorityGroup>();
            foreach (MetadataPriorityKey metadataPriorityKey in metadataPrioityDictionaryCopy.Keys)
            {
                metadataPrioityGroupSortedList.Add(new MetadataPriorityGroup(metadataPriorityKey, metadataPrioityDictionaryCopy[metadataPriorityKey]));
            }
            metadataPrioityGroupSortedList.Sort(); // (x, y) => x.CompareTo(y));

            foreach (MetadataPriorityGroup metadataPrioityGroup in metadataPrioityGroupSortedList)
            {
                if (!compositeList.Contains(metadataPrioityGroup.MetadataPriorityValues.Composite))
                {
                    compositeList.Add(metadataPrioityGroup.MetadataPriorityValues.Composite);
                    DataGridViewHandler.AddRow(dataGridView, columnIndex1, new DataGridViewGenericRow(metadataPrioityGroup.MetadataPriorityValues.Composite));
                }
            }

            foreach (MetadataPriorityGroup metadataPrioityGroup in metadataPrioityGroupSortedList)
            {
                DataGridViewHandler.AddRow(dataGridView, columnIndex1, new DataGridViewGenericRow(
                    metadataPrioityGroup.MetadataPriorityValues.Composite,
                    metadataPrioityGroup.MetadataPriorityKey.Region + " | " + metadataPrioityGroup.MetadataPriorityKey.Tag,
                    metadataPrioityGroup.MetadataPriorityKey),
                    metadataPrioityGroup.MetadataPriorityValues.Priority, false);
            }
            isCellValueUpdating = false;
        }

        public void AssignSelectedToNewTag(DataGridView dataGridView, Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionary, string composite)
        {
            List<int> rowSelected = DataGridViewHandler.GetRowSelected(dataGridView);

            foreach (int rowIndex in rowSelected)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                {
                    MetadataPriorityValues metadataPriorityValues = metadataPrioityDictionary[dataGridViewGenericRow.MetadataPriorityKey];
                    metadataPriorityValues.Composite = composite;
                }
            }
        }

        private void ToolStripMenuItemMoveAndAssign_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tagSender = (ToolStripMenuItem)sender;
            //DataGridViewHandler.
            AssignSelectedToNewTag(dataGridViewMetadataReadPriority, metadataPrioityDictionaryCopy, tagSender.Text);
            PopulateMetadataRead(dataGridViewMetadataReadPriority);
        }
        #endregion

        #region Config - Read Metadata - Drag and Drop
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private void dataGridViewMetadataReadPriority_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dataGridView.DoDragDrop(dataGridView.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
        }

        private void dataGridViewMetadataReadPriority_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dataGridView.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndexFromMouseDown);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                {
                    // Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being at the center of the rectangle.
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
                else
                {
                    rowIndexFromMouseDown = -1;
                    dragBoxFromMouseDown = Rectangle.Empty;
                }
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dataGridViewMetadataReadPriority_DragDrop(object sender, DragEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = dataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {


                DataGridViewGenericRow dataGridViewGenericRowFrom = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndexFromMouseDown);
                DataGridViewGenericRow dataGridViewGenericRowTo = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndexOfItemUnderMouseToDrop);

                if (dataGridViewGenericRowFrom != null && !dataGridViewGenericRowFrom.IsHeader &&
                    dataGridViewGenericRowTo != null && dataGridViewGenericRowTo.IsHeader)
                {

                    MetadataPriorityValues metadataPriorityValues = metadataPrioityDictionaryCopy[dataGridViewGenericRowFrom.MetadataPriorityKey];
                    metadataPriorityValues.Composite = dataGridViewGenericRowTo.HeaderName;
                }
                int toRowIndex = rowIndexOfItemUnderMouseToDrop + (rowIndexFromMouseDown < rowIndexOfItemUnderMouseToDrop ? 0 : 1);

                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridView.Rows.RemoveAt(rowIndexFromMouseDown);
                dataGridView.Rows.Insert(toRowIndex, rowToMove);
                dataGridView.CurrentCell = DataGridViewHandler.GetCellDataGridViewCell(dataGridView, 0, toRowIndex);
            }

        }

        private void dataGridViewMetadataReadPriority_DragOver(object sender, DragEventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            Point clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));
            // Get the row index of the item the mouse is below. 
            int rowIndex = dataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (dataGridViewGenericRow != null && dataGridViewGenericRow.IsHeader)
            {
                e.Effect = DragDropEffects.Move;
            }
            else e.Effect = DragDropEffects.None;
        }
        #endregion

        #region Config - Read Metadata - Cell Changed
        private bool isCellValueUpdating = false;
        private void dataGridViewMetadataReadPriority_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isCellValueUpdating) return;
            DataGridView dataGridView = (DataGridView)sender;
            string value = DataGridViewHandler.GetCellValueStringTrim(dataGridView, e.ColumnIndex, e.RowIndex);
            if (int.TryParse(value.ToString(), out int priority))
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                {
                    MetadataPriorityValues metadataPriorityValues = metadataPrioityDictionaryCopy[dataGridViewGenericRow.MetadataPriorityKey];
                    metadataPriorityValues.Priority = priority;
                }
            }
            else
            {
                isCellValueUpdating = true;
                DataGridViewHandler.SetCellValue(dataGridView, e.ColumnIndex, e.RowIndex, 100);
                isCellValueUpdating = false;
            }
        }
        #endregion

        #region Config - Read Metadata - Keydown and Item Click, Clipboard
        private void dataGridViewMetadataReadPriority_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }

        private void toolStripMenuItemMetadataReadCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }

        private void toolStripMenuItemMetadataReadCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
        }

        private void toolStripMenuItemMetadataReadPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
        }

        private void toolStripMenuItemMetadataReadDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }

        private void toolStripMenuItemMetadataReadUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.UndoDataGridView(dataGridView);
        }

        private void toolStripMenuItemMetadataReadRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.RedoDataGridView(dataGridView);
        }

        private void toolStripMenuItemMetadataReadFind_Click(object sender, EventArgs e)
        {
            //string header = DataGridViewHandlerX.headerKeywords;
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }

        private void toolStripMenuItemMetadataReadReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }

        private void toolStripMenuItemMetadataReadMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }

        private void toolStripMenuItemMetadataReadRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }

        private void toolStripMenuItemMetadataReadToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }

        private void toolStripMenuItemMetadataReadShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemMetadataReadShowFavorite);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, false, toolStripMenuItemMetadataReadShowFavorite.Checked);
        }

        private void dataGridViewMetadataReadPriority_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Config - Read Metadata - CellPaining 
        private void dataGridViewMetadataReadPriority_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            //DataGridViewUpdateThumbnail(dataGridView, e);
            DataGridViewHandler.CellPaintingHandleDefault(sender, e);
            //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        private void tabControlConfig_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Config_Load(object sender, EventArgs e)
        {
            dataGridViewMetadataReadPriority.Focus();
        }

        
    }
}
