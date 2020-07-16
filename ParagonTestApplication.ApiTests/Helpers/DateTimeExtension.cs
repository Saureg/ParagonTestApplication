using System;
using System.Globalization;

namespace ParagonTestApplication.ApiTests.Helpers
{
    public static class DateTimeExtension
    {
        private const string Format = "yyyy-MM-ddTHH:mm";

        public static string ToDateTimeWithMinutesString(this DateTime dateTime)
        {
            var dateTimeString = dateTime.ToString(Format);
            return dateTimeString;
        }

        public static DateTime RemoveSecondsAndMilliseconds(this DateTime dateTime)
        {
            var dateTimeString = dateTime.ToDateTimeWithMinutesString();
            var newDateTime = DateTime.ParseExact(dateTimeString, Format, CultureInfo.InvariantCulture);
            return newDateTime;
        }
    }
}