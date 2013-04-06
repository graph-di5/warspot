using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using InputControl = WarSpot.Client.XnaClient.Input.InputControl;
using WarSpot.Contracts.Service;
using WarSpot.Client.XnaClient.Screen;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class RegistrationScreen : GameScreen
	{
		private static Texture2D _texture;

		private LabelControl _nameLabel;
		private LabelControl _surnameLabel;
		private LabelControl _emailLabel;
        private LabelControl _universityLabel;
        private LabelControl _yearLabel;
		private LabelControl _nicknameLabel;
		private LabelControl _passwordLabel;

		private InputControl _nameBox;
		private InputControl _surnameBox;
		private InputControl _emailBox;
        private InputControl _universityBox;
        private InputControl _yearBox;
		private InputControl _nicknameBox;
		private InputControl _passwordBox;

		private ButtonControl _registerButton;
		private ButtonControl _backButton;


		public RegistrationScreen()
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
			_nameLabel = new LabelControl("Name:")
			{
				Bounds = new UniRectangle(
							new UniScalar(0.5f, -100),
							new UniScalar(0.1f, -70), 
                            new UniScalar(0.0f, 100),
                            new UniScalar(0.0f, 30))
			};

			_nameBox = new InputControl
			{
				Name = "name",
				IsHidden = false,
				Bounds = new UniRectangle(
							new UniScalar(0.5f, -100),
							new UniScalar(0.1f, -45), 
                            new UniScalar(0.0f, 200),
                            new UniScalar(0.0f, 30)),
				Text = ""
			};

            _surnameLabel = new LabelControl("Surname:")
            {
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, -10),
                            new UniScalar(0.0f, 100),
                            new UniScalar(0.0f, 30))
            };

            _surnameBox = new InputControl
            {
                Name = "surname",
                IsHidden = false,
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 15),
                            new UniScalar(0.0f, 200),
                            new UniScalar(0.0f, 30)),
                Text = ""
            };

            _emailLabel = new LabelControl("E-mail:")
            {
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 50),
                            new UniScalar(0.0f, 100),
                            new UniScalar(0.0f, 30))
            };


            _emailBox = new InputControl
            {
                Name = "email",
                IsHidden = false,
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 75),
                            new UniScalar(0.0f, 200),
                            new UniScalar(0.0f, 30)),
                Text = ""
            };

            _universityLabel = new LabelControl("University:")
            {
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 110),
                            new UniScalar(0.0f, 100),
                            new UniScalar(0.0f, 30))
            };

            _universityBox = new InputControl
            {
                Name = "university",
                IsHidden = false,
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 135),
                            new UniScalar(0.0f, 200),
                            new UniScalar(0.0f, 30)),
                Text = ""
            };

            _yearLabel = new LabelControl("Year of study:")
            {
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 170),
                            new UniScalar(0.0f, 100),
                            new UniScalar(0.0f, 30))
            };

            _yearBox = new InputControl
            {
                Name = "year",
                IsHidden = false,
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 195),
                            new UniScalar(0.0f, 200),
                            new UniScalar(0.0f, 30)),
                Text = ""
            };

            _nicknameLabel = new LabelControl("Nickname:")
            {
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 230),
                            new UniScalar(0.0f, 100),
                            new UniScalar(0.0f, 30))
            };

            _nicknameBox = new InputControl
            {
                Name = "nickname",
                IsHidden = false,
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 255),
                            new UniScalar(0.0f, 200),
                            new UniScalar(0.0f, 30)),
                Text = ""
            };

            _passwordLabel = new LabelControl("Password:")
            {
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 290),
                            new UniScalar(0.0f, 100),
                            new UniScalar(0.0f, 30))
            };

            _passwordBox = new InputControl
            {
                Name = "password",
                IsHidden = false,
                Bounds = new UniRectangle(
                            new UniScalar(0.5f, -100),
                            new UniScalar(0.1f, 315),
                            new UniScalar(0.0f, 200),
                            new UniScalar(0.0f, 30)),
                Text = ""
            };

            _backButton = new ButtonControl
            {
                Text = "Back",
                Bounds =
                        new UniRectangle(
                            new UniScalar(0.35f, 0),
                            new UniScalar(0.8f, 25),
                            new UniScalar(0.3f, 0),
                            new UniScalar(0.07f, 0)),
            };

			_registerButton = new ButtonControl
			{
				Text = "Register",
				Bounds =
						new UniRectangle(
							new UniScalar(0.35f, 0),
							new UniScalar(0.7f, 25),
							new UniScalar(0.3f, 0),
							new UniScalar(0.07f, 0)),
			};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_nameLabel);
			Desktop.Children.Add(_surnameLabel);
			Desktop.Children.Add(_emailLabel);
			Desktop.Children.Add(_universityLabel);
			Desktop.Children.Add(_yearLabel);
            Desktop.Children.Add(_nicknameLabel);
            Desktop.Children.Add(_passwordLabel);

			Desktop.Children.Add(_nameBox);
			Desktop.Children.Add(_surnameBox);
			Desktop.Children.Add(_emailBox);
            Desktop.Children.Add(_universityBox);
            Desktop.Children.Add(_yearBox);
            Desktop.Children.Add(_nicknameBox);
            Desktop.Children.Add(_passwordBox);

			Desktop.Children.Add(_backButton);
			Desktop.Children.Add(_registerButton);

			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_registerButton, RegisterButtonPressed);
		}

		private void RegisterButtonPressed(object sender, EventArgs args)
		{
			bool checker = InputValidator.CheckText(_nameBox.RealText) &&
				InputValidator.CheckText(_surnameBox.RealText) &&
				InputValidator.CheckEmail(_emailBox.RealText) &&
				InputValidator.CheckText(_nicknameBox.RealText) &&
				InputValidator.CheckText(_passwordBox.RealText) &&
                InputValidator.CheckText(_universityBox.RealText) &&
                InputValidator.isNumber(_yearBox.RealText);
			if (!checker)
			{
				MessageBox.Show("Incorrect input!", ScreenManager.ScreenEnum.RegistrationScreen);
			}
			else
			{

				ErrorCode errorCode = Network.ConnectionManager.Instance.Register(_nicknameBox.RealText,
					_passwordBox.RealText, _nameBox.RealText, _surnameBox.RealText, 
					_universityBox.RealText, Convert.ToInt32(_yearBox.Text), _emailBox.RealText);
				if (errorCode.Type != ErrorType.Ok)
				{
					MessageBox.Show("Registration failed",
					ScreenManager.ScreenEnum.RegistrationScreen);
				}
				else
				{
					MessageBox.Show("Registered successfully", ScreenManager.ScreenEnum.LoginScreen);
				}
			}
		}

		private void BackButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
		}
	}
}
