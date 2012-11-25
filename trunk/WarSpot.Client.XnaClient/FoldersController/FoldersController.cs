using System;
using System.IO;

namespace WarSpot.Client.XnaClient.FoldersController
{
	// TODO: refactor as singleton?
	static class FoldersController
	{
		private static readonly string WarSpotPath;
		private static readonly string ReplaysAbsolutePath;
		private static readonly string IntellectsAbsolutePath;

		static FoldersController()
		{
			WarSpotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WarSpot");
			// Define path for replays folder
			ReplaysAbsolutePath = Path.Combine(WarSpotPath, "Replays");
			if (!Directory.Exists(ReplaysAbsolutePath))
				Directory.CreateDirectory(ReplaysAbsolutePath);

			// Define path for dll folder
			IntellectsAbsolutePath = Path.Combine(WarSpotPath, "Intellects");
			if (!Directory.Exists(WarSpotPath))
				Directory.CreateDirectory(WarSpotPath);
		}

		public static string GetReplayPath()
		{
			return ReplaysAbsolutePath;
		}

		public static string GetDllPath()
		{
			return IntellectsAbsolutePath;
		}
	}
}
