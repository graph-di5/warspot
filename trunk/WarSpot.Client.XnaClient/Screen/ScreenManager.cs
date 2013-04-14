using System;
using System.Collections.Generic;
using Nuclex.Input;
using Nuclex.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WarSpot.Client.XnaClient.Input;

namespace WarSpot.Client.XnaClient.Screen
{
	class ScreenManager : DrawableGameComponent
	{
		public enum ScreenEnum
		{
			MainMenuScreen,
			RegistrationScreen,
			LoginScreen,
			MessageBox,
			WatchReplayScreen,
			SelectReplayScreen,
			LoadingScreen,
		}

		private static ScreenManager _instance;

		private readonly GuiManager _gui;
		private readonly InputManager _inputManager;
		private readonly Dictionary<ScreenEnum, GameScreen> _screens = new Dictionary<ScreenEnum, GameScreen>();

		private GameScreen _activeScreen;

		private ScreenManager(Game game)
			: base(game)
		{
			_gui = new GuiManager(Game.Services) { Visible = false };
			_inputManager = new InputManager(Game.Services, Game.Window.Handle);

			Game.Components.Add(_gui);
			Game.Components.Add(_inputManager);

			Controller = new InputHandler(_inputManager);
		}

		public static ScreenManager Instance
		{
			get { return _instance; }
		}

		public Controller Controller { get; private set; }

		public SpriteBatch SpriteBatch { get; private set; }

		public SpriteFont Font { get; private set; }

		public ContentManager ContentManager { get; private set; }

		public int Height
		{
			get { return GraphicsDevice.Viewport.Height; }
		}

		public int Width
		{
			get { return GraphicsDevice.Viewport.Width; }
		}

		public static void Init(Game game)
		{
			if (_instance == null)
				_instance = new ScreenManager(game);
			else
			{
				throw new Exception("Already initialized");
			}
		}

		public void SetActiveScreen(ScreenEnum screenName)
		{
			if (_screens.ContainsKey(screenName))
			{
				if (_activeScreen != null)
					_activeScreen.OnHide();

				_activeScreen = _screens[screenName];
				_gui.Screen = _activeScreen;

				_activeScreen.OnShow();
			}
			else
			{
				throw new Exception("Game screen not found");
			}
		}

		public void RegisterScreen(ScreenEnum screenName, GameScreen gameScreen)
		{
			if (!_screens.ContainsKey(screenName))
			{
				_screens.Add(screenName, gameScreen);
			}
			else
			{
				throw new Exception("Game screen is already initialized");
			}
		}

		public GameScreen GetActiveScreen()
		{
			return _activeScreen;
		}

		public override void Update(GameTime gameTime)
		{
			_activeScreen.Update(gameTime);
			Controller.Update();
			_activeScreen.HandleInput(Controller);
		}

		public override void Draw(GameTime gameTime)
		{
			_activeScreen.Draw(gameTime);
			_gui.Draw(gameTime);
		}

		protected override void LoadContent()
		{
			ContentManager = Game.Content;
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			Font = ContentManager.Load<SpriteFont>("Textures/menufont");

			foreach (string screen in Enum.GetNames(typeof(ScreenEnum)))
			{
				var type = Type.GetType("WarSpot.Client.XnaClient.Screen." + screen);
				if (type != null)
				{
					var constructorInfo = type.GetConstructor(new Type[0]);
					if (constructorInfo != null)
						RegisterScreen((ScreenEnum)Enum.Parse(typeof(ScreenEnum), screen),
						               (GameScreen)constructorInfo.Invoke(new object[0]));
				}
			}

			foreach (var gameScreen in _screens.Values)
			{
				gameScreen.LoadContent();
			}
		}

        public void ReloadWatchScreen()
        {
           GameScreen ScreenForReload;
           _screens.TryGetValue(ScreenEnum.WatchReplayScreen, out ScreenForReload);
           ScreenForReload = new WatchReplayScreen();
        }

		protected override void UnloadContent()
		{
			ContentManager.Unload();
			foreach (GameScreen gameScreen in _screens.Values)
			{
				gameScreen.Destroy();
			}
			_screens.Clear();
		}

		public void Resize()
		{
			foreach (var screen in _screens.Values)
			{
				screen.OnResize();
			}

			string current = GetActiveScreen().GetType().Name;
			SetActiveScreen((ScreenEnum)Enum.Parse(typeof(ScreenEnum), current));
		}
	}
}
	