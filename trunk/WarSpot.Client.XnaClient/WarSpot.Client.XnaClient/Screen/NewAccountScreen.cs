using System;
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
			SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);
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
        }

        private void InitializeControls()
        {
            Desktop.Children.Add(_nameLabel);
            Desktop.Children.Add(_surNameLabel);

            Desktop.Children.Add(_nameBox);
            Desktop.Children.Add(_surNameBox);
        }
    }
}
