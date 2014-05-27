using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using AForge.Imaging.Filters;
using AForge.Imaging.Textures;
using AForge;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Xml;
using ThangDC.Core.Entities;
using System.Runtime.InteropServices;
using ThangDC.Core.WebAutomation;
using System.Security.Permissions;
using System.Speech.Recognition;
using System.Diagnostics;
using System.Web.Script.Serialization;
using mshtml;
using System.Reflection;

namespace Automation
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class frmMain : Form, Automation.App_Code.BrowserOptions.IOleClientSite
    {
        #region IOleClientSite Members

        private Automation.App_Code.BrowserOptions.BrowserOption webBrowserOptions = Automation.App_Code.BrowserOptions.BrowserOption.DontRunActiveX | Automation.App_Code.BrowserOptions.BrowserOption.NoClientPull | Automation.App_Code.BrowserOptions.BrowserOption.NoJava |Automation.App_Code.BrowserOptions.BrowserOption.NoActiveXDownload;

        [DispId(-5512)]
        public virtual int IDispatch_Invoke_Handler()
        {
            System.Diagnostics.Debug.WriteLine("-5512");
            return (int)webBrowserOptions;
        }

        public int SaveObject()
        {
            return 0;
        }

        public int GetMoniker(int dwAssign, int dwWhichMoniker, out object moniker)
        {
            moniker = this;
            return 0;
        }

        public int GetContainer(out object container)
        {
            container = this;
            return 0;
        }

        public int ShowObject()
        {
            return 0;
        }

        public int OnShowWindow(int fShow)
        {
            return 0;
        }

        public int RequestNewObjectLayout()
        {
            return 0;
        }

        #endregion

        #region Mouse Keyboard Library

        private int lastTimeRecorded = 0;
        private MouseKeyboardLibrary.MouseHook mouseHook = new MouseKeyboardLibrary.MouseHook();
        private MouseKeyboardLibrary.KeyboardHook keyboardHook = new MouseKeyboardLibrary.KeyboardHook();

        #endregion

        #region static variable

        private bool IsStop = false;
        private TabPage currentTab = null;
        private string currentUrl = "";

        //WebBrowser Main
        private WebBrowser wbMain;
        private HtmlDocument htmlDoc;
        private HtmlElement htmlElm;

        private List<Website> lstWebsite = new List<Website>();
        private List<Website> lstTemp = new List<Website>();
        private List<Website> lstAllLink = new List<Website>();
        private string LastScriptFile = "";
        private string LastTemplateFile = "";

        private User CurrentUser = null;

        public string Version = "1.0.0";

        #endregion

        #region static cleaner

        [StructLayout(LayoutKind.Explicit, Size = 80)]
        public struct INTERNET_CACHE_ENTRY_INFOA
        {
            [FieldOffset(0)]
            public uint dwStructSize;
            [FieldOffset(4)]
            public IntPtr lpszSourceUrlName;
            [FieldOffset(8)]
            public IntPtr lpszLocalFileName;
            [FieldOffset(12)]
            public uint CacheEntryType;
            [FieldOffset(16)]
            public uint dwUseCount;
            [FieldOffset(20)]
            public uint dwHitRate;
            [FieldOffset(24)]
            public uint dwSizeLow;
            [FieldOffset(28)]
            public uint dwSizeHigh;
            [FieldOffset(32)]
            public FILETIME LastModifiedTime;
            [FieldOffset(40)]
            public FILETIME ExpireTime;
            [FieldOffset(48)]
            public FILETIME LastAccessTime;
            [FieldOffset(56)]
            public FILETIME LastSyncTime;
            [FieldOffset(64)]
            public IntPtr lpHeaderInfo;
            [FieldOffset(68)]
            public uint dwHeaderInfoSize;
            [FieldOffset(72)]
            public IntPtr lpszFileExtension;
            [FieldOffset(76)]
            public uint dwReserved;
            [FieldOffset(76)]
            public uint dwExemptDelta;
        }

        // For PInvoke: Initiates the enumeration of the cache groups in the Internet cache
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindFirstUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindFirstUrlCacheGroup(
            int dwFlags,
            int dwFilter,
            IntPtr lpSearchCondition,
            int dwSearchCondition,
            ref long lpGroupId,
            IntPtr lpReserved);

        // For PInvoke: Retrieves the next cache group in a cache group enumeration
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindNextUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool FindNextUrlCacheGroup(
            IntPtr hFind,
            ref long lpGroupId,
            IntPtr lpReserved);

        // For PInvoke: Releases the specified GROUPID and any associated state in the cache index file
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "DeleteUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool DeleteUrlCacheGroup(
            long GroupId,
            int dwFlags,
            IntPtr lpReserved);

        // For PInvoke: Begins the enumeration of the Internet cache
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindFirstUrlCacheEntryA",
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindFirstUrlCacheEntry(
            [MarshalAs(UnmanagedType.LPTStr)] string lpszUrlSearchPattern,
            IntPtr lpFirstCacheEntryInfo,
            ref int lpdwFirstCacheEntryInfoBufferSize);

        // For PInvoke: Retrieves the next entry in the Internet cache
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindNextUrlCacheEntryA",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool FindNextUrlCacheEntry(
            IntPtr hFind,
            IntPtr lpNextCacheEntryInfo,
            ref int lpdwNextCacheEntryInfoBufferSize);

        // For PInvoke: Removes the file that is associated with the source name from the cache, if the file exists
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "DeleteUrlCacheEntryA",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool DeleteUrlCacheEntry(
            IntPtr lpszUrlName);

        #endregion

        #region static shutdown, restart, logoff, hibernate
        [DllImport("user32.dll")]
        public static extern void LockWorkStation();
        [DllImport("user32.dll")]
        public static extern int ExitWindowsEx(int uFlags, int dwReason);

        #endregion

        #region Mouse Keyboard Init Event

        private void InitMouseKeyBoardEvent()
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

            CallBackWinAppWebBrowser();

            InitMouseKeyBoardEvent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            FormLoad();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            exit();
        }

        private void KeyEvent(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 123:
                    TooglePanel();
                    if (developerToolsToolStripMenuItem.Checked) tbxCode.Focus();
                    break;
                case 116:
                    toolStripRunning_Click(this, null);
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

        void doc_MouseMove(object sender, HtmlElementEventArgs e)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement ele = wb.Document.GetElementFromPoint(e.ClientMousePosition);
                CheckElement(ele);

                toolStripStatus.Text = GetXpath(ele);
            }
        }

        private void contextMenuBrowser_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string xpath = GetXpath(htmlElm);

            if (e.ClickedItem.ToString() == "Go")
            {
                if (MessageBox.Show(Language.Resource.ConfirmGoWebsite, Language.Resource.Message, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string t = tbxAddress.Text;
                    string promptValue = Prompt.ShowDialog(Language.Resource.Website, Language.Resource.Message, t, false);
                    if (!string.IsNullOrEmpty(promptValue))
                    {
                        tbxCode.Text += "go(\"" + promptValue + "\");\n";
                    }
                }
                else
                {
                    tbxCode.Text += "go(\"" + xpath + "\");\n";
                }
            }
            else if (e.ClickedItem.ToString() == "Sleep")
            {
                string promptValue = Prompt.ShowDialog(Language.Resource.Sleep, Language.Resource.Message, "1", false);
                if (MessageBox.Show(Language.Resource.ConfirmSleep, Language.Resource.Message, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    tbxCode.Text += "sleep(" + promptValue + ",true);\n";
                }
                else
                {
                    tbxCode.Text += "sleep(" + promptValue + ",false);\n";
                }

            }
            else if (e.ClickedItem.ToString() == "Fill")
            {
                string promptValue = Prompt.ShowDialog(Language.Resource.Fill, Language.Resource.Message, "", false);
                if (!string.IsNullOrEmpty(promptValue))
                {
                    tbxCode.Text += "fill(\"" + xpath + "\",\"" + promptValue + "\");\n";
                }
            }
            else if (e.ClickedItem.ToString() == "Click")
            {
                tbxCode.Text += "click(\"" + xpath + "\");\n";
            }
            else if (e.ClickedItem.ToString() == "Extract")
            {
                tbxCode.Text += "extract(\"" + xpath + "\");\n";
            }
            else if (e.ClickedItem.ToString() == "Browser")
            {
                tbxCode.Text += "browser(\"" + xpath + "\");\n";
            }
            else if (e.ClickedItem.ToString() == "FileUpload")
            {
                tbxCode.Text += "fileupload(\"path\",\"" + xpath + "\");\n";
            }
            else if (e.ClickedItem.ToString() == "FillDropdown")
            {
                tbxCode.Text += "filldropdown(\"" + xpath + "\",1);\n";
            }
            else if (e.ClickedItem.ToString() == "CheckElement")
            {
                tbxCode.Text += "checkelement(\"" + xpath + "\");\n";
            }

            developerToolsToolStripMenuItem.Checked = false;
            developerToolsToolStripMenuItem_Click(this, null);
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

        private void tbxCode_CharAdded(object sender, ScintillaNET.CharAddedEventArgs e)
        {
            /*if (e.Ch == '.')
            {
                System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();

                t.Interval = 10;
                t.Tag = tbxCode;
                t.Tick += new EventHandler((obj, ev) =>
                {
                    List<string> s = new List<string>();
                    s.Add("UnAbort");
                    s.Add("Abort");
                    s.Add("CheckAbort");
                    s.Add("tabnew");
                    s.Add("tabclose");
                    s.Add("tabcloseall");
                    s.Add("go");
                    s.Add("sleep");
                    s.Add("exit");
                    s.Add("click");
                    s.Add("log");
                    s.Add("clearlog");
                    s.Add("extract");
                    s.Add("fill");
                    s.Add("filldropdown");
                    s.Add("toObject");
                    s.Add("browser");
                    s.Add("resetlistwebsite");
                    s.Add("takesnapshot");
                    s.Add("imagetotext");
                    s.Add("fileupload");
                    s.Add("createfolder");
                    s.Add("download");
                    s.Add("getfiles");
                    s.Add("getfolders");
                    s.Add("read");
                    s.Add("save");
                    s.Add("remove");
                    s.Add("removefolder");
                    s.Add("explorer");
                    s.Add("excute");
                    s.Add("logoff");
                    s.Add("lockworkstation");
                    s.Add("forcelogoff");
                    s.Add("reboot");
                    s.Add("shutdown");
                    s.Add("hibernate");
                    s.Add("standby");
                    s.Add("runcommand");
                    s.Add("createtask");
                    s.Add("removetask");
                    s.Add("generatekeys");
                    s.Add("encrypt");
                    s.Add("decrypt");
                    s.Add("showpicture");
                    s.Add("savefilterimage");
                    s.Add("writetextimage");
                    s.Add("getcurrenturl");
                    s.Add("scrollto");
                    s.Add("getheight");
                    s.Add("gettitle");
                    s.Add("getlinks");
                    s.Add("getCurrentContent");
                    s.Add("getCurrentPath");
                    s.Add("checkelement");
                    s.Add("readCellExcel");
                    s.Add("writeCellExcel");
                    s.Add("replaceMsWord");
                    s.Add("loadHTML");
                    s.Add("textToJSON");
                    s.Add("login");
                    s.Add("register");
                    s.Add("getAccount");
                    s.Add("ocr");
                    s.Add("captchaborder");
                    s.Add("saveImageFromElement");
                    s.Add("getControlText");
                    s.Add("setControlText");
                    s.Add("clickControl");
                    s.Add("getCurrentMouseX");
                    s.Add("getCurrentMouseY");
                    s.Add("MouseDown");
                    s.Add("MouseUp");
                    s.Add("MouseClick");
                    s.Add("MouseDoubleClick");
                    s.Add("MouseMove");
                    s.Add("MouseWheel");
                    s.Add("KeyDown");
                    s.Add("KeyUp");
                    s.Add("Reload");
                    s.Add("getConnection");
                    s.Add("getEmailAccount");
                    s.Add("sendEmail");
                    s.Add("locdau");
                    s.Sort(); // don't forget to sort it

                    tbxCode.AutoComplete.ShowUserList(0, s);

                    t.Stop();
                    t.Enabled = false;
                    t.Dispose();
                });
                t.Start();
            }*/
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

            MachineAnswer(user_input);

        }

        #endregion

        #endregion

        #region Event Funtions

        #region Menu Event Functions

        public void NewScript()
        {
            ResetListWebsite();
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

            if (result == 1) { CurrentUser = user.GetBy(username); ; }
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
            this.Dispose();
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
            About frmAbout = new About();
            frmAbout.Show();
        }

        public void CheckUpdateClick()
        {
            WebClient wc = new WebClient();
            string data = wc.DownloadString("https://thangdc.googlecode.com/svn/trunk/WebAutomation/WebAutomation.xml");
            if (data != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                string version = doc.DocumentElement.SelectSingleNode("/root/version").InnerText;
                string name = doc.DocumentElement.SelectSingleNode("/root/name").InnerText;
                string url = doc.DocumentElement.SelectSingleNode("/root/url").InnerText;
                string content = doc.DocumentElement.SelectSingleNode("/root/content").InnerText;

                if (version != Application.ProductVersion)
                {
                    var dialog = MessageBox.Show(Language.Resource.ConfirmUpdate + "\r\n\r\n" + content, Language.Resource.NewVersion, MessageBoxButtons.YesNo);
                    if (dialog == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(url);
                    }
                }
                else
                {
                    MessageBox.Show(Language.Resource.OldVersion, Language.Resource.Message);
                }
            }
        }

        public void ShowWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
        }

        public void Regconization()
        {
            if (btnRegconization.Text == Language.Resource.StartRegconization)
            {
                btnRegconization.Text = Language.Resource.StopRegconization;
                InitSpeechToText();
                toolStripStatus.Text = Language.Resource.StartRegconization;
            }
            else
            {
                btnRegconization.Text = Language.Resource.StartRegconization;
                DisposeSpeechToText();
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
            ReleaseMemory.FlushMemory();
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
                    ReleaseMemory.FlushMemory();
                }
                catch { }
            }
            currentTab = null;
        }

        public void ShowHideDeveloperTools()
        {
            developerToolsToolStripMenuItem_Click(this, null);
            TooglePanel();
            if (developerToolsToolStripMenuItem.Checked) tbxCode.Focus();
        }

        public void DetectionClick()
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (btnDetection.Text == Language.Resource.ShowDetection)
            {
                btnDetection.Text = Language.Resource.HideDetection;

                if (wb != null)
                {
                    HtmlDocument doc = wb.Document;
                    doc.MouseMove += doc_MouseMove;
                }
            }
            else
            {
                btnDetection.Text = Language.Resource.ShowDetection;

                if (wb != null)
                {
                    HtmlDocument doc = wb.Document;
                    doc.MouseMove -= doc_MouseMove;
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

                System.IO.Stream file = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Automation.Resources.vn.png");
                sddLanguage.Image = Image.FromStream(file);
            }
            else
            {
                Language.Resource.Culture = CultureInfo.CreateSpecificCulture("en-US");
                sddLanguage.Text = Language.Resource.English;

                System.IO.Stream file = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Automation.Resources.usa.png");
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
                    currentTab.Controls.RemoveAt(0);
                }

                GoWebBrowser(url);
            }
            else
            {
                GoWebBrowserByXpath(url);
            }
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

        private void FormLoad()
        {
            TooglePanel();

            this.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyEvent);

            Language.Resource.Culture = CultureInfo.CreateSpecificCulture("vi-VN");
            InitLanguage();
            CheckManager();
            Message message = new Message();
            WndProc(ref message);
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

        #endregion

        #region functions

        public string extract(string xpath)
        {
            string result = "";

            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement elm = GetElementByXpath(wb.Document, xpath);
                if (elm != null)
                {
                    UpdateUrlAbsolute(elm);
                    result = elm.OuterHtml;
                }
            }

            return result;
        }

        public string countNodes(string xpath)
        {
            string result = "";

            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                if (wb.Document != null && wb.Document.Body != null)
                {
                    doc.LoadHtml(wb.Document.Body.InnerHtml);
                    HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//" + xpath.Remove(0, xpath.IndexOf("body/") + 5));
                    if (nodes != null)
                    {
                        result = nodes.Count.ToString();
                    }
                }
            }

            return result;
        }

        /*public void filldropdown(string xpath, string index)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement elm = GetElementByXpath(wb.Document, xpath);
                if (elm != null)
                {
                    try
                    {
                        elm.Focus();
                    }
                    catch { }
                    elm.SetAttribute("selectedIndex", index);
                }
            }
        }*/

        public void filldropdown(string xpath, string value)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement elm = GetElementByXpath(wb.Document, xpath);
                if (elm != null)
                {
                    try
                    {
                        elm.Focus();
                    }
                    catch { }

                    int index = 0;
                    foreach (HtmlElement item in elm.Children)
                    {
                        if (value.Equals(item.InnerText))
                        {
                            elm.SetAttribute("selectedIndex", index.ToString());
                            elm.RaiseEvent("onChange");
                            break;
                        }
                        index++;
                    }
                }
            }
        }

        public void click(string xpath)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement elm = GetElementByXpath(wb.Document, xpath);
                
                if (elm != null)
                {
                    try
                    {
                        elm.Focus();
                    }
                    catch { }
                    elm.InvokeMember("click");
                }
            }
        }

        private void Cleaner()
        {
            // Indicates that all of the cache groups in the user's system should be enumerated
            const int CACHEGROUP_SEARCH_ALL = 0x0;
            // Indicates that all the cache entries that are associated with the cache group
            // should be deleted, unless the entry belongs to another cache group.
            const int CACHEGROUP_FLAG_FLUSHURL_ONDELETE = 0x2;
            // File not found.
            const int ERROR_FILE_NOT_FOUND = 0x2;
            // No more items have been found.
            const int ERROR_NO_MORE_ITEMS = 259;
            // Pointer to a GROUPID variable
            long groupId = 0;

            // Local variables
            int cacheEntryInfoBufferSizeInitial = 0;
            int cacheEntryInfoBufferSize = 0;
            IntPtr cacheEntryInfoBuffer = IntPtr.Zero;
            INTERNET_CACHE_ENTRY_INFOA internetCacheEntry;
            IntPtr enumHandle = IntPtr.Zero;
            bool returnValue = false;

            // Delete the groups first.
            // Groups may not always exist on the system.
            // For more information, visit the following Microsoft Web site:
            // http://msdn.microsoft.com/library/?url=/workshop/networking/wininet/overview/cache.asp			
            // By default, a URL does not belong to any group. Therefore, that cache may become
            // empty even when the CacheGroup APIs are not used because the existing URL does not belong to any group.			
            enumHandle = FindFirstUrlCacheGroup(0, CACHEGROUP_SEARCH_ALL, IntPtr.Zero, 0, ref groupId, IntPtr.Zero);
            // If there are no items in the Cache, you are finished.
            if (enumHandle != IntPtr.Zero && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                return;

            // Loop through Cache Group, and then delete entries.
            while (true)
            {
                // Delete a particular Cache Group.
                returnValue = DeleteUrlCacheGroup(groupId, CACHEGROUP_FLAG_FLUSHURL_ONDELETE, IntPtr.Zero);
                if (!returnValue && ERROR_FILE_NOT_FOUND == Marshal.GetLastWin32Error())
                {
                    returnValue = FindNextUrlCacheGroup(enumHandle, ref groupId, IntPtr.Zero);
                }

                if (!returnValue && (ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error() || ERROR_FILE_NOT_FOUND == Marshal.GetLastWin32Error()))
                    break;
            }

            // Start to delete URLs that do not belong to any group.
            enumHandle = FindFirstUrlCacheEntry(null, IntPtr.Zero, ref cacheEntryInfoBufferSizeInitial);
            if (enumHandle == IntPtr.Zero && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                return;

            cacheEntryInfoBufferSize = cacheEntryInfoBufferSizeInitial;
            cacheEntryInfoBuffer = Marshal.AllocHGlobal(cacheEntryInfoBufferSize);
            enumHandle = FindFirstUrlCacheEntry(null, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial);

            while (true)
            {
                internetCacheEntry = (INTERNET_CACHE_ENTRY_INFOA)Marshal.PtrToStructure(cacheEntryInfoBuffer, typeof(INTERNET_CACHE_ENTRY_INFOA));

                cacheEntryInfoBufferSizeInitial = cacheEntryInfoBufferSize;
                returnValue = DeleteUrlCacheEntry(internetCacheEntry.lpszSourceUrlName);
                if (!returnValue)
                {
                    returnValue = FindNextUrlCacheEntry(enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial);
                }
                if (!returnValue && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                {
                    break;
                }
                if (!returnValue && cacheEntryInfoBufferSizeInitial > cacheEntryInfoBufferSize)
                {
                    cacheEntryInfoBufferSize = cacheEntryInfoBufferSizeInitial;
                    cacheEntryInfoBuffer = Marshal.ReAllocHGlobal(cacheEntryInfoBuffer, (IntPtr)cacheEntryInfoBufferSize);
                    returnValue = FindNextUrlCacheEntry(enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial);
                }
            }
            Marshal.FreeHGlobal(cacheEntryInfoBuffer);
        }

        public void sleep(int seconds, bool isBreakWhenWBCompleted)
        {
            WebBrowser wb = null;
            try
            {
                wb = (WebBrowser)GetCurrentWB();
            }
            catch { }

            for (int i = 0; i < seconds * 10; i++)
            {
                if (IsStop == false)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);

                    toolStripStatus.Text = "Sleep: " + ((i + 1) * 100) + "/" + (seconds * 1000);
                    if (isBreakWhenWBCompleted)
                    {
                        if (wb != null)
                        {
                            if (wb.ReadyState == WebBrowserReadyState.Complete) break;
                        }
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
            tbxPreview.Scrolling.ScrollBy(0, tbxPreview.Lines.Count);
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
            ReleaseMemory.FlushMemory();
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
                    System.IO.Directory.Delete(path);
                }
            }
            catch { }
        }

        public void excute(string script)
        {
            ExcuteJSCodeWebBrowser(script);
        }

        public void runcommand(string path, string parameters)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = path;
                startInfo.Arguments = parameters;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                try
                {
                    Process p = Process.Start(startInfo);
                    p.WaitForExit();
                }
                catch { }
                //System.Diagnostics.Process.Start(path);
            }
            catch { }
        }

        public void lockworkstation()
        {
            LockWorkStation();
        }

        public void logoff()
        {
            ExitWindowsEx(0, 0);
        }

        public void forcelogoff()
        {
            ExitWindowsEx(4, 0);
        }

        public void reboot()
        {
            //ExitWindowsEx(2, 0);
            System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
        }

        public void shutdown()
        {
            //ExitWindowsEx(1, 0);
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

        public void createtask(string name, string description, string path, string parameters, string workingdirectory, short day)
        {
            using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
            {
                Microsoft.Win32.TaskScheduler.TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = description;

                td.Triggers.Add(new Microsoft.Win32.TaskScheduler.DailyTrigger { DaysInterval = day });

                td.Actions.Add(new Microsoft.Win32.TaskScheduler.ExecAction(path, parameters, workingdirectory));

                ts.RootFolder.RegisterTaskDefinition(@"" + name, td);
            }
        }

        public void removetask(string name)
        {
            using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
            {
                ts.RootFolder.DeleteTask(name);
            }
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

        private void getalllink(string xpath)
        {
            List<string> links = new List<string>();

            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                Application.DoEvents();

                HtmlElementCollection collection = null;
                if (!string.IsNullOrEmpty(xpath))
                {
                    HtmlElement ele = GetElementByXpath(wb.Document, xpath);
                    if (ele != null)
                        collection = ele.GetElementsByTagName("a");
                }
                else
                {
                    collection = wb.Document.GetElementsByTagName("a");
                }
                if (collection != null)
                {
                    foreach (HtmlElement elm in collection)
                    {
                        if (IsStop == false)
                        {
                            string url = elm.GetAttribute("href");
                            if (!string.IsNullOrEmpty(url))
                            {
                                Website w = new Website();
                                w.Url = url;

                                var uri = new Uri(wb.Url.ToString());
                                var host = uri.Host;

                                string t = "";

                                string x = System.Web.HttpUtility.UrlDecode(uri.AbsolutePath);
                                string y = System.Web.HttpUtility.UrlDecode(uri.ToString());
                                if (x != "/" && x != "")
                                {
                                    t = y.Remove(y.IndexOf(x));
                                }
                                else { t = "http://" + host; }

                                if (url.StartsWith(t))
                                {
                                    w.IsInternal = true;
                                }
                                else
                                {
                                    w.IsInternal = false;
                                }

                                if (url.IndexOf("javascript") == -1)
                                {
                                    if (!Contains(links, url))
                                    {
                                        w.IsValid = true;
                                    }
                                }
                                else
                                {
                                    w.IsValid = false;
                                }

                                if (w.Url.IndexOf("q=related:") != -1 || w.Url.IndexOf("q=+site:") != -1 || w.Url.IndexOf("q=cache:") != -1)
                                {
                                    w.IsValid = false;
                                }

                                w.Parent = wb.Url.ToString();

                                links.Add(url);
                                lstTemp.Add(w);
                                lstAllLink.Add(w);
                            }
                        }
                    }
                }               
            }
        }

        public void browser(string xpath)
        {
            bool done = true;

            getalllink(xpath);

            while (done)
            {
                if (IsStop == false)
                {
                    Analyst(xpath);
                    done = IsContinue();
                }
                else
                {
                    break;
                }
            }

            foreach (Website w in lstAllLink)
            {
                if (w.IsValid && w.IsInternal == false)
                    log(w.Url);
            }
        }

        private void Analyst(string xpath)
        {
            RebuildMaster();

            foreach (Website ws in lstWebsite)
            {
                if (IsStop == false)
                {
                    if (ws.IsInternal && ws.IsValid && ws.IsRead == false)
                    {
                        tabnew();
                        go(ws.Url);
                        sleep(30, true);

                        if (!string.IsNullOrEmpty(tbxTemplate.Text))
                        {
                            object obj = wbMain.Document.InvokeScript("eval", new object[] { tbxTemplate.Text });
                        }
                        getalllink(xpath);
                        tabclose();
                        ws.IsRead = true;
                    }
                }
            }

            RebuildMaster();
        }

        private bool IsContinue()
        {
            bool result = false;
            foreach (Website ws in lstWebsite)
            {
                if (ws.IsInternal && ws.IsValid && ws.IsRead == false)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public void ResetListWebsite()
        {
            lstWebsite = new List<Website>();
        }

        private void RebuildMaster()
        {
            foreach (Website w in lstTemp)
            {
                if (!IsDuplicate(lstWebsite, w))
                {
                    if (w.Url != w.Parent)
                        lstWebsite.Add(w);
                }
            }

            lstTemp = new List<Website>();
        }        

        private bool Contains(List<string> list, string comparedValue)
        {
            foreach (string listValue in list)
            {
                if (listValue == comparedValue)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsDuplicate(List<Website> lstWebsite, Website ws)
        {
            foreach (Website w in lstWebsite)
            {
                if (w.Url == ws.Url) return true;
            }
            return false;
        }

        private Bitmap _bitmap = null;
        public void TakeSnapshot(string location)
        {
            try
            {
                WebBrowser wb = (WebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    if (wb.ReadyState == WebBrowserReadyState.Complete)
                    {
                        int width = wb.Document.Body.ScrollRectangle.Width;
                        int height = wb.Document.Body.ScrollRectangle.Height;

                        _bitmap = new Bitmap(width, height);
                        wb.Dock = DockStyle.None;
                        wb.ScrollBarsEnabled = false;

                        wb.Size = _bitmap.Size;
                        wb.DrawToBitmap(_bitmap, wb.Bounds);

                        if (location != "")
                            _bitmap.Save(location);

                        wb.Dock = DockStyle.Fill;
                        wb.ScrollBarsEnabled = true;
                    }
                }
            }
            catch { }

        }

        public string imgToText(string path, string language)
        {
            string data = "";

            if (!string.IsNullOrEmpty(path))
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

                data = read(text);

            }

            return data;
        }

        public void FileUpload(string path, string xpath)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement elm = GetElementByXpath(wb.Document, xpath);

                if (elm != null)
                {
                    try
                    {
                        elm.Focus();
                    }
                    catch { }

                    SendKeys.SendWait(path); 
                    SendKeys.Flush();
                    
                }
            }
        }

        public string getCurrentUrl()
        {
            return currentUrl;
        }

        public void scrollto(int value)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Document.Window.ScrollTo(0, value);
            }
        }

        public int getheight()
        {
            int result = 0;
            
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                var script = "this.getheight = function() { return Math.max(Math.max(document.body.scrollHeight, document.documentElement.scrollHeight),Math.max(document.body.offsetHeight, document.documentElement.offsetHeight),Math.max(document.body.clientHeight, document.documentElement.clientHeight)); }";
                wb.Document.InvokeScript("eval", new object[] { script });
                result = (int)wb.Document.InvokeScript("getheight");
            }

            return result;
        }

        public string gettitle()
        {
            string result = "";

            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                result = wb.Document.Title;
            }

            return result;
        }

        public string getlinks(string xpath)
        {
            string result = "";
            List<string> links = new List<string>();

            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElementCollection collection = null;
                if (!string.IsNullOrEmpty(xpath))
                {
                    HtmlElement ele = GetElementByXpath(wb.Document, xpath);
                    if (ele != null)
                        collection = ele.GetElementsByTagName("a");
                }
                else
                {
                    collection = wb.Document.GetElementsByTagName("a");
                }

                if (collection != null)
                {
                    foreach (HtmlElement elm in collection)
                    {
                        string url = elm.GetAttribute("href");

                        if (url.IndexOf("javascript") == -1)
                        {
                            if (!Contains(links, url))
                            {
                                links.Add(url);
                                result += url + "|";
                            }
                        }
                    }
                }
            }

            return result;
        }

        public bool checkelement(string xpath) {
            bool result = false;

            try
            {
                WebBrowser wb = (WebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    HtmlElement elm = GetElementByXpath(wb.Document, xpath);
                    if (elm != null)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch { result = false; }

            return result;
        }

        public string getCurrentContent()
        {
            string result = "";

            try
            {
                WebBrowser wb = (WebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    result = wb.Document.Body.InnerHtml;
                }
            }
            catch { }

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
            System.Diagnostics.Process.Start("explorer.exe", argument);
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

        public void CaptchaBorder(string xpath, string style)
        {
            try
            {
                WebBrowser wb = (WebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    HtmlElement ele = GetElementByXpath(wb.Document, xpath);
                    if (ele != null)
                    {
                        ele.Style = style;
                    }
                }
            }
            catch { }
        }

        [ComImport, InterfaceType((short)1), Guid("3050F669-98B5-11CF-BB82-00AA00BDCE0B")]
        private interface IHTMLElementRenderFixed
        {
            void DrawToDC(IntPtr hdc);
            void SetDocumentPrinter(string bstrPrinterName, IntPtr hdc);
        }

        public void SaveImageFromElement(string xpath, string path)
        {
            Bitmap bmp = null;
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement e = GetElementByXpath(wb.Document, xpath);
                if (e != null)
                {
                    mshtml.IHTMLImgElement img = (mshtml.IHTMLImgElement)e.DomElement;
                    IHTMLElementRenderFixed render = (IHTMLElementRenderFixed)img;

                    using (bmp = new Bitmap(img.width, img.height))
                    {
                        Graphics g = Graphics.FromImage(bmp);
                        IntPtr hdc = g.GetHdc();
                        render.DrawToDC(hdc);
                        g.ReleaseHdc(hdc);
                        bmp.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
            
        }

        private string MouseX = "";
        private string MouseY = "";

        public string GetCurrentMouseX()
        {
            return MouseX;
        }

        public string GetCurrentMouseY()
        {
            return MouseY;
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

        private void WaitApp(int seconds)
        {
            Application.DoEvents();
            Thread.Sleep(seconds);
        }

        public string sendEmail(string username, string password, string server, string port, string email, string subject, string content)
        {
            string result = "False";

            MailServer mailserver = new MailServer();
            mailserver.Username = username;
            mailserver.Password = password;
            mailserver.Server = server;
            mailserver.Port = int.Parse(port);

            result = mailserver.SendEmail(email, subject, content).ToString();

            return result;
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

            result = conn.ExcuteQuery(name, dbName, sql);

            return result;
        }

        public string DownloadWebsite(string url)
        {
            string result = "";

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Proxy = null;
                    wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    wc.Credentials = new NetworkCredential();

                    byte[] response = wc.DownloadData(url);
                    while (wc.IsBusy)
                    {
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }

                    result = Encoding.UTF8.GetString(response);
                }
            }
            catch { }

            return result;
        }

        public string DownloadWebsite1(string url)
        {
            string result = "";

            byte[] data = null;
            WebClient client = new WebClient();
            client.DownloadDataCompleted +=
            delegate(object sender, DownloadDataCompletedEventArgs e)
            {
                data = e.Result;
            };
            client.DownloadDataAsync(new Uri(url));
            while (client.IsBusy && data == null)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            result = Encoding.UTF8.GetString(data);

            return result;
        }

        public void BlockFlash(bool isBlock)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElementCollection hec = wb.Document.All;

                foreach (HtmlElement element in hec)
                {
                    if (element.TagName == "OBJECT")
                    {
                        
                    }
                }
            }
        }

        public void ReleaseMR()
        {
            ReleaseMemory.FlushMemory();
        }

        #endregion

        #region WebBrowser

        void CallBackWinAppWebBrowser()
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
                                                function extract(a) {CheckAbort(); return window.external.extract(a);}
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
                                                function imagetotext(a) { CheckAbort(); return window.external.OCR(a);}
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

                                                function ocr(a,b) { CheckAbort(); return window.external.imgToText(a,b); }

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

                                                function Reload() { CheckAbort(); window.external.Reload(); }

                                                function sendEmail(a,b,c,d,e,f,g) { CheckAbort(); return window.external.sendEmail(a,b,c,d,e,f,g); }" +

                                                "function getAccountBy(name) { CheckAbort(); var a = window.external.GetAccountBy(name); if(a != '') { return eval(\"(\" + a + \")\"); } else { return ''; } }" +

                                                @"function getDatabases(name) { CheckAbort(); return window.external.GetDatabases(name); } 

                                                function getTables(name, dbName) { CheckAbort(); return window.external.GetTables(name, dbName); }

                                                function getColumns(name, dbName, table) { CheckAbort(); return window.external.GetColumns(name, dbName, table); }

                                                function getRows(name, dbName, sql) { CheckAbort(); return window.external.GetRows(name, dbName, sql); }

                                                function excuteQuery(name, dbName, sql) { CheckAbort(); return window.external.ExcuteQuery(name, dbName, sql); } 
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

            WebBrowser wbWebBrowser = new WebBrowser();

            wbWebBrowser.ObjectForScripting = this;
            wbWebBrowser.ScriptErrorsSuppressed = true;

            wbWebBrowser.Navigating += new WebBrowserNavigatingEventHandler(wbWebBrowser_Navigating);
            wbWebBrowser.Navigated += new WebBrowserNavigatedEventHandler(wbWebBrowser_Navigated);
            wbWebBrowser.NewWindow += new CancelEventHandler(wbWebBrowser_NewWindow);

            wbWebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbWebBrowser_DocumentCompleted);
            wbWebBrowser.CanGoBackChanged += new EventHandler(wbWebBrowser_CanGoBackChanged);
            wbWebBrowser.CanGoForwardChanged += new EventHandler(wbWebBrowser_CanGoForwardChanged);

            if (cfgShowImages.Checked)
            {
                webBrowserOptions &= ~Automation.App_Code.BrowserOptions.BrowserOption.Images;
                Automation.App_Code.BrowserOptions.IOleObject obj = (Automation.App_Code.BrowserOptions.IOleObject)wbWebBrowser.ActiveXInstance;
                obj.SetClientSite(this);
            }
            
            wbWebBrowser.Dock = DockStyle.Fill;
            wbWebBrowser.Navigate(url);
            currentTab.Controls.Add(wbWebBrowser);
        }

        void wbWebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.Segments[e.Url.Segments.Length - 1].EndsWith(".zip"))
            {
                e.Cancel = true;
            }
        }

        void wbWebBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            var wbBrowser = (WebBrowser)sender;
            if (wbBrowser != null)
            {
                toolStripNext.Enabled = wbBrowser.CanGoForward;
            }
            else
            {
                toolStripNext.Enabled = false;
            }
        }

        void wbWebBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            var wbBrowser = (WebBrowser)sender;
            if (wbBrowser != null)
            {
                toolStripBack.Enabled = wbBrowser.CanGoBack;
            }
            else
            {
                toolStripBack.Enabled = false;
            }
        }

        void GoWebBrowserByXpath(string xpath)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement elm = GetElementByXpath(wb.Document, xpath);
                if (elm != null)
                {
                    string url = elm.GetAttribute("href");
                    if (!string.IsNullOrEmpty(url))
                        wb.Navigate(url);
                }
            }
        }

        void wbWebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            tbxAddress.Text = ((WebBrowser)sender).Url.ToString();
        }

        void wbWebBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Navigate(wb.StatusText);
            }
            
            e.Cancel = true;
        }

        void wbWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string title = "";

            var wbBrowser = (WebBrowser)sender;
            if (wbBrowser != null)
            {
                if (wbBrowser.ReadyState == WebBrowserReadyState.Complete)
                {
                    currentUrl = wbBrowser.Url.ToString();

                    htmlDoc = wbBrowser.Document;
                    htmlDoc.MouseDown += htmlDoc_MouseDown;
                    wbBrowser.IsWebBrowserContextMenuEnabled = false;

                    wbBrowser.Navigating -= wbWebBrowser_Navigating;
                    wbBrowser.Navigated -= wbWebBrowser_Navigated;
                    wbBrowser.NewWindow -= wbWebBrowser_NewWindow;

                    wbBrowser.DocumentCompleted -= wbWebBrowser_DocumentCompleted;
                    wbBrowser.CanGoBackChanged -= wbWebBrowser_CanGoBackChanged;
                    wbBrowser.CanGoForwardChanged -= wbWebBrowser_CanGoForwardChanged;
                }

                title = wbBrowser.DocumentTitle;
                SetControlPropertyValue(currentTab, "Text", (title.Length > 10 ? title.Substring(0, 10) + "..." : title));
            }
        }


        void htmlDoc_MouseDown(object sender, HtmlElementEventArgs e)
        {
            if (e.MouseButtonsPressed.ToString().IndexOf("Right") != -1)
            {
                htmlElm = null;
                WebBrowser wb = (WebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    htmlElm = wb.Document.GetElementFromPoint(e.ClientMousePosition);
                }

                contextMenuBrowser.Show(Cursor.Position);
            }
        }

        void NextWebBrowser()
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoForward();
            }
        }

        void BackWebBrowser()
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoBack();
            }
        }

        void ReloadWebBrowser()
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Refresh();
            }
        }

        void StopWebBrowser()
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Stop();
            }
        }

        void TabSelectedWebBrowser()
        {
            if (tabMain.TabCount > 0)
            {
                WebBrowser wb = (WebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    tbxAddress.Text = wb.Url.ToString();
                    string title = wb.DocumentTitle;
                    currentTab.Text = (title.Length > 10 ? title.Substring(0, 10) + "..." : title);
                    this.Text = title;
                }
            }
        }

        private HtmlElement GetElementByXpath(HtmlDocument doc, string xpath)
        {
            if (doc == null) return null;

            xpath = xpath.Replace("/html/", "");
            HtmlElementCollection eleColec = doc.GetElementsByTagName("html"); if (eleColec.Count == 0) return null;
            HtmlElement ele = eleColec[0];
            string[] tagList = xpath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tag in tagList)
            {
                System.Text.RegularExpressions.Match mat = System.Text.RegularExpressions.Regex.Match(tag, "(?<tag>.+)\\[@id='(?<id>.+)'\\]");
                if (mat.Success == true)
                {
                    string id = mat.Groups["id"].Value;
                    HtmlElement tmpEle = doc.GetElementById(id);
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
                        HtmlElement tmpEle = null;
                        foreach (HtmlElement it in ele.Children)
                        {
                            if (it.TagName.ToLower() == tag)
                            {
                                tmpEle = it;
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
                        HtmlElement tmpEle = null;
                        foreach (HtmlElement it in ele.Children)
                        {
                            if (it.TagName.ToLower() == tagName)
                            {
                                count++;
                                if (ind == count)
                                {
                                    tmpEle = it;
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

            if (ele != null)
            {
                if (scrollToViewToolStripMenuItem.Enabled == true)
                {
                    ele.ScrollIntoView(true);
                }
                else
                {
                    ele.ScrollIntoView(false);
                }

                if (colorElementToolStripMenuItem.Enabled == true)
                {
                    CheckElement(ele);
                }
                //ele.Focus();
            }

            return ele;
        }

        private string GetXpath(HtmlElement ele)
        {
            string xpath = "";
            while (ele != null)
            {
                int ind = GetXpathIndex(ele);
                if (ind > 1)
                    xpath = "/" + ele.TagName.ToLower() + "[" + ind + "]" + xpath;
                else
                    xpath = "/" + ele.TagName.ToLower() + xpath;

                ele = ele.Parent;
            }
            return xpath;
        }

        private string GetXpathId(HtmlElement ele)
        {
            string id = ele.GetAttribute("id");
            if (String.IsNullOrEmpty(id) == false)
            {
                HtmlElement tmpEle = ele.Document.GetElementById(id);
                if (ele.Equals(tmpEle)) return id;
            }
            return null;
        }

        private int GetXpathIndex(HtmlElement ele)
        {
            if (ele.Parent == null) return 0;
            int ind = 0, indEle = 0;
            string tagName = ele.TagName;
            HtmlElementCollection elecol = ele.Parent.Children;
            foreach (HtmlElement it in elecol)
            {
                if (it.TagName == tagName)
                {
                    ind++;
                    if (it == ele) indEle = ind;
                }
            }
            if (ind > 1) return indEle;
            return 0;
        }

        HtmlElement hoveringElement = null;
        private void CheckElement(HtmlElement ele)
        {
            if (ele != null && ele != hoveringElement)
            {
                if (hoveringElement != null)
                {
                    try
                    {
                        hoveringElement.Style = hoveringElement.GetAttribute("OldStyle");
                    }
                    catch { }
                }

                ele.SetAttribute("OldStyle", ele.Style);
                ele.Style = ele.Style + ";border:solid 1px red;";

                hoveringElement = ele;
            }
        }

        protected void UpdateUrlAbsolute(HtmlElement ele)
        {
            HtmlElementCollection eleColec = ele.GetElementsByTagName("IMG");
            foreach (HtmlElement it in eleColec)
            {
                it.SetAttribute("src", it.GetAttribute("src"));
            }
            eleColec = ele.GetElementsByTagName("A");
            foreach (HtmlElement it in eleColec)
            {
                it.SetAttribute("href", it.GetAttribute("href"));
            }

            if (ele.TagName == "A")
            {
                ele.SetAttribute("href", ele.GetAttribute("href"));
            }
            else if (ele.TagName == "IMG")
            {
                ele.SetAttribute("src", ele.GetAttribute("src"));
            }
        }

        #endregion  

        #region auto run

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        void timerstart()
        {
            timer.Interval = 500;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            autorun();
        }

        private void autorun()
        {

            tabcloseall();
            tabnew();
            go("google.com.vn");
            sleep(5, false);

            string path_config = Application.StartupPath + "\\command\\config.script";
            string path_cmd = Application.StartupPath + "\\command\\cmd.script";

            if (!System.IO.Directory.Exists(Application.StartupPath + "\\command"))
            {
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\command");
            }

            if (!System.IO.File.Exists(path_config))
            {
                save("var start = false;", path_config, false);
            }

            if (!System.IO.File.Exists(path_cmd))
            {
                save("tabcloseall();\ntabnew();\ngo(\"vnexpress.net\");sleep(15, false);\nexit();", path_cmd, false);

            }

            string result_config = read(path_config);
            string result_cmd = read(path_cmd);

            if (result_config.IndexOf("var start = false;") != -1)
            {
                excute(result_cmd);
                save("var start = true;", path_config, false);
            }
        }
        #endregion

        #region image processing
        private System.Drawing.Bitmap sourceImage;
        private System.Drawing.Bitmap filteredImage;
        private string oldurl = "";

        public void showimage(string url, string ft)
        {
            if (currentTab == null)
            {
                tabnew();
            }
            if (string.IsNullOrEmpty(oldurl))
                oldurl = url;

            if (sourceImage == null)
            {
                HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

                //Create a response for this request
                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                if (fileReq.ContentLength > 0)
                    fileResp.ContentLength = fileReq.ContentLength;

                //Get the Stream returned from the response
                System.IO.Stream stream = fileResp.GetResponseStream();

                sourceImage = (Bitmap)Bitmap.FromStream(stream);
            }
            else
            {
                if (oldurl != url)
                {
                    HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

                    //Create a response for this request
                    HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                    if (fileReq.ContentLength > 0)
                        fileResp.ContentLength = fileReq.ContentLength;

                    //Get the Stream returned from the response
                    System.IO.Stream stream = fileResp.GetResponseStream();

                    sourceImage = (Bitmap)Bitmap.FromStream(stream);
                    oldurl = url;
                }
            }

            PictureBox p = new PictureBox();
            p.SizeMode = PictureBoxSizeMode.CenterImage;
            p.Dock = DockStyle.Fill;

            currentTab.Text = "Preview";
            p.Image = filter(ft);

            if (currentTab.Controls.Count > 0)
            {
                currentTab.Controls.RemoveAt(0);
            }

            currentTab.Controls.Add(p);
            
        }

        public void savefilterimage(string path)
        {
            if (filteredImage != null)
            {
                filteredImage.Save(path);
            }
        }

        public void writetextimage(string url, string text)
        {
            if (currentTab == null)
            {
                tabnew();
            }

            HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

            //Create a response for this request
            HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

            if (fileReq.ContentLength > 0)
                fileResp.ContentLength = fileReq.ContentLength;

            //Get the Stream returned from the response
            System.IO.Stream stream = fileResp.GetResponseStream();

            Bitmap bmp = (Bitmap)Bitmap.FromStream(stream);

            PictureBox p = new PictureBox();
            p.SizeMode = PictureBoxSizeMode.CenterImage;
            p.Dock = DockStyle.Fill;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawString(text, new Font("Tahoma", 15), Brushes.White, 10, 10);
            }
            
            p.Image = bmp;

            if (currentTab.Controls.Count > 0)
            {
                currentTab.Controls.RemoveAt(0);
            }

            currentTab.Controls.Add(p);
        }

        private Bitmap filter(string ft)
        {
            Bitmap effect = sourceImage;

            filteredImage = sourceImage;

            if (ft == "none")
            {
                effect = sourceImage;
            }
            else if (ft == "grayscale")
            {
                effect = ApplyFilter(Grayscale.CommonAlgorithms.BT709);
            }
            else if (ft == "sepia")
            {
                effect = ApplyFilter(new Sepia());
            }
            else if (ft == "invert")
            {
                effect = ApplyFilter(new Invert());
            }
            else if (ft == "rotate")
            {
                effect = ApplyFilter(new RotateChannels());
            }
            else if (ft == "color")
            {
                effect = ApplyFilter(new ColorFiltering(new IntRange(25, 230), new IntRange(25, 230), new IntRange(25, 230)));
            }
            else if (ft == "hue")
            {
                effect = ApplyFilter(new HueModifier(50));
            }
            else if (ft == "saturation")
            {
                effect = ApplyFilter(new SaturationCorrection(0.15f));
            }
            else if (ft == "brightness")
            {
                effect = ApplyFilter(new BrightnessCorrection());
            }
            else if (ft == "contrast")
            {
                effect = ApplyFilter(new ContrastCorrection());
            }
            else if (ft == "hsl")
            {
                effect = ApplyFilter(new HSLFiltering(new IntRange(330, 30), new Range(0, 1), new Range(0, 1)));
            }
            else if (ft == "YCbCr")
            {
                YCbCrLinear f = new YCbCrLinear();
                f.InCb = new Range(-0.3f, 0.3f);
                effect = ApplyFilter(f);
            }
            else if (ft == "YCbCr")
            {
                effect = ApplyFilter(new YCbCrFiltering(new Range(0.2f, 0.9f), new Range(-0.3f, 0.3f), new Range(-0.3f, 0.3f)));
            }
            else if (ft == "threshold")
            {
                Bitmap originalImage = sourceImage;
                // get grayscale image
                sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
                // apply threshold filter
                effect = ApplyFilter(new Threshold());
                // delete grayscale image and restore original
                sourceImage.Dispose();
                sourceImage = originalImage;
            }
            else if (ft == "floyd")
            {
                Bitmap originalImage = sourceImage;
                // get grayscale image
                sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
                // apply threshold filter
                effect = ApplyFilter(new FloydSteinbergDithering());
                // delete grayscale image and restore original
                sourceImage.Dispose();
                sourceImage = originalImage;
            }
            else if (ft == "ordered")
            {
                // save original image
                Bitmap originalImage = sourceImage;
                // get grayscale image
                sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
                // apply threshold filter
                effect = ApplyFilter(new OrderedDithering());
                // delete grayscale image and restore original
                sourceImage.Dispose();
                sourceImage = originalImage;
            }
            else if (ft == "correlation")
            {
                effect = ApplyFilter(new Convolution(new int[,] {
								{ 1, 2, 3, 2, 1 },
								{ 2, 4, 5, 4, 2 },
								{ 3, 5, 6, 5, 3 },
								{ 2, 4, 5, 4, 2 },
								{ 1, 2, 3, 2, 1 } }));
            }
            else if (ft == "sharpen")
            {
                effect = ApplyFilter(new Sharpen());
            }
            else if (ft == "edgedetector")
            {
                // save original image
                Bitmap originalImage = sourceImage;
                // get grayscale image
                sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
                // apply edge filter
                effect = ApplyFilter(new DifferenceEdgeDetector());
                // delete grayscale image and restore original
                sourceImage.Dispose();
                sourceImage = originalImage;
            }
            else if (ft == "homogenity")
            {
                // save original image
                Bitmap originalImage = sourceImage;
                // get grayscale image
                sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
                // apply edge filter
                effect = ApplyFilter(new HomogenityEdgeDetector());
                // delete grayscale image and restore original
                sourceImage.Dispose();
                sourceImage = originalImage;
            }
            else if (ft == "sobel")
            {
                // save original image
                Bitmap originalImage = sourceImage;
                // get grayscale image
                sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
                // apply edge filter
                effect = ApplyFilter(new SobelEdgeDetector());
                // delete grayscale image and restore original
                sourceImage.Dispose();
                sourceImage = originalImage;
            }
            else if (ft == "rgbLinear")
            {
                LevelsLinear f = new LevelsLinear();

                f.InRed = new IntRange(30, 230);
                f.InGreen = new IntRange(50, 240);
                f.InBlue = new IntRange(10, 210);

                effect = ApplyFilter(f);
            }
            else if (ft == "jitter")
            {
                effect = ApplyFilter(new Jitter());
            }
            else if (ft == "oilpainting")
            {
                effect = ApplyFilter(new OilPainting());
            }
            else if (ft == "gaussinblur")
            {
                effect = ApplyFilter(new GaussianBlur(2.0, 7));
            }
            else if (ft == "texture")
            {
                effect = ApplyFilter(new Texturer(new TextileTexture(), 1.0, 0.8));
            }

            return effect;
        }

        private Bitmap ApplyFilter(IFilter filter)
        {
            filteredImage = filter.Apply(sourceImage);
            return filteredImage;
        }

        #endregion

        #region affiliate intelligence

        private void MachineAnswer(string input)
        {
            string bot_output = "";


            tbxAnswer.Text += "User:  " + input + "\n" + "Bot:  " + bot_output + "\n";
            tbxAsk.Text = "";

            tbxAnswer.SelectionStart = tbxAnswer.Text.Length;
            tbxAnswer.ScrollToCaret();

            if (string.Compare(input, "exit", true) == 0)
            {
                exit();
            }

            if (string.Compare(input, "clear", true) == 0)
            {
                tbxAnswer.Text = "";
            }
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
                                result = dataFormatter.FormatCellValue(cell, formulaEvaluator);
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
            Magnolia.FileUtilities.ExcelWrapper excel = new Magnolia.FileUtilities.ExcelWrapper(filePath);
            bool isSheetExists = excel.SheetExists(sheetname);
            if (isSheetExists)
            {
                excel.WriteCellValue(cellName, value);
                excel.SaveFile();
            }
            else
            {
                
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

            if (User.Current != null)
            {
                loginToolStripMenuItem.Text = Language.Resource.Logout;
            }
            else
            {
                loginToolStripMenuItem.Text = Language.Resource.Login;
            }

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

            this.Text = Language.Resource.WebAutomation + " - " + Language.Resource.Version + " - " + Version;
        }

        #endregion

        #region Automate System

        private const int WM_SETTEXT = 0x000C;
        private const int WM_GETTEXT = 0x000D;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_CHAR = 0x0102;
        private const int WM_KEYUP = 0x0101;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hwnd, int wFlag);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        private static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");

            list.Add(handle);
            return true;
        }

        private string GetTextWindow(IntPtr hwnd)
        {
            string result = "";
            
            try
            {
                IntPtr memmory = Marshal.AllocHGlobal(100);
                SendMessage(hwnd, WM_GETTEXT, (IntPtr)100, memmory);
                result = Marshal.PtrToStringAuto(memmory);
            }
            catch { }

            return result;
        }

        private void SetTextWindow(IntPtr hwnd, string text)
        {
            SendMessage(hwnd, WM_SETTEXT, IntPtr.Zero, Marshal.StringToHGlobalAuto(text));
        }

        public string GetControlText(string classname, string windowname, string index)
        {
            IntPtr hWnd = FindWindow(classname, windowname);
            List<IntPtr> listPtr = GetChildWindows(hWnd);

            string result = "";

            int i = 0;
            int.TryParse(index, out i);

            if (i >= 0 || i < listPtr.Count)
            {
                result = GetTextWindow(listPtr[i]);
            }

            return result;
        }

        public void SetControlText(string classname, string windowname, string index, string value)
        {
            IntPtr hWnd = FindWindow(classname, windowname);
            List<IntPtr> listPtr = GetChildWindows(hWnd);

            int i = 0;
            int.TryParse(index, out i);

            if (i >= 0 || i < listPtr.Count)
            {
                SetTextWindow(listPtr[i], value);
            }
        }

        public void ClickControl(string classname, string windowname, string index)
        {
            IntPtr hWnd = FindWindow(classname, windowname);
            List<IntPtr> listPtr = GetChildWindows(hWnd);

            int i = 0;
            int.TryParse(index, out i);

            if (i >= 0 || i < listPtr.Count)
            {
                SendMessage(listPtr[i], WM_LBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
                SendMessage(listPtr[i], WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
            }
        }

        #endregion

        #region Mouse Keyboard Library Event

        void mouseHook_MouseMove(object sender, MouseEventArgs e)
        {
            tbxCode.Text += "MouseMove(" + e.X + "," + e.Y + ",true, " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine;
            lastTimeRecorded = Environment.TickCount;
        }

        void mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            tbxCode.Text += "MouseDown('" + e.Button.ToString() + "', " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine;
            lastTimeRecorded = Environment.TickCount;
        }

        void mouseHook_MouseUp(object sender, MouseEventArgs e)
        {
            tbxCode.Text += "MouseUp('" + e.Button.ToString() + "', " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine;
            lastTimeRecorded = Environment.TickCount;
        }

        void keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            tbxCode.Text += "KeyDown('" + e.KeyCode + "', " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine;
            lastTimeRecorded = Environment.TickCount;
        }

        void keyboardHook_KeyUp(object sender, KeyEventArgs e)
        {
            tbxCode.Text += "KeyUp('" + e.KeyCode + "', " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine;
            lastTimeRecorded = Environment.TickCount;
        }

        void mouseHook_MouseWheel(object sender, MouseEventArgs e)
        {
            tbxCode.Text += "MouseWheel(" + e.Delta + ", " + (Environment.TickCount - lastTimeRecorded) + ");" + Environment.NewLine;
            lastTimeRecorded = Environment.TickCount;
        }

        #endregion

        #region Speech To Text

        SpeechRecognitionEngine speechRecognitionEngine = null;

        public void InitSpeechToText()
        {
            try
            {
                // create the engine
                speechRecognitionEngine = createSpeechEngine("vi-VN");

                // hook to events
                speechRecognitionEngine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(engine_AudioLevelUpdated);
                speechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(engine_SpeechRecognized);

                // load dictionary
                loadGrammarAndCommands();

                // use the system's default microphone
                speechRecognitionEngine.SetInputToDefaultAudioDevice();

                // start listening
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Voice recognition failed");
            }
        }

        public void DisposeSpeechToText()
        {
            speechRecognitionEngine.AudioLevelUpdated -= new EventHandler<AudioLevelUpdatedEventArgs>(engine_AudioLevelUpdated);
            speechRecognitionEngine.SpeechRecognized -= new EventHandler<SpeechRecognizedEventArgs>(engine_SpeechRecognized);
        }

        SpeechRecognitionEngine createSpeechEngine(string preferredCulture)
        {
            foreach (RecognizerInfo config in SpeechRecognitionEngine.InstalledRecognizers())
            {
                if (config.Culture.ToString() == preferredCulture)
                {
                    speechRecognitionEngine = new SpeechRecognitionEngine(config);
                    break;
                }
            }

            // if the desired culture is not found, then load default
            if (speechRecognitionEngine == null)
            {
                /*MessageBox.Show("The desired culture is not installed on this machine, the speech-engine will continue using "
                    + SpeechRecognitionEngine.InstalledRecognizers()[0].Culture.ToString() + " as the default culture.",
                    "Culture " + preferredCulture + " not found!");*/
                speechRecognitionEngine = new SpeechRecognitionEngine(SpeechRecognitionEngine.InstalledRecognizers()[0]);
            }

            return speechRecognitionEngine;
        }

        void loadGrammarAndCommands()
        {
            try
            {
                Choices texts = new Choices();
                texts.Add("Show Developer Tools");
                texts.Add("Hide Developer Tools");
                texts.Add("Start Record");
                texts.Add("Stop Record");
                texts.Add("New Script");
                texts.Add("Open Script");
                texts.Add("Save Script");
                texts.Add("Save As Script");
                texts.Add("Clear Script");
                texts.Add("New Template");
                texts.Add("Open Template");
                texts.Add("Save Template");
                texts.Add("Save As Template");
                texts.Add("Clear Template");
                texts.Add("Run");
                texts.Add("Stop");
                texts.Add("Login");
                texts.Add("Logout");
                texts.Add("Register");
                texts.Add("Hide Program");
                texts.Add("Show Program");
                texts.Add("English");
                texts.Add("Vietnamese");
                texts.Add("Manager");
                texts.Add("About Program");
                texts.Add("Check Update");
                texts.Add("Select Code Tab");
                texts.Add("Select Template Tab");
                texts.Add("Select Preview Tab");
                texts.Add("Open Tab");
                texts.Add("Close Tab");
                texts.Add("Open All Tab");
                texts.Add("Exit Program");
                texts.Add("Go Browser");
                texts.Add("Back Browser");
                texts.Add("Next Browser");
                texts.Add("Reload");
                texts.Add("Show Detection");
                texts.Add("Hide Detection");

                Grammar wordsList = new Grammar(new GrammarBuilder(texts));
                speechRecognitionEngine.LoadGrammar(wordsList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string result = e.Result.Text;

            if (result == "Start Record" || result == "Stop Record")
            {
                RecordEvents();
            }
            else if (result == "Login")
            {
                LoginClick();
            }
            else if (result == "Logout")
            {
                Logout();
            }
            else if (result == "Register")
            {
                RegisterClick();
            }
            else if (result == "Exit Program")
            {
                exit();
            }
            else if (result == "Hide Program")
            {
                MinimizeWindow();
            }
            else if (result == "Show Program")
            {
                ShowWindow();
            }
            else if (result == "Show Developer Tools")
            {
                DeveloperToolsClick();
            }
            else if (result == "Hide Developer Tools")
            {
                DeveloperToolsClick();
            }
            else if (result == "English" || result == "Vietnamese")
            {
                ChangeLanguage();
            }
            else if (result == "New Script")
            {
                NewScript();
            }
            else if (result == "Open Script")
            {
                OpenScript();
            }
            else if (result == "Save Script")
            {
                SaveScript();
            }
            else if (result == "Save As Script")
            {
                SaveAsScript();
            }
            else if (result == "Clear Script")
            {
                ClearScript();
            }
            else if (result == "New Template")
            {
                NewTemplate();
            }
            else if (result == "Open Template")
            {
                OpenTemplate();
            }
            else if (result == "Save Template")
            {
                SaveTemplate();
            }
            else if (result == "Save As Template")
            {
                SaveAsTemplate();
            }
            else if (result == "Clear Template")
            {
                ClearTemplate();
            }
            else if (result == "Manager")
            {
                ManagerClick();
            }
            else if (result == "About Program")
            {
                AboutClick();
            }
            else if (result == "Check Update")
            {
                CheckUpdateClick();
            }
            else if (result == "Select Code Tab")
            {
                tabControlCode.SelectTab(0);
            }
            else if (result == "Select Template Tab")
            {
                tabControlCode.SelectTab(1);
            }
            else if (result == "Select Preview Tab")
            {
                tabControlCode.SelectTab(2);
            }
            else if (result == "Open Tab")
            {
                tabnew();
            }
            else if (result == "Close All Tab")
            {
                tabcloseall();
            }
            else if (result == "Close Tab")
            {
                tabclose();
            }
            else if (result == "Go Browser")
            {
                GoClick();
            }
            else if (result == "Back Browser")
            {
                Back();
            }
            else if (result == "Next Browser")
            {
                Next();
            }
            else if (result == "Reload")
            {
                Reload();
            }
            else if (result == "Run")
            {
                RunCode();
            }
            else if (result == "Show Detection")
            {
                DetectionClick();
            }
            else if (result == "Hide Detection")
            {
                DetectionClick();
            }

            toolStripStatus.Text = result;
        }

        void engine_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            int level = e.AudioLevel;
            try
            {
                progressbar.Value = level;
            }
            catch { }
        }

        #endregion
    }


}
