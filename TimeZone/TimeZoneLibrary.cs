using GeoTimeZone;
using System;
using System.Collections.Generic;
using System.Globalization;
using TimeZoneConverter;
using TimeZoneNames;

namespace TimeZone
{
    public class TimeZoneLibrary
    {
        public const string ToW3CDTFformat = "yyyy-MM-ddTHH:mm:sszzz";
        public const string ToW3CDTFformatReadable = "yyyy-MM-dd HH:mm:sszzz";
        private const string DateTimeSortable = "yyyy-MM-dd HH:mm:ss";
        public static readonly string[] AllowedDateTimeFormatsWithTimeZone = new string[]
        {
            "yyyy:MM:ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH-mm-sszzz",
            "yyyy-MM-ddTHH:mm:sszzz",

            "yyyy:MM:dd HH:mm:sszzz",
            "yyyy-MM-dd HH:mm:sszzz",
            "yyyy-MM-dd HH-mm-sszzz"
        };

        public static readonly string[] AllowedDateTimeFormatsWithoutTimeZone = new string[]
        {
            
            "yyyy:MM:ddTHH:mm:ss",
            "yyyy-MM-ddTHH-mm-ss",
            "yyyy-MM-ddTHH:mm:ss",

            "yyyy:MM:dd HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH-mm-ss"
        };

        public static DateTime? ParseDateTimeAsLocal(string dataTimeString)
        {
            if (
                DateTime.TryParseExact(dataTimeString, TimeZoneLibrary.AllowedDateTimeFormatsWithoutTimeZone, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out DateTime dateTime) ||
                DateTime.TryParse(dataTimeString, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out dateTime))
            {
                return dateTime;
            }
            return null;
        }

        public static DateTime? ParseDateTimeAsUTC(string dataTimeString)
        {
            if (
                 DateTime.TryParseExact(dataTimeString, TimeZoneLibrary.AllowedDateTimeFormatsWithoutTimeZone, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal, out DateTime dateTime) ||
                 DateTime.TryParse(dataTimeString, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal, out dateTime))
            {                
                return dateTime;
            }

            return null;
        }

        public static DateTimeOffset? ParseDateTimeOffsetAsUTC(string dataTimeString)
        {
            if (DateTimeOffset.TryParseExact(dataTimeString, TimeZoneLibrary.AllowedDateTimeFormatsWithTimeZone, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal, out DateTimeOffset dateTimeZoneResult))
            {
                return dateTimeZoneResult;
            }
            return null;
        }

        public static TimeZoneInfo GetTimeZoneInfoOnGeoLocation(double latitude, double longitude)
        {
            TimeZoneResult timeZoneResult = TimeZoneLookup.GetTimeZone(latitude, longitude);
            return TZConvert.GetTimeZoneInfo(timeZoneResult.Result);
        }

        public static string TimeZoneNameStandarOrDaylight (TimeZoneInfo timeZoneInfo, DateTime date)
        {
            return timeZoneInfo.IsDaylightSavingTime(date) ? timeZoneInfo.DaylightName : timeZoneInfo.StandardName;
        }
        public static string GetTimeZoneName(TimeSpan timeSpan, DateTime date, string prefredTimeZoneName, out string alternatives)
        {
            string timeZoneName = "";
            alternatives = "";

            foreach (string id in TZConvert.KnownWindowsTimeZoneIds)
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(id);
                DateTime dateTime = new DateTime(
                    ((DateTime)date).Year, ((DateTime)date).Month, ((DateTime)date).Day,
                    ((DateTime)date).Hour, ((DateTime)date).Minute, ((DateTime)date).Second, ((DateTime)date).Millisecond, DateTimeKind.Utc);
                DateTime dateTimeUTC = TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo);

                TimeSpan baseUtcOffsetSummerAndWinder =                    
                    new DateTime(
                        dateTimeUTC.Year, dateTimeUTC.Month, dateTimeUTC.Day,
                        dateTimeUTC.Hour, dateTimeUTC.Minute, dateTimeUTC.Second, dateTimeUTC.Millisecond) - dateTime;

                if (Math.Abs((baseUtcOffsetSummerAndWinder - timeSpan).TotalMinutes) < 15)
                {                    
                    alternatives = alternatives + timeZoneInfo.DisplayName + "\r\n";
                    if (string.IsNullOrWhiteSpace(timeZoneName)) timeZoneName = TimeZoneNameStandarOrDaylight(timeZoneInfo, dateTime);
                    if (prefredTimeZoneName == timeZoneInfo.DisplayName) timeZoneName = TimeZoneNameStandarOrDaylight(timeZoneInfo, dateTime);
                }
            }

            return timeZoneName;
        }

        public static string GetTimeZoneDisplayName(double latitude, double longitude)
        {
            TimeZoneResult timeZoneResult = TimeZoneLookup.GetTimeZone(latitude, longitude);
            return TZConvert.GetTimeZoneInfo(timeZoneResult.Result).DisplayName;
        }

        public static string ToStringDateTimeSortable(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString(DateTimeSortable);
        }

        public static string ToStringDateTimeSortable(DateTimeOffset? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToString(DateTimeSortable);
        }

        public static string ToStringW3CDTF(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString(ToW3CDTFformatReadable);  
        }

        public static string ToStringW3CDTF_UTC(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToString(DateTimeSortable) + "Z";
        }

        public static string ToStringW3CDTF_UTC_Convert(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToUniversalTime().ToString(DateTimeSortable) + "Z";
        }



    }
}
