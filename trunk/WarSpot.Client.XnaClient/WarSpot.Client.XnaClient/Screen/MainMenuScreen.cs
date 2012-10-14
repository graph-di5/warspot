using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace WarSpot.Client.XnaClient.Screen
{
    internal class MainMenuScreen : GameScreen
    {
        private static Texture2D _texture;
        private ButtonControl _logInButton;
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
            SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);
            SpriteBatch.End();
        }
        private void CreateControls()
        {
            _logInButton = new ButtonControl
            {
                Text = "Log In",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.30f, 0),
                        new UniScalar(0.2f, 0),
                        new UniScalar(0.4f, 0),
                        new UniScalar(0.1f, 0)),
            };

            _optionsButton = new ButtonControl
            {
                Text = "Options",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.30f, 0),
                        new UniScalar(0.35f, 0),
                        new UniScalar(0.4f, 0),
                        new UniScalar(0.1f, 0)),
            };

            _exitButton = new ButtonControl
            {
                Text = "Exit",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.30f, 0),
                        new UniScalar(0.5f, 0),
                        new UniScalar(0.4f, 0),
                        new UniScalar(0.1f, 0)),
            };
        }

        private void InitializeControls()
        {
            Desktop.Children.Add(_logInButton);
            Desktop.Children.Add(_optionsButton);
            Desktop.Children.Add(_exitButton);

            ScreenManager.Instance.Controller.AddListener(_logInButton, logInButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_optionsButton, optionsButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_exitButton, exitButtonPressed);
        }

        private void logInButtonPressed(object sender, EventArgs e)
        {
        }

        private void optionsButtonPressed(object sender, EventArgs e)
        {
        }

        private void exitButtonPressed(object sender, EventArgs e)
        {
        }
    }
}
