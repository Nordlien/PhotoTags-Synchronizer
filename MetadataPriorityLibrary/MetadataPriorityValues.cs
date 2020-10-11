using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MetadataPriorityLibrary
{
    public class MetadataPriorityValues : IComparer<MetadataPriorityValues>
    {
        [JsonProperty("Composite")]
        public string Composite { get; set; }

        [JsonProperty("Priority")]
        public int Priority { get; set; }

        public MetadataPriorityValues()
        {
        }

        public MetadataPriorityValues(MetadataPriorityValues metadataPriorityValues)
        {
            Composite = metadataPriorityValues.Composite;
            Priority = metadataPriorityValues.Priority;
        }

        public MetadataPriorityValues(string composite, int priority)
        {
            Composite = composite;
            Priority = priority;
        }


        public int Compare(MetadataPriorityValues x, MetadataPriorityValues y)
        {
            return string.Compare(x.Composite, y.Composite);
        }

        public override bool Equals(object obj)
        {
            return obj is MetadataPriorityValues values &&
                   Composite == values.Composite &&
                   Priority == values.Priority;
        }

        public override int GetHashCode()
        {
            int hashCode = -512198499;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Composite);
            hashCode = hashCode * -1521134295 + Priority.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return "In app usage: " + Composite + " Prioirty: " + Priority;
        }

        public static bool operator ==(MetadataPriorityValues left, MetadataPriorityValues right)
        {
            return EqualityComparer<MetadataPriorityValues>.Default.Equals(left, right);
        }

        public static bool operator !=(MetadataPriorityValues left, MetadataPriorityValues right)
        {
            return !(left == right);
        }
    }
}