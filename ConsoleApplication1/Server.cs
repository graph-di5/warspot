using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Chat
{
    class Server
    {
        public static void Main (string[] args)
        {
            ServiceHost sh = new ServiceHost(typeof(ChatService));
            sh.Open();

            Console.WriteLine("Server started");
            Console.ReadKey();
        }
    }
}
