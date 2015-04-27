using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ganss.Text
{
    /// <summary>
    /// Replaces URIs with HTML links.
    /// </summary>
    public class AutoLink
    {
        /// <summary>
        /// The default URI prefixes which can start a URI.
        /// </summary>
        public static readonly ISet<string> DefaultSchemes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "http://", "https://", "ftp://", "mailto:", "www." };

        AhoCorasick Schemes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoLink"/> class.
        /// </summary>
        /// <param name="schemes">The prefixes of URIs to recognize. If null, uses <see cref="DefaultSchemes"/>.</param>
        public AutoLink(IEnumerable<string> schemes = null)
        {
            Schemes = new AhoCorasick(CharComparer.OrdinalIgnoreCase, schemes ?? DefaultSchemes);
        }

        private string DefaultReplace(string uri)
        {
            var url = uri.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? "http://" + uri : uri;
            return string.Format(@"<a href=""{0}"">{1}</a>", url, uri);
        }

        /// <summary>
        /// Replaces all recognized URIs in the specified text with HTML links, optionally using a specified replacer func.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method assumes that the input text does not contain HTML but only raw text.
        /// </para>
        /// <para>
        /// Effort is made to exclude trailing punctuation from the end of the URI while trying to include parentheses that are part of the URI.
        /// </para>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// var autolink = new AutoLink();
        /// var text = @"Check it out at Wikipedia (http://en.wikipedia.org/wiki/Link_(film)).";
        /// var html = autolink.Link(text);
        /// // -> Check it out at Wikipedia (<a href="http://en.wikipedia.org/wiki/Link_(film)">http://en.wikipedia.org/wiki/Link_(film)</a>).
        /// ]]>
        /// </code>
        /// </example>
        /// </remarks>
        /// <param name="s">The text to autolink.</param>
        /// <param name="replace">The replacer func. Takes a URI and returns the complete HTML link.</param>
        /// <returns>The autolinked text.</returns>
        public string Link(string s, Func<string, string> replace = null)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;

            var match = Schemes.Search(s).FirstOrDefault();
            if (match.Word == null) return s;

            var start = match.Index;
            var end = FindEnd(s, start);

            var r = replace ?? DefaultReplace;

            return s.Substring(0, start) + r(s.Substring(start, end - start)) + Link(s.Substring(end), r);
        }

        private int FindEnd(string s, int start)
        {
            var parens = 0;
            var end = start;

            // skip over URI characters, counting balanced parens
            for (; end < s.Length; end++)
            {
                var c = s[end];
                if (!IsUriChar(c)) break;
                if (c == '(') parens++;
                else if (c == ')') parens--;
            }

            // trim punctuation and unbalanced parens from end
            while (end > start)
            {
                var c = s[end - 1];

                if (parens < 0 && c == ')') parens++;
                else if (!IsPunctuation(c)) break;

                end--;
            }

            return end;
        }

        private bool IsPunctuation(char c)
        {
            return c == '!' || c == '?' || c == '.' || c == ':' || c == ';' || c == ',' || c == ']';
        }

        private static bool IsUriChar(char c)
        {
            return (c >= 'a' && c <= 'z')
                || (c >= '?' && c <= '[')
                || (c >= '#' && c <= ';')
                || c == '!' || c == '=' || c == '_' || c == '~' || c == ']';
        }
    }
}
