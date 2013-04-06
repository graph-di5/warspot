using System.Text.RegularExpressions;

namespace WarSpot.Client.XnaClient.Screen
{
	static class InputValidator
	{
		static public bool CheckText(string input)
		{
			if (input == null)
				return false;
			const string pattern = @"^[\w.-_]{6,18}$";
			return Regex.IsMatch(input, pattern);
		}

		static public bool CheckEmail(string input)
		{
			if (input == null)
				return false;
			const string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
			return Regex.IsMatch(input, pattern);
		}

		static public bool CheckReplay(string input)
		{
			if (input == null)
				return false;
			const string pattern = @".*\\.out$";
			return Regex.IsMatch(input, pattern);
		}

        static public bool isNumber(string input)
        {
            if (input == null)
                return false;
            const string pattern = "^[0-9]+$";
            return Regex.IsMatch(input, pattern);
        }
	}

}
