using System;
using System.Reflection;
using System.Diagnostics;

namespace WarSpot.Client.XnaClient.AIManager
{
	class Intellect
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public byte[] ByteDll;
		public int SizeInBits { get; set; }

		public Intellect(string path)
		{
			ByteDll = System.IO.File.ReadAllBytes(path);
			Path = path;
			Name = GetName(ByteDll);
		}

		private string GetName(byte[] byteDll)
		{
			//Didn't test yet.
			try
			{
				Assembly assembly = Assembly.Load(byteDll);
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (type.IsClass && type.GetInterface("IBeingInterface") != null)
					{
						return type.ToString();
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
			}
			return "BadFile" + Path.GetHashCode() % 1000;
		}
	}
}
