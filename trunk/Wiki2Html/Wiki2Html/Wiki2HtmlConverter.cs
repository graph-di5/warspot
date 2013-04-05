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
				string.Format("{1}{0}(.*?){0}{2}", wikiTag, fullString ? @"^\s*" : "", fullString ? @"\s*$" : ""),
				string.Format("<{0}>$1</{0}>", htmlTag));
		}

		private static readonly IDictionary<string, string> Rules;

		static Wiki2HtmlConverter()
		{
			Rules = new Dictionary<string, string>();
			// todo sidebar
			Rules.Add("^#(labels|sidebar|summary)(.*)$", "<!--$1: $2-->");

			// lists
			Rules.Add(@"^\s+#(.*)$", "<li>$1</li>");
			Rules.Add(@"^\s+\*(.*)$", "<li>$1</li>");

			Rules.Add(Tag(@"\*", "b"));
			Rules.Add(Tag(@"_", "i"));
			Rules.Add(Tag(@"`", "tt"));
			// todo skip replacement
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

			// todo add # link navigation
			Rules.Add(@"[^\]]?((http|ftp)s?://\S*)", "<a href=\"$1\">$1</a>");
			Rules.Add(@"\[(\w+?)\]", "<a href=\"$1\">$1</a>");
			Rules.Add(@"\[(\S+?)\s(.*?)\]", "<a href=\"$1\">$2</a>");
			Rules.Add(@"\[(\S+?)]", "<a href=\"$1\">$1</a>");

			Rules.Add(@"((http|ftp)s?.*?\.(png|gif|jpg|jpeg))", "<img src=\"$1\">");

			Rules.Add(@"^\s*\|", "<tr>");
			Rules.Add(@"\|\s*$", "</tr>");
			Rules.Add(@"\|(.*?)\|", "<td>$1</td>");

			TablePattern = new Regex(@"^\s*\|((\|.*?\|)+)\|\s*$");
			NumberedListPattern = new Regex(@"^(\s)+#");
			BulletedListPattern = new Regex(@"^(\s)+\*");
		}

		private static readonly Regex TablePattern, NumberedListPattern, BulletedListPattern;
		#endregion

		private bool _isTable;
		// todo merge this to stacks
		private readonly Stack<int> _numberedLists, _bulletedLists;

		public Wiki2HtmlConverter()
		{
			_isTable = false;
			_numberedLists = new Stack<int>();
			_bulletedLists= new Stack<int>();
		}

		public string Convert(string line)
		{
			var isTable = TablePattern.IsMatch(line);
			var isNumberedList = NumberedListPattern.IsMatch(line);
			var isBulletedList = BulletedListPattern.IsMatch(line);

			var numberedListShift = 0;
			if (isNumberedList)
			{
				numberedListShift = line.IndexOf('#') - 1;
			}
			if (isBulletedList)
			{
				numberedListShift = line.IndexOf('*') - 1;
			}

			var res = Rules.Aggregate(line, (current, rule) => Regex.Replace(current, rule.Key, rule.Value));

			#region <ol> - numbered list
			if (isNumberedList
				&& (_numberedLists.Count == 0 || _numberedLists.Peek() < numberedListShift))
			{
				res = "<ol> " + res;
				_numberedLists.Push(numberedListShift);
			}
			if (!isNumberedList && !isBulletedList)
			{
				while (_numberedLists.Count > 0)
				{
					res = "</ol>" + res;
					_numberedLists.Pop();
				}
			}
			if (_numberedLists.Count != 0 && _numberedLists.Peek() > numberedListShift)
			{
				res = "</ol>" + res;
				_numberedLists.Pop();
			}
			#endregion

			#region <ul> - bulleted list
			if (isBulletedList
				&& (_bulletedLists.Count == 0 || _bulletedLists.Peek() < numberedListShift))
			{
				res = "<ul> " + res;
				_bulletedLists.Push(numberedListShift);
			}
			if (!isNumberedList && !isBulletedList)
			{
				while (_bulletedLists.Count > 0)
				{
					res = "</ul>" + res;
					_bulletedLists.Pop();
				}
			}
			if (_bulletedLists.Count != 0 && _bulletedLists.Peek() > numberedListShift)
			{
				res = "</ul>" + res;
				_bulletedLists.Pop();
			}
			#endregion

			#region <table>
			if (!_isTable && isTable)
			{
				res = "<table> " + res;
			}
			if (_isTable && !isTable)
			{
				res = "</table> " + res;
			}
			_isTable = isTable;
			#endregion

			return res;
		}
	}
}