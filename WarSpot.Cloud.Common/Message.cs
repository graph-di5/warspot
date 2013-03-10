using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WarSpot.Cloud.Common
{
	[Serializable]
	public class Message
	{
		public Guid ID;
		public List<Guid> ListOfDlls;

		public Message(Guid id, List<Guid> listOfDlls)
		{
			ID = id;
			ListOfDlls = listOfDlls;
		}

		public Message()
		{
			ListOfDlls = new List<Guid>();
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
