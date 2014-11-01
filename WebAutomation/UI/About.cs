using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WebAutomation
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            this.Text = Language.Resource.About;
            this.label1.Text = Language.Resource.WebAutomation;
            this.lblDescription.Text = Language.Resource.AboutDescription;
            lblDescription.MaximumSize = new Size(480, 0);
            lblDescription.AutoSize = true;
        }

        private void lblWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.thangdc.com");
        }
    }
}
