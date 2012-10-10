using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Создаем непосредственно сам Хост 
            //192.168.0.195
            ServiceHost host = new ServiceHost(typeof(ChatService));
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            Uri adress = new Uri("net.tcp://localhost:20010/ChatService");
            host.AddServiceEndpoint(typeof(IChat), binding, adress.ToString());

            //Открываем порт и сервис ожидает клиентов  
            host.Open();
            Console.WriteLine("Service running...");
            Console.ReadKey();
            host.Close();
        }
    }
}
