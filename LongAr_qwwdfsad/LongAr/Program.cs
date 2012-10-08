using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LongAr
{

    class Program
    {   

        static void Main(string[] args)
        {
            BigNumbers[] idiotarray = new BigNumbers[10000];
            BigNumbers result = new BigNumbers("1");

            int fact = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < fact; i++)
            {
                string tmp = Convert.ToString(i + 1);
                idiotarray[i] = new BigNumbers(tmp);
            }
            
            for (int i = 0; i < fact; i++)
            {
                result = result * idiotarray[i];
            }
            
            result.Write();
            Console.ReadKey();                   

        }
    }
}
