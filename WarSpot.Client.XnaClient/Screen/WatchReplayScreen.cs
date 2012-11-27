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

		private int _XSize = 100;
		private int _YSize = 75;
		private float _scale = 1 / 8;

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
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();

			for (int i = 0; i < _XSize; i++)
			{
				for (int j = 0; j < _YSize; j++)
				{
					//SpriteBatch.Draw(_grass, new Vector2(i * _grass.Width, j * _grass.Height), Color.White);
					SpriteBatch.Draw(_grass, new Rectangle(i * 32, j * 32, i * 32 + (int)Math.Round(_scale * 32), j * 32 + (int)Math.Floor(_scale * 32)), Color.White);
				}
			}
			SpriteBatch.End();
		}

		private void CreateControls()
		{
			_listOfEvents = OfflineMatcher.Deserializator.Deserialize(@"replay_2012.11.25_21.05.02.out");

			foreach (WarSpotEvent i in _listOfEvents)
			{
				if (i is GameEventBirth)
				{
					
				//	int a = i.Newborn.Health;
				//	Creature tmp = new OfflineMatcher.Creature()
				//	_listOfCreatures.Add();
				}
			}

		}

		private void InitializeControls()
		{

		}
	}
}
