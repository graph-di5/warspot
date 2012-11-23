using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Intellect = WarSpot.Contracts.Intellect;

namespace WarSpot.Client.XnaClient.OfflineMatcher
{
	/// <summary>
	/// Returns deserializated list of GameActions from replay
	/// </summary>
	static class Deserializator
	{
		static List<Intellect.Actions.GameAction> Deserialize(string path)
		{
			List<Intellect.Actions.GameAction> deserializedActions = new List<Intellect.Actions.GameAction>();
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			BinaryFormatter bf = new BinaryFormatter();
			deserializedActions = (List<Intellect.Actions.GameAction>)bf.Deserialize(fs);
			fs.Close();
			return deserializedActions;
		}
	}
}
