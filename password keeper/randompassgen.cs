using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.WordsPlay;

namespace password_keeper
{
    public partial class randompassgen : Form
    {
        public randompassgen()
        {
            InitializeComponent();
            generatepassword();
        }
        public void generatepassword()
        {
            string allowedchars = "";
            if (checkBox4.Checked)
                allowedchars += " ";
            if (checkBox2.Checked)
                allowedchars += "abcdefghijklmnopqrstuvwxyz";
            if (checkBox3.Checked)
                allowedchars += "abcdefghijklmnopqrstuvwxyz".ToUpper();
            if (checkBox1.Checked)
                allowedchars += "0123456789";
            string res = "";
            var gen = new Edge.Random.GlobalRandomGenerator();
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                res += gen.randomchar(allowedchars);
            }
            richTextBox1.Text = res;
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            generatepassword();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            generatepassword();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            generatepassword();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            generatepassword();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            generatepassword();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            generatepassword();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.Text);
        }
    }
}
