using Newtonsoft.Json;
using System.Collections.Generic;

namespace LocationNames
{

    public class LocationDescription
    {
        public LocationDescription()
        {
            Name = null;
            City = null;
            Region = null;
            Country = null;
        }

        public LocationDescription(LocationDescription locationDescription) : this (locationDescription.Name, locationDescription.City, locationDescription.Region, locationDescription.Country)
        {
        }

        public LocationDescription(string name, string city, string region, string country)
        {
            Name = name;
            City = city;
            Region = region;
            Country = country;
        }

        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("City")]
        public string City { get; set; }
        [JsonProperty("Region")]
        public string Region { get; set; }
        [JsonProperty("Country")]
        public string Country { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LocationDescription description &&
                   Name == description.Name &&
                   City == description.City &&
                   Region == description.Region &&
                   Country == description.Country;
        }

        public override int GetHashCode()
        {
            int hashCode = -1994374546;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(City);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Region);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Country);
            return hashCode;
        }

        public static bool operator ==(LocationDescription left, LocationDescription right)
        {
            return EqualityComparer<LocationDescription>.Default.Equals(left, right);
        }

        public static bool operator !=(LocationDescription left, LocationDescription right)
        {
            return !(left == right);
        }
    }
}

