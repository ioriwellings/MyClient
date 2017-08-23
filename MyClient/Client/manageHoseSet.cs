using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class manageHoseSet : Form
    {
        service.manageHostService mhs = new service.manageHostService();
        public event EventHandler RefreshEvent;
        public string comCode = "";
        public manageHoseSet()
        {
            InitializeComponent();
        }
        private void manageHoseSet_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/insert.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/delete.png");
            this.button3.BackgroundImage = Image.FromFile(@str + "/images/update.png");
            this.button4.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            this.button5.BackgroundImage = Image.FromFile(@str + "/images/dqjl.png");
            querymanageHose();//查询管理主机信息
        }
        private void button1_Click(object sender, EventArgs e)
        {
            addManageHost amh = new addManageHost();
            if (amh.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("管理主机添加成功！");
                querymanageHose();//查询管理主机信息
                //刷新首页    
                if (this.RefreshEvent != null)
                {
                    RefreshEvent(null, null);
                }
            }
        }
        private void querymanageHose()
        {
            DataTable dt = mhs.queryManageHost();
            if (dt != null && dt.Rows.Count > 0)
            {
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].HeaderCell.Value = "管理主机名称";
                this.dataGridView1.Columns[2].Visible = false;
                this.dataGridView1.Columns[3].HeaderCell.Value = "通信协议";
                this.dataGridView1.Columns[4].Visible = false;

                this.dataGridView1.Columns[5].Visible = false;
                this.dataGridView1.Columns[6].Visible = false;

                this.dataGridView1.Columns[7].HeaderCell.Value = "测点数量";
                this.dataGridView1.Columns[8].HeaderCell.Value = "存储类型";
                this.dataGridView1.Columns[9].HeaderCell.Value = "管理主机编号";
                this.dataGridView1.Columns[10].Visible = false;
                this.dataGridView1.Columns[11].Visible = false;

                this.dataGridView1.Columns[12].Visible = false;

                this.dataGridView1.Columns[13].HeaderCell.Value = "所属库房";
                this.dataGridView1.Columns[1].Width = 200;
                this.dataGridView1.Columns[3].Width = 200;
                this.dataGridView1.Columns[7].Width = 80;
                this.dataGridView1.Columns[9].Width = 200;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                //for (int count = 0; (count <= (this.dataGridView1.Rows.Count - 2)); count++) 
                //{
                //    this.dataGridView1.Rows[count].HeaderCell.Value = String.Format("{0}", count + 1);
                //}
                this.dataGridView1.AllowUserToAddRows = false;
            }
            else {
                this.dataGridView1.DataSource = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool istrue = false;
            string id=this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string code = this.dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            if (id != null)
            {
                DialogResult rr=MessageBox.Show("确定要删除管理主机数据吗？", "管理主机数据删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                int tt = (int)rr;
                if (tt == 1)
                {//要删除管理主机的数据  同时删除和管理主机有联系的仪表数据         
                    istrue = mhs.deleteManageHost(id,code);
                    if (istrue)
                    {
                        querymanageHose();
                        MessageBox.Show("删除成功！");
                        //刷新首页
                        if (this.RefreshEvent != null)
                        {
                            RefreshEvent(null, null);
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateManageHost umh = new updateManageHost();
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string name= this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string address = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string txxy = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string txport = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            string cdnum = this.dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            string type = this.dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            string code = this.dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            string kflx = this.dataGridView1.SelectedRows[0].Cells[13].Value.ToString();
            string networkType = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString(); 
            string tcp_ip_Port = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            string [] result = tcp_ip_Port.Split(':');
            
            umh.textBox1.Text = name;
            umh.numericUpDown1.Value = Convert.ToInt32(cdnum);
            umh.comboBox2.SelectedItem = txxy;
            umh.comboBox3.SelectedItem = type;
            umh.textBox2.Text = code;
            umh.textBox3.Text = kflx;
            if (address!=null&&!"".Equals(address)) {
                if (networkType == "串口") { umh.radioButton1.Checked = true; } else if (networkType == "tcp") { umh.radioButton2.Checked = true; };
                if (umh.radioButton1.Checked)
                {
                    umh.numericUpDown2.Value = Convert.ToInt32(address);
                    umh.comboBox4.Text = txport;
                    umh.numericUpDown2.Enabled = true;
                    umh.comboBox4.Enabled = true;
                } else if (umh.radioButton2.Checked) {
                    umh.numericUpDown3.Value = Convert.ToInt32(address);
                    umh.textBox4.Text = result[0];
                    umh.numericUpDown4.Value = decimal.Parse(result[1]);
                    umh.numericUpDown3.Enabled = true;
                    umh.textBox4.Enabled = true;
                    umh.numericUpDown4.Enabled = true;
                }

            }
            if (umh.ShowDialog() == DialogResult.OK)
            {
                querymanageHose();
                MessageBox.Show("修改成功！");
                //刷新首页
                if (this.RefreshEvent != null)
                {
                    RefreshEvent(null, null);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string name = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string address = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string code = this.dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            if (address!=""&&!"".Equals(address)&&Int32.Parse(address) > 0)
            {
                readHistoryData rhd = new readHistoryData();
                rhd.address = address;
                rhd.measureCode = code;
                rhd.comCode = comCode;
                rhd.label1.Text = "读取【"+name+"】的历史记录";
                int ww = (rhd.Width - rhd.label1.Width) / 2;
                rhd.label1.Location = new Point(ww-10,22);
                rhd.ShowDialog();
            }
            else {
                MessageBox.Show("不是通过串口通信的管理主机！");
            }
        }
    }
}
