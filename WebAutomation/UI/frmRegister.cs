using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WebAutomation
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            lblUsername.Text = Language.Resource.Username;
            lblEmail.Text = Language.Resource.Email;
            lblPassword.Text = Language.Resource.Password;
            lblConfirm.Text = Language.Resource.Confirm;
            btnRegister.Text = Language.Resource.Register;
            btnCancel.Text = Language.Resource.Cancel;

            this.Text = Language.Resource.Register;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = tbxUsername.Text;
            string email = tbxEmail.Text;
            string password = tbxPassword.Text;
            string confirm = tbxConfirm.Text;

            frmMain main = (frmMain)Application.OpenForms["frmMain"];

            int result = main.register(username, email, password, confirm);

            string title = Language.Resource.Message;
            string message = "";

            if (result == -1)
            {
                message = Language.Resource.UsernameRequired;
                MessageBox.Show(message, title);
            }
            else if (result == -2)
            {
                message = Language.Resource.EmailRequired;
                MessageBox.Show(message, title);
            }
            else if (result == -3)
            {
                message = Language.Resource.PasswordRequired;
                MessageBox.Show(message, title);
            }
            else if (result == -4)
            {
                message = Language.Resource.ConfirmRequrired;
                MessageBox.Show(message, title);
            }
            else if (result == -5)
            {
                message = Language.Resource.EmailNotValid;
                MessageBox.Show(message, title);
            }
            else if (result == -6)
            {
                message = Language.Resource.PasswordAndConfirmNotTheSame;
                MessageBox.Show(message, title);
            }
            else if (result == -7)
            {
                message = Language.Resource.EmailExists;
                MessageBox.Show(message, title);
            }
            else if (result == 1)
            {
                message = Language.Resource.RegisterSuccessful;
                MessageBox.Show(message, title);
                main.CheckManager();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
