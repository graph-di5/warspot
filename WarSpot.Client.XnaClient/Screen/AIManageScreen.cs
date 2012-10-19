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
using System.Windows.Forms;

namespace WarSpot.Client.XnaClient.Screen
{
    internal class AIManageScreen : GameScreen
    {
        private Texture2D _texture;

        private ButtonControl _browseButton;
        private ButtonControl _addAIButton;

        private LabelControl _pathLabel;

        private InputControl _pathBox;

        private OpenFileDialog _pathDialog = new OpenFileDialog();

        public AIManageScreen()
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
            _pathLabel = new LabelControl("Select path for your .dll file:")
            {
                Bounds = new UniRectangle(new UniScalar(0.05f, -100), new UniScalar(0.05f, -70), 100, 30)
            };


            _pathBox = new InputControl
            {
                IsHidden = false,
                Bounds = new UniRectangle(new UniScalar(0.05f, -100f), new UniScalar(0.05f, -45), 300, 30),
                Text = "",
                Enabled = false
            };
            _browseButton = new ButtonControl
            {
                Text = "Browse",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.4f, 0),
                        new UniScalar(0.01f, -25),
                        new UniScalar(0.2f, 0),
                        new UniScalar(0.07f, -2)),
            };

           _addAIButton = new ButtonControl
            {
                Text = "Add",
                Bounds =
                    new UniRectangle(
                        new UniScalar(0.01f, -75),
                        new UniScalar(0.05f, 0),
                        new UniScalar(0.2f, 0),
                        new UniScalar(0.07f, -2)),
            };

           _pathDialog.FileName = "";
        }

        private void InitializeControls()
        {
            Desktop.Children.Add(_pathLabel);

            Desktop.Children.Add(_pathBox);

            Desktop.Children.Add(_browseButton);
            Desktop.Children.Add(_addAIButton);

            ScreenManager.Instance.Controller.AddListener(_browseButton, browseButtonPressed);
            ScreenManager.Instance.Controller.AddListener(_addAIButton, addAIButtonPressed);
        }


        private void browseButtonPressed(object sender, EventArgs e)
        {
            DialogResult dialog = _pathDialog.ShowDialog();
            _pathBox.Text = _pathDialog.FileName;
        }


        private void addAIButtonPressed(object sender, EventArgs e)
        {

        }
                
    }
}
