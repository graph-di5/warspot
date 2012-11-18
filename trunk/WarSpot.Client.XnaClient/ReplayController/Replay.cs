using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WarSpot.Client.XnaClient.ReplayController
{
	class Replay
	{
		private static string _globalPath; 
		public string Name { get; set; }
		private byte[] _replay;

		// static constructor to define path for saving replays
		// TODO: some tests, it isn't even tested
		static Replay()
		{
			_globalPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "zReplays";
			if (!Directory.Exists(_globalPath))
			{
				try
				{
					Directory.CreateDirectory(_globalPath);
				}
				catch(Exception e)
				{
					// хрен знает, что тут может произойти
					System.Diagnostics.Trace.WriteLine(e);
				}
			}
		}

		public void saveReplay(byte[] replay, string name)
		{
			// TODO: tests
			// Экранирует ли первый слэш второй?
			File.WriteAllBytes(_globalPath + "\\" + name, replay);
		}
	}
}
