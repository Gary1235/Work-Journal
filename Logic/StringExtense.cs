using System.Globalization;

namespace Logic
{
    public static class StringExtense
    {
        public static string ToRocShortDataTime(this DateTime dt, char symbol = '/')
        {
            TaiwanCalendar tc = new TaiwanCalendar();
            var str = $"{tc.GetYear(dt)}{symbol}{tc.GetMonth(dt)}{symbol}{tc.GetDayOfMonth(dt)}";

            return str;
        }
    }
}
