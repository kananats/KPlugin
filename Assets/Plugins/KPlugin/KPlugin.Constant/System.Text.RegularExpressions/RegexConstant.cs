using System.Text.RegularExpressions;

namespace KPlugin.Constant
{
    /// <summary>
    /// The class to declare <c>Regex</c> constants
    /// </summary>
    public static class RegexConstant
    {
        private static Regex _numeric;

        /// <summary>
        /// Regular expression for numerics
        /// </summary>
        public static Regex numeric
        {
            get
            {
                if (_numeric == null)
                    _numeric = new Regex("^[0-9]+$");

                return _numeric;
            }
        }

        private static Regex _alphabet;

        /// <summary>
        /// Regular expression for english alphabets, including regular and capital letters
        /// </summary>
        public static Regex alphabet
        {
            get
            {
                if (_alphabet == null)
                    _alphabet = new Regex("^[a-zA-Z]+$");

                return _alphabet;
            }
        }

        private static Regex _alphabetOrUnderscore;

        /// <summary>
        /// Regular expression for english alphabets, including regular, capital letters, and underscore
        /// </summary>
        public static Regex alphabetOrUnderscore
        {
            get
            {
                if (_alphabetOrUnderscore == null)
                    _alphabetOrUnderscore = new Regex("^[a-zA-Z_]+$");

                return _alphabetOrUnderscore;
            }
        }

        private static Regex _alphanumeric;

        /// <summary>
        /// Regular expression for english alphabets, including regular, capital letters, and numerics
        /// </summary>
        public static Regex alphanumeric
        {
            get
            {
                if (_alphanumeric == null)
                    _alphanumeric = new Regex("^[a-zA-Z0-9]+$");

                return _alphanumeric;
            }
        }

        private static Regex _alphanumericOrUnderscore;

        /// <summary>
        /// Regular expression for english alphabets, including regular, capital letters, numerics, and underscore
        /// </summary>
        public static Regex alphanumericOrUnderscore
        {
            get
            {
                if (_alphanumericOrUnderscore == null)
                    _alphanumericOrUnderscore = new Regex("^[a-zA-Z0-9_]+$");

                return _alphanumericOrUnderscore;
            }
        }
    }
}
