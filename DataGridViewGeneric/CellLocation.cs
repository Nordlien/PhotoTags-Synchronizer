namespace DataGridViewGeneric
{
    public class CellLocation
    {
        public CellLocation(int column, int row)
        {
            this.ColumnIndex = column;
            this.RowIndex = row;
        }

        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
    }
}
