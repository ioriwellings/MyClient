using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;

namespace LBKJClient
{
    public partial class showReport : Form
    {

        public SerialPort port = new SerialPort();
        DataTable dt;
 


        public showReport()
        {
            InitializeComponent();
        }

        private void showReport_Load(object sender, EventArgs e)
        {
            service.showReportService lls = new service.showReportService();
            dt = lls.queryReport();
            if (dt.Rows.Count > 0)
            {
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].HeaderCell.Value = "时间";
                this.dataGridView1.Columns[2].HeaderCell.Value = "事件";
                this.dataGridView1.Columns[1].Width = 200;
                this.dataGridView1.Columns[2].Width = 287;

                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
            }
            else
            {
                MessageBox.Show("当前时间段无查询结果！");
            }

        }

       
    }
}
