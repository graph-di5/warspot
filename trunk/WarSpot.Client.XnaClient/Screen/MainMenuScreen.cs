using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class MainMenuScreen : GameScreen
	{
		private static Texture2D _texture;


		private ButtonControl _startGameButton;
		private ButtonControl _localAIButton;
		private ButtonControl _logOutButton;
		private ButtonControl _exitButton;
		private ButtonControl _watchReplayButton;

		public MainMenuScreen()
		{
			CreateControls();
			InitializeControls();
		}

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/Screens/mainMenuScreen");
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, WarSpotGame.Instance.GetScreenBounds(), Color.White);
			SpriteBatch.End();
		}
		private void CreateControls()
		{
			_startGameButton = new ButtonControl
			{
				Text = "New game",
				Bounds =
					new UniRectangle(
						new UniScalar(0.30f, 0),
						new UniScalar(0.1f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.1f, 0)),
			};

			_localAIButton = new ButtonControl
			{
				Text = "AI Manage",
				Bounds =
						new UniRectangle(
								new UniScalar(0.30f, 0),
								new UniScalar(0.25f, 0),
								new UniScalar(0.4f, 0),
								new UniScalar(0.1f, 0)),
			};

			_watchReplayButton = new ButtonControl
			{
				Text = "Watch replay",
				Bounds =
					new UniRectangle(
						new UniScalar(0.30f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.1f, 0)),
			};

			_logOutButton = new ButtonControl
			{
				Text = "Log Out",
				Bounds =
					new UniRectangle(
						new UniScalar(0.30f, 0),
						new UniScalar(0.55f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.1f, 0)),
			};

			_exitButton = new ButtonControl
			{
				Text = "Exit",
				Bounds =
						new UniRectangle(
								new UniScalar(0.30f, 0),
								new UniScalar(0.7f, 0),
								new UniScalar(0.4f, 0),
								new UniScalar(0.1f, 0)),
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_localAIButton);
			ScreenManager.Instance.Controller.AddListener(_localAIButton, localAIButtonPressed);

			Desktop.Children.Add(_logOutButton);
			Desktop.Children.Add(_exitButton);
			Desktop.Children.Add(_watchReplayButton);
			Desktop.Children.Add(_startGameButton);

			ScreenManager.Instance.Controller.AddListener(_startGameButton, startGameButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_logOutButton, logOutButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_watchReplayButton, watchReplayButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_exitButton, exitButtonPressed);
		}

		private void startGameButtonPressed(object sender, EventArgs e)
		{

		}

		private void localAIButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.AIManageScreen);
		}

		private void watchReplayButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.SelectReplayScreen);
		}

		private void logOutButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
		}

		private void exitButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.Game.Exit();
		}

	}
}
