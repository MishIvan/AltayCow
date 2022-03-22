using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LazerMark
{
    public partial class MainForm 
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

        private ToolStripMenuItem refferenceToolStripMenuItem;

        private ToolStripMenuItem systemSetingToolStripMenuItem;

        private Button btnStart;
        private Button btnBrowse;
        private TextBox txtDataFile;
        private Label label3;
        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnListen = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtIpPort = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPrintedCount = new System.Windows.Forms.Label();
            this.refferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemSetingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtDataFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
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
            this.btnListen.Click += new System.EventHandler(this.OnListen);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(16, 247);
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
            this.groupBox1.Location = new System.Drawing.Point(16, 11);
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
            this.lblPrintedCount.Location = new System.Drawing.Point(16, 173);
            this.lblPrintedCount.Margin = new System.Windows.Forms.Padding(80, 0, 4, 0);
            this.lblPrintedCount.Name = "lblPrintedCount";
            this.lblPrintedCount.Size = new System.Drawing.Size(464, 53);
            this.lblPrintedCount.TabIndex = 64;
            this.lblPrintedCount.Text = "Printing:0/0";
            this.lblPrintedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // refferenceToolStripMenuItem
            // 
            this.refferenceToolStripMenuItem.Name = "refferenceToolStripMenuItem";
            this.refferenceToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // systemSetingToolStripMenuItem
            // 
            this.systemSetingToolStripMenuItem.Name = "systemSetingToolStripMenuItem";
            this.systemSetingToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(488, 173);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(95, 53);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.OnStart);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowse.Location = new System.Drawing.Point(525, 117);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(41, 22);
            this.btnBrowse.TabIndex = 67;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.OnFileBrowse);
            // 
            // txtDataFile
            // 
            this.txtDataFile.Location = new System.Drawing.Point(104, 117);
            this.txtDataFile.Margin = new System.Windows.Forms.Padding(4);
            this.txtDataFile.Name = "txtDataFile";
            this.txtDataFile.ReadOnly = true;
            this.txtDataFile.Size = new System.Drawing.Size(413, 22);
            this.txtDataFile.TabIndex = 66;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 117);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 65;
            this.label3.Text = "Data File:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 476);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDataFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblPrintedCount);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtLog);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "BslSockets System";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClose);
            this.Load += new System.EventHandler(this.Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


    }
}
