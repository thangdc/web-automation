using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebAutomation.UI
{
    public partial class NewDownload : Form
    {
        public NewDownload()
        {
            InitializeComponent();
        }

        private void InitLanguage()
        {
            btnOK.Text = Language.Resource.OK;
            btnCancel.Text = Language.Resource.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxUrl.Text))
            {
                MessageBox.Show(Language.Resource.EmptyUrl, Language.Resource.Message);
            }
            else
            {
                frmMain main = (frmMain)Application.OpenForms["frmMain"];
                main.AddDownload("", tbxUrl.Text, "", 0);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
