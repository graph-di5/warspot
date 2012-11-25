﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using WarSpot.Contracts.Intellect;
using WarSpot.MatchComputer;

namespace WarSpot.ConsoleComputer
{
	class ConsoleComputer
	{
		static void Main(string[] args)
		{
			var listIntellect = new List<TeamIntellectList>();
			if (args.Length == 0)
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("{0} [i-th.dll]", Assembly.GetExecutingAssembly().GetName().Name);
				return;
			}

			int team = 0;
			foreach (var s in args)
			{
				var t = new TeamIntellectList();
				t.Number = ++team;
				t.Members.Add(ParseIntellect(s));
				listIntellect.Add(t);
			}

			var outFileName = String.Format("replay_{0}.out", DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss"));
			var fs = new FileStream(outFileName, FileMode.CreateNew);
			var computer = new Computer(listIntellect, fs);
			computer.Compute();
			fs.Close();
			Console.WriteLine("Done.");
		}


		public static IBeingInterface ParseIntellect(string fullPath)
		{
			Assembly assembly = Assembly.LoadFrom(fullPath);//вытаскиваем библиотеку
			string iMyInterfaceName = typeof(IBeingInterface).ToString();
			foreach (var t in assembly.GetTypes())
			{
				if (t.GetInterface(iMyInterfaceName) != null)
				{
					var defaultCtor = t.GetConstructor(new Type[0]);
					if (defaultCtor != null)
					{
						var inst = defaultCtor.Invoke(new Type[0]);
						return inst as IBeingInterface;
					}
				}
			}
			return null;
		}
	}
}
