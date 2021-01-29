#define MonoSqlite
#if MonoSqlite
#else
using System.Data.SQLite;
#endif
using System;
using System.Collections.Generic;

namespace MetadataLibrary
{
    public class MetadataRegionNameKey
    {
        public MetadataRegionNameKey(MetadataBrokerType metadataBrokerType, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            MetadataBrokerType = metadataBrokerType;
            DateTimeFrom = dateTimeFrom;
            DateTimeTo = dateTimeTo;
        }

        public MetadataBrokerType MetadataBrokerType { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MetadataRegionNameKey key &&
                   MetadataBrokerType == key.MetadataBrokerType &&
                   DateTimeFrom == key.DateTimeFrom &&
                   DateTimeTo == key.DateTimeTo;
        }

        public override int GetHashCode()
        {
            int hashCode = -793585214;
            hashCode = hashCode * -1521134295 + MetadataBrokerType.GetHashCode();
            hashCode = hashCode * -1521134295 + DateTimeFrom.GetHashCode();
            hashCode = hashCode * -1521134295 + DateTimeTo.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(MetadataRegionNameKey left, MetadataRegionNameKey right)
        {
            return EqualityComparer<MetadataRegionNameKey>.Default.Equals(left, right);
        }

        public static bool operator !=(MetadataRegionNameKey left, MetadataRegionNameKey right)
        {
            return !(left == right);
        }
    }
}
