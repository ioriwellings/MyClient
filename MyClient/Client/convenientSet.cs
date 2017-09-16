using System;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class convenientSet : Form
    {
        SerialPort port=new SerialPort();
        public delegate void UpdateAcceptTextBoxTextHandler(string text);
        public UpdateAcceptTextBoxTextHandler UpdateTextHandler;
        public delegate void SendDeviceInfoHandler(string text);
        public SendDeviceInfoHandler SendInfotHandler;
        public delegate void invokeDisplay(string str);
        Byte[] totalByteRead = new Byte[0];
        string text = string.Empty;
        int flag = 0;
        string id = "";
        string address = "";
        string TarStr1 = "yyyy-MM-dd HH:mm:ss";
        string TarStr = "yyMMddHHmmss";
        IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
        DateTime MyDate;

        public convenientSet()
        {
            InitializeComponent();
        }

        private void convenientSet_Load(object sender, EventArgs e) 
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/cancel.png");

            id = this.textBox4.Text.Split('-').Last();
            address = this.textBox5.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = this.textBox3.Text;
            string codemeter = this.textBox4.Text;
            string t_high = this.numericUpDown4.Value.ToString();
            string t_low = this.numericUpDown2.Value.ToString();
            string h_high = this.numericUpDown1.Value.ToString();
            string h_low = this.numericUpDown5.Value.ToString();
            if (name != null && double.Parse(t_high) > double.Parse(t_low))
            { 
                bean.deviceInformation di = new bean.deviceInformation();
                string[] mm = codemeter.Split('-');
                di.measureCode = mm[0];
                di.meterNo = mm[1];
                di.terminalname = name;
                di.t_high = float.Parse(t_high);
                di.t_low = float.Parse(t_low);
                di.h_high = float.Parse(h_high);
                di.h_low = float.Parse(h_low);
                if (this.checkBox1.Checked)
                {
                    di.powerflag = Int32.Parse(this.checkBox1.Tag.ToString());
                }
                else
                {
                    di.powerflag = 1;
                };
                service.deviceInformationService dis = new service.deviceInformationService();
                bool isok = dis.updateIformation(di);
                if (isok)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                MessageBox.Show("测点名称不能为空，温度上限应大于温度下限！");
            }
        }

    }
}
