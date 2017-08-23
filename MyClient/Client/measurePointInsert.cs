using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class measurePointInsert : Form
    {
        service.deviceInformationService dis = new service.deviceInformationService();
        public measurePointInsert()
        {
            InitializeComponent();
        }

        private void measurePointInsert_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/cancel.png");
            service.manageHostService mh = new service.manageHostService();
            DataTable dt1 = mh.queryManageHost();
            if (dt1.Rows.Count>0) {
                this.comboBox1.DataSource = dt1;//绑定数据源
                this.comboBox1.DisplayMember = "hostName";//显示给用户的数据集表项
                this.comboBox1.ValueMember = "measureCode";//操作时获取的值 
                
            }
            service.houseTypeService hts = new service.houseTypeService();
            this.comboBox3.DataSource = hts.queryhouseType();//绑定数据源
            this.comboBox3.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox3.ValueMember = "id";//操作时获取的值 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string cdname = this.textBox1.Text;
            string cdnum = this.textBox2.Text;
            string mcode = this.comboBox1.SelectedValue.ToString();
            string type = this.comboBox3.SelectedValue.ToString();
            string t1 = this.numericUpDown1.Value.ToString();
            string t2 = this.numericUpDown2.Value.ToString();
            string h1 = this.numericUpDown3.Value.ToString();
            string h2 = this.numericUpDown4.Value.ToString();

            if (cdname != null && !"".Equals(cdname) && cdnum != null && !"".Equals(cdnum) && mcode != null && !"".Equals(mcode))
            {
                if (float.Parse(t1) > float.Parse(t2))
                {
                    if (cdnum.Length == 2)
                    {
                        bean.deviceInformation di = new bean.deviceInformation();
                        di.h_high = float.Parse(h1);
                        di.h_low = float.Parse(h2);
                        di.t_high = float.Parse(t1);
                        di.t_low = float.Parse(t2);
                        di.terminalname = cdname;
                        di.measureCode = mcode;
                        di.meterNo = cdnum;
                        di.housecode = type;


                        bool bl = dis.queryDeviceBycode(di);
                        if (bl)
                        {
                            bool isok = dis.insertDeviceInformation(di);
                            if (isok)
                            {//测点添加成功的同时   要修改管理主机表中的测点个数
                                bean.manageHose bm = new bean.manageHose();
                                bm.measureCode = di.measureCode;
                                service.manageHostService mh = new service.manageHostService();
                                mh.updateManageHostCdNum(bm);

                                this.DialogResult = DialogResult.OK;
                                this.Close();

                            }
                        }
                        else
                        {
                            MessageBox.Show("测点编号已存在，请重新输入！");
                        }

                    }
                    else
                    {
                        MessageBox.Show("请填写格式正确的测点编号！");
                    }
                }
                else {
                    MessageBox.Show("温度上限应大于温度下限，请重新输入！");
                }
            }
            else
            {
                MessageBox.Show("信息输入不完整，请重新输入！");
            }
        }
        
    }
}
