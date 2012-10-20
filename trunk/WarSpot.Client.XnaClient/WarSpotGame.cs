using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarSpot.Client.XnaClient.Screen;

namespace WarSpot.Client.XnaClient
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class WarSpotGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

		private static WarSpotGame _instance;

		public static WarSpotGame Instance { get { return _instance; } }

        public WarSpotGame()
        {
			_instance = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            bool fscreen = Settings.Default.FullScreenSelected;
            switch (fscreen)
            {
                case false:
                    {
                        _graphics.PreferredBackBufferWidth = 800;
                        _graphics.PreferredBackBufferHeight = 600;
                    }
                    break;
                case true:
					System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
					_graphics.PreferredBackBufferWidth = rect.Width;
					_graphics.PreferredBackBufferHeight = rect.Height;
					System.Windows.Forms.Form.FromHandle(Window.Handle).FindForm().FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
					//_graphics.IsFullScreen = true;
                    break;
            }

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ScreenManager.Init(this);
            Components.Add(ScreenManager.Instance);

            base.Initialize();

            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

		public void ToggleFullScreen()
		{
			//_graphics.ToggleFullScreen();
			//if (_graphics.IsFullScreen)
			//{
			//    System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
			//    _graphics.PreferredBackBufferWidth = rect.Width;
			//    _graphics.PreferredBackBufferHeight = rect.Height;
			//}
			//else
			//{
			//    _graphics.PreferredBackBufferWidth = 800;
			//    _graphics.PreferredBackBufferHeight = 600;
			//}
			//_graphics.ApplyChanges();
		}

		public bool IsFullScreen()
		{
			return _graphics.IsFullScreen;
		}

		public Rectangle GetScreenBounds()
		{
			return new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
		}
    }
}
