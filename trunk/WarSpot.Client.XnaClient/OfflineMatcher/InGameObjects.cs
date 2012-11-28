using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WarSpot.Client.XnaClient.OfflineMatcher
{
	/*internal enum Team
	{
		Red,
		Blue
	}*/

	internal enum GameObjectType
	{
		Stone,
		Monster,
		Bonus
	}

	class Creature
	{		
		public Guid Id { get; private set; }
		public GameObjectType Type { get; private set; }
		public int Team { get; private set; }
		public int X { get; set; }
		public int Y { get; set; }
		public bool isAlive;
		public float Ci { get; private set; }

		public Creature(Guid id, int x, int y, int team)
		{
			Id = id;
			Team = team;
			X = x;
			Y = y;
			isAlive = true;
		}
	}
}
