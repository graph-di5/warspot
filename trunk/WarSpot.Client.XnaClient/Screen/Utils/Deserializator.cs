using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WarSpot.MatchComputer;
using Events = WarSpot.MatchComputer;

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
			var checker = WarSpot.Common.VersionHelper.CheckVersionFromStream(fs, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
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
