using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using System.Globalization;

namespace WarSpot.Client.XnaClient.Screen
{
    internal class OptionsScreen : GameScreen
    {
        private static Texture2D _texture;

        private LabelControl _titleLabel;
        private LabelControl _volumeLabel;
        private LabelControl _fullscreenLabel;

        private OptionControl _fullscreenButton;

        private ButtonControl _backButton;
        private ButtonControl _upVolume;
        private ButtonControl _downVolume;

        private LabelControl _volumeValueLabel;

        public OptionsScreen()
		{
			CreateControls();
			InitializeControls();
		}

        public override void LoadContent()
        {
            _texture = ContentManager.Load<Texture2D>("Textures/screens/mainMenuScreen");
        }

        private void CreateControls()
        {
            _titleLabel = new LabelControl("Options")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -SpriteFont.MeasureString("Options").X / 4), 
					0, 0, 0)
            };

            _fullscreenLabel = new LabelControl("FullScreen: ")
            {
                Bounds =
					new UniRectangle(new UniScalar(0.5f, -SpriteFont.MeasureString("FullScreen: ").X / 4 - 10), 
						new UniScalar(0.1f, 0), 0, 20)
            };

            _fullscreenButton = new OptionControl
            {
                Bounds =
					new UniRectangle(new UniScalar(0.5f, SpriteFont.MeasureString("FullScreen: ").X / 4 - 10), 
						new UniScalar(0.1f, 0), 20, 20)
            };

            _backButton = new ButtonControl
            {
                Text = "Back",
                Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(1f, -50), 100, 30)
            };

            _upVolume = new ButtonControl
            {
                Text = "+",
                Bounds = new UniRectangle(new UniScalar(0.5f, -25), new UniScalar(0.25f, 20), 50, 30)
            };

            _downVolume = new ButtonControl
            {
                Text = "-",
                Bounds = new UniRectangle(new UniScalar(0.5f, -25), new UniScalar(0.25f, -20), 50, 30)
            };

            _volumeLabel = new LabelControl("Volume:")
            {
                Bounds =
					new UniRectangle(new UniScalar(0.5f, -SpriteFont.MeasureString("Volume:").X / 2 - 35),
						new UniScalar(0.25f, 0), 0, 30)
            };

            _volumeValueLabel = new LabelControl
            {
                Text = (100 * Math.Round(Settings.Default.Volume, 1)).ToString(CultureInfo.InvariantCulture) + "%",
				Bounds = 
				new UniRectangle(new UniScalar(0.5f, 35), new UniScalar(0.25f, 0), 0, 30)
            };
        }

        private void InitializeControls()
        {
            Desktop.Children.Add(_titleLabel);
            Desktop.Children.Add(_fullscreenLabel);
            Desktop.Children.Add(_fullscreenButton);
            Desktop.Children.Add(_backButton);
            Desktop.Children.Add(_upVolume);
            Desktop.Children.Add(_downVolume);
            Desktop.Children.Add(_volumeLabel);
            Desktop.Children.Add(_volumeValueLabel);

            _fullscreenButton.Selected = Settings.Default.FullScreenSelected;

            _fullscreenButton.Changed += FullScreenSelected;
            _upVolume.Pressed += UpButtonPressed;
            _downVolume.Pressed += DownButtonPressed;

            ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_upVolume, UpButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_downVolume, DownButtonPressed);
        }

        private void BackButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
        }

        private void UpButtonPressed(object sender, EventArgs e)
        {
            Settings.Default.Volume = MathHelper.Clamp(Settings.Default.Volume + 0.05f, 0.0f, 1.0f);
            Settings.Default.Save();
            _volumeValueLabel.Text = (100 * Math.Round(Settings.Default.Volume, 1)).ToString(CultureInfo.InvariantCulture) + "%";
        }

        private void DownButtonPressed(object sender, EventArgs e)
        {
            Settings.Default.Volume = MathHelper.Clamp(Settings.Default.Volume - 0.05f, 0.0f, 1.0f);
            Settings.Default.Save();
            _volumeValueLabel.Text = (100 * Math.Round(Settings.Default.Volume, 1)).ToString(CultureInfo.InvariantCulture) + "%";
        }

        private void FullScreenSelected(object sender, EventArgs e)
        {
            Settings.Default.FullScreenSelected = _fullscreenButton.Selected;
            Settings.Default.Save();

			if (WarSpotGame.Instance.IsFullScreen != _fullscreenButton.Selected)
			{
				WarSpotGame.Instance.ToggleFullScreen();
			}
        }

		public override void OnShow()
		{
			base.OnShow();
			_fullscreenButton.Selected = WarSpotGame.Instance.IsFullScreen;
		}

    }
}
