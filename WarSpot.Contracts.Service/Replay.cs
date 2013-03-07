using System.Runtime.Serialization;
using System;

// todo move to models
namespace WarSpot.Contracts.Service
{
	/// <summary>
	/// Prototype-class for uploading replays to client
	/// </summary>
	[DataContract]
	public class Replay
	{
		[DataMember]
		public Guid id;
		[DataMember]
		public byte[] data;

		public Replay(Guid _id, byte[] _data)
		{
			this.id = _id;
			this.data = _data;
		}
	}
}
