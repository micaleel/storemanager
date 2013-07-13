using System;

namespace StoreManager.Models
{
    class RandomDates
    {
        private readonly Random _random = new Random();

        public DateTime Date(DateTime? start = null, DateTime? end = null)
        {
            if (start.HasValue && end.HasValue && start.Value >= end.Value)
                throw new Exception("start date must be less than end date!");

            DateTime min = start ?? DateTime.MinValue;
            DateTime max = end ?? DateTime.MaxValue;

            // for timespan approach see: http://stackoverflow.com/questions/1483670/whats-the-best-practice-for-getting-a-random-date-time-between-two-date-times/1483677#1483677
            TimeSpan timeSpan = max - min;

            // for random long see: http://stackoverflow.com/questions/677373/generate-random-values-in-c/677384#677384
            byte[] bytes = new byte[8];
            _random.NextBytes(bytes);

            long int64 = Math.Abs(BitConverter.ToInt64(bytes, 0)) % timeSpan.Ticks;

            TimeSpan newSpan = new TimeSpan(int64);

            return min + newSpan;
        }
    }
}