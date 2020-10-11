using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MetadataPriorityLibrary
{

    public class MetadataPriorityGroup : IComparable<MetadataPriorityGroup> //,*/ IComparer<MetadataPriorityGroup>
    {
        [JsonProperty("MetadataPriorityKey")]
        public MetadataPriorityKey MetadataPriorityKey { get; set; }

        [JsonProperty("MetadataPriorityValues")]
        public MetadataPriorityValues MetadataPriorityValues { get; set; }


        public MetadataPriorityGroup(MetadataPriorityKey metadataPriorityKey, MetadataPriorityValues metadataPriorityValues)
        {
            this.MetadataPriorityKey = metadataPriorityKey; // new MetadataPriorityKey(metadataGroup);
            this.MetadataPriorityValues = metadataPriorityValues; // new MetadataPriorityValues(metadataPriorityValues);
        }

        
        public int CompareTo(MetadataPriorityGroup other)
        {
            string thisComposite = this.MetadataPriorityValues.Composite;
            string otherComposite = other.MetadataPriorityValues.Composite;
            if (thisComposite == CompositeTags.Ignore) thisComposite = "ZZZ1" + thisComposite; //Sort it next last
            if (otherComposite == CompositeTags.Ignore) otherComposite = "ZZZ1" + otherComposite; //Sort it next last
            if (thisComposite == CompositeTags.NotDefined) thisComposite = "ZZZ2" + thisComposite; //Sort it last
            if (otherComposite == CompositeTags.NotDefined) otherComposite = "ZZZ2" + otherComposite; //Sort it last

            if (string.Compare(thisComposite, otherComposite) != 0) return string.Compare(thisComposite, otherComposite);
            if (string.Compare(this.MetadataPriorityKey.Region, other.MetadataPriorityKey.Region) != 0) return string.Compare(this.MetadataPriorityKey.Region, other.MetadataPriorityKey.Region);
            return string.Compare(this.MetadataPriorityKey.Tag, other.MetadataPriorityKey.Tag);

        }

        public override bool Equals(object obj)
        {
            return obj is MetadataPriorityGroup group &&
                   EqualityComparer<MetadataPriorityKey>.Default.Equals(MetadataPriorityKey, group.MetadataPriorityKey) &&
                   EqualityComparer<MetadataPriorityValues>.Default.Equals(MetadataPriorityValues, group.MetadataPriorityValues);
        }

        public override int GetHashCode()
        {
            int hashCode = -887867187;
            hashCode = hashCode * -1521134295 + EqualityComparer<MetadataPriorityKey>.Default.GetHashCode(MetadataPriorityKey);
            hashCode = hashCode * -1521134295 + EqualityComparer<MetadataPriorityValues>.Default.GetHashCode(MetadataPriorityValues);
            return hashCode;
        }

        public override string ToString()
        {
            return MetadataPriorityValues.ToString() + "\r\n" + MetadataPriorityKey.ToString();
        }

        public static bool operator ==(MetadataPriorityGroup left, MetadataPriorityGroup right)
        {
            return EqualityComparer<MetadataPriorityGroup>.Default.Equals(left, right);
        }

        public static bool operator !=(MetadataPriorityGroup left, MetadataPriorityGroup right)
        {
            return !(left == right);
        }
    }
}