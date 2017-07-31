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
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            //this.button1.BackgroundImage = Image.FromFile(@str + "/images/assign.png");
            this.pictureBox1.BackgroundImage= Image.FromFile(@str + "/images/about.png");
            string aa = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string bb = Environment.Version.ToString().Substring(0, 9);
            this.label1.Text = "\n" + "温湿度自动化检测系统" + "\n" + "\n" + "版本  主程序版本：V"+aa+"\n" + "\n" + "核心版本：V"+bb+" Copyright  龙邦科技"+"\n" + "\n" + "北京龙邦科技发展有限公司"+"\n" + "\n" + "温湿度自动化检测系统.NET版(2017)";
        }
    }
}
