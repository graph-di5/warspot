using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
// Вот этот коммент
using ServerClient;


namespace ChatUserClient
{
    class Chat
    {
        static void Main(string[] args)
        {
            // Создаем объект, отвечающий за обратную связь 
            InstanceContext context = new InstanceContext(new ChatCallBackHandler());
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            DuplexChannelFactory<ServerClient.IChatController> factory = new DuplexChannelFactory<ServerClient.IChatController>(context, binding);
            Uri adress = new Uri("net.tcp://localhost:30042/ChatService");
            EndpointAddress endpoint = new EndpointAddress(adress.ToString());
            ServerClient.IChatController chat = factory.CreateChannel(endpoint);

            Console.Write("Введите никнейм:");
            string nickname = Console.ReadLine();
            string[] usersInChat = chat.ConnectToChat(nickname);
            Console.WriteLine("В чате сейчас:");
            foreach (string nick in usersInChat)
            {
                Console.WriteLine(nick);
            }

            while (true)
            {
                string s = Console.ReadLine();
                chat.SendMessage(s);
            }
            chat.LeaveChat();
        }
    }
}
