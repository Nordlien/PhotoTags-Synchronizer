using System;
using System.Windows.Forms;
using DataGridViewGeneric;
using TimeZone;
using Krypton.Toolkit;
using System.Collections.Generic;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        private FormLocationHistoryAnalytics formLocationHistoryAnalytics = new FormLocationHistoryAnalytics();

        #region ShowFormLocationHistoryAnalyticsInit
        private void ShowFormLocationHistoryAnalyticsInit()
        {
            if (formLocationHistoryAnalytics == null || formLocationHistoryAnalytics.IsDisposed) formLocationHistoryAnalytics = new FormLocationHistoryAnalytics();
            formLocationHistoryAnalytics.Owner = this;
            formLocationHistoryAnalytics.GoogleLocationHistoryDatabaseCache = databaseGoogleLocationHistory;
            formLocationHistoryAnalytics.KryptonPalette = (KryptonPalette)kryptonManager1.GlobalPalette;
        }
        #endregion

        #region ShowFormLocationHistoryAnalytics
        private void ShowFormLocationHistoryAnalytics(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            ShowFormLocationHistoryAnalyticsInit();
            formLocationHistoryAnalytics.DefaultDateTimeFrom = dateTimeFrom;
            formLocationHistoryAnalytics.DefaultDateTimeTo = dateTimeTo;
            if (formLocationHistoryAnalytics.WindowState == FormWindowState.Minimized) formLocationHistoryAnalytics.WindowState = FormWindowState.Normal;
            formLocationHistoryAnalytics.BringToFront();
            formLocationHistoryAnalytics.Show();
        }
        #endregion

        #region AddDatesFound
        private void AddDatesFound(DateTime dateTime, ref List<DateTime> dates)
        {
            DateTime dateTimeFound = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
            if (!dates.Contains(dateTimeFound.AddDays(-1))) dates.Add(dateTimeFound.AddDays(-1));
            if (!dates.Contains(dateTimeFound)) dates.Add(dateTimeFound);
            if (!dates.Contains(dateTimeFound.AddDays(1))) dates.Add(dateTimeFound.AddDays(1));
        }
        #endregion

        #region ShowFormLocationHistoryAnalytics
        private void ShowFormLocationHistoryAnalytics()
        {
            List<DateTime> datesFound = new List<DateTime>();
            DateTime? dateTimeFrom = null;
            DateTime? dateTimeTo = null;
            DataGridView dataGridViewActive = GetActiveTabDataGridView();
            if (DataGridViewHandler.GetIsAgregated(dataGridViewActive))
            {
                ShowFormLocationHistoryAnalyticsInit();
                formLocationHistoryAnalytics.PopulateMetadataLocationsClear(formLocationHistoryAnalytics.DataGridView);

                foreach (int columnIndex in DataGridViewHandler.GetColumnSelected(dataGridViewActive))
                {
                    DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridViewActive, columnIndex);

                    if (dataGridViewGenericColumn != null)
                    {
                        DateTime? date = DataGridViewHandlerDate.GetUserInputDateTaken(dataGridViewDate, null, dataGridViewGenericColumn.FileEntryAttribute);
                        if (date != null)
                        {
                            AddDatesFound((DateTime)date, ref datesFound);
                            if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                            if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                        }

                        date = DataGridViewHandlerDate.GetUserInputLocationDate(dataGridViewDate, null, dataGridViewGenericColumn.FileEntryAttribute);
                        if (date != null)
                        {
                            AddDatesFound((DateTime)date, ref datesFound);
                            if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                            if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                        }

                        if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.Metadata != null)
                        {
                            date = dataGridViewGenericColumn.Metadata.MediaDateTaken;
                            if (date != null)
                            {
                                AddDatesFound((DateTime)date, ref datesFound);
                                if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                            }

                            date = dataGridViewGenericColumn.Metadata.LocationDateTime;
                            if (date != null)
                            {
                                AddDatesFound((DateTime)date, ref datesFound);
                                if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                            }

                            date = dataGridViewGenericColumn.Metadata.FileDateCreated;
                            if (date != null)
                            {
                                if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                            }

                            date = dataGridViewGenericColumn.Metadata.FileDateModified;
                            if (date != null)
                            {
                                if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                            }

                            if (dataGridViewGenericColumn.Metadata.FileDateCreated != null && dataGridViewGenericColumn.Metadata.FileDateModified != null)
                            {
                                date = (dataGridViewGenericColumn.Metadata.FileDateCreated < dataGridViewGenericColumn.Metadata.FileDateModified ? dataGridViewGenericColumn.Metadata.FileDateCreated : dataGridViewGenericColumn.Metadata.FileDateModified);
                                AddDatesFound((DateTime)date, ref datesFound);
                            }
                        }

                    }
                }

                DateTime? dateTimeFoundFrom = null;
                DateTime? dateTimeFoundTo = null;
                for (int index = 0; index < datesFound.Count - 1; index++)
                {
                    if (index == 0) dateTimeFoundFrom = datesFound[index];
                    if (datesFound[index].AddDays(1) != datesFound[index + 1]) dateTimeFoundTo = datesFound[index];
                    if (index == datesFound.Count - 2) dateTimeFoundTo = datesFound[index + 1];
                    if (dateTimeFoundFrom != null && dateTimeFoundTo == null) dateTimeFoundTo = ((DateTime)dateTimeFoundFrom).AddDays(1);
                    if (dateTimeFoundFrom == null && dateTimeFoundTo != null) dateTimeFoundFrom = ((DateTime)dateTimeFoundTo).AddDays(-1);

                    if (dateTimeFoundFrom != null && dateTimeFoundTo != null)
                    {
                        formLocationHistoryAnalytics.PopulateMetadataLocationsAdd(formLocationHistoryAnalytics.DataGridView, (DateTime)dateTimeFoundFrom, ((DateTime)dateTimeFoundTo).AddDays(1).AddMilliseconds(-1), Properties.Settings.Default.LocationAnalyticsMinimumTimeInterval * 60, (float)Properties.Settings.Default.LocationAnalyticsMinimumDistance);
                        dateTimeFoundFrom = datesFound[index + 1];
                        dateTimeFoundTo = null;
                    }
                }

            }

            if (dateTimeFrom == null) dateTimeFrom = DateTime.Now.AddDays(-1);
            if (dateTimeTo == null) dateTimeTo = DateTime.Now;
            ShowFormLocationHistoryAnalytics(
                new DateTime(((DateTime)dateTimeFrom).Year, ((DateTime)dateTimeFrom).Month, ((DateTime)dateTimeFrom).Day, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(((DateTime)dateTimeTo).Year, ((DateTime)dateTimeTo).Month, ((DateTime)dateTimeTo).Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1));
        }
        #endregion

        
    }
}
