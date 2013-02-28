using System.Linq;
using Nuclex.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WarSpot.Client.XnaClient.Input;
using System.Collections.Generic;

namespace WarSpot.Client.XnaClient.Screen
{
	public abstract class GameScreen : Nuclex.UserInterface.Screen
	{
		protected SpriteBatch SpriteBatch { get; private set; }

		protected ContentManager ContentManager { get; private set; }

		protected SpriteFont SpriteFont { get; set; }

		protected GameScreen()
		{
			Desktop.Bounds = new UniRectangle(
				new UniScalar(0f, 20f),
				new UniScalar(0f, 20f),
				new UniScalar(1f, -40f),
				new UniScalar(1f, -40f));

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;

			SpriteBatch = ScreenManager.Instance.SpriteBatch;

			ContentManager = ScreenManager.Instance.ContentManager;

			SpriteFont = ScreenManager.Instance.Font;
		}

		public bool IsActive { get; set; }

		/// <summary>
		/// Loading of necessary content for current screen
		/// </summary>
		public virtual void LoadContent()
		{
		}

		/// <summary>
		/// Unloading all stuff after screen disposing
		/// </summary>
		public virtual void Destroy()
		{

		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void HandleInput(Controller controller)
		{
		}

		public virtual void Draw(GameTime gameTime)
		{
		}

		/// <summary>
		/// Call before showing screen
		/// </summary>
		public virtual void OnShow()
		{

		}

		/// <summary>
		/// Call before hiding
		/// </summary>
		public virtual void OnHide()
		{

		}

		/// <summary>
		/// Calls when window is resized
		/// </summary>
		public virtual void OnResize()
		{
			this.Height = WarSpotGame.Instance.GraphicsDevice.Viewport.Height;
			this.Width = WarSpotGame.Instance.GraphicsDevice.Viewport.Width;
		}
	}
}
