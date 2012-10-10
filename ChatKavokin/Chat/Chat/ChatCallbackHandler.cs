using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatServer;
using System.Windows.Forms;

namespace Chat
{
    public class ChatCallbackHandler : IChatCallback
    {
        RichTextBox textbox;
        public ChatCallbackHandler(RichTextBox TB)
        {
            this.textbox = TB;
        }
        public void Receive(string name, string msg)
        {
            textbox.AppendText(name + ": " + msg + "\n");
        }

        public void UserEnter(string name)
        {
            textbox.AppendText("User enter: " + name + "\n");
        }

        public void UserLeave(string name)
        {
            textbox.AppendText("User leave: " + name + "\n");
        }
    }
}
