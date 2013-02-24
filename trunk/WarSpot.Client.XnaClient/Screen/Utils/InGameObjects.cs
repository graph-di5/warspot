using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
		public Guid Id { get; private set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int Team { get; private set; }
		public float MaxHealth { get; private set; }
		public float CurrentHealth { get; set; }
		public float CurrentCi { get; set; }
		public float MaxCi { get; private set; }
		public GameObjectType Type { get; private set; }
		public bool isAlive;

		public Creature(Guid id, int x, int y, int team, float maxHealth, float currHP, float currCi)
		{
			Id = id;
			X = x;
			Y = y;
			Team = team;
			MaxHealth = maxHealth;
			CurrentHealth = currHP;
			CurrentCi = currCi;
			MaxCi = maxHealth;
			isAlive = true;
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
