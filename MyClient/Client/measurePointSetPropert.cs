using System;
using System.Drawing;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class measurePointSetPropert : Form
    {
        public measurePointSetPropert()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string code = this.textBox5.Text;
            string meter = this.textBox6.Text;
            string name=this.textBox1.Text;
            string t_high = this.numericUpDown1.Value.ToString();
            string t_low = this.numericUpDown2.Value.ToString();
            string h_high = this.numericUpDown3.Value.ToString();
            string h_low = this.numericUpDown4.Value.ToString();
            string type = this.comboBox3.SelectedValue.ToString();
            if (name != null && double.Parse(t_high) > double.Parse(t_low))
            {
                bean.deviceInformation di = new bean.deviceInformation();
                di.measureCode = code;
                di.meterNo = meter;
                di.terminalname = name;
                di.t_high = float.Parse(t_high);
                di.t_low = float.Parse(t_low);
                di.h_high = float.Parse(h_high);
                di.h_low = float.Parse(h_low);
                di.housecode = type;
                if (this.checkBox2.Checked) {
                    di.powerflag = Int32.Parse(this.checkBox2.Tag.ToString());
                }
                if (this.checkBox1.Checked)
                {
                    di.powerflag = Int32.Parse(this.checkBox1.Tag.ToString());
                }
                service.deviceInformationService dis = new service.deviceInformationService();
                bool isok = dis.updateIformation(di);
                if (isok)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            else {
                MessageBox.Show("测点名称不能为空，温度上限应大于温度下限！");
            }
        }

        private void measurePointSetPropert_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/cancel.png");
            service.houseTypeService hts = new service.houseTypeService();
            this.comboBox3.DataSource = hts.queryhouseType();//绑定数据源
            this.comboBox3.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox3.ValueMember = "id";//操作时获取的值 
            string hcode=this.textBox2.Text;
            if (hcode != null && !"".Equals(hcode))
            {
            this.comboBox3.Text =hcode;
            }      
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            this.checkBox2.Checked = false;
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            this.checkBox1.Checked = false;
        }
    }
}
