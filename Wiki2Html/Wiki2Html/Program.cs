﻿using System;
using System.Collections.Generic;
using System.IO;
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

				while((line = inFile.ReadLine()) != null)
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
		private static readonly Dictionary<string,string> Rules = new Dictionary<string, string>
		                                         	{
		                                         		{"^#(.*)$", "<!--$1-->"},
																								{"====.*====", "<h4>$1</h4>"},
																								{"===.*===", "<h3>$1</h3>"},
																								{"==.*==", "<h2>$1</h2>"},
																								{"=.*=", "<h1>$1</h1>"},
		                                         	};
		public string Convert(string line)
		{
			foreach (var rule in Rules)
			{
				line = Regex.Replace(line, rule.Key, rule.Value);
			}
			return line;
		}
	}
}
