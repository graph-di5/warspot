﻿using System;
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
			Name = GetName(path);
		}

		private string GetName(string path)
		{
			//Didn't test yet.
			AppDomain appDomain = AppDomain.CreateDomain("tmp");
			Assembly assembly = appDomain.Load(path);
			string name = assembly.GetName().ToString();
			AppDomain.Unload(appDomain);
			return name;
		}
	}
}
