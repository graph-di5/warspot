using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LongAriphmetic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private bool isCorrect()
        {
            for (int posInNum = 0; posInNum < firstNum.TextLength; posInNum++)
            {
                switch (firstNum.Text[posInNum])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;

                    default:
                        MessageBox.Show("ERROR", "Invalid value");
                        return false;
                }
            }

            if (String.IsNullOrEmpty(firstNum.Text) || String.IsNullOrEmpty(secondNum.Text))
            {
                MessageBox.Show("ERROR", "Invalid value");
                return false;
            }
            return true;
        }

        private void add_Click(object sender, EventArgs e)
        {
            if (isCorrect())
            {
                string first = firstNum.Text;
                string second = secondNum.Text;

                BinaryOperations res = new BinaryOperations();
                int[] result = res.Addition(res.ConvertStringToArray(first), res.ConvertStringToArray(second));
                writeResult(result);
            }
        }

        private void writeResult(int[] result)
        {
            sum.Clear();
            for (int pos = 0; pos < result.Length ; pos++)
            {
                sum.AppendText(result[pos].ToString());
            }
        }
    }
}
