using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigCalc
{
	class Program
	{
		static void Main(string[] args)
		{
			System.Numerics.BigInteger first, second, result;
			
			System.Console.WriteLine("Enter a first number: ");

			if (!(System.Numerics.BigInteger.TryParse(System.Console.ReadLine(), out first)))
			{
				System.Console.WriteLine("Can't parse a number");
				System.Console.ReadKey();
				return;
			}

			System.Console.WriteLine("Enter a second number: ");

			if (!(System.Numerics.BigInteger.TryParse(System.Console.ReadLine(), out second)))
			{
				System.Console.WriteLine("Can't parse a number");
				System.Console.ReadKey();
				return;
			}

			System.Console.WriteLine("Enter an operator (+,-,*,/): ");
			string Operator = System.Console.ReadLine();
			switch (Operator)
			{
				case "+":
					result = first + second;
					break;
				case "-":
					result = first - second;
					break;
				case "*":
					result = first * second;
					break;
				case "/":
					result = first / second;
					break;
				default:
					System.Console.WriteLine("Can't parse an operator");
					System.Console.ReadKey();
					return;
			}

			System.Console.WriteLine("Result is: {0}", result.ToString());
			System.Console.ReadKey();
		}
	}
}
