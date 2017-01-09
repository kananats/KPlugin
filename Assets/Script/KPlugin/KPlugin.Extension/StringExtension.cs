namespace KPlugin.Extension
{
    using System.Text.RegularExpressions;

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
    }
}