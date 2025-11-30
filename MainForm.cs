using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PrintMenuCleaner
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            dgvKeys.Rows.Clear();
            Cleaner.ScanRegistry();
            foreach (var key in Cleaner.FoundKeys)
            {
                dgvKeys.Rows.Add(true, key);
            }
            Log("扫描完成");
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            Cleaner.BackupKeys();
            Log($"备份完成：{Cleaner.BackupPath}");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var toDelete = new List<string>();
            foreach (DataGridViewRow row in dgvKeys.Rows)
            {
                if ((bool)row.Cells[0].Value)
                    toDelete.Add((string)row.Cells[1].Value);
            }
            // 使用 SetFoundKeys 方法，而不是直接赋值
            Cleaner.SetFoundKeys(toDelete);
            Cleaner.DeleteKeys();
            Log("删除完成");
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new()
            {
                Filter = "注册表文件|*.reg",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Cleaner.Restore(ofd.FileName);
                Log("恢复完成");
            }
        }

        private void Log(string msg)
        {
            txtLog.AppendText($"{DateTime.Now:HH:mm:ss} {msg}\r\n");
        }
    }
}
