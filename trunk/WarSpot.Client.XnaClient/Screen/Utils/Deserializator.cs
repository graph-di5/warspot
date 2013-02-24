using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WarSpot.Common.Utils;
using WarSpot.MatchComputer;
using Events = WarSpot.MatchComputer;
using WarSpot.Common;

namespace WarSpot.Client.XnaClient.Screen.Utils
{
	static class Deserializator
	{
		/// <summary>
		/// Returns deserializated list of GameEvents from replay
		/// </summary>
		public static bool Deserialize(string path)
		{
			var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			var bf = new BinaryFormatter();
			var checker = VersionHelper.CheckVersionFromStream(fs);
			if (checker)
			{
				ReplayHelper.Instance.replayEvents = (List<WarSpotEvent>)bf.Deserialize(fs);
				fs.Close();
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
