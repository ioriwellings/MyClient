using System;
using System.Drawing;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class updateManageHost : Form
    {
        bean.manageHose mh = null;
        public updateManageHost()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           string name= this.textBox1.Text;
           string cdnum= this.numericUpDown1.Value.ToString();
           string cktype = this.comboBox3.SelectedItem.ToString();
           string txxy= this.comboBox2.SelectedItem.ToString();
           string kflx = this.comboBox1.SelectedValue.ToString();
            if (name!=null&&!"".Equals(name)&&cdnum!=null&& cktype!=null&& txxy!=null && kflx != null) {
                mh = new bean.manageHose();
                service.manageHostService mhs = new service.manageHostService();
                bool istrue = false;
                mh.hostName = name;
                mh.CommunicationType = txxy;
                mh.portNumber = cdnum;
                mh.storeType = cktype;
                mh.measureCode = this.textBox2.Text;
                mh.houseType = kflx;
                if (this.radioButton1.Checked)
                {
                    mh.hostAddress = this.numericUpDown2.Value.ToString();
                    mh.serialPort = this.comboBox4.Text;
                    mh.tcp_ip_Port = "";
                    mh.networkType = "COM";
                }
                else if (this.radioButton2.Checked)
                {
                    mh.hostAddress = this.numericUpDown3.Value.ToString();
                    mh.tcp_ip_Port = this.textBox4.Text + ":" + this.numericUpDown4.Value.ToString();
                    mh.serialPort = "";
                    mh.networkType = "TCP";
                }
                else
                {
                    mh.hostAddress = "";
                    mh.serialPort = "";
                    mh.tcp_ip_Port = "";
                    mh.networkType = "YUN";
                }
                istrue = mhs.updateManageHost(mh);
                this.DialogResult = DialogResult.OK;

            }
            else {
                MessageBox.Show("请输入主机名称！");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void updateManageHost_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/cancel.png");
            service.houseTypeService hts = new service.houseTypeService();
            this.comboBox1.DataSource = hts.queryhouseType();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "id";//操作时获取的值
            string hcode = this.textBox3.Text;
            if (hcode != null && !"".Equals(hcode))
            {
                this.comboBox1.Text = hcode;
            }
            //string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
            //this.comboBox4.Items.Clear();
            //if (ArryPort.Length > 0)
            //{
            //    for (int i = 0; i < ArryPort.Length; i++)
            //    {
            //        this.comboBox4.Items.Add(ArryPort[i]);
            //    }
            //}
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.numericUpDown2.Enabled = true;
                this.comboBox4.Enabled = true;
            }
            if (!this.radioButton1.Checked)
            {
                this.numericUpDown2.Enabled = false;
                this.comboBox4.Enabled = false;

            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.numericUpDown3.Enabled = true; 
                this.textBox4.Enabled = true; 
                this.numericUpDown4.Enabled = true;
            }
            if (!this.radioButton2.Checked)
            {
                this.numericUpDown3.Enabled = false;
                this.textBox4.Enabled = false;
                this.numericUpDown4.Enabled = false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedItem.ToString() != "[管理主机]LB863RSB_N1(LBGZ-02)")
            {
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton1.Enabled = false;
                this.radioButton2.Enabled = false;
            }
            else
            {
                this.radioButton1.Enabled = true;
                this.radioButton2.Enabled = true;
            }
        }
    }
}
