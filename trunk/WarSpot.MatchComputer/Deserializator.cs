using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WarSpot.Common.Utils;

namespace WarSpot.MatchComputer
{
	public static class Deserializator
	{
		public static List<WarSpotEvent> Deserialize(Stream fs)
		{
			var bf = new BinaryFormatter();
			var checker = VersionHelper.CheckVersionFromStream(fs);
			if (checker)
			{
				return (List<WarSpotEvent>)bf.Deserialize(fs);
			}
			else
			{
				return new List<WarSpotEvent>();
			}
		}

		/// <summary>
		/// Returns deserializated list of GameEvents from replay
		/// </summary>
		public static List<WarSpotEvent> Deserialize(string path)
		{
			var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			var res = Deserialize(fs);
			fs.Close();
			return res;
		}

	}
}
