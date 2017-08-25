using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class measurePointSet : Form
    {
        public event EventHandler RefreshEvent;
        public measurePointSet()
        {
            InitializeComponent();
        }

        private void measurePointSet_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/shezhi.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            this.button3.BackgroundImage = Image.FromFile(@str + "/images/insert.png");
            this.button4.BackgroundImage = Image.FromFile(@str + "/images/delete.png");
            checkPointInfo();
        }
        private void checkPointInfo() {
            service.deviceInformationService dis = new service.deviceInformationService();
            int flag = 0;
            DataTable dt = dis.checkPointInfo(flag);
            if (dt != null)
            {
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].HeaderCell.Value = "管理主机编号";
                this.dataGridView1.Columns[2].HeaderCell.Value = "测点(仪表)编号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "测点(仪表)名称";
                this.dataGridView1.Columns[4].Visible = false;
                this.dataGridView1.Columns[5].HeaderCell.Value = "通信协议";
                this.dataGridView1.Columns[6].Visible = false;
                this.dataGridView1.Columns[7].Visible = false;
                this.dataGridView1.Columns[8].Visible = false;
                this.dataGridView1.Columns[9].Visible = false;
                this.dataGridView1.Columns[10].Visible = false;
                this.dataGridView1.Columns[11].HeaderCell.Value = "库房类型名称";
                this.dataGridView1.Columns[12].Visible = false;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.Columns[1].Width = 150;
                this.dataGridView1.Columns[2].Width = 130;
                this.dataGridView1.Columns[3].Width = 150;
                this.dataGridView1.Columns[5].Width = 220;
                this.dataGridView1.AllowUserToAddRows = false;

            }
            else {
                this.dataGridView1.DataSource = null;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            measurePointSetPropert mpsp = new measurePointSetPropert();
            string code = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string meter = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string name = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string t_high = this.dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            string t_low = this.dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            string h_high = this.dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            string h_low = this.dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
            string housecode = this.dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
            string pflag = this.dataGridView1.SelectedRows[0].Cells[12].Value.ToString();
            if (pflag == "1")
            {
                mpsp.checkBox2.Checked = true;
            }
            else {
                mpsp.checkBox1.Checked = true;
            }
            if (name != null && !"".Equals(name))
            {
                mpsp.textBox1.Text = name;
            }
            else {
                mpsp.textBox1.Text = "";
            }
            if (t_high != null && !"".Equals(t_high))
            {
                mpsp.numericUpDown1.Value = (decimal)Convert.ToDouble(t_high);
            }
            else {
                mpsp.numericUpDown1.Value = 0;
            }
            if (t_low != null && !"".Equals(t_low))
            {
                mpsp.numericUpDown2.Value = (decimal)Convert.ToDouble(t_low);
            }
            else
            {
                mpsp.numericUpDown2.Value = 0;
            }
            if (h_high != null && !"".Equals(h_high))
            {
                mpsp.numericUpDown3.Value = (decimal)Convert.ToDouble(h_high);
            }
            else
            {
                mpsp.numericUpDown3.Value = 0;
            }
            if (h_low != null && !"".Equals(h_low))
            {
                mpsp.numericUpDown4.Value = (decimal)Convert.ToDouble(h_low);
            }
            else
            {
                mpsp.numericUpDown4.Value = 0;
            }
            if (housecode!=null&&!"".Equals(housecode)) {
                mpsp.textBox2.Text=housecode;
            }
            mpsp.textBox5.Text = code;
            mpsp.textBox6.Text =meter;
            if (mpsp.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("测点(仪表)参数修改成功！");
                checkPointInfo();
                //刷新首页
                if (this.RefreshEvent != null)
                {
                    RefreshEvent(null, null);
                }
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            measurePointInsert mp = new measurePointInsert();
            if (mp.ShowDialog()== DialogResult.OK) {
                checkPointInfo();
                MessageBox.Show("测点(仪表)信息添加成功！");
                //刷新首页
                if (this.RefreshEvent != null)
                {
                    RefreshEvent(1, null);
                }
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult rr = MessageBox.Show("确定要删除测点(仪表)数据吗？", "确定要删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1) {
            string code = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string meter = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            if (code != null && !"".Equals(code) && meter != null && !"".Equals(meter))
            {
                bean.deviceInformation di = new bean.deviceInformation();
                di.measureCode = code;
                di.meterNo = meter;
                service.deviceInformationService dis = new service.deviceInformationService();
                bool bl = dis.deletetDeviceInformation(di);
                if (bl)
                  { //删除仪表数据的同时修改管理主机的仪表数量
                    service.manageHostService mhs = new service.manageHostService();
                    bean.manageHose mh = new bean.manageHose();
                    mh.measureCode = di.measureCode;
                    mhs.updateManageHostCdNum(mh);
                    //刷新仪表页面
                    checkPointInfo();
                    MessageBox.Show("删除成功！");
                    //刷新首页
                    if (this.RefreshEvent != null)
                    {
                        RefreshEvent(1, null);
                    }   
                }
            }
          }
        }
    }
}
