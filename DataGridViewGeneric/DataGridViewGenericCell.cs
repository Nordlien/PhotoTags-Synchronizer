using System.Collections.Generic;
using WinProps;

namespace DataGridViewGeneric
{

    public class DataGridViewGenericRowAndValue
    {
        public DataGridViewGenericRowAndValue(string headerName, PropertyKey propertyKey, bool cellReadOnly)
        {
            DataGridViewGenericRow = new DataGridViewGenericRow(headerName); 
            DataGridViewGenericCell = new DataGridViewGenericCell(null, new DataGridViewGenericCellStatus(cellReadOnly));
        }

        public DataGridViewGenericRowAndValue(string headerName, string rowName, PropertyKey propertyKey, ReadWriteAccess readWriteAccess, bool isMultiLine, bool cellReadyOnly, object value)
        {
            DataGridViewGenericRow = new DataGridViewGenericRow(headerName, rowName, readWriteAccess, isMultiLine, propertyKey);
            DataGridViewGenericCell = new DataGridViewGenericCell(value, new DataGridViewGenericCellStatus(cellReadyOnly));
        }

        
        public DataGridViewGenericRowAndValue(DataGridViewGenericRow dataGridViewGenericRow, DataGridViewGenericCell dataGridViewGenericCell)
        {
            DataGridViewGenericRow = dataGridViewGenericRow;
            DataGridViewGenericCell = dataGridViewGenericCell;
        }

        public DataGridViewGenericRow DataGridViewGenericRow { get; set; }
        public DataGridViewGenericCell DataGridViewGenericCell { get; set; }
    }

    public class DataGridViewGenericCell
    {
        public DataGridViewGenericCell(DataGridViewGenericCell dataGridViewGenericCell) : this (dataGridViewGenericCell.Value, dataGridViewGenericCell.CellStatus)
        {
        }

        public DataGridViewGenericCell(object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatus)
        {
            Value = DataGridViewHandler.DeepCopy(value);
            CellStatus = dataGridViewGenericCellStatus == null ? null : new DataGridViewGenericCellStatus(dataGridViewGenericCellStatus);
        }

        public object Value { get; set; }
        public DataGridViewGenericCellStatus CellStatus { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DataGridViewGenericCell cellCheck && 
                cellCheck.Value != null &&
                !(cellCheck.Value is string) &&
                !(cellCheck.Value is MetadataLibrary.RegionStructure))
            {
                //DEBUG
            }

            return obj is DataGridViewGenericCell cell &&
                   EqualityComparer<object>.Default.Equals(Value, cell.Value) &&
                   EqualityComparer<DataGridViewGenericCellStatus>.Default.Equals(CellStatus, cell.CellStatus);
        }

        public override int GetHashCode()
        {
            int hashCode = -1235765383;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + EqualityComparer<DataGridViewGenericCellStatus>.Default.GetHashCode(CellStatus);
            return hashCode;
        }

        public static bool operator ==(DataGridViewGenericCell left, DataGridViewGenericCell right)
        {
            return EqualityComparer<DataGridViewGenericCell>.Default.Equals(left, right);
        }

        public static bool operator !=(DataGridViewGenericCell left, DataGridViewGenericCell right)
        {
            return !(left == right);
        }
    }
}
