using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebAutomation.DownloadManager;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace WebAutomation.UI
{
    public partial class DownloadList : UserControl
    {
        delegate void ActionDownloader(Downloader d, ListViewItem item);
        Hashtable mapItemToDownload = new Hashtable();
        Hashtable mapDownloadToItem = new Hashtable();

        ListViewItem lastSelection = null;

        public event EventHandler SelectionChanged;
        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                if (this.SelectionChanged != null)
                    this.SelectionChanged(this, new EventArgs());
            }
        }

        public DownloadList()
        {
            InitializeComponent();
            InitLanguage();

            ProtocolProviderFactory.RegisterProtocolHandler("http", typeof(HttpProtocolProvider));
            ProtocolProviderFactory.RegisterProtocolHandler("https", typeof(HttpProtocolProvider));
            ProtocolProviderFactory.RegisterProtocolHandler("ftp", typeof(FtpProtocolProvider));

            WebAutomation.DownloadManager.DownloadManager.Instance.DownloadAdded += new EventHandler<DownloaderEventArgs>(Instance_DownloadAdded);
            WebAutomation.DownloadManager.DownloadManager.Instance.DownloadRemoved += new EventHandler<DownloaderEventArgs>(Instance_DownloadRemoved);
            WebAutomation.DownloadManager.DownloadManager.Instance.DownloadEnded += new EventHandler<DownloaderEventArgs>(Instance_DownloadEnded);

            for (int i = 0; i < WebAutomation.DownloadManager.DownloadManager.Instance.Downloads.Count; i++)
            {
                AddDownload(WebAutomation.DownloadManager.DownloadManager.Instance.Downloads[i]);
            }

            lvwDownloads.SmallImageList = FileTypeImageList.GetSharedInstance();
        }

        #region Functions

        public void InitLanguage()
        {
            newDownloadToolStripMenuItem.Text = Language.Resource.NewDownload;
            startToolStripMenuItem.Text = Language.Resource.Start;
            pauseToolStripMenuItem.Text = Language.Resource.Pause;
            removeToolStripMenuItem.Text = Language.Resource.Delete;
            openFileToolStripMenuItem.Text = Language.Resource.Open;
            copyURLToClipboardToolStripMenuItem.Text = Language.Resource.CopyUrlToClipboard;
            showInExplorerToolStripMenuItem.Text = Language.Resource.ShowInExplore;
        }

        public Downloader[] SelectedDownloaders
        {
            get
            {
                if (lvwDownloads.SelectedItems.Count > 0)
                {
                    Downloader[] downloaders = new Downloader[lvwDownloads.SelectedItems.Count];
                    for (int i = 0; i < downloaders.Length; i++)
                    {
                        downloaders[i] = mapItemToDownload[lvwDownloads.SelectedItems[i]] as Downloader;
                    }
                    return downloaders;
                }

                return null;
            }
        }

        public void UpdateUI()
        {
            isSelected = lvwDownloads.SelectedItems.Count > 0;

            removeToolStripMenuItem.Enabled = isSelected;
            startToolStripMenuItem.Enabled = isSelected;
            pauseToolStripMenuItem.Enabled = isSelected;
            
            OnSelectionChange();

            isSelected = lvwDownloads.SelectedItems.Count == 1;
            copyURLToClipboardToolStripMenuItem.Enabled = isSelected;

            showInExplorerToolStripMenuItem.Enabled = isSelected;
            openFileToolStripMenuItem.Enabled = isSelected && SelectedDownloaders[0].State == DownloaderState.Ended;
        }

        public event EventHandler SelectionChange;
        protected virtual void OnSelectionChange()
        {
            if (SelectionChange != null)
            {
                SelectionChange(this, EventArgs.Empty);
            }
        }

        private void lvwDownloads_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            UpdateUI();
        }

        private void DownloadsAction(ActionDownloader action)
        {
            if (lvwDownloads.SelectedItems.Count > 0)
            {
                try
                {
                    lvwDownloads.BeginUpdate();

                    lvwDownloads.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);

                    for (int i = lvwDownloads.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        ListViewItem item = lvwDownloads.SelectedItems[i];
                        action((Downloader)mapItemToDownload[item], item);
                    }

                    lvwDownloads.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);
                    lvwDownloads_ItemSelectionChanged(null, null);
                }
                finally
                {
                    lvwDownloads.EndUpdate();
                }
            }
        }

        public void NewFileDownload(string fileName, string url, string location, int segments)
        {
            ResourceLocation rl = new ResourceLocation();
            rl.Authenticate = false;
            rl.URL = url;

            rl.BindProtocolProviderType();
            if (rl.ProtocolProviderType != null)
            {
                string localFile = string.Empty;

                if (segments > Settings.Default.MaxSegments)
                {
                    segments = Settings.Default.MaxSegments;
                }

                if (segments < 1)
                {
                    segments = 1;
                }

                localFile = GetLocalFile(fileName, url, location);

                Downloader download = WebAutomation.DownloadManager.DownloadManager.Instance.Add(
                        rl,
                        null,
                        localFile,
                        segments,
                        false);
            }
        }

        private string GetLocalFile(string title, string url, string folder)
        {
            string result = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(folder))
                    folder = Settings.Default.DownloadFolder;

                if (string.IsNullOrEmpty(title))
                {
                    Uri u = new Uri(url);
                    string fileName = u.Segments[u.Segments.Length - 1];
                    result = PathHelper.GetWithBackslash(folder) + fileName;

                }
                else
                {
                    result = PathHelper.GetWithBackslash(folder) + title + System.IO.Path.GetExtension(url);
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        public void StartSelections()
        {
            DownloadsAction(
                delegate(Downloader d, ListViewItem item)
                {
                    d.Start();
                }
            );
        }

        public void Pause()
        {
            DownloadsAction(
                delegate(Downloader d, ListViewItem item)
                {
                    d.Pause();
                }
            );
        }

        void Instance_DownloadRemoved(object sender, DownloaderEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                ListViewItem item = mapDownloadToItem[e.Downloader] as ListViewItem;

                if (item != null)
                {
                    if (item.Selected)
                    {
                        lastSelection = null;
                    }

                    mapDownloadToItem[e.Downloader] = null;
                    mapItemToDownload[item] = null;

                    item.Remove();
                }
            }
            );
        }

        void Instance_DownloadAdded(object sender, DownloaderEventArgs e)
        {
            if (IsHandleCreated)
            {
                this.BeginInvoke((MethodInvoker)delegate() { AddDownload(e.Downloader); });
            }
            else
            {
                AddDownload(e.Downloader);
            }
        }

        private void AddDownload(Downloader d)
        {
            d.RestartingSegment += new EventHandler<SegmentEventArgs>(download_RestartingSegment);
            d.SegmentStoped += new EventHandler<SegmentEventArgs>(download_SegmentEnded);
            d.SegmentFailed += new EventHandler<SegmentEventArgs>(download_SegmentFailed);
            d.SegmentStarted += new EventHandler<SegmentEventArgs>(download_SegmentStarted);
            d.InfoReceived += new EventHandler(download_InfoReceived);
            d.SegmentStarting += new EventHandler<SegmentEventArgs>(Downloader_SegmentStarting);

            string ext = Path.GetExtension(d.LocalFile);

            ListViewItem item = new ListViewItem();

            item.ImageIndex = FileTypeImageList.GetImageIndexByExtention(ext);
            item.Text = Path.GetFileName(d.LocalFile);

            // size
            item.SubItems.Add(ByteFormatter.ToString(d.FileSize));
            // completed
            item.SubItems.Add(ByteFormatter.ToString(d.Transfered));
            // progress
            item.SubItems.Add(String.Format("{0:0.##}%", d.Progress));
            // left
            item.SubItems.Add(TimeSpanFormatter.ToString(d.Left));
            // rate
            item.SubItems.Add("0");
            // added
            item.SubItems.Add(d.CreatedDateTime.ToShortDateString() + " " + d.CreatedDateTime.ToShortTimeString());
            // state
            item.SubItems.Add(d.State.ToString());
            // resume
            item.SubItems.Add(GetResumeStr(d));
            // url
            item.SubItems.Add(d.ResourceLocation.URL);

            mapDownloadToItem[d] = item;
            mapItemToDownload[item] = d;

            lvwDownloads.Items.Add(item);
        }

        void download_InfoReceived(object sender, EventArgs e)
        {
            Downloader d = (Downloader)sender;

            Log(
                d,
                String.Format(
                "Connected to: {2}. File size = {0}, Resume = {1}",
                ByteFormatter.ToString(d.FileSize),
                d.RemoteFileInfo.AcceptRanges,
                d.ResourceLocation.URL),
                LogMode.Information);
        }

        void Downloader_SegmentStarting(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Starting segment for {3}, start position = {0}, end position {1}, segment size = {2}",
                ByteFormatter.ToString(e.Segment.InitialStartPosition),
                ByteFormatter.ToString(e.Segment.EndPosition),
                ByteFormatter.ToString(e.Segment.TotalToTransfer),
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        void download_SegmentStarted(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Started segment for {3}, start position = {0}, end position {1}, segment size = {2}",
                ByteFormatter.ToString(e.Segment.InitialStartPosition),
                ByteFormatter.ToString(e.Segment.EndPosition),
                ByteFormatter.ToString(e.Segment.TotalToTransfer),
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        void download_SegmentFailed(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Download segment ({0}) failed for {2}, reason = {1}",
                e.Segment.Index,
                e.Segment.LastError.Message,
                e.Downloader.ResourceLocation.URL),
                LogMode.Error);
        }

        void download_SegmentEnded(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Download segment ({0}) ended for {1}",
                e.Segment.Index,
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        void download_RestartingSegment(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Download segment ({0}) is restarting for {1}",
                e.Segment.Index,
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        void Instance_DownloadEnded(object sender, DownloaderEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Download ended {0}",
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        enum LogMode
        {
            Error,
            Information
        }

        void Log(Downloader downloader, string msg, LogMode m)
        {
            try
            {
                this.BeginInvoke(
                    (MethodInvoker)
                  delegate()
                  {
                      UpdateList();
                  }
              );
            }
            catch { }
        }

        private static string GetResumeStr(Downloader d)
        {
            return (d.RemoteFileInfo != null && d.RemoteFileInfo.AcceptRanges ? "Yes" : "No");
        }

        public void UpdateList()
        {
            for (int i = 0; i < lvwDownloads.Items.Count; i++)
            {
                ListViewItem item = lvwDownloads.Items[i];
                if (item == null) return;
                Downloader d = mapItemToDownload[item] as Downloader;
                if (d == null) return;

                DownloaderState state;

                if (item.Tag == null) state = DownloaderState.Working;
                else state = (DownloaderState)item.Tag;

                if (state != d.State ||
                    state == DownloaderState.Working ||
                    state == DownloaderState.WaitingForReconnect)
                {
                    item.SubItems[1].Text = ByteFormatter.ToString(d.FileSize);
                    item.SubItems[2].Text = ByteFormatter.ToString(d.Transfered);
                    item.SubItems[3].Text = String.Format("{0:0.##}%", d.Progress);
                    item.SubItems[4].Text = TimeSpanFormatter.ToString(d.Left);
                    item.SubItems[5].Text = String.Format("{0:0.##}", d.Rate / 1024.0);

                    if (d.LastError != null)
                    {
                        item.SubItems[7].Text = d.State.ToString() + ", " + d.LastError.Message;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(d.StatusMessage))
                        {
                            item.SubItems[7].Text = d.State.ToString();
                        }
                        else
                        {
                            item.SubItems[7].Text = d.State.ToString() + ", " + d.StatusMessage;
                        }
                    }

                    item.SubItems[8].Text = GetResumeStr(d);
                    item.SubItems[9].Text = d.ResourceLocation.URL;
                    item.Tag = d.State;
                }
            }
        }

        public void PauseAll()
        {
            WebAutomation.DownloadManager.DownloadManager.Instance.PauseAll();
            UpdateList();
        }

        public void RemoveSelections()
        {
            if (MessageBox.Show("Are you sure that you want to remove selected downloads?",
                this.ParentForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    lvwDownloads.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);
                    WebAutomation.DownloadManager.DownloadManager.Instance.DownloadRemoved -= new EventHandler<DownloaderEventArgs>(Instance_DownloadRemoved);

                    DownloadsAction(
                        delegate(Downloader d, ListViewItem item)
                        {
                            lvwDownloads.Items.RemoveAt(item.Index);
                            WebAutomation.DownloadManager.DownloadManager.Instance.RemoveDownload(d);
                        }
                    );
                }
                finally
                {
                    lvwDownloads.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);
                    lvwDownloads_ItemSelectionChanged(null, null);

                    WebAutomation.DownloadManager.DownloadManager.Instance.DownloadRemoved += new EventHandler<DownloaderEventArgs>(Instance_DownloadRemoved);
                }
            }
        }

        public void SelectAll()
        {
            using (WebAutomation.DownloadManager.DownloadManager.Instance.LockDownloadList(false))
            {
                lvwDownloads.BeginUpdate();
                try
                {
                    lvwDownloads.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);

                    for (int i = 0; i < lvwDownloads.Items.Count; i++)
                    {
                        lvwDownloads.Items[i].Selected = true;
                    }
                }
                finally
                {
                    lvwDownloads.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);
                    lvwDownloads_ItemSelectionChanged(null, null);

                    if (this.SelectionChanged != null)
                        this.SelectionChanged(this, new EventArgs());

                    lvwDownloads.EndUpdate();
                }
            }
        }

        public void RemoveCompleted()
        {
            lvwDownloads.BeginUpdate();
            try
            {
                WebAutomation.DownloadManager.DownloadManager.Instance.ClearEnded();
                UpdateList();
            }
            finally
            {
                lvwDownloads.EndUpdate();
            }
        }

        #endregion

        #region Events

        private void popUpContextMenu_Opening(object sender, CancelEventArgs e)
        {
            UpdateUI();
        }

        private void newDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //NewFileDownload(null, true);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartSelections();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSelections();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(SelectedDownloaders[0].LocalFile);
            }
            catch (Exception)
            {
            }
        }

        private void copyURLToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(SelectedDownloaders[0].ResourceLocation.URL);
        }

        private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", String.Format("/select,{0}", SelectedDownloaders[0].LocalFile));
        }

        private void lvwDownloads_DoubleClick(object sender, EventArgs e)
        {
            UpdateUI();

            openFileToolStripMenuItem_Click(sender, e);
        }

        #endregion
    }
}
