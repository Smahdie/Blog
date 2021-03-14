using System;
using System.Globalization;

namespace Core.Helpers
{
    public static class PersianDateHelper
    {
        public static string ConvertToLocalDateTime(DateTime localDateTime)
        {
            var persianDate = $"{ConvertToLocalDate(localDateTime)} {localDateTime:HH:mm}";

            return persianDate;
        }

        public static string ConvertToLocalDate(DateTime localDateTime)
        {
            var persianCalendar = new PersianCalendar();

            var year = persianCalendar.GetYear(localDateTime).ToString("0000");
            var month = persianCalendar.GetMonth(localDateTime).ToString("00");
            var day = persianCalendar.GetDayOfMonth(localDateTime).ToString("00");

            var persianDate = $"{year}/{month}/{day}";

            return persianDate;
        }
    }
}
