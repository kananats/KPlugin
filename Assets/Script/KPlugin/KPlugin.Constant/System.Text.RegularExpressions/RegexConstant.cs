namespace KPlugin.Constant
{
    using System.Text.RegularExpressions;

    public static class RegexConstant
    {
        private static Regex _numeric;
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
