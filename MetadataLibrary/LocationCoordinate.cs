using System;
using System.Collections.Generic;

namespace MetadataLibrary
{
    public class LocationCoordinate
    {
        public LocationCoordinate()
        {
            Latitude = 0;
            Longitude = 0;
        }

        public LocationCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

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
            TryParse(locationCoordinateString, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out LocationCoordinate result);
            return result;
        }

        public static LocationCoordinate Parse(string locationCoordinateString, System.Globalization.NumberStyles style)
        {
            TryParse(locationCoordinateString, style, System.Globalization.CultureInfo.CurrentCulture, out LocationCoordinate result);
            return result;
        }

        public static LocationCoordinate Parse(string locationCoordinateString, System.IFormatProvider provider)
        {
            TryParse(locationCoordinateString, System.Globalization.NumberStyles.Float, provider, out LocationCoordinate result);
            return result;
        }

        public static LocationCoordinate Parse(string locationCoordinateString, System.Globalization.NumberStyles style, System.IFormatProvider provider)
        {
            LocationCoordinate result;
            if (TryParse(locationCoordinateString, style, provider, out result))
                return result;
            return null;
        }

        public static bool TryParse(string locationCoordinateString, out LocationCoordinate result)
        {
            return TryParse(locationCoordinateString, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        public static bool TryParse(string locationCoordinateString, System.Globalization.NumberStyles style, System.IFormatProvider provider, out LocationCoordinate result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(locationCoordinateString)) return false;
            string[] latitideAndlogitude = locationCoordinateString.Split((System.Globalization.CultureInfo.CurrentCulture).TextInfo.ListSeparator[0]);
            if (latitideAndlogitude.Length == 2)
            {
                float latitude;
                float longitude;
                if (
                    float.TryParse(latitideAndlogitude[0], style, provider, out latitude) && 
                    float.TryParse(latitideAndlogitude[1], style, provider, out longitude))
                {
                    result = new LocationCoordinate(latitude, longitude);
                    return true;
                }
            }
            return false;
        }

    }
}

