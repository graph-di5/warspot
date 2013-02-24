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
		public static List<WarSpotEvent> Deserialize(string path)
		{
			var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			var bf = new BinaryFormatter();
			//TODO: Надо сравнивать с System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            System.Version wsVersion = (System.Version)bf.Deserialize(fs);
			ScreenHelper.Instance.CurrReplayVersion = wsVersion;
            var deserializedActions = (List<WarSpotEvent>)bf.Deserialize(fs);
			fs.Close();
			return deserializedActions;
		}
	}
}
