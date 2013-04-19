using System;
using System.Collections.Generic;
using WarSpot.Contracts.Intellect;

namespace WarSpot.Common
{
	/// <summary>
	/// Class for command with loaded intellects
	/// </summary>
	public class TeamIntellectList
	{
		public Guid TeamId { set; get ;}
        public List<IBeingInterface> Members = new List<IBeingInterface>();
	}
}