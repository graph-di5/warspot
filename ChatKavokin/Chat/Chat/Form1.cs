using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using ChatServer;

namespace Chat
{
    public partial class Chat : Form
    {
        public Chat()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            chat.Send(messageTB.Text);
            messageTB.Clear();
        }

        private void joinButton_Click(object sender, EventArgs e)
        {

            //Создаем объект который отвечает за обратную связь  
            InstanceContext context = new InstanceContext(new ChatCallbackHandler(ChatTB));
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            DuplexChannelFactory<IChat> factory = new DuplexChannelFactory<IChat>(context, binding);
            Uri adress = new Uri("net.tcp://192.168.0.195:20010/ChatService");
            EndpointAddress endpoint = new EndpointAddress(adress.ToString());

            try
            {
                //Связь с сервером не устанавливается до тех пор, пока не будет вызван метод Join
                chat = factory.CreateChannel(endpoint);

                string[] userInChat = chat.Join(nameTB.Text);

                foreach (string user in userInChat)
                {
                    ChatTB.AppendText("User in chat: " + user+"\n");
                }
                //dialog(chat);
            }
            catch (Exception s)
            {
                MessageBox.Show("Cannot connect!");
                Application.Exit();
            }

            // убираем уже ненужные поля и активируем нужные
            ChatTB.Enabled = true;
            leaveButton.Enabled = true;
            messageTB.Enabled = true;
            SendButton.Enabled = true;
            nameTB.Visible = false;
            joinButton.Visible = false;
            label1.Visible = false;

            
        }

        static IChat chat;
        static Chat form = new Chat();

        private void leaveButton_Click(object sender, EventArgs e)
        {
            chat.Leave();
            Application.Exit();
        }
    }
}
