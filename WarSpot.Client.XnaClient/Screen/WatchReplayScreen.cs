using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSpot.Client.XnaClient.Screen.Utils;
using WarSpot.Contracts.Service;
using WarSpot.MatchComputer;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

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

        // Array which contains world's size and ci of every single piece of world
		private int _worldWidth;
        private int _wordlHeight;
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
		// Control for preventing from too fast replay speed

		private bool _localPause = false;
		private bool _isPrepared = false;
		private const int _barHeight = 30;
		private int _timeSinceLastTurn = 0;
		private int size = 0;

        // Team info
        public static int TeamsCount = 0;
        public static Dictionary<Guid, int> Teams = new Dictionary<Guid,int>();

		// Buttons
		private ButtonControl _pauseButton;
		private ButtonControl _nextButton;
		private ButtonControl _menuButton;
		private LabelControl _turnLabel;

		public WatchReplayScreen()
		{
            TeamsCount = 0;
            Teams = new Dictionary<Guid,int>();
			CreateControls();
			InitializeControls();
		}

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
                            var tmp = _listOfCreatures.First(creature => creature.Id == tmpEvent.SubjectId);
							tmp.CurrentHealth = tmpEvent.Health;
							_listOfEvents.Remove(wsEvent);
							break;
						}
					case EventTypes.GameEventCiChange:
						{
							var tmpEvent = wsEvent as GameEventCiChange;
							var tmp = _listOfCreatures.First(creture => creture.Id == tmpEvent.SubjectId);
							tmp.CurrentCi = tmpEvent.Ci;
							_listOfEvents.Remove(wsEvent);
							break;
						}
					case EventTypes.GameEventMove:
						{
							_localPause = true;
							var tmpEvent = wsEvent as GameEventMove;
                            var tmp = _listOfCreatures.First(creture => creture.Id == tmpEvent.SubjectId);
							if (tmp.X + tmpEvent.ShiftX >= _worldWidth)
							{
								tmp.Y += tmpEvent.ShiftY;
								tmp.X = (tmp.X + tmpEvent.ShiftX) % _worldWidth;
							}
							else if (tmp.Y + tmpEvent.ShiftY >= _wordlHeight)
							{
								tmp.X += tmpEvent.ShiftX;
								tmp.Y = (tmp.Y + tmpEvent.ShiftY) % _wordlHeight;
							}
							else
							{
								tmp.X += tmpEvent.ShiftX;
								tmp.Y += tmpEvent.ShiftY;
							}
							_listOfEvents.Remove(wsEvent);
							break;
						}
					case EventTypes.GameEventDeath:
						{
							_localPause = true;
							var tmpEvent = wsEvent as GameEventDeath;
							var tmp = _listOfCreatures.First(creture => creture.Id == tmpEvent.SubjectId);
							_listOfCreatures.Remove(tmp);
							_listOfEvents.Remove(wsEvent);
							break;
						}
					case EventTypes.GameEventBirth:
						{
							var tmp = wsEvent as GameEventBirth;
							_listOfCreatures.Add(new Creature(tmp.SubjectId, tmp.Newborn.X, tmp.Newborn.Y,
								tmp.Newborn.Team, tmp.Newborn.MaxHealth, tmp.Newborn.Health, tmp.Newborn.Ci));
							_localPause = true;
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
							_turnLabel.Text = "Turn " + tmp.Number.ToString() + " / " + size.ToString();
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
							ExitReplay();
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
					if (_timeSinceLastTurn > 200)
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
					SpriteBatch.Draw(_grass, new Rectangle(j * _scaledSpriteWidth, i * _scaledSpriteHeight + _barHeight,
						_scaledSpriteWidth, _scaledSpriteHeight), Color.White);
				}
			}

			foreach (var gameObject in _listOfCreatures)
			{
				switch (gameObject.Team)
				{
				case 0:
					{
						SpriteBatch.Draw(_hedge, new Rectangle(gameObject.Y * _scaledSpriteWidth,
							gameObject.X * _scaledSpriteHeight + _barHeight,
							_scaledSpriteWidth, _scaledSpriteHeight), Color.White);
						break;
					}
				case 1:
					{
						SpriteBatch.Draw(_creatureOfFirstTeam, new Rectangle(gameObject.X * _scaledSpriteWidth, 
							gameObject.Y * _scaledSpriteHeight + _barHeight,
							_scaledSpriteWidth, _scaledSpriteHeight), Color.White);
						break;
					}
				case 2:
					{
						SpriteBatch.Draw(_creatureOfSecondTeam, new Rectangle(gameObject.X * _scaledSpriteWidth, 
							gameObject.Y * _scaledSpriteHeight + _barHeight,
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
			_listOfEvents = Utils.ScreenHelper.Instance.ReplayEvents;
			this.PrepareScreen();
			size = _listOfEvents.Count(x => x.EventType==EventTypes.SystemEventTurnStarted);
			_globalPause = false;
		}

		public override void OnHide()
		{
			base.OnHide();
			_turnLabel.Text = "0";
			size = 0;
			_globalPause = true;
			_localPause = false;
			_listOfEvents = new List<WarSpotEvent>();
			_listOfCreatures = new List<Creature>();
			_isPrepared = false;
		}

		public override void OnResize()
		{
			base.OnResize();
			this.SetScalings();
		}

		// Define a world's size
		private void CreateWorld()
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
			int width = WarSpotGame.Instance.GraphicsDevice.Viewport.Width;
			int height = WarSpotGame.Instance.GraphicsDevice.Viewport.Height - _barHeight;
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
			this.CreateWorld();
			// Prepare args for drawing
			this.SetScalings();
			// Resize screen for preventing rounding errors
			if (!_isPrepared)
			{
				this.ResizeScreenByRequirements();
			}
		}

		private void ResizeScreenByRequirements()
		{
			_isPrepared = true;
			Rectangle rect = WarSpotGame.Instance.GetScreenBounds();
			int w = SizeForWidth();
			int h = SizeForHeight();
			if (w != rect.Width || h != rect.Height)
			{
				WarSpotGame.Instance.SetScreenSize(w, h);
			}
		}

		private int SizeForWidth()
		{
			return _worldWidth * _scaledSpriteWidth;
		}

		private int SizeForHeight()
		{
			return _wordlHeight * _scaledSpriteHeight + _barHeight;
		}

		private void ExitReplay()
		{
			_listOfCreatures = new List<Creature>();
			_isPrepared = false;
			_globalPause = false;
			WarSpotGame.Instance.SetScreenSize(800, 600);
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.SelectReplayScreen);
		}

		private void CreateControls()
		{
			_pauseButton = new ButtonControl
			{
				Text = "Pause",
				Bounds =
						new UniRectangle(
								new UniScalar(0.0f, -18),
								new UniScalar(0.0f, -18),
								new UniScalar(0f, 50),
								new UniScalar(0f, 25))
			};

			_nextButton = new ButtonControl
			{
				Text = "Next",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, 35),
								new UniScalar(0f, -18),
								new UniScalar(0f, 50),
								new UniScalar(0f, 25))
			};

			_menuButton = new ButtonControl
			{
				Text = "Menu",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, WarSpotGame.Instance.GetScreenBounds().Width - 50 - 22),
								new UniScalar(0f, -18),
								new UniScalar(0f, 50),
								new UniScalar(0f, 25))
			};

			_turnLabel = new LabelControl("Turn 0")
			{
				Bounds = new UniRectangle(new UniScalar(0f, 300), new UniScalar(0f, -5), 0, 0)
			};
		}	

		private void InitializeControls()
		{
			Desktop.Children.Add(_pauseButton);
		//	Desktop.Children.Add(_nextButton);
			Desktop.Children.Add(_menuButton);
			Desktop.Children.Add(_turnLabel);

			ScreenManager.Instance.Controller.AddListener(_pauseButton, PauseButtonPressed);
		//	ScreenManager.Instance.Controller.AddListener(_nextButton, NextButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_menuButton, MenuButtonPressed);
		}

		private void PauseButtonPressed(object sender, EventArgs e)
		{
			_globalPause = !_globalPause;
		}

		private void NextButtonPressed(object sender, EventArgs e)
		{
			_globalPause = false;	
		}

		private void MenuButtonPressed(object sender, EventArgs e)
		{
			ExitReplay();
		}
	}
}
