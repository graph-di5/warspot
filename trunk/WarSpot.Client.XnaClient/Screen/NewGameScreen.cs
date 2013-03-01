using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using System.Collections.Generic;
using System.Linq;
using WarSpot.Client.XnaClient.Screen.Utils;
using WarSpot.Client.XnaClient.Network;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class NewGameScreen : GameScreen
	{
		private static Texture2D _texture;

		private LabelControl _descrLabel;

		private ButtonControl _startGameButton;
		private ButtonControl _backButton;

		private InputControl _gameNameBox;

		private LabelControl _gameNameLabel;

		private ListControl _intellectFirstList;
		private ListControl _intellectSecondList;

		private bool _isShowedAgain = false;
		public bool _isOnline = true;

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

		public override void OnShow()
		{
			base.OnShow();
			/*
			if (!_isShowedAgain)
			{
				var isOk = ConnectionManager.Instance.GetListOfIntellects();
				if (isOk.Type != Contracts.Service.ErrorType.Ok)
				{
					_isShowedAgain = true;
					_isOnline = false;
					MessageBox.Show("No connection, only offline mode available", ScreenManager.ScreenEnum.NewGameScreen);
				}
				else
				{
					_isShowedAgain = true;
					_isOnline = true;
					foreach (var i in ScreenHelper.Instance.AvailableIntellects)
					{
						_intellectFirstList.Items.Add(i.Value);
						_intellectSecondList.Items.Add(i.Value);
					}
				}
			}*/
		}

		public override void OnHide()
		{
			base.OnHide();
			_intellectFirstList.Items.Clear();
			_intellectSecondList.Items.Clear();
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
						new UniScalar(0f, 540),
						new UniScalar(0f, 200),
						new UniScalar(0f, 30))
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

			_gameNameBox = new InputControl
			{
				Bounds = new UniRectangle(
							new UniScalar(0f, 275),
							new UniScalar(0f, 500),
							new UniScalar(0f, 200),
							new UniScalar(0f, 30)),
				Text = DateTime.Now.ToString()
			};

			_gameNameLabel = new LabelControl("Game name:")
			{
				Bounds = new UniRectangle(
							new UniScalar(0f, 275),
							new UniScalar(0f, 470),
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

			Desktop.Children.Add(_gameNameBox);

			Desktop.Children.Add(_gameNameLabel);

			Desktop.Children.Add(_intellectFirstList);
			Desktop.Children.Add(_intellectSecondList);

			ScreenManager.Instance.Controller.AddListener(_startGameButton, StartGameButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
		}

		private void StartGameButtonPressed(object sender, EventArgs e)
		{
			if (_intellectFirstList.SelectedItems.Count != 0 && _intellectSecondList.SelectedItems.Count != 0)
			{
				List<Guid> selectedIntellects =
					(from x in ScreenHelper.Instance.AvailableIntellects where x.Value == _intellectFirstList.Items[_intellectFirstList.SelectedItems[0]]
						 || x.Value == _intellectSecondList.Items[_intellectSecondList.SelectedItems[0]] select x.Key).ToList();
				if (_gameNameBox.Text != null)
				{
					ConnectionManager.Instance.BeginMatch(selectedIntellects, _gameNameBox.Text);
				}
				else
					ConnectionManager.Instance.BeginMatch(selectedIntellects, DateTime.Now.ToString());
			}
		}

		private void BackButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
			_isShowedAgain = false;
		}
	}
}

