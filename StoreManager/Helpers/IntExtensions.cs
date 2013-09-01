using System;

namespace StoreManager.Helpers {

    public static class IntExtensions {
        public static string MonthName(this Int32 nMonth) {
            var months = new [] 
            { "January", 
              "February",
              "March",
              "April",
              "May",
              "June", 
              "July",
              "August",
              "September",
              "October",
              "November",
              "December"};

            if (nMonth < 0 || nMonth > 11)
                throw new ArgumentOutOfRangeException(String.Format("{0} is not a valid value for month", nMonth));

            return months[nMonth];
        }
    }
}