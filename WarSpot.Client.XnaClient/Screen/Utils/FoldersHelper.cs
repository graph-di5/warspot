using System;
using System.IO;

namespace WarSpot.Client.XnaClient.Screen.Utils
{
	static class FoldersHelper
	{
		private static readonly string ReplaysAbsolutePath;
		static FoldersHelper()
		{
            var x = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WarSpotReplays");
            if (!Directory.Exists(x))
                Directory.CreateDirectory(x);
            ReplaysAbsolutePath = x;
		}

		public static string GetReplayPath()
		{
			return ReplaysAbsolutePath;
		}
	}
}
