using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileLockInfo;
using System.Diagnostics;

namespace TestBlock
{
    public partial class MainForm : Form
    {
        class Info
        {
            public string ProcName { get; set; }
            public string FileName { get; set; }

        }
        List<Info> info;
        public MainForm()
        {
            InitializeComponent();
            info = new List<Info>();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            foreach(Process proc in Process.GetProcesses())
            {
                List<string> files = Win32Processes.GetFilesLockedBy(proc);
                foreach(string fname in files)
                {
                    info.Add(new Info { ProcName = proc.ProcessName, FileName = fname });
                }
                
            }
            procDataGridView.DataSource = info;

        }
    }
}
