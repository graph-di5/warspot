using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WarSpot.Contracts.Service
{
	[DataContract]
	public class TeamDescription
	{
		[DataMember]
		public Guid ID { get; set; }

		[DataMember]
		public List<Guid> Intellects { get; set; }
	}
}
