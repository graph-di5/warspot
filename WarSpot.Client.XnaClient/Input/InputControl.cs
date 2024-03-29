﻿namespace WarSpot.Client.XnaClient.Input
{
	internal class InputControl : Nuclex.UserInterface.Controls.Desktop.InputControl
	{
		public bool IsHidden { get; set; }

		private string _realText = "";

		public string RealText
		{
			get
			{
				return _realText;
			}
			set
			{
				_realText = value;
				Text = IsHidden ? new string('*', value.Length) : value;
			}
		}

		new public string Name { get; set; }

		public static string HiddenText(string text)
		{
			return new string('*', text.Length);
		}

		protected override bool OnButtonPressed(Microsoft.Xna.Framework.Input.Buttons button)
		{
			return false;
		}

		protected override bool OnKeyPressed(Microsoft.Xna.Framework.Input.Keys keyCode)
		{
			switch (keyCode)
			{
			case Microsoft.Xna.Framework.Input.Keys.Left:
				CaretPosition = CaretPosition > 0 ? CaretPosition - 1 : 0;
				return true;
			case Microsoft.Xna.Framework.Input.Keys.Right:
				CaretPosition = CaretPosition < RealText.Length ? CaretPosition + 1 : CaretPosition;
				return true;
			case Microsoft.Xna.Framework.Input.Keys.Delete:
				if (CaretPosition < RealText.Length)
				{
					RealText = RealText.Substring(0, CaretPosition) + RealText.Substring(CaretPosition + 1);
				}
				return true;
			default:
				return false;
			}
		}

		protected override void OnButtonReleased(Microsoft.Xna.Framework.Input.Buttons button)
		{
		}

		protected override void OnKeyReleased(Microsoft.Xna.Framework.Input.Keys keyCode)
		{
		}

		protected override void OnCharacterEntered(char character)
		{
			if (character == '\b' && CaretPosition > 0)
			{
				int cp = CaretPosition;
				RealText = RealText.Substring(0, CaretPosition - 1) + RealText.Substring(CaretPosition);
				CaretPosition = cp - 1;
			}
			else if (character != '\b' && character != '\t' && character != '\n')
			{
				if (RealText == null)
				{
					RealText = "";
				}
				RealText = RealText.Substring(0, CaretPosition) + character + RealText.Substring(CaretPosition);
				CaretPosition++;
			}
		}
	}
}
