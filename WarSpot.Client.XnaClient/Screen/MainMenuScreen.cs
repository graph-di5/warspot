﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace WarSpot.Client.XnaClient.Screen
{
    internal class MainMenuScreen : GameScreen
    {
        private static Texture2D _texture;
        
        private ButtonControl _LocalAIButton;
        private ButtonControl _logOutButton;
        private ButtonControl _optionsButton;
        private ButtonControl _exitButton;

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
            _LocalAIButton = new ButtonControl
            {
                Text = "AI Manage",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.30f, 0),
                        new UniScalar(0.1f, 0),
                        new UniScalar(0.4f, 0),
                        new UniScalar(0.1f, 0)),
            };

            _optionsButton = new ButtonControl
            {
                Text = "Options",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.30f, 0),
                        new UniScalar(0.25f, 0),
                        new UniScalar(0.4f, 0),
                        new UniScalar(0.1f, 0)),
			};

			_logOutButton = new ButtonControl
			{
				Text = "Log Out",
				Bounds =
					new UniRectangle(
						new UniScalar(0.30f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.4f, 0),
						new UniScalar(0.1f, 0)),
			};
        
            _exitButton = new ButtonControl
            {
                Text = "Exit",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.30f, 0),
                        new UniScalar(0.55f, 0),
                        new UniScalar(0.4f, 0),
                        new UniScalar(0.1f, 0)),
            };
        }

        private void InitializeControls()
        {
            // tmp
            Desktop.Children.Add(_LocalAIButton);
            ScreenManager.Instance.Controller.AddListener(_LocalAIButton, localAIButtonPressed);

            Desktop.Children.Add(_logOutButton);
            Desktop.Children.Add(_optionsButton);
            Desktop.Children.Add(_exitButton);

            ScreenManager.Instance.Controller.AddListener(_logOutButton, logOutButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_optionsButton, optionsButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_exitButton, exitButtonPressed);
        }

        private void localAIButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.AIManageScreen);
        }

        private void logOutButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
        }

        private void optionsButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.OptionsScreen);
        }

        private void exitButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.Game.Exit();
        }

    }
}
