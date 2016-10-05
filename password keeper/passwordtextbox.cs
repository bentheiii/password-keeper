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
    public partial class PasswordTextBox : TextBox
    {
        private const int EM_SHOWBALLOONTIP = 0x1503;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == EM_SHOWBALLOONTIP)
            {
                m.Result = (IntPtr)0;
                return;
            }
            base.WndProc(ref m);
        }
        public PasswordTextBox(): base()
        {
            //InitializeComponent();
        }
    }
}
