using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Events = WarSpot.MatchComputer;

namespace WarSpot.Client.XnaClient.OfflineMatcher
{
	/// <summary>
	/// Returns deserializated list of GameActions from replay
	/// </summary>
	static class Deserializator
	{
		static List<Events.WarSpotEvent> Deserialize(string path)
		{
			List<Events.WarSpotEvent> deserializedActions = new List<Events.WarSpotEvent>();
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			BinaryFormatter bf = new BinaryFormatter();
			deserializedActions = (List<Events.WarSpotEvent>)bf.Deserialize(fs);
			fs.Close();
			return deserializedActions;
		}
	}
}
