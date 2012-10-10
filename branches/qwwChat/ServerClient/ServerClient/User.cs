using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerClient
{
    class User
    {
        public string Nickname;
        public IChatCallback Callback;

        public User(string nickname, IChatCallback callback)
        {
            this.Nickname = nickname;
            this.Callback = callback;
        }
    }
}
