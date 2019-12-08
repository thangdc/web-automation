using ScintillaNET;
using System.Drawing;

namespace WebAutomation
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressbar = new System.Windows.Forms.ToolStripProgressBar();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.developerToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scrollToViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cfgShowImages = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maintoolStrip = new System.Windows.Forms.ToolStrip();
            this.btnRunTwo = new System.Windows.Forms.ToolStripButton();
            this.btnRecord = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnNewTab = new System.Windows.Forms.ToolStripButton();
            this.btnCloseTab = new System.Windows.Forms.ToolStripButton();
            this.btnCloseAllTab = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnShowHideDeveloperTool = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDetection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRegconization = new System.Windows.Forms.ToolStripButton();
            this.sddLanguage = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnGo = new System.Windows.Forms.Button();
            this.tbxAddress = new System.Windows.Forms.TextBox();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabControlCode = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripRunning = new System.Windows.Forms.ToolStripButton();
            this.btnNewScript = new System.Windows.Forms.ToolStripButton();
            this.btnOpenScript = new System.Windows.Forms.ToolStripButton();
            this.btnSaveScript = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAsScript = new System.Windows.Forms.ToolStripButton();
            this.btnScriptClear = new System.Windows.Forms.ToolStripButton();
            this.tbxCode = new ScintillaNET.Scintilla();
            this.tabTemplate = new System.Windows.Forms.TabPage();
            this.tbxTemplate = new ScintillaNET.Scintilla();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnNewTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnOpenTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnSaveTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAsTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnTemplateClear = new System.Windows.Forms.ToolStripButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbxPreview = new ScintillaNET.Scintilla();
            this.tabDownload = new System.Windows.Forms.TabPage();
            this.downloadList1 = new WebAutomation.UI.DownloadList();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolNewDownload = new System.Windows.Forms.ToolStripButton();
            this.toolStart = new System.Windows.Forms.ToolStripButton();
            this.toolPause = new System.Windows.Forms.ToolStripButton();
            this.toolPauseAll = new System.Windows.Forms.ToolStripButton();
            this.toolRemove = new System.Windows.Forms.ToolStripButton();
            this.toolRemoveCompleted = new System.Windows.Forms.ToolStripButton();
            this.tbxAutoBot = new System.Windows.Forms.TabPage();
            this.tbxAsk = new System.Windows.Forms.TextBox();
            this.tbxAnswer = new System.Windows.Forms.RichTextBox();
            this.contextMenuBrowser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sleepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.attributeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.htmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.srcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.urlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fillToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dropdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.notifyIconAutomation = new System.Windows.Forms.NotifyIcon(this.components);
            this.mainStatusStrip.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.maintoolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControlCode.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabTemplate.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabDownload.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.tbxAutoBot.SuspendLayout();
            this.contextMenuBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus,
            this.progressbar});
            this.mainStatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 562);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(1003, 22);
            this.mainStatusStrip.TabIndex = 0;
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // progressbar
            // 
            this.progressbar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progressbar.Name = "progressbar";
            this.progressbar.Size = new System.Drawing.Size(100, 16);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.configToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(1003, 24);
            this.mainMenuStrip.TabIndex = 1;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newScriptToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator7,
            this.loginToolStripMenuItem,
            this.registerToolStripMenuItem,
            this.hideWindowToolStripMenuItem,
            this.toolStripSeparator8,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileToolStripMenuItem.Image")));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newScriptToolStripMenuItem
            // 
            this.newScriptToolStripMenuItem.Image = global::WebAutomation.Properties.Resources._new;
            this.newScriptToolStripMenuItem.Name = "newScriptToolStripMenuItem";
            this.newScriptToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newScriptToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newScriptToolStripMenuItem.Text = "New";
            this.newScriptToolStripMenuItem.Click += new System.EventHandler(this.newScriptToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::WebAutomation.Properties.Resources.open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::WebAutomation.Properties.Resources.save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::WebAutomation.Properties.Resources.save_as;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(143, 6);
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Image = global::WebAutomation.Properties.Resources.login;
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // registerToolStripMenuItem
            // 
            this.registerToolStripMenuItem.Image = global::WebAutomation.Properties.Resources.register;
            this.registerToolStripMenuItem.Name = "registerToolStripMenuItem";
            this.registerToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.registerToolStripMenuItem.Text = "Register";
            this.registerToolStripMenuItem.Click += new System.EventHandler(this.registerToolStripMenuItem_Click);
            // 
            // hideWindowToolStripMenuItem
            // 
            this.hideWindowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("hideWindowToolStripMenuItem.Image")));
            this.hideWindowToolStripMenuItem.Name = "hideWindowToolStripMenuItem";
            this.hideWindowToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.hideWindowToolStripMenuItem.Text = "Hide Window";
            this.hideWindowToolStripMenuItem.Click += new System.EventHandler(this.hideWindowToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.managerToolStripMenuItem,
            this.developerToolsToolStripMenuItem});
            this.toolsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("toolsToolStripMenuItem.Image")));
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // managerToolStripMenuItem
            // 
            this.managerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("managerToolStripMenuItem.Image")));
            this.managerToolStripMenuItem.Name = "managerToolStripMenuItem";
            this.managerToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.managerToolStripMenuItem.Text = "Manager";
            this.managerToolStripMenuItem.Click += new System.EventHandler(this.managerToolStripMenuItem_Click);
            // 
            // developerToolsToolStripMenuItem
            // 
            this.developerToolsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("developerToolsToolStripMenuItem.Image")));
            this.developerToolsToolStripMenuItem.Name = "developerToolsToolStripMenuItem";
            this.developerToolsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.developerToolsToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.developerToolsToolStripMenuItem.Text = "Developer Tools";
            this.developerToolsToolStripMenuItem.Click += new System.EventHandler(this.developerToolsToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scrollToViewToolStripMenuItem,
            this.colorElementToolStripMenuItem,
            this.cfgShowImages});
            this.configToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("configToolStripMenuItem.Image")));
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.configToolStripMenuItem.Text = "Configs";
            // 
            // scrollToViewToolStripMenuItem
            // 
            this.scrollToViewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("scrollToViewToolStripMenuItem.Image")));
            this.scrollToViewToolStripMenuItem.Name = "scrollToViewToolStripMenuItem";
            this.scrollToViewToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.scrollToViewToolStripMenuItem.Text = "Scroll To View";
            this.scrollToViewToolStripMenuItem.Click += new System.EventHandler(this.scrollToViewToolStripMenuItem_Click);
            // 
            // colorElementToolStripMenuItem
            // 
            this.colorElementToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("colorElementToolStripMenuItem.Image")));
            this.colorElementToolStripMenuItem.Name = "colorElementToolStripMenuItem";
            this.colorElementToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.colorElementToolStripMenuItem.Text = "Color Element";
            this.colorElementToolStripMenuItem.Click += new System.EventHandler(this.colorElementToolStripMenuItem_Click);
            // 
            // cfgShowImages
            // 
            this.cfgShowImages.Image = global::WebAutomation.Properties.Resources.images;
            this.cfgShowImages.Name = "cfgShowImages";
            this.cfgShowImages.Size = new System.Drawing.Size(149, 22);
            this.cfgShowImages.Text = "Show Images";
            this.cfgShowImages.Click += new System.EventHandler(this.cfgShowImages_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.checkForUpdateToolStripMenuItem});
            this.helpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripMenuItem.Image")));
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("checkForUpdateToolStripMenuItem.Image")));
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.checkForUpdateToolStripMenuItem.Text = "Check for Update";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdateToolStripMenuItem_Click);
            // 
            // maintoolStrip
            // 
            this.maintoolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRunTwo,
            this.btnRecord,
            this.toolStripSeparator6,
            this.toolStripBack,
            this.toolStripNext,
            this.toolStripReload,
            this.toolStripSeparator1,
            this.btnNewTab,
            this.btnCloseTab,
            this.btnCloseAllTab,
            this.toolStripSeparator2,
            this.btnShowHideDeveloperTool,
            this.toolStripSeparator3,
            this.btnDetection,
            this.toolStripSeparator4,
            this.btnRegconization,
            this.sddLanguage});
            this.maintoolStrip.Location = new System.Drawing.Point(0, 24);
            this.maintoolStrip.Name = "maintoolStrip";
            this.maintoolStrip.Size = new System.Drawing.Size(1003, 25);
            this.maintoolStrip.TabIndex = 2;
            this.maintoolStrip.Text = "toolStrip1";
            // 
            // btnRunTwo
            // 
            this.btnRunTwo.Image = ((System.Drawing.Image)(resources.GetObject("btnRunTwo.Image")));
            this.btnRunTwo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRunTwo.Name = "btnRunTwo";
            this.btnRunTwo.Size = new System.Drawing.Size(48, 22);
            this.btnRunTwo.Text = "Run";
            this.btnRunTwo.Click += new System.EventHandler(this.btnRunTwo_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnRecord.Image")));
            this.btnRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(64, 22);
            this.btnRecord.Text = "Record";
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripBack
            // 
            this.toolStripBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBack.Enabled = false;
            this.toolStripBack.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBack.Image")));
            this.toolStripBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBack.Name = "toolStripBack";
            this.toolStripBack.Size = new System.Drawing.Size(23, 22);
            this.toolStripBack.Text = "Back";
            this.toolStripBack.Click += new System.EventHandler(this.toolStripBack_Click);
            // 
            // toolStripNext
            // 
            this.toolStripNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripNext.Enabled = false;
            this.toolStripNext.Image = ((System.Drawing.Image)(resources.GetObject("toolStripNext.Image")));
            this.toolStripNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripNext.Name = "toolStripNext";
            this.toolStripNext.Size = new System.Drawing.Size(23, 22);
            this.toolStripNext.Text = "Next";
            this.toolStripNext.Click += new System.EventHandler(this.toolStripNext_Click);
            // 
            // toolStripReload
            // 
            this.toolStripReload.Image = ((System.Drawing.Image)(resources.GetObject("toolStripReload.Image")));
            this.toolStripReload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripReload.Name = "toolStripReload";
            this.toolStripReload.Size = new System.Drawing.Size(63, 22);
            this.toolStripReload.Text = "Reload";
            this.toolStripReload.Click += new System.EventHandler(this.toolStripReload_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnNewTab
            // 
            this.btnNewTab.Image = ((System.Drawing.Image)(resources.GetObject("btnNewTab.Image")));
            this.btnNewTab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewTab.Name = "btnNewTab";
            this.btnNewTab.Size = new System.Drawing.Size(73, 22);
            this.btnNewTab.Text = "New Tab";
            this.btnNewTab.Click += new System.EventHandler(this.btnNewTab_Click);
            // 
            // btnCloseTab
            // 
            this.btnCloseTab.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseTab.Image")));
            this.btnCloseTab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCloseTab.Name = "btnCloseTab";
            this.btnCloseTab.Size = new System.Drawing.Size(78, 22);
            this.btnCloseTab.Text = "Close Tab";
            this.btnCloseTab.Click += new System.EventHandler(this.btnCloseTab_Click);
            // 
            // btnCloseAllTab
            // 
            this.btnCloseAllTab.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseAllTab.Image")));
            this.btnCloseAllTab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCloseAllTab.Name = "btnCloseAllTab";
            this.btnCloseAllTab.Size = new System.Drawing.Size(95, 22);
            this.btnCloseAllTab.Text = "Close All Tab";
            this.btnCloseAllTab.Click += new System.EventHandler(this.btnCloseAllTab_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnShowHideDeveloperTool
            // 
            this.btnShowHideDeveloperTool.Image = ((System.Drawing.Image)(resources.GetObject("btnShowHideDeveloperTool.Image")));
            this.btnShowHideDeveloperTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowHideDeveloperTool.Name = "btnShowHideDeveloperTool";
            this.btnShowHideDeveloperTool.Size = new System.Drawing.Size(168, 22);
            this.btnShowHideDeveloperTool.Text = "Show/Hide Developer Tool";
            this.btnShowHideDeveloperTool.Click += new System.EventHandler(this.btnShowHideDeveloperTool_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDetection
            // 
            this.btnDetection.Image = ((System.Drawing.Image)(resources.GetObject("btnDetection.Image")));
            this.btnDetection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDetection.Name = "btnDetection";
            this.btnDetection.Size = new System.Drawing.Size(110, 22);
            this.btnDetection.Text = "Show Detection";
            this.btnDetection.Click += new System.EventHandler(this.btnDetection_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRegconization
            // 
            this.btnRegconization.Image = global::WebAutomation.Properties.Resources.speech;
            this.btnRegconization.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegconization.Name = "btnRegconization";
            this.btnRegconization.Size = new System.Drawing.Size(102, 22);
            this.btnRegconization.Text = "Regconization";
            this.btnRegconization.Click += new System.EventHandler(this.btnRegconization_Click);
            // 
            // sddLanguage
            // 
            this.sddLanguage.Image = global::WebAutomation.Properties.Resources.usa;
            this.sddLanguage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sddLanguage.Name = "sddLanguage";
            this.sddLanguage.Size = new System.Drawing.Size(65, 22);
            this.sddLanguage.Text = "English";
            this.sddLanguage.Click += new System.EventHandler(this.sddLanguage_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlCode);
            this.splitContainer1.Size = new System.Drawing.Size(1003, 513);
            this.splitContainer1.SplitterDistance = 310;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnGo);
            this.splitContainer2.Panel1.Controls.Add(this.tbxAddress);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabMain);
            this.splitContainer2.Size = new System.Drawing.Size(1003, 310);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(925, 0);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // tbxAddress
            // 
            this.tbxAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxAddress.Location = new System.Drawing.Point(0, 2);
            this.tbxAddress.Name = "tbxAddress";
            this.tbxAddress.Size = new System.Drawing.Size(919, 20);
            this.tbxAddress.TabIndex = 0;
            this.tbxAddress.Text = "http://www.google.com.vn";
            this.tbxAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxAddress_KeyDown);
            // 
            // tabMain
            // 
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1003, 281);
            this.tabMain.TabIndex = 0;
            this.tabMain.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabMain_Selected);
            // 
            // tabControlCode
            // 
            this.tabControlCode.Controls.Add(this.tabPage1);
            this.tabControlCode.Controls.Add(this.tabTemplate);
            this.tabControlCode.Controls.Add(this.tabPage2);
            this.tabControlCode.Controls.Add(this.tabDownload);
            this.tabControlCode.Controls.Add(this.tbxAutoBot);
            this.tabControlCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCode.Location = new System.Drawing.Point(0, 0);
            this.tabControlCode.Name = "tabControlCode";
            this.tabControlCode.SelectedIndex = 0;
            this.tabControlCode.Size = new System.Drawing.Size(1003, 199);
            this.tabControlCode.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(995, 173);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Code";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tbxCode);
            this.splitContainer3.Size = new System.Drawing.Size(989, 167);
            this.splitContainer3.SplitterDistance = 25;
            this.splitContainer3.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRunning,
            this.btnNewScript,
            this.btnOpenScript,
            this.btnSaveScript,
            this.btnSaveAsScript,
            this.btnScriptClear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(989, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripRunning
            // 
            this.toolStripRunning.Image = ((System.Drawing.Image)(resources.GetObject("toolStripRunning.Image")));
            this.toolStripRunning.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRunning.Name = "toolStripRunning";
            this.toolStripRunning.Size = new System.Drawing.Size(48, 22);
            this.toolStripRunning.Text = "Run";
            this.toolStripRunning.Click += new System.EventHandler(this.toolStripRunning_Click);
            // 
            // btnNewScript
            // 
            this.btnNewScript.Image = ((System.Drawing.Image)(resources.GetObject("btnNewScript.Image")));
            this.btnNewScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewScript.Name = "btnNewScript";
            this.btnNewScript.Size = new System.Drawing.Size(51, 22);
            this.btnNewScript.Text = "New";
            this.btnNewScript.Click += new System.EventHandler(this.btnNewScript_Click);
            // 
            // btnOpenScript
            // 
            this.btnOpenScript.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenScript.Image")));
            this.btnOpenScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenScript.Name = "btnOpenScript";
            this.btnOpenScript.Size = new System.Drawing.Size(56, 22);
            this.btnOpenScript.Text = "Open";
            this.btnOpenScript.Click += new System.EventHandler(this.btnOpenScript_Click);
            // 
            // btnSaveScript
            // 
            this.btnSaveScript.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveScript.Image")));
            this.btnSaveScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveScript.Name = "btnSaveScript";
            this.btnSaveScript.Size = new System.Drawing.Size(51, 22);
            this.btnSaveScript.Text = "Save";
            this.btnSaveScript.Click += new System.EventHandler(this.btnSaveScript_Click);
            // 
            // btnSaveAsScript
            // 
            this.btnSaveAsScript.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAsScript.Image")));
            this.btnSaveAsScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAsScript.Name = "btnSaveAsScript";
            this.btnSaveAsScript.Size = new System.Drawing.Size(65, 22);
            this.btnSaveAsScript.Text = "Save as";
            this.btnSaveAsScript.Click += new System.EventHandler(this.btnSaveAsScript_Click);
            // 
            // btnScriptClear
            // 
            this.btnScriptClear.Image = ((System.Drawing.Image)(resources.GetObject("btnScriptClear.Image")));
            this.btnScriptClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnScriptClear.Name = "btnScriptClear";
            this.btnScriptClear.Size = new System.Drawing.Size(54, 22);
            this.btnScriptClear.Text = "Clear";
            this.btnScriptClear.Click += new System.EventHandler(this.btnScriptClear_Click);
            // 
            // tbxCode
            // 
            
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            this.tbxCode.StyleResetDefault();
            this.tbxCode.Styles[Style.Default].Font = "Consolas";
            this.tbxCode.Styles[Style.Default].Size = 10;
            this.tbxCode.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            this.tbxCode.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            this.tbxCode.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            this.tbxCode.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            this.tbxCode.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            this.tbxCode.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            this.tbxCode.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            this.tbxCode.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            this.tbxCode.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            this.tbxCode.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            this.tbxCode.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            this.tbxCode.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            this.tbxCode.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            this.tbxCode.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            this.tbxCode.Margins[0].Width = 35;
            this.tbxCode.Lexer = ScintillaNET.Lexer.Cpp;

            this.tbxCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxCode.Location = new System.Drawing.Point(0, 0);
            //this.tbxCode.Multiline = true;
            this.tbxCode.Name = "tbxCode";
            this.tbxCode.Size = new System.Drawing.Size(989, 138);
            this.tbxCode.TabIndex = 0;
            // 
            // tabTemplate
            // 
            this.tabTemplate.Controls.Add(this.tbxTemplate);
            this.tabTemplate.Controls.Add(this.toolStrip2);
            this.tabTemplate.Location = new System.Drawing.Point(4, 22);
            this.tabTemplate.Name = "tabTemplate";
            this.tabTemplate.Padding = new System.Windows.Forms.Padding(3);
            this.tabTemplate.Size = new System.Drawing.Size(995, 173);
            this.tabTemplate.TabIndex = 2;
            this.tabTemplate.Text = "Template";
            this.tabTemplate.UseVisualStyleBackColor = true;
            // 
            // tbxTemplate
            // 

            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            this.tbxTemplate.StyleResetDefault();
            this.tbxTemplate.Styles[Style.Default].Font = "Consolas";
            this.tbxTemplate.Styles[Style.Default].Size = 10;
            this.tbxTemplate.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            this.tbxTemplate.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            this.tbxTemplate.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            this.tbxTemplate.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            this.tbxTemplate.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            this.tbxTemplate.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            this.tbxTemplate.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            this.tbxTemplate.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            this.tbxTemplate.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            this.tbxTemplate.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            this.tbxTemplate.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            this.tbxTemplate.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            this.tbxTemplate.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            this.tbxTemplate.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            this.tbxTemplate.Margins[0].Width = 35;
            this.tbxTemplate.Lexer = ScintillaNET.Lexer.Cpp;

            this.tbxTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxTemplate.Location = new System.Drawing.Point(3, 28);
            //this.tbxTemplate.Multiline = true;
            this.tbxTemplate.Name = "tbxTemplate";
            this.tbxTemplate.Size = new System.Drawing.Size(989, 142);
            this.tbxTemplate.TabIndex = 2;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewTemplate,
            this.btnOpenTemplate,
            this.btnSaveTemplate,
            this.btnSaveAsTemplate,
            this.btnTemplateClear});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(989, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnNewTemplate
            // 
            this.btnNewTemplate.Image = ((System.Drawing.Image)(resources.GetObject("btnNewTemplate.Image")));
            this.btnNewTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewTemplate.Name = "btnNewTemplate";
            this.btnNewTemplate.Size = new System.Drawing.Size(51, 22);
            this.btnNewTemplate.Text = "New";
            this.btnNewTemplate.Click += new System.EventHandler(this.btnNewTemplate_Click);
            // 
            // btnOpenTemplate
            // 
            this.btnOpenTemplate.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenTemplate.Image")));
            this.btnOpenTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenTemplate.Name = "btnOpenTemplate";
            this.btnOpenTemplate.Size = new System.Drawing.Size(56, 22);
            this.btnOpenTemplate.Text = "Open";
            this.btnOpenTemplate.Click += new System.EventHandler(this.btnOpenTemplate_Click);
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveTemplate.Image")));
            this.btnSaveTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(51, 22);
            this.btnSaveTemplate.Text = "Save";
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // btnSaveAsTemplate
            // 
            this.btnSaveAsTemplate.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAsTemplate.Image")));
            this.btnSaveAsTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAsTemplate.Name = "btnSaveAsTemplate";
            this.btnSaveAsTemplate.Size = new System.Drawing.Size(65, 22);
            this.btnSaveAsTemplate.Text = "Save as";
            this.btnSaveAsTemplate.Click += new System.EventHandler(this.btnSaveAsTemplate_Click);
            // 
            // btnTemplateClear
            // 
            this.btnTemplateClear.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplateClear.Image")));
            this.btnTemplateClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTemplateClear.Name = "btnTemplateClear";
            this.btnTemplateClear.Size = new System.Drawing.Size(54, 22);
            this.btnTemplateClear.Text = "Clear";
            this.btnTemplateClear.Click += new System.EventHandler(this.btnTemplateClear_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbxPreview);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(995, 173);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Preview";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbxPreview
            // 

            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            this.tbxPreview.StyleResetDefault();
            this.tbxPreview.Styles[Style.Default].Font = "Consolas";
            this.tbxPreview.Styles[Style.Default].Size = 10;
            this.tbxPreview.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            this.tbxPreview.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            this.tbxPreview.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            this.tbxPreview.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            this.tbxPreview.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            this.tbxPreview.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            this.tbxPreview.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            this.tbxPreview.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            this.tbxPreview.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            this.tbxPreview.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            this.tbxPreview.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            this.tbxPreview.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            this.tbxPreview.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            this.tbxPreview.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            this.tbxPreview.Margins[0].Width = 35;
            this.tbxPreview.Lexer = ScintillaNET.Lexer.Cpp;

            this.tbxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxPreview.Location = new System.Drawing.Point(3, 3);
            //this.tbxPreview.Multiline = true;
            this.tbxPreview.Name = "tbxPreview";
            this.tbxPreview.Size = new System.Drawing.Size(989, 167);
            this.tbxPreview.TabIndex = 0;
            // 
            // tabDownload
            // 
            this.tabDownload.Controls.Add(this.downloadList1);
            this.tabDownload.Controls.Add(this.toolStrip3);
            this.tabDownload.Location = new System.Drawing.Point(4, 22);
            this.tabDownload.Name = "tabDownload";
            this.tabDownload.Size = new System.Drawing.Size(995, 173);
            this.tabDownload.TabIndex = 4;
            this.tabDownload.Text = "Download";
            this.tabDownload.UseVisualStyleBackColor = true;
            // 
            // downloadList1
            // 
            this.downloadList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downloadList1.IsSelected = false;
            this.downloadList1.Location = new System.Drawing.Point(0, 25);
            this.downloadList1.Name = "downloadList1";
            this.downloadList1.Size = new System.Drawing.Size(995, 148);
            this.downloadList1.TabIndex = 2;
            // 
            // toolStrip3
            // 
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNewDownload,
            this.toolStart,
            this.toolPause,
            this.toolPauseAll,
            this.toolRemove,
            this.toolRemoveCompleted});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(995, 25);
            this.toolStrip3.TabIndex = 1;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolNewDownload
            // 
            this.toolNewDownload.Image = global::WebAutomation.Properties.Resources.document_text_add;
            this.toolNewDownload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNewDownload.Name = "toolNewDownload";
            this.toolNewDownload.Size = new System.Drawing.Size(108, 22);
            this.toolNewDownload.Text = "New Download";
            this.toolNewDownload.Click += new System.EventHandler(this.toolNewDownload_Click);
            // 
            // toolStart
            // 
            this.toolStart.Image = global::WebAutomation.Properties.Resources.download_run;
            this.toolStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStart.Name = "toolStart";
            this.toolStart.Size = new System.Drawing.Size(51, 22);
            this.toolStart.Text = "Start";
            this.toolStart.Click += new System.EventHandler(this.toolStart_Click);
            // 
            // toolPause
            // 
            this.toolPause.Image = global::WebAutomation.Properties.Resources.download_pause;
            this.toolPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPause.Name = "toolPause";
            this.toolPause.Size = new System.Drawing.Size(58, 22);
            this.toolPause.Text = "Pause";
            this.toolPause.Click += new System.EventHandler(this.toolPause_Click);
            // 
            // toolPauseAll
            // 
            this.toolPauseAll.Image = global::WebAutomation.Properties.Resources.firewall_pause;
            this.toolPauseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPauseAll.Name = "toolPauseAll";
            this.toolPauseAll.Size = new System.Drawing.Size(75, 22);
            this.toolPauseAll.Text = "Pause All";
            this.toolPauseAll.Click += new System.EventHandler(this.toolPauseAll_Click);
            // 
            // toolRemove
            // 
            this.toolRemove.Image = global::WebAutomation.Properties.Resources.delete;
            this.toolRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRemove.Name = "toolRemove";
            this.toolRemove.Size = new System.Drawing.Size(70, 22);
            this.toolRemove.Text = "Remove";
            this.toolRemove.Click += new System.EventHandler(this.toolRemove_Click);
            // 
            // toolRemoveCompleted
            // 
            this.toolRemoveCompleted.Image = global::WebAutomation.Properties.Resources.server_cancel;
            this.toolRemoveCompleted.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRemoveCompleted.Name = "toolRemoveCompleted";
            this.toolRemoveCompleted.Size = new System.Drawing.Size(132, 22);
            this.toolRemoveCompleted.Text = "Remove Completed";
            this.toolRemoveCompleted.Click += new System.EventHandler(this.toolRemoveCompleted_Click);
            // 
            // tbxAutoBot
            // 
            this.tbxAutoBot.Controls.Add(this.tbxAsk);
            this.tbxAutoBot.Controls.Add(this.tbxAnswer);
            this.tbxAutoBot.Location = new System.Drawing.Point(4, 22);
            this.tbxAutoBot.Name = "tbxAutoBot";
            this.tbxAutoBot.Padding = new System.Windows.Forms.Padding(3);
            this.tbxAutoBot.Size = new System.Drawing.Size(995, 173);
            this.tbxAutoBot.TabIndex = 3;
            this.tbxAutoBot.Text = "Auto bot";
            this.tbxAutoBot.UseVisualStyleBackColor = true;
            // 
            // tbxAsk
            // 
            this.tbxAsk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxAsk.Location = new System.Drawing.Point(0, 152);
            this.tbxAsk.Name = "tbxAsk";
            this.tbxAsk.Size = new System.Drawing.Size(972, 20);
            this.tbxAsk.TabIndex = 1;
            this.tbxAsk.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxAsk_KeyDown);
            // 
            // tbxAnswer
            // 
            this.tbxAnswer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxAnswer.Location = new System.Drawing.Point(-1, 0);
            this.tbxAnswer.Name = "tbxAnswer";
            this.tbxAnswer.Size = new System.Drawing.Size(974, 154);
            this.tbxAnswer.TabIndex = 0;
            this.tbxAnswer.Text = "";
            // 
            // contextMenuBrowser
            // 
            this.contextMenuBrowser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToolStripMenuItem,
            this.sleepToolStripMenuItem,
            this.extractToolStripMenuItem1,
            this.fillToolStripMenuItem1});
            this.contextMenuBrowser.Name = "contextMenuBrowser";
            this.contextMenuBrowser.Size = new System.Drawing.Size(110, 92);
            // 
            // goToolStripMenuItem
            // 
            this.goToolStripMenuItem.Name = "goToolStripMenuItem";
            this.goToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.goToolStripMenuItem.Text = "Go";
            // 
            // sleepToolStripMenuItem
            // 
            this.sleepToolStripMenuItem.Name = "sleepToolStripMenuItem";
            this.sleepToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.sleepToolStripMenuItem.Text = "Sleep";
            // 
            // extractToolStripMenuItem1
            // 
            this.extractToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.attributeToolStripMenuItem,
            this.htmlToolStripMenuItem,
            this.srcToolStripMenuItem,
            this.textToolStripMenuItem,
            this.urlToolStripMenuItem});
            this.extractToolStripMenuItem1.Name = "extractToolStripMenuItem1";
            this.extractToolStripMenuItem1.Size = new System.Drawing.Size(109, 22);
            this.extractToolStripMenuItem1.Text = "Extract";
            // 
            // attributeToolStripMenuItem
            // 
            this.attributeToolStripMenuItem.Name = "attributeToolStripMenuItem";
            this.attributeToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.attributeToolStripMenuItem.Text = "Attribute";
            // 
            // htmlToolStripMenuItem
            // 
            this.htmlToolStripMenuItem.Name = "htmlToolStripMenuItem";
            this.htmlToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.htmlToolStripMenuItem.Text = "Html";
            // 
            // srcToolStripMenuItem
            // 
            this.srcToolStripMenuItem.Name = "srcToolStripMenuItem";
            this.srcToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.srcToolStripMenuItem.Text = "Src";
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.textToolStripMenuItem.Text = "Text";
            // 
            // urlToolStripMenuItem
            // 
            this.urlToolStripMenuItem.Name = "urlToolStripMenuItem";
            this.urlToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.urlToolStripMenuItem.Text = "Url";
            // 
            // fillToolStripMenuItem1
            // 
            this.fillToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textToolStripMenuItem1,
            this.dropdownToolStripMenuItem,
            this.iFrameToolStripMenuItem});
            this.fillToolStripMenuItem1.Name = "fillToolStripMenuItem1";
            this.fillToolStripMenuItem1.Size = new System.Drawing.Size(109, 22);
            this.fillToolStripMenuItem1.Text = "Fill";
            // 
            // textToolStripMenuItem1
            // 
            this.textToolStripMenuItem1.Name = "textToolStripMenuItem1";
            this.textToolStripMenuItem1.Size = new System.Drawing.Size(130, 22);
            this.textToolStripMenuItem1.Text = "Textbox";
            // 
            // dropdownToolStripMenuItem
            // 
            this.dropdownToolStripMenuItem.Name = "dropdownToolStripMenuItem";
            this.dropdownToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.dropdownToolStripMenuItem.Text = "Dropdown";
            // 
            // iFrameToolStripMenuItem
            // 
            this.iFrameToolStripMenuItem.Name = "iFrameToolStripMenuItem";
            this.iFrameToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.iFrameToolStripMenuItem.Text = "iFrame";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // notifyIconAutomation
            // 
            this.notifyIconAutomation.BalloonTipText = "Still working";
            this.notifyIconAutomation.BalloonTipTitle = "Web Automation";
            this.notifyIconAutomation.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconAutomation.Icon")));
            this.notifyIconAutomation.Text = "Web Automation";
            this.notifyIconAutomation.Visible = true;
            this.notifyIconAutomation.Click += new System.EventHandler(this.notifyIconAutomation_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 584);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.maintoolStrip);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web Automation - Version 1.0.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.maintoolStrip.ResumeLayout(false);
            this.maintoolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControlCode.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabTemplate.ResumeLayout(false);
            this.tabTemplate.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabDownload.ResumeLayout(false);
            this.tabDownload.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.tbxAutoBot.ResumeLayout(false);
            this.tbxAutoBot.PerformLayout();
            this.contextMenuBrowser.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStrip maintoolStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem developerToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripBack;
        private System.Windows.Forms.ToolStripButton toolStripNext;
        private System.Windows.Forms.ToolStripButton toolStripReload;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox tbxAddress;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabControl tabControlCode;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripRunning;
        private ScintillaNET.Scintilla tbxCode;
        private ScintillaNET.Scintilla tbxPreview;
        private System.Windows.Forms.ContextMenuStrip contextMenuBrowser;
        private System.Windows.Forms.ToolStripMenuItem goToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sleepToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripButton btnSaveScript;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabPage tabTemplate;
        //private System.Windows.Forms.TextBox tbxTemplate;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnOpenTemplate;
        private System.Windows.Forms.ToolStripButton btnSaveTemplate;
        private System.Windows.Forms.ToolStripButton btnNewTemplate;
        private System.Windows.Forms.ToolStripButton btnNewScript;
        private System.Windows.Forms.ToolStripButton btnSaveAsTemplate;
        private System.Windows.Forms.ToolStripButton btnSaveAsScript;
        private System.Windows.Forms.ToolStripButton btnShowHideDeveloperTool;
        private System.Windows.Forms.ToolStripButton btnNewTab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnCloseTab;
        private System.Windows.Forms.ToolStripButton btnCloseAllTab;
        private System.Windows.Forms.ToolStripButton btnOpenScript;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnDetection;
        private System.Windows.Forms.ToolStripButton btnScriptClear;
        private System.Windows.Forms.ToolStripButton btnTemplateClear;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scrollToViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorElementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.TabPage tbxAutoBot;
        private System.Windows.Forms.TextBox tbxAsk;
        private System.Windows.Forms.RichTextBox tbxAnswer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton sddLanguage;
        private System.Windows.Forms.ToolStripMenuItem managerToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnRunTwo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnRecord;
        private System.Windows.Forms.NotifyIcon notifyIconAutomation;
        private System.Windows.Forms.ToolStripMenuItem hideWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem registerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnRegconization;
        private System.Windows.Forms.ToolStripProgressBar progressbar;
        private System.Windows.Forms.ToolStripMenuItem cfgShowImages;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem attributeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem htmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem srcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem urlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fillToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dropdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iFrameToolStripMenuItem;
        private System.Windows.Forms.TabPage tabDownload;
        private UI.DownloadList downloadList1;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStart;
        private System.Windows.Forms.ToolStripButton toolPause;
        private System.Windows.Forms.ToolStripButton toolPauseAll;
        private System.Windows.Forms.ToolStripButton toolRemove;
        private System.Windows.Forms.ToolStripButton toolRemoveCompleted;
        private System.Windows.Forms.ToolStripButton toolNewDownload;
        private ScintillaNET.Scintilla tbxTemplate;
    }
}

