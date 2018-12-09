using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace KPlugin.Extension
{
    using Internal;

    /// <summary>
    /// An internal class for adding functionalities to <c>string</c>
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Returns a copy of this <c>string</c> converted to uppercase
        /// </summary>
        public static string Upper(this string s)
        {
            return s.ToUpper();
        }

        /// <summary>
        /// Returns a copy of this <c>string</c> converted to lowercase
        /// </summary>
        public static string Lower(this string s)
        {
            return s.ToLower();
        }

        /// <summary>
        /// Returns a copy of this <c>string</c> converted to capital
        /// </summary>
        public static string Capital(this string s)
        {
            if (s.Length > 1)
                return char.ToUpper(s[0]) + s.Substring(1);

            return s.ToUpper();
        }

        /// <summary>
        /// Returns a copy of this <c>string</c> after adding or removing single quote
        /// </summary>
        /// <param name="singleQuote">A <c>bool</c> indicating whether adding or removing</param>
        public static string SingleQuote(this string s, bool singleQuote = true)
        {
            if (s.Length <= 1)
                return s;

            bool alreadyHas = s[0] == '\'' && s[s.Length - 1] == '\'';

            if (singleQuote)
            {
                if (alreadyHas)
                    return s;

                return '\'' + s + '\'';
            }

            if (!alreadyHas)
                return s;

            return s.Substring(1, s.Length - 2).SingleQuote(singleQuote);
        }

        /// <summary>
        /// Returns a copy of this <c>string</c> after adding or removing double quote
        /// </summary>
        /// <param name="doubleQuote">A <c>bool</c> indicating whether adding or removing</param>
        public static string DoubleQuote(this string s, bool doubleQuote = true)
        {
            if (s.Length <= 1)
                return s;

            bool alreadyHas = s[0] == '\"' && s[s.Length - 1] == '\"';

            if (doubleQuote)
            {
                UnityEngine.Debug.Log("2");
                if (alreadyHas)
                    return s;

                return '\"' + s + '\"';
            }

            if (!alreadyHas)
                return s;

            return s.Substring(1, s.Length - 2).DoubleQuote(doubleQuote);
        }

        /// <summary>
        /// Returns a literal copy of this <c>string</c>
        /// </summary>
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

        /// <summary>
        /// Returns a regular copy of this <c>string</c>
        /// </summary>
        public static string Regular(this string s)
        {
            return Regex.Replace(s, "[a-z][A-Z]", token => token.Value[0] + " " + token.Value[1]).Capital();
        }

        /// <summary>
        /// Returns a bold copy of this <c>string</c>
        /// </summary>
        public static KString Bold(this string s, bool bold = true)
        {
            return s.Rich().Bold();
        }

        /// <summary>
        /// Returns an italic copy of this <c>string</c>
        /// </summary>
        public static KString Italic(this string s, bool italic = true)
        {
            return s.Rich().Italic();
        }

        /// <summary>
        /// Returns a colored copy of this <c>string</c>
        /// </summary>
        public static KString Color(this string s, Color? color = null)
        {
            return s.Rich().Color(color);
        }

        /// <summary>
        /// Returns a copy of this <c>string</c> in new size
        /// </summary>
        public static KString Size(this string s, int? size = null)
        {
            return s.Rich().Size(size);
        }

        /// <summary>
        /// Makes a <c>KString</c>
        /// </summary>
        private static KString Rich(this string s)
        {
            return s;
        }

        /// <summary>
        /// Check if the <c>string</c> matches with the regular expression
        /// </summary>
        /// <param name="regex">Regular expression</param>
        /// <returns>Returns `true` if the <c>string</c> matches with the regular expression</returns>
        public static bool IsMatch(this string s, Regex regex)
        {
            return regex.IsMatch(s);
        }

        /// <summary>
        /// Returns tokens after splitting by white space
        /// </summary>
        public static IEnumerable<string> SplitByWhiteSpace(this string s)
        {
            return Regex.Split(s, @"\s+").Where(token => token != string.Empty);
        }

        /// <summary>
        /// Returns tokens after splitting by white space except when in quote
        /// </summary>
        public static IEnumerable<string> SplitByWhiteSpaceExceptQuote(this string s)
        {
            return Regex.Matches(s, @""".*?""|[^\s]+").Cast<Match>().Select(m => m.Value);
        }

        /// <summary>
        /// Interpolates the formatted string
        /// </summary>
        /// <param name="args">An object array that contains zero or more objects to format</param>
        public static string ReplacedBy(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        /// <summary>
        /// Check if two strings are equal if cases are ignored
        /// </summary>
        public static bool EqualsIgnoreCase(this string s, string t)
        {
            return string.Equals(s, t, StringComparison.OrdinalIgnoreCase);
        }
    }
}
