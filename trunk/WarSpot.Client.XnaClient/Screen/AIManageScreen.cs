using System;
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
		private ButtonControl _backButton;

        private LabelControl _pathLabel;

        private InputControl _pathBox;

        private Nuclex.UserInterface.Controls.Desktop.ListControl _intellectsList;

        private OpenFileDialog _pathDialog = new OpenFileDialog();

        public AIManageScreen()
        {
            CreateControls();
            InitializeControls();
        }

        public override void LoadContent()
        {
            _texture = ContentManager.Load<Texture2D>("Textures/Screens/screen_02");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, WarSpotGame.Instance.GetScreenBounds(), Color.White);
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
                        new UniScalar(0.07f, -2))
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
			_backButton = new ButtonControl
			{
				Text = "Main Menu",
				Bounds = 
					new UniRectangle(
						new UniScalar(0.01f, -75),
						new UniScalar(0.18f, 300),
						new UniScalar(0.2f, 0),
						new UniScalar(0.07f, 0)),
			};


            _intellectsList = new Nuclex.UserInterface.Controls.Desktop.ListControl
            {
                Bounds = new UniRectangle(-70f, 70.0f, 250.0f, 300.0f),
            };
            // Странноватый у этой штуки конструктор, ничего не разрешает    

            /* Правильный вариант:
            List<string> tmp = new List<string>();
            tmp.AddRange(WarSpot.Client.XnaClient.AIManager.IntellectStorageController.GetIntellectsNames());
            foreach (var intellect in tmp)
            {
                _intellectsList.Items.Add(intellect);
            }*/

            _intellectsList.Items.Add("Zerling Rush");
            _intellectsList.Items.Add("Photon them all");
            _intellectsList.Items.Add("Fast reapers");

            _intellectsList.Slider.Bounds.Location.X.Offset -= 1.0f;
            _intellectsList.Slider.Bounds.Location.Y.Offset += 1.0f;
            _intellectsList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            _intellectsList.SelectionMode = Nuclex.UserInterface.Controls.Desktop.ListSelectionMode.Single;
            _intellectsList.SelectedItems.Add(0);
        }

        private void InitializeControls()
        {
            Desktop.Children.Add(_pathLabel);

            Desktop.Children.Add(_pathBox);

			Desktop.Children.Add(_browseButton);
			Desktop.Children.Add(_addAIButton);
			Desktop.Children.Add(_backButton);

            Desktop.Children.Add(_intellectsList);

            ScreenManager.Instance.Controller.AddListener(_browseButton, browseButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_addAIButton, addAIButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
        }


        private void browseButtonPressed(object sender, EventArgs e)
        {
            DialogResult dialog = _pathDialog.ShowDialog();
            _pathBox.Text = _pathDialog.FileName;
        }


        private void addAIButtonPressed(object sender, EventArgs e)
        {
            // Проверить, что это .dll, да еще и подходящая под условия, а потом добавить в список интеллектов (или вызвать апдейт?)
        }

		private void BackButtonPressed(object sender, EventArgs args)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
		}
                
    }
}
