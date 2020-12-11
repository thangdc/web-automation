using System.Windows.Forms;
using System;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Gecko;
using System.Text;
using ThangDC.Core.Entities;
using System.Web.Script.Serialization;
using WebAutomation.App_Code;
using Gecko.DOM;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using WebAutomation.UI;
using System.Configuration;
using WebAutomation.SingleInstancing;
using System.Threading.Tasks;

namespace WebAutomation
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public partial class frmMain : Form, ISingleInstanceEnforcer
    {
        #region static variable

        private bool IsStop = false;
        private TabPage currentTab = null;

        //WebBrowser Main
        private WebBrowser wbMain;
        private GeckoHtmlElement htmlElm;

        private string LastScriptFile = "";
        private string LastTemplateFile = "";

        private User CurrentUser = null;
        public string Version = Application.ProductVersion;

        public string MaxWait = string.Empty;

        private bool IsBreakSleep = false;

        private string arguments = string.Empty;

        #endregion

        #region Mouse Keyboard Library

        private int lastTimeRecorded = 0;
        private MouseKeyboardLibrary.MouseHook mouseHook = new MouseKeyboardLibrary.MouseHook();
        private MouseKeyboardLibrary.KeyboardHook keyboardHook = new MouseKeyboardLibrary.KeyboardHook();

        #endregion

        #region Mouse Keyboard Init Event

        public void InitMouseKeyBoardEvent()
        {
            //Record
            mouseHook.MouseMove += new MouseEventHandler(mouseHook_MouseMove);
            mouseHook.MouseDown += new MouseEventHandler(mouseHook_MouseDown);
            mouseHook.MouseUp += new MouseEventHandler(mouseHook_MouseUp);
            mouseHook.MouseWheel += new MouseEventHandler(mouseHook_MouseWheel);

            keyboardHook.KeyDown += new KeyEventHandler(keyboardHook_KeyDown);
            keyboardHook.KeyUp += new KeyEventHandler(keyboardHook_KeyUp);
        }

        #endregion

        #region main

        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(string[] args)
        {
            InitializeComponent();

            CallBackWinAppWebBrowser();
            InitMouseKeyBoardEvent();

            var path = $"{Application.StartupPath}\\Firefox";

            Xpcom.Initialize(path);
            
            FormLoad();
            
            arguments = string.Join(",", args);

            LoadScript(args);
        }

        private void KeyEvent(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                //F12
                case 123:
                    TooglePanel();
                    if (developerToolsToolStripMenuItem.Checked) tbxCode.Focus();
                    break;
                //F5
                case 116:
                    toolStripRunning_Click(this, null);
                    break;
                //F3
                case 114:
                    tbxCode.AppendText("MouseMove(" + Cursor.Position.X + ", " + Cursor.Position.Y + ", true, 10);" + Environment.NewLine);
                    break;
                //F4
                case 115:
                    GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                    if (wb != null)
                    {
                        htmlElm = (GeckoHtmlElement)wb.Document.ElementFromPoint(Cursor.Position.X, Cursor.Position.Y);
                    }
                    break;
                default:
                    break;
            }            
        }

        #endregion

        #region Menu Events

        private void newScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewScript();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenScript();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveScript();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsScript();
        }


        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginClick();
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegisterClick();
        }


        private void hideWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MinimizeWindow();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exit();
        }



        private void managerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManagerClick();
        }

        private void developerToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeveloperToolsClick();
        }


        private void scrollToViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScrollViewClick();
        }

        private void colorElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorElementClick();
        }

        private void cfgShowImages_Click(object sender, EventArgs e)
        {
            ShowImageClick();
        }
        
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutClick();
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckUpdateClick();
        }       


        private void notifyIconAutomation_Click(object sender, EventArgs e)
        {
            ShowWindow();
        }

        #endregion

        #region ToolBar Events

        private void btnRunTwo_Click(object sender, EventArgs e)
        {
            RunCode();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            RecordEvents();
        }

        private void toolStripBack_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void toolStripNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void toolStripReload_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void btnNewTab_Click(object sender, EventArgs e)
        {
            tabnew();
        }

        private void btnCloseTab_Click(object sender, EventArgs e)
        {
            tabclose();
        }

        private void btnCloseAllTab_Click(object sender, EventArgs e)
        {
            tabcloseall();
        }

        private void btnShowHideDeveloperTool_Click(object sender, EventArgs e)
        {
            ShowHideDeveloperTools();
        }

        private void btnDetection_Click(object sender, EventArgs e)
        {
            DetectionClick();
        }

        private void sddLanguage_Click(object sender, EventArgs e)
        {
            ChangeLanguage();
        }

        private void btnRegconization_Click(object sender, EventArgs e)
        {
            Regconization();
        }

        #endregion

        #region TextBox and Go Events

        private void btnGo_Click(object sender, EventArgs e)
        {
            GoClick();
        }

        private void tbxAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;

            this.btnGo_Click(sender, e);
        }

        #endregion

        #region Main Browser Events

        private void tabMain_Selected(object sender, TabControlEventArgs e)
        {
            TabSelected();
        }

        #endregion

        #region Developer Tools Events

        #region Script Code

        private void toolStripRunning_Click(object sender, EventArgs e)
        {
            RunCode();
        }

        private void btnNewScript_Click(object sender, EventArgs e)
        {
            NewScript();
        }

        private void btnOpenScript_Click(object sender, EventArgs e)
        {
            OpenScript();
        }

        private void btnSaveScript_Click(object sender, EventArgs e)
        {
            SaveScript();
        }

        private void btnSaveAsScript_Click(object sender, EventArgs e)
        {
            SaveAsScript();
        }

        private void btnScriptClear_Click(object sender, EventArgs e)
        {
            ClearScript();
        }

        private void tbxCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyData != (Keys.RButton | Keys.ShiftKey | Keys.Alt | Keys.Control) && e.KeyValue == 83)
            {
                btnSaveScript_Click(this, null);
            }
            else if (e.Control && e.KeyData != (Keys.RButton | Keys.ShiftKey | Keys.Alt | Keys.Control) && e.KeyValue == 79)
            {
                btnOpenScript_Click(this, null);
            }
            else if (e.Control && e.KeyData != (Keys.RButton | Keys.ShiftKey | Keys.Alt | Keys.Control) && e.KeyValue == 78)
            {
                btnNewScript_Click(this, null);
            }
        }

        #endregion

        #region Template Code

        private void btnNewTemplate_Click(object sender, EventArgs e)
        {
            NewTemplate();
        }

        private void btnOpenTemplate_Click(object sender, EventArgs e)
        {
            OpenTemplate();
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            SaveTemplate();
        }

        private void btnSaveAsTemplate_Click(object sender, EventArgs e)
        {
            SaveAsTemplate();
        }

        private void btnTemplateClear_Click(object sender, EventArgs e)
        {
            ClearTemplate();
        }

        private void tbxTemplate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyData != (Keys.RButton | Keys.ShiftKey | Keys.Alt | Keys.Control) && e.KeyValue == 83)
            {
                btnSaveTemplate_Click(this, null);
            }
            else if (e.Control && e.KeyData != (Keys.RButton | Keys.ShiftKey | Keys.Alt | Keys.Control) && e.KeyValue == 79)
            {
                btnOpenTemplate_Click(this, null);
            }
            else if (e.Control && e.KeyData != (Keys.RButton | Keys.ShiftKey | Keys.Alt | Keys.Control) && e.KeyValue == 78)
            {
                btnNewTemplate_Click(this, null);
            }
        }

        #endregion

        #region AutoBot

        private void tbxAsk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;
            string user_input = tbxAsk.Text;
        }

        #endregion

        #region Download

        private void toolNewDownload_Click(object sender, EventArgs e)
        {
            NewDownload download = new NewDownload();
            download.ShowDialog();
        }

        private void toolStart_Click(object sender, EventArgs e)
        {
            downloadList1.StartSelections();
        }

        private void toolPause_Click(object sender, EventArgs e)
        {
            downloadList1.Pause();
        }

        private void toolPauseAll_Click(object sender, EventArgs e)
        {
            downloadList1.PauseAll();
        }

        private void toolRemove_Click(object sender, EventArgs e)
        {
            downloadList1.RemoveSelections();
        }

        private void toolRemoveCompleted_Click(object sender, EventArgs e)
        {
            downloadList1.RemoveCompleted();
        }

        #endregion

        #endregion

        #region Event Funtions

        #region Menu Event Functions

        public void NewScript()
        {
            tbxCode.Text = "";
            LastScriptFile = "";
            toolStripStatus.Text = Language.Resource.NewScript;
        }

        public void OpenScript()
        {
            openFileDialog1.Filter = Language.Resource.UnicodeScriptFile;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = Language.Resource.OpenScriptTitle;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string code = File.ReadAllText(openFileDialog1.FileName);
                    tbxCode.Text = "";
                    if (!string.IsNullOrEmpty(code))
                    {
                        tbxCode.Text = code;
                    }

                    LastScriptFile = openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    log(string.Format(Language.Resource.OpenFileError, ex.Message));
                }
            }
        }

        public void SaveScript()
        {
            toolStripStatus.Text = Language.Resource.Saving;
            if (string.IsNullOrEmpty(LastScriptFile))
            {
                saveFileDialog1.Filter = Language.Resource.UnicodeScriptFile;
                saveFileDialog1.Title = Language.Resource.SaveScriptTitle;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName, false))
                        {
                            file.Write(tbxCode.Text);
                        }

                        toolStripStatus.Text = Language.Resource.Saved;
                        LastScriptFile = saveFileDialog1.FileName;
                    }
                    catch (Exception ex)
                    {
                        log(string.Format(Language.Resource.SaveFileError, ex.Message));
                    }
                }
                else
                {
                    toolStripStatus.Text = "";
                }
            }
            else
            {
                FileInfo fileInfo = new FileInfo(LastScriptFile);
                if (fileInfo.IsReadOnly)
                {
                    log(Language.Resource.ReadOnlyFile);
                }
                else
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(LastScriptFile, false))
                    {
                        file.Write(tbxCode.Text);
                    }
                    toolStripStatus.Text = Language.Resource.Saved;
                }
            }
        }

        public void SaveAsScript()
        {
            LastScriptFile = "";
            btnSaveScript_Click(this, null);
        }


        public void LoginClick()
        {
            if (loginToolStripMenuItem.Text == Language.Resource.Login)
            {
                frmLogin signin = new frmLogin();
                signin.Show();
            }
            else
            {
                Logout();
            }
        }

        public void RegisterClick()
        {
            frmRegister register = new frmRegister();
            register.Show();
        }


        public int login(string username, string password)
        {
            int result = 0;

            User user = new User();
            user.UserName = username;
            user.Password = password;
            user.Path = Application.StartupPath + "\\" + username + ".tdc";
            result = user.Login();

            if (result == 1)
            {
                frmMain main = (frmMain)Application.OpenForms["frmMain"];
                CurrentUser = user.GetBy(username);
                main.ChangeButtonLogin(true);
                main.CheckManager();
            }
            else { CurrentUser = null; }

            return result;
        }

        public string getCurrentUser()
        {
            string result = "";

            result = new JavaScriptSerializer().Serialize(CurrentUser);

            return result;
        }

        public int register(string username, string email, string password, string confirm)
        {
            int result = 0;

            User user = new User();
            user.UserName = username;
            user.Email = email;
            user.Password = password;
            user.Confirm = confirm;
            user.Path = Application.StartupPath + "\\" + username + ".tdc";
            result = user.Register();

            return result;
        }

        public void Logout()
        {
            User.Current = null;
            loginToolStripMenuItem.Text = Language.Resource.Login;
        }


        public void MinimizeWindow()
        {
            notifyIconAutomation.Visible = true;
            notifyIconAutomation.ShowBalloonTip(500);

            this.Hide();
        }

        public void exit()
        {
            Stop();
            //this.Dispose();
            Application.Exit();
        }


        public void ManagerClick()
        {
            frmManager manager = new frmManager();
            manager.Show();
        }

        public void DeveloperToolsClick()
        {
            if (developerToolsToolStripMenuItem.Checked == false)
            {
                developerToolsToolStripMenuItem.Checked = true;
            }
            else
            {
                developerToolsToolStripMenuItem.Checked = false;
            }
            TooglePanel();
        }


        public void ScrollViewClick()
        {
            if (scrollToViewToolStripMenuItem.Checked == false)
            {
                scrollToViewToolStripMenuItem.Checked = true;
            }
            else
            {
                scrollToViewToolStripMenuItem.Checked = false;
            }
        }

        public void ColorElementClick()
        {
            if (colorElementToolStripMenuItem.Checked == false)
            {
                colorElementToolStripMenuItem.Checked = true;
            }
            else
            {
                colorElementToolStripMenuItem.Checked = false;
            }
        }

        public void ShowImageClick()
        {
            if (cfgShowImages.Checked == false)
            {   
                cfgShowImages.Checked = true;
            }
            else
            {
                cfgShowImages.Checked = false;
            }
        }


        public void AboutClick()
        {
            
        }

        public void CheckUpdateClick()
        {
            
        }

        public void ShowWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Activate();
        }

        public void Regconization()
        {
            if (btnRegconization.Text == Language.Resource.StartRegconization)
            {
                btnRegconization.Text = Language.Resource.StopRegconization;
                toolStripStatus.Text = Language.Resource.StartRegconization;
            }
            else
            {
                btnRegconization.Text = Language.Resource.StartRegconization;
                toolStripStatus.Text = Language.Resource.StopRegconization;
            }
        }

        
        #endregion

        #region Toolbar Event Functions

        public void RunCode()
        {
            if (toolStripRunning.Text.Equals(Language.Resource.Run))
            {
                IsStop = false;
                btnRecord.Enabled = false;

                toolStripStatus.Text = Language.Resource.RunBegin;
                toolStripRunning.Text = Language.Resource.Stop;
                btnRunTwo.Text = Language.Resource.Stop;
                wbMain.Document.InvokeScript("UnAbort");
                if (!string.IsNullOrEmpty(tbxCode.Text))
                {
                    ExcuteJSCode(tbxCode.Text);
                }

                toolStripRunning.Text = Language.Resource.Run;
                btnRunTwo.Text = Language.Resource.Run;
                toolStripStatus.Text = Language.Resource.RunComplete;
                btnRecord.Enabled = true;
            }
            else
            {
                IsStop = true;
                toolStripRunning.Text = Language.Resource.Run;
                btnRunTwo.Text = Language.Resource.Run;
                toolStripStatus.Text = "";

                wbMain.Document.InvokeScript("Abort");
            }
        }

        public void RecordEvents()
        {
            lastTimeRecorded = Environment.TickCount;

            if (btnRecord.Text.Equals(Language.Resource.Record))
            {
                tbxCode.Text = "";
                toolStripRunning.Enabled = false;
                btnRunTwo.Enabled = false;
                mouseHook.Start();
                keyboardHook.Start();
                btnRecord.Text = Language.Resource.Stop;
            }
            else
            {
                toolStripRunning.Enabled = true;
                btnRunTwo.Enabled = true;
                mouseHook.Stop();
                keyboardHook.Stop();
                btnRecord.Text = Language.Resource.Record;
            }
        }

        public void Back()
        {
            //Back WebBrowser
            BackWebBrowser();
        }

        public void Next()
        {
            //Next WebBrowser
            NextWebBrowser();
        }

        public void Reload()
        {
            //Reload WebBrowser
            ReloadWebBrowser();
        }

        public void Stop()
        {

        }

        public void tabnew()
        {
            TabPage tab = new TabPage("new tab");
            tabMain.Controls.Add(tab);
            currentTab = tab;
            tabMain.SelectedTab = currentTab;
        }

        public void tabclose()
        {
            if (tabMain.TabPages.Count > 0)
            {
                if (tabMain.SelectedTab.Controls.Count > 0)
                {
                    tabMain.SelectedTab.Controls[0].Dispose();
                }
                tabMain.SelectedTab.Dispose();

                if (tabMain.TabPages.Count > 1)
                {
                    tabMain.SelectTab(tabMain.TabPages.Count - 1);
                }
                currentTab = tabMain.SelectedTab;
            }
            else
            {
                currentTab = null;
            }
        }

        public void tabcloseall()
        {

            while (tabMain.TabPages.Count > 0)
            {
                try
                {
                    Application.DoEvents();
                    if (tabMain.TabPages[0].Controls.Count > 0)
                    {
                        tabMain.TabPages[0].Controls[0].Dispose();
                    }
                    tabMain.TabPages[0].Dispose();
                }
                catch { }
            }
            currentTab = null;
        }

        public void ShowHideDeveloperTools()
        {
            developerToolsToolStripMenuItem_Click(this, null);
            TooglePanel();
            if (developerToolsToolStripMenuItem.Checked)
            {
                if (tbxCode != null)
                    tbxCode.Focus();
            }
        }

        public void DetectionClick()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (btnDetection.Text == Language.Resource.ShowDetection)
            {
                btnDetection.Text = Language.Resource.HideDetection;

                if (wb != null)
                {
                    GeckoDocument doc = wb.Document;
                }
            }
            else
            {
                btnDetection.Text = Language.Resource.ShowDetection;

                if (wb != null)
                {
                    GeckoDocument doc = wb.Document;
                    toolStripStatus.Text = "";
                }
            }
        }

        public void ChangeLanguage()
        {
            if (Language.Resource.Culture.Name == "en-US")
            {
                Language.Resource.Culture = CultureInfo.CreateSpecificCulture("vi-VN");
                sddLanguage.Text = Language.Resource.English;
                System.Reflection.Assembly.GetExecutingAssembly();

                System.IO.Stream file = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("WebAutomation.Resources.vn.png");
                sddLanguage.Image = Image.FromStream(file);
            }
            else if(Language.Resource.Culture.Name == "vi-VN")
            {
                Language.Resource.Culture = CultureInfo.CreateSpecificCulture("ko-KR");
                sddLanguage.Text = Language.Resource.English;

                System.IO.Stream file = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("WebAutomation.Resources.kor.png");
                sddLanguage.Image = Image.FromStream(file);
            }
            else
            {
                Language.Resource.Culture = CultureInfo.CreateSpecificCulture("en-US");
                sddLanguage.Text = Language.Resource.English;

                System.IO.Stream file = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("WebAutomation.Resources.usa.png");
                sddLanguage.Image = Image.FromStream(file);
            }

            InitLanguage();
        }

        private void TooglePanel()
        {
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();

            if (developerToolsToolStripMenuItem.Checked)
            {
                btnShowHideDeveloperTool.Text = Language.Resource.HideDeveloperTools;
                this.splitContainer1.Panel2Collapsed = false;
                this.splitContainer1.Panel2.Show();
            }
            else
            {
                btnShowHideDeveloperTool.Text = Language.Resource.ShowDeveloperTools;
                this.splitContainer1.Panel2Collapsed = true;
                this.splitContainer1.Panel2.Hide();
            }

            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
        }

        #endregion

        #region TextBox and Go Event Functions

        public void GoClick()
        {
            go(tbxAddress.Text);
        }

        nsIMemory _memoryService = null;
        public void go(string url)
        {
            if (currentTab == null)
            {
                tabnew();
            }

            //WebBrowser
            if (!url.StartsWith("/"))
            {
                if (currentTab.Controls.Count > 0)
                {
                    Control ctr = currentTab.Controls[0];
                    if (ctr != null)
                    {
                        var wb = (GeckoWebBrowser)ctr;
                        wb.Stop();
                        wb.ProgressChanged -= wbBrowser_ProgressChanged;
                        wb.Navigated -= wbBrowser_Navigated;
                        wb.DocumentCompleted -= wbBrowser_DocumentCompleted;
                        wb.CanGoBackChanged -= wbBrowser_CanGoBackChanged;
                        wb.CanGoForwardChanged -= wbBrowser_CanGoForwardChanged;
                        wb.ShowContextMenu -= new EventHandler<GeckoContextMenuEventArgs>(wbBrowser_ShowContextMenu);
                        wb.DomContextMenu -= wbBrowser_DomContextMenu;
                        wb.Dispose();
                        wb = null;
                        
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        if (_memoryService == null)
                        {
                            _memoryService = Xpcom.GetService<nsIMemory>("@mozilla.org/xpcom/memory-service;1");
                        }
                        _memoryService.HeapMinimize(false);
                    }                    
                    currentTab.Controls.Clear();
                }
                GoWebBrowser(url);
            }
            else
            {
                GoWebBrowserByXpath(url);
            }
        }

        public void goWithProxy(string url, string proxyUrl)
        {
            //var proxyAddress = new Uri(proxyUrl);
            //GeckoPreferences.Default["network.proxy.type"] = 1;
            //GeckoPreferences.Default["network.proxy.http"] =  proxyAddress.Host;
            //GeckoPreferences.Default["network.proxy.http_port"] = proxyAddress.Port;
            go(url);
        }
        private void TabSelected()
        {
            TabSelectedWebBrowser();
        }

        #endregion

        #region Script Code Event Funtions

        public void ClearScript()
        {
            tbxCode.Text = "";
        }

        #endregion

        #region Template Event Functions

        public void OpenTemplate()
        {
            openFileDialog1.Filter = Language.Resource.UnicodeTemplateFile;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = Language.Resource.OpenTemplateTitle;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string code = File.ReadAllText(openFileDialog1.FileName);

                    if (!string.IsNullOrEmpty(code))
                    {
                        tbxTemplate.Text = code;
                    }

                    LastTemplateFile = openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Language.Resource.OpenFileError + ex.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SaveTemplate()
        {
            toolStripStatus.Text = Language.Resource.Saving;
            if (string.IsNullOrEmpty(LastTemplateFile))
            {
                saveFileDialog1.Filter = Language.Resource.UnicodeTemplateFile;
                saveFileDialog1.Title = Language.Resource.SaveTemplateTitle;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string result = "";
                        result = tbxTemplate.Text;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName, false))
                        {
                            file.Write(result);
                        }
                        toolStripStatus.Text = Language.Resource.Saved;
                        LastTemplateFile = saveFileDialog1.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Language.Resource.SaveFileError + ex.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    toolStripStatus.Text = "";
                }
            }
            else
            {
                FileInfo fileInfo = new FileInfo(LastTemplateFile);
                if (fileInfo.IsReadOnly)
                {
                    MessageBox.Show(Language.Resource.ReadOnlyFile, Language.Resource.Message);
                }
                else
                {
                    string result = "";
                    result = tbxTemplate.Text;

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(LastTemplateFile, false))
                    {
                        file.Write(result);
                    }

                    toolStripStatus.Text = Language.Resource.Saved;
                }
            }
        }

        public void NewTemplate()
        {
            tbxTemplate.Text = "";
            LastTemplateFile = "";
            toolStripStatus.Text = Language.Resource.NewTemplate;
        }

        public void SaveAsTemplate()
        {
            LastTemplateFile = "";
            btnSaveTemplate_Click(this, null);
        }

        public void ClearTemplate()
        {
            tbxTemplate.Text = "";
        }

        #endregion

        #endregion

        #region Delegate

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl != null)
            {
                if (oControl.InvokeRequired)
                {
                    SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                    oControl.Invoke(d, new object[] { oControl, propName, propValue });
                }
                else
                {
                    Type t = oControl.GetType();
                    System.Reflection.PropertyInfo[] props = t.GetProperties();
                    foreach (System.Reflection.PropertyInfo p in props)
                    {
                        if (p.Name.ToUpper() == propName.ToUpper())
                        {
                            p.SetValue(oControl, propValue, null);
                        }
                    }
                }
            }
        }

        #endregion

        #region System Function

        public void FormLoad()
        {
            TooglePanel();

            KeyUp += new KeyEventHandler(KeyEvent);
            Language.Resource.Culture = CultureInfo.CreateSpecificCulture("en-US");
            InitLanguage();
            InitContextMenu();

            EnableDownloadButton(false);
            downloadList1.SelectionChange += downloadList1_SelectionChange;

            MaxWait = (ConfigurationManager.AppSettings["MaxWait"] != null ? ConfigurationManager.AppSettings["MaxWait"] : string.Empty);

            GeckoPreferences.Default["dom.max_script_run_time"] = 0;
            ShowWindow();
        }

        void downloadList1_SelectionChange(object sender, EventArgs e)
        {
            var isSelected = (sender as DownloadList).IsSelected;
            EnableDownloadButton(isSelected);
        }

        void EnableDownloadButton(bool isSelected)
        {
            toolRemove.Enabled = isSelected;
            toolStart.Enabled = isSelected;
            toolPause.Enabled = isSelected;
            toolPauseAll.Enabled = isSelected;
            toolRemoveCompleted.Enabled = isSelected;
        }

        private void ExcuteJSCode(string code)
        {
            ExcuteJSCodeWebBrowser(code);
        }

        private object GetCurrentWB()
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

        public void LoadScript(string[] args)
        {
            if (args.Length > 0)
            {
                var path = args[0];
                if (!string.IsNullOrEmpty(path))
                {
                    btnGo_Click(null, null);
                    if (File.Exists(path))
                    {
                        sleep(1, false);
                        tbxCode.Text = read(path);
                        RunCode();
                    }
                }
            }
        }

        public void HideForm()
        {
            this.ShowInTaskbar = false;
            this.Visible = false;
        }
        
        #endregion

        #region ISingleInstanceEnforcer Members

        public void OnMessageReceived(MessageEventArgs e)
        {
            string[] args = (string[])e.Message;

            this.BeginInvoke((MethodInvoker)delegate
            {
                LoadScript(args);
            });
        }

        public void OnNewInstanceCreated(EventArgs e)
        {
            this.Focus();
        }

        #endregion

        #region functions

        public string extract(string xpath, string type)
        {
            string result = string.Empty;
            GeckoHtmlElement elm = null;

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                elm = GetElement(wb, xpath);
                if (elm != null)
                    UpdateUrlAbsolute(wb.Document, elm);

                if (elm != null)
                {
                    switch (type)
                    {
                        case "html":
                            result = elm.OuterHtml;
                            break;
                        case "text":
                            if (elm.GetType().Name == "GeckoTextAreaElement")
                            {
                                result = ((GeckoTextAreaElement)elm).Value;
                            }
                            else
                            {
                                result = elm.TextContent.Trim();
                            }
                            break;
                        case "value":
                            result = ((GeckoInputElement)elm).Value;
                            break;
                        default:
                            result = extractData(elm, type);
                            break;
                    }
                }
            }

            return result;
        }

        public string extractAll(string xpath, string type)
        {
            StringBuilder result = new StringBuilder();

            List<GeckoNode> elm = new List<GeckoNode>();

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                elm = GetElements(wb, xpath);
                if (elm != null) 
                { 
                    UpdateUrlAbsolute(wb.Document, (GeckoHtmlElement)elm.FirstOrDefault());
                    
                    foreach(GeckoHtmlElement item in elm) {
                        switch (type)
                        {
                            case "html":
                                result.AppendLine($"{{ value: '{item.OuterHtml.Replace("\'", "\\'").Replace("{", "").Replace("}", "") }'}},");
                                break;
                            case "text":
                                if (elm.GetType().Name == "GeckoTextAreaElement")
                                {
                                    result.AppendLine($"{{ value: '{((GeckoTextAreaElement)item).Value.Replace("\'", "\\'").Replace("{", "").Replace("}", "")}'}},");
                                }
                                else
                                {
                                    result.AppendLine($"{{ value: '{item.TextContent.Trim().Replace("\'", "\\'").Replace("{", "").Replace("}", "")}'}},");
                                }
                                break;
                            case "value":
                                result.AppendLine($"{{ value: '{((GeckoInputElement)item).Value.Replace("\'", "\\'").Replace("{", "").Replace("}", "")}'}},");
                                break;
                            default:
                                result.AppendLine($"{{ value: '{extractData(item, type).Replace("\'", "\\'").Replace("{", "").Replace("}", "")}'}},");
                                break;
                        }
                    }
                }
            }

            return $"[{result.ToString()}]";
        }

        public string extractUntil(string xpath, string type)
        {
            var result = string.Empty;

            GeckoHtmlElement elm = null;

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                elm = GetCompleteElementByXPath(wb, xpath);
                if (elm != null)
                    UpdateUrlAbsolute(wb.Document, elm);

                if (elm != null)
                {
                    switch (type)
                    {
                        case "html":
                            result = elm.OuterHtml;
                            break;
                        case "text":
                            if (elm.GetType().Name == "GeckoTextAreaElement")
                            {
                                result = ((GeckoTextAreaElement)elm).Value;
                            }
                            else
                            {
                                result = elm.TextContent.Trim();
                            }
                            break;
                        default:
                            result = extractData(elm, type);
                            break;
                    }
                }
            }

            return result;
        }

        public void filliframe(string title, string value)
        {
            /*GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                foreach (GeckoWindow ifr in wb.Window.Frames)
                {
                    if (ifr.Document.Title == title)
                    {
                        foreach (var item in ifr.Document.ChildNodes)
                        {
                            if (item.NodeName == "HTML")
                            {
                                foreach (var it in item.ChildNodes)
                                {
                                    if (it.NodeName == "BODY")
                                    {
                                        GeckoBodyElement elem = (GeckoBodyElement)it;
                                        elem.InnerHtml = value;
                                        elem.Focus();
                                    }
                                }                                
                                break;
                            }
                        }
                        break;
                    }
                }
            }*/
        }

        public void fill(string xpath, string value)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                if (xpath.StartsWith("/"))
                {
                    GeckoHtmlElement elm = GetElement(wb, xpath);
                    if (elm != null)
                    {
                        switch (elm.TagName)
                        {
                            case "IFRAME":
                                /*foreach (GeckoWindow ifr in wb.Window.Frames)
                                {
                                    if (ifr.Document == elm.DOMElement)
                                    {
                                        ifr.Document.TextContent = value;
                                        break;
                                    }
                                }*/
                                break;
                            case "INPUT":
                                GeckoInputElement input = (GeckoInputElement)elm;
                                input.Value = value;
                                input.Focus();
                                break;
                            case "TEXTAREA":
                                GeckoTextAreaElement input1 = (GeckoTextAreaElement)elm;
                                input1.Value = value;
                                input1.Focus();
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Byte[] bytes = Encoding.UTF32.GetBytes(value);
                    StringBuilder asAscii = new StringBuilder();
                    for (int idx = 0; idx < bytes.Length; idx += 4)
                    {
                        uint codepoint = BitConverter.ToUInt32(bytes, idx);
                        if (codepoint <= 127)
                            asAscii.Append(Convert.ToChar(codepoint));
                        else
                            asAscii.AppendFormat("\\u{0:x4}", codepoint);
                    }
                    /*var id = xpath;
                    using (AutoJSContext context = new AutoJSContext(wb.Window.JSContext))
                    {
                        context.EvaluateScript("document.getElementById('" + id + "').value = '" + asAscii.ToString() + "';");
                        context.EvaluateScript("document.getElementById('" + id + "').scrollIntoView();");
                    }*/
                }
            }
            
        }

        public void filldropdown(string xpath, string value)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                if (xpath.StartsWith("/"))
                {
                    GeckoHtmlElement elm = GetElement(wb, xpath);
                    if (elm != null)
                    {
                        var dropdown = elm as GeckoSelectElement;
                        var length = dropdown.Options.Length;
                        var items = dropdown.Options;
                        for(var i = 0; i < length; i++){
                            var item = dropdown.Options.Item((uint)i);
                            if(item.Text.ToUpper() == value.ToUpper()){
                                item.SetAttribute("selected", "selected");
                            }
                            else
                            {
                                item.RemoveAttribute("selected");
                            }
                        }
                        elm.Focus();
                        //elm.SetAttribute("value", value);
                        //elm.SetAttribute("selectedIndex", value);
                        //elm.Focus();
                    }
                }
                else
                {
                    /*var id = xpath;
                    using (AutoJSContext context = new AutoJSContext(wb.Window.JSContext))
                    {
                        string javascript = string.Empty;
                        context.EvaluateScript("document.getElementById('" + id + "').selectedIndex = " + value + ";");
                        JQueryExecutor jquery = new JQueryExecutor(wb.Window);
                        jquery.ExecuteJQuery("$('#" + id + "').trigger('change');");
                        context.EvaluateScript("document.getElementById('" + id + "').scrollIntoView();");
                    }*/
                }
            }
        }

        public void click(string xpath)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                if (xpath.StartsWith("/"))
                {
                    GeckoHtmlElement elm = GetElement(wb, xpath);
                    if (elm != null)
                    {
                        elm.Click();
                        elm.Focus();
                    }
                }
                else
                {
                    /*var id = xpath;
                    using (AutoJSContext context = new AutoJSContext(wb.Window.JSContext))
                    {
                        context.EvaluateScript("document.getElementById('" + id + "').click();");
                        context.EvaluateScript("document.getElementById('" + id + "').scrollIntoView();");
                    }*/
                }
            }
        }

        public void sleep(int seconds, bool isBreakWhenWBCompleted)
        {
            IsBreakSleep = false;
            for (int i = 0; i < seconds * 10; i++)
            {
                if (IsStop == false)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);

                    toolStripStatus.Text = "Sleep: " + ((i + 1) * 100) + "/" + (seconds * 1000);
                    if (isBreakWhenWBCompleted && IsBreakSleep)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            toolStripStatus.Text = "";
        }

        public void log(string text)
        {
            tbxPreview.Text += text + "\n";
            //tbxPreview.SelectionStart = tbxPreview.Text.Length;
            //tbxPreview.ScrollToCaret();
            //tbxPreview.ScrollRange(0, tbxPreview.Lines.Count);
        }

        public void clearlog()
        {
            tbxPreview.Text = "";
        }

        public void createfolder(string path)
        {
            try
            {
                if (System.IO.Directory.Exists(path) == false)
                    System.IO.Directory.CreateDirectory(path);
            }
            catch { }
        }

        public string getfiles(string path)
        {
            string result = "";
            string r = "";

            string[] filePaths = Directory.GetFiles(path);

            try
            {
                foreach (string f in filePaths)
                {
                    r += f + ",";
                }
                if (!string.IsNullOrEmpty(r))
                {
                    result = r.Substring(0, r.Length - 1);
                }
                else
                {
                    result = r;
                }
            }
            catch { }

            return result;
        }

        public string getfolders(string path)
        {
            string result = "";
            string r = "";

            try
            {
                string[] directoryPaths = Directory.GetDirectories(path);

                foreach (string f in directoryPaths)
                {
                    r += f + ",";
                }
                if (!string.IsNullOrEmpty(r))
                {
                    result = r.Substring(0, r.Length - 1);
                }
                else
                {
                    result = r;
                }
            }
            catch { }

            return result;
        }

        public void download(string savePath, string url)
        {
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Credentials = System.Net.CredentialCache.DefaultCredentials;
                wc.Proxy = null;
                wc.Headers.Add(System.Net.HttpRequestHeader.UserAgent, "anything");
                try
                {
                    Uri uri = new Uri(url);
                    wc.DownloadFileAsync(uri, savePath);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        public string read(string path)
        {
            string result = "";
            try
            {
                string[] list = System.IO.File.ReadAllLines(path);
                foreach (string l in list)
                {
                    result += l + "\n";
                }
            }
            catch { }
            return result;
        }

        public void save(string content, string path, bool isOverride)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, isOverride))
                {
                    file.WriteLine(content);
                }
            }
            catch { }
        }

        public void remove(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch { }
        }

        public void removefolder(string path)
        {
            try
            {
                if (System.IO.Directory.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);

                    string[] files = Directory.GetFiles(path);
                    string[] dirs = Directory.GetDirectories(path);

                    foreach (string file in files)
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }

                    foreach (string dir in dirs)
                    {
                        removefolder(dir);
                    }

                    Directory.Delete(path, false);
                }
            }
            catch { }
        }

        public void copyFolder(string source, string target, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(source);

            if (!dir.Exists)
            {
                log("Source directory does not exist or could not be found: " + source);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(target, file.Name);
                if (File.Exists(temppath))
                    File.SetAttributes(temppath, FileAttributes.Normal);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(target, subdir.Name);
                    copyFolder(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public void copyFile(string source, string target, bool isOverride)
        {
            if (File.Exists(target))
                File.SetAttributes(target, FileAttributes.Normal);

            FileInfo file = new FileInfo(source);
            file.CopyTo(target, isOverride);
        }

        public void replaceTextinFile(string path, string pattern, string value)
        {
            if (File.Exists(path))
                File.SetAttributes(path, FileAttributes.Normal);

            string text = File.ReadAllText(path);
            var regex = new Regex(pattern);
            if (regex.IsMatch(text))
            {
                text = text.Replace(regex.Match(text).Groups[0].Value, value);
                File.WriteAllText(path, text);
            }
        }

        public void excute(string script)
        {
            ExcuteJSCodeWebBrowser(script);
        }

        public void runcommand(string path, string parameters)
        {
            var strCmdText = $"/C {path} {parameters}";

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = strCmdText;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            //try
            //{
            //    ProcessStartInfo startInfo = new ProcessStartInfo();
            //    startInfo.WorkingDirectory = getCurrentPath();
            //    startInfo.FileName = path;
            //    startInfo.Arguments = parameters;
            //    //startInfo.RedirectStandardOutput = true;
            //    //startInfo.RedirectStandardError = true;
            //    //startInfo.UseShellExecute = false;
            //    //startInfo.CreateNoWindow = true;
            //    try
            //    {
            //        Process p = Process.Start(startInfo);
            //        p.WaitForExit();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
            //catch { }
        }

        public void reboot()
        {
            System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
        }

        public void shutdown()
        {
            System.Diagnostics.Process.Start("shutdown", "/s /t 0");
        }

        public void hibernate()
        {
            Application.SetSuspendState(PowerState.Hibernate, true, true);
        }

        public void standby()
        {
            Application.SetSuspendState(PowerState.Suspend, true, true);
        }

        public void generatekeys()
        {
            System.Security.Cryptography.CspParameters cspParams = null;
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = null;

            string publicKey = "";
            string privateKey = "";

            try
            {
                cspParams = new System.Security.Cryptography.CspParameters();
                cspParams.ProviderType = 1;
                cspParams.Flags = System.Security.Cryptography.CspProviderFlags.UseArchivableKey;
                cspParams.KeyNumber = (int)System.Security.Cryptography.KeyNumber.Exchange;
                rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

                publicKey = rsaProvider.ToXmlString(false);
                privateKey = rsaProvider.ToXmlString(true);

                log("Public Key");
                log(publicKey);
                log("");
                log("Private Key");
                log(privateKey);

                tabControlCode.SelectedIndex = 2;
            }
            catch (Exception ex)
            {

            }
        }

        public string encrypt(string publicKey, string plainText)
        {
            System.Security.Cryptography.CspParameters cspParams = null;
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = null;
            byte[] plainBytes = null;
            byte[] encryptedBytes = null;

            string result = "";
            try
            {
                cspParams = new System.Security.Cryptography.CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(publicKey);

                plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                encryptedBytes = rsaProvider.Encrypt(plainBytes, false);
                result = Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex) { }
            return result;
        }

        public string decrypt(string privateKey, string encrypted)
        {
            System.Security.Cryptography.CspParameters cspParams = null;
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = null;
            byte[] encryptedBytes = null;
            byte[] plainBytes = null;

            string result = "";
            try
            {
                cspParams = new System.Security.Cryptography.CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(privateKey);

                encryptedBytes = Convert.FromBase64String(encrypted);
                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

                result = System.Text.Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex) { }
            return result;
        }

        public void TakeSnapshot(string location)
        {
            try
            {
                GeckoWebBrowser wbBrowser = (GeckoWebBrowser)GetCurrentWB();
                ImageCreator creator = new ImageCreator(wbBrowser);
                byte[] rs = creator.CanvasGetPngImage((uint)wbBrowser.Document.ActiveElement.ScrollWidth, (uint)wbBrowser.Document.ActiveElement.ScrollHeight);


                MemoryStream ms = new MemoryStream(rs);
                Image returnImage = Image.FromStream(ms);

                returnImage.Save(location);

            }
            catch { }
        }

        public string imgToText(string xpath, string language)
        {
            string data = string.Empty;
            string path = string.Empty;
            path = Application.StartupPath + "\\captcha\\image.png";
            bool isSaveSuccess = saveImage(xpath, path);

            if (isSaveSuccess)
            {
                string text = Application.StartupPath + "\\captcha\\output.txt";

                string param = "";
                if (language == "vie")
                {
                    param = "\"" + path + "\" \"" + Application.StartupPath + "\\captcha\\output" + "\" -l vie";
                }
                else
                {
                    param = "\"" + path + "\" \"" + Application.StartupPath + "\\captcha\\output" + "\" -l eng";
                }


                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Application.StartupPath + "\\tesseract.exe";
                process.StartInfo.Arguments = param;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                data = read(text).Replace("\n","");

            }

            return data;
        }

        private bool saveImage(string xpath, string location)
        {
            bool result = false;
            try
            {
                GeckoWebBrowser wbBrowser = (GeckoWebBrowser)GetCurrentWB();
                if (wbBrowser != null)
                {
                    GeckoImageElement element = null;
                    if (xpath.StartsWith("/"))
                        element = (GeckoImageElement)wbBrowser.Document.EvaluateXPath(xpath).GetNodes().FirstOrDefault();
                    else
                        element = (GeckoImageElement)wbBrowser.Document.GetElementById(xpath);
                    GeckoSelection selection = wbBrowser.Window.Selection;
                    selection.SelectAllChildren(element);
                    wbBrowser.CopyImageContents();
                    if (Clipboard.ContainsImage())
                    {
                        Image img = Clipboard.GetImage();
                        img.Save(location, System.Drawing.Imaging.ImageFormat.Png);
                        result = true;
                    }
                }
            }
            catch { result = false; }

            return result;
        }

        async Task PopulateInputFile(GeckoHtmlElement file, string location)
        {
            file.Focus();

            // delay the execution of SendKey to let the Choose File dialog show up
            var sendKeyTask = Task.Delay(500).ContinueWith((_) =>
            {
                // this gets executed when the dialog is visible
                SendKeys.SendWait(location + "{ENTER}");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            file.Click(); // this shows up the dialog

            await sendKeyTask;

            // delay continuation to let the Choose File dialog hide
            await Task.Delay(500);
        }

        public async Task FileUpload(string xpath, string location)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                var file = GetElement(wb, xpath);
                if (file != null)
                {
                    file.Focus();
                    await PopulateInputFile(file, location);
                }
            }
        }

        public void sendKeys(string key)
        {
            SendKeys.SendWait(key);
        }

        public string getCurrentUrl()
        {
            string url = string.Empty;
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                url = wb.Url.ToString();
            }
            return url;
        }

        public void scrollto(int value)
        {
            var wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                using (Gecko.AutoJSContext context = new AutoJSContext(wb.Window))
                {
                    var result = context.EvaluateScript($"window.scrollTo(0, {value})", wb.Window.DomWindow);
                }
            }
        }

        public int getheight()
        {
            int result = 0;

            

            return result;
        }

        public string gettitle()
        {
            string result = "";

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                result = wb.DocumentTitle;
            }

            return result;
        }

        public bool checkelement(string xpath) {
            bool result = false;

            

            return result;
        }

        public string getCurrentContent()
        {
            string result = "";

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                result = wb.Document.Body.InnerHtml;
            }
           
            return result;
        }

        public string getCurrentPath()
        {
            string result = "";
            try
            {
                result = Application.StartupPath;
            }
            catch { }
            return result;
        }

        public void explorer(string path)
        {
            string argument = "/select, \"" + path + "\"";
            Process.Start("explorer.exe", argument);
        }

        public void loadHTML(string path)
        {
            go(path);
        }

        public string textToJSON(string text)
        {
            return text;
        }

        public void ChangeButtonLogin(bool flag)
        {
            if (flag)
            {
                loginToolStripMenuItem.Text = Language.Resource.Logout;
            }
            else
            {
                loginToolStripMenuItem.Text = Language.Resource.Login;
            }
        }

        public void CheckManager()
        {
            if (User.Current != null)
            {
                loginToolStripMenuItem.Text = Language.Resource.Logout;
                managerToolStripMenuItem.Enabled = true;
            }
            else
            {
                loginToolStripMenuItem.Text = Language.Resource.Login;
                managerToolStripMenuItem.Enabled = false;
            }
        }

        public string GetAccountBy(string name)
        {
            string result = "";

            if (User.Current != null)
            {
                Account account = new Account();
                result = account.GetByJSON(name);
            }
            return result;
        }

        public string sendEmail(string name, string email, string subject, string content)
        {
            string result = "False";

            if (User.Current != null && !string.IsNullOrEmpty(name))
            {
                MailServer mailserver = new MailServer();
                var item = mailserver.GetBy(name);
                if (item != null)
                {
                    mailserver.Username = item.Username;
                    mailserver.Password = item.Password;
                    mailserver.Server = item.Server;
                    mailserver.Port = item.Port;

                    result = mailserver.SendEmail(email, subject, content).ToString();
                }
            }
            return result;
        }

        public void CaptchaBorder(string xpath, string style)
        {
            
        }

        [ComImport, InterfaceType((short)1), Guid("3050F669-98B5-11CF-BB82-00AA00BDCE0B")]
        private interface IHTMLElementRenderFixed
        {
            void DrawToDC(IntPtr hdc);
            void SetDocumentPrinter(string bstrPrinterName, IntPtr hdc);
        }

        public void SaveImageFromElement(string xpath, string path)
        {
            saveImage(xpath, path);
        }


        public string GetCurrentMouseX()
        {
            return Cursor.Position.X.ToString();
        }

        public string GetCurrentMouseY()
        {
            return Cursor.Position.Y.ToString();
        }

        public void Mouse_Down(string mouseButton, int LastTime)
        {
            WaitApp(LastTime);

            if (mouseButton == "Left")
            {
                MouseKeyboardLibrary.MouseSimulator.MouseDown(MouseKeyboardLibrary.MouseButton.Left);
            }
            else if (mouseButton == "Right")
            {
                MouseKeyboardLibrary.MouseSimulator.MouseDown(MouseKeyboardLibrary.MouseButton.Right);
            }
            else if (mouseButton == "Middle")
            {
                MouseKeyboardLibrary.MouseSimulator.MouseDown(MouseKeyboardLibrary.MouseButton.Middle);
            }
        }

        public void Mouse_Up(string mouseButton, int LastTime)
        {
            WaitApp(LastTime);

            if (mouseButton == "Left")
            {
                MouseKeyboardLibrary.MouseSimulator.MouseUp(MouseKeyboardLibrary.MouseButton.Left);
            }
            else if (mouseButton == "Right")
            {
                MouseKeyboardLibrary.MouseSimulator.MouseUp(MouseKeyboardLibrary.MouseButton.Right);
            }
            else if (mouseButton == "Middle")
            {
                MouseKeyboardLibrary.MouseSimulator.MouseUp(MouseKeyboardLibrary.MouseButton.Middle);
            }

        }

        public void Mouse_Click(string mouseButton, int LastTime)
        {
            WaitApp(LastTime);

            if (mouseButton == "Left")
            {
                MouseKeyboardLibrary.MouseSimulator.Click(MouseKeyboardLibrary.MouseButton.Left);
            }
            else if (mouseButton == "Right")
            {
                MouseKeyboardLibrary.MouseSimulator.Click(MouseKeyboardLibrary.MouseButton.Right);
            }
            else if (mouseButton == "Middle")
            {
                MouseKeyboardLibrary.MouseSimulator.Click(MouseKeyboardLibrary.MouseButton.Middle);
            }
        }

        public void Mouse_Double_Click(string mouseButton, int LastTime)
        {
            WaitApp(LastTime);

            if (mouseButton == "Left")
            {
                MouseKeyboardLibrary.MouseSimulator.DoubleClick(MouseKeyboardLibrary.MouseButton.Left);
            }
            else if (mouseButton == "Right")
            {
                MouseKeyboardLibrary.MouseSimulator.DoubleClick(MouseKeyboardLibrary.MouseButton.Right);
            }
            else if (mouseButton == "Middle")
            {
                MouseKeyboardLibrary.MouseSimulator.DoubleClick(MouseKeyboardLibrary.MouseButton.Middle);
            }

        }

        public void Mouse_Show(int x, int y, bool isShow, int LastTime)
        {
            WaitApp(LastTime);

            MouseKeyboardLibrary.MouseSimulator.X = x;
            MouseKeyboardLibrary.MouseSimulator.Y = y;

            if (isShow)
            {
                MouseKeyboardLibrary.MouseSimulator.Show();
            }
            else if (isShow == false)
            {
                MouseKeyboardLibrary.MouseSimulator.Hide();
            }
        }

        public void Mouse_Wheel(int delta, int LastTime)
        {
            WaitApp(LastTime);

            MouseKeyboardLibrary.MouseSimulator.MouseWheel(delta);
        }

        public void Key_Down(string key, int LastTime)
        {
            WaitApp(LastTime);

            KeysConverter k = new KeysConverter();
            Keys mykey = (Keys)k.ConvertFromString(key);
            MouseKeyboardLibrary.KeyboardSimulator.KeyDown(mykey);
        }

        public void Key_Up(string key, int LastTime)
        {
            WaitApp(LastTime);

            KeysConverter k = new KeysConverter();
            Keys mykey = (Keys)k.ConvertFromString(key);
            MouseKeyboardLibrary.KeyboardSimulator.KeyUp(mykey);
        }

        private Keys ConvertCharToVirtualKey(char ch)
        {
            short vkey = VkKeyScan(ch);
            Keys retval = (Keys)(vkey & 0xff);
            int modifiers = vkey >> 8;
            if ((modifiers & 1) != 0) retval |= Keys.Shift;
            if ((modifiers & 2) != 0) retval |= Keys.Control;
            if ((modifiers & 4) != 0) retval |= Keys.Alt;
            return retval;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern short VkKeyScan(char ch);

        public void sendText(string text)
        {
            foreach (var item in text.ToArray())
            {
                if (item.ToString() == ":")
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Shift);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Oem1);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyUp(Keys.Shift);
                }
                else
                {
                    var key = ConvertCharToVirtualKey(item);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(key);
                }
            }
            /*var actualValue = string.Empty;
            var values = Enum.GetValues(typeof(Keys));
            KeysConverter converter = new KeysConverter();
            string data = converter.ConvertToString(text);

            foreach (var item in text.ToArray())
            {
                WaitApp(2);
                if (item.ToString() == "_")
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Shift);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.OemMinus);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyUp(Keys.Shift);
                }
                else if (item.ToString() == " ")
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Space);
                }
                else if (item.ToString() == ":")
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Shift);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Oem1);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyUp(Keys.Shift);
                }
                else if (item.ToString() == "\\")
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Shift);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Oem5);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyUp(Keys.Shift);
                }
                else if (item.ToString() == "-")
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Shift);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.OemMinus);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyUp(Keys.Shift);
                }
                else if (item.ToString() == ".")
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Shift);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.OemPeriod);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyUp(Keys.Shift);
                }
                else if (item.ToString() == "/")
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Shift);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.OemQuestion);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyUp(Keys.Shift);
                }
                else if (item.ToString() == "/")
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Shift);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown(Keys.Oemcomma);
                    MouseKeyboardLibrary.KeyboardSimulator.KeyUp(Keys.Shift);
                }
                else
                {
                    MouseKeyboardLibrary.KeyboardSimulator.KeyDown((Keys)converter.ConvertFromString(item.ToString().ToUpper()));
                }
            }*/
        }

        private void WaitApp(int seconds)
        {
            Application.DoEvents();
            System.Threading.Thread.Sleep(seconds);
        }

        public void Abort()
        {
            IsStop = true;
            Stop();
        }

        public void UnAbort()
        {
            IsStop = false;
        }

        public string GetDatabases(string name)
        {
            string result = "";

            Connection conn = new Connection();
            result = conn.GetDatabases(name);

            return result;
        }

        public string GetTables(string name, string dbName)
        {
            string result = "";

            Connection conn = new Connection();
            result = conn.GetTables(name, dbName);

            return result;
        }

        public string GetColumns(string name, string dbName, string table)
        {
            string result = "";

            Connection conn = new Connection();
            result = conn.GetColumns(name, dbName, table);

            return result;
        }

        public string GetRows(string name, string dbName, string sql)
        {
            string result = "";

            Connection conn = new Connection();

            result = conn.GetRows(name, dbName, sql);

            return result;
        }

        public int ExcuteQuery(string name, string dbName, string sql)
        {
            int result = -1;

            Connection conn = new Connection();
            try
            {
                result = conn.ExcuteQuery(name, dbName, sql);
            }
            catch { }
            return result;
        }

        public string RemoveStopWords(string text)
        {
            string result = string.Empty;
            
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("\n", "");
                List<string> data = new List<string>();
                string[] array = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string stopwordPath = getCurrentPath() + "\\resources\\stopword.xml";
                XDocument xdoc = XDocument.Load(stopwordPath);
                foreach (string arr in array)
                {
                    var rs = (from w in xdoc.Descendants("w") where w.Value == arr select w).FirstOrDefault();
                    if (rs == null)
                    {
                        data.Add(arr);
                    }
                }
                result = string.Join(",", data);
            }

            return result;
        }

        public void AddElement(string path, string node1, string node2, string text)
        {
            XDocument xdoc = XDocument.Load(path);
            var item = xdoc.Element(node1).Element(node2);
            if (item != null)
                item.Add(new XElement("w", text));
            else
                xdoc.Element(node1).Add(new XElement(node2, new XElement("w", text)));

            xdoc.Save(path);
        }

        public bool CheckXmlElement(string path, string node, string text)
        {
            bool result = false;

            XDocument xdoc = XDocument.Load(path);
            
            var rs = (from w in xdoc.Descendants(node) where w.Value == text select w).FirstOrDefault();
            if (rs != null) result = true;

            return result;
        }

        public string GetParentElement(string path, string node, string text)
        {
            string result = string.Empty;

            XDocument xdoc = XDocument.Load(path);

            var rs = (from w in xdoc.Descendants(node) where w.Value == text select w).FirstOrDefault();
            if (rs != null) result = rs.Parent.Name.LocalName;

            return result;
        }

        public string GetXmlElement(string path, string node)
        {
            string result = string.Empty;
            List<string> data = new List<string>();
            XDocument xdoc = XDocument.Load(path);
            var list = xdoc.Root.Nodes();
            foreach (XElement elem in list)
            {
                data.Add(elem.Name.LocalName);
            }
            result = string.Join(",", data);
            return result;
        }

        public string ExtractUsingRegularExpression(string pattern, string groupName)
        {
            string result = string.Empty;

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                string doc = wb.Document.Body.TextContent;
                Match m = Regex.Match(doc, pattern);
                if (m.Success)
                {
                    if (m.Groups.Count > 0)
                    {
                        result = m.Groups[groupName].Value;
                    }
                }
            }

            return result;
        }

        public void AddToDownload(string fileName, string url, string folder)
        {
            downloadList1.NewFileDownload(fileName, url, folder, 1);
        }

        public void StartDownload()
        {
            tabMain.SelectedTab = tabDownload;
            downloadList1.SelectAll();
            downloadList1.StartSelections();
        }

        public string GetArguments()
        {
            return arguments;
        }

        private void InitContextMenu()
        {
            contextMenuBrowser.Items.Clear();

            var goItem = new ToolStripMenuItem(Language.Resource.Go);
            var sleepItem = new ToolStripMenuItem(Language.Resource.Sleep);
            var extractItem = new ToolStripMenuItem(Language.Resource.Extract);
            var fillItem = new ToolStripMenuItem(Language.Resource.Fill);
            var clickItem = new ToolStripMenuItem(Language.Resource.Click);

            goItem.Click += item_Click;
            sleepItem.Click += item_Click;

            var attributeItem = new ToolStripMenuItem(Language.Resource.Attribute);
            var htmlItem = new ToolStripMenuItem(Language.Resource.Html);
            var srcItem = new ToolStripMenuItem(Language.Resource.Src);
            var textItem = new ToolStripMenuItem(Language.Resource.Text);
            var urlItem = new ToolStripMenuItem(Language.Resource.Url);
            var repeatItem = new ToolStripMenuItem(Language.Resource.RepeatElement);

            attributeItem.Click += item_Click;
            htmlItem.Click += item_Click;
            srcItem.Click += item_Click;
            textItem.Click += item_Click;
            urlItem.Click += item_Click;
            repeatItem.Click += item_Click;

            extractItem.DropDownItems.Add(attributeItem);
            extractItem.DropDownItems.Add(htmlItem);
            extractItem.DropDownItems.Add(srcItem);
            extractItem.DropDownItems.Add(textItem);
            extractItem.DropDownItems.Add(urlItem);
            extractItem.DropDownItems.Add(repeatItem);

            var textboxItem = new ToolStripMenuItem(Language.Resource.Textbox);
            var dropdownItem = new ToolStripMenuItem(Language.Resource.Dropdown);
            var iframeItem = new ToolStripMenuItem(Language.Resource.iFrame);

            textboxItem.Click += item_Click;
            dropdownItem.Click += item_Click;
            iframeItem.Click += item_Click;

            fillItem.DropDownItems.Add(textboxItem);
            fillItem.DropDownItems.Add(dropdownItem);
            fillItem.DropDownItems.Add(iframeItem);

            var elementItem = new ToolStripMenuItem(Language.Resource.Element);
            var fileUploadItem = new ToolStripMenuItem(Language.Resource.FileUpload);

            elementItem.Click += item_Click;
            fileUploadItem.Click += item_Click;

            clickItem.DropDownItems.Add(elementItem);
            clickItem.DropDownItems.Add(fileUploadItem);

            var mouseItem = new ToolStripMenuItem(Language.Resource.Mouse);
            var currentMouseItem = new ToolStripMenuItem(Language.Resource.GetCurrentMouse);
            var mouseMoveItem = new ToolStripMenuItem(Language.Resource.MouseMove);
            var mouseDownItem = new ToolStripMenuItem(Language.Resource.MouseDown);
            var mouseUpItem = new ToolStripMenuItem(Language.Resource.MouseUp);
            var mouseClickItem = new ToolStripMenuItem(Language.Resource.MouseClick);
            var mouseDoubleClickItem = new ToolStripMenuItem(Language.Resource.MouseDoubleClick);
            var mouseWheelItem = new ToolStripMenuItem(Language.Resource.MouseWheel);

            currentMouseItem.Click += item_Click;
            mouseMoveItem.Click += item_Click;
            mouseDownItem.Click += item_Click;
            mouseUpItem.Click += item_Click;
            mouseClickItem.Click += item_Click;
            mouseDoubleClickItem.Click += item_Click;
            mouseWheelItem.Click += item_Click;

            mouseItem.DropDownItems.Add(currentMouseItem);
            mouseItem.DropDownItems.Add(mouseMoveItem);
            mouseItem.DropDownItems.Add(mouseDownItem);
            mouseItem.DropDownItems.Add(mouseUpItem);
            mouseItem.DropDownItems.Add(mouseClickItem);
            mouseItem.DropDownItems.Add(mouseDoubleClickItem);
            mouseItem.DropDownItems.Add(mouseWheelItem);

            var keyboardItem = new ToolStripMenuItem(Language.Resource.Keyboard);
            var keyDownItem = new ToolStripMenuItem(Language.Resource.KeyDown);
            var keyUpItem = new ToolStripMenuItem(Language.Resource.KeyUp);
            var sendTextItem = new ToolStripMenuItem(Language.Resource.SendText);

            keyDownItem.Click += item_Click;
            keyUpItem.Click += item_Click;
            sendTextItem.Click += item_Click;

            keyboardItem.DropDownItems.Add(keyDownItem);
            keyboardItem.DropDownItems.Add(keyUpItem);
            keyboardItem.DropDownItems.Add(sendTextItem);

            var sqlItem = new ToolStripMenuItem(Language.Resource.Sql);
            var getDatabaseItem = new ToolStripMenuItem(Language.Resource.GetDatabase);
            var getTableItem = new ToolStripMenuItem(Language.Resource.GetTable);
            var getColumnItem = new ToolStripMenuItem(Language.Resource.GetColumn);
            var getRowItem = new ToolStripMenuItem(Language.Resource.GetRow);
            var executeQueryItem = new ToolStripMenuItem(Language.Resource.ExecuteQuery);

            getDatabaseItem.Click += item_Click;
            getTableItem.Click += item_Click;
            getColumnItem.Click += item_Click;
            getRowItem.Click += item_Click;
            executeQueryItem.Click += item_Click;

            sqlItem.DropDownItems.Add(getDatabaseItem);
            sqlItem.DropDownItems.Add(getTableItem);
            sqlItem.DropDownItems.Add(getColumnItem);
            sqlItem.DropDownItems.Add(getRowItem);
            sqlItem.DropDownItems.Add(executeQueryItem);

            var utilityItem = new ToolStripMenuItem(Language.Resource.Utility);
            var imageToTextItem = new ToolStripMenuItem(Language.Resource.ImageToText);
            var takesnapshotItem = new ToolStripMenuItem(Language.Resource.TakeSnapShot);
            var textToJsonItem = new ToolStripMenuItem(Language.Resource.TextToJson);
            var textToXmlItem = new ToolStripMenuItem(Language.Resource.TextToXml);
            var getAccountByItem = new ToolStripMenuItem(Language.Resource.GetAccount);
            var sendEmailItem = new ToolStripMenuItem(Language.Resource.SendEmail);

            imageToTextItem.Enabled = false;

            imageToTextItem.Click += item_Click;
            takesnapshotItem.Click += item_Click;
            textToJsonItem.Click += item_Click;
            textToXmlItem.Click += item_Click;
            getAccountByItem.Click += item_Click;
            sendEmailItem.Click += item_Click;

            utilityItem.DropDownItems.Add(imageToTextItem);
            utilityItem.DropDownItems.Add(takesnapshotItem);
            utilityItem.DropDownItems.Add(textToJsonItem);
            utilityItem.DropDownItems.Add(textToXmlItem);
            utilityItem.DropDownItems.Add(getAccountByItem);
            utilityItem.DropDownItems.Add(sendEmailItem);

            var excelItem = new ToolStripMenuItem(Language.Resource.Excel);
            var readExcelItem = new ToolStripMenuItem(Language.Resource.ReadCellExcel);
            var writeExcelItem = new ToolStripMenuItem(Language.Resource.WriteCellExcel);

            readExcelItem.Click += item_Click;
            writeExcelItem.Click += item_Click;

            excelItem.DropDownItems.Add(readExcelItem);
            excelItem.DropDownItems.Add(writeExcelItem);

            var exploreItem = new ToolStripMenuItem(Language.Resource.Explore);
            var runCommandItem = new ToolStripMenuItem(Language.Resource.RunCommand);
            var createfolderItem = new ToolStripMenuItem(Language.Resource.CreateFolder);
            var getfoldersItem = new ToolStripMenuItem(Language.Resource.GetFolders);
            var removefolderItem = new ToolStripMenuItem(Language.Resource.RemoveFolder);
            var createfileItem = new ToolStripMenuItem(Language.Resource.CreateFile);
            var getfilesItem = new ToolStripMenuItem(Language.Resource.GetFiles);
            var removefileItem = new ToolStripMenuItem(Language.Resource.RemoveFile);
            var downloadFileItem = new ToolStripMenuItem(Language.Resource.Download);
            var openExploreItem = new ToolStripMenuItem(Language.Resource.OpenExplore);
            var copyFolderItem = new ToolStripMenuItem(Language.Resource.CopyFolder);
            var copyFileItem = new ToolStripMenuItem(Language.Resource.CopyFile);

            runCommandItem.Click += item_Click;
            createfolderItem.Click += item_Click;
            getfoldersItem.Click += item_Click;
            removefolderItem.Click += item_Click;
            createfileItem.Click += item_Click;
            getfilesItem.Click += item_Click;
            removefileItem.Click += item_Click;
            downloadFileItem.Click += item_Click;
            openExploreItem.Click += item_Click;
            copyFolderItem.Click += item_Click;
            copyFileItem.Click += item_Click;

            exploreItem.DropDownItems.Add(runCommandItem);
            exploreItem.DropDownItems.Add(createfolderItem);
            exploreItem.DropDownItems.Add(getfoldersItem);
            exploreItem.DropDownItems.Add(removefolderItem);
            exploreItem.DropDownItems.Add(createfileItem);
            exploreItem.DropDownItems.Add(getfilesItem);
            exploreItem.DropDownItems.Add(removefileItem);
            exploreItem.DropDownItems.Add(downloadFileItem);
            exploreItem.DropDownItems.Add(openExploreItem);

            contextMenuBrowser.Items.Add(goItem);
            contextMenuBrowser.Items.Add(sleepItem);
            contextMenuBrowser.Items.Add(extractItem);
            contextMenuBrowser.Items.Add(fillItem);
            contextMenuBrowser.Items.Add(clickItem);
            contextMenuBrowser.Items.Add(mouseItem);
            contextMenuBrowser.Items.Add(keyboardItem);
            contextMenuBrowser.Items.Add(sqlItem);
            contextMenuBrowser.Items.Add(utilityItem);
            contextMenuBrowser.Items.Add(excelItem);
            contextMenuBrowser.Items.Add(exploreItem);
            exploreItem.DropDownItems.Add(copyFolderItem);
            exploreItem.DropDownItems.Add(copyFileItem);
        }

        int repeatCount = 0;
        string repeatItem = string.Empty;

        void item_Click(object sender, EventArgs e)
        {
            string xpath = GetXpath(htmlElm);
            contextMenuBrowser.Hide();

            string item = sender.ToString();
            if (item == Language.Resource.GetCurrentMouse)
            {
                tbxCode.AppendText("var x = getCurrentMouseX();" + Environment.NewLine);
                tbxCode.AppendText("var y = getCurrentMouseY();" + Environment.NewLine);
                tbxCode.AppendText("log(x + ',' + y);" + Environment.NewLine);
            }
            else if (item == Language.Resource.MouseMove)
            {
                tbxCode.AppendText("MouseMove(" + CurrentMouseX + ", " + CurrentMouseY + ", true, 10);" + Environment.NewLine);
            }
            else if (item == Language.Resource.MouseDown)
            {
                tbxCode.AppendText("MouseMove(" + CurrentMouseX + ", " + CurrentMouseY + ", true, 10);" + Environment.NewLine);
                tbxCode.AppendText("MouseDown('Left', 10);" + Environment.NewLine);
            }
            else if (item == Language.Resource.MouseUp)
            {
                tbxCode.AppendText("MouseMove(" + CurrentMouseX + ", " + CurrentMouseY + ", true, 10);" + Environment.NewLine);
                tbxCode.AppendText("MouseUp('Left', 10);" + Environment.NewLine);
            }
            else if (item == Language.Resource.MouseClick)
            {
                tbxCode.AppendText("MouseMove(" + CurrentMouseX + ", " + CurrentMouseY + ", true, 10);" + Environment.NewLine);
                tbxCode.AppendText("MouseClick('Left', 10);" + Environment.NewLine);
            }
            else if (item == Language.Resource.MouseDoubleClick)
            {
                tbxCode.AppendText("MouseMove(" + CurrentMouseX + ", " + CurrentMouseY + ", true, 10);" + Environment.NewLine);
                tbxCode.AppendText("MouseDoubleClick('Left', 10);" + Environment.NewLine);
            }
            else if (item == Language.Resource.MouseWheel)
            {
                tbxCode.AppendText("MouseMove(" + CurrentMouseX + ", " + CurrentMouseY + ", true, 10);" + Environment.NewLine);
                tbxCode.AppendText("MouseWheel(-15, 5);" + Environment.NewLine);
            }
            else if (item == Language.Resource.KeyDown)
            {
                tbxCode.AppendText("MouseMove(" + CurrentMouseX + ", " + CurrentMouseY + ", true, 10);" + Environment.NewLine);
                tbxCode.AppendText("KeyDown('A', 5);" + Environment.NewLine);
            }
            else if (item == Language.Resource.KeyUp)
            {
                tbxCode.AppendText("MouseMove(" + CurrentMouseX + ", " + CurrentMouseY + ", true, 10);" + Environment.NewLine);
                tbxCode.AppendText("KeyUp('A', 5);" + Environment.NewLine);
            }
            else if (item == Language.Resource.SendText)
            {
                string promptValue = Prompt.ShowDialog(Language.Resource.SendText, Language.Resource.Message, "", false);
                if (!string.IsNullOrEmpty(promptValue))
                {
                    tbxCode.AppendText("MouseMove(" + CurrentMouseX + ", " + CurrentMouseY + ", true, 10);" + Environment.NewLine);
                    tbxCode.AppendText("sendText('" + promptValue + "');" + Environment.NewLine);
                }
            }
            else if (item == Language.Resource.GetDatabase)
            {
                tbxCode.AppendText("//List All Database" + Environment.NewLine);
                tbxCode.AppendText("//Please login and create new item in Connection tab before use this function" + Environment.NewLine);
                tbxCode.AppendText("var listDatabases = getDatabases('localhost');" + Environment.NewLine);
                tbxCode.AppendText("log(listDatabases);" + Environment.NewLine);
            }
            else if (item == Language.Resource.GetTable)
            {
                tbxCode.AppendText("//List Tables from Database" + Environment.NewLine);
                tbxCode.AppendText("var tables = getTables('name', 'dbName');" + Environment.NewLine);
                tbxCode.AppendText("log(tables);" + Environment.NewLine);
            }
            else if (item == Language.Resource.GetColumn)
            {
                tbxCode.AppendText("//List columns from table" + Environment.NewLine);
                tbxCode.AppendText("var columns = getColumns('name', 'dbName', 'table');" + Environment.NewLine);
                tbxCode.AppendText("log(columns);" + Environment.NewLine);
            }
            else if (item == Language.Resource.GetRow)
            {
                tbxCode.AppendText("//List rows from table" + Environment.NewLine);
                tbxCode.AppendText("var rows = getRows('name', 'dbName', 'sql');" + Environment.NewLine);
                tbxCode.AppendText("log(rows);" + Environment.NewLine);
            }
            else if (item == Language.Resource.ExecuteQuery)
            {
                tbxCode.AppendText("//Execute query from database" + Environment.NewLine);
                tbxCode.AppendText("var query = excuteQuery('name', 'dbName', 'sql');" + Environment.NewLine);
                tbxCode.AppendText("log(query);" + Environment.NewLine);
            }
            else if (item == Language.Resource.ImageToText)
            {
                tbxCode.AppendText("var text = imageToText('" + xpath + "', 'vie');" + Environment.NewLine);
                tbxCode.AppendText("log(text);" + Environment.NewLine);
            }
            else if (item == Language.Resource.TakeSnapShot)
            {
                tbxCode.AppendText("//Take Snapshot" + Environment.NewLine);
                tbxCode.AppendText("var location = getCurrentPath() + '\\\\image.png';" + Environment.NewLine);
                tbxCode.AppendText("takesnapshot(location);" + Environment.NewLine);

            }
            else if (item == Language.Resource.TextToJson)
            {
                tbxCode.AppendText("//Text to JSON" + Environment.NewLine);
                tbxCode.AppendText("var text = textToJSON('{data: 123}');" + Environment.NewLine);
                tbxCode.AppendText("log(text.data);" + Environment.NewLine);
            }
            else if (item == Language.Resource.TextToXml)
            {
                tbxCode.AppendText("//Text to XML (Not test)" + Environment.NewLine);
                tbxCode.AppendText("var text = stringtoXML('text');" + Environment.NewLine);
                tbxCode.AppendText("log(text);" + Environment.NewLine);
            }
            else if (item == Language.Resource.GetAccount)
            {
                tbxCode.AppendText("//Get Account By Name" + Environment.NewLine);
                tbxCode.AppendText("var account = getAccountBy('name');" + Environment.NewLine);
                tbxCode.AppendText("log(account.Username + ' - ' + account.Password);" + Environment.NewLine);
            }
            else if (item == Language.Resource.SendEmail)
            {
                tbxCode.AppendText("//Send Email" + Environment.NewLine);
                tbxCode.AppendText("var email = sendEmail('name', 'email', 'subject', 'content');" + Environment.NewLine);
                tbxCode.AppendText("log(email);" + Environment.NewLine);
            }
            else if (item == Language.Resource.ReadCellExcel)
            {
                tbxCode.AppendText("//Read Cell in Excel file" + Environment.NewLine);
                tbxCode.AppendText("var readItem = readCellExcel('filePath', 'sheetname', 'row', 'column');" + Environment.NewLine);
                tbxCode.AppendText("log(readItem);" + Environment.NewLine);
            }
            else if (item == Language.Resource.WriteCellExcel)
            {
                tbxCode.AppendText("//Write Cell in Excel file" + Environment.NewLine);
                tbxCode.AppendText("writeCellExcel('filePath', 'sheetname', 'A1', 'value');" + Environment.NewLine);
            }
            else if (item == Language.Resource.RunCommand)
            {
                tbxCode.AppendText("//Run Command Line" + Environment.NewLine);
                tbxCode.AppendText("runcommand('path', 'parameters');" + Environment.NewLine);
            }
            else if (item == Language.Resource.CreateFolder)
            {
                tbxCode.AppendText("//Create Folder" + Environment.NewLine);
                tbxCode.AppendText("createfolder('path');" + Environment.NewLine);
            }
            else if (item == Language.Resource.GetFolders)
            {
                tbxCode.AppendText("//List Folder" + Environment.NewLine);
                tbxCode.AppendText("var folders = getfolders('path');" + Environment.NewLine);
                tbxCode.AppendText("log(folders);" + Environment.NewLine);
            }
            else if (item == Language.Resource.RemoveFolder)
            {
                tbxCode.AppendText("//Remove Folder" + Environment.NewLine);
                tbxCode.AppendText("removefolder('path');" + Environment.NewLine);
            }
            else if (item == Language.Resource.CreateFile)
            {
                tbxCode.AppendText("//Create File" + Environment.NewLine);
                tbxCode.AppendText("save('content', 'path', 'isAppend');" + Environment.NewLine);
            }
            else if (item == Language.Resource.GetFiles)
            {
                tbxCode.AppendText("//List File" + Environment.NewLine);
                tbxCode.AppendText("var files= getfiles('path');" + Environment.NewLine);
                tbxCode.AppendText("log(files);" + Environment.NewLine);
            }
            else if (item == Language.Resource.RemoveFile)
            {
                tbxCode.AppendText("//Remove File" + Environment.NewLine);
                tbxCode.AppendText("remove('path');" + Environment.NewLine);
            }
            else if (item == Language.Resource.Download)
            {
                tbxCode.AppendText("//Download File" + Environment.NewLine);
                tbxCode.AppendText("download('path', 'url');" + Environment.NewLine);
            }
            else if (item == Language.Resource.OpenExplore)
            {
                tbxCode.AppendText("//Open Explorer" + Environment.NewLine);
                tbxCode.AppendText("explorer('path');" + Environment.NewLine);
            }
            else if (item == Language.Resource.CopyFolder)
            {
                tbxCode.AppendText("//Copy" + Environment.NewLine);
                tbxCode.AppendText("copyfolder('source', 'destination', isIncludeSubFolder);" + Environment.NewLine);
            }
            else if (item == Language.Resource.CopyFile)
            {
                tbxCode.AppendText("//Copy" + Environment.NewLine);
                tbxCode.AppendText("copyfile('source', 'destination', isOverride);" + Environment.NewLine);
            }
            else if (item == Language.Resource.Go)
            {
                tbxCode.AppendText("//Open Website" + Environment.NewLine);
                if (MessageBox.Show(Language.Resource.ConfirmGoWebsite, Language.Resource.Message, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string address = tbxAddress.Text;
                    string promptValue = Prompt.ShowDialog(Language.Resource.Website, Language.Resource.Message, address, false);
                    if (!string.IsNullOrEmpty(promptValue))
                    {
                        tbxCode.AppendText("go(\"" + promptValue + "\");\n");
                    }
                }
                else
                {
                    tbxCode.AppendText("go(\"" + xpath + "\");\n");
                }
            }
            else if (item == Language.Resource.Sleep)
            {
                string promptValue = Prompt.ShowDialog(Language.Resource.Sleep, Language.Resource.Message, "1", false);
                if (MessageBox.Show(Language.Resource.ConfirmSleep, Language.Resource.Message, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    tbxCode.AppendText("sleep(" + promptValue + ",true);\n");
                }
                else
                {
                    tbxCode.AppendText("sleep(" + promptValue + ",false);\n");
                }
            }
            else if (item == Language.Resource.Attribute)
            {
                string promptValue = Prompt.ShowDialog(Language.Resource.Attribute, Language.Resource.Message, "", false);
                if (!string.IsNullOrEmpty(promptValue))
                {
                    tbxCode.AppendText("var attribute = extract(\"" + xpath + "\", \"" + promptValue + "\");" + Environment.NewLine);
                    tbxCode.AppendText("log(attribute);" + Environment.NewLine);
                }
            }
            else if (item == Language.Resource.Html)
            {
                tbxCode.AppendText("var html = extract(\"" + xpath + "\", \"html\");" + Environment.NewLine);
                tbxCode.AppendText("log(html);" + Environment.NewLine);
            }
            else if (item == Language.Resource.Src)
            {
                tbxCode.AppendText("var src = extract(\"" + xpath + "\", \"src\");" + Environment.NewLine);
                tbxCode.AppendText("log(src);" + Environment.NewLine);
            }
            else if (item == Language.Resource.Text)
            {
                tbxCode.AppendText("var text = extract(\"" + xpath + "\", \"text\");" + Environment.NewLine);
                tbxCode.AppendText("log(text);" + Environment.NewLine);
            }
            else if (item == Language.Resource.Url)
            {
                tbxCode.AppendText("var url = extract(\"" + xpath + "\", \"href\");" + Environment.NewLine);
                tbxCode.AppendText("log(url);" + Environment.NewLine);
            }
            else if (item == Language.Resource.Textbox)
            {
                string promptValue = Prompt.ShowDialog(Language.Resource.Textbox, Language.Resource.Message, "", false);
                if (!string.IsNullOrEmpty(promptValue))
                    tbxCode.AppendText("fill(\"" + xpath + "\", \"" + promptValue + "\");" + Environment.NewLine);
            }
            else if (item == Language.Resource.Dropdown)
            {
                string promptValue = Prompt.ShowDialog(Language.Resource.Dropdown, Language.Resource.Message, "", false);
                if (!string.IsNullOrEmpty(promptValue))
                    tbxCode.AppendText("filldropdown(\"" + xpath + "\", \"" + promptValue + "\");" + Environment.NewLine);
            }
            else if (item == Language.Resource.iFrame)
            {
                string promptValue = Prompt.ShowDialog(Language.Resource.iFrame, Language.Resource.Message, "", false);
                if (!string.IsNullOrEmpty(promptValue))
                    tbxCode.AppendText("filliframe(\"title\", \"" + promptValue + "\");" + Environment.NewLine);
            }
            else if (item == Language.Resource.Element)
            {
                tbxCode.AppendText("click(\"" + xpath + "\");" + Environment.NewLine);
            }
            else if (item == Language.Resource.FileUpload)
            {
                tbxCode.AppendText("setTimeout(function(){" + Environment.NewLine);
                tbxCode.AppendText("\tsendText('filePath');" + Environment.NewLine);
                tbxCode.AppendText("\tsleep(1, false);" + Environment.NewLine);
                tbxCode.AppendText("\tsendKeys('\\t');" + Environment.NewLine);
                tbxCode.AppendText("\tsleep(1, false);" + Environment.NewLine);
                tbxCode.AppendText("\tsendKeys('\\t');" + Environment.NewLine);
                tbxCode.AppendText("\tsleep(1, false);" + Environment.NewLine);
                tbxCode.AppendText("\tsendKeys('\\t');" + Environment.NewLine);
                tbxCode.AppendText("\tsendKeys('{ENTER}');" + Environment.NewLine);
                tbxCode.AppendText("}, 3000);" + Environment.NewLine);
                tbxCode.AppendText("click(\"" + xpath + "\");");
                //tbxCode.AppendText("fileUpload(\"" + xpath + "\", 'path');");
            }
            else if (item == Language.Resource.RepeatElement)
            {
                xpath = GetXpath(htmlElm);

                if (repeatCount == 0)
                {
                    repeatItem = xpath;
                    repeatCount++;
                }
                else
                {
                    int index = 0;

                    string begin = string.Empty;
                    string end = string.Empty;

                    int max = Math.Min(repeatItem.Length, xpath.Length);
                    while (index < max && repeatItem[index] == xpath[index]) index++;
                    if (index > 0)
                    {
                        if (repeatItem[index] == '/')
                        {
                            end = string.Empty;

                            var sb = new StringBuilder(repeatItem);
                            sb.Remove(index, 1);
                            begin = "1";
                            sb.Insert(index, "[\"+ i +\"]/");

                            int endIndex = 0;

                            for (int i = index + 1; i < xpath.Length; i++)
                            {
                                if (xpath[i] == ']')
                                {
                                    break;
                                }
                                else
                                {
                                    end += xpath[i];
                                }
                                endIndex++;
                            }

                            tbxCode.AppendText("for(i = 1; i <= " + end + "; i++)" + Environment.NewLine);
                            tbxCode.AppendText("{" + Environment.NewLine);
                            tbxCode.AppendText("\t" + "var text = extract(\"" + sb.ToString() + "\", \"text\");" + Environment.NewLine);
                            tbxCode.AppendText("\tlog(text);" + Environment.NewLine);
                            tbxCode.AppendText("}" + Environment.NewLine);
                        }
                        else
                        {
                            while (repeatItem[index] != '[')
                            {
                                index--;
                            }

                            index = index + 1;

                            int endIndex = 0;
                            end = string.Empty;

                            for (int i = index; i < repeatItem.Length; i++)
                            {
                                if (repeatItem[i] == ']')
                                {
                                    break;
                                }
                                else
                                {
                                    begin += repeatItem[i];
                                }
                                endIndex++;
                            }

                            for (int j = index; j < xpath.Length; j++)
                            {
                                if (xpath[j] == ']')
                                {
                                    break;
                                }
                                else
                                {
                                    end += xpath[j];
                                }
                            }

                            var sb = new StringBuilder(repeatItem);
                            sb.Remove(index, endIndex);
                            sb.Insert(index, "\"+ i +\"");
                            tbxCode.AppendText("for(i = " + begin + "; i <= " + end + "; i++)" + Environment.NewLine);
                            tbxCode.AppendText("{" + Environment.NewLine);
                            tbxCode.AppendText("\t" + "var text = extract(\"" + sb.ToString() + "\", \"text\");" + Environment.NewLine);
                            tbxCode.AppendText("\tlog(text);" + Environment.NewLine);
                            tbxCode.AppendText("}" + Environment.NewLine);
                        }

                        repeatCount = 0;
                        repeatItem = string.Empty;
                    }
                }
            }

            developerToolsToolStripMenuItem.Checked = false;
            developerToolsToolStripMenuItem_Click(this, null);
        }

        public void AddDownload(string title, string url, string folder, int segnments)
        {
            downloadList1.NewFileDownload(title, url, folder, segnments);
        }

        private string extractData(GeckoHtmlElement ele, string attribute)
        {
            var result = string.Empty;

            if (ele != null)
            {
                var tmp = ele.GetAttribute(attribute);
                /*if (tmp == null)
                {
                    tmp = extractData(ele.Parent, attribute);
                }*/
                if (tmp != null)
                    result = tmp.Trim();
            }

            return result;
        }

        #endregion

        #region WebBrowser

        public void CallBackWinAppWebBrowser()
        {
            wbMain = new WebBrowser();
            wbMain.ObjectForScripting = this;
            wbMain.ScriptErrorsSuppressed = true;

            wbMain.DocumentText = @"<html>
                                        <head>
                                            <script type='text/javascript'>
                                                var isAborted = false;

                                                function UnAbort() {isAborted = false; window.external.UnAbort();}
                                                function Abort() {isAborted = true; release(); window.external.Abort();}
                                                function CheckAbort() {if(isAborted == true) { window.external.Abort(); throw new Error('Aborted');} }

                                                /*var isAborted = false;
                                                function UnAbort() {isAborted = false;}
                                                function Abort() {isAborted = true;}
                                                function CheckAbort() {if(isAborted == true) throw new Error('Aborted');}*/

                                                function stringtoXML(data){ if (window.ActiveXObject){ var doc = new ActiveXObject('Microsoft.XMLDOM'); doc.async='false'; doc.loadXML(data); } else { var parser = new DOMParser(); var doc = parser.parseFromString(data,'text/xml'); }	return doc; }

                                                /* Open new tab */

                                                function release() { CheckAbort(); window.external.ReleaseMR();  }

                                                function countNodes(xpath) { CheckAbort(); return window.external.countNodes(xpath); } 

                                                function tabnew() { CheckAbort(); window.external.tabnew();}
                                                /* Close current tab  */
                                                function tabclose() { CheckAbort(); window.external.tabclose();}
                                                /* Close all tab  */
                                                function tabcloseall() { CheckAbort(); window.external.tabcloseall();}
                                                /* Go to website by url or xpath  */
                                                function go(a) { CheckAbort(); window.external.go(a);}
                                                
                                                function goWithProxy(url, proxyUrl){ CheckAbort(); window.external.goWithProxy(url, proxyUrl); }

                                                function back() { CheckAbort(); window.external.Back(); }
                                                function next() { CheckAbort(); window.external.Next(); }
                                                function reload() { CheckAbort(); window.external.Reload(); }
                                                function stop() { CheckAbort(); window.external.Stop(); }

                                                /* Sleep with a = miliseconds to sleep, b = true if wait until browser loading finished, b = false wait until timeout miliseconds  */
                                                function sleep(a, b) { CheckAbort(); window.external.sleep(a,b);}
                                                /* Quit application  */
                                                function exit() { CheckAbort(); window.external.exit();}
                                                /* Click by xpath  */
                                                function click(a) { CheckAbort(); window.external.click(a);}
                                                /* write a log to preview, a = content of log  */
                                                function log(a) { CheckAbort(); window.external.log(a);}
                                                /* clear log on the preview  */
                                                function clearlog() { CheckAbort(); window.external.clearlog();}
                                                /* extract data from xpath  */
                                                //function extract(a) {CheckAbort(); return window.external.extract(a);}
                                                function extract(xpath, type) {CheckAbort(); return window.external.extract(xpath, type);}

                                                function extractAll(xpath, type) {CheckAbort(); return textToJSON(window.external.extractAll(xpath, type));}

                                                function extractUntil(xpath, type){ CheckAbort(); return window.external.extractUntil(xpath, type); }

                                                function filliframe(title, value) { CheckAbort(); window.external.filliframe(title, value); }                                                

                                                /* fill xpath by value, a = xpath, b = value  */
                                                function fill(a,b) { CheckAbort(); window.external.fill(a,b);}
                                                /* convert extract string to object  */

                                                /*function filldropdown(a, b) { CheckAbort(); window.external.filldropdown(a, b); }*/
                                                function filldropdown(xpath, value) { CheckAbort(); window.external.filldropdown(xpath, value); }
                                                function toObject(a) {CheckAbort(); var wrapper= document.createElement('div'); wrapper.innerHTML= a; return wrapper;}
                                                function blockFlash(isBlock) { CheckAbort(); window.external.BlockFlash(isBlock); }

                                                /* browser get all link in the area of xpath, it will stop until program go all of link , a = xpath */
                                                function browser(a) {CheckAbort(); window.external.browser(a);}
                                                /* reset list website to unread so program can go back and browser continue */
                                                function resetlistwebsite() {CheckAbort(); window.external.ResetListWebsite();}
                                                /* take a snapshot from current website on current tab, a = location to save a snapshot */

                                                function takesnapshot(a) {CheckAbort(); window.external.TakeSnapshot(a);}
                                                /* reconigze text of image from url, a = url of image  */
                                                function imageToText(xpath, language) { CheckAbort(); return window.external.imgToText(xpath, language);}
                                                /* set value to file upload (not work in ie)  */
                                                function fileupload(a,b){CheckAbort(); window.external.FileUpload(a,b);}

                                                /* create folder, a = location  */
                                                function createfolder(a) { CheckAbort(); window.external.createfolder(a);}
                                                /* download file from url, a = url to download, b = location where file located  */
                                                function download(a,b) {CheckAbort(); window.external.download(a,b);}

                                                function downloadWebsite(url) { CheckAbort(); return window.external.DownloadWebsite(url); } 

                                                function getfiles(a) { CheckAbort(); return window.external.getfiles(a); }
                                                function getfolders(a) { CheckAbort(); return window.external.getfolders(a); }

                                                /* read a file, a = location of file  */
                                                function read(a) { CheckAbort(); return window.external.read(a);}
                                                /* save file, a = content of file, b = location of file to save, c = is file override (true: fill will be override, false: not override)  */
                                                function save(a,b,c) { CheckAbort(); return window.external.save(a,b,c);}
                                                /* remove a file, a = location of file will be removed */
                                                function remove(a) { CheckAbort(); window.external.remove(a);}
                                                function removefolder(a) {CheckAbort(); window.external.removefolder(a);}
                                                
                                                function copyfolder(a,b,c) {CheckAbort(); window.external.copyFolder(a,b,c);}
                                                function copyfile(a,b,c) {CheckAbort(); window.external.copyFile(a,b,c);}

                                                function replacetextinfile(a, b, c) { CheckAbort(); window.external.replaceTextinFile(a,b,c); }

                                                function explorer(a) { CheckAbort(); window.external.explorer(a); }

                                                /* run code from string, a = code to run  */
                                                function excute(a) { CheckAbort(); window.external.excute(a);}

                                                function logoff() { CheckAbort(); window.external.logoff();} 
                                                function lockworkstation() {CheckAbort(); window.external.lockworkstation();} 
                                                function forcelogoff() { CheckAbort(); window.external.forcelogoff();} 
                                                function reboot() { CheckAbort(); window.external.reboot();} 
                                                function shutdown() { CheckAbort(); window.external.shutdown();} 
                                                function hibernate() { CheckAbort(); window.external.hibernate();} 
                                                function standby() { CheckAbort(); window.external.standby();} 


                                                /* run application, a = location of application */
                                                function runcommand(path, parameters) { CheckAbort(); window.external.runcommand(path, parameters); }

                                                function createtask(a,b,c,d,e,f) { CheckAbort(); window.external.createtask(a,b,c,d,e,f); }
                                                function removetask(a) { CheckAbort(); window.external.removetask(a);}

                                                function generatekeys() { CheckAbort(); window.external.generatekeys();}
                                                function encrypt(a, b) { CheckAbort(); return window.external.encrypt(a, b);}
                                                function decrypt(a, b) { CheckAbort(); return window.external.decrypt(a, b);}

                                                function showpicture(a,b) { CheckAbort(); window.external.showimage(a,b); }
                                                function savefilterimage(a) { CheckAbort(); window.external.savefilterimage(a); }

                                                function writetextimage(a, b) {CheckAbort(); window.external.writetextimage(a,b); } 

                                                function getcurrenturl() {CheckAbort(); return window.external.getCurrentUrl();}

                                                function scrollto(a) {CheckAbort(); window.external.scrollto(a); }

                                                function getheight() { CheckAbort(); return window.external.getheight(); }

                                                function gettitle() { CheckAbort(); return window.external.gettitle(); } 

                                                function getlinks(a) { CheckAbort(); return window.external.getlinks(a); } 

                                                function getCurrentContent() { CheckAbort(); return window.external.getCurrentContent(); } 

                                                function getCurrentPath() { CheckAbort(); return window.external.getCurrentPath(); } 

                                                function checkelement(a) { CheckAbort(); return window.external.checkelement(a);}

                                                function readCellExcel(a, b, c, d) { CheckAbort(); return window.external.readCellExcel(a,b,c,d);}

                                                function writeCellExcel(a, b, c, d) { CheckAbort(); window.external.writeCellExcel(a,b,c,d); }

                                                function replaceMsWord(a, b, c, d) { CheckAbort(); window.external.replaceMsWord(a,b,c,d); } 

                                                function loadHTML(a) { CheckAbort(); window.external.loadHTML(a); }" +

                                                "function textToJSON(a) { CheckAbort(); var b = eval(\"(\" + window.external.textToJSON(a) + \")\"); return b; }" +

                                                @"function getCurrentLogin() { return textToJSON(window.external.getCurrentUser());}

                                                function login(a, b) { return window.external.login(a,b); }

                                                function register(a, b, c, d) { return window.external.register(a, b, c, d);}

                                                function getAccount(a) { CheckAbort(); var b = window.external.GetAccount(a); if(b == '') return ''; else return textToJSON(b); }

                                                function captchaborder(a,b) { CheckAbort(); window.external.CaptchaBorder(a,b); } 

                                                function saveImageFromElement(a,b) { CheckAbort(); window.external.SaveImageFromElement(a,b);}

                                                function getControlText(a,b,c) { CheckAbort(); return window.external.GetControlText(a,b,c); }

                                                function setControlText(a,b,c,d) { CheckAbort(); window.external.SetControlText(a,b,c,d); }

                                                function clickControl(a,b,c) { CheckAbort(); window.external.ClickControl(a,b,c); } 

                                                function getCurrentMouseX() { CheckAbort(); return window.external.GetCurrentMouseX(); } 

                                                function getCurrentMouseY() { CheckAbort(); return window.external.GetCurrentMouseY(); } 

                                                function MouseDown(a,b) { CheckAbort(); window.external.Mouse_Down(a,b); }

                                                function MouseUp(a,b) { CheckAbort(); window.external.Mouse_Up(a,b); }

                                                function MouseClick(a,b) { CheckAbort(); window.external.Mouse_Click(a,b); }

                                                function MouseDoubleClick(a,b) { CheckAbort(); window.external.Mouse_Double_Click(a,b); }

                                                function MouseMove(a,b,c,d) {CheckAbort(); window.external.Mouse_Show(a,b,c,d); }

                                                function MouseWheel(a,b) { CheckAbort(); window.external.Mouse_Wheel(a,b); }

                                                function KeyDown(a,b) { CheckAbort(); window.external.Key_Down(a,b); }

                                                function KeyUp(a,b) { CheckAbort(); window.external.Key_Up(a,b); }

                                                function sendText(a) { CheckAbort(); window.external.sendText(a); }

                                                function Reload() { CheckAbort(); window.external.Reload(); }

                                                function sendEmail(name, email, subject, content) { CheckAbort(); return window.external.sendEmail(name, email, subject, content); }" +

                                                "function getAccountBy(name) { CheckAbort(); var a = window.external.GetAccountBy(name); if(a != '') { return eval(\"(\" + a + \")\"); } else { return ''; } }" +

                                                @"function getDatabases(name) { CheckAbort(); return window.external.GetDatabases(name); } 

                                                function getTables(name, dbName) { CheckAbort(); return window.external.GetTables(name, dbName); }

                                                function getColumns(name, dbName, table) { CheckAbort(); return window.external.GetColumns(name, dbName, table); }

                                                function getRows(name, dbName, sql) { CheckAbort(); return window.external.GetRows(name, dbName, sql); }

                                                function excuteQuery(name, dbName, sql) { CheckAbort(); return window.external.ExcuteQuery(name, dbName, sql); } 

                                                function removeStopWords(text) { CheckAbort(); return window.external.RemoveStopWords(text); }

                                                function addElement(path, node1, node2, text) { CheckAbort(); return window.external.AddElement(path, node1, node2, text); }

                                                function checkXmlElement(path, node, text) { CheckAbort(); return window.external.CheckXmlElement(path, node, text); }

                                                function getXmlElement(path, node) { CheckAbort(); return window.external.GetXmlElement(path, node); }

                                                function getParentElement(path, node, text) { CheckAbort(); return window.external.GetParentElement(path, node, text); }
                                                
                                                function extractbyRegularExpression(pattern, groupName) { CheckAbort(); return window.external.ExtractUsingRegularExpression(pattern, groupName); }

                                                function addToDownload(fileName, url, folder) { CheckAbort(); return window.external.AddToDownload(fileName, url, folder); }

                                                function startDownload() { CheckAbort(); return window.external.StartDownload(); }

                                                function hide() { CheckAbort(); return window.external.MinimizeWindow(); }

                                                function sendKeys(key) { CheckAbort(); window.external.sendKeys(key); }

                                                function getArguments() { CheckAbort(); return window.external.GetArguments(); }

                                            </script>
                                        </head>
                                        <body>
                                            
                                        </body>
                                    </html>";
            this.Controls.Add(wbMain);
        }

        void ExcuteJSCodeWebBrowser(string code)
        {
            wbMain.Document.InvokeScript("UnAbort");
            object obj = wbMain.Document.InvokeScript("eval", new object[] { code });
        }

        void GoWebBrowser(string url)
        {
            if (String.IsNullOrEmpty(url)) return;
            if (url.Equals("about:blank")) return;

            GeckoWebBrowser wbBrowser = new GeckoWebBrowser();

            wbBrowser.ProgressChanged += wbBrowser_ProgressChanged;
            wbBrowser.Navigated += wbBrowser_Navigated;
            wbBrowser.DocumentCompleted += wbBrowser_DocumentCompleted;
            wbBrowser.CanGoBackChanged += wbBrowser_CanGoBackChanged;
            wbBrowser.CanGoForwardChanged += wbBrowser_CanGoForwardChanged;
            wbBrowser.ShowContextMenu += new EventHandler<GeckoContextMenuEventArgs>(wbBrowser_ShowContextMenu);
            wbBrowser.DomContextMenu += wbBrowser_DomContextMenu;
            wbBrowser.NoDefaultContextMenu = true;

            currentTab.Controls.Add(wbBrowser);
            wbBrowser.Dock = DockStyle.Fill;
            wbBrowser.Navigate(url);
        }

        void wbBrowser_ProgressChanged(object sender, GeckoProgressEventArgs e)
        {
            progressbar.Maximum = (int)e.MaximumProgress;
            var currentProgress = (int)e.CurrentProgress;
            if (currentProgress <= progressbar.Maximum)
                progressbar.Value = (int)e.CurrentProgress;
        }

        void wbBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            var wbBrowser = (GeckoWebBrowser)sender;
            if (wbBrowser != null)
            {
                toolStripNext.Enabled = wbBrowser.CanGoForward;
            }
            else
            {
                toolStripNext.Enabled = false;
            }
        }

        void wbBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            var wbBrowser = (GeckoWebBrowser)sender;
            if (wbBrowser != null)
            {
                toolStripBack.Enabled = wbBrowser.CanGoBack;
            }
            else
            {
                toolStripBack.Enabled = false;
            }
        }

        void wbBrowser_Navigated(object sender, GeckoNavigatedEventArgs e)
        {
            string url = string.Empty;
            url = ((GeckoWebBrowser)sender).Url.ToString();
            if (url != "about:blank")
                tbxAddress.Text = url;
        }

        void wbBrowser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            if (e.Uri.AbsolutePath != (sender as GeckoWebBrowser).Url.AbsolutePath)
                return; 

            GeckoWebBrowser wbBrowser = (GeckoWebBrowser)sender;

            string title = wbBrowser.DocumentTitle;
            currentTab.Text = (title.Length > 10 ? title.Substring(0, 10) + "..." : title);
            tbxAddress.Text = wbBrowser.Url.ToString();

            IsBreakSleep = true;
        }

        void wbBrowser_ShowContextMenu(object sender, GeckoContextMenuEventArgs e)
        {
            //contextMenuBrowser.Show(Cursor.Position);

            //CurrentMouseX = Cursor.Position.X;
            //CurrentMouseY = Cursor.Position.Y;

            /*GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                htmlElm = (GeckoHtmlElement)wb.Document.ElementFromPoint(Cursor.Position.X, Cursor.Position.Y);
                if (htmlElm != null)
                {
                    if (htmlElm.GetType().Name == "GeckoIFrameElement")
                    {
                        var iframe = (GeckoIFrameElement)wb.Document.GetElementById(htmlElm.Id);
                        if (iframe != null)
                        {
                            var contentDocument = iframe.ContentWindow.Document;
                            if (contentDocument != null)
                                htmlElm = (GeckoHtmlElement)contentDocument.ElementFromPoint(Cursor.Position.X, Cursor.Position.Y);
                        }
                    }
                }
            }*/
        }

        private int CurrentMouseX = 0;
        private int CurrentMouseY = 0;

        void wbBrowser_DomContextMenu(object sender, DomMouseEventArgs e)
        {
            if (e.Button.ToString().IndexOf("Right") != -1)
            {
                contextMenuBrowser.Show(Cursor.Position);

                CurrentMouseX = Cursor.Position.X;
                CurrentMouseY = Cursor.Position.Y;

                GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    htmlElm = (GeckoHtmlElement)wb.Document.ElementFromPoint(e.ClientX, e.ClientY);
                }
            }
        }

        void GoWebBrowserByXpath(string xpath)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                GeckoHtmlElement elm = GetCompleteElementByXPath(wb, xpath);
                if (elm != null)
                {
                    UpdateUrlAbsolute(wb.Document, elm);
                    string url = extractData(elm, "href");
                    if (!string.IsNullOrEmpty(url))
                        wb.Navigate(url);
                }
            }
        }

        void NextWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoForward();
            }
        }

        void BackWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoBack();
            }
        }

        void ReloadWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Refresh();
            }
        }

        void StopWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Stop();
            }
        }

        void TabSelectedWebBrowser()
        {
            if (tabMain.TabCount > 0)
            {
                GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    tbxAddress.Text = wb.Url.ToString();
                    string title = wb.DocumentTitle;
                    currentTab.Text = (title.Length > 10 ? title.Substring(0, 10) + "..." : title);
                    this.Text = title;
                }
            }
        }

        private GeckoHtmlElement GetElementByXpath(GeckoDocument doc, string xpath)
        {
            if (doc == null) return null;

            xpath = xpath.Replace("/html/", "");
            GeckoElementCollection eleColec = doc.GetElementsByTagName("html"); if (eleColec.Length == 0) return null;
            GeckoHtmlElement ele = eleColec[0];
            string[] tagList = xpath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tag in tagList)
            {
                System.Text.RegularExpressions.Match mat = System.Text.RegularExpressions.Regex.Match(tag, "(?<tag>.+)\\[@id='(?<id>.+)'\\]");
                if (mat.Success == true)
                {
                    string id = mat.Groups["id"].Value;
                    GeckoHtmlElement tmpEle = doc.GetHtmlElementById(id);
                    if (tmpEle != null) ele = tmpEle;
                    else
                    {
                        ele = null;
                        break;
                    }
                }
                else
                {
                    mat = System.Text.RegularExpressions.Regex.Match(tag, "(?<tag>.+)\\[(?<ind>[0-9]+)\\]");
                    if (mat.Success == false)
                    {
                        GeckoHtmlElement tmpEle = null;
                        foreach (GeckoNode it in ele.ChildNodes)
                        {
                            if (it.NodeName.ToLower() == tag)
                            {
                                tmpEle = (GeckoHtmlElement)it;
                                break;
                            }
                        }
                        if (tmpEle != null) ele = tmpEle;
                        else
                        {
                            ele = null;
                            break;
                        }
                    }
                    else
                    {
                        string tagName = mat.Groups["tag"].Value;
                        int ind = int.Parse(mat.Groups["ind"].Value);
                        int count = 0;
                        GeckoHtmlElement tmpEle = null;
                        foreach (GeckoNode it in ele.ChildNodes)
                        {
                            if (it.NodeName.ToLower() == tagName)
                            {
                                count++;
                                if (ind == count)
                                {
                                    tmpEle = (GeckoHtmlElement)it;
                                    break;
                                }
                            }
                        }
                        if (tmpEle != null) ele = tmpEle;
                        else
                        {
                            ele = null;
                            break;
                        }
                    }
                }
            }

            return ele;
        }

        private string GetXpath(GeckoNode node)
        {
            if (node == null)
                return string.Empty;

            if (node.NodeType == NodeType.Attribute)
            {
                return string.Format("{0}/@{1}", GetXpath(((GeckoAttribute)node).OwnerDocument), node.NodeName);
            }
            if (node.ParentNode == null)
            {
                return "";
            }
            string elementId = ((GeckoHtmlElement)node).Id;
            if (!String.IsNullOrEmpty(elementId))
            {
                return String.Format("//*[@id='{0}']", elementId);
            }

            int indexInParent = 1;
            GeckoNode siblingNode = node.PreviousSibling;

            while (siblingNode != null)
            {

                if (siblingNode.NodeName == node.NodeName)
                {
                    indexInParent++;
                }
                siblingNode = siblingNode.PreviousSibling;
            }

            return String.Format("{0}/{1}[{2}]", GetXpath(node.ParentNode), node.NodeName, indexInParent);
        }

        private int GetXpathIndex(GeckoHtmlElement ele)
        {
            if (ele.Parent == null) return 0;
            int ind = 0, indEle = 0;
            string tagName = ele.TagName;
            GeckoNodeCollection elecol = ele.Parent.ChildNodes;
            foreach (GeckoNode it in elecol)
            {
                if (it.NodeName == tagName)
                {
                    ind++;
                    if (it.TextContent == ele.TextContent) indEle = ind;
                }
            }
            if (ind > 1) return indEle;
            return 0;
        }

        protected void UpdateUrlAbsolute(GeckoDocument doc, GeckoHtmlElement ele)
        {
            string link = doc.Url.GetLeftPart(UriPartial.Authority);

            if (ele != null)
            {
                var eleColec = ele.GetElementsByTagName("IMG");
                foreach (GeckoHtmlElement it in eleColec)
                {
                    if (it.GetAttribute("src") != null &&
                        !it.GetAttribute("src").StartsWith("http"))
                        it.SetAttribute("src", link + it.GetAttribute("src"));
                }
                eleColec = ele.GetElementsByTagName("A");
                foreach (GeckoHtmlElement it in eleColec)
                {
                    if (it.GetAttribute("href") != null &&
                        !it.GetAttribute("href").StartsWith("http") &&
                        !it.GetAttribute("href").StartsWith("javascript"))
                    {
                        it.SetAttribute("href", link + it.GetAttribute("href"));
                    }
                }
            }
        }

        private GeckoHtmlElement GetCompleteElementByXPath(GeckoWebBrowser wb, string xpath)
        {
            GeckoHtmlElement elm = GetElement(wb, xpath);

            int waitUntil = 0;
            int count = 0;

            int.TryParse(MaxWait, out waitUntil);

            while (elm == null)
            {
                //Stop when click Stop button
                if (IsStop) break;

                //It will stop when get the limit configuration
                if (count > waitUntil) break;

                elm = GetElement(wb, xpath);
                sleep(1, false);
                count++;
            }

            return elm;
        }

        private GeckoHtmlElement GetElement(GeckoWebBrowser wb, string xpath)
        {
            GeckoHtmlElement elm = null;
            if (xpath.StartsWith("/"))
            {
                if (xpath.Contains("@"))
                {
                    var html = GetHtmlFromGeckoDocument(wb.Document);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);

                    var node = doc.DocumentNode.SelectSingleNode(xpath);
                    if (node != null)
                    {
                        xpath = "/" + node.XPath;
                    }
                    elm = (GeckoHtmlElement)wb.Document.EvaluateXPath(xpath).GetNodes().FirstOrDefault();
                }
                else
                {
                    elm = (GeckoHtmlElement)GetElementInIframe(wb, xpath).GetNodes().FirstOrDefault();
                }
            }
            else
            {
                elm = (GeckoHtmlElement)wb.Document.GetElementById(xpath);
            }
            return elm;
        }

        private XPathResult GetElementInIframe(GeckoWebBrowser wb, string xpath)
        {
            XPathResult elm = null;

            var iframes = wb.Document.GetElementsByTagName("iframe");
            if (iframes != null && iframes.Count() > 0)
            {
                foreach (GeckoIFrameElement iframe in iframes)
                {
                    if (xpath.StartsWith("/"))
                    {
                        var contentDocument = iframe.ContentDocument;
                        if (contentDocument != null)
                        {
                            elm = contentDocument.EvaluateXPath(xpath);
                            if (elm != null)
                                break;
                        }
                    }
                }
            }
            else
            {
                elm = wb.Document.EvaluateXPath(xpath);
            }

            return elm;
        }

        private List<GeckoNode> GetElements(GeckoWebBrowser wb, string xpath)
        {
            List<GeckoNode> elm = new List<GeckoNode>();
            if (xpath.StartsWith("/"))
            {
                if (xpath.Contains("@"))
                {
                    var html = GetHtmlFromGeckoDocument(wb.Document);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);

                    var node = doc.DocumentNode.SelectSingleNode(xpath);
                    if (node != null)
                    {
                        var currentXpath = xpath;
                        if (!node.XPath.StartsWith("/body"))
                        {
                            currentXpath = "/" + node.XPath;
                        }
                        elm = wb.Document.EvaluateXPath(currentXpath).GetNodes().ToList();
                    }
                }
                else
                {
                    elm = wb.Document.EvaluateXPath(xpath).GetNodes().ToList();
                }
            }
            return elm;
        }

        private string GetHtmlFromGeckoDocument(GeckoDocument doc)
        {
            var result = string.Empty;

            GeckoHtmlElement element = null;
            var geckoDomElement = doc.DocumentElement;
            if (geckoDomElement is GeckoHtmlElement)
            {
                element = (GeckoHtmlElement)geckoDomElement;
                result = element.InnerHtml;
            }

            return result;
        }

        #endregion  

        #region Excel

        public string readCellExcel(string filePath, string isheetname, int irow, int icolumn)
        {
            string result = "";
            try
            {
                using (StreamReader input = new StreamReader(filePath))
                {
                    NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(new NPOI.POIFS.FileSystem.POIFSFileSystem(input.BaseStream));
                    if (null == workbook)
                    {
                        result = "";
                    }

                    NPOI.HSSF.UserModel.HSSFFormulaEvaluator formulaEvaluator = new NPOI.HSSF.UserModel.HSSFFormulaEvaluator(workbook);
                    NPOI.HSSF.UserModel.HSSFDataFormatter dataFormatter = new NPOI.HSSF.UserModel.HSSFDataFormatter(new CultureInfo("vi-VN"));

                    NPOI.SS.UserModel.ISheet sheet = workbook.GetSheet(isheetname);
                    NPOI.SS.UserModel.IRow row = sheet.GetRow(irow);

                    if (row != null)
                    {
                        short minColIndex = row.FirstCellNum;
                        short maxColIndex = row.LastCellNum;

                        if (icolumn >= minColIndex || icolumn <= maxColIndex)
                        {
                            NPOI.SS.UserModel.ICell cell = row.GetCell(icolumn);
                            if (cell != null)
                            {
                                if (cell.CellType == NPOI.SS.UserModel.CellType.FORMULA)
                                {
                                    result = cell.StringCellValue;
                                }
                                else
                                {
                                    result = dataFormatter.FormatCellValue(cell, formulaEvaluator);
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }

            return result;
        }

        public void writeCellExcel(string filePath, string sheetname, string cellName, string value)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook workbook;
            if (!File.Exists(filePath))
            {
                workbook = NPOI.HSSF.UserModel.HSSFWorkbook.Create(NPOI.HSSF.Model.InternalWorkbook.CreateWorkbook());
                var sheet = workbook.CreateSheet(sheetname);
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
            using (var input = new StreamReader(filePath))
            {
                workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(new NPOI.POIFS.FileSystem.POIFSFileSystem(input.BaseStream));
                if (workbook != null)
                {
                    var sheet = workbook.GetSheet(sheetname);
                    NPOI.SS.Util.CellReference celRef = new NPOI.SS.Util.CellReference(cellName); 
                    var row = sheet.GetRow(celRef.Row);
                    if (row == null)
                        row = sheet.CreateRow(celRef.Row);

                    var cell = row.GetCell(celRef.Col);
                    if (cell == null)
                        cell = row.CreateCell(celRef.Col);

                    cell.SetCellValue(value);
                }
            }

            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                workbook.Write(file);
                file.Close();
            }
        }

        #endregion

        #region Init Language

        public void InitLanguage()
        {
            aboutToolStripMenuItem.Text = Language.Resource.About;
            fileToolStripMenuItem.Text = Language.Resource.File;
            exitToolStripMenuItem.Text = Language.Resource.Exit; ;
            toolStripReload.Text = Language.Resource.Reload;
            btnNewTab.Text = Language.Resource.NewTab;
            btnCloseTab.Text = Language.Resource.CloseTab;
            btnCloseAllTab.Text = Language.Resource.CloseAllTab;
            toolsToolStripMenuItem.Text = Language.Resource.Tools;
            configToolStripMenuItem.Text = Language.Resource.Configs;
            developerToolsToolStripMenuItem.Text = Language.Resource.DeveloperTools;
            scrollToViewToolStripMenuItem.Text = Language.Resource.ScrollToView;
            colorElementToolStripMenuItem.Text = Language.Resource.ColorElement;
            cfgShowImages.Text = Language.Resource.ShowImages;
            helpToolStripMenuItem.Text = Language.Resource.Help;
            checkForUpdateToolStripMenuItem.Text = Language.Resource.CheckUpdate;
            btnGo.Text = Language.Resource.Go;

            loginToolStripMenuItem.Text = Language.Resource.Login;

            btnShowHideDeveloperTool.Text = Language.Resource.ShowDeveloperTools;
            btnDetection.Text = Language.Resource.ShowDetection;
            registerToolStripMenuItem.Text = Language.Resource.Register;

            tabPage1.Text = Language.Resource.Code;
            tabTemplate.Text = Language.Resource.Template;
            tabPage2.Text = Language.Resource.Preview;
            tbxAutoBot.Text = Language.Resource.Autobot;

            toolStripRunning.Text = Language.Resource.Run;
            btnRunTwo.Text = Language.Resource.Run;

            btnNewScript.Text = Language.Resource.New;
            btnOpenScript.Text = Language.Resource.Open;
            btnSaveScript.Text = Language.Resource.Save;
            btnSaveAsScript.Text = Language.Resource.SaveAs;
            btnScriptClear.Text = Language.Resource.Clear;

            newScriptToolStripMenuItem.Text = Language.Resource.New;
            openToolStripMenuItem.Text = Language.Resource.Open;
            saveToolStripMenuItem.Text = Language.Resource.Save;
            saveAsToolStripMenuItem.Text = Language.Resource.SaveAs;

            btnNewTemplate.Text = Language.Resource.New;
            btnOpenTemplate.Text = Language.Resource.Open;
            btnSaveTemplate.Text = Language.Resource.Save;
            btnSaveAsTemplate.Text = Language.Resource.SaveAs;
            btnTemplateClear.Text = Language.Resource.Clear;
            btnRecord.Text = Language.Resource.Record;

            managerToolStripMenuItem.Text = Language.Resource.Manager;
            hideWindowToolStripMenuItem.Text = Language.Resource.HideWindow;

            notifyIconAutomation.BalloonTipText = Language.Resource.BallonTipText;
            notifyIconAutomation.BalloonTipTitle = Language.Resource.BallonTipText;

            toolStripStatus.Text = "";

            sddLanguage.Text = Language.Resource.English;

            btnRegconization.Text = Language.Resource.StartRegconization;
            
            tabDownload.Text = Language.Resource.Download;
            toolNewDownload.Text = Language.Resource.NewDownload;
            toolStart.Text = Language.Resource.Start;
            toolPause.Text = Language.Resource.Pause;
            toolPauseAll.Text = Language.Resource.PauseAll;
            toolRemove.Text = Language.Resource.Delete;
            toolRemoveCompleted.Text = Language.Resource.DeleteCompleted;

            downloadList1.InitLanguage();

            this.Text = Language.Resource.WebAutomation + " - " + Language.Resource.Version + " - " + Version;
        }

        #endregion

        #region Mouse Keyboard Library Event

        void mouseHook_MouseMove(object sender, MouseEventArgs e)
        {
            tbxCode.AppendText("MouseMove(" + e.X + "," + e.Y + ",true, " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine);
            lastTimeRecorded = Environment.TickCount;
        }

        void mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            tbxCode.AppendText("MouseDown('" + e.Button.ToString() + "', " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine);
            lastTimeRecorded = Environment.TickCount;
        }

        void mouseHook_MouseUp(object sender, MouseEventArgs e)
        {
            tbxCode.AppendText("MouseUp('" + e.Button.ToString() + "', " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine);
            lastTimeRecorded = Environment.TickCount;
        }

        void keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            tbxCode.AppendText("KeyDown('" + e.KeyCode + "', " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine);
            lastTimeRecorded = Environment.TickCount;
        }

        void keyboardHook_KeyUp(object sender, KeyEventArgs e)
        {
            tbxCode.AppendText("KeyUp('" + e.KeyCode + "', " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine);
            lastTimeRecorded = Environment.TickCount;
        }

        void mouseHook_MouseWheel(object sender, MouseEventArgs e)
        {
            tbxCode.AppendText("MouseWheel(" + e.Delta + ", " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine);
            lastTimeRecorded = Environment.TickCount;
        }

        #endregion

    }
}
