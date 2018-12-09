using System;

namespace KPlugin.Extension
{
    /// <summary>
    /// A class for adding functionalities to <c>enum</c>
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Check if the enum has flag
        /// </summary>
        /// <param name="variable">The enum</param>
        /// <param name="flag">The flag</param>
        /// <returns>`true` if enum has flag</returns>
        public static bool HasFlag(this Enum variable, Enum flag)
        {
            if (variable == null)
                return false;

            if (flag == null)
                throw new ArgumentNullException("value");

            if (!Enum.IsDefined(variable.GetType(), flag))
                throw new ArgumentException(string.Format("Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.", flag.GetType(), variable.GetType()));

            ulong num = Convert.ToUInt64(flag);
            return (Convert.ToUInt64(variable) & num) == num;
        }
    }
}
