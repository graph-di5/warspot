using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using WarSpot.Client.XnaClient.Screen;
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
				var i = ParseIntellect(s);
				if(i == null)
				{
					continue;
				}
				t.Members.Add(i); 
				listIntellect.Add(t);
			}

			var outFileName = String.Format("replay_{0}.out", DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss"));
			var fs = new FileStream(outFileName, FileMode.CreateNew);
			var computer = new Computer(listIntellect, fs);
			var sw = new Stopwatch();
			Console.WriteLine("Game started: {0}", listIntellect.Count);
			sw.Start();
			computer.Compute();
			sw.Stop();
			fs.Close();
			Console.WriteLine("Done: {0}.", sw.Elapsed);
			Console.ReadLine();
#if true
			var list = Deserializator.Deserialize(outFileName);
#endif
		}


		public static Type ParseIntellect(string fullPath)
		{
			Assembly assembly = Assembly.LoadFrom(fullPath);//вытаскиваем библиотеку
			var referencedAssemblies = assembly.GetReferencedAssemblies();
			// LOOOOOOOOOOOOL
			var foundIntellect = false;
			foreach (var referencedAssembly in referencedAssemblies)
			{
				if(referencedAssembly.Name == "WarSpot.Contracts.Intellect")
				{

					if (referencedAssembly.Version.Major == Assembly.GetExecutingAssembly().GetName().Version.Major &&
						referencedAssembly.Version.Minor == Assembly.GetExecutingAssembly().GetName().Version.Minor)
					{
						foundIntellect = true;
					}
					break;
				}
			}
			if(!foundIntellect)
				return null;

			string iMyInterfaceName = typeof(IBeingInterface).ToString();
			foreach (var t in assembly.GetTypes())
			{
				if (t.GetInterface(iMyInterfaceName) != null)
				{
					return t;
#if false
					var defaultCtor = t.GetConstructor(new Type[0]);
					if (defaultCtor != null)
					{
						var inst = defaultCtor.Invoke(new Type[0]);
						return inst as IBeingInterface;
					}
#endif
				}
			}
			return null;
		}
	}
}
