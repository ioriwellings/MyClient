using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LBKJClient.utils
{
    class DataBaseUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPath">需要备份的数据库路径</param>
        /// <param name="targetPath">数据库备份目标路径</param>
        public static void backupDatabase(String srcPath, String targetPath)
        {
            Process p = new Process();
            p.StartInfo.FileName = @"cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            p.StandardInput.WriteLine("sqlite3.exe " + srcPath); 
            p.StandardInput.WriteLine();
            p.StandardInput.AutoFlush = true;
            Thread.Sleep(500);
            p.StandardInput.WriteLine(".backup " + targetPath);  
            p.StandardInput.WriteLine();
            p.StandardInput.AutoFlush = true;
            Thread.Sleep(500);
            p.StandardInput.WriteLine("exit");
            p.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPath">需要还原的数据库路径</param>
        /// <param name="targetPath">使用的备份数据库路径</param>
        public static void restoreDatabase(String srcPath, String targetPath)
        {
            Process p = new Process();
            p.StartInfo.FileName = @"cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            p.StandardInput.WriteLine("sqlite3.exe " + srcPath); 
            p.StandardInput.WriteLine();
            p.StandardInput.AutoFlush = true;
            Thread.Sleep(500);
            p.StandardInput.WriteLine(".restore " + targetPath); 
            p.StandardInput.WriteLine();
            p.StandardInput.AutoFlush = true;
            Thread.Sleep(500);
            p.StandardInput.WriteLine("exit");
            p.Close();
        }
    }
}
