using System;
using System.Collections.Generic;

namespace WarSpot.Client.XnaClient.Screen.Utils
{
	internal enum GameObjectType
	{
		Stone,
		Monster,
		Bonus
	}

	/// <summary>
	/// Class for storing data of all drawable objects
	/// </summary>
	class Creature
	{
		private static int _teamsCount;
		private static Dictionary<Guid, int> _teams;
		public static void ResetTeams()
		{
			_teamsCount = 0;
			_teams = new Dictionary<Guid, int>();
		}
		public Guid Id { get; private set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int Team { get; private set; }
		public float MaxHealth { get; private set; }
		public float CurrentHealth { get; set; }
		public float CurrentCi { get; set; }
		public float MaxCi { get; private set; }
		public GameObjectType Type { get; private set; }
		public bool IsAlive;

		public Creature(Guid id, int x, int y, Guid team, float maxHealth, float currHp, float currCi)
		{
			if(_teams.ContainsKey(team))
			{
				Team = _teams[team];
			}
			else
			{
				if(team == Guid.Empty)
				{
					Team = 0;
				}
				else
				{
					Team = ++_teamsCount;
					_teams.Add(team, _teamsCount);
				}
			}
			Id = id;
			X = x;
			Y = y;
			MaxHealth = maxHealth;
			CurrentHealth = currHp;
			CurrentCi = currCi;
			MaxCi = maxHealth;
			IsAlive = true;
		}
	}

	/// <summary>
	/// Class for working with Ci (because Ci isn't inGameObject)
	/// </summary>
	class WorldCell
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public float Ci { get; private set; }

		public WorldCell(int x, int y)
		{
			this.X = x;
			this.Y = y;
			this.Ci = 0;
		}

		public void changeCi(float ci)
		{
			this.Ci = ci;
		}
	}
}
