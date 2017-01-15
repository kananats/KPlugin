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
    }
}
