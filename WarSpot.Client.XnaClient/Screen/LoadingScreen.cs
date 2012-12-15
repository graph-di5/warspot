using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using InputControl = WarSpot.Client.XnaClient.Input.InputControl;
using WarSpot.Contracts.Service;
using System.Threading;


namespace WarSpot.Client.XnaClient.Screen
{
	class LoadingScreen : GameScreen
	{
		// Bool reference type
		class objBool
		{
			public bool isReplayDeserialized;
			public objBool()
			{
				isReplayDeserialized = false;
			}
		}
		private Texture2D _texture;
		private objBool checker = new objBool();

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
			lock (checker)
			{
				if (checker.isReplayDeserialized)
				{
					checker.isReplayDeserialized = false;
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
			Utils.ScreenHelper.Instance.replayEvents = OfflineMatcher.Deserializator.Deserialize(path);
			lock (checker)
				checker.isReplayDeserialized = true;
		}
	}
}
