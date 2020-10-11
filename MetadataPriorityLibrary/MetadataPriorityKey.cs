using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MetadataPriorityLibrary
{

    public class MetadataPriorityKey : IComparer<MetadataPriorityKey> 
    //Prioritization
    {
        [JsonProperty("Region")]
        public string Region { get; set; }

        [JsonProperty("Tag")]
        public string Tag { get; set; }


        public MetadataPriorityKey()
        {
        }


        public MetadataPriorityKey(MetadataPriorityKey metadataPriorityKey)
        {
            Region = metadataPriorityKey.Region;
            Tag = metadataPriorityKey.Tag;
        }
        public MetadataPriorityKey(string region, string command)
        {
            Region = region;
            Tag = command;
        }

        public int Compare(MetadataPriorityKey x, MetadataPriorityKey y)
        {
            if (string.Compare(x.Region, y.Region) != 0) return string.Compare(x.Region, y.Region);
            return string.Compare(x.Tag, y.Tag);
        }

        public override bool Equals(object obj)
        {
            return obj is MetadataPriorityKey key &&
                   Region == key.Region &&
                   Tag == key.Tag;
        }

        public override int GetHashCode()
        {
            int hashCode = 1545717336;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Region);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tag);
            return hashCode;
        }

        public override string ToString()
        {
            return "Exiftool: " + Region + " | " + Tag;
        }

        public static bool operator ==(MetadataPriorityKey left, MetadataPriorityKey right)
        {
            return EqualityComparer<MetadataPriorityKey>.Default.Equals(left, right);
        }

        public static bool operator !=(MetadataPriorityKey left, MetadataPriorityKey right)
        {
            return !(left == right);
        }
    }
}