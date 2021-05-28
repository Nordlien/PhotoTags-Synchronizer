using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LocationNames
{
    
    public class LocationCoordinate
    {
        public LocationCoordinate()
        {
            Latitude = 0;
            Longitude = 0;
        }

        public LocationCoordinate(LocationCoordinate locationCoordinate) : this (locationCoordinate.Latitude, locationCoordinate.Longitude)
        { 
        }

        public LocationCoordinate(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        [JsonProperty("Latitude")]
        public float Latitude { get; set; }
        [JsonProperty("Longitude")]
        public float Longitude { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LocationCoordinate coordinates &&
                Latitude == coordinates.Latitude &&
                Longitude == coordinates.Longitude;
        }

        public override int GetHashCode()
        {
            int hashCode = -1416534245;
            hashCode = hashCode * -1521134295 + Latitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Longitude.GetHashCode();
            return hashCode;
        }

        public override string ToString() =>
            ((double)Latitude).ToString("N5") +
            System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + " " +
            ((double)Longitude).ToString("N5");

        public static bool operator ==(LocationCoordinate left, LocationCoordinate right)
        {           
            return EqualityComparer<LocationCoordinate>.Default.Equals(left, right);
        }

        public static bool operator !=(LocationCoordinate left, LocationCoordinate right)
        {
            return !(left == right);
        }

        public static LocationCoordinate Parse(string locationCoordinateString)
        {
            TryParse(locationCoordinateString, out LocationCoordinate result);
            return result;
        }

        public static bool TryParse(string locationCoordinateString, out LocationCoordinate result)
        {
            //System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, 
            result = null;
            if (string.IsNullOrWhiteSpace(locationCoordinateString)) return false;
            string[] latitideAndlogitude = locationCoordinateString.Split((System.Globalization.CultureInfo.CurrentCulture).TextInfo.ListSeparator[0]);
            if (latitideAndlogitude.Length == 2)
            {
                //System.Globalization.CultureInfo.CurrentCulture
                if (
                    float.TryParse(latitideAndlogitude[0], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float latitudeCurrentCulture) &&
                    float.TryParse(latitideAndlogitude[1], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float longitudeCurrentCulture))
                {
                    result = new LocationCoordinate(latitudeCurrentCulture, longitudeCurrentCulture);
                    return true;
                }

                //System.Globalization.CultureInfo.InvariantCulture
                if (
                    float.TryParse(latitideAndlogitude[0], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out float latitudeInvariantCulture) &&
                    float.TryParse(latitideAndlogitude[1], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out float longitudeInvariantCulture))
                {
                    result = new LocationCoordinate(latitudeInvariantCulture, longitudeInvariantCulture);
                    return true;
                }
            }
            return false;
        }

    }
}

