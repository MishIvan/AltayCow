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
    public partial class MainForm : Form
    {
        SentInfo sentInfo; // имя файла данных и
        /// <summary>
        /// Статус отсылки
        /// 0 - обработка не начиналась
        /// 1 - соединение усрановлено
        /// 2 - данные передаются
        /// 3 - данные переданы
        /// -1 - ошибка передачи данных или соединения
        /// </summary>
        int sentStatus; // если все данные из файла были отправлены
        int stringCounter;
        public MainForm()
        {
            this.InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            sentInfo = new SentInfo();
            sentStatus = 0;
            stringCounter = 0;
        }

        /// <summary>
        /// Установка таймера для отсылки данных из файла построчно на маркиратор
        /// </summary>
        private void AutoPrint()
        {
            sendTimer.Interval = 50;
            sendTimer.Enabled = sentStatus == 1 || sentStatus == 2;
            sendTimer.Start();
        }

        /// <summary>
        /// Проверка соединения с удалённым хостом
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
                sentStatus = 1;
            }
            catch (Exception exception)
            {
                this.ShowMsg(exception.Message);
                sentStatus = -1;
            }
        }

        /// <summary>
        /// Нажатие на кнопку Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStart(object sender, EventArgs e)
        {
            String srcFile = txtDataFile.Text;
            if(String.IsNullOrEmpty(srcFile) || String.IsNullOrWhiteSpace(srcFile))
            {
                WriteLogMsg(LazerMark.Properties.Resources.FILE_NAME_EMPTY);
                sentStatus = -1;
                return;
            }
            if(!File.Exists(srcFile))
            {
                WriteLogMsg(String.Format(LazerMark.Properties.Resources.FILE_NOT_EXISTS,srcFile));
                sentStatus = -1;
                return;

            }
            sentInfo.srcFile = srcFile;
            if (sentInfo.FillData())
            {
                lblPrintedCount.Text = $"Printing: 0\\{sentInfo.initialLineCount}";
                AutoPrint();
            }
            else
            {
                WriteLogMsg(String.Format(LazerMark.Properties.Resources.ERROR_FILE_READ, srcFile));
                sentStatus = -1;
            }
                
            btnStart.Enabled = false;
        }

        /// <summary>
        /// отсылка данных и получение ответа от удалённого хоста
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void CheckData(object source, EventArgs e)
        {
            if(!sentInfo.IsEmpty())
            {
                sentStatus = 2;
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
                            lblPrintedCount.Text = $"Printing {++stringCounter}/{sentInfo.initialLineCount}";
                        }
                    }
                    socket.Dispose();
                }
                catch(Exception ex)
                {
                    WriteLogMsg("Received: " + ex.Message);
                    sentStatus = -1;
                }
            }
            else
            {
                WriteLogMsg("All data has been sent...");
                sentStatus = 3;
                sendTimer.Stop();
                btnStart.Enabled = true;
                txtIP.Enabled = true;
                txtPort.Enabled = true;
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
        /// Меню Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, EventArgs e)
        {
            if(sentStatus < 1 || sentStatus == 3) Close();
        }
        
        /// <summary>
        /// Не закрывть форму, пока идёт передача данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (sentStatus == 2)
            {
                WriteLogMsg(LazerMark.Properties.Resources.DATA_TRANSFER);
                e.Cancel = true;
            }                
            else
                e.Cancel = false;
        }
        /// <summary>
        /// Очистить лог
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogClear(object sender, EventArgs e)
        {
            txtLog.Text = String.Empty;
        }
    }
}