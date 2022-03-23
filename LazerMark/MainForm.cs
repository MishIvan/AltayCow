using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Resources;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace LazerMark
{
    delegate void wLog(String msg);
    public partial class MainForm : Form
    {
        SentInfo sentInfo; // имя файла данных и
        bool dataSent; // если все данные из файла были отправлены
        System.Timers.Timer sendTimer;
        public MainForm()
        {
            this.InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            sentInfo = new SentInfo();
            dataSent = false;
        }

        /// <summary>
        /// Установка таймера для отсылки данных из файла построчно на маркиратор
        /// </summary>
        private void AutoPrint()
        {
            sendTimer = new System.Timers.Timer(50)
            {
                AutoReset = true,
                Enabled = !dataSent
            };
            sendTimer.Elapsed += new ElapsedEventHandler(this.CheckData);        
        }

        /// <summary>
        /// Нажание на кнопку Listen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCheck(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            IPEndPoint point = new IPEndPoint(ip, int.Parse(txtPort.Text));
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(point);
                WriteLogMsg(LazerMark.Properties.Resources.CONNECTION_ESTABLISHES);
                socket.Dispose();
                btnCheck.Enabled = false;
                txtIP.Enabled = false;
                txtPort.Enabled = false;
            }
            catch (Exception exception)
            {
                this.ShowMsg(exception.Message);
            }
        }

        /// <summary>
        /// Нажатие на кнопку Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStart(object sender, EventArgs e)
        {
            String srcFile = MyXmlSet.GetMyConfig("/configuration/srcFile");
            if(String.IsNullOrEmpty(srcFile) || String.IsNullOrWhiteSpace(srcFile))
            {
                WriteLogMsg(LazerMark.Properties.Resources.FILE_NAME_EMPTY);
                return;
            }
            if(!File.Exists(srcFile))
            {
                WriteLogMsg(String.Format(LazerMark.Properties.Resources.FILE_NOT_EXISTS,srcFile));
                return;

            }
            sentInfo.srcFile = srcFile;
            if (sentInfo.FillData())
                AutoPrint();
            else
                WriteLogMsg(String.Format(LazerMark.Properties.Resources.ERROR_FILE_READ, srcFile));
            btnStart.Enabled = false;
        }

    private void CheckData(object source, ElapsedEventArgs e)
    {
        if(!sentInfo.IsEmpty())
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            IPEndPoint point = new IPEndPoint(ip, int.Parse(txtPort.Text));
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                String str = sentInfo.GetString();
                byte[] buffer = Encoding.UTF8.GetBytes(string.Concat(str, ";;"));
                socket.Connect(point);
                int n = socket.Send(buffer);
                if (n > 0)
                {
                    WriteLogMsg("Sent: " + str);
                    byte[] recvbuf = new byte[1024];
                    n = socket.Receive(recvbuf);
                    if (n > 0)
                    {
                        String s1 = Encoding.UTF8.GetString(recvbuf, 0, n);
                        sentInfo.Remove();
                        WriteLogMsg("Received: " + s1);
                    }
                }
                socket.Dispose();
            }
            catch(Exception ex)
            {
                WriteLogMsg("Received: " + ex.Message);
            }
        }
        else
        {
            WriteLogMsg("All data has been sent...");
            dataSent = true;
        }          
    }


    private void Form_Load(object sender, EventArgs e)
    {
        txtDataFile.Text = MyXmlSet.GetMyConfig("/configuration/srcFile");
        txtIP.Text = MyXmlSet.GetMyConfig("/configuration/net/ip");
        txtPort.Text = MyXmlSet.GetMyConfig("/configuration/net/port");
    }

        /// <summary>
        /// Выбрать файл, строки из которого следует отправить на маркиратор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFileBrowse(object sender, EventArgs e)
        {
            string resultFile = String.Empty;
            OpenFileDialog ofn = new OpenFileDialog();
            ofn.InitialDirectory = Environment.CurrentDirectory; //"./OriginalData/",
            ofn.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofn.FilterIndex = 2;
            ofn.RestoreDirectory = true;
            if (ofn.ShowDialog() == DialogResult.OK)
            {
                resultFile = ofn.FileName;
                this.txtDataFile.Text = resultFile;
            }
        }


        public void SetPrintedCount(string s)
        {
            if (!base.InvokeRequired)
            {
                this.lblPrintedCount.Text = s;
            }
            else
            {
                MainForm.DelegateShowPrinted delegateShowPrinted = new MainForm.DelegateShowPrinted(this.SetPrintedCount);
                object[] objArray = new object[] { s };
                base.Invoke(delegateShowPrinted, objArray);
            }
        }

        private void ShowMsg(string msg)
        {
            this.txtLog.AppendText(string.Concat(msg, "\r\n"));
        }

        /// <summary>
        /// Добавить строку в окно лога
        /// </summary>
        /// <param name="s"></param>
        public void WriteLogMsg(string s)
        {
            if (!base.InvokeRequired)
            {
                TextBox textBox = this.txtLog;
                textBox.Text = string.Concat(textBox.Text, s, "\r\n");
            }
            else
            {
                MainForm.DelegateShowPrinted delegateShowPrinted = new MainForm.DelegateShowPrinted(this.WriteLogMsg);
                object[] objArray = new object[] { s };
                base.Invoke(delegateShowPrinted, objArray);
            }
        }

        private delegate void DelegateShowPrinted(string s);

        /// <summary>
        /// Сохранить настройки. Меню Save Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveConfig(object sender, EventArgs e)
        {
            MyXmlSet.SetMyConfig("/configuration/srcFile", txtDataFile.Text);
            MyXmlSet.SetMyConfig("/configuration/net/ip", txtIP.Text);
            MyXmlSet.SetMyConfig("/configuration/net/port", txtPort.Text);
        }
        /// <summary>
        /// Сохранить настройки. Меню Save Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, EventArgs e)
        {
            if(!dataSent) Close();
        }
    }
}