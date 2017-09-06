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
    public partial class warningForm : Form
    {
        string TarStr = "yyyyMMddHHmmss";
        IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
        DataTable dt= new DataTable();
        DataTable dtcount = new DataTable();
        service.warningCheckService wcs = new service.warningCheckService();
        string time1 = null;
        string time2 = null;
        string cd = null;
        public warningForm()
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
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = System.Drawing.Image.FromFile(@str + "/images/check.png");
            this.button2.BackgroundImage = System.Drawing.Image.FromFile(@str + "/images/bjcl.png");
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            //查询所有测点或主机编号信息
            service.changGuicheckService cgs = new service.changGuicheckService();
            DataTable dt1 = cgs.checkcedian(null).Tables[0];
            this.comboBox1.DataSource = dt1;//绑定数据源

            DataRow newRow;
            newRow = dt1.NewRow();
            newRow["measureMeterCode"] = "0";
            newRow["terminalname"] = "全部监测点";
            dt1.Rows.Add(newRow);
            newRow.AcceptChanges();
            int DeptIndex = this.comboBox1.FindString("全部监测点");
            if (DeptIndex != -1)
            this.comboBox1.SelectedIndex = DeptIndex;
            this.comboBox1.DisplayMember = "terminalname";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "measureMeterCode";//操作时获取的值
            this.comboBox1.Text="--请选择--";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            time1 = this.dateTimePicker1.Text.ToString();
            time2 = this.dateTimePicker2.Text.ToString();
            cd = this.comboBox1.SelectedValue.ToString();

            if (cd != "" && !"".Equals(cd))
            {
                this.dataGridView1.DataSource = null;
                pagerControl1.OnPageChanged += new EventHandler(pagerControl1_OnPageChanged);
                dtcount = wcs.warningcheck(time1, time2, cd);
                LoadData();

            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            if (dtcount!=null&&dtcount.Rows.Count > 0)
            {
                double wd;
                double sd;
                double wdsx;
                double wdxx;
                double sdsx;
                double sdxx;
                string warningTime;
                int powerwarn = 0;
                int row = this.dtcount.Rows.Count;//得到总行数 
                for (int i = 0; i < row; i++)
                {
                    string warningEvent = "";
                    wd = double.Parse(this.dtcount.Rows[i][4].ToString());
                    sd = double.Parse(this.dtcount.Rows[i][5].ToString());
                    string pw = this.dtcount.Rows[i][6].ToString();
                    if (pw != null && !"".Equals(pw))
                    {
                        powerwarn = Int32.Parse(pw);
                        if (powerwarn == 1)
                        {
                            warningEvent = "断电报警 ";
                        }
                    }
                    wdsx = double.Parse(this.dtcount.Rows[i][7].ToString());
                    wdxx = double.Parse(this.dtcount.Rows[i][8].ToString());
                    sdsx = double.Parse(this.dtcount.Rows[i][9].ToString());
                    sdxx = double.Parse(this.dtcount.Rows[i][10].ToString());
                    warningTime = dtcount.Rows[i][11].ToString();
                    //if (warningTime != null && !"".Equals(warningTime))
                    //{
                    //    this.dataGridView1.Rows[i].Cells[11].Value = "已处理";
                    //}
                    //else
                    //{
                    //    this.dataGridView1.Rows[i].Cells[11].Value = "未处理";
                    //}
                    if (wd > wdsx)
                    {
                        this.dtcount.Rows[i][12] = Math.Round(wd - wdsx, 1).ToString("0.0");
                        warningEvent += "温度上限报警 ";
                    }
                    else if (wd < wdxx)
                    {
                        this.dtcount.Rows[i][12] = Math.Round(wd - wdxx, 1).ToString("0.0");
                        warningEvent += "温度下限报警 ";
                    }
                    else
                    {
                        this.dtcount.Rows[i][12] = "0.0";
                    }

                    if (sd > sdsx)
                    {
                        this.dtcount.Rows[i][13] = Math.Round(sd - sdsx, 1).ToString("0.0");
                        warningEvent += "湿度上限报警 ";
                    }
                    else if (sd < sdxx)
                    {
                        this.dtcount.Rows[i][13] = Math.Round(sd - sdxx, 1).ToString("0.0");
                        warningEvent += "湿度下限报警 ";
                    }
                    else
                    {
                        dtcount.Rows[i][13] = "0.0";
                    }

                    if (warningEvent == "")
                    {
                        warningEvent = "解除报警 ";
                        this.dtcount.Rows[i][11] = "";
                    }
                    else
                    {

                    }
                    this.dtcount.Rows[i][6] = warningEvent;
                }
               
                    string localFilePath = String.Empty;
                    SaveFileDialog fileDialog = new SaveFileDialog();

                    fileDialog.InitialDirectory = "C://";

                    fileDialog.Filter = "txt files (*.db)|*.db|All files (*.*)|*.*";

                    //设置文件名称：
                    fileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "报警数据查询结果.pdf";

                    fileDialog.FilterIndex = 2;

                    fileDialog.RestoreDirectory = true;
                    if (fileDialog.ShowDialog() == DialogResult.OK)

                    {   //获得文件路径
                        localFilePath = fileDialog.FileName.ToString();
                        CreateTable(dtcount, localFilePath);
                        MessageBox.Show("恭喜，PDF文件生产成功!");
                    }
            }
            else
            {
                MessageBox.Show("无报警数据，请先查询报警数据后再生成PDF文件!");
            }
        }
        private void CreateTable(DataTable dts,string path)
        {
            //定义一个Document，并设置页面大小为A4，竖向 
            Document doc = new Document(PageSize.A4);
            try
            {
                //写实例 
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                // #endregion //打开document
                doc.Open();
                //载入字体 
                string str = Application.StartupPath;//项目路径                          
                BaseFont baseFT = BaseFont.CreateFont(@str + "/fonts/simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                iTextSharp.text.Font fonttitle = new iTextSharp.text.Font(baseFT, 16); //标题字体 Paragraph 
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT, 10);//内容字体

                //标题
                Paragraph pdftitle = new Paragraph("报警数据查询结果", fonttitle);
                pdftitle.Alignment = 1;
                doc.Add(pdftitle);
                //标题和内容间的空白行
                Paragraph null1 = new Paragraph("  ", fonttitle);
                null1.Leading = 30;
                doc.Add(null1);

                //列表数据 dataTable  
                //调整列顺序 ，列排序从0开始  
                dts.Columns["devtime"].SetOrdinal(0);
                //dts.Columns["terminalname"].SetOrdinal(1);
                //删除列顺序
                dts.Columns.Remove("t_high");
                dts.Columns.Remove("t_low");
                dts.Columns.Remove("h_high");
                dts.Columns.Remove("h_low");
                dts.Columns.Remove("warningTime");

                PdfPTable table = new PdfPTable(10);
                table.WidthPercentage = 100;//table占宽度百分比 100%
                table.SetWidths(new int[] { 5, 20, 15, 15, 5, 7, 7, 14, 8, 8 });
                PdfPCell cell;
                string[] columnsnames = { "序号", "采集时间", "设备标识", "管理主机编号", "仪表编号", "温度", "湿度", "报警事件", "温度差值", "湿度差值" };
                for (int i = 0; i < columnsnames.Length; i++)
                {
                    cell = new PdfPCell(new Phrase(columnsnames[i], font));
                    table.AddCell(cell);
                }
                for (int rowNum = 0; rowNum != dts.Rows.Count; rowNum++)
                {
                    table.AddCell(new Phrase((rowNum + 1).ToString(), font));
                    for (int columNum = 0; columNum != dts.Columns.Count; columNum++)
                    {  
                      table.AddCell(new Phrase(dts.Rows[rowNum][columNum].ToString(), font));
                    }
                }
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.UseAscender = (true);
                //cell.UseDescender = (true);
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                doc.Add(table);
                //关闭document 
                doc.Close();
                //打开PDF，看效果 
                    DialogResult result = MessageBox.Show("是否打开PDF文件！", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                       Process.Start(path);
                    }
                }
            catch (DocumentException de)
            {
                Console.WriteLine(de.Message); Console.ReadKey();
            }
            catch (IOException io)
            {
                Console.WriteLine(io.Message); Console.ReadKey();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool istrue = false;
            double temperature;
            double humidity;
            double t_high;
            double t_low;
            double h_high;
            double h_low;
            string time="";
            if (dataGridView1.RowCount > 0) { 
            string warn = this.dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
            if (!"已处理".Equals(warn))
            {
                warningHandle wh = new warningHandle();
                wh.Text = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + "报警处理";
                wh.label2.Text = frmLogin.name;
                wh.textBox1.Text = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                
                string timew = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string code = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string meter = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                DataTable dtData = wcs.checkData(timew, code, meter);
                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtData.Rows.Count; i++)
                        {
                            temperature = Double.Parse( dtData.Rows[i]["temperature"].ToString());
                            humidity = Double.Parse(dtData.Rows[i]["humidity"].ToString());
                            t_high = Double.Parse(dtData.Rows[i]["t_high"].ToString());
                            t_low = Double.Parse(dtData.Rows[i]["t_low"].ToString());
                            h_high = Double.Parse(dtData.Rows[i]["h_high"].ToString());
                            h_low = Double.Parse(dtData.Rows[i]["h_low"].ToString());

                            if (temperature <= t_high && temperature >= t_low && humidity <= h_high && humidity >= h_low) {
                                time = dtData.Rows[i]["devtime"].ToString();
                                istrue = true;
                                break;
                            }
                        }
                        if (!istrue) {
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    else {
                        time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    //wh.textBox2.Text = time;
                    wh.dateTimePicker1.Value =Convert.ToDateTime(time);
                    wh.textBox5.Text = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString() + "_" + this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                if (wh.ShowDialog() == DialogResult.OK)
                {
                    button1_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("报警已处理，报警处理详情请去报警处理功能中查看！");
            }
        }
            else
            {
                MessageBox.Show("无报警数据！");
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string bs=this.dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
            if (!"未处理".Equals(bs))
            {
                string hostcode = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string metercode = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string time = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string name = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                service.warningCheckService wcs = new service.warningCheckService();
                DataTable dtw = wcs.warningCheck(hostcode+"_"+metercode, time);
                if (dtw.Rows.Count > 0)
                {
                    warningHandle wh = new warningHandle();
                    wh.label2.Text = frmLogin.name;
                    wh.textBox1.Text = time;
                    //wh.textBox2.Text = dtw.Rows[0][3].ToString();
                    wh.dateTimePicker1.Value= Convert.ToDateTime(dtw.Rows[0][3].ToString());
                    wh.textBox3.Text = dtw.Rows[0][4].ToString();
                    wh.textBox4.Text = dtw.Rows[0][5].ToString();
                    wh.richTextBox1.Text = dtw.Rows[0][6].ToString();
                    wh.button1.Visible = false;
                    wh.button2.Visible = false;
                    wh.Show();
                }
            }
            else {
                MessageBox.Show("报警未处理！请先处理报警信息。");
            }
            
        }

      private void pagerControl1_OnPageChanged(object sender, EventArgs e)
        {
            LoadData();
        }
      private void LoadData()
        {
            this.dataGridView1.DataSource = null;
            pagerControl1.DrawControl(dtcount.Rows.Count);//数据总条数
            dt = wcs.warningcheckfenye(time1, time2, cd,pagerControl1.PageIndex,pagerControl1.PageSize);
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
                dv.Sort = "measureCode,meterNo,devtime asc";
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
                    //if (warningTime != null && !"".Equals(warningTime))
                    //{
                    //    this.dataGridView1.Rows[i].Cells[11].Value = "已处理";
                    //}
                    //else
                    //{
                    //    this.dataGridView1.Rows[i].Cells[11].Value = "未处理";
                    //    this.dataGridView1.Rows[i].Cells[11].Style.ForeColor = Color.Red;
                    //}
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
            }
            else
            {
                MessageBox.Show("当前测点无数据，请重新查询！");
            };
        }
    }
}