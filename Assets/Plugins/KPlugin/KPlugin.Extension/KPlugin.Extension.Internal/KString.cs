using System.Text.RegularExpressions;
using UnityEngine;

namespace KPlugin.Extension.Internal
{
    using Extension;

    /// <summary>
    /// Class representing custom string
    /// </summary>
    public sealed class KString
    {
        /// <summary>
        /// Original string
        /// </summary>
        private string text;

        /// <summary>
        /// Size of the text (nullable)
        /// </summary>
        private int? size;

        /// <summary>
        /// Color of the text (nullable)
        /// </summary>
        private Color32? color;

        /// <summary>
        /// Is bold
        /// </summary>
        private bool bold;

        /// <summary>
        /// Is italic
        /// </summary>
        private bool italic;

        /// <summary>
        /// Initializes a <c>KString</c>
        /// </summary>
        /// <param name="text">Original string</param>
        private KString(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Implicit conversion from <c>string</c> to <c>KString</c>
        /// </summary>
        /// <param name="text">Original text</param>
        public static implicit operator KString(string text)
        {
            return new KString(text);
        }

        /// <summary>
        /// Implicit conversion from <c>KString</c> to <c>string</c>
        /// </summary>
        /// <param name="text"></param>
        public static implicit operator string(KString text)
        {
            return text.ToString();
        }

        /// <summary>
        /// Sets whether bold is enabled then returns itself
        /// </summary>
        public KString Bold(bool bold = true)
        {
            this.bold = bold;
            return this;
        }

        /// <summary>
        /// Sets whether italic is enabled then returns itself
        /// </summary>
        public KString Italic(bool italic = true)
        {
            this.italic = italic;
            return this;
        }

        /// <summary>
        /// Sets the color then returns itself
        /// </summary>
        public KString Color(Color? color = null)
        {
            this.color = color;
            return this;
        }

        /// <summary>
        /// Sets the size then returns itself
        /// </summary>
        public KString Size(int? size = null)
        {
            this.size = size;
            return this;
        }

        /// <summary>
        /// Makes the text uppercase then returns itself
        /// </summary>
        public KString Upper()
        {
            text = text.Upper();
            return this;
        }

        /// <summary>
        /// Makes the text lowercase then returns itself
        /// </summary>
        public KString Lower()
        {
            text = text.Lower();
            return this;
        }

        /// <summary>
        /// Makes the text capital then returns itself
        /// </summary>
        public KString Capital()
        {
            text = text.Capital();
            return this;
        }

        /// <summary>
        /// Makes the text regular then returns itself
        /// </summary>
        public KString Regular()
        {
            text = text.Regular();
            return this;
        }

        /// <summary>
        /// Makes the text literal then returns itself
        /// </summary>
        public KString Literal()
        {
            text = text.Literal();
            return this;
        }

        /// <summary>
        /// Check if the <c>KString</c> matches with the regular expression
        /// </summary>
        /// <param name="regex">Regular expression</param>
        /// <returns>Returns `true` if the <c>KString</c> matches with the regular expression</returns>
        public bool IsMatch(Regex regex)
        {
            return text.IsMatch(regex);
        }

        /// <summary>
        /// Interpolates the formatted string then returns itself
        /// </summary>
        /// <param name="args">An object array that contains zero or more objects to format</param>
        public KString ReplacedBy(params object[] args)
        {
            text = text.ReplacedBy(args);
            return this;
        }

        /// <summary>
        /// Returns the raw text
        /// </summary>
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
