using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer
{
    class ChatUser
    {
        public String Name;
        public IChatCallback Callback;

        public ChatUser(string name, IChatCallback callback)
        {
            this.Name = name;
            this.Callback = callback;
        }
    }
}
