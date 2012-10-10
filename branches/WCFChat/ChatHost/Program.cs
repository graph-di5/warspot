using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WCFChat
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(MessageKeeper), new Uri("http://localhost:1234/"));
            host.Open();
            Console.WriteLine("Press <ENTER> to terminate Host");
            Console.ReadLine();
        }
    }
}
