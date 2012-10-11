using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
 
namespace Chat
{
    class Client : ServiceReference.ChatServiceClient  
    {
        string login;
        int last = 0;
        Thread newMessageThread;

        bool Register (string login)
        {
            if (base.Register(login)) {
                this.login = login;
                return true;
            }
            else {
                return false;
            }
        }

        void SendMessage (string mess)
        {
            base.SendMessage(new Message(login, mess));
        }

        void GetNewMessages ()
        {
            Message mess = GetNewMessages(last);
            while (mess != null) {
                Console.WriteLine(mess);
                last++;
                mess = GetNewMessages(last);
            }
        }

        string GetCommand ()
        {
            string str = Console.ReadLine();
            
            if (str == "/exit") {
                newMessageThread.Abort();

                Console.WriteLine("Application stoped");
                while (true) ;
            }
            else
                return str;
        }

        public void Start ()
        {
            Open();
            newMessageThread = new Thread(GetNewMessageThread);

            Console.WriteLine("Type your login");
            while (true) {
                string str = GetCommand();
                if (Register(str))
                    break;
                else
                    Console.WriteLine("Can not use this login. Type another one");
            }

            newMessageThread.Start();

            while (true) {
                string str = GetCommand();
                SendMessage(str);
            }
        }

        void GetNewMessageThread ()
        {
            while (true) {
                GetNewMessages();
                Thread.Sleep(10);                
            }
        }

        static void Main (string[] args)
        {
            Client client = new Client();
            client.Start();
        }
    }
}
