using System;
using System.Windows.Forms;
using DataGridViewGeneric;
using TimeZone;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {


        #region Date description
        /*
        JPEG:
        EXIF:DateTimeOriginal - local time

        [XML]           CreationDateValue               : 2014:08:12 16:03:01+01:00
        [QuickTime]     CreateDate                      : 2014:08:12 16:03:01-04:00
        GPSDateStamp/GPSTimeStamp

        .MOV, .MP4
        creation_time - creation_time and other date properties are in UTC
        QuickTimeUTC
        QuickTimeUTC=1

        Most cameras store photos on flash media formatted using the FAT file system. 
        Conveniently, FAT uses local time. So, by comparing the metadata time against the file system time, 
        you can detect whether whether a video creation_time is in local or UTC. 
        If it's in UTC then the timezone can be detected from the difference.

        W3CDTF and ISO 8601
        Local time with timezone (Best Practice)	2018-11-28T13:25:04-05:00
				        2018-12-25T07:05:01+00:00
        Local time with	unstated timezone.		2018-11-28T13:25:04
				        2018-12-25T07:05:01
        UTC time with	unstated timezone.		2018-11-28T18:25:04Z
				        2018-12-25T07:05:01Z


        12	Minute	1976-07-04T21:05-05:00
        14	Second	1976-07-04T21:05:02-05:00
        17	Millisecond	1976-07-04T21:05:02.319-05:00
        */
        #endregion

        #region dataGridViewDate_CellValueChanged
        private bool isDataGridViewDate_CellValueChanging = false; 
        private void dataGridViewDate_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isDataGridViewDate_CellValueChanging) return; //To avoid loop and stack overflow
            if (GlobalData.IsApplicationClosing) return;
            //if (ClipboardUtility.IsClipboardActive) return;
            //if (GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress) return;
            if (e.ColumnIndex < 0) return;
            if (e.RowIndex < 0) return;

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;
            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;
            if (IsPopulatingAnything("Date Cell value changed")) return;

            DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
            if (gridViewGenericDataRow == null) return;
            if (!gridViewGenericDataRow.HeaderName.Equals(DataGridViewHandlerDate.headerMedia)) return;

            isDataGridViewDate_CellValueChanging = true;

            if (gridViewGenericDataRow.RowName.Equals(DataGridViewHandlerDate.tagMediaDateTaken)) //headerMedia, tagMediaDateTaken
            {
                string dataTimeString = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, e.ColumnIndex, e.RowIndex);

                DateTimeOffset? dateTimeZoneResult = TimeZoneLibrary.ParseDateTimeOffsetAsUTC(dataTimeString);
                if (dateTimeZoneResult != null) //If date and time has +00:00 offset
                {
                    DateTime? dateTimeLocal = TimeZoneLibrary.ParseDateTimeAsLocal(dataTimeString.Substring(0, TimeZoneLibrary.AllowedDateTimeFormatsWithoutTimeZone[0].Length));

                    DataGridViewHandler.AddRow(dataGridView, e.ColumnIndex, new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagMediaDateTaken),
                         TimeZoneLibrary.ToStringSortable(dateTimeLocal), false, false);

                    DataGridViewHandler.AddRow(dataGridView, e.ColumnIndex, new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagGPSLocationDateTime),
                        TimeZoneLibrary.ToStringW3CDTF_UTC(((DateTimeOffset)dateTimeZoneResult).UtcDateTime), false, false);
                }
                else
                {
                    DateTime? dateTime = TimeZoneLibrary.ParseDateTimeAsLocal(dataTimeString);
                    if (dateTime != null)
                    {
                        DataGridViewHandler.AddRow(dataGridView, e.ColumnIndex,
                            new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagMediaDateTaken),
                            TimeZoneLibrary.ToStringSortable((DateTime)dateTime), false, false);
                    }
                    else
                    {
                        DataGridViewHandler.AddRow(dataGridView, e.ColumnIndex,
                            new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagMediaDateTaken),
                            "Error", false, false);
                    }
                }
            }

            if (gridViewGenericDataRow.RowName.Equals(DataGridViewHandlerDate.tagGPSLocationDateTime)) //headerMedia, tagGPSLocationDateTime
            {
                string dataTimeString = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, e.ColumnIndex, e.RowIndex);

                DateTime? dateTime = TimeZoneLibrary.ParseDateTimeAsUTC(dataTimeString);
                if (dateTime != null)
                    DataGridViewHandler.AddRow(dataGridView, e.ColumnIndex,
                        new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagGPSLocationDateTime),
                        TimeZoneLibrary.ToStringW3CDTF_UTC((DateTime)dateTime), false, false);
                else
                    DataGridViewHandler.AddRow(dataGridView, e.ColumnIndex,
                        new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagGPSLocationDateTime),
                        "Error", false, false);
            }

            DataGridViewHandlerDate.PopulateTimeZone(dataGridView, e.ColumnIndex, null);
            UpdateGoodleHistoryCoordinateAndNearBy(e.ColumnIndex);
            
            isDataGridViewDate_CellValueChanging = false;
        }
        #endregion

        #region dataGridViewDate_CellEnter
        private void dataGridViewDate_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }
        #endregion

        #region dataGridViewDate_CellMouseClick
        private void dataGridViewDate_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (e.RowIndex == -1) RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }
        #endregion 
    }
}
