namespace KPlugin.Extension.Internal
{
    using System.Text.RegularExpressions;
    using UnityEngine;
    using Extension;

    public sealed class KString
    {
        private string text;

        private int? size;
        private Color32? color;

        private bool bold;
        private bool italic;

        private KString(string text)
        {
            this.text = text;
        }

        public static implicit operator KString(string text)
        {
            return new KString(text);
        }

        public static implicit operator string(KString text)
        {
            return text.ToString();
        }

        public KString Bold(bool bold = true)
        {
            this.bold = bold;
            return this;
        }

        public KString Italic(bool italic = true)
        {
            this.italic = italic;
            return this;
        }

        public KString Color(Color? color = null)
        {
            this.color = color;
            return this;
        }

        public KString Size(int? size = null)
        {
            this.size = size;
            return this;
        }

        public KString Upper()
        {
            text = text.Upper();
            return this;
        }

        public KString Lower()
        {
            text = text.Lower();
            return this;
        }

        public KString Capital()
        {
            text = text.Capital();
            return this;
        }

        public KString Regular()
        {
            text = text.Regular();
            return this;
        }

        public KString Literal()
        {
            text = text.Literal();
            return this;
        }

        public bool IsMatch(Regex regex)
        {
            return text.IsMatch(regex);
        }

        public KString ReplacedBy(params object[] args)
        {
            text = text.ReplacedBy(args); 
            return this;
        }

        public string Raw()
        {
            return text;
        }

        override public string ToString()
        {
            string text = this.text;

            if (size != null)
                text = "<size=" + size.Value + ">" + text + "</size>";

            if (color != null)
                text = "<color=#" + color.Value.r.ToString("x2") + color.Value.g.ToString("x2") + color.Value.b.ToString("x2") + color.Value.a.ToString("x2") + ">" + text + "</color>";

            if (bold)
                text = "<b>" + text + "</b>";

            if (italic)
                text = "<i>" + text + "</i>";

            return text;
        }
    }
}
