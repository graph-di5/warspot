using System;
using System.Collections.Generic;

namespace WarSpot.Common
{
	/// <summary>
	/// Class for command with loaded intellects
	/// </summary>
	public class TeamIntellectList
	{
		public Guid TeamId { set; get ;}
		public List<Type> Members = new List<Type>();
	}
}