using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StoreManager.Helpers {

    public static class HtmlHelpers {
        public static string AppVersion(this HtmlHelper html) {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetName().Version.ToString();
        }

        public static string AppPublishDate(this HtmlHelper html) {
            var assembly = Assembly.GetExecutingAssembly();
            return File.GetLastWriteTime(assembly.Location).ToShortDateString();
        }

        public static IHtmlString Calendar(this HtmlHelper helper, DateTime dateToShow) {
            var dateTimeFormatInfo = DateTimeFormatInfo.CurrentInfo;
            var builder = new StringBuilder();
            var date = new DateTime(dateToShow.Year, dateToShow.Month, 1);

            if (dateTimeFormatInfo != null) {
                var emptyCells = ((int)date.DayOfWeek + 7 - (int)dateTimeFormatInfo.FirstDayOfWeek) % 7;
                var daysInMonth = DateTime.DaysInMonth(dateToShow.Year, dateToShow.Month);

                builder.Append("<table class='table'><tr><th colspan='7'>" + dateTimeFormatInfo.MonthNames[date.Month - 1] + " " + dateToShow.Year + "</th></tr>");
                // Feb has 35 days
                // Others have 42 days.

                for (var i = 0; i < ((daysInMonth + emptyCells) > 35 ? 42 : 35); i++) {
                    if (i % 7 == 0) {
                        if (i > 0) builder.Append("</tr>");
                        builder.Append("<tr>");
                    }

                    if (i < emptyCells || i >= emptyCells + daysInMonth) {
                        builder.Append("<td class='cal-empty'>&nbsp;</td>");
                    }
                    else {
                        builder.Append("<td class='cal-day'>" + date.Day + "</td>");
                        date = date.AddDays(1);
                    }
                }
            }
            builder.Append("</tr></table>");

            return helper.Raw(builder.ToString());
        }
    }
}