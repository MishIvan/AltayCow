using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SocketSample
{
    public class frmSet : Form
    {
        private IContainer components = null;

        private Label label1;

        private TextBox txtDataFile;

        private Button btnBrowse;

        public frmSet()
        {
            this.InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string resultFile = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                InitialDirectory = "./OriginalData/",
                Filter = "Text file (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                resultFile = openFileDialog1.FileName;
                this.txtDataFile.Text = resultFile;
                MyXmlSet.SetMyConfig("/configuration/srcFile", resultFile);
                if (File.Exists(MyPublicCS.destFile))
                {
                    File.Delete(MyPublicCS.destFile);
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

        private void frmSet_Load(object sender, EventArgs e)
        {
            this.MyInit();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSet));
            this.label1 = new System.Windows.Forms.Label();
            this.txtDataFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 61);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "Data File Location:";
            // 
            // txtDataFile
            // 
            this.txtDataFile.Location = new System.Drawing.Point(32, 83);
            this.txtDataFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDataFile.Name = "txtDataFile";
            this.txtDataFile.ReadOnly = true;
            this.txtDataFile.Size = new System.Drawing.Size(356, 22);
            this.txtDataFile.TabIndex = 21;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(31, 113);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(72, 31);
            this.btnBrowse.TabIndex = 22;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // frmSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 197);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDataFile);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "frmSet";
            this.Text = "frmSet";
            this.Load += new System.EventHandler(this.frmSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MyInit()
        {
            this.txtDataFile.Text = MyXmlSet.GetMyConfig("/configuration/srcFile");
        }

        public static event frmSet.DBrowseOpen OnBrowseOpen;

        public delegate void DBrowseOpen(string ss);
    }
}