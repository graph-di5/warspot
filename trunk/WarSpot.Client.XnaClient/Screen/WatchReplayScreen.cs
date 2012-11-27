using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSpot.MatchComputer;
using System.Collections.Generic;
using WarSpot.Client.XnaClient.OfflineMatcher;
using WarSpot.Contracts.Intellect;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class WatchReplayScreen : GameScreen
	{

		private Texture2D _creature;
		private Texture2D _grass;
		private Texture2D _hedge;
		private List<WarSpotEvent> _listOfEvents = new List<WarSpotEvent>();
		private List<Creature> _listOfCreatures = new List<Creature>();
		// Для экрана 800х600, потом изменять динамически
		// 100х75 размер в клетках
		private int _XSize = 100;
		private int _YSize = 75;
		private float _scale = 0.125f;
		private int scaledWidth;
		private int scaledHeight;

		public WatchReplayScreen()
		{
			CreateControls();
			InitializeControls();
		}

		public override void LoadContent()
		{
			_creature = ContentManager.Load<Texture2D>("Textures/GameSprites/creature");
			_grass = ContentManager.Load<Texture2D>("Textures/GameSprites/grass");
			_hedge = ContentManager.Load<Texture2D>("Textures/GameSprites/hedge");
			scaledWidth = (int)Math.Round(_grass.Width * _scale);
			scaledHeight = (int)Math.Round(_grass.Height * _scale);
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();

			for (int i = 0; i <= _XSize; i++)
			{
				for (int j = 0; j <= _YSize; j++)
				{
					SpriteBatch.Draw(_grass, new Rectangle(i * scaledWidth, j * scaledHeight, i * 32 + scaledWidth, j * 32 + scaledHeight), Color.White);					
				}
			}
			SpriteBatch.End();
		}

		private void CreateControls()
		{
	
		}

		private void InitializeControls()
		{

		}
	}
}
