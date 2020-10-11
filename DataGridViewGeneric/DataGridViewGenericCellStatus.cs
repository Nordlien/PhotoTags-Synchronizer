using MetadataLibrary;
using WinProps;

namespace DataGridViewGeneric
{
    public class DataGridViewGenericCellStatus
    {
        public DataGridViewGenericCellStatus()
        {
            MetadataBrokerTypes = MetadataBrokerTypes.Empty;
            SwitchState = SwitchStates.Disabled;
            CellReadOnly = false;
        }

        public DataGridViewGenericCellStatus(bool cellReadOnly) : this (MetadataBrokerTypes.Empty, SwitchStates.Disabled, cellReadOnly)
        {
        }

        public DataGridViewGenericCellStatus(DataGridViewGenericCellStatus dataGridViewGenericCellStatus) : this(dataGridViewGenericCellStatus.MetadataBrokerTypes, dataGridViewGenericCellStatus.SwitchState, dataGridViewGenericCellStatus.CellReadOnly)
        { }

        public DataGridViewGenericCellStatus(MetadataBrokerTypes metadataBrokerTypes, SwitchStates switchStates, bool cellReadOnly)
        {
            MetadataBrokerTypes = metadataBrokerTypes;
            SwitchState = switchStates;
            CellReadOnly = cellReadOnly;
        }

        public MetadataBrokerTypes MetadataBrokerTypes { get; set; }
        public SwitchStates SwitchState { get; set; }
        public bool CellReadOnly { get; set; }
        
    }
}
