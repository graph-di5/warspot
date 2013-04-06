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
			_watchReplayButton = new ButtonControl
			{
				Text = "Watch replay",
				Bounds =
					new UniRectangle(
						new UniScalar(0.30f, 0),
						new UniScalar(0.2f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.1f, 0)),
			};

			_logOutButton = new ButtonControl
			{
				Text = "Log Out",
				Bounds =
					new UniRectangle(
						new UniScalar(0.30f, 0),
						new UniScalar(0.35f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.1f, 0)),
			};

			_exitButton = new ButtonControl
			{
				Text = "Exit",
				Bounds =
						new UniRectangle(
								new UniScalar(0.30f, 0),
								new UniScalar(0.5f, 0),
								new UniScalar(0.4f, 0),
								new UniScalar(0.1f, 0)),
			};
		}

		private void InitializeControls()
		{

			Desktop.Children.Add(_logOutButton);
			Desktop.Children.Add(_exitButton);
			Desktop.Children.Add(_watchReplayButton);

			ScreenManager.Instance.Controller.AddListener(_logOutButton, LogOutButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_watchReplayButton, WatchReplayButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_exitButton, ExitButtonPressed);
		}

		private void WatchReplayButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.SelectReplayScreen);
		}

		private void LogOutButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
		}

		private void ExitButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.Game.Exit();
		}

	}
}
