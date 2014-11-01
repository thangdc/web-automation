using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ThangDC.Core.Entities;
using ThangDC.Core.Securities;

namespace WebAutomation
{
    public partial class frmUserAccount : Form
    {
        private string _name;
        private string _oldPassword;
        public frmUserAccount()
        {
            InitializeComponent();
            tbxUserName.Enabled = true;
        }

        public frmUserAccount(string name)
        {
            _name = name;
            _oldPassword = "";
            InitializeComponent();
            tbxUserName.Enabled = false;

            if (User.Current != null)
            {
                User user = new User();
                var current = user.GetBy(name);
                if (current != null)
                {
                    tbxUserName.Text = current.UserName;
                    _oldPassword = current.Password;
                    tbxEmail.Text = current.Email;
                }
            }
        }
        private void frmUserAccount_Load(object sender, EventArgs e)
        {
            this.Text = Language.Resource.UserAccountUpdate;

            lblUserName.Text = Language.Resource.Username;
            lblOldPassword.Text = Language.Resource.OldPassword;
            lblNewPassword.Text = Language.Resource.NewPassword;
            lblConfirm.Text = Language.Resource.Confirm;
            lblEmail.Text = Language.Resource.Email;
            btnOK.Text = Language.Resource.OK;
            btnCancel.Text = Language.Resource.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateUser();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateUser()
        {
            if (User.Current != null)
            {
                User user = new User();
                string message = Language.Resource.Message;

                if (!string.IsNullOrEmpty(tbxUserName.Text))
                {
                    if (_oldPassword == tbxOldPassword.Text)
                    {
                        if (!string.IsNullOrEmpty(tbxNewPassword.Text))
                        {
                            if (tbxNewPassword.Text == tbxConfirm.Text)
                            {
                                user.UserName = tbxUserName.Text;
                                user.Password = tbxNewPassword.Text;
                                user.Email = tbxEmail.Text;

                                int result = user.Update();

                                if (result == 1)
                                {
                                    frmManager manager = (frmManager)Application.OpenForms["frmManager"];
                                    manager.SelectTab("tabUsers");
                                    this.Close();
                                }
                            }
                            else
                            {
                                MessageBox.Show(Language.Resource.NewPasswordAndConfirmNotTheSame, message);
                            }
                        }
                        else
                        {
                            MessageBox.Show(Language.Resource.NewPasswordRequire, message);
                        }
                    }
                    else
                    {
                        MessageBox.Show(Language.Resource.OldPasswordIncorrect, message);
                    }
                }
                else
                {
                    MessageBox.Show(Language.Resource.UsernameRequired, message);
                }

            }
        }
    }
}
