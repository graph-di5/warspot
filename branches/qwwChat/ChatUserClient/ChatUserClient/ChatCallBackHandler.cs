using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;


namespace ChatUserClient
{
    class ChatCallBackHandler : IChatCallback
    {
        public void ReceiveMsg(string name, string msg)
        {
            Console.WriteLine("{0}: {1}", name, msg);
        }

        public void UserConnectChat(string name)
        {
            Console.WriteLine("К нам присоеденился: {0}", name);
        }

        public void UserLeaveChat(string name)
        {
            Console.WriteLine("Нас покинул: {0}", name);
        }
    }
}
