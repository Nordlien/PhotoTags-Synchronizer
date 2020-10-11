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
        public DataGridViewGenericCell(object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatus)
        {
            Value = value;
            CellStatus = dataGridViewGenericCellStatus;
        }

        public object Value { get; set; }
        public DataGridViewGenericCellStatus CellStatus { get; set; }

        

    }
}
