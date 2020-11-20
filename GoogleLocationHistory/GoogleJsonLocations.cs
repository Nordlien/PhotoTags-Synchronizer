using System;


namespace GoogleLocationHistory
{
    public class GoogleJsonLocations
    {
        private DateTime timestamp;

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public string TimestampMs
        {
            get { return (timestamp.AddYears(-1970).Millisecond).ToString(); }
            set { timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(long.Parse(value)); }
        }

        private Double latitudeE7;
        public double Latitude { get => latitudeE7; set => latitudeE7 = value; }
        public string LatitudeE7
        {
            get
            {
                return (latitudeE7 * 10000000).ToString();
            }
            set
            {
                //They seem to have an integer overflow error in preparing the data for the takeout (downloading the kml directly from google maps for a specific day works correct).
                //If the number is greater than 1800000000 for longitude, 900000000 for latitude you need to subtract 2^32 (=4294967296) and you get the correct latitudeE7 or longitudeE7.

                latitudeE7 = (Double.Parse(value));
                if (latitudeE7 > 900000000.0) latitudeE7 -= 4294967296.0;
                latitudeE7 /= 10000000.0;
            }
        }

        private Double longitudeE7;
        public double Longitude { get => longitudeE7; set => longitudeE7 = value; }
        public string LongitudeE7
        {
            get
            {
                return (longitudeE7 * 10000000).ToString();
            }
            set
            {
                //They seem to have an integer overflow error in preparing the data for the takeout (downloading the kml directly from google maps for a specific day works correct).
                //If the number is greater than 1800000000 for longitude, 900000000 for latitude you need to subtract 2^32 (=4294967296) and you get the correct latitudeE7 or longitudeE7.

                longitudeE7 = Double.Parse(value);
                if (longitudeE7 > 1800000000.0) longitudeE7 -= 4294967296.0;
                longitudeE7 /= 10000000.0;
            }
        }

        private double altitude;
        public Double Altitude
        {
            get
            {
                return altitude * 1;
            }
            set
            {
                altitude = value / 1;
            }
        }
        public Double Accuracy
        {
            get
            {
                return altitude;
            }
            set
            {
                altitude = value;
            }
        }

    }
}
