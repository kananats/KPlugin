namespace KPlugin.Common
{
    using UnityEngine;

    public sealed class KString
    {
        private string text;

        private bool upper;
        private bool lower;

        private int? size;
        private Color32? color;

        private bool bold;
        private bool italic;

        public KString(string text)
        {
            this.text = text;
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

        public KString Upper(bool upper = true)
        {
            this.upper = upper;
            if (upper)
                lower = false;

            return this;
        }

        public KString Lower(bool lower = true)
        {
            this.lower = lower;
            if (lower)
                upper = false;

            return this;
        }

        public string Raw()
        {
            return text;
        }

        override public string ToString()
        {
            string text = this.text;

            if (upper)
                text = text.ToUpper();

            else if (lower)
                text = text.ToLower();

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

