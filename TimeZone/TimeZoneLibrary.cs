using GeoTimeZone;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
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

        public static bool IsTimeSpanEqual(TimeSpan timeSpanInGeneral, TimeSpan timeSpan, int acceptMinutesDifffence)
        {
            return (Math.Abs((timeSpanInGeneral - timeSpan).TotalMinutes) < acceptMinutesDifffence);
        }
        public static string GetTimeZoneName(TimeSpan? timeSpan, DateTime? date, string prefredTimeZoneName, out string alternatives)
        {
            alternatives = "";
            if (timeSpan == null) return "Unknown time zone";
            string timeZoneName = "";
            
            if (Regex.IsMatch(prefredTimeZoneName, @"^\((\+|\-)(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23):(0|1|2|3|4|5)\d\)[ ]"))
            {
                prefredTimeZoneName = prefredTimeZoneName.Remove(0, 9);
            }
                
            foreach (string id in TZConvert.KnownWindowsTimeZoneIds)
            {
                TimeZoneInfo timeZoneInfoToCheck = TimeZoneInfo.FindSystemTimeZoneById(id);

                
                if (date != null)
                {
                    //When using date time when check TimeSpan, we have knowlgde about **Standard timer** and **daylight saving***
                    DateTime dateTime = new DateTime(((DateTime)date).Ticks, DateTimeKind.Utc);
                    DateTime dateTimeAdjustedToTimeZone = TimeZoneInfo.ConvertTime(dateTime, timeZoneInfoToCheck);

                    TimeSpan timeSpanWithStandardOrDaylightKnowlegde = new DateTime(dateTimeAdjustedToTimeZone.Ticks) - dateTime;

                    //if (Math.Abs((timeSpanWithStandardOrDaylightKnowlegde - (TimeSpan)timeSpan).TotalMinutes) < 15)
                    if (IsTimeSpanEqual(timeSpanWithStandardOrDaylightKnowlegde, (TimeSpan)timeSpan, 15))
                        {
                            alternatives = alternatives + timeZoneInfoToCheck.DisplayName + "\r\n";
                        if (string.IsNullOrWhiteSpace(timeZoneName)) timeZoneName = TimeZoneNameStandarOrDaylight(timeZoneInfoToCheck, dateTime);
                        if (prefredTimeZoneName == timeZoneInfoToCheck.DaylightName) timeZoneName = TimeZoneNameStandarOrDaylight(timeZoneInfoToCheck, dateTime);
                        if (prefredTimeZoneName == timeZoneInfoToCheck.StandardName) timeZoneName = TimeZoneNameStandarOrDaylight(timeZoneInfoToCheck, dateTime);
                        if (prefredTimeZoneName == timeZoneInfoToCheck.DisplayName) timeZoneName = TimeZoneNameStandarOrDaylight(timeZoneInfoToCheck, dateTime);
                    }
                } else
                {
                    TimeSpan timeSpanInGeneral = timeZoneInfoToCheck.BaseUtcOffset - (TimeSpan)timeSpan;

                    if (IsTimeSpanEqual(timeSpanInGeneral, (TimeSpan)timeSpan, 15))
                    {
                        alternatives = alternatives + timeZoneInfoToCheck.DisplayName + "\r\n";
                        if (string.IsNullOrWhiteSpace(timeZoneName)) timeZoneName = timeZoneInfoToCheck.DisplayName;
                    }
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

        public static string ToStringOffset(TimeSpan timeSpan, bool withParenthesis)
        {
            return (withParenthesis ? "(" : "") + (timeSpan < new TimeSpan(0, 0, 0) ? "" : "+") + timeSpan.ToString().Substring(0, timeSpan.ToString().Length - 3) + (withParenthesis ? ")" :"");
        }

        public static string ToStringOffset(TimeSpan timeSpan)
        {
            return ToStringOffset(timeSpan, true);
        }

        //TimeZoneInfo timeZoneInfoGPSLocation = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataLocationLatitude, (double)metadataLocationLongitude);

        public static TimeSpan? CalulateTimeDiffrent(DateTime? dateTime1, DateTime? dateTime2)
        {
            if (dateTime1 != null && dateTime2 != null)
            {
                //Remove time zone and location information so we can substract  
                return
                    new DateTime(
                        ((DateTime)dateTime1).Year, ((DateTime)dateTime1).Month, ((DateTime)dateTime1).Day,
                        ((DateTime)dateTime1).Hour, ((DateTime)dateTime1).Minute, ((DateTime)dateTime1).Second, ((DateTime)dateTime1).Millisecond) -
                    new DateTime(
                        ((DateTime)dateTime2).Year, ((DateTime)dateTime2).Month, ((DateTime)dateTime2).Day,
                        ((DateTime)dateTime2).Hour, ((DateTime)dateTime2).Minute, ((DateTime)dateTime2).Second, ((DateTime)dateTime2).Millisecond);
            }
            return null;
        }

        public static TimeSpan? CalulateTimeDiffrent(string dataTimeString1, string dateTimeString2)
        {
            DateTime? dateTime1 = TimeZoneLibrary.ParseDateTimeAsUTC(dataTimeString1);
            DateTime? dateTime2 = TimeZoneLibrary.ParseDateTimeAsUTC(dateTimeString2);

            return CalulateTimeDiffrent(dateTime1, dateTime2);
        }
        
        
        public static bool VerifyTimeZoon(double metadataLocationLatitude, double metadataLocationLongitude, DateTime dateTimeMediaTaken, DateTime dateTimeLocation)
        {
            DateTime dateTimeMediaTakenWithoutZone = new DateTime(dateTimeMediaTaken.Ticks);
            DateTime dateTimeLocationUTC = new DateTime(dateTimeLocation.ToUniversalTime().Ticks, DateTimeKind.Utc);
            TimeZoneInfo timeZoneInfoGPSLocation = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation(metadataLocationLatitude, metadataLocationLongitude);
            TimeSpan timeSpanForDate = timeZoneInfoGPSLocation.GetUtcOffset(dateTimeLocation);
            TimeSpan timeSpanBetweenLocalAndUTC = dateTimeMediaTakenWithoutZone - dateTimeLocationUTC;
            return IsTimeSpanEqual(timeSpanForDate, timeSpanBetweenLocalAndUTC, 15);
        }
    }
}
