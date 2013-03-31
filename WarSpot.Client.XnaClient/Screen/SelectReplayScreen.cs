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
	internal class SelectReplayScreen : GameScreen
	{
		private static Texture2D _texture;

		private ButtonControl _watchButton;
		private ButtonControl _deleteButton;
		private ButtonControl _backButton;

		private LabelControl _replayLabel;

		private ListControl _replaysList;

		private bool _isShowedAgain = false;

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

			if (_replaysList.SelectedItems.Count == 0)
				_watchButton.Enabled = false;
			else
				_watchButton.Enabled = true;
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, WarSpotGame.Instance.GetScreenBounds(), Color.White);
			SpriteBatch.End();
		}

		public override void OnShow()
		{
			if (!_isShowedAgain)
			{
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
						//_isShowedAgain = true;
						//MessageBox.Show("Some error occurs, only saved replays available", ScreenManager.ScreenEnum.SelectReplayScreen);
					}
				}
				else
				{
					GetSavedReplays();
					//_isShowedAgain = true;
					//MessageBox.Show("You play in offline mode\n, only saved replays available", ScreenManager.ScreenEnum.SelectReplayScreen);

				}
			}
			base.OnShow();
		}

		public override void OnHide()
		{
			base.OnHide();
			_replaysList.Items.Clear();
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

		private void WatchButtonPressed(object sender, EventArgs e)
		{
			Utils.ScreenHelper.Instance.ReplayPath = "replay_test.out";
			Utils.ScreenHelper.Instance.DownloadedGameGuid = Guid.Parse(_replaysList.Items[_replaysList.SelectedItems[0]]);
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
			_isShowedAgain = false;
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
