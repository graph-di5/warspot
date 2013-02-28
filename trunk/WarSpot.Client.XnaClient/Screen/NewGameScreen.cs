using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class NewGameScreen : GameScreen
	{
		private static Texture2D _texture;

		private LabelControl _descrLabel;

		private ButtonControl _startGameButton;
		private ButtonControl _backButton;

		private ListControl _intellectFirstList;
		private ListControl _intellectSecondList;

		public NewGameScreen()
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
			_descrLabel = new LabelControl("Select two intellects to start game:")
			{
				Bounds = new UniRectangle(
					new UniScalar(0f, 250),
					new UniScalar(0f, 20), 
					0, 0)
			};

			_startGameButton = new ButtonControl
			{
				Text = "Fight!",
				Bounds =
					new UniRectangle(
						new UniScalar(0f, 275),
						new UniScalar(0f, 470),
						new UniScalar(0f, 200),
						new UniScalar(0f, 60))
			};

			_backButton = new ButtonControl
			{
				Text = "Back",
				Bounds =
					new UniRectangle(
						new UniScalar(0f, -10),
						new UniScalar(0f, 540),
						new UniScalar(0f, 100),
						new UniScalar(0f, 30))
			};

			_intellectFirstList = new Nuclex.UserInterface.Controls.Desktop.ListControl
			{
				SelectionMode = ListSelectionMode.Single,
				Bounds =
					new UniRectangle(
						new UniScalar(0f, 50),
						new UniScalar(0f, 50),
						new UniScalar(0f, 250),
						new UniScalar(0f, 400))
			};

			_intellectSecondList = new Nuclex.UserInterface.Controls.Desktop.ListControl
			{
				SelectionMode = ListSelectionMode.Single,
				Bounds =
					new UniRectangle(
						new UniScalar(0f, 450),
						new UniScalar(0f, 50),
						new UniScalar(0f, 250),
						new UniScalar(0f, 400))
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_descrLabel);

			Desktop.Children.Add(_startGameButton);
			Desktop.Children.Add(_backButton);

			Desktop.Children.Add(_intellectFirstList);
			Desktop.Children.Add(_intellectSecondList);

			ScreenManager.Instance.Controller.AddListener(_startGameButton, StartGameButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
		}

		private void StartGameButtonPressed(object sender, EventArgs e)
		{
		}

		private void BackButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen	);
		}
	}
}

