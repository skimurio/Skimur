using System;
namespace Skimur.Caching
{
    public static class CacheTime
    {
        static CacheTime()
        {
            Short = TimeSpan.FromMinutes(1);
            Average = TimeSpan.FromMinutes(5);
            Long = TimeSpan.FromMinutes(10);
        }

        public static TimeSpan Short { get; private set; }

        public static TimeSpan Average { get; private set; }

        public static TimeSpan Long { get; private set; }
    }
}
