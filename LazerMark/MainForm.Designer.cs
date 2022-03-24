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
        private TextBox txtLog;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        public Label lblPrintedCount;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPrintedCount = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtDataFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.menuFile = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveConfigurationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendTimer = new System.Windows.Forms.Timer(this.components);
            this.mainFormToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.menuFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(47, 30);
            this.txtIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(123, 22);
            this.txtIP.TabIndex = 0;
            this.txtIP.Text = "192.168.1.30";
            this.mainFormToolTip.SetToolTip(this.txtIP, "IP setting");
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(227, 30);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(67, 22);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "34567";
            this.mainFormToolTip.SetToolTip(this.txtPort, "Port setting");
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(16, 315);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(565, 207);
            this.txtLog.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtInterval);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnCheck);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtIP);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Location = new System.Drawing.Point(16, 43);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(425, 100);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP Server Config";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(319, 28);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(82, 32);
            this.btnCheck.TabIndex = 10;
            this.btnCheck.Text = "Check";
            this.mainFormToolTip.SetToolTip(this.btnCheck, "Checking connection");
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.OnCheck);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
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
            this.lblPrintedCount.Location = new System.Drawing.Point(16, 241);
            this.lblPrintedCount.Margin = new System.Windows.Forms.Padding(80, 0, 4, 0);
            this.lblPrintedCount.Name = "lblPrintedCount";
            this.lblPrintedCount.Size = new System.Drawing.Size(464, 53);
            this.lblPrintedCount.TabIndex = 64;
            this.lblPrintedCount.Text = "Printing:0/0";
            this.lblPrintedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(488, 241);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(95, 53);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "Start";
            this.mainFormToolTip.SetToolTip(this.btnStart, "Start data sending");
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.OnStart);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowse.Location = new System.Drawing.Point(525, 166);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(41, 22);
            this.btnBrowse.TabIndex = 67;
            this.btnBrowse.Text = "...";
            this.mainFormToolTip.SetToolTip(this.btnBrowse, "Data File choice");
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.OnFileBrowse);
            // 
            // txtDataFile
            // 
            this.txtDataFile.Location = new System.Drawing.Point(98, 166);
            this.txtDataFile.Margin = new System.Windows.Forms.Padding(4);
            this.txtDataFile.Name = "txtDataFile";
            this.txtDataFile.ReadOnly = true;
            this.txtDataFile.Size = new System.Drawing.Size(413, 22);
            this.txtDataFile.TabIndex = 66;
            this.mainFormToolTip.SetToolTip(this.txtDataFile, "Data File full name");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 65;
            this.label3.Text = "Data File:";
            // 
            // menuFile
            // 
            this.menuFile.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuFile.Location = new System.Drawing.Point(0, 0);
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(625, 28);
            this.menuFile.TabIndex = 68;
            this.menuFile.Text = "File";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveConfigurationMenuItem,
            this.clearLogMenuItem,
            this.exitMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveConfigurationMenuItem
            // 
            this.saveConfigurationMenuItem.Name = "saveConfigurationMenuItem";
            this.saveConfigurationMenuItem.Size = new System.Drawing.Size(180, 26);
            this.saveConfigurationMenuItem.Text = "Save Settings";
            this.saveConfigurationMenuItem.Click += new System.EventHandler(this.OnSaveConfig);
            // 
            // clearLogMenuItem
            // 
            this.clearLogMenuItem.Name = "clearLogMenuItem";
            this.clearLogMenuItem.Size = new System.Drawing.Size(180, 26);
            this.clearLogMenuItem.Text = "Clear Log";
            this.clearLogMenuItem.Click += new System.EventHandler(this.OnLogClear);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(180, 26);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.OnExit);
            // 
            // sendTimer
            // 
            this.sendTimer.Tick += new System.EventHandler(this.CheckData);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 17);
            this.label4.TabIndex = 69;
            this.label4.Text = "Sending interval";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(129, 67);
            this.txtInterval.MaxLength = 7;
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(100, 22);
            this.txtInterval.TabIndex = 70;
            this.mainFormToolTip.SetToolTip(this.txtInterval, "Time interval fo sending data");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 545);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDataFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblPrintedCount);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.menuFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuFile;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Sending Mark Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuFile.ResumeLayout(false);
            this.menuFile.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button btnCheck;
        private MenuStrip menuFile;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveConfigurationMenuItem;
        private ToolStripMenuItem exitMenuItem;
        private Timer sendTimer;
        private ToolStripMenuItem clearLogMenuItem;
        private ToolTip mainFormToolTip;
        private TextBox txtInterval;
        private Label label4;
    }
}
