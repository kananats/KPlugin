namespace KPlugin.Constant
{
    using System.Text.RegularExpressions;

    public static class RegexConstant
    {
        private static Regex _alphanumeric;

        public static Regex alphanumeric
        {
            get
            {
                if (_alphanumeric == null)
                    _alphanumeric = new Regex("^[a-zA-Z0-9]*$");

                return _alphanumeric;
            }
        }
    }
}
