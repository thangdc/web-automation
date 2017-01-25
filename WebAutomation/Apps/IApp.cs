using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebAutomation
{
    public interface IApp : IDisposable
    {
        Form MainForm { get; }
        void Start(string[] args);
    }
}
