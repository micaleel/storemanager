using System;

namespace DialogueStore.Web.Infrastructure
{
    public static class DateTimeExtensions
    {
        public static string FormatDate(this DateTime date)
        {
            return date.ToString("dd-MMM-yyyy");
        }

        public static string FormatDate(this DateTime? date, string text)
        {
            return date.HasValue ? date.Value.ToString("dd-MMM-yyyy") : text;
        }

        public static string Ago(this DateTime d)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - d.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            if (delta < 0)
            {
                return "not yet";
            }
            if (delta < 1 * minute)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 2 * minute)
            {
                return "a minute ago";
            }
            if (delta < 45 * minute)
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 90 * minute)
            {
                return "an hour ago";
            }
            if (delta < 24 * hour)
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 48 * hour)
            {
                return "yesterday";
            }
            if (delta < 30 * day)
            {
                return ts.Days + " days ago";
            }
            if (delta < 12 * month)
            {
                var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }

            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }
}
