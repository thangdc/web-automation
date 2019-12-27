using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WebAutomation
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmMain main = (frmMain)Application.OpenForms["frmMain"];
            int result = main.login(tbxUsername.Text, tbxPassword.Text);
            string message = Language.Resource.Message;

            if (result == 1)
            {
                Close();
            }
            else if(result == -1)
            {
                tbxUsername.Focus();
                MessageBox.Show(Language.Resource.UsernameRequired, message);
            }
            else if (result == -2)
            {
                tbxPassword.Focus();
                MessageBox.Show(Language.Resource.PasswordRequired, message);
            }
            else if (result == -3)
            {
                tbxUsername.Focus();
                MessageBox.Show(Language.Resource.UserNotExists, message);
            }
            else if (result == -4 || result == 0)
            {
                tbxUsername.Focus();
                MessageBox.Show(Language.Resource.AccountIncorrect, message);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Language.Resource.Username;
            lblPassword.Text = Language.Resource.Password;
            btnLogin.Text = Language.Resource.Login;
            btnCancel.Text = Language.Resource.Cancel;
            this.Text = Language.Resource.Login;
        }
    }
}
