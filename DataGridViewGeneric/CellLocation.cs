namespace DataGridViewGeneric
{
    public class CellLocation
    {
        public CellLocation(int column, int row)
        {
            this.Column = column;
            this.Row = row;
        }

        public int Column { get; set; }
        public int Row { get; set; }
    }
}
