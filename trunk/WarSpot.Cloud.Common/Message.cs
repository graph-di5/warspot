using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WarSpot.Cloud.Common
{
	public class Message
	{
		public Guid ID;
		public List<Guid> ListOfDlls;

        public Message(Guid _ID, List<Guid> _ListOfDlls)
        {
            this.ID = _ID;
            this.ListOfDlls = _ListOfDlls;
        }

        public Message()
        {
            this.ListOfDlls = new List<Guid>();
        }

        public override byte[] ToByteArray()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            bf.Serialize(stream, this);
            return stream.ToArray();
        }
	}
}
