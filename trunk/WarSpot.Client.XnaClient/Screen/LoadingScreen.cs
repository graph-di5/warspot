using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSpot.Client.XnaClient.Screen.Utils;
using WarSpot.Contracts.Service;
using WarSpot.MatchComputer;
using System.IO;
using System.Linq;

namespace WarSpot.Client.XnaClient.Screen
{
	class LoadingScreen : GameScreen
	{
		private bool _isReplayDeserialized = false;
		private bool _isReplayDownloaded = false;
		private Texture2D _texture;
		bool isCorrect = true;

		public LoadingScreen()
		{

		}

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/Screens/loadingScreen");
			base.LoadContent();
		}

		// Check if replay is deserialized and set watching this replay as active screen 
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			lock (this)
			{
				if (_isReplayDeserialized || _isReplayDownloaded)
				{
					if (isCorrect)
					{
						_isReplayDeserialized = false;
						ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.WatchReplayScreen);
						if (_isReplayDownloaded && ScreenHelper.Instance.SaveReplay)
							SaveReplay();
					}
					else
						MessageBox.Show("Wrong replay type", ScreenManager.ScreenEnum.SelectReplayScreen);
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, WarSpotGame.Instance.GetScreenBounds(), Color.White);
			SpriteBatch.End();
		}

		public override void OnShow()
		{
			base.OnShow();
			Thread replayHandler;
			if (ScreenHelper.Instance.OnlineReplayMode)
				replayHandler = new Thread(DownloadReplay);
			else
				replayHandler = new Thread(Deserialize);
			replayHandler.Start();
		}

		private void Deserialize()
		{
			string path = ScreenHelper.Instance.ReplayPath;
			try
			{
				ScreenHelper.Instance.ReplayEvents = SerializationHelper.Deserialize(path).Events;
				isCorrect = ScreenHelper.Instance.ReplayEvents.Count > 0;
			}
			catch
			{
				MessageBox.Show("Wrong replay type", ScreenManager.ScreenEnum.SelectReplayScreen);
			}
			lock (this)
			{
				_isReplayDeserialized = true;
			}
		}

		private void DownloadReplay()
		{
			try
			{
				Network.ConnectionManager.Instance.DownloadReplay(Utils.ScreenHelper.Instance.DownloadedGameGuid);
			}
			catch
			{
				MessageBox.Show("Wrong replay type", ScreenManager.ScreenEnum.LoginScreen);
			}
			isCorrect = ScreenHelper.Instance.ReplayEvents.Count > 0;
			lock (this)
			{
				_isReplayDownloaded = true;
			}
		}

		private void SaveReplay()
		{
			var x = (from rep in ScreenHelper.Instance.ListOfReplays where rep.ID == ScreenHelper.Instance.DownloadedGameGuid select rep.Name).First();
			using (var sr = new FileStream(Path.Combine(FoldersHelper.GetReplayPath(), x), FileMode.CreateNew))
			{
				SerializationHelper.Serialize(ScreenHelper.Instance.ToSerialize, sr);
			}
		}
	}
}
