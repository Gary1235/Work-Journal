using System.Globalization;
using System.Text.RegularExpressions;

namespace Logic
{
    public static class DateTimeExtense
    {
        public static string ToRocShortDataTime(this DateTime dt, char symbol = '/')
        {
            TaiwanCalendar tc = new TaiwanCalendar();
            var str = $"{tc.GetYear(dt)}{symbol}{tc.GetMonth(dt)}{symbol}{tc.GetDayOfMonth(dt)}";

            return str;
        }

        public static string ToRocShortDataTime(this DateTime? dtnull, char symbol = '/')
        {
            var str = "";
            if (dtnull != null)
            {
                var dt = (DateTime)dtnull;
                TaiwanCalendar tc = new TaiwanCalendar();
                str = $"{tc.GetYear(dt)}{symbol}{tc.GetMonth(dt)}{symbol}{tc.GetDayOfMonth(dt)}";
            }

            return str;
        }

        public static DateTime RocShortToDateTime(this string? dtString)
        {
            var arr = AnalyzeString(dtString);
            if (arr.Length != 3)
            {
                return DateTime.Now;
            }

            var year = int.Parse(arr[0]) + 1911;
            var month = int.Parse(arr[1]);
            var day = int.Parse(arr[2]);

            var dateTime = new DateTime(year, month, day);
            return dateTime;
        }

        public static DateTime ToDateTime(this string? dtString)
        {
            if (DateTime.TryParse(dtString, out DateTime result))
            {
                return result;
            }

            return DateTime.Now;
        }

        private static string[] AnalyzeString(string dtString)
        {
            Match match = Regex.Match(dtString, @"(\d+)\/(\d+)\/(\d+)");

            if (match.Success)
            {
                return new string[] { match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value };
            }
            else
            {
                return new string[0];
            }
        }
    }
}
