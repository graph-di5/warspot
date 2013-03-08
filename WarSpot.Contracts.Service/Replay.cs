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
        public Guid Id { get; set; }

        [DataMember]
        public MatchReplay Data { get; set; }

        public Replay(Guid id, MatchReplay data)
		{
			this.Id = id;
			this.Data = data;
		}
	}
}
