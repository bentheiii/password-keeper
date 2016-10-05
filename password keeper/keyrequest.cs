using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace password_keeper
{
    
    public partial class keyrequest : Form
    {
        public string Key = "";
        public keyrequest()
        {
            InitializeComponent();
            passwordTextBox1.Select();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Key= passwordTextBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = (passwordTextBox1.Text.Length >= 1);
        }
        static Point _cursoradjust = new Point(-1, -1), _formadjust;
        private void keyrequest_MouseDown(object sender, MouseEventArgs e)
        {
            _cursoradjust = Cursor.Position;
            _formadjust = this.Location;
        }

        private void keyrequest_MouseMove(object sender, MouseEventArgs e)
        {
            if (_cursoradjust.X != -1)
            {
                this.Location = new Point(_formadjust.X + Cursor.Position.X - _cursoradjust.X, _formadjust.Y + Cursor.Position.Y - _cursoradjust.Y);
            }
        }

        private void keyrequest_MouseUp(object sender, MouseEventArgs e)
        {
            _cursoradjust = new Point(-1, -1);
        }

        private void keyrequest_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            passwordTextBox1.UseSystemPasswordChar = !((CheckBox)sender).Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Visible = IsKeyLocked(Keys.CapsLock);
        }
    }
}
