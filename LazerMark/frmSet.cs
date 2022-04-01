using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace LazerMark
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmSet));
            this.label1 = new Label();
            this.txtDataFile = new TextBox();
            this.btnBrowse = new Button();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(25, 46);
            this.label1.Name = "label1";
            this.label1.Size = new Size(119, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "Data File Location:";
            this.txtDataFile.Location = new Point(24, 62);
            this.txtDataFile.Name = "txtDataFile";
            this.txtDataFile.ReadOnly = true;
            this.txtDataFile.Size = new Size(268, 21);
            this.txtDataFile.TabIndex = 21;
            this.btnBrowse.Location = new Point(23, 85);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new Size(54, 23);
            this.btnBrowse.TabIndex = 22;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(304, 148);
            base.Controls.Add(this.btnBrowse);
            base.Controls.Add(this.txtDataFile);
            base.Controls.Add(this.label1);
            base.Icon = (Icon)resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "frmSet";
            this.Text = "frmSet";
            base.Load += new EventHandler(this.frmSet_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void MyInit()
        {
            this.txtDataFile.Text = MyXmlSet.GetMyConfig("/configuration/srcFile");
        }

        public static event frmSet.DBrowseOpen OnBrowseOpen;

        public delegate void DBrowseOpen(string ss);
    }
}