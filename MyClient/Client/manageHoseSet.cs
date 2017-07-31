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
                this.dataGridView1.Columns[5].HeaderCell.Value = "测点数量";
                this.dataGridView1.Columns[6].HeaderCell.Value = "存储类型";
                this.dataGridView1.Columns[7].HeaderCell.Value = "管理主机编号";
                this.dataGridView1.Columns[8].Visible = false;
                this.dataGridView1.Columns[9].Visible = false;
                this.dataGridView1.Columns[10].HeaderCell.Value = "所属库房";
                this.dataGridView1.Columns[1].Width = 200;
                this.dataGridView1.Columns[3].Width = 200;
                this.dataGridView1.Columns[5].Width = 80;
                this.dataGridView1.Columns[7].Width = 200;
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
            string code = this.dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
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
            string cdnum = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            string type = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            string code = this.dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            string kflx = this.dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
            //if (!"LBCC-16".Equals(txxy)) {   
            //    umh.textBox1.Text = name;
            //    umh.numericUpDown1.Value = Convert.ToInt32(cdnum);
            //    umh.numericUpDown2.Value = Convert.ToInt32(address);
            //    umh.comboBox2.SelectedItem = txxy;
            //    umh.comboBox1.SelectedItem = txport;
            //    umh.comboBox3.SelectedItem = type;
            //    umh.textBox2.Text = code;
            //}
            //else
            //{
            //    umh.textBox1.Text = name;
            //    umh.numericUpDown1.Value = Convert.ToInt32(cdnum);
            //    umh.comboBox2.SelectedItem = txxy;
            //    umh.textBox2.Text = code;
            //    umh.label7.Visible = true;
            //    umh.textBox2.Visible = true;
            //    umh.radioButton1.Visible = false;
            //    umh.label5.Visible = false;
            //    umh.comboBox1.Visible = false;
            //    umh.label3.Visible = false;
            //    umh.numericUpDown2.Visible = false;
            //}
            umh.textBox1.Text = name;
            umh.numericUpDown1.Value = Convert.ToInt32(cdnum);
            umh.comboBox2.SelectedItem = txxy;
            umh.comboBox3.SelectedItem = type;
            umh.textBox2.Text = code;
            umh.textBox3.Text = kflx;
            if (address!=null&&!"".Equals(address)) {
                umh.checkBox1.Checked = true;
                umh.numericUpDown2.Value = Convert.ToInt32(address);
                umh.numericUpDown2.Enabled = true;
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
            string code = this.dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
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
