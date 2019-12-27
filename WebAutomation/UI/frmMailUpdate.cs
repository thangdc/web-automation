using System;
using System.Windows.Forms;
using ThangDC.Core.Entities;

namespace WebAutomation
{
    public partial class frmMailUpdate : Form
    {
        private bool IsAddNew = false;
        private string _name = "";

        public frmMailUpdate()
        {
            InitializeComponent();
            IsAddNew = true;
            LoadLanguage(IsAddNew);
        }

        public frmMailUpdate(string name)
        {
            InitializeComponent();

            if (User.Current != null)
            {
                IsAddNew = false;
                LoadLanguage(IsAddNew);
                _name = name;

                MailServer mailserver = new MailServer();
                var current = mailserver.GetBy(name);

                if (current != null)
                {
                    tbxName.Text = current.Name;
                    tbxServer.Text = current.Server;
                    tbxUsername.Text = current.Username;
                    tbxPassword.Text = current.Password;
                    tbxPort.Text = current.Port.ToString();
                }
            }
        }

        public void LoadLanguage(bool isAdd)
        {
            lblName.Text = Language.Resource.Name;
            lblServer.Text = Language.Resource.Server;
            lblUsername.Text = Language.Resource.Username;
            lblPassword.Text = Language.Resource.Password;
            lblPort.Text = Language.Resource.Port;
            if (isAdd)
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxName.Text))
            {
                string message = Language.Resource.Message;
                MessageBox.Show(Language.Resource.NameRequired, message);
            }
            else
            {
                if (IsAddNew)
                {
                    AddNew();
                }
                else
                {
                    UpdateMail(_name);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddNew()
        {
            var mailServer = new MailServer();
            string message = Language.Resource.Message;            

            bool check = mailServer.CheckExists(tbxName.Text);

            if (check)
            {
                MessageBox.Show(Language.Resource.NameExists, message);
            }
            else
            {
                bool isNum = int.TryParse(tbxPort.Text, out int num);

                if (isNum)
                {
                    mailServer.Name = tbxName.Text;
                    mailServer.Server = tbxServer.Text;
                    mailServer.Username = tbxUsername.Text;
                    mailServer.Password = tbxPassword.Text;
                    mailServer.Port = num;

                    int result = mailServer.Add();
                    if (result == 1)
                    {
                        var manager = (frmManager)Application.OpenForms["frmManager"];
                        manager.SelectTab("tabMails");
                        Close();
                    }
                }
                else
                {
                    tbxPort.Focus();
                    MessageBox.Show(Language.Resource.NotNumber, message);
                }
            }
        }

        public void UpdateMail(string name)
        {
            var mailServer = new MailServer();
            var current = mailServer.GetBy(name);

            if (current != null)
            {
                int Num;
                bool isNum = int.TryParse(tbxPort.Text, out Num);

                if (isNum)
                {
                    current.Name = tbxName.Text;
                    current.Server = tbxServer.Text;
                    current.Username = tbxUsername.Text;
                    current.Password = tbxPassword.Text;
                    current.Port = Num;

                    int result = current.Update();
                    if (result == 1)
                    {
                        var manager = (frmManager)Application.OpenForms["frmManager"];
                        manager.SelectTab("tabMails");
                        Close();
                    }
                }                
            }
        }
    }
}
