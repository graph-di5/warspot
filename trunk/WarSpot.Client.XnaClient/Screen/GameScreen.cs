using Nuclex.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WarSpot.Client.XnaClient.Input;
using System.Collections.Generic;
using System;

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
        /// Загрузка контента, необходимого
        /// для отображения экрана 
        /// </summary>
        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// Уничтожение экрана, освобождение всех ресурсов
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
        /// Вызывается перед отображением экрана
        /// </summary>
        public virtual void OnShow()
        {

        }

        /// <summary>
        /// Вызывается при скрытии экрана
        /// </summary>
        public virtual void OnHide()
        {

        }

		public Dictionary<string, string> GetTexts()
		{
			Dictionary<string, string> ans = new Dictionary<string,string>();
			foreach (var control in Desktop.Children)
			{
				if (control.GetType() == typeof(InputControl))
				{
					InputControl ic = (InputControl)control;
					if (ic.Name != null)
					{
						ans.Add(ic.Name, ic.RealText);
					}
				}
			}
			return ans;
		}

		public void UseTexts(Dictionary<string, string> dict)
		{
			foreach (var control in Desktop.Children)
			{
				if (control.GetType() == typeof(InputControl))
				{
					InputControl ic = (InputControl)control;
					if (dict.ContainsKey(ic.Name))
					{
						ic.RealText = dict[ic.Name];
					}
				}
			}
		}

        //protected void DrawString(string text, float positionX, float positionY, Color color)
        //{
        //    SpriteBatch.DrawString(
        //        SpriteFont,
        //        text,
        //        new Vector2(positionX, positionY),
        //        color, 0, new Vector2(0f, 0f), 0.8f, SpriteEffects.None,
        //        layerDepth: Constants.TEXT_TEXTURE_LAYER);
        //}
    }
}
