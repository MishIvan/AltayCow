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

namespace SocketSample
{
    public class Form1 : Form
    {
        private IContainer components = null;

        private TextBox txtIP;

        private TextBox txtPort;

        private Button btnListen;

        private TextBox txtLog;

        private TextBox txtIpPort;

        private GroupBox groupBox1;

        private Label label1;

        private Label label2;

        public Label lblPrintedCount;

        private MenuStrip menuStrip1;

        private ToolStripMenuItem refferenceToolStripMenuItem;

        private ToolStripMenuItem systemSetingToolStripMenuItem;

        private Button btnStart;
        private ToolStripMenuItem referenceToolStripMenuItem;
        private ToolStripMenuItem systemSetToolStripMenuItem;
        private Dictionary<string, Socket> dic = new Dictionary<string, Socket>();
        int timeout;

        public Form1()
        {
            this.InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            timeout = 0;
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
                this.ShowMsg("The server is listening...");
                Thread thread = new Thread(new ParameterizedThreadStart(this.AcceptInfo))
                {
                    IsBackground = true
                };
                thread.Start(socket);
                this.btnListen.Enabled = false;
            }
            catch (Exception exception)
            {
                this.ShowMsg(exception.Message);
            }
        }

        //private void btnSendMsg_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.SendMsg("");
        //    }
        //    catch (Exception exception)
        //    {
        //        this.ShowMsg(exception.Message);
        //    }
        //}

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
            string srcFile = MyXmlSet.GetMyConfig("/configuration/srcFile");
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
            }
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MyInit();
        }

        private void frmSet_OnBrowseOpen(string s)
        {
            this.SetPrintedCount(s);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnListen = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtIpPort = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPrintedCount = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.referenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemSetingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(47, 36);
            this.txtIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(123, 22);
            this.txtIP.TabIndex = 0;
            this.txtIP.Text = "192.168.1.30";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(227, 40);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(67, 22);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "34567";
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(304, 37);
            this.btnListen.Margin = new System.Windows.Forms.Padding(4);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(73, 31);
            this.btnListen.TabIndex = 2;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(16, 231);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(565, 207);
            this.txtLog.TabIndex = 4;
            // 
            // txtIpPort
            // 
            this.txtIpPort.Location = new System.Drawing.Point(417, 39);
            this.txtIpPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtIpPort.Name = "txtIpPort";
            this.txtIpPort.ReadOnly = true;
            this.txtIpPort.Size = new System.Drawing.Size(133, 22);
            this.txtIpPort.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtIP);
            this.groupBox1.Controls.Add(this.txtIpPort);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.btnListen);
            this.groupBox1.Location = new System.Drawing.Point(16, 51);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(567, 89);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP Server Config";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP:";
            // 
            // lblPrintedCount
            // 
            this.lblPrintedCount.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblPrintedCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPrintedCount.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintedCount.ForeColor = System.Drawing.Color.Black;
            this.lblPrintedCount.Location = new System.Drawing.Point(16, 157);
            this.lblPrintedCount.Margin = new System.Windows.Forms.Padding(80, 0, 4, 0);
            this.lblPrintedCount.Name = "lblPrintedCount";
            this.lblPrintedCount.Size = new System.Drawing.Size(464, 53);
            this.lblPrintedCount.TabIndex = 64;
            this.lblPrintedCount.Text = "Printing:0/0";
            this.lblPrintedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.referenceToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(604, 30);
            this.menuStrip1.TabIndex = 65;
            this.menuStrip1.Text = "Recceffrence";
            // 
            // referenceToolStripMenuItem
            // 
            this.referenceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemSetToolStripMenuItem});
            this.referenceToolStripMenuItem.Name = "referenceToolStripMenuItem";
            this.referenceToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
            this.referenceToolStripMenuItem.Text = "Reference";
            // 
            // systemSetToolStripMenuItem
            // 
            this.systemSetToolStripMenuItem.Name = "systemSetToolStripMenuItem";
            this.systemSetToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.systemSetToolStripMenuItem.Text = "System set";
            this.systemSetToolStripMenuItem.Click += new System.EventHandler(this.systemSetingToolStripMenuItem_Click);
            // 
            // refferenceToolStripMenuItem
            // 
            this.refferenceToolStripMenuItem.Name = "refferenceToolStripMenuItem";
            this.refferenceToolStripMenuItem.Size = new System.Drawing.Size(82, 21);
            this.refferenceToolStripMenuItem.Text = "Refference";
            // 
            // systemSetingToolStripMenuItem
            // 
            this.systemSetingToolStripMenuItem.Name = "systemSetingToolStripMenuItem";
            this.systemSetingToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.systemSetingToolStripMenuItem.Text = "System Set";
            this.systemSetingToolStripMenuItem.Click += new System.EventHandler(this.systemSetingToolStripMenuItem_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(488, 157);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(95, 53);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 455);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblPrintedCount);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "BslSockets System";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MyInit()
        {
            frmSet.OnBrowseOpen += new frmSet.DBrowseOpen(this.frmSet_OnBrowseOpen);
            if (File.Exists(MyPublicCS.destFile))
            {
                File.Delete(MyPublicCS.destFile);
            }
            String tms = MyXmlSet.GetMyConfig("configuration/net/timeout");
            timeout = Convert.ToInt32(tms);
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
                Form1.DelegateShowPrinted delegateShowPrinted = new Form1.DelegateShowPrinted(this.SetPrintedCount);
                object[] objArray = new object[] { s };
                base.Invoke(delegateShowPrinted, objArray);
            }
        }

        private void ShowMsg(string msg)
        {
            this.txtLog.AppendText(string.Concat(msg, "\r\n"));
        }

        private void systemSetingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new frmSet()).Show();
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