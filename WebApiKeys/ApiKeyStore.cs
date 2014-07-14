using System;
using System.Collections.Generic;

namespace WebApiKeys
{
    public static class ApiKeyStore
    {
        private static IDictionary<string, string> clientKeys = new Dictionary<string, string>
        {
            {"client-one-key","client-one-secret"}
        };

        public static string GetSecrectForKey(string key)
        {
            string clientSecret;
            return clientKeys.TryGetValue(key, out clientSecret) ? clientSecret : string.Empty;
        }
    }

    public static class DateTimeExtensions
    {
        public static long ToUnixTime(this DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }

        public static DateTime UnixTimeToDateTime(this long unixTimestamp)
        {
            DateTime unixYear0 = new DateTime(1970, 1, 1);
            long unixTimeStampInTicks = unixTimestamp * TimeSpan.TicksPerSecond;
            DateTime dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
            return dtUnix;
        }         
    }
}