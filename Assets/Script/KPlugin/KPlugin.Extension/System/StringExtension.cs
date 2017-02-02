namespace KPlugin.Extension
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Common;

    public static class StringExtension
    {
        public static string ToCapital(this string s)
        {
            if (s == null)
                return null;

            if (s.Length > 1)
                return char.ToUpper(s[0]) + s.Substring(1);

            return s.ToUpper();
        }

        public static string ToRegular(this string s)
        {
            return Regex.Replace(s, "[a-z][A-Z]", x => x.Value[0] + " " + x.Value[1]).ToCapital();
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

        public static KString Rich(this string s)
        {
            return new KString(s);
        }
    }
}
