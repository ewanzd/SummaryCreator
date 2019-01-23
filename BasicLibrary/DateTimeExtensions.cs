using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLibrary
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get total unix seconds of this DateTime (Universal time).
        /// </summary>
        /// <param name="dateTime">The method get the total unix seconds from this DateTime.</param>
        /// <returns>Return the toal unix seconds.</returns>
        public static long GetUnixSeconds(this DateTime dateTime)
        {
            return (long)Math.Truncate((dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }
    }
}
