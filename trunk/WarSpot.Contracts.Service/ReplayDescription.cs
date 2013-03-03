using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
		public List<string> Intellects;

		/*
		public Guid ID
		{
				get { return id; }
				set { id = value; }
		}

		public string Name
		{
				get { return name; }
				set { name = value; }
		}

		public List<string> Intellects
		{
				get { return intellects; }
				set { intellects = value; }
		}

		*/


	}
}
