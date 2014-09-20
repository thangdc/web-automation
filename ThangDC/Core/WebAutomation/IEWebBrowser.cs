using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ThangDC.Core.Entities;

namespace ThangDC.Core.WebAutomation
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class IEWebBrowser
    {
        #region Variable

        private WebBrowser wbMain;
        public TabControl _tabMain;
        public TabPage _currentTab;
        public string _Logger;

        #endregion

        #region Initation

        public IEWebBrowser(TabControl tabMain, TabPage tabPage, ContextMenuStrip contextMenuBrowser)
        {
            _tabMain = tabMain;
            _currentTab = tabPage;

            wbMain = CallBackWinAppWebBrowser();
        }

        #endregion

        #region WebBrowser Init

        public WebBrowser CallBackWinAppWebBrowser()
        {
            WebBrowser wbBrowser = new WebBrowser();
            wbBrowser.ObjectForScripting = this;
            wbBrowser.ScriptErrorsSuppressed = true;
            wbBrowser.DocumentText = "" + 
                "<html>" +
                    "<head>" + 
                        "<script type=\"text/javascript\">" +
                            "var isAborted = false;" +
                            "function UnAbort() { isAborted = false; }" +
                            "function Abort() { isAborted = true; }" +
                            "function CheckAbort() { if(isAborted == true) throw new Error('Aborted'); }" +
                            "function GetAccount(name) { CheckAbort(); return eval(\"(\" + window.external.GetAccount(name) + \")\"); }" +

                            "function MouseDown(mouseButton,lastTime) { CheckAbort(); window.external.Mouse_Down(mouseButton,lastTime); }" +
                            "function MouseUp(mouseButton,lastTime) { CheckAbort(); window.external.Mouse_Up(mouseButton,lastTime); }" +
                            "function MouseClick(mouseButton,lastTime) { CheckAbort(); window.external.Mouse_Click(mouseButton,lastTime); }" +
                            "function MouseDoubleClick(mouseButton,lastTime) { CheckAbort(); window.external.Mouse_Double_Click(mouseButton,lastTime); }" +
                            "function MouseMove(x,y,isShow,lastTime) {CheckAbort(); window.external.Mouse_Show(x,y,isShow,lastTime); }" +
                            "function MouseWheel(detal,lastTime) { CheckAbort(); window.external.Mouse_Wheel(detal,lastTime); }" +
                            "function KeyDown(key,lastTime) { CheckAbort(); window.external.Key_Down(key,lastTime); }" +
                            "function KeyUp(key,lastTime) { CheckAbort(); window.external.Key_Up(key,lastTime); }" + 

                        "</script>" + 
                    "</head>" + 
                    "<body>" + 
                    "</body>" + 
                "</html>";
            return wbBrowser;
        }

        #endregion

        #region WebBrowser Function

        public void ExcuteJSCodeWebBrowser(string code)
        {
            wbMain.Document.InvokeScript("UnAbort");
            object obj = wbMain.Document.InvokeScript("eval", new object[] { code });
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
                ele.Style = ele.Style + ";background: #3c3c3c; zoom: 1;filter: alpha(opacity=30);opacity: 0.3;";

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

        private object GetCurrentWB()
        {
            if (_tabMain.SelectedTab != null)
            {
                if (_tabMain.SelectedTab.Controls.Count > 0)
                {
                    Control ctr = _tabMain.SelectedTab.Controls[0];
                    if (ctr != null)
                    {
                        return ctr as object;
                    }
                }
            }
            return null;
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

            return ele;
        }

        #endregion

        #region Private Function

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

        #endregion

        #region Program Function

        public string GetCurrentPath()
        {
            string result = "";
            try
            {
                result = Application.StartupPath;
            }
            catch (Exception ex) { Log(ex.Message); }
            return result;
        }

        public string GetCurrentContent()
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
            catch (Exception ex) { Log(ex.Message); }

            return result;
        }

        public string GetAllLinks(string xpath)
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

        public void GenerateKeys()
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

            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        public string Encrypt(string publicKey, string plainText)
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
            catch (Exception ex) { Log(ex.Message); }
            return result;
        }

        public string Descript(string privateKey, string encrypted)
        {
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            byte[] encryptedBytes = null;
            byte[] plainBytes = null;

            string result = "";
            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(privateKey);

                encryptedBytes = Convert.FromBase64String(encrypted);
                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

                result = System.Text.Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex) { Log(ex.Message); }
            return result;
        }

        public void Sleep(int seconds, bool isBreakWhenWBCompleted)
        {
            WebBrowser wb = null;
            try
            {
                wb = (WebBrowser)GetCurrentWB();
            }
            catch { }

            for (int i = 0; i < seconds * 10; i++)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);

                if (isBreakWhenWBCompleted)
                {
                    if (wb != null)
                    {
                        if (wb.ReadyState == WebBrowserReadyState.Complete) break;
                    }
                }
            }
            
        }

        public void Log(string text)
        {
            _Logger += text + "\n";
        }

        public void ClearLog()
        {
            _Logger = "";
        }

        public string Extract(string xpath)
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

        public void Fill(string xpath, string value)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement elm = GetElementByXpath(wb.Document, xpath);
                if (elm != null)
                {
                    switch (elm.TagName)
                    {
                        case "IFRAME":
                            foreach (HtmlWindow ifr in wb.Document.Window.Frames)
                            {
                                if (ifr.WindowFrameElement == elm)
                                {
                                    ifr.Document.Body.InnerHtml = value;
                                    break;
                                }
                            }
                            break;
                        default:
                            elm.Focus();
                            elm.InnerText = value;
                            break;
                    }
                }
            }
        }

        public void FillDropDown(string xpath, string index)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement elm = GetElementByXpath(wb.Document, xpath);
                if (elm != null)
                {
                    elm.Focus();
                    elm.SetAttribute("selectedIndex", index);
                }
            }
        }

        public void Click(string xpath)
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                HtmlElement elm = GetElementByXpath(wb.Document, xpath);

                if (elm != null)
                {
                    elm.Focus();
                    elm.InvokeMember("click");
                }
            }
        }

        public int GetWebHeight()
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

        public void HideWindow()
        {
            
        }

        public void ShowWindow()
        {
            
        }

        public string GetAccount(string name)
        {
            string result = "";

            Account account = new Account();
            result = account.GetByJSON(name);

            return result;
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

        #endregion

        #region User Interface Function

        public void NewScript()
        {

        }

        public void OpenScript()
        {

        }

        public void SaveScript()
        {

        }

        public void SaveAsScript()
        {

        }

        public void Login()
        {

        }

        public void Register()
        {

        }

        public void ShowHideWindow()
        {

        }

        public void ShowHideDeveloperTool()
        {

        }

        public void ScrollToView()
        {

        }

        public void ColorElement()
        {

        }

        public void About()
        {

        }

        public void CheckUpdate()
        {

        }

        public void RunCode(string script)
        {
            ExcuteJSCodeWebBrowser(script);
        }

        public void Record()
        {
            
        }

        public void Next()
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoForward();
            }
        }

        public void Back()
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoBack();
            }
        }

        public void Reload()
        {
            WebBrowser wb = (WebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Refresh();
            }
        }

        public void TabNew()
        {
            TabPage tab = new TabPage("new tab");

            _tabMain.Controls.Add(tab);
            _currentTab = tab;
            _tabMain.SelectedTab = _currentTab;
        }

        public void TabClose()
        {
            if (_tabMain.TabPages.Count > 0)
            {
                if (_tabMain.SelectedTab.Controls.Count > 0)
                {
                    _tabMain.SelectedTab.Controls[0].Dispose();
                }
                _tabMain.SelectedTab.Dispose();

                if (_tabMain.TabPages.Count > 1)
                {
                    _tabMain.SelectTab(_tabMain.TabPages.Count - 1);
                }
                _currentTab = _tabMain.SelectedTab;
            }
            else
            {
                _currentTab = null;
            }
        }

        public void TabCloseAll()
        {
            while (_tabMain.TabPages.Count > 0)
            {
                try
                {
                    Application.DoEvents();
                    if (_tabMain.TabPages[0].Controls.Count > 0)
                    {
                        _tabMain.TabPages[0].Controls[0].Dispose();
                    }
                    _tabMain.TabPages[0].Dispose();
                }
                catch { }
            }
            _currentTab = null;
        }

        public void Exit()
        {
            Application.Exit();
        }

        public void ChangeLanguage()
        {

        }

        #endregion

        #region System Function

        public void RunCommand(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch (Exception ex) { Log(ex.Message); }
        }

        public void CreateFolder(string path)
        {
            try
            {
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex) { Log(ex.Message); }
        }

        public string GetAllFiles(string path)
        {
            string result = "";
            string r = "";

            string[] filePaths = Directory.GetFiles(path);

            try
            {
                foreach (string f in filePaths)
                {
                    r += f + "|";
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

        public string GetAllFolders(string path)
        {
            string result = "";
            string r = "";

            try
            {
                string[] directoryPaths = Directory.GetDirectories(path);

                foreach (string f in directoryPaths)
                {
                    r += f + "|";
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
            catch (Exception ex) { Log(ex.Message); }

            return result;
        }

        public string ReadTextFile(string path)
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
            catch (Exception ex) { Log(ex.Message); }
            return result;
        }

        public void SaveToFile(string content, string path, bool isOverride)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(path, isOverride))
                {
                    file.WriteLine(content);
                }
            }
            catch (Exception ex) { Log(ex.Message); }
        }

        public void RemoveFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex) { Log(ex.Message); }
        }

        public void RemoveFolder(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path);
                }
            }
            catch (Exception ex) { Log(ex.Message); }
        }

        public void OpenExplore(string path)
        {
            string argument = "/select, \"" + path + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
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
                    mshtml.IHTMLElement element = (mshtml.IHTMLElement)e.DomElement;
                    IHTMLElementRenderFixed render = (IHTMLElementRenderFixed)element;

                    bmp = new Bitmap(element.offsetWidth, element.offsetHeight);
                    Graphics g = Graphics.FromImage(bmp);
                    IntPtr hdc = g.GetHdc();
                    render.DrawToDC(hdc);
                    g.ReleaseHdc(hdc);
                    bmp.Save(path);
                }
            }

        }

        #endregion

        #region Utilities Function

        public void Download(string savePath, string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Headers.Add(HttpRequestHeader.UserAgent, "anything");
                try
                {
                    wc.DownloadFile(url, savePath);
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
            }
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

        #endregion
    }
}
