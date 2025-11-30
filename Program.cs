using System;
using System.Linq;
using System.Windows.Forms;

namespace PrintMenuCleaner
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var firstArg = args[0].ToLower();
                if (firstArg == "--scan-only")
                {
                    Console.WriteLine("CLI 模式：只扫描注册表...");
                    Cleaner.ScanAndPrint();
                    return;
                }
                else if (firstArg == "--no-backup")
                {
                    Console.WriteLine("CLI 模式：删除所有 print/printto，不生成备份...");
                    Cleaner.ScanAndDelete(noBackup: true);
                    return;
                }
                else if (firstArg == "--restore" && args.Length > 1)
                {
                    Console.WriteLine($"CLI 模式：从备份文件恢复 {args[1]} ...");
                    Cleaner.Restore(args[1]);
                    return;
                }
                else
                {
                    Console.WriteLine("未知参数。支持：--scan-only, --no-backup, --restore <file>");
                    return;
                }
            }

            // GUI 模式
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
