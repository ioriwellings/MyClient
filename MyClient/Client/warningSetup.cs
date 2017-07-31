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
    public partial class warningSetup : Form
    {
        service.deviceInformationService dis = new service.deviceInformationService();
        public warningSetup()
        {
            InitializeComponent();
        }

        private void warningSetup_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");

            selectdeviceinfo();
        }
        private void selectdeviceinfo() {
            DataTable dt = dis.selectBydeviceInfo("", "");
            if (dt.Rows.Count > 0)
            {
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].Visible = false;
                this.dataGridView1.Columns[2].HeaderCell.Value = "测点名称";
                this.dataGridView1.Columns[3].HeaderCell.Value = "温度上限";
                this.dataGridView1.Columns[4].HeaderCell.Value = "温度下限";
                this.dataGridView1.Columns[5].HeaderCell.Value = "湿度上限";
                this.dataGridView1.Columns[6].HeaderCell.Value = "湿度下限";
                this.dataGridView1.Columns[7].Visible = false;
                this.dataGridView1.Columns[8].Visible = false;
                this.dataGridView1.Columns[2].Width = 160;
                this.dataGridView1.Columns[3].Width = 80;
                this.dataGridView1.Columns[4].Width = 80;
                this.dataGridView1.Columns[5].Width = 80;
                this.dataGridView1.Columns[6].Width = 80;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要统一设置所有温湿度上下限吗？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string t_high = this.numericUpDown4.Value.ToString();
                string t_low = this.numericUpDown2.Value.ToString();
                string h_high = this.numericUpDown1.Value.ToString();
                string h_low = this.numericUpDown5.Value.ToString();
                bean.deviceInformation di = new bean.deviceInformation();
                di.t_high = float.Parse(t_high);
                di.t_low = float.Parse(t_low);
                di.h_high = float.Parse(h_high);
                di.h_low = float.Parse(h_low);
                bool bl=  dis.updateAllIformation(di);
                if (bl) {
                    selectdeviceinfo();
                    MessageBox.Show("测点报警信息设置成功！");
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
