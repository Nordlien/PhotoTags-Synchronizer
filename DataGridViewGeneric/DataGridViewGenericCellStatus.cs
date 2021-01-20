using MetadataLibrary;
using WinProps;

namespace DataGridViewGeneric
{
    public class DataGridViewGenericCellStatus
    {
        public DataGridViewGenericCellStatus()
        {
            MetadataBrokerTypes = MetadataBrokerType.Empty;
            SwitchState = SwitchStates.Disabled;
            CellReadOnly = false;
        }

        public DataGridViewGenericCellStatus(bool cellReadOnly) : this (MetadataBrokerType.Empty, SwitchStates.Disabled, cellReadOnly)
        {
        }

        public DataGridViewGenericCellStatus(DataGridViewGenericCellStatus dataGridViewGenericCellStatus) : this(dataGridViewGenericCellStatus.MetadataBrokerTypes, dataGridViewGenericCellStatus.SwitchState, dataGridViewGenericCellStatus.CellReadOnly)
        { }

        public DataGridViewGenericCellStatus(MetadataBrokerType metadataBrokerTypes, SwitchStates switchStates, bool cellReadOnly)
        {
            MetadataBrokerTypes = metadataBrokerTypes;
            SwitchState = switchStates;
            CellReadOnly = cellReadOnly;
        }

        public MetadataBrokerType MetadataBrokerTypes { get; set; }
        public SwitchStates SwitchState { get; set; }
        public bool CellReadOnly { get; set; }
        
    }
}
