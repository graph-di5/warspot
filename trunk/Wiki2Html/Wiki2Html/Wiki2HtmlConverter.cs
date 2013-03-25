using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wiki2Html
{
	/// <summary>
	/// Class isn't static because we should can parse more than one wiki file at once.
	/// </summary>
	internal class Wiki2HtmlConverter
	{
		#region static part

		static KeyValuePair<string, string> Tag(string wikiTag, string htmlTag, bool fullString = false)
		{
			return new KeyValuePair<string, string>(
				string.Format("{1}{0}(.*?){0}{2}", wikiTag, fullString ? "^" : "", fullString ? "$" : ""),
				string.Format("<{0}>$1</{0}>", htmlTag));
		}

		private static readonly IDictionary<string, string> Rules;

		static Wiki2HtmlConverter()
		{
			Rules = new Dictionary<string, string>();
			// todo sidebar
			Rules.Add("^#(labels|sidebar|summary)(.*)$", "<!--$1: $2-->");

			Rules.Add(Tag(@"\*", "b"));
			Rules.Add(Tag(@"_", "i"));
			Rules.Add(Tag(@"`", "tt"));
			Rules.Add("{{{", "<pre>");
			Rules.Add("}}}", "</pre>");
			Rules.Add(Tag(@"\^", "sup"));
			Rules.Add(Tag(@",,", "sub"));
			Rules.Add(Tag(@"~~", "s"));

			Rules.Add(Tag("======", "h6", true));
			Rules.Add(Tag("=====", "h5", true));
			Rules.Add(Tag("====", "h4", true));
			Rules.Add(Tag("===", "h3", true));
			Rules.Add(Tag("==", "h2", true));
			Rules.Add(Tag("=", "h1", true));

			Rules.Add("^$", "<br/>");
			Rules.Add(@"^\s*-{4,}\s*$", "<hr/>");

			/* todo
			 The following is:
  * A list
  * Of bulleted items
    # This is a numbered sublist
    # Which is done by indenting further
  * And back to the main bulleted list

 * This is also a list
 * With a single leading space
 * Notice that it is rendered
  # At the same levels
  # As the above lists.
 * Despite the different indentation levels.
			 */

			// todo
			// Plain URLs such as http://www.google.com/ or ftp://ftp.kernel.org/ are automatically made into links. 
			Rules.Add(@"\[(\S+?)\s(.*?)\]", "<a href=\"$1\">$2</a>");
			Rules.Add(@"\[(\S+?)]", "<a href=\"$1\">$1</a>");

			Rules.Add(@"(https+.*?\.(png|gif|jpg|jpeg))", "<img src=\"$1\">");

			Rules.Add(@"^\s*\|", "<tr>");
			Rules.Add(@"\|\s*$", "</tr>");
			Rules.Add(@"\|(.*?)\|", "<td>$1</td>");
			_tableString = new Regex(@"^\s*\|((\|.*?\|)+)\|\s*$");
		}

		private static Regex _tableString;
		#endregion

		private bool _isTable;

		public Wiki2HtmlConverter()
		{
			_isTable = false;
		}

		public string Convert(string line)
		{
			var isTable = _tableString.Match(line).Success;

			var res = Rules.Aggregate(line, (current, rule) => Regex.Replace((string)current, (string)rule.Key, (string)rule.Value));

			if (!_isTable && isTable)
			{
				res = "<table>" + res;
			}
			if (_isTable && !isTable)
			{
				res = "</table>" + res;
			}
			_isTable = isTable;

			return res;
		}
	}
}