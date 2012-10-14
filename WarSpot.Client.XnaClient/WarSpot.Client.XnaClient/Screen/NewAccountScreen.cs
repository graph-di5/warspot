using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using InputControl = WarSpot.Client.XnaClient.Input.InputControl;

namespace WarSpot.Client.XnaClient.Screen
{
    internal class NewAccountScreen :  GameScreen
    {
        private static Texture2D _texture;

        private ButtonControl _registerButton;
        private ButtonControl _backButton;

        private LabelControl _name;
        private LabelControl _sureName;
        private LabelControl _email;
        private LabelControl _nickname;
        private LabelControl _password;

    }
}
