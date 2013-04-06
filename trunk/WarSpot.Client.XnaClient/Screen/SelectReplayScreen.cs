using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using System.IO;
using WarSpot.Client.XnaClient.Screen.Utils;
using WarSpot.Client.XnaClient.Network;
using System.Linq;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class SelectReplayScreen : GameScreen
	{
		private static Texture2D _texture;
		private ButtonControl _watchButton;
		private ButtonControl _backButton;
		private LabelControl _replayLabel;
		private ListControl _replaysList;
        private OptionControl _saveReplayCheckBox;
        private LabelControl _saveReplayLabel;

		public SelectReplayScreen()
		{
			CreateControls();
			InitializeControls();
		}

        public override void OnShow()
        {
            base.OnShow();
            if (ConnectionManager.Instance.IsOnline())
            {
                var x = ConnectionManager.Instance.GetListOfGames();
                if (x.Type == Contracts.Service.ErrorType.Ok)
                {
                    foreach (var r in ScreenHelper.Instance.ListOfReplays)
                    {
                        _replaysList.Items.Add(r.Name);
                    }
                }
                else
                    GetSavedReplays();
            }
            else
                GetSavedReplays();
        }

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/Screens/screen_02");
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, WarSpotGame.Instance.GetScreenBounds(), Color.White);
			SpriteBatch.End();
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

			_backButton = new ButtonControl
			{
				Text = "Back",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, 0),
								new UniScalar(0.15f, 280),
								new UniScalar(0f, 100),
								new UniScalar(0f, 30))
			};

            _saveReplayCheckBox = new OptionControl()
            {
                Bounds = new UniRectangle(
                            new UniScalar(0f, 210),
                                new UniScalar(0.1f, 275),
                                new UniScalar(0f, 20),
                                new UniScalar(0f, 20))
            };

            _saveReplayLabel = new LabelControl()
            {
                Text = "Save replay in My Documents: ",
                Bounds = new UniRectangle(
                            new UniScalar(0f, 0),
                                new UniScalar(0.1f, 275),
                                new UniScalar(0f, 20),
                                new UniScalar(0f, 20))
            };
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_watchButton);
			Desktop.Children.Add(_backButton);
			Desktop.Children.Add(_replayLabel);
			Desktop.Children.Add(_replaysList);
            Desktop.Children.Add(_saveReplayCheckBox);
            Desktop.Children.Add(_saveReplayLabel);

			ScreenManager.Instance.Controller.AddListener(_watchButton, WatchButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
		}

		private void WatchButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoadingScreen);
		}


		private void BackButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
		}

		private void GetSavedReplays()
		{
            string[] replays = Directory.GetFiles(FoldersHelper.GetReplayPath()).Select(path => Path.GetFileName(path)).ToArray();
			foreach (var i in replays)
			{
				_replaysList.Items.Add(i);
			}
		}
	}
}
