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
        ComboBox comboBox;
        public ChatCallbackHandler(RichTextBox TB, ComboBox userList)
        {
            this.comboBox = userList;
            this.textbox = TB;
        }

        public void Receive(string name, string msg)
        {
            textbox.AppendText(name + ": " + msg + "\n");
        }

        public void ReceiveTo(string name, string nameTo, string msg)
        {
            textbox.AppendText(name + " to " + nameTo + " : " + msg + "\n");
        }

        public void ReceiveToPrivate(string name, string nameTo, string msg)
        {
            textbox.AppendText(name + " private to " + nameTo + " : " + msg + "\n");
        }

        public void UserEnter(string name)
        {
            comboBox.Items.Add(name);
            textbox.AppendText("User enter: " + name + "\n");
        }

        public void UserLeave(string name)
        {
            comboBox.Items.Remove(name);
            textbox.AppendText("User leave: " + name + "\n");
        }
    }
}
