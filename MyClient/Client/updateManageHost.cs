using System;
using System.Data;
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
            //验证主机名称是否重复
            service.manageHostService mhs = new service.manageHostService();
            DataTable dt = mhs.queryManageHost();
            if (name0 != "" && name0 != name)
            {
                DataRow[] drr = dt.Select("hostName = '" + name + "'");
                if (drr.Length > 0)
                {
                    MessageBox.Show("主机名称已存在，请重新输入！");
                    return;
                }
            }
            string cdnum= this.numericUpDown1.Value.ToString();
           string cktype = this.comboBox3.Text;
           string txxy= this.comboBox2.Text;
           string kflx = this.comboBox1.SelectedValue.ToString();
            /////////////////////////设置主机信息
            int RecordM = int.Parse(this.numericUpDown5.Value.ToString());
            int WarningM = int.Parse(this.numericUpDown6.Value.ToString());
            int PhoneNo = int.Parse(this.textBox5.Text.ToString());
            int State = 1;
            ////////////////////////////////
            if (name!=null&&!"".Equals(name)&&cdnum!=null&& cktype!=null&& txxy!=null && kflx != null) {
                mh = new bean.manageHose();
                bool istrue = false;
                mh.hostName = name;
                mh.CommunicationType = txxy;
                mh.portNumber = cdnum;
                mh.storeType = cktype;
                mh.measureCode = this.textBox2.Text;
                mh.houseType = kflx;
                /////////////////////////设置主机信息
                mh.RecordM = RecordM;
                mh.WarningM = WarningM;
                mh.PhoneNo = PhoneNo;
                mh.State = State;
                ////////////////////////////////
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
                else if (this.radioButton3.Checked)
                {
                    mh.hostAddress = "";
                    mh.serialPort = "";
                    mh.tcp_ip_Port = "";
                    mh.networkType = "YUN";
                }else{
                    MessageBox.Show("请选择通讯方式");
                    return;
                }
                istrue = mhs.updateManageHost(mh);
                if (istrue)
                {
                    service.deviceInformationService ds = new service.deviceInformationService();
                    bool isfalse = ds.updateDeviceInformation(mh);
                    if (isfalse)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }
            else {
                MessageBox.Show("请输入主机名称！");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }
        string name0 = "";
        private void updateManageHost_Load(object sender, EventArgs e)
        {
            name0 = this.textBox1.Text;
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
            string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
            //this.comboBox4.Items.Clear();
            if (ArryPort.Length > 0)
            {
                for (int i = 0; i < ArryPort.Length; i++)
                {
                    this.comboBox4.Items.Add(ArryPort[i]);
                }
            }
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
            if (this.comboBox2.SelectedItem.ToString() == "串口通讯协议")
            {
                this.radioButton1.Checked = true;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;
                this.radioButton1.Enabled = true;
                this.radioButton2.Enabled = false;
                this.radioButton3.Enabled = false;
            }
            else if (this.comboBox2.SelectedItem.ToString() == "TCP协议")
            {
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = true;
                this.radioButton3.Checked = false;
                this.radioButton1.Enabled = false;
                this.radioButton2.Enabled = true;
                this.radioButton3.Enabled = false;
            }
            else
            {
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = true;
                this.radioButton1.Enabled = false;
                this.radioButton2.Enabled = false;
                this.radioButton3.Enabled = true;
            }
        }
    }
}
