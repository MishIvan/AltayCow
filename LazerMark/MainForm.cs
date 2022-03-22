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
        private Dictionary<string, Socket> dic = new Dictionary<string, Socket>();
        SentInfo sentInfo;
        bool dataSent;
        Socket socket;
        public MainForm()
        {
            this.InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            sentInfo = new SentInfo();
            dataSent = false;
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
                    this.ShowMsg(string.Concat(point, "Connect successfully！"));
                    this.txtIpPort.Text = point;
                    this.dic.Add(point, tSocket);
                    Thread th = new Thread(new ParameterizedThreadStart(this.ReceiveMsg))
                    {
                        IsBackground = true
                    };
                    th.Start(tSocket);
                }
                catch (Exception exception)
                {
                    this.ShowMsg(exception.Message);
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
            //System.Timers.Timer t = new System.Timers.Timer(10);
            //t.Elapsed += new ElapsedEventHandler(this.CheckData);
            //t.AutoReset = true;
            //t.Enabled = !dataSent;
            CheckData(null, null);
        }

        /// <summary>
        /// Нажание на кнопку Listen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListen(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            IPEndPoint point = new IPEndPoint(ip, int.Parse(txtPort.Text));
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //socket.Bind(point);
                //socket.Listen(10);
                socket.Connect(point);
                this.ShowMsg("The server is listening...");
                //Thread thread = new Thread(new ParameterizedThreadStart(this.AcceptInfo))
                //{
                //    IsBackground = true
                //};
                //thread.Start(socket);
                this.btnListen.Enabled = false;
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

            /*if (this.txtIpPort.Text == "")
            {
                MessageBox.Show("The client is not connect,please check!");
            }
            else
            if (!(MyXmlSet.GetMyConfig("/configuration/srcFile").Trim() == ""))
            {
                this.AutoPrint();
                this.btnStart.Enabled = false;
            }
            else
            {
                MessageBox.Show("Please set source data first!");
            } */
        }

    private void CheckData(object source, ElapsedEventArgs e)
    {
        if(!sentInfo.IsEmpty())
        {
            String str = sentInfo.GetString();            
            byte[] buffer = Encoding.UTF8.GetBytes(string.Concat(str, ";;"));
            int n = socket.Send(buffer);
            if (n > 0)
            {
                WriteLogMsg("Sent: " + str);
                byte[] recvbuf = new byte[1024];
                n = socket.Receive(recvbuf);
                if(n > 0)
                {
                    String s1 = Encoding.UTF8.GetString(recvbuf);
                    sentInfo.Remove();
                    WriteLogMsg("Received: " + s1);

                }
            }

        }
        else
        {
            WriteLogMsg("All data has been sent...");
            dataSent = true;
        }
          
        /*string srcFile = MyXmlSet.GetMyConfig("/configuration/srcFile");            
        if (File.Exists(srcFile))
        {
            if (!File.Exists(MyPublicCS.destFile))
            {
                File.Copy(srcFile, MyPublicCS.destFile);
                File.Delete(srcFile);
                int dataRowCount = 0;
                DataTable dt = MyPublicCS.GetTxtData(MyPublicCS.destFile);
                if ((dt == null ? true : dt.Rows.Count <= 0))
                {
                    MessageBox.Show("The source text file is empty,please check!");
                }
                else
                {
                    dataRowCount = dt.Rows.Count;
                    MyXmlSet.SetMyConfig("/configuration/dataRowCount", dataRowCount.ToString());
                    MyXmlSet.SetMyConfig("/configuration/dataLineNumber", "1");
                    this.MySendMsg();
                }
            }
        }*/
    }


    private void Form_Load(object sender, EventArgs e)
    {
            this.txtDataFile.Text = Settings.Default.srcFile;
                //MyXmlSet.GetMyConfig("/configuration/srcFile");

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
            MyXmlSet.SetMyConfig("/configuration/srcFile", resultFile);
            if (File.Exists(MyPublicCS.destFile))
            {
                File.Delete(MyPublicCS.destFile);
            }
        }
    }

    private void MySendMsg()
    {
        DataTable dtSrc = MyPublicCS.GetTxtData(MyPublicCS.destFile);
        if ((dtSrc == null ? false : dtSrc.Rows.Count >= 1))
        {
            string nDataLineNumber = MyXmlSet.GetMyConfig("/configuration/dataLineNumber");
            string dataRowCount = MyXmlSet.GetMyConfig("/configuration/dataRowCount");
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
                WriteLogMsg(msg);
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
                    this.ShowMsg(string.Concat(client.RemoteEndPoint.ToString(), ":", words));
                    if (words.IndexOf("MarkCount:") > -1)
                    {
                        int nDataLineNumber = int.Parse(MyXmlSet.GetMyConfig("/configuration/dataLineNumber"));
                        int dataRowCount = int.Parse(MyXmlSet.GetMyConfig("/configuration/dataRowCount"));
                        if ((0 >= nDataLineNumber ? true : nDataLineNumber >= dataRowCount))
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
                        }
                        else
                        {
                            MyXmlSet.SetMyConfig("/configuration/dataLineNumber", Convert.ToString(nDataLineNumber + 1));
                            this.MySendMsg();
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.ShowMsg(exception.Message);
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
                MainForm.DelegateShowPrinted delegateShowPrinted = new MainForm.DelegateShowPrinted(this.SetPrintedCount);
                object[] objArray = new object[] { s };
                base.Invoke(delegateShowPrinted, objArray);
            }
        }

        private void ShowMsg(string msg)
        {
            this.txtLog.AppendText(string.Concat(msg, "\r\n"));
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
                MainForm.DelegateShowPrinted delegateShowPrinted = new MainForm.DelegateShowPrinted(this.WriteLogMsg);
                object[] objArray = new object[] { s };
                base.Invoke(delegateShowPrinted, objArray);
            }
        }

        private delegate void DelegateShowPrinted(string s);

        private void OnClose(object sender, FormClosedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection app = config.AppSettings;
            this.socket.Close();
            //app.Settings["srcFile"].Value = txtDataFile.Text;
            //config.Save(ConfigurationSaveMode.Modified);
        }
    }
}