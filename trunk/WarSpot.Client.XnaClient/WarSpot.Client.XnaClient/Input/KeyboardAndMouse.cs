using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.Input;
using Microsoft.Xna.Framework.Input;

namespace WarSpot.Client.XnaClient.Input
{
    internal class KeyboardAndMouse : Controller
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _lastKeyboardState;

        private MouseState _currentMouseState;
        private MouseState _lastMouseState;

        public KeyboardAndMouse(InputManager inputManager)
            : base(inputManager)
        {
            _currentKeyboardState = InputManager.GetKeyboard().GetState();
            _currentMouseState = InputManager.GetMouse().GetState();
        }
    }
}
