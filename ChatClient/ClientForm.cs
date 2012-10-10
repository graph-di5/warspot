using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WCFChat
{
    public partial class ClientForm : Form
    {
        private MessageKeeperClient mkc;
        private int id = 0;
        private string nickname = "";

        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientForm());
        }

        public ClientForm()
        {
            InitializeComponent();
            richTextBox1.Text = "Enter your nickname.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (nickname.Length == 0)
            {
                if (textBox1.Text.Length == 0)
                {
                    return;
                }
                nickname = textBox1.Text;
                richTextBox1.Text = "";
                mkc = new MessageKeeperClient();
                mkc.LogIn(nickname);
                new Thread(refresh).Start();
            }
            else
            {
                mkc.SendMessage(id, textBox1.Text);
            }
            textBox1.Text = "";
        }

        private void refresh()
        {
            while (true)
            {
                string m = mkc.GetNewMessages(id);
                richTextBox1.Text += m;
                Thread.Sleep(100);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

    }
}
