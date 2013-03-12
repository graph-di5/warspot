using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using WarSpot.Common;

namespace WarSpot.Cloud.Common
{
	[Serializable]
	public class Message
	{
		public Guid ID;
		public List<TeamInfo> TeamList;
		//public List<Guid> ListOfDlls;

		public Message(Guid id, List<TeamInfo> teamList)
		{
			ID = id;
			TeamList = teamList;
			//ListOfDlls = listOfDlls;
		}

		public Message()
		{
			TeamList = new List<TeamInfo>();
			//ListOfDlls = new List<Guid>();
		}

		public byte[] ToByteArray()
		{
			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();

			bf.Serialize(stream, this);
			return stream.ToArray();
		}
	}
}
