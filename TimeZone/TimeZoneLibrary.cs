using GeoTimeZone;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using TimeZoneConverter;

namespace TimeZone
{
    public class TimeZoneLibrary
    {
        public const string ToW3CDTFformat = "yyyy-MM-ddTHH:mm:sszzz";
        public const string ToW3CDTFformatReadable = "yyyy-MM-dd HH:mm:sszzz";
        private const string DateTimeFilename = "yyyy-MM-dd HH-mm-ss";
        private const string DateTimeSortable = "yyyy-MM-dd HH:mm:ss";
        private const string DateTimeExiftool = "yyyy:MM:dd HH:mm:ss";
        
        private const string DateTimeExiftoolDateStamp = "yyyy:MM:dd";
        private const string DateTimeFilenameDateStamp = "yyyy-MM-dd";

        private const string DateTimeExiftoolTimeStamp = "HH:mm:ss";
        private const string DateTimeFilenameTimeStamp = "HH-mm-ss";

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


        #region To yyyy MM dd HH mm ss
        public static string ToStringDateTime_yyyy(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString("yyyy");
        }
        public static string ToStringDateTime_MM(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString("MM");
        }
        public static string ToStringDateTime_dd(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString("dd");
        }
        public static string ToStringDateTime_HH(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString("HH");
        }
        public static string ToStringDateTime_mm(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString("mm");
        }

        public static string ToStringDateTime_ss(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString("ss");
        }
        #endregion

        #region To DateTime
        public static string ToStringSortable(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString(DateTimeSortable);
        }

        public static string ToStringFilename(DateTimeOffset? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToString(DateTimeFilename);
        }
        public static string ToStringFilenameUTC(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToString(DateTimeFilename) + "Z";
        }


        public static string ToStringExiftool(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString(DateTimeExiftool);
        }

        public static string ToStringExiftoolUTC(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToString(DateTimeExiftool) + "Z";
        }

        
        #endregion

        #region To DateStamp TimeStamp       
        public static string ToStringFilenameTimeStamp(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToString(DateTimeFilenameTimeStamp);
        }

        public static string ToStringFilenameDateStamp(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToString(DateTimeFilenameDateStamp);
        }

        public static string ToStringExiftoolDateStamp(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToString(DateTimeExiftoolDateStamp);
        }

        public static string ToStringExiftoolTimeStamp(DateTime? dateTime)
        {
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToString(DateTimeExiftoolTimeStamp);
        }        
        #endregion 


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
            if (dateTime != null && ((DateTime)dateTime).Kind == DateTimeKind.Unspecified)
                return ((DateTimeOffset)dateTime).ToString(DateTimeSortable) + "Z";
            return dateTime == null ? "" : ((DateTimeOffset)dateTime).ToUniversalTime().ToString(DateTimeSortable) + "Z";
        }

        #region To Offset
        public static string ToStringOffset(TimeSpan timeSpan, bool withParenthesis)
        {
            return (withParenthesis ? "(" : "") + (timeSpan < new TimeSpan(0, 0, 0) ? "" : "+") + timeSpan.ToString().Substring(0, timeSpan.ToString().Length - 3) + (withParenthesis ? ")" :"");
        }

        public static string ToStringOffset(TimeSpan timeSpan)
        {
            return ToStringOffset(timeSpan, true);
        }
        #endregion 

        public static TimeSpan? CalulateTimeDiffrentWithoutTimeZone(DateTime? dateTime1, DateTime? dateTime2)
        {
            if (dateTime1 != null && dateTime2 != null)
            {
                //Remove time zone and location information so we can substract  
                return new DateTime(((DateTime)dateTime1).Ticks) - new DateTime(((DateTime)dateTime2).Ticks);
            }
            return null;
        }

        public static TimeSpan? CalulateTimeDiffrentWithoutTimeZone(string dataTimeString1, string dateTimeString2)
        {
            DateTime? dateTime1 = TimeZoneLibrary.ParseDateTimeAsUTC(dataTimeString1);
            DateTime? dateTime2 = TimeZoneLibrary.ParseDateTimeAsUTC(dateTimeString2);

            return CalulateTimeDiffrentWithoutTimeZone(dateTime1, dateTime2);
        }

        #region Check DateTime Equal -accept one secound mismatch
        public static bool IsDateTimeEqualWithinOneSecond(DateTime? c1, DateTime? c2)
        {
            if (c1 == null && c2 == null) return true;
            if (c1 == null && c2 != null) return false;
            if (c1 != null && c2 == null) return false;

            TimeSpan t = ((DateTime)c1).Subtract((DateTime)c2);
            if (System.Math.Abs(t.TotalSeconds) < 1)
            {
                return true;
            }
            return false;
        }
        #endregion
        public static bool IsTimeZoneEqual(double metadataLocationLatitude, double metadataLocationLongitude, DateTime dateTimeMediaTaken, DateTime dateTimeLocation, out string TimeZoneVerfification)
        {
            DateTime dateTimeMediaTakenWithoutZone = new DateTime(dateTimeMediaTaken.Ticks);
            DateTime dateTimeLocationUTC = new DateTime(dateTimeLocation.ToUniversalTime().Ticks, DateTimeKind.Utc);
            TimeZoneInfo timeZoneInfoGPSLocation = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation(metadataLocationLatitude, metadataLocationLongitude);
            TimeSpan timeSpanForDate = timeZoneInfoGPSLocation.GetUtcOffset(dateTimeLocation);
            
            TimeZoneVerfification = 
                "Media Digitized: " + dateTimeMediaTakenWithoutZone.ToString() + "\r\n" +
                "GPS UTC date/time: " + dateTimeLocationUTC.ToString() + "\r\n" +
                "Time zone name (Lat,Long): " + timeZoneInfoGPSLocation.DisplayName + "\r\n" +
                "Base UTC offset (Lat,Long): " + timeZoneInfoGPSLocation.BaseUtcOffset.ToString() + "\r\n" +
                "Daylight/Standard time (GPS clock):" + (timeZoneInfoGPSLocation.IsDaylightSavingTime(dateTimeLocationUTC) ? timeZoneInfoGPSLocation.DaylightName : timeZoneInfoGPSLocation.StandardName) + "\r\n" +
                "Offset UTC offset (GPS clock): " + timeZoneInfoGPSLocation.GetUtcOffset(dateTimeLocationUTC).ToString() + "\r\n" +
                "Time Span between GPS and Digitized: " + timeSpanForDate.ToString();

            return IsTimeSpanEqual(timeSpanForDate, timeZoneInfoGPSLocation.GetUtcOffset(dateTimeLocationUTC), 15);
        }
    }
}
