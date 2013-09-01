using System;
using System.Globalization;

namespace StoreManager.Helpers
{
    public static class DateUtils {
        public static DateTime Parse(this string s, string culture = "en-GB") {
            return DateTime.Parse(s, new CultureInfo(culture));
        }
    }
}