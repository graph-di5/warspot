﻿using System.Runtime.Serialization;

namespace WarSpot.Contracts.Service
{
	/// <summary>
	/// Prototype-class for uploading replays to client
	/// </summary>
	[DataContract]
	class Replay
	{
		[DataMember]
		public string name;
		[DataMember]
		public byte[] data;
	}
}
