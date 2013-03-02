using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using System.IO;
using WarSpot.Client.XnaClient.Screen.Utils;
using WarSpot.Client.XnaClient.Network;

namespace WarSpot.Client.XnaClient.Screen
{
	// TODO: filling listbox and deleting replays
	internal class SelectReplayScreen : GameScreen
	{
		private static Texture2D _texture;

		private ButtonControl _watchButton;
		private ButtonControl _deleteButton;
		private ButtonControl _backButton;

		private LabelControl _replayLabel;

		private ListControl _replaysList;

		public SelectReplayScreen()
		{
			CreateControls();
			InitializeControls();
		}

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/Screens/screen_02");
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (ScreenHelper.Instance.IsOnline)
				_deleteButton.Enabled = false;
			else
				_deleteButton.Enabled = true;
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
			if (ScreenHelper.Instance.IsOnline)
			{
				var isOk = ConnectionManager.Instance.GetListOfGames();
				if (isOk.Type == Contracts.Service.ErrorType.Ok)
				{
					foreach (var i in ScreenHelper.Instance.ListOfGames)
					{
						_replaysList.Items.Add(i.ToString());
					}
				}
				else
				{
					GetSavedReplays();
					MessageBox.Show("Some error occurs, only saved replays available", ScreenManager.ScreenEnum.SelectReplayScreen);
				}
			}
			else
			{
				GetSavedReplays();
				MessageBox.Show("You play in offline mode, only saved replays available", ScreenManager.ScreenEnum.SelectReplayScreen);
			}

		}

		private void CreateControls()
		{
			_replaysList = new ListControl
			{
				SelectionMode = ListSelectionMode.Single,
				Bounds =
					new UniRectangle(
						new UniScalar(0f, 0),
						new UniScalar(0f, 20),
						new UniScalar(0f, 300),
						new UniScalar(0f, 300))
			};

			_replaysList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_replaysList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_replaysList.Slider.Bounds.Size.Y.Offset -= 2.0f;
			_replaysList.SelectionMode = Nuclex.UserInterface.Controls.Desktop.ListSelectionMode.Single;
			_replaysList.SelectedItems.Add(0);

			_replayLabel = new LabelControl("Select replay:")
			{
				Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), 0, 0)
			};

			_watchButton = new ButtonControl
			{
				Text = "Watch",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, 310),
								new UniScalar(0f, 20),
								new UniScalar(0f, 100),
								new UniScalar(0f, 30))
			};

			_deleteButton = new ButtonControl
			{
				Text = "Delete",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, 310),
								new UniScalar(0.1f, 0),
								new UniScalar(0f, 100),
								new UniScalar(0f, 30))
			};

			_backButton = new ButtonControl
			{
				Text = "Back",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, 0),
								new UniScalar(0.1f, 280),
								new UniScalar(0f, 100),
								new UniScalar(0f, 30))
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_deleteButton);
			Desktop.Children.Add(_watchButton);
			Desktop.Children.Add(_backButton);

			Desktop.Children.Add(_replayLabel);

			Desktop.Children.Add(_replaysList);

			ScreenManager.Instance.Controller.AddListener(_watchButton, WatchButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_deleteButton, DeleteButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);

			UpdateReplaysList();
		}

		private static void WatchButtonPressed(object sender, EventArgs e)
		{
			// Temporary solution
			//string selectedReplay = _replaysBox.Items[_replaysBox.SelectedItems[0]];
			//string path = Path.Combine(FoldersHelper.FoldersHelper.GetReplayPath(), selectedReplay);
			Utils.ScreenHelper.Instance.ReplayPath = "replay_test.out";
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoadingScreen);
		}

		private void DeleteButtonPressed(object sender, EventArgs e)
		{
			try
			{
				string forDelete = _replaysList.Items[_replaysList.SelectedItems[0]];
				File.Delete(Path.Combine(Utils.FoldersHelper.GetReplayPath(), forDelete));
				UpdateReplaysList();
			}
			catch
			{
				MessageBox.Show("Select saved replay!", ScreenManager.ScreenEnum.WatchReplayScreen);
			}

		}

		private void BackButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
		}

		private void UpdateReplaysList()
		{
			if (!ScreenHelper.Instance.IsOnline)
			{
				GetSavedReplays();
			}
		}

		private void GetSavedReplays()
		{
			string[] replays = Directory.GetFiles(FoldersHelper.GetReplayPath());
			foreach (var i in replays)
			{
				_replaysList.Items.Add(i);
			}
		}
	}
}
