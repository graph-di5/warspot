namespace LongAriphmetic
{
    partial class Form1
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
            this.firstNum = new System.Windows.Forms.TextBox();
            this.secondNum = new System.Windows.Forms.TextBox();
            this.add = new System.Windows.Forms.Button();
            this.minus = new System.Windows.Forms.Button();
            this.mult = new System.Windows.Forms.Button();
            this.div = new System.Windows.Forms.Button();
            this.sum = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // firstNum
            // 
            this.firstNum.Location = new System.Drawing.Point(22, 34);
            this.firstNum.Name = "firstNum";
            this.firstNum.Size = new System.Drawing.Size(450, 20);
            this.firstNum.TabIndex = 0;
            // 
            // secondNum
            // 
            this.secondNum.Location = new System.Drawing.Point(22, 94);
            this.secondNum.Name = "secondNum";
            this.secondNum.Size = new System.Drawing.Size(450, 20);
            this.secondNum.TabIndex = 1;
            // 
            // add
            // 
            this.add.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.add.Location = new System.Drawing.Point(22, 60);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(64, 28);
            this.add.TabIndex = 2;
            this.add.Text = "+";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // minus
            // 
            this.minus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.minus.Location = new System.Drawing.Point(126, 60);
            this.minus.Name = "minus";
            this.minus.Size = new System.Drawing.Size(64, 28);
            this.minus.TabIndex = 3;
            this.minus.Text = "-";
            this.minus.UseVisualStyleBackColor = true;
            // 
            // mult
            // 
            this.mult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mult.Location = new System.Drawing.Point(238, 60);
            this.mult.Name = "mult";
            this.mult.Size = new System.Drawing.Size(64, 28);
            this.mult.TabIndex = 4;
            this.mult.Text = "*";
            this.mult.UseVisualStyleBackColor = true;
            // 
            // div
            // 
            this.div.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.div.Location = new System.Drawing.Point(360, 60);
            this.div.Name = "div";
            this.div.Size = new System.Drawing.Size(64, 28);
            this.div.TabIndex = 5;
            this.div.Text = "/";
            this.div.UseVisualStyleBackColor = true;
            // 
            // sum
            // 
            this.sum.Location = new System.Drawing.Point(22, 179);
            this.sum.Name = "sum";
            this.sum.Size = new System.Drawing.Size(450, 20);
            this.sum.TabIndex = 7;
            this.sum.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 272);
            this.Controls.Add(this.sum);
            this.Controls.Add(this.div);
            this.Controls.Add(this.mult);
            this.Controls.Add(this.minus);
            this.Controls.Add(this.add);
            this.Controls.Add(this.secondNum);
            this.Controls.Add(this.firstNum);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox firstNum;
        private System.Windows.Forms.TextBox secondNum;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button minus;
        private System.Windows.Forms.Button mult;
        private System.Windows.Forms.Button div;
        private System.Windows.Forms.TextBox sum;
    }
}

