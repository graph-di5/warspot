using System;
using System.IO;

namespace WarSpot.Client.XnaClient.ReplayController
{
	class Replay
	{
		private static readonly string GlobalPath;
		public string Name { get; set; }
		private byte[] _replay;

		// static constructor to define path for saving replays
		// TODO: some tests, it isn't even tested
		static Replay()
		{
			// !! todo move this to Common
			GlobalPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "zReplays";
			if (!Directory.Exists(GlobalPath))
			{
				try
				{
					Directory.CreateDirectory(GlobalPath);
				}
				catch (Exception e)
				{
					// хрен знает, что тут может произойти
					// DI: ну это смотря, что мы выше можем напортить
					System.Diagnostics.Trace.WriteLine(e);
				}
			}
		}

		public void SaveReplay(byte[] replay, string name)
		{
			// TODO: tests
			// Экранирует ли первый слэш второй?
			File.WriteAllBytes(Path.Combine(GlobalPath, name), replay);
		}
	}
}
