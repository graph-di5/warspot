using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;


namespace WarSpot.Client.XnaClient.Input
{
	/// <summary>
	/// ЦЕА??? о-О
	/// </summary>
	internal class KeyboardAndMouse : Controller
	{
		private KeyboardState _currentKeyboardState;
		private MouseState _currentMouseState;

		private bool _justMaximazed = false;

		public KeyboardAndMouse(InputManager inputManager)
			: base(inputManager)
		{
			_currentKeyboardState = InputManager.GetKeyboard().GetState();
			_currentMouseState = InputManager.GetMouse().GetState();
		}
		public override void Update()
		{
			_currentKeyboardState = InputManager.GetKeyboard().GetState();
			_currentMouseState = InputManager.GetMouse().GetState();

			/*
			 * It is not possible for XNA functions to assign Alt+Enter combination for fullscreen.
			 */

			/*
			 * bool alt = _currentKeyboardState.IsKeyDown(Keys.RightAlt) doesn't work.
			 */
			bool alt = (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Alt) ==
				System.Windows.Forms.Keys.Alt;

			/*
			 * WinAPI function is used, because _currentKeyboardState.IsKeyDown('some key')
			 * doesn't work, when alt is pressed.
			 */
			if (!alt || !IsKeyPressed((int)Keys.Enter))
			{
				_justMaximazed = false;
			}
			if (!_justMaximazed && alt && IsKeyPressed((int)Keys.Enter))
			{
				WarSpotGame.Instance.ToggleFullScreen();
				_justMaximazed = true;
			}
		}

		public override void AddListener(Control control, EventHandler buttonPressed)
		{
			base.AddListener(control, buttonPressed);

			(control as ButtonControl).Pressed += buttonPressed;
		}

		//=========================

		private const ushort MASK = 0x8000;

		private static bool IsKeyPressed(int virtKey)
		{
			return ((GetKeyState(virtKey) & MASK) > 0);
		}

		[DllImport("User32.dll")]
		private static extern short GetKeyState(int nVirtKey);

	}
}
