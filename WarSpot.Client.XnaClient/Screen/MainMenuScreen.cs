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
        
        // tmp
        private ButtonControl _AIButton;

        private ButtonControl _logInButton;
        private ButtonControl _optionsButton;
        private ButtonControl _newAccountButton;
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
            _AIButton = new ButtonControl
            {
                Text = "AI Manage (tmp)",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.30f, 0),
                        new UniScalar(0.05f, 0),
                        new UniScalar(0.4f, 0),
                        new UniScalar(0.1f, 0)),
            };

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
                        new UniScalar(0.5f, 0),
                        new UniScalar(0.4f, 0),
                        new UniScalar(0.1f, 0)),
            };
            _newAccountButton = new ButtonControl
            {
                Text = "Registration",
                Bounds =
                    new UniRectangle(
                        //x
                        new UniScalar(0.30f, 0),
                        // y
                        new UniScalar(0.35f, 0),
                        // width
                        new UniScalar(0.4f, 0),
                        // height
                        new UniScalar(0.1f, 0)),
            };
        
            _exitButton = new ButtonControl
            {
                Text = "Exit",
                Bounds =
                    new UniRectangle(
                        //x
                        new UniScalar(0.30f, 0),
                        // y
                        new UniScalar(0.65f, 0),
                        // width
                        new UniScalar(0.4f, 0),
                        // height
                        new UniScalar(0.1f, 0)),
            };
        }

        private void InitializeControls()
        {
            // tmp
            Desktop.Children.Add(_AIButton);
            ScreenManager.Instance.Controller.AddListener(_AIButton, AIButtonPressed);

            Desktop.Children.Add(_logInButton);
            Desktop.Children.Add(_optionsButton);
            Desktop.Children.Add(_newAccountButton);
            Desktop.Children.Add(_exitButton);

            ScreenManager.Instance.Controller.AddListener(_logInButton, logInButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_optionsButton, optionsButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_newAccountButton, newAccountButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_exitButton, exitButtonPressed);
        }

        // tmp

        private void AIButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.AIManageScreen);
        }

        private void logInButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
        }

        private void optionsButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.OptionsScreen);
        }

        private void newAccountButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.NewAccountScreen);
        }

        private void exitButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Instance.Game.Exit();
        }

    }
}
