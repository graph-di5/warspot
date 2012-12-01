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
		// temporary default value
		private string _replayPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
			"replay_2012.11.25_21.05.02.out");

		// temporary constants
		private int _worldWidth = 100;
		private int _wordlHeight = 75;

		// Array which contains world's size and ci of every single piece of world
		private WorldCell[][] _worldMap;

		// define a scale of drawable sprites
		private float _widthScaling;
		private float _heightScaling;

		public WatchReplayScreen()
		{
		}

		public override void LoadContent()
		{
			_creature = ContentManager.Load<Texture2D>("Textures/GameSprites/creature");
			_grass = ContentManager.Load<Texture2D>("Textures/GameSprites/grass");
			_hedge = ContentManager.Load<Texture2D>("Textures/GameSprites/hedge");
	
		}

		// TODO: correct game state updating (using timer?)
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.End();
		}

		// TODO: add worldCell initializing
		private void CreateGameObjects()
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
		/// Use only in gameobject in gameo bjects initialization!
		/// </summary>
		/// <param name="x"> world's width </param>
		/// <param name="y"> world's height </param>
		private void SetWorldSize(int x, int y)
		{
			// TODO: test this.

			/*	for (int i = 0; i < y; i++)
				{
					for (int j = 0; j < x; j++)
					{
						_worldMap[i][j] = new WorldCell(j, i);
					}
				}
				_worldWidth = _worldMap[0].Length;
				_wordlHeight = _worldMap.Length;
			 */
		}

		///<summary>
		/// Uses size of game world and calculate scaling according to screen resolution
		/// it's needed for correct drawing of whole world
		/// </summary>		
		private void SetScaling()
		{
			// Refatcor this if there apperas any necessity in frames (for turn/statictics e.g.)
			int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			// Use after adding world's size event
			//_widthScaling = _worldMap[0].Length / width;
			//_heightScaling = _worldMap.Length / height;

			// Temporary
			_widthScaling = _worldWidth / width;
			_heightScaling = _wordlHeight / height;
		}

		/// <summary>
		/// Initialize list of events
		/// </summary>
		private void InitializeReplay()
		{
			_listOfEvents = Deserializator.Deserialize(_replayPath);
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
			// Initializing of event list
			this.InitializeReplay();

			// Initializing of initial inGameObjects and world map
			this.CreateGameObjects();
			
			// Prepare args for drawing
			this.SetScaling();
		}
	}
}
