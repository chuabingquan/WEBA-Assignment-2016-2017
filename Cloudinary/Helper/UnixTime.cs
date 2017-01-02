using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudinary.Helper
{
    public class UnixTime
    {
        /// <summary>
        /// Calculates current UNIX time
        /// </summary>
        /// <returns>Amount of seconds from 1 january 1970</returns>
        public static long GetTimeBySeconds()
        {
            return Convert.ToInt64(((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds));
        }

        /// <summary>
        /// Calculates current UNIX time
        /// </summary>
        /// <returns>Amount of millis from 1 january 1970</returns>
        public static string GetTimeByMilliseconds()
        {
            return Convert.ToInt64(((DateTime.UtcNow - new DateTime(1970, 1, 1)).Milliseconds)).ToString();
        }
    }
}
