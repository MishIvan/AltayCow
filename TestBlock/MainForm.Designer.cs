
namespace TestBlock
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.procDataGridView = new System.Windows.Forms.DataGridView();
            this.ProcColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.procDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // procDataGridView
            // 
            this.procDataGridView.AllowUserToAddRows = false;
            this.procDataGridView.AllowUserToDeleteRows = false;
            this.procDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.procDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProcColumn,
            this.FileColumn});
            this.procDataGridView.Location = new System.Drawing.Point(13, 13);
            this.procDataGridView.Name = "procDataGridView";
            this.procDataGridView.ReadOnly = true;
            this.procDataGridView.RowHeadersWidth = 51;
            this.procDataGridView.RowTemplate.Height = 24;
            this.procDataGridView.Size = new System.Drawing.Size(623, 413);
            this.procDataGridView.TabIndex = 0;
            // 
            // ProcColumn
            // 
            this.ProcColumn.DataPropertyName = "ProcName";
            this.ProcColumn.HeaderText = "Процесс";
            this.ProcColumn.MinimumWidth = 6;
            this.ProcColumn.Name = "ProcColumn";
            this.ProcColumn.ReadOnly = true;
            this.ProcColumn.Width = 200;
            // 
            // FileColumn
            // 
            this.FileColumn.DataPropertyName = "FileName";
            this.FileColumn.HeaderText = "Файл";
            this.FileColumn.MinimumWidth = 6;
            this.FileColumn.Name = "FileColumn";
            this.FileColumn.ReadOnly = true;
            this.FileColumn.Width = 350;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 450);
            this.Controls.Add(this.procDataGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Test Locked Files";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.procDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView procDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileColumn;
    }
}

