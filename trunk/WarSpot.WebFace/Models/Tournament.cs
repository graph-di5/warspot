using System;
using System.Collections.Generic;

namespace WarSpot.WebFace.Models
{
	public class Tournament
	{
		public Guid ID;
		public string Creator;
		public string TournamentName;
		public string Description;
		public DateTime StartTime;
		public int MaxPlayers;
		public List<string> Players;
		// todo 
		//public string State;
	}
}