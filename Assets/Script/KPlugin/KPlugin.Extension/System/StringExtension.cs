namespace KPlugin.Extension
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Common.Internal;
    using UnityEngine;

    public static class StringExtension
    {
        public static string Upper(this string s)
        {
            return s.ToUpper();
        }

        public static string Lower(this string s)
        {
            return s.ToLower();
        }

        public static string Capital(this string s)
        {
            if (s == null)
                return null;

            if (s.Length > 1)
                return char.ToUpper(s[0]) + s.Substring(1);

            return s.ToUpper();
        }

        public static string Regular(this string s)
        {
            return Regex.Replace(s, "[a-z][A-Z]", x => x.Value[0] + " " + x.Value[1]).Capital();
        }

        public static KString Bold(this string s, bool bold = true)
        {
            return s.Rich().Bold();
        }

        public static KString Italic(this string s, bool italic = true)
        {
            return s.Rich().Italic();
        }

        public static KString Color(this string s, Color? color = null)
        {
            return s.Rich().Color(color);
        }

        public static KString Size(this string s, int? size = null)
        {
            return s.Rich().Size(size);
        }

        private static KString Rich(this string s)
        {
            return s;
        }

        public static bool IsMatch(this string s, Regex regex)
        {
            return regex.IsMatch(s);
        }

        public static IEnumerable<string> SplitByWhiteSpace(this string s)
        {
            return Regex.Split(s, @"\s+").Where(x => x != string.Empty);
        }

        public static string ReplacedBy(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}
