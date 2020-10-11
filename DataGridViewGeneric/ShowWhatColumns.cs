namespace DataGridViewGeneric
{
    public enum ShowWhatColumns
    {
        ErrorColumns = 1,
        HistoryColumns = 2
    }

    public class ShowWhatColumnHandler
    {
        public static ShowWhatColumns SetShowWhatColumns(bool histortyColumns, bool errorColumns)
        {
            return (histortyColumns ? ShowWhatColumns.HistoryColumns : 0) | (errorColumns ? ShowWhatColumns.ErrorColumns : 0);
        }
    }

}









