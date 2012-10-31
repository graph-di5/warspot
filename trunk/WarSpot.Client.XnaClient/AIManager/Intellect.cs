using System;
using System.Reflection;
using System.Diagnostics;
using WarSpot.Contracts.Intellect;

namespace WarSpot.Client.XnaClient.AIManager
{
    class Intellect
	{
		public string Name { get; set; }
		public string Path { get; set; }
        public byte[] byteDll;
        public int SizeInBits { get; set; }

        public Intellect(string path)
        {
            this.byteDll = System.IO.File.ReadAllBytes(path);
            this.Path = path;
			Name = getName(byteDll);
        }

		private string getName(byte[] byteDll)
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
