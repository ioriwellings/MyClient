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
           string address= this.numericUpDown2.Value.ToString();
           string txxy= this.comboBox2.SelectedItem.ToString();
           string kflx = this.comboBox1.SelectedValue.ToString();
            if (name!=null&&!"".Equals(name)&&cdnum!=null&& cktype!=null&& address!=null&& txxy!=null && kflx != null) {
                mh = new bean.manageHose();
                service.manageHostService mhs = new service.manageHostService();
                bool istrue = false;
                //if (!"LBCC-16".Equals(txxy))
                //{
                //    mh.hostName = name;
                //    mh.hostAddress = address;
                //    mh.CommunicationType = txxy;
                //    mh.portNumber = cdnum;
                //    mh.serialPort = txport;
                //    mh.storeType = cktype;
                //    mh.measureCode = this.textBox2.Text;
                //}
                //else {
                //    mh.hostName = name;
                //    mh.CommunicationType = txxy;
                //    mh.portNumber = cdnum;
                //    mh.storeType = cktype;
                //    mh.measureCode = this.textBox2.Text;
                //}
                mh.hostName = name;
                mh.CommunicationType = txxy;
                mh.portNumber = cdnum;
                mh.storeType = cktype;
                mh.measureCode = this.textBox2.Text;
                mh.houseType = kflx;
                if (this.checkBox1.Checked)
                {
                    mh.hostAddress = this.numericUpDown2.Value.ToString();
                }
               istrue = mhs.updateManageHost(mh);
                this.DialogResult = DialogResult.OK;
                //if (istrue)
                //{
                //    service.deviceInformationService ds = new service.deviceInformationService();
                //    ds.addDeviceInformation(mh);
                //    this.DialogResult = DialogResult.OK;
                //}
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
                //this.comboBox3.SelectedIndex = Int32.Parse(hcode)-1;
                this.comboBox1.Text = hcode;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.numericUpDown2.Enabled = true;
            }
            if (!this.checkBox1.Checked)
            {
                this.numericUpDown2.Enabled = false;
            }
        }
    }
}
