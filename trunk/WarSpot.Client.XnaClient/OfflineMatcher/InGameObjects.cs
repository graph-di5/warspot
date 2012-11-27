using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WarSpot.Client.XnaClient.OfflineMatcher
{
	internal enum Team
	{
		Red,
		Blue
	}

	internal enum GameObjectType
	{
		Stone,
		Monstrer
	}

	class Creature
	{		
		public Guid id { get; private set; }
		public Team team { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public bool isAlive;

		public Creature(Guid id, int x, int y)
		{
			this.id = id;
			X = x;
			Y = y;
		}

	}

	class Ground
	{
		public static Texture2D texture;
		public int X;
		public int Y;
	}
}
