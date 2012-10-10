using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServerClient
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatController : IChatController
    {
        // Паблик модификатор не заработал О_о
        private static List<User> ChatUsers = new List<User>();
        private User currentUser;

        // Клиент коннектится к чату, магией включается колбэк, оповещает всех
        public string[] ConnectToChat(string nickname)
        {
            IChatCallback callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();

            User NewChatUser = new User(nickname, callback);
            ChatUsers.Add(NewChatUser);
            currentUser = NewChatUser;
            // Зашедшему тоже отсылается
            foreach (User user in ChatUsers)
            {
                user.Callback.UserConnectChat(nickname);
            }

            // Это если надо мониторить сервер (опционально)
            Console.WriteLine("К нам присоединился {0}", nickname);

            string[] usersInChat = new string[ChatUsers.Count];
            int i = -1;
            foreach (var user in ChatUsers)
            {
                ++i;
                usersInChat[i] = ChatUsers[i].Nickname;
            }

            return usersInChat;
        }

        // Отправляет сообщение сюды и мифическим колбэком заставляет других увидеть его
        public void SendMessage(string message)
        {
            foreach (var user in ChatUsers)
            {
                user.Callback.ReceiveMsg(user.Nickname, message);
            }
            // Если мониторить сервер (опционально)
            Console.WriteLine("");
        }

        public void LeaveChat()
        {
            foreach (var user in ChatUsers)
            {
                user.Callback.UserLeaveChat(currentUser.Nickname);
            }
            // Еще одна опциональная оповещалка
            Console.WriteLine();
        }
    }
}
