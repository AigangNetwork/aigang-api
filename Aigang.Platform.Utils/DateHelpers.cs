using System;

namespace Aigang.Platform.Utils
{
    public static class DateHelpers
    {
        public static DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            double seconds = Convert.ToDouble(unixTimeStamp);
            dtDateTime = dtDateTime.AddSeconds(seconds);
            return dtDateTime;
        }
    }
}
