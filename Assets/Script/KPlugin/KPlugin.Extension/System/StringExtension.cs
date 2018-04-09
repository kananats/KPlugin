namespace KPlugin.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using Internal;

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

        public static string Literal(this string s)
        {
            StringBuilder literal = new StringBuilder(s.Length);
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\'': literal.Append(@"\'"); break;
                    case '\"': literal.Append("\\\""); break;
                    case '\\': literal.Append(@"\\"); break;
                    case '\0': literal.Append(@"\0"); break;
                    case '\a': literal.Append(@"\a"); break;
                    case '\b': literal.Append(@"\b"); break;
                    case '\f': literal.Append(@"\f"); break;
                    case '\n': literal.Append(@"\n"); break;
                    case '\r': literal.Append(@"\r"); break;
                    case '\t': literal.Append(@"\t"); break;
                    case '\v': literal.Append(@"\v"); break;
                    default:
                        if (c >= 0x20 && c <= 0x7e)
                            literal.Append(c);

                        else
                        {
                            literal.Append(@"\u");
                            literal.Append(((int)c).ToString("x4"));
                        }
                        break;
                }
            }
            return literal.ToString();
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

        public static IEnumerable<string> SplitByWhiteSpaceExceptQuote(this string s)
        {
            return Regex.Matches(s, @""".*?""|[^\s]+").Cast<Match>().Select(m => m.Value);
        }

        public static string ReplacedBy(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        public static bool EqualsIgnoreCase(this string s, string t)
        {
            return string.Equals(s, t, StringComparison.OrdinalIgnoreCase);
        }
    }
}
