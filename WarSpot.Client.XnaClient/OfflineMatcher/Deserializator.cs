using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WarSpot.MatchComputer;
using Events = WarSpot.MatchComputer;

namespace WarSpot.Client.XnaClient.OfflineMatcher
{
	static class Deserializator
	{
		/// <summary>
		/// Returns deserializated list of GameEvents from replay
		/// </summary>
		public static List<WarSpotEvent> Deserialize(string path)
		{
			var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			var bf = new BinaryFormatter();
			var deserializedActions = (List<WarSpotEvent>)bf.Deserialize(fs);
			fs.Close();
			return deserializedActions;
		}
	}
}
