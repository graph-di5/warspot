using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WarSpot.Client.XnaClient.Input
{
    static class InputValidator
    {
        static public bool checkText(string input)
        {
            if (input == null)
                return false;
            string pattern = "^[0-9a-zA-Z_]{6,18}$";
            return Regex.IsMatch(pattern, input);
        }

        static public bool checkEmail(string input)
        {
            if (input == null)
                return false;
            string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";
            return Regex.IsMatch(input, pattern);
        }
    }

}
