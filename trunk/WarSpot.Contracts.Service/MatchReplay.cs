using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WarSpot.Contracts.Service
{
	/// <summary>
	/// Используем этот класс для хранения и сериализации истории событий.
	/// </summary>
	[DataContract]
	public class MatchReplay
	{
		[DataMember]
		public Version AssemblyVersion { get; set; }

		[DataMember]
		public List<WarSpotEvent> Events;

		[DataMember]
		public Guid WinnerTeam;

		[DataMember]
		public ulong Steps;

		public MatchReplay()
		{
			AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			Events = new List<WarSpotEvent>();
		}
	}
}
