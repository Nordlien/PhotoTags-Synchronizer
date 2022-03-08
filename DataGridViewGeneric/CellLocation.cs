using System.Collections.Generic;
using System.Windows.Forms;

namespace DataGridViewGeneric
{
    public class CellLocation
    {
        public CellLocation(int column, int row)
        {
            this.ColumnIndex = column;
            this.RowIndex = row;
        }

        public CellLocation(DataGridViewCell cell)
        {
            this.ColumnIndex = cell.ColumnIndex;
            this.RowIndex = cell.RowIndex;
        }

        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CellLocation location &&
                   ColumnIndex == location.ColumnIndex &&
                   RowIndex == location.RowIndex;
        }

        public override int GetHashCode()
        {
            int hashCode = 196471078;
            hashCode = hashCode * -1521134295 + ColumnIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + RowIndex.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(CellLocation left, CellLocation right)
        {
            return EqualityComparer<CellLocation>.Default.Equals(left, right);
        }

        public static bool operator !=(CellLocation left, CellLocation right)
        {
            return !(left == right);
        }
    }
}
