using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LBKJClient
{
    public partial class addManageHost : Form
    {
        bean.manageHose mh = null;
        private string path = @"config.xml";
        public addManageHost()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           string name = this.textBox1.Text;
           string cdnum= this.numericUpDown1.Value.ToString();
           string cktype = this.comboBox3.SelectedItem.ToString();
           string txxy= this.comboBox2.SelectedItem.ToString();
           string kflx = this.comboBox1.SelectedValue.ToString();

            if (name!=null&&!"".Equals(name)&&cdnum!=null&& cktype!=null&& txxy!=null&& kflx!=null) {
                mh = new bean.manageHose();
                service.manageHostService mhs = new service.manageHostService();
                bool istrue = false;
                mh.hostName = name;
                mh.CommunicationType = txxy;
                mh.portNumber = cdnum;
                mh.storeType = cktype;
                mh.houseType = kflx;

                if (this.radioButton1.Checked)
                {
                    mh.hostAddress = this.numericUpDown2.Value.ToString();
                    mh.serialPort = this.comboBox5.SelectedItem.ToString();
                    mh.networkType = "串口";
                }
                else if (this.radioButton2.Checked)
                {
                    mh.hostAddress = this.numericUpDown5.Value.ToString();
                    mh.tcp_ip_Port = this.textBox1.Text + ":" + this.numericUpDown3.Value.ToString();
                    mh.networkType = "tcp";
                }

                if (this.textBox2.Text != null && !"".Equals(this.textBox2.Text))
                    {
                        mh.measureCode = this.textBox2.Text;
                        istrue = mhs.addManageHost(mh);
                    }
                    else
                    {
                        MessageBox.Show("管理主机编号为空，请输入...");
                    }
                if (istrue)
                    {
                    service.deviceInformationService ds = new service.deviceInformationService();
                    bool isfalse= ds.addDeviceInformation(mh);
                    if (isfalse)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }
            else {
                MessageBox.Show("信息填写不完整，请重新填写！");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public string RandCode(int n)
        {

            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < n; i++)
            {

                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.comboBox2.SelectedItem.ToString() == "LBCC-16")
            //{
            //    this.label7.Visible = true;
            //    this.textBox2.Visible = true;
            //    this.radioButton1.Visible = false;
            //    this.label5.Visible = false;
            //    this.comboBox1.Visible = false;
            //    this.label3.Visible = false;
            //    this.numericUpDown2.Visible = false;
            //}
            //else {
            //    this.label7.Visible = false;
            //    this.textBox2.Visible = false;
            //    this.radioButton1.Visible = true;
            //    this.label5.Visible = true;
            //    this.comboBox1.Visible = true;
            //    this.label3.Visible = true;
            //    this.numericUpDown2.Visible = true;
            //}
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void addManageHost_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/cancel.png");

            service.houseTypeService hts = new service.houseTypeService();
            this.comboBox1.DataSource = hts.queryhouseType();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "id";//操作时获取的值 

            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;

            string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
            this.comboBox5.Items.Clear();
            if (ArryPort.Length > 0)
            {
                for (int i = 0; i < ArryPort.Length; i++)
                {
                    this.comboBox5.Items.Add(ArryPort[i]);
                }
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked) {
                this.numericUpDown2.Enabled = true;
            }
            if (!this.radioButton1.Checked)
            {
                this.numericUpDown2.Enabled = false;
            }
        }

        private void saveToXmlsStoptime(string communication)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/communicationType");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("communicationType");
                n.InnerText = communication;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = communication;
            }
            xmlDoc.Save(path);
        }
    }
}
