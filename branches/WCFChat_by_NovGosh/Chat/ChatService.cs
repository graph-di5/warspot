using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Chat
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string login;
        [DataMember]
        public string message;

        public Message (string login, string message)
        {
            this.login = login;
            this.message = message;
        }

        public override string ToString ()
        {
            return login + " : " + message;
        }
    }

    public class ChatService : IChatService
    {
        static List <string> users = new List <string> ();
        static List <Message> messages = new List<Message>();

        public bool Register (string login)
        {
            Console.WriteLine("Message, Login = " + login);
            
            bool flag = false;
            foreach (string s in users)
                if (s == login)
                    flag = true;

            Console.WriteLine(flag);

            if (!flag) {
                users.Add(login);
                return true;
            }
            else
                return false;
        }
    
        public void SendMessage (Message mess)
        {
            messages.Add(mess);
        }

        public Message GetNewMessages (int last) 
        {
            if (last + 1 < messages.Count)
                return messages[last + 1];
            else
                return null;        
        }
    }
}