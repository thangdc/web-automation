using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ThangDC.Core.Entities;
using ThangDC.Core.Securities;

namespace WebAutomation
{
    public partial class frmManager : Form
    {
        private TabPage currentTab;
        public frmManager()
        {
            InitializeComponent();
        }

        private void frmManager_Load(object sender, EventArgs e)
        {
            currentTab = tabMain.SelectedTab;

            if (User.Current != null)
            {
                LoadUser();
                LoadConnection();
                LoadMail();
                LoadAccount();
            }
            else
            {
                this.Close();
            }

            LoadLanguage();
        }

        private void tabMain_Selected(object sender, TabControlEventArgs e)
        {
            currentTab = e.TabPage;
            
            if (currentTab.Text == Language.Resource.ManageUsers)
            {
                LoadUser();
            }
            else if (currentTab.Text == Language.Resource.ManageConnections)
            {
                LoadConnection();
            }
            else if (currentTab.Text == Language.Resource.ManageEmails)
            {
                LoadMail();
            }
            else if (currentTab.Text == Language.Resource.ManageAccounts)
            {
                LoadAccount();
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            if (currentTab.Text == Language.Resource.ManageEmails)
            {
                frmMailUpdate mails = new frmMailUpdate();
                mails.Show();
            }
            else if (currentTab.Text == Language.Resource.ManageConnections)
            {
                frmConnectionUpdate connections = new frmConnectionUpdate();
                connections.Show();
            }
            else if (currentTab.Text == Language.Resource.ManageAccounts)
            {
                frmAccountUpdate accounts = new frmAccountUpdate();
                accounts.Show();
            }
            else if (currentTab.Text == Language.Resource.ManageUsers)
            {
                frmMain main = (frmMain)Application.OpenForms["frmMain"];
                main.Logout();

                this.Close();

                frmRegister register = new frmRegister();
                register.Show();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentTab != null)
            {
                string message = Language.Resource.Message;

                ListView lsView = (ListView)GetCurrentListView();
                if (lsView != null)
                {
                    if (lsView.SelectedItems.Count > 0)
                    {
                        if (currentTab.Name == "tabMails")
                        {
                            frmMailUpdate mail = new frmMailUpdate(lsView.SelectedItems[0].Text);
                            mail.Show();
                        }
                        else if (currentTab.Name == "tabConnections")
                        {
                            frmConnectionUpdate connection = new frmConnectionUpdate(lsView.SelectedItems[0].Text);
                            connection.Show();
                        }
                        else if (currentTab.Name == "tabAccounts")
                        {
                            frmAccountUpdate account = new frmAccountUpdate(lsView.SelectedItems[0].Text);
                            account.Show();
                        }
                        else if (currentTab.Name == "tabUsers")
                        {
                            frmUserAccount useraccount = new frmUserAccount(lsView.SelectedItems[0].Text);
                            useraccount.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show(Language.Resource.RowNotSelectedForUpdate, message);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentTab != null)
            {
                string message = Language.Resource.Message;

                ListView lsView = (ListView)GetCurrentListView();
                if (lsView != null)
                {
                    if (lsView.SelectedItems.Count > 0)
                    {
                        var result = MessageBox.Show(Language.Resource.DeleteConfirm, message, MessageBoxButtons.YesNo);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (currentTab.Name == "tabMails")
                            {
                                DeleteMail(lsView.SelectedItems[0].Text);
                            }
                            else if (currentTab.Name == "tabConnections")
                            {
                                DeleteConnection(lsView.SelectedItems[0].Text);
                            }
                            else if (currentTab.Name == "tabAccounts")
                            {
                                DeleteAccount(lsView.SelectedItems[0].Text);
                            }
                            else if (currentTab.Name == "tabUsers")
                            {
                                DeleteUser(lsView.SelectedItems[0].Text);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(Language.Resource.RowNotSelectedForDelete, message);
                    }
                }
            }
        }

        private object GetCurrentListView()
        {
            if (tabMain.SelectedTab != null)
            {
                if (tabMain.SelectedTab.Controls.Count > 0)
                {
                    Control ctr = tabMain.SelectedTab.Controls[0];
                    if (ctr != null)
                    {
                        return ctr as object;
                    }
                }
            }
            return null;
        }

        private void LoadUser()
        {
            User user = new User();
            List<User> listUser = user.GetAll();

            Security security = new Security();           

            lvUsers.Items.Clear();
            
            foreach (User usr in listUser)
            {
                ListViewItem item = new ListViewItem(new[] { usr.UserName, usr.Email, security.encrypt(User.Current.PublicKey, usr.Password), usr.PublicKey, usr.PrivateKey });
                lvUsers.Items.Add(item);
            }
        }

        private void LoadConnection()
        {
            Connection connection = new Connection();
            Security security = new Security();
            List<Connection> lstConnection = connection.GetAll();

            lvConnections.Items.Clear();

            foreach (Connection conn in lstConnection)
            {
                ListViewItem item = new ListViewItem(new[] { conn.Name, conn.Server, conn.Database, conn.Username, security.encrypt(User.Current.PublicKey, conn.Password), conn.Provider });
                lvConnections.Items.Add(item);
            }
        }

        private void LoadMail()
        {
            MailServer mailserver = new MailServer();
            List<MailServer> lstMail = mailserver.GetAll();

            Security security = new Security();

            lvMails.Items.Clear();

            foreach(MailServer m in lstMail)
            {
                ListViewItem item = new ListViewItem(new[] { m.Name, m.Server, m.Username, security.encrypt(User.Current.PublicKey, m.Password), m.Port.ToString() });
                lvMails.Items.Add(item);
            }
        }

        private void LoadAccount()
        {
            Account account = new Account();
            List<Account> lstAccount = new List<Account>();
            lstAccount = account.GetAll();

            lvAccounts.Items.Clear();
            Security security = new Security();

            foreach (Account acc in lstAccount)
            {
                ListViewItem item = new ListViewItem(new[] { acc.Name, acc.Username, security.encrypt(User.Current.PublicKey, acc.Password), acc.Description });
                lvAccounts.Items.Add(item);
            }
        }

        public void SelectTab(string name)
        {
            if (name == "tabMails")
            {
                LoadMail();
                tabMain.SelectTab(name);
            }
            else if (name == "tabConnections")
            {
                LoadConnection();
                tabMain.SelectTab(name);
            }
            else if (name == "tabAccounts")
            {
                LoadAccount();
                tabMain.SelectTab(name);
            }
            else if (name == "tabUsers")
            {
                LoadUser();
                tabMain.SelectTab(name);
            }
        }

        public void DeleteMail(string name)
        {
            MailServer mail = new MailServer();
            mail.Name = name;
            bool result = mail.Delete();
            if (result)
            {
                LoadMail();
            }
        }

        public void DeleteConnection(string name)
        {
            Connection connection = new Connection();
            connection.Name = name;
            bool result = connection.Delete();
            if (result)
            {
                LoadConnection();
            }
        }

        public void DeleteAccount(string name)
        {
            Account account = new Account();
            account.Name = name;
            bool result = account.Delete();
            if (result)
            {
                LoadAccount();
            }
        }

        public void DeleteUser(string name)
        {
            User user = new User();
            user.UserName = name;
            bool result = user.Delete();
            
            if (result)
            {
                frmMain main = (frmMain)Application.OpenForms["frmMain"];
                main.Logout();
                this.Close();
            }
        }

        public void LoadLanguage()
        {
            btnAddNew.Text = Language.Resource.AddNew;
            btnUpdate.Text = Language.Resource.Update;
            btnDelete.Text = Language.Resource.Delete;
            tabUsers.Text = Language.Resource.ManageUsers;
            tabConnections.Text = Language.Resource.ManageConnections;
            tabMails.Text = Language.Resource.ManageEmails;
            tabAccounts.Text = Language.Resource.ManageAccounts;
            clUsername.Text = Language.Resource.Username;
            clPassword.Text = Language.Resource.Password;
            clEmail.Text = Language.Resource.Email;
            clPublicKey.Text = Language.Resource.PublicKey;
            clPrivateKey.Text = Language.Resource.PrivateKey;
            clName.Text = Language.Resource.Name;
            clServer.Text = Language.Resource.Server;
            clUserMail.Text = Language.Resource.Username;
            clPasswordMail.Text = Language.Resource.Password;
            clPort.Text = Language.Resource.Port;
            clConnectionName.Text = Language.Resource.Name;
            clConnectionServer.Text = Language.Resource.Server;
            clConnectionUserName.Text = Language.Resource.Username;
            clConnectionPassword.Text = Language.Resource.Password;
            clDB.Text = Language.Resource.Database;
            clProvider.Text = Language.Resource.Provider;
            clAccountName.Text = Language.Resource.Name;
            clAccountUser.Text = Language.Resource.Username;
            clAccountPass.Text = Language.Resource.Password;
            clDescription.Text = Language.Resource.Description;
            this.Text = Language.Resource.Manager;
        }

    }
}
