using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMenuCleaner
{
    public static class Cleaner
    {
        static readonly string[] CriticalPrefixes = new[]
        {
            "CLSID", "AppID", "TypeLib", "Interface",
            "Wow6432Node", "Windows.", "SystemFileAssociations"
        };

        // FoundKeys 改为只读，提供方法设置
        public static List<string> FoundKeys { get; private set; } = new List<string>();

        private static string AppRoot =>
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.FullName;

        public static string BackupPath =>
            Path.Combine(AppRoot, $"PrintMenuBackup_{DateTime.Now:yyyyMMdd_HHmmss}.reg");

        public static string LogPath =>
            Path.Combine(AppRoot, $"PrintMenuLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

        #region 设置 FoundKeys 方法
        public static void SetFoundKeys(List<string> keys)
        {
            FoundKeys = keys;
        }
        #endregion

        #region CLI辅助方法
        public static void ScanAndPrint()
        {
            ScanRegistry();
            Console.WriteLine($"共找到 {FoundKeys.Count} 个 print / printto 菜单：");
            foreach (var k in FoundKeys) Console.WriteLine(k);
        }

        public static void ScanAndDelete(bool noBackup = false)
        {
            ScanRegistry();
            if (!noBackup) BackupKeys();
            DeleteKeys();
        }

        public static void Restore(string regFile)
        {
            if (!File.Exists(regFile))
            {
                Console.WriteLine($"文件不存在：{regFile}");
                return;
            }
            var psi = new System.Diagnostics.ProcessStartInfo("regedit.exe", $"/s \"{regFile}\"")
            {
                UseShellExecute = true,
                Verb = "runas"
            };
            System.Diagnostics.Process.Start(psi)?.WaitForExit();
            Console.WriteLine("恢复完成！");
        }
        #endregion

        #region 核心逻辑
        public static void ScanRegistry()
        {
            FoundKeys.Clear();
            using RegistryKey root = Registry.ClassesRoot;
            string[] subKeys = root.GetSubKeyNames();

            Parallel.ForEach(subKeys, sub =>
            {
                if (IsCritical(sub)) return;

                string shellPath = $@"{sub}\shell";
                using var shell = root.OpenSubKey(shellPath);
                if (shell == null) return;

                foreach (string action in shell.GetSubKeyNames())
                {
                    if (action.Equals("print", StringComparison.OrdinalIgnoreCase) ||
                        action.Equals("printto", StringComparison.OrdinalIgnoreCase))
                    {
                        lock (FoundKeys)
                        {
                            string fullPath = $@"HKCR\{shellPath}\{action}";
                            FoundKeys.Add(fullPath);
                            Log("发现：" + fullPath);
                        }
                    }
                }
            });

            FoundKeys.Sort();
        }

        private static bool IsCritical(string name)
        {
            return CriticalPrefixes.Any(p => name.StartsWith(p, StringComparison.OrdinalIgnoreCase));
        }

        public static void BackupKeys()
        {
            using var writer = new StreamWriter(BackupPath, false, Encoding.Unicode);
            writer.WriteLine("Windows Registry Editor Version 5.00\n");

            foreach (var key in FoundKeys)
            {
                string pureKey = key.Replace("HKCR\\", "");
                using var reg = Registry.ClassesRoot.OpenSubKey(pureKey);
                if (reg == null) continue;

                writer.WriteLine($"[{key}]");
                foreach (var val in reg.GetValueNames())
                {
                    var data = reg.GetValue(val);
                    if (data == null) continue;
                    writer.WriteLine($"\"{val}\"=\"{data}\"");
                }
                writer.WriteLine();
            }
            Log($"已写入备份：{BackupPath}");
        }

        public static void DeleteKeys()
        {
            foreach (var key in FoundKeys)
            {
                try
                {
                    string path = key.Replace("HKCR\\", "");
                    Registry.ClassesRoot.DeleteSubKeyTree(path, false);
                    Log("删除成功：" + key);
                }
                catch (Exception ex)
                {
                    Log($"删除失败：{key} —— {ex.Message}");
                }
            }
        }

        public static void Log(string msg)
        {
            lock (LogPath)
            {
                File.AppendAllText(LogPath, msg + Environment.NewLine, Encoding.UTF8);
            }
        }
        #endregion
    }
}
