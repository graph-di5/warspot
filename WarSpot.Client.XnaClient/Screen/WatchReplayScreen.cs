using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarSpot.MatchComputer;
using System.Collections.Generic;
using WarSpot.Client.XnaClient.OfflineMatcher;
using WarSpot.Contracts.Intellect;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class WatchReplayScreen : GameScreen
	{
		// Size of every single sprite
		private const int _sizeOfSprite = 32;
		private Texture2D _creatureOfFirstTeam;
		private Texture2D _creatureOfSecondTeam;
		private Texture2D _grass;
		private Texture2D _hedge;
		private List<WarSpotEvent> _listOfEvents = new List<WarSpotEvent>();
		private List<Creature> _listOfCreatures = new List<Creature>();
		// temporary constants
		private int _worldWidth = 40;
		private int _wordlHeight = 30;
		// Array which contains world's size and ci of every single piece of world
		private WorldCell[][] _worldMap;
		// Scaled sizes of sprites for prevention of calculating this constant every Draw(),
		// because all sprites've got the same size
		private int _scaledSpriteWidth;
		private int _scaledSpriteHeight;
		// Define a scale of drawable sprites
		private float _widthScaling;
		private float _heightScaling;
		// Variable for contolling game pause
		private bool _globalPause = false;

		public WatchReplayScreen()
		{
		}

		public override void LoadContent()
		{
			// Size of all sprites is 32x32
			_creatureOfFirstTeam = ContentManager.Load<Texture2D>("Textures/GameSprites/creature_1");
			_creatureOfSecondTeam = ContentManager.Load<Texture2D>("Textures/GameSprites/creature_2");
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
			WarSpotGame.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);
			SpriteBatch.Begin();
			for (int i = 0; i < _wordlHeight; i++)
			{
				for (int j = 0; j < _worldWidth; j++)
				{
					SpriteBatch.Draw(_creatureOfSecondTeam, new Rectangle(j * _scaledSpriteWidth, i * _scaledSpriteHeight,
						_scaledSpriteWidth, _scaledSpriteHeight), Color.White);
				}
			}

			
			SpriteBatch.End();
		}

		public override void OnShow()
		{
			base.OnShow();
			_listOfEvents = Utils.ScreenHelper.Instance.replayEvents;
			this.PrepareScreen();
		}

		// Process all initial states
		private void CreateGameObjects()
		{
			bool flag = true;
			int i = 0;
			while(flag)
			{
				WarSpotEvent WSEvent = _listOfEvents[i++];
				switch (WSEvent.EventType)
				{
					case EventTypes.SystemEventTurnStarted:
						{
							// It stops events processing after end of zero-turn
							var tmp = WSEvent as SystemEventTurnStarted;
							if (tmp.Number == 0)
								_listOfEvents.Remove(tmp);
							else
								flag = false;
							break;
						}

					case EventTypes.SystemEventWorldCreated:
						{
							var tmp = WSEvent as SystemEventWorldCreated;
							SetWorldSize(tmp.Width, tmp.Height);
							_listOfEvents.Remove(tmp);
							break;
						}
			
					case EventTypes.GameEventBirth:
						{
							var tmp = WSEvent as GameEventBirth;
							_listOfCreatures.Add(new Creature(tmp.SubjectId, tmp.Newborn.X, tmp.Newborn.Y,
								tmp.Newborn.Team, tmp.Newborn.Health, tmp.Newborn.Ci));
							_listOfEvents.Remove(tmp);
							break;
						}
					case EventTypes.GameEventWorldCiChanged:
						{
							var tmp = WSEvent as GameEventWorldCiChanged;
							_worldMap[tmp.Y][tmp.X].changeCi(tmp.Ci);
							break;
						}
				}
			}
		}

		/// <summary>
		/// Define a size of world. Must be used before any 
		/// actions over sprites, scaling etc.
		/// Use only in gameobject initialization!
		/// </summary>
		/// <param name="x"> world's width </param>
		/// <param name="y"> world's height </param>
		private void SetWorldSize(int x, int y)
		{
			_worldMap = new WorldCell[y][];
			// TODO: test this.
			for (int i = 0; i < y; i++)
			{
				_worldMap[i] = new WorldCell[x];
				for (int j = 0; j < x; j++)
				{
					_worldMap[i][j] = new WorldCell(j, i);
				}
			}
			_worldWidth = x;
			_wordlHeight = y;
			 
		}

		///<summary>
		/// Uses size of game world and calculate scaling according to screen resolution
		/// it's needed for correct drawing of whole world
		/// </summary>		
		private void SetScalings()
		{
			// Refatcor this if there apperas any necessity in frames (for turn/statictics e.g.)
			int width = WarSpotGame.Instance.GraphicsDevice.Viewport.Width;
			int height = WarSpotGame.Instance.GraphicsDevice.Viewport.Height;
			_widthScaling = (float)width / (float)(_worldWidth * _sizeOfSprite);
			_heightScaling = (float)height / (float)(_wordlHeight * _sizeOfSprite);
			_scaledSpriteWidth = (int)Math.Round(_widthScaling * _sizeOfSprite);
			_scaledSpriteHeight = (int)Math.Round(_heightScaling * _sizeOfSprite);
		}

		/// <summary>
		/// Initialize all basic data like scalings, started inGameObjects and other
		/// </summary>
		public void PrepareScreen()
		{
			// Initialization of initial inGameObjects and world map
			this.CreateGameObjects();
			
			// Prepare args for drawing
			this.SetScalings();
		}

		public override void OnResize()
		{
			base.OnResize();
			this.SetScalings();
		}
	}
}
