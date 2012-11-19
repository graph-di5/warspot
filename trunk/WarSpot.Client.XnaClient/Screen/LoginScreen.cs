using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using InputControl = WarSpot.Client.XnaClient.Input.InputControl;
using WarSpot.Contracts.Service;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class LoginScreen : GameScreen
	{
		private static Texture2D _texture;

		private LabelControl _loginLabel;
		private LabelControl _passwordLabel;

		private InputControl _loginBox;
		private InputControl _passwordBox;

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

		private void CreateControls()
		{
			_loginBox = new InputControl
											{
												Name = "login",
												IsHidden = false,
												Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
												Text = ""
											};

			_passwordBox = new InputControl
											{
												Name = "password",
												IsHidden = true,
												Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
												RealText = "",
												Text = InputControl.HiddenText("")
											};

			_loginLabel = new LabelControl("Username")
							{
								Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, -70), 100, 30)
							};

			_passwordLabel = new LabelControl("Password")
								{
									Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, 0), 100, 30)
								};

			_loginButton = new ButtonControl
							{
								Text = "Login",
								Bounds = new UniRectangle(new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32)
							};

			_exitButton = new ButtonControl
							{
								Text = "Exit",
								Bounds = new UniRectangle(new UniScalar(0.5f, -210), new UniScalar(0.4f, 70), 100, 32),
							};

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
			Desktop.Children.Add(_exitButton);
			Desktop.Children.Add(_newAccountButton);
			Desktop.Children.Add(_loginButton);

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
			// todo check this TEMPORARY
			if (_loginBox.Text == "" && _passwordBox.Text == "")
			{
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
				return;
			}

			ErrorCode errorCode = Network.ConnectionManager.Instance.Login(_loginBox.RealText, _passwordBox.RealText);
			if (errorCode.Type != ErrorType.Ok)
			{
				MessageBox.Show("Incorrect login or password!", ScreenManager.ScreenEnum.LoginScreen);
			}
			else
			{
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
			}
		}

		private void NewAccountButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.NewAccountScreen);
		}
	}
}
