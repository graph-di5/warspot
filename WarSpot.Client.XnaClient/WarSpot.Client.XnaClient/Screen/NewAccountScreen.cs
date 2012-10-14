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

        private LabelControl _nameLabel;
        private LabelControl _surNameLabel;
        private LabelControl _emailLabel;
        private LabelControl _nicknameLabel;
        private LabelControl _passwordLabel;

        private InputControl _nameBox;
        private InputControl _surNameBox;
        private InputControl _emailBox;

    }
}
