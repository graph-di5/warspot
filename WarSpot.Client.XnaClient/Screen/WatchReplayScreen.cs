﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSpot.Client.XnaClient.OfflineMatcher;
using WarSpot.MatchComputer;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class WatchReplayScreen : GameScreen
	{
		// Size of every single sprite
		private const int SizeOfSprite = 32;
		private Texture2D _creatureOfFirstTeam;
		private Texture2D _creatureOfSecondTeam;
		private Texture2D _grass;
		private Texture2D _hedge;
		private List<WarSpotEvent> _listOfEvents = new List<WarSpotEvent>();
		private List<Creature> _listOfCreatures = new List<Creature>();
		private int _worldWidth;
		private int _wordlHeight;
		// Array which contains world's size and ci of every single piece of world
		private WorldCell[][] _worldMap;
		// Scaled sizes of sprites for prevention of calculating this constant every Draw(),
		// because all sprites've got the same size
		private int _scaledSpriteWidth;
		private int _scaledSpriteHeight;
		// Define a scale of drawable sprites
		private float _widthScaling;
		private float _heightScaling;
		// Variable for contolling game pause by user
		private bool _globalPause = false;
		// Controls update for preventing from too fast replay speed
		private bool _localPause = false;
		private int _timeSinceLastTurn = 0;

		public override void LoadContent()
		{
			_creatureOfFirstTeam = ContentManager.Load<Texture2D>("Textures/GameSprites/creature_1");
			_creatureOfSecondTeam = ContentManager.Load<Texture2D>("Textures/GameSprites/creature_2");
			_grass = ContentManager.Load<Texture2D>("Textures/GameSprites/grass");
			_hedge = ContentManager.Load<Texture2D>("Textures/GameSprites/hedge");
		}

		public override void Update(GameTime gameTime)
		{
			if (!_globalPause)
			{
				if (!_localPause)
				{
					WarSpotEvent wsEvent = _listOfEvents.First();

					switch (wsEvent.EventType)
					{
					case EventTypes.GameEventHealthChange:
						{
							var tmpEvent = wsEvent as GameEventHealthChange;
							var tmp = _listOfCreatures.Where(creture => creture.Id == tmpEvent.SubjectId).First();
							tmp.CurrentHealth = tmpEvent.Health;
							_listOfEvents.Remove(wsEvent);
							break;
						}
					case EventTypes.GameEventCiChange:
						{
							var tmpEvent = wsEvent as GameEventCiChange;
							var tmp = _listOfCreatures.Where(creture => creture.Id == tmpEvent.SubjectId).First();
							tmp.CurrentCi = tmpEvent.Ci;
							foreach (var i in _listOfCreatures)
							{
								if (i.Id == tmpEvent.SubjectId)
								{
									int x = 0;
								}
							}

							_listOfEvents.Remove(wsEvent);
							break;
						}
					case EventTypes.GameEventMove:
						{
							_localPause = true;
							var tmpEvent = wsEvent as GameEventMove;
							var tmp = _listOfCreatures.Where(creture => creture.Id == tmpEvent.SubjectId).First();
							tmp.X += tmpEvent.ShiftX;
							tmp.Y += tmpEvent.ShiftY;
							_listOfEvents.Remove(wsEvent);
							break;
						}
					case EventTypes.GameEventDeath:
						{
							_localPause = true;
							var tmpEvent = wsEvent as GameEventDeath;
							var tmp = _listOfCreatures.Where(creture => creture.Id == tmpEvent.SubjectId).First();
							_listOfCreatures.Remove(tmp);
							_listOfEvents.Remove(wsEvent);
							break;
						}
					case EventTypes.GameEventBirth:
						{
							var tmp = wsEvent as GameEventBirth;
							_listOfCreatures.Add(new Creature(tmp.SubjectId, tmp.Newborn.X, tmp.Newborn.Y,
								tmp.Newborn.Team, tmp.Newborn.MaxHealth, tmp.Newborn.Health, tmp.Newborn.Ci));
							_listOfEvents.Remove(tmp);
							break;
						}
					case EventTypes.GameEventWorldCiChanged:
						{
							var tmp = wsEvent as GameEventWorldCiChanged;
							_worldMap[tmp.Y][tmp.X].changeCi(tmp.Ci);
							_listOfEvents.Remove(tmp);
							break;
						}
					case EventTypes.SystemEventTurnStarted:
						{
							var tmp = wsEvent as SystemEventTurnStarted;
							_listOfEvents.Remove(tmp);
							break;
						}
					case EventTypes.SystemEventCommandWin:
						{
							var tmp = wsEvent as SystemEventCommandWin;
							_listOfEvents.Remove(tmp);
							break;
						}
					case EventTypes.SystemEventCommandDead:
						{
							var tmp = wsEvent as SystemEventCommandDead;
							_listOfEvents.Remove(tmp);
							break;
						}
					case EventTypes.SystemEventMatchEnd:
						{
							var tmp = wsEvent as SystemEventMatchEnd;
							ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.SelectReplayScreen);
							_listOfEvents.Remove(tmp);
							break;
						}
					default:
						_listOfEvents.RemoveAt(0);
						break;
					}
				}
				else
				{
					// Checks time per unit game action
					_timeSinceLastTurn += gameTime.ElapsedGameTime.Milliseconds;
					// 1000 - delay between game actions	
					if (_timeSinceLastTurn > 1000)
					{
						_localPause = false;
						_timeSinceLastTurn = 0;
					}
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			WarSpotGame.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);
			SpriteBatch.Begin();
			for (int i = 0; i < _wordlHeight; i++)
			{
				for (int j = 0; j < _worldWidth; j++)
				{
					SpriteBatch.Draw(_grass, new Rectangle(j * _scaledSpriteWidth, i * _scaledSpriteHeight,
						_scaledSpriteWidth, _scaledSpriteHeight), Color.White);
				}
			}

			foreach (var gameObject in _listOfCreatures)
			{
				switch (gameObject.Team)
				{
				case 0:
					{
						SpriteBatch.Draw(_hedge, new Rectangle(gameObject.Y * _scaledSpriteWidth, gameObject.X * _scaledSpriteHeight,
							_scaledSpriteWidth, _scaledSpriteHeight), Color.White);
						break;
					}
				case 1:
					{
						SpriteBatch.Draw(_creatureOfFirstTeam, new Rectangle(gameObject.X * _scaledSpriteWidth, gameObject.Y * _scaledSpriteHeight,
							_scaledSpriteWidth, _scaledSpriteHeight), Color.White);
						//float HPpercent = gameObject.CurrentHealth / gameObject.MaxHealth;
						break;
					}
				case 2:
					{
						SpriteBatch.Draw(_creatureOfSecondTeam, new Rectangle(gameObject.X * _scaledSpriteWidth, gameObject.Y * _scaledSpriteHeight,
							_scaledSpriteWidth, _scaledSpriteHeight), Color.White);
						break;
					}
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
			if (_listOfEvents.Count != 0)
			{
				WarSpotEvent wsEvent = _listOfEvents.First();
				if (wsEvent.EventType == EventTypes.SystemEventWorldCreated)
				{
					var tmp = wsEvent as SystemEventWorldCreated;
					SetWorldSize(tmp.Width, tmp.Height);
					_listOfEvents.Remove(tmp);
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
			_widthScaling = (float)width / (float)(_worldWidth * SizeOfSprite);
			_heightScaling = (float)height / (float)(_wordlHeight * SizeOfSprite);
			_scaledSpriteWidth = (int)Math.Round(_widthScaling * SizeOfSprite);
			_scaledSpriteHeight = (int)Math.Round(_heightScaling * SizeOfSprite);
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
