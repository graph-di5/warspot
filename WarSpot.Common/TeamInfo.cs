using System;
using System.Collections.Generic;

namespace WarSpot.Common
{
	/// <summary>
	/// Class for command with loaded intellects
	/// </summary>
	[Serializable]
	public class TeamInfo
	{
		public Guid TeamId { set; get ;}
		public List<Guid> Members = new List<Guid>();
	}

	public class TeamTextInfo
	{
		public bool IsWinner;
		public Guid TeamId { set; get; }
		public List<String> Members = new List<string>();
	}
}