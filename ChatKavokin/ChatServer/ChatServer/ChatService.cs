using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Windows.Forms;

namespace ChatServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class ChatService : IChat
    {
        private static List<ChatUser> chatUsers = new List<ChatUser>();
        private ChatUser _user;

        public String[] Join(string name)
        {
            //Получаем Интерфейс обратного вызова  
            IChatCallback callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();
            //Массив всех участников чата, который мы вернем клиенту  
            string[] tmpUsers = new string[chatUsers.Count];
            for (int i = 0; i < chatUsers.Count; i++)
            {
                tmpUsers[i] = chatUsers[i].Name;
            }
            //Оповещаем всех клиентов что в чат вошел новый пользователь  
            foreach (ChatUser user in chatUsers)
            {
                user.Callback.UserEnter(name);
            }
            //Создаем новый экземпляр пользователя и заполняем все его поля  
            ChatUser chatUser = new ChatUser(name, callback);
            chatUsers.Add(chatUser);
            _user = chatUser;
            Console.WriteLine(">>User Enter: {0}", name);
            // возвращаем новичку список клиентов чата
            return tmpUsers;
        }

        public void Leave()
        {
            Console.WriteLine(">>User Leave: {0}", _user.Name);
            chatUsers.Remove(_user);
            //Оповещаем всех клиентов о том что пользователь нас покинул  
            foreach (ChatUser item in chatUsers)
            {
                item.Callback.UserLeave(_user.Name);
            }
            _user = null;
            //Закрываем канал связи с текущим пользователем  
            OperationContext.Current.Channel.Close();
        }

        public void Send(string msg)
        {
            //var usersSending = from u in chatUsers where u.Name != _user.Name select u;
            foreach (ChatUser item in chatUsers)
            {
                item.Callback.Receive(_user.Name, msg);
            }
        }
    }
}
