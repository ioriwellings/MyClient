using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NT88Test;
using SmartX1Demo;

namespace LBKJClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            int vv = 1;
            //int[] keyHandles = new int[8];
            //int[] keyNumber = new int[8];
            //SmartApp smart = new SmartApp();
            //vv = smart.SmartX1Find("GSPAutoMonitor", keyHandles, keyNumber);

            if (IntPtr.Size == 4)
            {
                vv = NT88_X86.NTFindFirst("longbangrj716");
            }
            else
            {
                vv = NT88_X64.NTFindFirst("longbangrj716");
            }
            if (vv != 0)
            {
                //MessageBox.Show("系统程序未检测到加密狗，请插入加密狗或联系售后人员！");
                DialogResult rr = MessageBox.Show("未检测到软件加密锁,请检查是否正确连接，点击忽略可以试用30分钟!", "操作提示", MessageBoxButtons.AbortRetryIgnore);
                int tt = (int)rr;
                if (tt == 4)
                {
                    Main();
                } else if (tt == 5) {
                    bool ret;
                    System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
                    if (ret)
                    {
                        Application.EnableVisualStyles();   //这两行实现   XP   可视风格   
                        Application.DoEvents();             //这两行实现   XP   可视风格

                        // 启动  开始动画
                        SplashScreen.ShowSplashScreen();
                        System.Threading.Thread.Sleep(1500);
                        SplashScreen.CloseSplashScreen();
                        //Application.SetCompatibleTextRenderingDefault(false);   
                        Application.Run(new frmMain());
                        //Main   为你程序的主窗体，如果是控制台程序不用这句   
                        mutex.ReleaseMutex();
                    }
                    else
                    {
                        MessageBox.Show(null, "有一个和本程序相同的应用程序正在运行，请先关闭！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //提示信息，可以删除
                        Application.Exit();//退出程序
                    }
                }
            }
            else
            {
                bool ret;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
            if (ret)
            {
                Application.EnableVisualStyles();   //这两行实现   XP   可视风格   
                Application.DoEvents();             //这两行实现   XP   可视风格

                // 启动  开始动画
                SplashScreen.ShowSplashScreen();
                System.Threading.Thread.Sleep(1500);
                //Application.SetCompatibleTextRenderingDefault(false);   
                Application.Run(new frmLogin());

                //Main   为你程序的主窗体，如果是控制台程序不用这句   
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show(null, "有一个和本程序相同的应用程序正在运行，请先关闭！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //提示信息，可以删除
                Application.Exit();//退出程序
            }
         }
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmLogin());
        }
    }
}
