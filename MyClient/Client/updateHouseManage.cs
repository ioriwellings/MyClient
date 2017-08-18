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
    public partial class updateHouseManage : Form
    {
        public updateHouseManage()
        {
            InitializeComponent();
        }

        private void updateHouseManage_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/update.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
        }

        private void button1_Click(object sender, EventArgs e) { 
            string name = this.textBox1.Text;
            string t_high = this.numericUpDown1.Value.ToString();
            string t_low = this.numericUpDown2.Value.ToString();
            string h_high = this.numericUpDown3.Value.ToString();
            string h_low = this.numericUpDown4.Value.ToString();  
            string id = this.textBox2.Text.ToString();
            if (name != null&&!"".Equals(name)&&id!=null&&!"".Equals(id))
            {
                bean.houseInfo di = new bean.houseInfo();
                di.name = name;    
                di.t_high = float.Parse(t_high);
                di.t_low = float.Parse(t_low);
                di.h_high = float.Parse(h_high);
                di.h_low = float.Parse(h_low);
                //di.id = Int32.Parse(id);
                di.id = id;
                if (this.checkBox2.Checked)
                {
                    di.isUsed = 1;
                }
                else {
                    di.isUsed = 0;
                }
                service.houseTypeService his = new service.houseTypeService();
                bool isok = his.updateHouseInfoById(di);
                if (isok)
                {
                    service.deviceInformationService dis = new service.deviceInformationService();

                    if (this.checkBox1.Checked)
                    {
                        bool bl = dis.updateWsdByHouseCode(di);
                        if (bl)
                        {
                            if (di.isUsed == 1)
                            {
                                MessageBox.Show(di.name + "  库房下所有的测点(仪表)都已修改为空库测点！");
                            }
                            MessageBox.Show(di.name + "  库房下所有的测点(仪表)温湿度上下限已同步修改成功！");

                        }
                    }
                    else {
                        bool bl = dis.updateWsdByHouseKong(di);
                        if (bl) {
                            if (di.isUsed == 1)
                            {
                                MessageBox.Show(di.name + "  库房下所有的测点(仪表)都已修改为空库测点！");
                            }
                            else {
                                MessageBox.Show(di.name + "  库房下所有的测点(仪表)都已取消空库设定！");

                            }
                        }
                    }
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
