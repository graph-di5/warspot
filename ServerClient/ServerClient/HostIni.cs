using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ServerClient
{
    class HostIni
    {
        static void Main(string[] args)
        {
            // Создается хост опять какой-то интернетной магией, конечные точки, подключения D:
            ServiceHost host = new ServiceHost(typeof(ChatController));
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            Uri adress = new Uri("net.tcp://localhost:30042/ServerClient");
            host.AddServiceEndpoint(typeof(IChatController), binding, adress.ToString());

            host.Open();
            Console.WriteLine("Жду юзеров :3");
            // Никто же ничего не хочет писать в консоль
            Console.ReadLine();
            host.Close();

            // Тут всё непонятно
        }
    }
}
