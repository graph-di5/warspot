using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// todo move to models
namespace WarSpot.Contracts.Service
{
	[DataContract]
	public class ReplayDescription
	{
		[DataMember]
		public Guid ID { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public List<TeamDescription> Teams { get; set; }

	}
}
