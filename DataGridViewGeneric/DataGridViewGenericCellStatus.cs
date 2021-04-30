using MetadataLibrary;
using WinProps;

namespace DataGridViewGeneric
{
    public class DataGridViewGenericCellStatus
    {
        public static DataGridViewGenericCellStatus DefaultEmpty()
        {
            return new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true);
        }
        public DataGridViewGenericCellStatus()
        {
            MetadataBrokerType = MetadataBrokerType.Empty;
            SwitchState = SwitchStates.Disabled;
            CellReadOnly = false;
        }

        public DataGridViewGenericCellStatus(bool cellReadOnly) : this (MetadataBrokerType.Empty, SwitchStates.Disabled, cellReadOnly)
        {
        }

        public DataGridViewGenericCellStatus(DataGridViewGenericCellStatus dataGridViewGenericCellStatus) : this(dataGridViewGenericCellStatus.MetadataBrokerType, dataGridViewGenericCellStatus.SwitchState, dataGridViewGenericCellStatus.CellReadOnly)
        { }

        public DataGridViewGenericCellStatus(MetadataBrokerType metadataBrokerTypes, SwitchStates switchStates, bool cellReadOnly)
        {
            MetadataBrokerType = metadataBrokerTypes;
            SwitchState = switchStates;
            CellReadOnly = cellReadOnly;
        }

        public MetadataBrokerType MetadataBrokerType { get; set; }
        public SwitchStates SwitchState { get; set; }
        public bool CellReadOnly { get; set; }
        
    }
}
