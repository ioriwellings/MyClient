using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;


namespace LBKJClient
{
    public partial class warningForm2 : Form
    {
        IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
        DataTable dt = new DataTable();
        DataTable dtcount = new DataTable();
        service.warningCheckService wcs = new service.warningCheckService();
        public string time1 = null;
        public string time2 = null;
        public string time22 { get; set; }
        public string measureNolist = null;
        public string cdlist = null;
        public int pageNo = 0;
        public warningForm2()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void warningForm_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            pagerControl1.OnPageChanged += new EventHandler(pagerControl1_OnPageChanged);
           


            //报警数据表lb_warning_data查询出所选时段所选测点的报警数据dtcount
            //  dtcount = wcs.warningcheckliutengfei(time1, time2, cd);
            LoadData();


        }
        private void pagerControl1_OnPageChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void LoadData()
        {
            this.dataGridView1.DataSource = null;
            service.changGuicheckService cgs = new service.changGuicheckService();
            if (pageNo == 0)
            {
                //dt = wcs.warningcheckfenyeliutengfei(time1, time2, cd, pagerControl1.PageIndex, pagerControl1.PageSize);
                //dtcount = cgs.changguicheckliutengfei(time1,
                //  time2, cdlist, measureNolist).Tables[0];

            }
            else
            {
                //dtcount = cgs.changguicheckliutengfeiGLZJ(time1,
                //time2, cdlist, measureNolist).Tables[0];


            }
            pagerControl1.DrawControl(dtcount.Rows.Count);//数据总条数

            dt = wcs.warningcheckfenyeliutengfei(time1, time2, "", pagerControl1.PageIndex, pagerControl1.PageSize);
            if (dt != null && dt.Rows.Count > 0)
            {
                int num = dt.Rows.Count;
                double wd;
                double sd;
                double wdsx;
                double wdxx;
                double sdsx;
                double sdxx;
                string warningTime;
                int powerwarn = 0;

                DataView dv = dt.DefaultView;//虚拟视图
                dv.Sort = "devtime,measureCode,meterNo asc";
                DataTable dts = dv.ToTable(true);
                this.dataGridView1.DataSource = dts.DefaultView;

                this.dataGridView1.Columns[0].HeaderCell.Value = "数据采集时间";
                this.dataGridView1.Columns[1].HeaderCell.Value = "设备标识名称";
                this.dataGridView1.Columns[2].HeaderCell.Value = "管理主机编号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "仪表编号";
                this.dataGridView1.Columns[4].HeaderCell.Value = "温度";
                this.dataGridView1.Columns[5].HeaderCell.Value = "湿度";
                this.dataGridView1.Columns[6].HeaderCell.Value = "报警事件";
                this.dataGridView1.Columns[7].Visible = false;
                this.dataGridView1.Columns[8].Visible = false;
                this.dataGridView1.Columns[9].Visible = false;
                this.dataGridView1.Columns[10].Visible = false;
                this.dataGridView1.Columns[11].HeaderCell.Value = "报警处理";
                this.dataGridView1.Columns[12].HeaderCell.Value = "温度超标差值";
                this.dataGridView1.Columns[13].HeaderCell.Value = "湿度超标差值";
                this.dataGridView1.Columns[0].Width = 150;
                this.dataGridView1.Columns[1].Width = 170;
                this.dataGridView1.Columns[2].Width = 150;
                this.dataGridView1.Columns[3].Width = 100;
                this.dataGridView1.Columns[4].Width = 100;
                this.dataGridView1.Columns[5].Width = 100;
                this.dataGridView1.Columns[6].Width = 250;
                this.dataGridView1.Columns[11].Width = 120;
                this.dataGridView1.Columns[4].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[5].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[12].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[13].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;

                this.dataGridView1.AllowUserToAddRows = false;

                int row = this.dataGridView1.Rows.Count;//得到总行数 
                for (int i = 0; i < row; i++)
                {
                    string warningEvent = "";
                    wd = double.Parse(this.dataGridView1.Rows[i].Cells[4].Value.ToString());
                    sd = double.Parse(this.dataGridView1.Rows[i].Cells[5].Value.ToString());
                    string pw = this.dataGridView1.Rows[i].Cells[6].Value.ToString();
                    if (pw != null && !"".Equals(pw))
                    {
                        powerwarn = Int32.Parse(pw);
                        if (powerwarn == 1)
                        {
                            warningEvent = "断电报警 ";
                        }
                    }
                    wdsx = double.Parse(this.dataGridView1.Rows[i].Cells[7].Value.ToString());
                    wdxx = double.Parse(this.dataGridView1.Rows[i].Cells[8].Value.ToString());
                    sdsx = double.Parse(this.dataGridView1.Rows[i].Cells[9].Value.ToString());
                    sdxx = double.Parse(this.dataGridView1.Rows[i].Cells[10].Value.ToString());
                    warningTime = dt.Rows[i][11].ToString();

                    if (wd > wdsx)
                    {
                        this.dataGridView1.Rows[i].Cells[12].Value = Math.Round(wd - wdsx, 1).ToString("0.0");
                        this.dataGridView1.Rows[i].Cells[4].Style.ForeColor = Color.Red;
                        warningEvent += "温度上限报警 ";
                    }
                    else if (wd < wdxx)
                    {
                        this.dataGridView1.Rows[i].Cells[12].Value = Math.Round(wd - wdxx, 1).ToString("0.0");
                        this.dataGridView1.Rows[i].Cells[4].Style.ForeColor = Color.Red;
                        warningEvent += "温度下限报警 ";
                    }
                    else
                    {
                        this.dataGridView1.Rows[i].Cells[12].Value = "0.0";
                    }

                    if (sd > sdsx)
                    {
                        this.dataGridView1.Rows[i].Cells[13].Value = Math.Round(sd - sdsx, 1).ToString("0.0");
                        this.dataGridView1.Rows[i].Cells[5].Style.ForeColor = Color.Red;
                        warningEvent += "湿度上限报警 ";
                    }
                    else if (sd < sdxx)
                    {
                        this.dataGridView1.Rows[i].Cells[13].Value = Math.Round(sd - sdxx, 1).ToString("0.0");
                        this.dataGridView1.Rows[i].Cells[5].Style.ForeColor = Color.Red;
                        warningEvent += "湿度下限报警 ";
                    }
                    else
                    {
                        dt.Rows[i][13] = "0.0";
                    }

                    if (warningEvent == "")
                    {
                        warningEvent = "解除报警 ";
                        this.dataGridView1.Rows[i].Cells[11].Value = "";
                    }
                    else
                    {
                        this.dataGridView1.Rows[i].Cells[6].Style.ForeColor = Color.Red;
                    }
                    this.dataGridView1.Rows[i].Cells[6].Value = warningEvent;
                }
                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {
                    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            else
            {
                MessageBox.Show("当前测点无数据，请重新查询！");
            };
        }

    }
}