using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using InputControl = WarSpot.Client.XnaClient.Input.InputControl;
using WarSpot.Contracts.Service;
using WarSpot.Client.XnaClient.Network;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class LoginScreen : GameScreen
	{
		private static Texture2D _texture;

		private LabelControl _loginLabel;
		private LabelControl _passwordLabel;
		private LabelControl _rememberLabel;

		private InputControl _loginBox;
		private InputControl _passwordBox;

		private OptionControl _rememberCheckBox;

		private ButtonControl _exitButton;
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
			SpriteBatch.Draw(_texture, WarSpotGame.Instance.GetScreenBounds(), Color.White);
			SpriteBatch.End();
		}

		public override void OnShow()
		{
			base.OnShow();
			if (Settings.Default.login != "" && Settings.Default.password != "")
			{
				_loginBox.RealText = Settings.Default.login;
				_loginBox.Text = Settings.Default.login;
				_passwordBox.RealText = Settings.Default.password;
				_passwordBox.Text = new String('*', Settings.Default.password.Length);
			}
		}

		private void CreateControls()
		{
			_loginBox = new InputControl
			{
				Name = "login",
				IsHidden = false,
				Bounds = new UniRectangle(
							new UniScalar(0.5f, -100f),
							new UniScalar(0.4f, -30), 200, 30),
				Text = ""
			};

			_passwordBox = new InputControl
			{
				Name = "password",
				IsHidden = true,
				Bounds = new UniRectangle(
							new UniScalar(0.5f, -100f), 
							new UniScalar(0.4f, 30), 
							new UniScalar(0f, 200),
							new UniScalar(0f, 30)),
				RealText = "",
				Text = InputControl.HiddenText("")
			};

			_rememberCheckBox = new OptionControl()
			{
				Bounds = new UniRectangle(
							new UniScalar(0.5f, 60),
							new UniScalar(0.4f, 70),
							new UniScalar(0f, 20),
							new UniScalar(0f, 20))
			};

			_loginLabel = new LabelControl("Username")
			{
				Bounds = new UniRectangle(
							new UniScalar(0.5f, -32), 
							new UniScalar(0.4f, -70),
							new UniScalar(0f, 100),
							new UniScalar(0f, 30))
			};

			_passwordLabel = new LabelControl("Password")
			{
				Bounds = new UniRectangle(
							new UniScalar(0.5f, -32),
							new UniScalar(0.4f, 0),
							new UniScalar(0f, 100),
							new UniScalar(0f, 30))
			};

			_rememberLabel = new LabelControl("Remember me:")
			{
				Bounds = new UniRectangle(
							new UniScalar(0.5f, -50),
							new UniScalar(0.4f, 65),
							new UniScalar(0f, 100),
							new UniScalar(0f, 30))
			};

			_loginButton = new ButtonControl
			{
				Text = "Login",
				Bounds = new UniRectangle(
							new UniScalar(0.5f, 110), 
							new UniScalar(0.4f, 100),
							new UniScalar(0f, 100),
							new UniScalar(0f, 32))
			};

			_exitButton = new ButtonControl
			{
				Text = "Exit",
				Bounds = new UniRectangle(
							new UniScalar(0.5f, -210),
							new UniScalar(0.4f, 100),
							new UniScalar(0f, 100),
							new UniScalar(0f, 32)),
			};

			_newAccountButton = new ButtonControl
			{
				Text = "Create new account",
				Bounds = new UniRectangle(
							new UniScalar(0.5f, -75),
							new UniScalar(0.4f, 100),
							new UniScalar(0f, 150),
							new UniScalar(0f, 32))
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_loginBox);
			Desktop.Children.Add(_passwordBox);

			Desktop.Children.Add(_loginLabel);
			Desktop.Children.Add(_passwordLabel);
			Desktop.Children.Add(_rememberLabel);

			Desktop.Children.Add(_exitButton);
			Desktop.Children.Add(_newAccountButton);
			Desktop.Children.Add(_loginButton);

			Desktop.Children.Add(_rememberCheckBox);

			ScreenManager.Instance.Controller.AddListener(_loginButton, LoginButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_exitButton, ExitButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_newAccountButton, NewAccountButtonPressed);
		}

		private void ExitButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.Game.Exit();
		}

		private void LoginButtonPressed(object sender, EventArgs args)
		{
			// TODO: Delete it after appearance test data in SQL
			if (_loginBox.Text == "" && _passwordBox.Text == "")
			{
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
				return;
			}

			if (_rememberCheckBox.Selected)
			{
				Settings.Default.login = _loginBox.RealText;
				Settings.Default.password = _passwordBox.RealText;
				Settings.Default.Save();
			}

			ErrorCode errorCode = ConnectionManager.Instance.Login(_loginBox.RealText, _passwordBox.RealText);
			if (errorCode.Type == ErrorType.WrongLoginOrPassword)
			{
				MessageBox.Show("Incorrect login or password!", ScreenManager.ScreenEnum.LoginScreen);
			}
			else
			{
				// Temporary
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
			}
		}

		private void NewAccountButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.NewAccountScreen);
		}
	}
}
