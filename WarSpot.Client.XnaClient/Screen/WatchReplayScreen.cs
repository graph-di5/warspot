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
		// Path, which contains path to replay, selected in SelectReplayScreen (or in new Game) 
		private string _replayPath;

		// temporary constants
		private int _XSize = 100;
		private int _YSize = 75;
		private float _scale = 0.125f;
		private int scaledWidth;
		private int scaledHeight;

		public WatchReplayScreen()
		{
			CreateControls();
			InitializeControls();
			_listOfEvents = Deserializator.Deserialize(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
				"replay_2012.11.25_21.05.02.out"));
			createGameObjects();
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
					SpriteBatch.Draw(_grass, new Rectangle(i * scaledWidth, j * scaledHeight, i * scaledWidth + scaledWidth, j * scaledHeight + scaledHeight), Color.White);
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

		// TODO: maybe refactor this and make it as part of update
		private void createGameObjects()
		{
			int i = 0;
			while (_listOfEvents[i++] is GameEventBirth)
			{
				var tmp = _listOfEvents[i] as GameEventBirth;
				_listOfCreatures.Add(new Creature(tmp.SubjectId, tmp.Newborn.X, tmp.Newborn.Y, tmp.Newborn.Team));
				_listOfEvents.Remove(tmp);
			}
		}

		public void SetReplayPath(string replayPath)
		{
			_replayPath = replayPath;
		}
	}
}
