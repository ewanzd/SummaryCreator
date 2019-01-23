using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummaryCreator.Basic
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Get total unix seconds of this DateTime. Attention: The method get not the unix seconds of the universal time.
        /// </summary>
        /// <param name="dateTime">The method get the total unix seconds from this DateTime.</param>
        /// <returns>Return the toal unix seconds.</returns>
        public static long GetUnixSeconds(this DateTime dateTime)
        {
            return (long)Math.Truncate((dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

        /// <summary>
        /// Get total unix days of this DateTime. Attention: The method get not the unix days of the universal time.
        /// </summary>
        /// <param name="dateTime">The method get the total unix days from this DateTime.</param>
        /// <returns>Return the toal unix days.</returns>
        public static long GetUnixDays(this DateTime dateTime)
        {
            return (long)Math.Truncate((dateTime.Subtract(new DateTime(1970, 1, 1))).TotalDays);
        }
    }
}
