using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ThangDC.Core.Entities;

namespace WebAutomation
{
    public partial class frmAccountUpdate : Form
    {
        private bool _IsAdd = false;
        private string _name = "";

        public frmAccountUpdate()
        {
            _IsAdd = true;
            InitializeComponent();
        }

        public frmAccountUpdate(string name)
        {
            _IsAdd = false;
            _name = name;
            InitializeComponent();

            if (User.Current != null)
            {
                Account account = new Account();
                var acc = account.GetBy(name);

                if (acc != null)
                {
                    tbxName.Text = acc.Name;
                    tbxUsername.Text = acc.Username;
                    tbxPassword.Text = acc.Password;
                    tbxDescription.Text = acc.Description;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxName.Text))
            {
                string message = Language.Resource.Message;
                MessageBox.Show(Language.Resource.NameRequired, message);
            }
            else
            {
                if (_IsAdd)
                {
                    AddNew();
                }
                else
                {
                    UpdateAccount(_name);
                }
            }
        }

        private void frmAccountUpdate_Load(object sender, EventArgs e)
        {
            lblName.Text = Language.Resource.Name;
            lblUser.Text = Language.Resource.Username;
            lblPassword.Text = Language.Resource.Password;
            lblDescription.Text = Language.Resource.Description;
            if (_IsAdd)
            {
                this.tbxName.Enabled = true;
                this.Text = Language.Resource.AddNew;
                btnOK.Text = Language.Resource.AddNew;
            }
            else
            {
                this.tbxName.Enabled = false;
                this.Text = Language.Resource.Update;
                btnOK.Text = Language.Resource.Update;
            }
            btnCancel.Text = Language.Resource.Cancel;
        }

        private void AddNew()
        {
            if (User.Current != null)
            {
                Account account = new Account();

                bool check = account.CheckExists(tbxName.Text);

                string message = Language.Resource.Message;

                if (check)
                {
                    MessageBox.Show(Language.Resource.NameExists, message);
                }
                else
                {
                    account.Name = tbxName.Text;
                    account.Username = tbxUsername.Text;
                    account.Password = tbxPassword.Text;
                    account.Description = tbxDescription.Text;
                    int result = account.Add();

                    if (result == 1)
                    {
                        frmManager manager = (frmManager)Application.OpenForms["frmManager"];
                        manager.SelectTab("tabAccounts");
                        this.Close();
                    }
                }
            }
        }

        public void UpdateAccount(string name)
        {
            if (User.Current != null)
            {
                Account account = new Account();
                var currentAccount = account.GetBy(name);

                if (currentAccount != null)
                {
                    account.Name = tbxName.Text;
                    account.Username = tbxUsername.Text;
                    account.Password = tbxPassword.Text;
                    account.Description = tbxDescription.Text;
                    int result = account.Update();

                    if (result == 1)
                    {
                        frmManager manager = (frmManager)Application.OpenForms["frmManager"];
                        manager.SelectTab("tabAccounts");
                        this.Close();
                    }
                }
            }
        }
    }
}
