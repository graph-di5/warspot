using System;
using System.Collections.Generic;
using WarSpot.Contracts.Intellect;

namespace WarSpot.MatchComputer
{
	/// <summary>
	/// Class for command with loaded intellects
	/// </summary>
	public class TeamIntellectList
	{
		public int Number { set; get ;}
		public List<Type> Members = new List<Type>();
	}
}