using System;
using System.Windows.Forms;

namespace LazerMark
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "App.config");
            Application.Run(new MainForm());
        }
    }
}