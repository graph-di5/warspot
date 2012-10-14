﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using InputControl = WarSpot.Client.XnaClient.Input.InputControl;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class LoginScreen : GameScreen
	{
		private static Texture2D _texture;

		private LabelControl _loginLabel;
		private LabelControl _passwordLabel;

		private InputControl _loginBox;
		private InputControl _passwordBox;

		private ButtonControl _backButton;
		private ButtonControl _loginButton;
		private ButtonControl _newAccountButton;

		public LoginScreen()
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
			SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			SpriteBatch.End();
		}

		private void CreateControls()
		{
			// Login Input
            _loginBox = new InputControl
                            {
                                IsHidden = false,
                                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
                                //Text = Settings.Default.login
                                Text = ""
                            };

			// Password Input
            _passwordBox = new InputControl
                            {
                                IsHidden = true,
                                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
                                //RealText = Settings.Default.password,
                                //Text = InputControl.HiddenText(Settings.Default.password)
                                RealText = "",
                                Text = InputControl.HiddenText("")
                            };

			// Login Label
			_loginLabel = new LabelControl("Username")
							{
								Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, -70), 100, 30)
							};

			// Password Label
			_passwordLabel = new LabelControl("Password")
								{
									Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, 0), 100, 30)
								};

			// Login Button
			_loginButton = new ButtonControl
							{
								Text = "Login",
								Bounds = new UniRectangle(new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32)
							};

			// Back Button
			_backButton = new ButtonControl
							{
								Text = "Back",
								Bounds = new UniRectangle(new UniScalar(0.5f, -210f), new UniScalar(0.4f, 70), 100, 32),
							};

			// New Account Button
			_newAccountButton = new ButtonControl
									{
										Text = "Create new account",
										Bounds = new UniRectangle(new UniScalar(0.5f, -75f), new UniScalar(0.4f, 70), 150, 32)
									};
		}

		private void InitializeControls()
		{
            Desktop.Children.Add(_loginBox);
            Desktop.Children.Add(_passwordBox);
			Desktop.Children.Add(_loginLabel);
			Desktop.Children.Add(_passwordLabel);
			Desktop.Children.Add(_backButton);
			Desktop.Children.Add(_newAccountButton);
			Desktop.Children.Add(_loginButton);

			ScreenManager.Instance.Controller.AddListener(_loginButton, LoginButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_newAccountButton, NewAccountButtonPressed);
		}

		private void BackButtonPressed(object sender, EventArgs args)
		{
            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
		}

		private void LoginButtonPressed(object sender, EventArgs args)
		{
		}

		private void NewAccountButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.NewAccountScreen);
		}
    }
}
