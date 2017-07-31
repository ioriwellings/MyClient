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
    public partial class houseManage : Form
    {
        service.houseTypeService hts = new service.houseTypeService();
        public houseManage()
        {
            InitializeComponent();
        }

        private void houseManage_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/insert.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/delete.png");
            this.button3.BackgroundImage = Image.FromFile(@str + "/images/update.png");
            this.button4.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            queryhouseManage();
        }

        private void queryhouseManage()
        {
            DataTable dt = hts.queryhouseType();
            if (dt != null)
            {
                this.dataGridView1.DataSource = dt;
                //this.dataGridView1.Columns[0].HeaderCell.Value = "库房类型编号";
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].HeaderCell.Value = "库房名称";
                this.dataGridView1.Columns[2].HeaderCell.Value = "库房温度上限℃";
                this.dataGridView1.Columns[3].HeaderCell.Value = "库房温度下限℃";
                this.dataGridView1.Columns[4].HeaderCell.Value = "库房湿度上限%RH";
                this.dataGridView1.Columns[5].HeaderCell.Value = "库房湿度下限%RH";
                this.dataGridView1.Columns[6].HeaderCell.Value = "库房平面图名称";
                this.dataGridView1.Columns[7].Visible = false;
                this.dataGridView1.Columns[1].Width = 150;
                this.dataGridView1.Columns[2].Width = 120;
                this.dataGridView1.Columns[3].Width = 120;
                this.dataGridView1.Columns[4].Width = 120;
                this.dataGridView1.Columns[5].Width = 120;
                this.dataGridView1.Columns[6].Width = 180;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.Columns[4].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[5].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[2].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[3].DefaultCellStyle.Format = "0.0";
                //for (int count = 0; (count <= (this.dataGridView1.Rows.Count - 2)); count++) 
                //{
                //    this.dataGridView1.Rows[count].HeaderCell.Value = String.Format("{0}", count + 1);
                //}
                this.dataGridView1.AllowUserToAddRows = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult rr = MessageBox.Show("确定要删除当前数据吗？", "确定要删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {
                string code = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                if (code != null && !"".Equals(code) && !"1".Equals(code))
                {
                    bool bl = hts.deleteHouseTypeById(code);
                    if (bl)
                    {   //删除库房数据的同时修改仪表表中的house_code
                        service.deviceInformationService dis = new service.deviceInformationService();
                        dis.updateIformationByHouseCode(code);
                        //刷新页面
                        queryhouseManage();
                        MessageBox.Show("删除成功！");
                    }
                }
                else {
                    MessageBox.Show("当前库房信息不能被删除，只能修改库房信息，因为软件系统使用中！");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateHouseManage uhm = new updateHouseManage();
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string name = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string t_high = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string t_low = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string h_high = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            string h_low = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            string isUsed = this.dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            if (id != null && !"".Equals(id))
            {
                uhm.textBox2.Text = id;
            }
            if (name != null && !"".Equals(name))
            {
                uhm.textBox1.Text = name;
            }
           
            if (t_high != null && !"".Equals(t_high))
            {
                uhm.numericUpDown1.Value = (decimal)Convert.ToDouble(t_high);
            }
            
            if (t_low != null && !"".Equals(t_low))
            {
                uhm.numericUpDown2.Value = (decimal)Convert.ToDouble(t_low);
            }
            
            if (h_high != null && !"".Equals(h_high))
            {
                uhm.numericUpDown3.Value = (decimal)Convert.ToDouble(h_high);
            }
           
            if (h_low != null && !"".Equals(h_low))
            {
                uhm.numericUpDown4.Value = (decimal)Convert.ToDouble(h_low);
            }
            if (isUsed != null&&"1".Equals(isUsed)) {
                uhm.checkBox2.Checked = true;
            }
            DialogResult rr = MessageBox.Show("确定要修改当前库房设置吗？", "确定要修改库房提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1) {
                
                if (uhm.ShowDialog() == DialogResult.OK)
                {
                    //刷新库房页面
                    queryhouseManage();
                    MessageBox.Show("库房信息修改成功！");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            insertHouseManage hm = new insertHouseManage();
            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                queryhouseManage();
                MessageBox.Show("库房添加成功！");
               
            }
        }
    }
}
