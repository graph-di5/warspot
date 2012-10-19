namespace WarSpot.Client.XnaClient.Input
{
    internal class InputControl : Nuclex.UserInterface.Controls.Desktop.InputControl
    {
        private int _passwordLength = 0;

        public bool IsHidden { get; set; }

        public string RealText { get; set; }

        public static string HiddenText(string text)
        {
            return new string('*', text.Length);
        }

        protected override void OnCharacterEntered(char character)
        {
            if (character == '\b' && _passwordLength != 0)
            {
                RealText = RealText.Substring(0, _passwordLength - 1);
                _passwordLength--;
            }
            else if (char.IsLetter(character) || char.IsDigit(character) || (character == '_'))
            {
                _passwordLength++;
                RealText += character;
                Text += IsHidden ? '*' : character;
                CaretPosition++;
            }
        }
    }
}
