﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using System.IO;

namespace WarSpot.Client.XnaClient.Screen
{
	// TODO: filling listbox and deleting replays
	internal class SelectReplayScreen : GameScreen
	{
		private static Texture2D _texture;

		private ButtonControl _watchButton;
		private ButtonControl _deleteButton;

		private LabelControl _replayLabel;

		private ListControl _replaysBox;

		public SelectReplayScreen()
		{
			CreateControls();
			InitializeControls();
		}

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/Screens/screen_02");
		}



		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, WarSpotGame.Instance.GetScreenBounds(), Color.White);
			SpriteBatch.End();
		}

		private void CreateControls()
		{
			_replaysBox = new ListControl
			{
				SelectionMode = ListSelectionMode.Single,
				Bounds =
					new UniRectangle(
						new UniScalar(0f, 0),
						new UniScalar(0f, 20),
						new UniScalar(0f, 300),
						new UniScalar(0f, 300))
			};

			_replaysBox.Slider.Bounds.Location.X.Offset -= 1.0f;
			_replaysBox.Slider.Bounds.Location.Y.Offset += 1.0f;
			_replaysBox.Slider.Bounds.Size.Y.Offset -= 2.0f;
			_replaysBox.SelectionMode = Nuclex.UserInterface.Controls.Desktop.ListSelectionMode.Single;
			_replaysBox.SelectedItems.Add(0);

			_replayLabel = new LabelControl("Select replay:")
			{
				Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), 0, 0)
			};

			_watchButton = new ButtonControl
			{
				Text = "Watch",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, 300 + 10),
								new UniScalar(0f, 20),
								new UniScalar(0f, 100),
								new UniScalar(0f, 30))
			};

			_deleteButton = new ButtonControl
			{
				Text = "Delete",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, 300 + 10),
								new UniScalar(0.1f, 0),
								new UniScalar(0f, 100),
								new UniScalar(0f, 30))
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_deleteButton);
			Desktop.Children.Add(_watchButton);

			Desktop.Children.Add(_replayLabel);

			Desktop.Children.Add(_replaysBox);

			ScreenManager.Instance.Controller.AddListener(_watchButton, watchButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_deleteButton, deleteButtonPressed);

			updataReplaysList();
		}

		private void watchButtonPressed(object sender, EventArgs e)
		{
			//string selectedReplay = _replaysBox.Items[_replaysBox.SelectedItems[0]];
			//string path = Path.Combine(FoldersHelper.FoldersHelper.GetReplayPath(), selectedReplay);
			//Utils.ScreenHelper.Instance.ReplayPath = path;
			ScreenManager.Instance.PreapareReplayScreen();
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.WatchReplayScreen);
		}

		private void deleteButtonPressed(object sender, EventArgs e)
		{
			// TODO: test this
			try
			{
				string forDelete = _replaysBox.Items[_replaysBox.SelectedItems[0]];
				File.Delete(Path.Combine(FoldersHelper.FoldersHelper.GetReplayPath(), forDelete));
				updataReplaysList();
			}
			catch
			{
				MessageBox.Show("Select replay!", ScreenManager.ScreenEnum.WatchReplayScreen);
			}

		}


		private void updataReplaysList()
		{
			// TODO: test this
			DirectoryInfo tmp = new DirectoryInfo(FoldersHelper.FoldersHelper.GetReplayPath());
			var replays = tmp.GetFiles();
			foreach (var replay in replays)
			{
				_replaysBox.Items.Add(replay.Name);
			}
			

		}
	}
}