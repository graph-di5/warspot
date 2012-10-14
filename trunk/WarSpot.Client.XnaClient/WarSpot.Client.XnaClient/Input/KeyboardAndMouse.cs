using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.Input;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSpot.Client.XnaClient.Screen;

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
        public override void Update()
        {
        }

        public override void AddListener(Control control, EventHandler buttonPressed)
        {
            base.AddListener(control, buttonPressed);

            (control as ButtonControl).Pressed += buttonPressed;
        }

    }
}
