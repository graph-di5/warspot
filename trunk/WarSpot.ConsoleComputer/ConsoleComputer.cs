using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using WarSpot.Common.Utils;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Service;
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
			var computer = new Computer(listIntellect);
			var sw = new Stopwatch();
			Console.WriteLine("Game started: {0}", listIntellect.Count);
			sw.Start();
			var res = computer.Compute();
			SerializationHelper.Serialize(res, fs);
			sw.Stop();
			fs.Close();
			Console.WriteLine("Done: {0}.", sw.Elapsed);
			Console.ReadLine();
#if true
			var list = SerializationHelper.Deserialize(outFileName);
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
					// todo rewrite this to CheckVersion
                    if (VersionHelper.CheckVersion(referencedAssembly.Version))
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
