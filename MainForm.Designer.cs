namespace PrintMenuCleaner
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvKeys;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.TextBox txtLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvKeys = new System.Windows.Forms.DataGridView();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKeys)).BeginInit();
            this.SuspendLayout();
            // dgvKeys
            this.dgvKeys.AllowUserToAddRows = false;
            this.dgvKeys.AllowUserToDeleteRows = false;
            this.dgvKeys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKeys.Columns.Add(new System.Windows.Forms.DataGridViewCheckBoxColumn() { HeaderText = "删除", Width = 50 });
            this.dgvKeys.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn() { HeaderText = "注册表键", Width = 600 });
            this.dgvKeys.Location = new System.Drawing.Point(12, 12);
            this.dgvKeys.Name = "dgvKeys";
            this.dgvKeys.RowTemplate.Height = 24;
            this.dgvKeys.Size = new System.Drawing.Size(760, 350);
            this.dgvKeys.TabIndex = 0;
            // btnScan
            this.btnScan.Location = new System.Drawing.Point(12, 370);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(90, 30);
            this.btnScan.TabIndex = 1;
            this.btnScan.Text = "扫描";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // btnBackup
            this.btnBackup.Location = new System.Drawing.Point(108, 370);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(90, 30);
            this.btnBackup.TabIndex = 2;
            this.btnBackup.Text = "备份";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(204, 370);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 30);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // btnRestore
            this.btnRestore.Location = new System.Drawing.Point(300, 370);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(90, 30);
            this.btnRestore.TabIndex = 4;
            this.btnRestore.Text = "恢复";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // txtLog
            this.txtLog.Location = new System.Drawing.Point(12, 410);
            this.txtLog.Multiline = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(760, 130);
            this.txtLog.TabIndex = 5;
            // MainForm
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.dgvKeys);
            this.Name = "MainForm";
            this.Text = "PrintMenuCleaner";
            ((System.ComponentModel.ISupportInitialize)(this.dgvKeys)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
