using System;
using System.Windows.Forms;
using ThangDC.Core.Entities;
using ThangDC.Core.Securities;

namespace WebAutomation
{
    public partial class frmConnectionUpdate : Form
    {
        private bool _isAdd = false;
        private string _name;
        public frmConnectionUpdate()
        {
            _isAdd = true;
            InitializeComponent();
        }

        public frmConnectionUpdate(string name)
        {
            _name = name;
            _isAdd = false;

            InitializeComponent();

            if (User.Current != null)
            {
                Connection connection = new Connection();
                var currentConnection = connection.GetBy(name);

                if (currentConnection != null)
                {
                    tbxName.Text = currentConnection.Name;
                    tbxServer.Text = currentConnection.Server;
                    tbxDatabase.Text = currentConnection.Database;
                    tbxUser.Text = currentConnection.Username;
                    tbxPassword.Text = currentConnection.Password;
                    tbxProvider.Text = currentConnection.Provider;
                }
            }
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
                if (_isAdd)
                {
                    AddNew();
                }
                else
                {
                    UpdateConnection(_name);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmConnectionUpdate_Load(object sender, EventArgs e)
        {

            lblName.Text = Language.Resource.Name;
            lblServer.Text = Language.Resource.Server;
            lblDatabase.Text = Language.Resource.Database;
            lblUser.Text = Language.Resource.Username;
            lblPassword.Text = Language.Resource.Password;
            lblProvider.Text = Language.Resource.Provider;

            if (_isAdd)
            {
                tbxName.Enabled = true;
                this.Text = Language.Resource.AddNew;
                btnOK.Text = Language.Resource.AddNew;
            }
            else
            {
                tbxName.Enabled = false;
                this.Text = Language.Resource.Update;
                btnOK.Text = Language.Resource.Update;
            }

            btnCancel.Text = Language.Resource.Cancel;
        }

        private void AddNew()
        {
            var conn = new Connection();
            string message = Language.Resource.Message;
            bool check = conn.CheckExists(tbxName.Text);

            if (check)
            {
                MessageBox.Show(Language.Resource.NameExists, message);
            }
            else
            {
                conn.Name = tbxName.Text;
                conn.Server = tbxServer.Text;
                conn.Database = tbxDatabase.Text;
                conn.Username = tbxUser.Text;
                conn.Password = tbxPassword.Text;
                conn.Provider = tbxProvider.Text;

                int result = conn.Add();
                if (result == 1)
                {
                    var manager = (frmManager)Application.OpenForms["frmManager"];
                    manager.SelectTab("tabConnections");
                    Close();
                }
            }
        }

        public void UpdateConnection(string name)
        {
            Connection conn = new Connection();
            conn.Name = tbxName.Text;
            conn.Server = tbxServer.Text;
            conn.Database = tbxDatabase.Text;
            conn.Username = tbxUser.Text;
            conn.Password = tbxPassword.Text;
            conn.Provider = tbxProvider.Text;
            int result = conn.Update();
            if (result == 1)
            {
                var manager = (frmManager)Application.OpenForms["frmManager"];
                manager.SelectTab("tabConnections");
                Close();
            }
        }
    }
}
