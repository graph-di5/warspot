using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WCFChat
{
    public class MessageKeeper : IMessageKeeper
    {
        private const int MAX_USERS = 1000;

        private static List<string> messages = new List<string>();
        private static int[] last = new int[MAX_USERS];
        private static string[] names = new string[MAX_USERS];
        private static Dictionary<string, int> users = new Dictionary<string, int>();
        private static int lastID = 0;

        public string GetNewMessages(int userid)
        {
            try
            {
                StringBuilder ret = new StringBuilder();
                for (int i = (last[userid] >= 0 ? last[userid] : 0); i < messages.Count; ++i)
                {
                    ret.AppendLine(messages[i]);
                }
                last[userid] = messages.Count;
                return ret.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public void SendMessage(int userid, string message)
        {
            try
            {
                messages.Add(names[userid] + ": " + message);
                Console.WriteLine(names[userid] + ": " + message);
            }
            catch (Exception ex)
            {

            }
        }

        public int LogIn(string nickname)
        {
            messages.Add("--> " + nickname + " logged in.");
            if (users.ContainsKey(nickname))
            {
                Console.WriteLine("--> User " + nickname + " logged in.");
                last[users[nickname]] = 0;
                return users[nickname];
            }
            else
            {
                Console.WriteLine("--> User " + nickname + " logged in. (first time)");
                users.Add(nickname, lastID);
                names[lastID] = nickname;
                ++lastID;
                return lastID - 1;
            }
        }
    }
}
