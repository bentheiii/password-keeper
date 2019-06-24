#define auth


using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CipherStone;
using Edge.Guard;
using Edge.PermanentObject;
using Edge.Random;

namespace password_keeper
{
    public partial class Form1 : Form
    {
        private EventGuard<string> _path;
        private EventGuard<bool> _editable;
        private string[] _lines;
        private byte[] _password;
        private TimeSpan _timesincelogin;
        private readonly PermaObject<string> _permaPath = new PermaObject<string>("pass_keeper__defaultFilePath",true,valueIfCreated:"");
        private const string Authstring = "__authorized__";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _path = new EventGuard<string>();
            _path.changed += this.path_changed;
            _path.EventValue = "";
            Exception ex;
            this._permaPath.tryParse(out ex);
            if (ex == null)
                _path.EventValue = this._permaPath.value;
            _editable = new EventGuard<bool>();
            _editable.changed += this.editable_changed;
            _editable.EventValue = false;
        }

        private void editable_changed(object sender, EventGuard<bool>.EventGuardChangeArgs args)
        {
            richTextBox1.ReadOnly = !args.newVal;
            button3.Enabled = args.newVal;
        }

        private void path_changed(object sender, EventGuard<string>.EventGuardChangeArgs args)
        {
            label1.Text = args.newVal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var keyrequest = new keyrequest();
            if (openFileDialog1.ShowDialog() == DialogResult.OK && keyrequest.ShowDialog(this) == DialogResult.OK)
            {
                _path.EventValue = _permaPath.value =openFileDialog1.FileName;
                _password = Encoding.Unicode.GetBytes(keyrequest.Key);
                decrypt(_path.EventValue);
            }
        }
        public void decrypt (string path)
        {

            var temp = File.ReadAllBytes(path);
            if (temp.Length != 0)
            {
                var decrypted = Encoding.Unicode.GetString(SecureEncryptionV1.Decrypt(temp, _password));
                _lines = decrypted.Split('\n');
#if (auth)
                if (_lines[0] != Authstring)
                {
                    MessageBox.Show("_password: is invalid");
                    Application.Exit();
                }
#endif
            }
            else
            {
                _lines = new string[0];
            }
            filterlines(textBox1.Text, richTextBox1);
            timer1.Start();
            _timesincelogin = new TimeSpan(1,0,0);
        }
        public void filterlines(string filter, RichTextBox target)
        {
            target.Text = "";
            filter = filter.ToUpper();
            foreach (string t in this._lines)
            {
                if (t.ToUpper().IndexOf(filter) == -1) continue;
#if (auth)
                if (t != Authstring)
#endif
                    target.Text += t + "\n";
            }
            _editable.EventValue = (filter.Length == 0);
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            filterlines(textBox1.Text, richTextBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var keyrequest = new keyrequest();
            if (keyrequest.ShowDialog(this) == DialogResult.OK)
            {
                _password = Encoding.Unicode.GetBytes(keyrequest.Key);
                decrypt(_path.EventValue);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var l = Authstring + "\n" + richTextBox1.Text;
            var plaintext = Encoding.Unicode.GetBytes(l);
            var key = _password;
            var padding = new GlobalRandomGenerator().Int(plaintext.Length/4,plaintext.Length/2);
            var gen = new GlobalRandomGenerator();
            File.WriteAllBytes(_path.EventValue, SecureEncryptionV1.Encrypt(plaintext, _password, padding, () => gen.Bytes(1)[0]));
            timer1.Start();
            _timesincelogin = new TimeSpan(1,0,0);
            _lines = l.Split('\n');
        }

        private void button4_Click(object sender, EventArgs e)
        {
            (new randompassgen()).ShowDialog();
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
            {
                button3.PerformClick();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _timesincelogin = _timesincelogin.Subtract(new TimeSpan(0,0,1));
            toolStripStatusLabel1.Text = "will exit in " + _timesincelogin.ToString();
            if (_timesincelogin.TotalSeconds <= 0)
                this.Close();
        }
    }
}
