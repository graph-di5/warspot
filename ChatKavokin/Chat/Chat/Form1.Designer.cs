namespace Chat
{
    partial class Chat
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChatTB = new System.Windows.Forms.RichTextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.messageTB = new System.Windows.Forms.RichTextBox();
            this.userList = new System.Windows.Forms.ListBox();
            this.TO = new System.Windows.Forms.Label();
            this.leaveButton = new System.Windows.Forms.Button();
            this.joinButton = new System.Windows.Forms.Button();
            this.nameTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ChatTB
            // 
            this.ChatTB.BackColor = System.Drawing.SystemColors.Control;
            this.ChatTB.Location = new System.Drawing.Point(0, 1);
            this.ChatTB.Name = "ChatTB";
            this.ChatTB.ReadOnly = true;
            this.ChatTB.Size = new System.Drawing.Size(404, 226);
            this.ChatTB.TabIndex = 0;
            this.ChatTB.Text = "";
            // 
            // SendButton
            // 
            this.SendButton.Enabled = false;
            this.SendButton.Location = new System.Drawing.Point(352, 256);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(111, 31);
            this.SendButton.TabIndex = 1;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // messageTB
            // 
            this.messageTB.Enabled = false;
            this.messageTB.Location = new System.Drawing.Point(105, 235);
            this.messageTB.Name = "messageTB";
            this.messageTB.Size = new System.Drawing.Size(218, 52);
            this.messageTB.TabIndex = 2;
            this.messageTB.Text = "";
            // 
            // userList
            // 
            this.userList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.userList.FormattingEnabled = true;
            this.userList.Items.AddRange(new object[] {
            "everybody"});
            this.userList.Location = new System.Drawing.Point(0, 256);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(95, 17);
            this.userList.TabIndex = 3;
            // 
            // TO
            // 
            this.TO.AutoSize = true;
            this.TO.Location = new System.Drawing.Point(15, 235);
            this.TO.Name = "TO";
            this.TO.Size = new System.Drawing.Size(42, 13);
            this.TO.TabIndex = 4;
            this.TO.Text = "send to";
            // 
            // leaveButton
            // 
            this.leaveButton.Enabled = false;
            this.leaveButton.Location = new System.Drawing.Point(410, 123);
            this.leaveButton.Name = "leaveButton";
            this.leaveButton.Size = new System.Drawing.Size(124, 31);
            this.leaveButton.TabIndex = 5;
            this.leaveButton.Text = "Leave chat";
            this.leaveButton.UseVisualStyleBackColor = true;
            this.leaveButton.Click += new System.EventHandler(this.leaveButton_Click);
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point(410, 63);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(124, 30);
            this.joinButton.TabIndex = 6;
            this.joinButton.Text = "Join chat";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // nameTB
            // 
            this.nameTB.Location = new System.Drawing.Point(410, 37);
            this.nameTB.Name = "nameTB";
            this.nameTB.Size = new System.Drawing.Size(124, 20);
            this.nameTB.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(417, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "My name is:";
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 299);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameTB);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.leaveButton);
            this.Controls.Add(this.TO);
            this.Controls.Add(this.userList);
            this.Controls.Add(this.messageTB);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.ChatTB);
            this.Name = "Chat";
            this.Text = "Chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox ChatTB;
        private System.Windows.Forms.Button SendButton;
        public System.Windows.Forms.RichTextBox messageTB;
        public System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.Label TO;
        public System.Windows.Forms.Button leaveButton;
        public System.Windows.Forms.Button joinButton;
        public System.Windows.Forms.TextBox nameTB;
        private System.Windows.Forms.Label label1;
    }
}

