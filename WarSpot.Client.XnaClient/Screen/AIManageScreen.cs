using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using InputControl = WarSpot.Client.XnaClient.Input.InputControl;
using System.Windows.Forms;
using WarSpot.Client.XnaClient.AIManager;
using WarSpot.Contracts.Service;
using WarSpot.Client.XnaClient.Network;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class AIManageScreen : GameScreen
	{
		private Texture2D _texture;

		private ButtonControl _browseButton;
		private ButtonControl _uploadButton;
		private ButtonControl _backButton;

		private ButtonControl _refreshButton;
		private ButtonControl _deleteButton;

		private LabelControl _pathLabel;

		private InputControl _pathBox;

		private Nuclex.UserInterface.Controls.Desktop.ListControl _intellectList;

		private readonly OpenFileDialog _pathDialog;

		public AIManageScreen()
		{
			_pathDialog = new OpenFileDialog();
			CreateControls();
			InitializeControls();
			LoadIntellects();
		}

		private void LoadIntellects()
		{
			string[] intellects = ConnectionManager.Instance.GetListOfIntellects();
			_intellectList.Items.Clear();
			foreach (string i in intellects)
			{
				_intellectList.Items.Add(i);
			}
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
				Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), 0, 0)
			};

			_pathBox = new InputControl
			{
				Name = "path",
				IsHidden = false,
				Bounds = new UniRectangle(new UniScalar(0, 0), new UniScalar(0f, 20), 300, 30),
				Text = "",
				Enabled = false
			};

			_browseButton = new ButtonControl
			{
				Text = "Browse",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, 300 + 10),
								new UniScalar(0f, 20),
								new UniScalar(0f, 100),
								new UniScalar(0f, 30))
			};

			_uploadButton = new ButtonControl
			{
				Text = "Upload",
				Bounds =
						new UniRectangle(
								new UniScalar(0f, 0),
		new UniScalar(0f, 60),
		new UniScalar(0f, 100),
		new UniScalar(0f, 30))
			};
			_backButton = new ButtonControl
			{
				Text = "Main Menu",
				Bounds =
					new UniRectangle(
						new UniScalar(0f, 0),
						new UniScalar(0f, 410),
						new UniScalar(0f, 100),
						new UniScalar(0f, 30))
			};


			_intellectList = new Nuclex.UserInterface.Controls.Desktop.ListControl
			{
				SelectionMode = ListSelectionMode.Single,
				Bounds =
					new UniRectangle(
						new UniScalar(0f, 0),
						new UniScalar(0f, 100),
						new UniScalar(0f, 300),
						new UniScalar(0f, 300))
			};

			_refreshButton = new ButtonControl
			{
				Text = "Refresh",
				Bounds =
					new UniRectangle(
						new UniScalar(0f, 310),
						new UniScalar(0f, 100),
						new UniScalar(0f, 100),
						new UniScalar(0f, 30))
			};
			_deleteButton = new ButtonControl
			{
				Text = "Delete",
				Bounds =
					new UniRectangle(
						new UniScalar(0f, 310),
						new UniScalar(0f, 180),
						new UniScalar(0f, 100),
						new UniScalar(0f, 30))
			};

			// Странноватый у этой штуки конструктор, ничего не разрешает    

			/* Правильный вариант:
			List<string> tmp = new List<string>();
			tmp.AddRange(WarSpot.Client.XnaClient.AIManager.IntellectStorageController.GetIntellectsNames());
			foreach (var intellect in tmp)
			{
					_intellectsList.Items.Add(intellect);
			}*/

			_intellectList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_intellectList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_intellectList.Slider.Bounds.Size.Y.Offset -= 2.0f;
			_intellectList.SelectionMode = Nuclex.UserInterface.Controls.Desktop.ListSelectionMode.Single;
			_intellectList.SelectedItems.Add(0);
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_pathLabel);

			Desktop.Children.Add(_pathBox);

			Desktop.Children.Add(_browseButton);
			Desktop.Children.Add(_uploadButton);
			Desktop.Children.Add(_backButton);

			Desktop.Children.Add(_intellectList);

			Desktop.Children.Add(_refreshButton);
			Desktop.Children.Add(_deleteButton);

			ScreenManager.Instance.Controller.AddListener(_browseButton, browseButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_backButton, BackButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_refreshButton, refreshButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_uploadButton, uploadButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_deleteButton, deleteButtonPressed);
		}


		private void browseButtonPressed(object sender, EventArgs e)
		{
			DialogResult dialog = _pathDialog.ShowDialog();
			_pathBox.RealText = _pathDialog.FileName;
		}

		private void BackButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
		}

		private void refreshButtonPressed(object sender, EventArgs e)
		{
			LoadIntellects();
		}

		private void uploadButtonPressed(object sender, EventArgs e)
		{
			if (_pathBox.Text == "")
			{
				return;
			}
			Intellect i = new Intellect(_pathBox.Text);
			ErrorCode ec = Network.ConnectionManager.Instance.UploadIntellect(i);
			if (ec.Type == ErrorType.Ok)
			{
				_pathBox.Text = "";
				MessageBox.Show("Intellect " + i + " was uploaded successfully!", ScreenManager.ScreenEnum.AIManageScreen);
			}
			else
			{
				MessageBox.Show("Some error occured...", ScreenManager.ScreenEnum.AIManageScreen);
			}
			LoadIntellects();
		}

		private void deleteButtonPressed(object sender, EventArgs e)
		{
			if (_intellectList.Items.Count == 0)
			{
				return;
			}
			ConnectionManager.Instance.DeleteIntellect(_intellectList.Items[_intellectList.SelectedItems[0]]);
			LoadIntellects();
		}

	}
}
