namespace WebAutomation
{
    partial class frmManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManager));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddNew = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.tabConnections = new System.Windows.Forms.TabPage();
            this.lvConnections = new System.Windows.Forms.ListView();
            this.clConnectionName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clConnectionServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clDB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clConnectionUserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clConnectionPassword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clProvider = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabMails = new System.Windows.Forms.TabPage();
            this.lvMails = new System.Windows.Forms.ListView();
            this.clName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clUserMail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clPasswordMail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.lvUsers = new System.Windows.Forms.ListView();
            this.clUsername = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clEmail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clPassword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clPublicKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clPrivateKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabAccounts = new System.Windows.Forms.TabPage();
            this.lvAccounts = new System.Windows.Forms.ListView();
            this.clAccountName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clAccountUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clAccountPass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.tabConnections.SuspendLayout();
            this.tabMails.SuspendLayout();
            this.tabUsers.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabAccounts.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddNew,
            this.btnUpdate,
            this.btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(568, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddNew
            // 
            this.btnAddNew.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNew.Image")));
            this.btnAddNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(76, 22);
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(65, 22);
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // tabConnections
            // 
            this.tabConnections.Controls.Add(this.lvConnections);
            this.tabConnections.Location = new System.Drawing.Point(4, 22);
            this.tabConnections.Name = "tabConnections";
            this.tabConnections.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnections.Size = new System.Drawing.Size(560, 381);
            this.tabConnections.TabIndex = 2;
            this.tabConnections.Text = "Connections";
            this.tabConnections.UseVisualStyleBackColor = true;
            // 
            // lvConnections
            // 
            this.lvConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clConnectionName,
            this.clConnectionServer,
            this.clDB,
            this.clConnectionUserName,
            this.clConnectionPassword,
            this.clProvider});
            this.lvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvConnections.FullRowSelect = true;
            this.lvConnections.Location = new System.Drawing.Point(3, 3);
            this.lvConnections.Name = "lvConnections";
            this.lvConnections.Size = new System.Drawing.Size(554, 375);
            this.lvConnections.TabIndex = 0;
            this.lvConnections.UseCompatibleStateImageBehavior = false;
            this.lvConnections.View = System.Windows.Forms.View.Details;
            // 
            // clConnectionName
            // 
            this.clConnectionName.Text = "Name";
            this.clConnectionName.Width = 80;
            // 
            // clConnectionServer
            // 
            this.clConnectionServer.Text = "Server";
            this.clConnectionServer.Width = 120;
            // 
            // clDB
            // 
            this.clDB.Text = "Database";
            this.clDB.Width = 90;
            // 
            // clConnectionUserName
            // 
            this.clConnectionUserName.Text = "User";
            this.clConnectionUserName.Width = 100;
            // 
            // clConnectionPassword
            // 
            this.clConnectionPassword.Text = "Password";
            // 
            // clProvider
            // 
            this.clProvider.Text = "Provider";
            this.clProvider.Width = 100;
            // 
            // tabMails
            // 
            this.tabMails.Controls.Add(this.lvMails);
            this.tabMails.Location = new System.Drawing.Point(4, 22);
            this.tabMails.Name = "tabMails";
            this.tabMails.Padding = new System.Windows.Forms.Padding(3);
            this.tabMails.Size = new System.Drawing.Size(560, 381);
            this.tabMails.TabIndex = 1;
            this.tabMails.Text = "Mails";
            this.tabMails.UseVisualStyleBackColor = true;
            // 
            // lvMails
            // 
            this.lvMails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clName,
            this.clServer,
            this.clUserMail,
            this.clPasswordMail,
            this.clPort});
            this.lvMails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMails.FullRowSelect = true;
            this.lvMails.Location = new System.Drawing.Point(3, 3);
            this.lvMails.Name = "lvMails";
            this.lvMails.Size = new System.Drawing.Size(554, 375);
            this.lvMails.TabIndex = 0;
            this.lvMails.UseCompatibleStateImageBehavior = false;
            this.lvMails.View = System.Windows.Forms.View.Details;
            // 
            // clName
            // 
            this.clName.Text = "Name";
            this.clName.Width = 90;
            // 
            // clServer
            // 
            this.clServer.Text = "Server";
            this.clServer.Width = 140;
            // 
            // clUserMail
            // 
            this.clUserMail.Text = "User";
            this.clUserMail.Width = 120;
            // 
            // clPasswordMail
            // 
            this.clPasswordMail.Text = "Password";
            this.clPasswordMail.Width = 140;
            // 
            // clPort
            // 
            this.clPort.Text = "Port";
            // 
            // tabUsers
            // 
            this.tabUsers.Controls.Add(this.lvUsers);
            this.tabUsers.Location = new System.Drawing.Point(4, 22);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsers.Size = new System.Drawing.Size(560, 381);
            this.tabUsers.TabIndex = 0;
            this.tabUsers.Text = "Users";
            this.tabUsers.UseVisualStyleBackColor = true;
            // 
            // lvUsers
            // 
            this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clUsername,
            this.clEmail,
            this.clPassword,
            this.clPublicKey,
            this.clPrivateKey});
            this.lvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.Location = new System.Drawing.Point(3, 3);
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(554, 375);
            this.lvUsers.TabIndex = 0;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            // 
            // clUsername
            // 
            this.clUsername.Text = "Username";
            this.clUsername.Width = 95;
            // 
            // clEmail
            // 
            this.clEmail.Text = "Email";
            this.clEmail.Width = 110;
            // 
            // clPassword
            // 
            this.clPassword.Text = "Password";
            this.clPassword.Width = 114;
            // 
            // clPublicKey
            // 
            this.clPublicKey.Text = "Public Key";
            this.clPublicKey.Width = 115;
            // 
            // clPrivateKey
            // 
            this.clPrivateKey.Text = "Private Key";
            this.clPrivateKey.Width = 112;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabUsers);
            this.tabMain.Controls.Add(this.tabMails);
            this.tabMain.Controls.Add(this.tabConnections);
            this.tabMain.Controls.Add(this.tabAccounts);
            this.tabMain.Location = new System.Drawing.Point(0, 28);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(568, 407);
            this.tabMain.TabIndex = 1;
            this.tabMain.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabMain_Selected);
            // 
            // tabAccounts
            // 
            this.tabAccounts.Controls.Add(this.lvAccounts);
            this.tabAccounts.Location = new System.Drawing.Point(4, 22);
            this.tabAccounts.Name = "tabAccounts";
            this.tabAccounts.Padding = new System.Windows.Forms.Padding(3);
            this.tabAccounts.Size = new System.Drawing.Size(560, 381);
            this.tabAccounts.TabIndex = 3;
            this.tabAccounts.Text = "Accounts";
            this.tabAccounts.UseVisualStyleBackColor = true;
            // 
            // lvAccounts
            // 
            this.lvAccounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clAccountName,
            this.clAccountUser,
            this.clAccountPass,
            this.clDescription});
            this.lvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvAccounts.FullRowSelect = true;
            this.lvAccounts.Location = new System.Drawing.Point(3, 3);
            this.lvAccounts.Name = "lvAccounts";
            this.lvAccounts.Size = new System.Drawing.Size(554, 375);
            this.lvAccounts.TabIndex = 0;
            this.lvAccounts.UseCompatibleStateImageBehavior = false;
            this.lvAccounts.View = System.Windows.Forms.View.Details;
            // 
            // clAccountName
            // 
            this.clAccountName.Text = "Name";
            this.clAccountName.Width = 120;
            // 
            // clAccountUser
            // 
            this.clAccountUser.Text = "Username";
            this.clAccountUser.Width = 140;
            // 
            // clAccountPass
            // 
            this.clAccountPass.Text = "Password";
            this.clAccountPass.Width = 120;
            // 
            // clDescription
            // 
            this.clDescription.Text = "Description";
            this.clDescription.Width = 170;
            // 
            // frmManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 435);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabMain);
            this.Name = "frmManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manager";
            this.Load += new System.EventHandler(this.frmManager_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabConnections.ResumeLayout(false);
            this.tabMails.ResumeLayout(false);
            this.tabUsers.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabAccounts.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddNew;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.TabPage tabConnections;
        private System.Windows.Forms.ListView lvConnections;
        private System.Windows.Forms.ColumnHeader clConnectionName;
        private System.Windows.Forms.ColumnHeader clConnectionServer;
        private System.Windows.Forms.ColumnHeader clDB;
        private System.Windows.Forms.ColumnHeader clConnectionUserName;
        private System.Windows.Forms.ColumnHeader clConnectionPassword;
        private System.Windows.Forms.ColumnHeader clProvider;
        private System.Windows.Forms.TabPage tabMails;
        private System.Windows.Forms.ListView lvMails;
        private System.Windows.Forms.ColumnHeader clName;
        private System.Windows.Forms.ColumnHeader clServer;
        private System.Windows.Forms.ColumnHeader clUserMail;
        private System.Windows.Forms.ColumnHeader clPasswordMail;
        private System.Windows.Forms.ColumnHeader clPort;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.ListView lvUsers;
        private System.Windows.Forms.ColumnHeader clUsername;
        private System.Windows.Forms.ColumnHeader clEmail;
        private System.Windows.Forms.ColumnHeader clPassword;
        private System.Windows.Forms.ColumnHeader clPublicKey;
        private System.Windows.Forms.ColumnHeader clPrivateKey;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabAccounts;
        private System.Windows.Forms.ListView lvAccounts;
        private System.Windows.Forms.ColumnHeader clAccountName;
        private System.Windows.Forms.ColumnHeader clAccountUser;
        private System.Windows.Forms.ColumnHeader clAccountPass;
        private System.Windows.Forms.ColumnHeader clDescription;

    }
}