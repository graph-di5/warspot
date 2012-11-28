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
		// Contains path to replay, selected in SelectReplayScreen (or in new Game) 
		private string _replayPath;

		// temporary constants
		private int _worldWidth = 100;
		private int _wordlHeight = 75;
		// define a scale of drawable sprites
		private float _widthScaling;
		private float _heightScaling;

		public WatchReplayScreen()
		{
			// Temporary
			_listOfEvents = Deserializator.Deserialize(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
				"replay_2012.11.25_21.05.02.out"));
			createGameObjects();
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
			SpriteBatch.End();
		}

		// TODO: maybe refactor this and make it as part of update
		// or make own udpate with blackjack (called one time before drawing and base updating)
		private void createGameObjects()
		{
			int i = 0;
			// Temporary, incorrect for random replay
			while (_listOfEvents[i++] is GameEventBirth)
			{
				var tmp = _listOfEvents[i] as GameEventBirth;
				_listOfCreatures.Add(new Creature(tmp.SubjectId, tmp.Newborn.X, tmp.Newborn.Y, tmp.Newborn.Team));
				_listOfEvents.Remove(tmp);
			}
		}

		/// <summary>
		/// Define a size of world. Must be used before any 
		/// actions over sprites, scaling etc.
		/// </summary>
		/// <param name="x"> world's width</param>
		/// <param name="y"> world's height</param>
		private void setWorldSize(int x, int y)
		{
			_worldWidth = x;
			_wordlHeight = y;
		}

		///<summary>
		/// Uses size of game world and calculate scaling according to screen resolution
		/// it's needed for correct drawing of whole world
		/// </summary>		
		private void setScaling()
		{
			// Refatcor this if there apperas any necessity in frames (for turn/statictics e.g.)
			int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			_widthScaling = _worldWidth / width;
			_heightScaling = _wordlHeight / height;
		}

		/// <summary>
		/// Public function for passing current replay's path to WatchReplayScreen object
		/// </summary>
		/// <param name="replayPath"> determine path for rendered replay </param>
		public void SetReplayPath(string replayPath)
		{
			_replayPath = replayPath;
		}

		/// <summary>
		/// Initialize all basic data like scalings, started inGameObjects and other
		/// </summary>
		public void PrepareScreen()
		{
			// Prepare args for drawing
			this.setScaling();
			// TODO: initialize of all initial inGameObjects
		}
	}
}
