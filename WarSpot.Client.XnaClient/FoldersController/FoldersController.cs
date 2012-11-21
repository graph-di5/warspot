using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WarSpot.Client.XnaClient.FoldersController
{
	// TODO: refactor as singleton?
	static class FoldersController
	{
		private static string _replaysAbsolutePath;
		private static string _intellectsAbsolutePath;

		static FoldersController()
		{
			/// <summary>
			/// Define path for replays folder
			/// </summary>
			string absolutePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			Path.Combine(absolutePath, "Replays");
			if (!Directory.Exists(absolutePath))
				Directory.CreateDirectory(absolutePath);
			_replaysAbsolutePath = absolutePath;


			/// <summary>
			/// Define path for dll folder
			/// </summary>
			// TODO: test this
			absolutePath.Remove(0);
			absolutePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			Path.Combine(absolutePath, "Intellects");
			if (!Directory.Exists(absolutePath))
				Directory.CreateDirectory(absolutePath);
			_intellectsAbsolutePath = absolutePath;
		}

		public static string GetReplayPath()
		{
			return _replaysAbsolutePath;
		}

		public static string GetDllPath()
		{
			return _intellectsAbsolutePath;
		}
	}
}
