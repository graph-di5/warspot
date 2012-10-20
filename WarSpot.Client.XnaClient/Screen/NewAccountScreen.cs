﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using InputControl = WarSpot.Client.XnaClient.Input.InputControl;

namespace WarSpot.Client.XnaClient.Screen
{
    internal class NewAccountScreen :  GameScreen
    {
        private static Texture2D _texture;

        private LabelControl _nameLabel;
        private LabelControl _surNameLabel;
        private LabelControl _emailLabel;
        private LabelControl _nicknameLabel;
        private LabelControl _passwordLabel;

        private InputControl _nameBox;
        private InputControl _surNameBox;
        private InputControl _emailBox;
        private InputControl _nicknameBox;
        private InputControl _passwordBox;

        private ButtonControl _registerButton;
        private ButtonControl _backButton;

        public NewAccountScreen()
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
            _nameLabel = new LabelControl("Enter your name:")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100), new UniScalar(0.1f, -70), 100, 30)
            };

            _surNameLabel = new LabelControl("Enter your surname:")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100), new UniScalar(0.25f, -70), 100, 30)
            };

            _emailLabel = new LabelControl("Enter your e-mail:")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100), new UniScalar(0.4f, -70), 100, 30)
            };

            _nicknameLabel = new LabelControl("Enter your nickname:")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100), new UniScalar(0.55f, -70), 100, 30)
            };
            
            _passwordLabel = new LabelControl("Enter your password:")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100), new UniScalar(0.7f, -70), 100, 30)
            };


            _nameBox = new InputControl
            {
                IsHidden = false,
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.1f, -45), 200, 30),
                Text = ""
            };

            _surNameBox = new InputControl
            {
                IsHidden = false,
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.25f, -45), 200, 30),
                Text = ""
            };

            _emailBox = new InputControl
            {
                IsHidden = false,
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -45), 200, 30),
                Text = ""
            };

            _nicknameBox = new InputControl
            {
                IsHidden = false,
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.55f, -45), 200, 30),
                Text = ""
            };

            _passwordBox = new InputControl
            {
                IsHidden = true,
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.7f, -45), 200, 30),
                Text = ""
            };

            _backButton = new ButtonControl
            {
                Text = "Main Menu",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.35f, -5),
                        new UniScalar(0.8f, 0),
                        new UniScalar(0.3f, 10),
                        new UniScalar(0.07f, 0)),
            };

            _registerButton = new ButtonControl
            {
                Text = "Register",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.35f, -5),
                        new UniScalar(0.7f, 0),
                        new UniScalar(0.3f, 10),
                        new UniScalar(0.07f, 0)),
            };
        }

        private void InitializeControls()
        {
            Desktop.Children.Add(_nameLabel);
            Desktop.Children.Add(_surNameLabel);
            Desktop.Children.Add(_emailLabel);
            Desktop.Children.Add(_nicknameLabel);
            Desktop.Children.Add(_passwordLabel);

            Desktop.Children.Add(_nameBox);
            Desktop.Children.Add(_surNameBox);
            Desktop.Children.Add(_emailBox);
            Desktop.Children.Add(_nicknameBox);
            Desktop.Children.Add(_passwordBox);

            Desktop.Children.Add(_backButton);
            Desktop.Children.Add(_registerButton);

			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
        }

		private void BackButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
		}
    }
}
