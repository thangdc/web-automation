using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebAutomation.SingleInstancing;
using Gecko;
using System.Globalization;

namespace WebAutomation
{
    public class App: IApp
    {
        private static App instance = new App();
        public static App Instance
        {
            get
            {
                return instance;
            }
        }

        public App()
        {
            AppManager.Instance.Initialize(this);
        }

        private SingleInstanceTracker tracker = null;
        private bool disposed = false;

        public Form MainForm
        {
            get
            {
                return (frmMain)tracker.Enforcer;
            }
        }

        private ISingleInstanceEnforcer GetSingleInstanceEnforcer()
        {
            return new frmMain();
        }

        public void StartSingleInstance(string[] args)
        {
            try
            {
                // Attempt to create a tracker
                tracker = new SingleInstanceTracker("SingleInstanceSample", new SingleInstanceEnforcerRetriever(GetSingleInstanceEnforcer));

                // If this is the first instance of the application, run the main form
                if (tracker.IsFirstInstance)
                {
                    try
                    {
                        frmMain form = (frmMain)tracker.Enforcer;

                        form.CallBackWinAppWebBrowser();
                        form.InitMouseKeyBoardEvent();

                        Xpcom.Initialize("Firefox");

                        if (Array.IndexOf<string>(args, "/as") >= 0)
                        {
                            form.WindowState = FormWindowState.Minimized;
                        }

                        form.Load += delegate(object sender, EventArgs e)
                        {
                            form.FormLoad();

                            if (form.WindowState == FormWindowState.Minimized)
                            {
                                form.HideForm();
                            }

                            if (args.Length > 0)
                            {
                                form.OnMessageReceived(new MessageEventArgs(args));
                            }
                        };

                        form.FormClosing += delegate(object sender, FormClosingEventArgs e)
                        {
                            form.exit();
                            Dispose();
                        };

                        Application.Run(form);
                    }
                    finally
                    {
                        Dispose();
                    }
                }
                else
                {
                    // This is not the first instance of the application, so do nothing but send a message to the first instance
                    if (args.Length > 0)
                    {
                        tracker.SendMessageToFirstInstance(args);
                    }
                }
            }
            catch (SingleInstancingException ex)
            {
                MessageBox.Show("Could not create a SingleInstanceTracker object:\n" + ex.Message + "\nApplication will now terminate.\n" + ex.InnerException.ToString());

                return;
            }
            finally
            {
                if (tracker != null)
                    tracker.Dispose();
            }
        }

        public void Start(string[] args)
        {
            var form = new frmMain(args);
            Application.Run(form);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
            }
        }
    }
}
