using System;

namespace BasicLibrary
{
    // from http://www.codeproject.com/Articles/37921/Enums-Flags-and-C-Oh-my-bad-pun

    /// <summary>
    /// Extensions from Enum.
    /// </summary>
    public static class EnumerationExtensions
    {
        /// <summary>
        /// Check Enum has value.
        /// </summary>
        /// <typeparam name="T">The type of enum.</typeparam>
        /// <param name="type">Type Enum.</param>
        /// <param name="value">The value for equal.</param>
        /// <returns>Return true if available.</returns>
        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check Enum is value.
        /// </summary>
        /// <typeparam name="T">The type of enum.</typeparam>
        /// <param name="type">Type Enum.</param>
        /// <param name="value">The value for equal.</param>
        /// <returns>Return is euqal.</returns>
        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of enum.</typeparam>
        /// <param name="type">Type Enum.</param>
        /// <param name="value">The value for add.</param>
        /// <returns>Return combined enum.</returns>
        public static T Add<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch(Exception ex)
            {
                throw new ArgumentException(String.Format("Could not append value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of enum.</typeparam>
        /// <param name="type">Type Enum.</param>
        /// <param name="value">The value for remove.</param>
        /// <returns>Return the enum without value.</returns>
        public static T Remove<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch(Exception ex)
            {
                throw new ArgumentException(String.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }
    }
}
