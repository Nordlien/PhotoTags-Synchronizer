using MetadataLibrary;
using System.Collections.Generic;
using WinProps;

namespace DataGridViewGeneric
{
    public class DataGridViewGenericCellStatus
    {
        public static DataGridViewGenericCellStatus DefaultEmpty()
        {
            return new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true);
        }

        public override bool Equals(object obj)
        {
            return obj is DataGridViewGenericCellStatus status &&
                   MetadataBrokerType == status.MetadataBrokerType &&
                   SwitchState == status.SwitchState &&
                   CellReadOnly == status.CellReadOnly;
        }

        public override int GetHashCode()
        {
            int hashCode = 677046786;
            hashCode = hashCode * -1521134295 + MetadataBrokerType.GetHashCode();
            hashCode = hashCode * -1521134295 + SwitchState.GetHashCode();
            hashCode = hashCode * -1521134295 + CellReadOnly.GetHashCode();
            return hashCode;
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

        public static bool operator ==(DataGridViewGenericCellStatus left, DataGridViewGenericCellStatus right)
        {
            return EqualityComparer<DataGridViewGenericCellStatus>.Default.Equals(left, right);
        }

        public static bool operator !=(DataGridViewGenericCellStatus left, DataGridViewGenericCellStatus right)
        {
            return !(left == right);
        }
    }
}
