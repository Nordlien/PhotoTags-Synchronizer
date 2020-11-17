using System;
using System.Collections.Generic;
using System.Linq;


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

        private float latitudeE7;
        public float Latitude { get => latitudeE7; set => latitudeE7 = value; }

        
        /*public string LatitudeE7
        {
            get
            {
                return (latitudeE7 * 10000000).ToString();
            }
            set
            {
                //They seem to have an integer overflow error in preparing the data for the takeout (downloading the kml directly from google maps for a specific day works correct).
                //If the number is greater than 1800000000 for longitude, 900000000 for latitude you need to subtract 2^32 (=4294967296) and you get the correct latitudeE7 or longitudeE7.

                latitudeE7 = (float.Parse(value));
                if (latitudeE7 > 900000000.0) latitudeE7 -= 4294967296.0F;
                latitudeE7 /= 10000000.0F;
            }
        }*/
        private float longitudeE7; 


        public float Longitude { get => longitudeE7; set => longitudeE7 = value; }
        /*public string LongitudeE7
        {
            get
            {
                return (longitudeE7 * 10000000).ToString();
            }
            set
            {
                //They seem to have an integer overflow error in preparing the data for the takeout (downloading the kml directly from google maps for a specific day works correct).
                //If the number is greater than 1800000000 for longitude, 900000000 for latitude you need to subtract 2^32 (=4294967296) and you get the correct latitudeE7 or longitudeE7.

                longitudeE7 = float.Parse(value);
                if (longitudeE7 > 1800000000.0) longitudeE7 -= 4294967296.0F;
                longitudeE7 /= 10000000.0F;
            }
        }*/

        private float altitude;
        public float Altitude
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
        public float Accuracy
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

    public class GoogleLocationHistoryItems
    {
        private String googleTakeoutAccount;
        private SortedList<DateTime, GoogleJsonLocations> googleLocationItems;

        public GoogleLocationHistoryItems(string userAccount)
        {
            googleTakeoutAccount = userAccount;
            googleLocationItems = new SortedList<DateTime, GoogleJsonLocations>();
        }

        /*
        public string GoogleTakeoutAccount { get => googleTakeoutAccount; set => googleTakeoutAccount = value; }
        public SortedList<DateTime, GoogleJsonLocations> GoogleLocationItems { get => googleLocationItems; set => googleLocationItems = value; }
        public long Count { get => googleLocationItems.Count; }
        */
        public void Add(GoogleJsonLocations googleJsonLocationsItem)
        {
            googleLocationItems.Add(googleJsonLocationsItem.Timestamp, googleJsonLocationsItem);
        }

        /*
        public Int32 BinarySearch(DateTime thisValue)
        {
            // Check to see if we need to search the list.
            if (googleLocationItems == null || googleLocationItems.Count <= 0) { return -1; }
            if (googleLocationItems.Count == 1) { return 0; }

            // Setup the variables needed to find the closest index
            int lower = 0;
            int upper = googleLocationItems.Count - 1;
            int index = (lower + upper) / 2;

            // Find the closest index (rounded down)
            bool searching = true;
            while (searching)
            {
                int comparisonResult = DateTime.Compare(thisValue, googleLocationItems.Keys[index]);
                if (comparisonResult == 0) { return index; } //Found
                else if (comparisonResult < 0) { upper = index - 1; }
                else { lower = index + 1; }

                index = (lower + upper) / 2;
                if (lower > upper) { searching = false; }
            }

            // Check to see if we are under or over the max values.
            if (index >= googleLocationItems.Count - 1) { return googleLocationItems.Count - 1; }
            if (index < 0) { return 0; }
            if (googleLocationItems.Keys[index + 1] - thisValue < thisValue - (googleLocationItems.Keys[index])) { index++; }

            return index;
        }

        public Double? GetBestLongitude(Int32 i, DateTime dateTime)
        {

            if (i < 0) return null;
            if (i > googleLocationItems.Count() - 2) return null; //No extra value to compare with

            DateTime lowDateTime = googleLocationItems.Values[i].Timestamp;
            DateTime highDateTime = googleLocationItems.Values[i].Timestamp;
            Double lowValue = googleLocationItems.Values[i].Longitude;
            Double highValue = googleLocationItems.Values[i].Longitude;

            //10:00[i]  (DateTime 11:59) 
            if (dateTime < googleLocationItems.Values[i].Timestamp)
            {
                lowDateTime = googleLocationItems.Values[i - 1].Timestamp;
                highDateTime = googleLocationItems.Values[i].Timestamp;
                lowValue = googleLocationItems.Values[i - 1].Longitude;
                highValue = googleLocationItems.Values[i].Longitude;
            }
            else
            {
                lowDateTime = googleLocationItems.Values[i].Timestamp;
                highDateTime = googleLocationItems.Values[i + 1].Timestamp;
                lowValue = googleLocationItems.Values[i].Longitude;
                highValue = googleLocationItems.Values[i + 1].Longitude;
            }


            Double lowDiffInSeconds = Math.Abs((lowDateTime - (DateTime)dateTime).TotalSeconds);
            Double highDiffInSeconds = Math.Abs((highDateTime - (DateTime)dateTime).TotalSeconds);
            Double totalDifInSeconds = lowDiffInSeconds + highDiffInSeconds;

            return (
                    (lowValue * (totalDifInSeconds - lowDiffInSeconds)) +
                    (highValue * (totalDifInSeconds - highDiffInSeconds))
                   ) / totalDifInSeconds;
        }

        public Double? GetBestAltitude(Int32 i, DateTime dateTime)
        {
            if (i < 0) return null;
            if (i > googleLocationItems.Count() - 2) return null; //No extra value to compare with

            DateTime lowDateTime = googleLocationItems.Values[i].Timestamp;
            DateTime highDateTime = googleLocationItems.Values[i].Timestamp;
            Double lowValue = googleLocationItems.Values[i].Altitude;
            Double highValue = googleLocationItems.Values[i].Altitude;

            //10:00[i]  (DateTime 11:59) 
            if (dateTime < googleLocationItems.Values[i].Timestamp)
            {
                lowDateTime = googleLocationItems.Values[i - 1].Timestamp;
                highDateTime = googleLocationItems.Values[i].Timestamp;
                lowValue = googleLocationItems.Values[i - 1].Altitude;
                highValue = googleLocationItems.Values[i].Altitude;
            }
            else
            {
                lowDateTime = googleLocationItems.Values[i].Timestamp;
                highDateTime = googleLocationItems.Values[i + 1].Timestamp;
                lowValue = googleLocationItems.Values[i].Altitude;
                highValue = googleLocationItems.Values[i + 1].Altitude;
            }


            Double lowDiffInSeconds = Math.Abs((lowDateTime - (DateTime)dateTime).TotalSeconds);
            Double highDiffInSeconds = Math.Abs((highDateTime - (DateTime)dateTime).TotalSeconds);
            Double totalDifInSeconds = lowDiffInSeconds + highDiffInSeconds;

            return (
                    (lowValue * (totalDifInSeconds - lowDiffInSeconds)) +
                    (highValue * (totalDifInSeconds - highDiffInSeconds))
                   ) / totalDifInSeconds;
        }

        public Double? GetBestLatitude(Int32 i, DateTime dateTime)
        {

            if (i < 0) return null;
            if (i > googleLocationItems.Count() - 2) return null; //No extra value to compare with

            DateTime lowDateTime = googleLocationItems.Values[i].Timestamp;
            DateTime highDateTime = googleLocationItems.Values[i].Timestamp;
            Double lowValue = googleLocationItems.Values[i].Latitude;
            Double highValue = googleLocationItems.Values[i].Latitude;

            //10:00[i]  (DateTime 11:59) 
            if (dateTime < googleLocationItems.Values[i].Timestamp)
            {
                lowDateTime = googleLocationItems.Values[i - 1].Timestamp;
                highDateTime = googleLocationItems.Values[i].Timestamp;
                lowValue = googleLocationItems.Values[i - 1].Latitude;
                highValue = googleLocationItems.Values[i].Latitude;
            }
            else
            {
                lowDateTime = googleLocationItems.Values[i].Timestamp;
                highDateTime = googleLocationItems.Values[i + 1].Timestamp;
                lowValue = googleLocationItems.Values[i].Latitude;
                highValue = googleLocationItems.Values[i + 1].Latitude;
            }


            Double lowDiffInSeconds = Math.Abs((lowDateTime - (DateTime)dateTime).TotalSeconds);
            Double highDiffInSeconds = Math.Abs((highDateTime - (DateTime)dateTime).TotalSeconds);
            Double totalDifInSeconds = lowDiffInSeconds + highDiffInSeconds;

            return (
                    (lowValue * (totalDifInSeconds - lowDiffInSeconds)) +
                    (highValue * (totalDifInSeconds - highDiffInSeconds))
                   ) / totalDifInSeconds;
        }
        */
    }
}
