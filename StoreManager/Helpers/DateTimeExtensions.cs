using System;

namespace StoreManager.Helpers {
    public static class DateTimeExtensions {
        public static DateTime FirstDayOfMonth(this DateTime date) {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime date) {
            return date.FirstDayOfMonth().AddMonths(1).Subtract(new TimeSpan(1, 0, 0, 0));
        }
    }
}