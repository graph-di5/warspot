using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarSpot.Client.XnaClient.Screen
{
	internal class WatchReplayScreen : GameScreen
	{
		private static Texture2D _texture;

		public WatchReplayScreen()
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

		}

		private void InitializeControls()
		{

		}
	}
}
