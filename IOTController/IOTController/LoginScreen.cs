using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IOTController
{
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void LoginPicBtn_Click(object sender, EventArgs e)
        {
            if (TxtUsername.Text == "admin" && TxtPassword.Text == "admin")
            {
                this.Hide();
                Form1 frm = new Form1();
                frm.Show();
            }
            else
                MessageBox.Show("Invalid username/password", "Authentication", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PicBtnClose_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
    }
}
