using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class SplashScreen : Form
    {
        /// <summary>  
        /// 启动画面本身  
        /// </summary>  
        static SplashScreen instance;

        /// <summary>  
        /// 显示的图片  
        /// </summary>  
        Bitmap bitmap;

        public static SplashScreen Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        public SplashScreen()
        {
            InitializeComponent();
            string str = Application.StartupPath;//项目路径
            // 设置窗体的类型  
            const string showInfo = "软件程序加载中，请稍后...";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = false;
            bitmap = new Bitmap(@str + "/images/startPic.png");
            ClientSize = bitmap.Size;

            using (Font font = new Font("Consoles", 12))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    //g.DrawString(showInfo, font, Brushes.Black, 130, 315);
                    g.DrawString(showInfo, font, Brushes.Black, 250, 255);
                    Font font1 = new Font("Consoles", 12);
                    string aa = "主程序版本：V" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    g.DrawString(aa, font1, Brushes.Red, 20, 330);
                    string bb = "核心版本：V"+Environment.Version.ToString().Substring(0,9);
                    g.DrawString(bb, font1, Brushes.Red, 195, 330);
                    string cc = "本软件受版权法保护";
                    g.DrawString(cc, font1, Brushes.Red, 380, 330);
                }
            }

            BackgroundImage = bitmap;
        }
    
        public static void ShowSplashScreen()
        {
            instance = new SplashScreen();
            instance.Show();
        }
        public static void CloseSplashScreen()
        {
            instance.Close();
        }
        private void SplashScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
