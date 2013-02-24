using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WarSpot.Client.XnaClient.Screen
{
	class LoadingScreen : GameScreen
	{
		// Bool reference type
		class ObjBool
		{
			public bool IsReplayDeserialized;
			public ObjBool()
			{
				IsReplayDeserialized = false;
			}
		}
		private Texture2D _texture;
		private readonly ObjBool _checker = new ObjBool();

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
				if (_checker.IsReplayDeserialized)
				{
					_checker.IsReplayDeserialized = false;
					ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.WatchReplayScreen);
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
			Thread replayHandler = new Thread(Deserialize);
			replayHandler.Start();
		}

		private void Deserialize()
		{
			string path = Utils.ScreenHelper.Instance.ReplayPath;
			Utils.ScreenHelper.Instance.replayEvents = Utils.Deserializator.Deserialize(path);
			lock (_checker)
			{
				_checker.IsReplayDeserialized = true;
			}
		}
	}
}
