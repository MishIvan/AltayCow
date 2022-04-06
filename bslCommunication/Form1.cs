using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Linq;

namespace SocketSample
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Socket> dic = new Dictionary<string, Socket>();
        int timeout; // интервал для отсылки собщений
        int dataRowCount; // число строк для маркировки
        int dataRowNumber; // текущая строка
        bool receiving; // был ли получен код для маркировки
        Queue<string> srcFiles; // очередь файлов для печати

        public Form1()
        {
            this.InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            timeout = 0;
            dataRowCount = 0;
            dataRowNumber = -1;
            receiving = false;
            srcFiles = new Queue<string>();
        }

        private void AcceptInfo(object o)
        {
            Socket socket = o as Socket;
            while (true)
            {
                try
                {
                    Socket tSocket = socket.Accept();
                    string point = tSocket.RemoteEndPoint.ToString();
                    this.ShowLogMessage(string.Concat(point, "Connect successfully！"));
                    this.txtIpPort.Text = point;
                    this.dic.Add(point, tSocket);
                    if (this.dataRowNumber == -1 && this.dataRowCount == 0) {
                        while(srcFiles.Count < 1)
                        {
                            FillSrcFles();
                        }                        
                        CheckData(null, null);
                    }
                    else
                    {
                        if (!receiving && this.dataRowNumber < this.dataRowCount) MySendMsg();
                    }

                    //Thread th = new Thread(new ParameterizedThreadStart(this.ReceiveMsg))
                    //{
                    //    IsBackground = true
                    //};
                    //th.Start(tSocket);
                    this.ReceiveMsg(tSocket);
                }
                catch (Exception exception)
                {
                    this.ShowLogMessage(exception.Message);
                    break;
                }
            }
        }

        private string AdptLength(string p, int p_2)
        {
            string str;
            str = (p.Length >= p_2 ? p : string.Concat("0", p));
            return str;
        }

        private void AutoPrint()
        {
            System.Timers.Timer t = new System.Timers.Timer(this.timeout);
            t.Elapsed += new ElapsedEventHandler(this.CheckData);
            t.AutoReset = true;
            t.Enabled = true;
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(this.txtIP.Text);
            IPEndPoint point = new IPEndPoint(ip, int.Parse(this.txtPort.Text));
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(point);
                socket.Listen(10);
                this.ShowLogMessage("The server is listening...");
                Thread thread = new Thread(new ParameterizedThreadStart(this.AcceptInfo))
                {
                    IsBackground = true
                };
                thread.Start(socket);
                this.btnListen.Enabled = false;
            }
            catch (Exception exception)
            {
                this.ShowLogMessage(exception.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.txtIpPort.Text == "")
            {
                MessageBox.Show("The client is not connect,please check!");
            }
            else if (!(MyXmlSet.GetMyConfig("/configuration/srcFile").Trim() == ""))
            {
                this.AutoPrint();
                this.btnStart.Enabled = false;
            }
            else
            {
                MessageBox.Show("Please set source data first!");
            }
        }

        private void CheckData(object source, ElapsedEventArgs e)
        {
            string srcFile = string.Empty; //MyXmlSet.GetMyConfig("/configuration/srcFile");
            if(srcFiles.Count < 1)
            {
                MessageBox.Show("No one the source text file,please check source directory!");
                return;
            }
            else
                srcFile = srcFiles.Peek();
            if (File.Exists(srcFile))
            {
                if (!File.Exists(MyPublicCS.destFile))
                {
                    File.Copy(srcFile, MyPublicCS.destFile);
                    bool occuped = false;
                    while (!occuped)
                    {
                        try
                        {
                            File.Delete(srcFile);
                            srcFiles.Dequeue();
                            occuped = false;
                        }
                        catch (IOException)
                        {
                            occuped = true;
                        }

                    }

                    this.dataRowCount = 0;
                    DataTable dt = MyPublicCS.GetTxtData(MyPublicCS.destFile);
                    if ((dt == null ? true : dt.Rows.Count <= 0))
                    {
                        MessageBox.Show("The source text file is empty,please check!");
                    }
                    else
                    {
                        this.dataRowCount = dt.Rows.Count;
                        this.dataRowNumber = 1;
                        //MyXmlSet.SetMyConfig("/configuration/dataRowCount", dataRowCount.ToString());
                        //MyXmlSet.SetMyConfig("/configuration/dataLineNumber", "1");
                        this.MySendMsg();
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(MyPublicCS.destFile))
            {
                File.Delete(MyPublicCS.destFile);
            }
            String tms = MyXmlSet.GetMyConfig("configuration/net/timeout");
            timeout = Convert.ToInt32(tms);
            txtIP.Text = MyXmlSet.GetMyConfig("configuration/net/ip");
            txtPort.Text = MyXmlSet.GetMyConfig("configuration/net/port");
            string prgPath = Environment.CurrentDirectory + "\\FinishedData";
            if (!Directory.Exists(prgPath))
            {
                Directory.CreateDirectory(prgPath);
            }
            btnStart.Visible = false;
            FillSrcFles();
        }
        /// <summary>
        /// Заполнить очередь исходных файлов с кодами
        /// </summary>
        private void FillSrcFles()
        {
            string srcDir = MyXmlSet.GetMyConfig("/configuration/srcFolder");
            IEnumerable<string> files = Directory.EnumerateFiles(srcDir).Where(s=> s.ToLower().Contains(".txt")).OrderBy(s => s);
            srcFiles.Clear(); 
            foreach(string src in files)
            {
                srcFiles.Enqueue(src);
            }
        }

        private void frmSet_OnBrowseOpen(string s)
        {
            this.SetPrintedCount(s);
        }

        private void MySendMsg()
        {
            DataTable dtSrc = MyPublicCS.GetTxtData(MyPublicCS.destFile);
            if ((dtSrc == null ? false : dtSrc.Rows.Count >= 1))
            {
                string nDataLineNumber = this.dataRowNumber.ToString(); //MyXmlSet.GetMyConfig("/configuration/dataLineNumber");
                string dataRowCount = this.dataRowCount.ToString();//MyXmlSet.GetMyConfig("/configuration/dataRowCount");
                string msg = "";
                for (int i = 0; i < dtSrc.Rows.Count; i++)
                {
                    if (nDataLineNumber == dtSrc.Rows[i][0].ToString())
                    {
                        msg = dtSrc.Rows[i][1].ToString();
                    }
                }
                if (msg.Trim() != "")
                {
                    this.SendMsg(msg);
                    this.ShowLogMessage("Sent: "+ msg);
                    this.SetPrintedCount(string.Concat("Printing:", nDataLineNumber, "/", dataRowCount));
                }
            }
            else
            {
                MessageBox.Show("The source data text file is empty,please check!");
            }
        }

        private void ReceiveMsg(object o)
        {
            Socket client = o as Socket;
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int n = client.Receive(buffer);
                    string words = Encoding.UTF8.GetString(buffer, 0, n);
                    this.ShowLogMessage(string.Concat(client.RemoteEndPoint.ToString(), ":", words));
                    if (n > 0)
                    {
                        receiving = true;
                        int nDataLineNumber = this.dataRowNumber;//int.Parse(MyXmlSet.GetMyConfig("/configuration/dataLineNumber"));
                        int dataRowCount = this.dataRowCount;// int.Parse(MyXmlSet.GetMyConfig("/configuration/dataRowCount"));
                        if (nDataLineNumber >= dataRowCount)
                        {
                            this.SetPrintedCount("Printing completed!");
                            DateTime now = DateTime.Now;
                            object[] year = new object[] { now.Year, null, null, null, null, null, null, null };
                            int month = now.Month;
                            year[1] = this.AdptLength(month.ToString(), 2);
                            month = now.Day;
                            year[2] = this.AdptLength(month.ToString(), 2);
                            month = now.Hour;
                            year[3] = this.AdptLength(month.ToString(), 2);
                            month = now.Minute;
                            year[4] = this.AdptLength(month.ToString(), 2);
                            month = now.Second;
                            year[5] = this.AdptLength(month.ToString(), 2);
                            year[6] = now.Millisecond;
                            year[7] = ".txt";
                            string finishedFile = string.Concat(year);
                            if (File.Exists(MyPublicCS.destFile))
                            {
                                File.Copy(MyPublicCS.destFile, string.Concat(MyPublicCS.finishedFolder, finishedFile));
                                File.Delete(MyPublicCS.destFile);
                            }
                            this.dataRowNumber = -1;
                            this.dataRowCount = 0;
                            while (srcFiles.Count < 1)
                            {
                                FillSrcFles();
                            }
                            CheckData(null, null);
                        }
                        else
                        {
                            //MyXmlSet.SetMyConfig("/configuration/dataLineNumber", Convert.ToString(nDataLineNumber + 1));
                            this.dataRowNumber = nDataLineNumber + 1;
                            this.MySendMsg();                            
                        }
                    }
                    else 
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                        receiving = false;
                        break;
                    }
                }
                catch (Exception exception)
                {
                    this.ShowLogMessage(exception.Message);
                    break;
                }
            }
        }

        private void SendMsg(string msg)
        {
            string ip = this.txtIpPort.Text;
            byte[] buffer = Encoding.UTF8.GetBytes(string.Concat(msg, ";;"));
            try
            {
                this.dic[ip].Send(buffer);
            }
            catch
            {
                MessageBox.Show("The client ip is empty,please check!");
                return;
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
                Form1.DelegateShowPrinted delegateShowPrinted = new Form1.DelegateShowPrinted(this.SetPrintedCount);
                object[] objArray = new object[] { s };
                base.Invoke(delegateShowPrinted, objArray);
            }
        }

        private void ShowLogMessage(string msg)
        {
            this.txtLog.AppendText(string.Concat(msg, "\r\n"));
        }

        /// <summary>
        /// задать папку, где располагаются исходные файлы с кодами для маркировки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void systemSetingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = "Select source folder";
            folderBrowserDialog.SelectedPath = MyXmlSet.GetMyConfig("/configuration/srcFolder");
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                MyXmlSet.SetMyConfig("/configuration/srcFolder", folderBrowserDialog.SelectedPath);
                FillSrcFles();
            }
        }

        public void WriteLogMsg(string s)
        {
            if (!base.InvokeRequired)
            {
                TextBox textBox = this.txtLog;
                textBox.Text = string.Concat(textBox.Text, s, "\r\n");
            }
            else
            {
                Form1.DelegateShowPrinted delegateShowPrinted = new Form1.DelegateShowPrinted(this.WriteLogMsg);
                object[] objArray = new object[] { s };
                base.Invoke(delegateShowPrinted, objArray);
            }
        }

        private delegate void DelegateShowPrinted(string s);
    }
}