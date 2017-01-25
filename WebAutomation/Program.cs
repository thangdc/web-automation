using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WebAutomation;

namespace WebAutomation
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            App.Instance.Start(args);
        }
    }
}
