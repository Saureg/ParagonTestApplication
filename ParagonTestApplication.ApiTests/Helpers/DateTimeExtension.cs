namespace ParagonTestApplication.ApiTests.Helpers
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Extensions for DateTime.
    /// </summary>
    public static class DateTimeExtension
    {
        private const string Format = "yyyy-MM-ddTHH:mm";

        /// <summary>
        /// Convert DateTime to string (with minutes).
        /// </summary>
        /// <param name="dateTime">DateTime.</param>
        /// <returns>string.</returns>
        public static string ToDateTimeWithMinutesString(this DateTime dateTime)
        {
            var dateTimeString = dateTime.ToString(Format);
            return dateTimeString;
        }

        /// <summary>
        /// Remove seconds and milliseconds.
        /// </summary>
        /// <param name="dateTime">DateTime.</param>
        /// <returns>DateTime without seconds and milliseconds.</returns>
        public static DateTime RemoveSecondsAndMilliseconds(this DateTime dateTime)
        {
            var dateTimeString = dateTime.ToDateTimeWithMinutesString();
            var newDateTime = DateTime.ParseExact(dateTimeString, Format, CultureInfo.InvariantCulture);
            return newDateTime;
        }
    }
}