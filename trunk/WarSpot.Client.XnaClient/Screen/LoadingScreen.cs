using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSpot.Client.XnaClient.Screen.Utils;
using WarSpot.Contracts.Service;
using WarSpot.MatchComputer;


namespace WarSpot.Client.XnaClient.Screen
{
	class LoadingScreen : GameScreen
	{
		// Bool reference type
		class ObjBool
		{
			public bool IsReplayDeserialized { get; set; }
			public bool IsReplayDownloaded { get; set; }
			public ObjBool()
			{
				IsReplayDeserialized = false;
				IsReplayDownloaded = false;
				
			}
		}

		private Texture2D _texture;
		private readonly ObjBool _checker = new ObjBool();
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
			lock (_checker)
			{
				if (_checker.IsReplayDeserialized || _checker.IsReplayDownloaded)
				{
					if (isCorrect)
					{
						_checker.IsReplayDeserialized = false;
						ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.WatchReplayScreen);
					}
					else
						MessageBox.Show("Wrong version", ScreenManager.ScreenEnum.SelectReplayScreen);
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
			if (Network.ConnectionManager.Instance.IsOnline())
			{
				replayHandler = new Thread(DownloadReplay);
			}
			else
			{
				replayHandler = new Thread(Deserialize);
			}
			replayHandler.Start();
		}

		private void Deserialize()
		{
			string path = ScreenHelper.Instance.ReplayPath;
			ScreenHelper.Instance.ReplayEvents = SerializationHelper.Deserialize(path).Events;
			isCorrect = ScreenHelper.Instance.ReplayEvents.Count > 0;
			lock (_checker)
			{
				_checker.IsReplayDeserialized = isCorrect;
			}
		}

		private void DownloadReplay()
		{
			Network.ConnectionManager.Instance.DownloadReplay(Utils.ScreenHelper.Instance.DownloadedGameGuid);
			lock (_checker)
			{
				_checker.IsReplayDownloaded = true;
			}
		}
	}
}
