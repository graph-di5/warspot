using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;
using CommandLine.Text;

namespace Wiki2Html
{
	class Wiki2Html
	{
		class Options
		{
			[Option('i', "input", HelpText = "Input file name.", Required = true)]
			public string InFileName { get; set; }

			[Option('o', "output", HelpText = "Output file name.", Required = true)]
			public String OutFileName { get; set; }

			[HelpOption]
			public string GetUsage()
			{
				return HelpText.AutoBuild(this,
					(HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
			}
		}

		static void Main(string[] args)
		{
			var line = "";
			var options = new Options();
			var converter = new Wiki2HtmlConverter();
			if (Parser.Default.ParseArguments(args, options))
			{
				// Values are available here
				//if (options.Verbose) Console.WriteLine("Filename: {0}", options.InputFile);
				if (!File.Exists(options.InFileName))
				{
					Console.WriteLine("Input file doesn't exist.");
					return;
				}
				var inFile = new StreamReader(options.InFileName);
				var outFile = new StreamWriter(options.OutFileName);

				while ((line = inFile.ReadLine()) != null)
				{
					outFile.WriteLine(converter.Convert(line));
				}

				inFile.Close();
				outFile.Close();
			}
		}
	}

	internal class Wiki2HtmlConverter
	{
		static KeyValuePair<string, string> Tag(string wikiTag, string htmlTag, bool fullString = false)
		{
			return new KeyValuePair<string, string>(
				string.Format("{1}{0}(.*){0}{2}", wikiTag, fullString ? "^" : "", fullString ? "$" : ""),
				string.Format("<{0}>$1</{0}>", htmlTag));
		}


		private static readonly IDictionary<string, string> Rules;

		static Wiki2HtmlConverter()
		{
			Rules = new Dictionary<string, string>();
			Rules.Add(Tag(@"\*", "b"));
			Rules.Add("^#(.*)$", "<!--$1-->");
			Rules.Add(Tag("====", "h4", true));
			Rules.Add(Tag("===", "h3", true));
			Rules.Add(Tag("==", "h2", true));
			Rules.Add(Tag("=", "h1", true));
			Rules.Add("{{{", "<pre>");
			Rules.Add("}}}", "</pre>");
			Rules.Add("^$", "<br/>");
			Rules.Add(@"\[(\S+?)\s(.*?)\]", "<a href=\"$1\">$2</a>");
			Rules.Add(@"\[(\S+?)]", "<a href=\"$1\">$1</a>");
			Rules.Add(@"(https+.*?\.(png|gif|jpg|jpeg))", "<img src=\"$1\">");
		}

		public string Convert(string line)
		{
			return Rules.Aggregate(line, (current, rule) => Regex.Replace(current, rule.Key, rule.Value));
		}
	}
}
