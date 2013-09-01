using System.Globalization;

namespace StoreManager.Helpers {

    public static class StringExtensions {
        public static string ToTitleCase(this string text, CultureInfo culture, bool forceCasing = false) {
            return culture.TextInfo.ToTitleCase(forceCasing ? text.ToLower() : text);
        }

        public static string ToRepetitionDays(this string day) {
            if (day.Length > 2)
                return day.Substring(0, 2).ToUpper();
            return day;
        }

        public static string ToLongDayName(this string day) {
            if (string.IsNullOrEmpty(day) || day.Length < 2)
                return day;

            switch (day) {
                case "SU":
                    return "Sunday";
                case "MO":
                    return "Monday";
                case "TU":
                    return "Tuesday";
                case "WE":
                    return "WE";
                case "TH":
                    return "Thursday";
                case "FR":
                    return "Friday";
                case "SA":
                    return "Saturday";
                default:
                    return day.Substring(0, 2);
            }
        }
    }
}