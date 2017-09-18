using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace LBKJClient
{
    public partial class historydata : Form
    {
        changGuiCheck changeguicheck;
        service.changGuicheckService cgs = new service.changGuicheckService();
        DataTable dd = null;
      
        int flag = 0;
        public historydata()
        {
            InitializeComponent();
            this.groupBox2.Visible = false;
            this.groupBox1.Size = new Size(1347, 710);
        }

        private void historydata_Load(object sender, EventArgs e)
        {
            changeguicheck = new changGuiCheck();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            changeguicheck.flag = 1;
            DataTable dts = null;
            if (this.groupBox2.Visible)
            {
                if (this.checkBox1.Checked)
                {
                    this.checkBox1.Checked = false;
                }
                this.groupBox2.Visible = false;
                this.groupBox1.Size = new Size(1900, 610);
            }
            //常规查询数据展现
            if (changeguicheck.ShowDialog() == DialogResult.OK)
            {
                DateTime t1 = DateTime.Parse(changeguicheck.time1);
                DateTime t2 = DateTime.Parse(changeguicheck.time2);
                TimeSpan span = t2.Subtract(t1);
               
                if (span.Days> 180)
                {
                    MessageBox.Show("你好，不能查询超过半年的数据!");
                    return;
                }
                this.dataGridView1.DataSource = null;
                pagerControl1.OnPageChanged += new EventHandler(pagerControl_OnPageChanged);
                DataTable dtcountliutengfei = null;
                if (changeguicheck.pageNo == 0)
                {
                    dtcountliutengfei = cgs.changguicheck(changeguicheck.time1,
                        changeguicheck.time2, changeguicheck.cdlist, changeguicheck.measureNolist).Tables[0];
                  
                }
                else
                {
                    dtcountliutengfei = cgs.changguicheckGlzj(changeguicheck.time1,
                        changeguicheck.time2, changeguicheck.cdlist).Tables[0];
                }
                if (dtcountliutengfei.Rows[0][0].ToString() != "0")
                {
                   
                    this.label3.Text = "总数据量：" + dtcountliutengfei.Rows[0][0].ToString() + " 条";
               
                    this.label4.Text = "报警数据：" + dtcountliutengfei.Rows[1][0].ToString() + " 条";
                    DataTable dtd = new DataTable();
                    dtd.Columns.Add("title", typeof(string));
                    dtd.Columns.Add("wdcz", typeof(double));
                    dtd.Columns.Add("sdcz", typeof(double));
                    DataRow newRow;
                    newRow = dtd.NewRow();
                    newRow["title"] = "最大值";
                    newRow["wdcz"] = Math.Round(Convert.ToDouble(dtcountliutengfei.Rows[0][1]), 1);
                    newRow["sdcz"] = Math.Round(Convert.ToDouble(dtcountliutengfei.Rows[0][4]), 1);
                    dtd.Rows.Add(newRow);
                    newRow = dtd.NewRow();
                    newRow["title"] = "最小值";
                    newRow["wdcz"] = Math.Round(Convert.ToDouble(dtcountliutengfei.Rows[0][2]), 1);
                    newRow["sdcz"] = Math.Round(Convert.ToDouble(dtcountliutengfei.Rows[0][5]), 1);
                    dtd.Rows.Add(newRow);
                    newRow = dtd.NewRow();
                    newRow["title"] = "平均值";
                    newRow["wdcz"] = Math.Round(Convert.ToDouble(dtcountliutengfei.Rows[0][3]), 1);
                    newRow["sdcz"] = Math.Round(Convert.ToDouble(dtcountliutengfei.Rows[0][6]), 1);
                    dtd.Rows.Add(newRow);
                    this.dataGridView3.DataSource = dtd;
                    this.dataGridView3.Columns[0].HeaderCell.Value = "类型";
                    this.dataGridView3.Columns[1].HeaderCell.Value = "温度";
                    this.dataGridView3.Columns[2].HeaderCell.Value = "湿度";
                    this.dataGridView3.Columns[0].Width = 100;
                    this.dataGridView3.Columns[1].Width = 75;
                    this.dataGridView3.Columns[2].Width = 75;
                    this.dataGridView3.Columns[1].DefaultCellStyle.Format = "0.0";
                    this.dataGridView3.Columns[2].DefaultCellStyle.Format = "0.0";
                    this.dataGridView3.RowHeadersVisible = false;
                    this.dataGridView3.AllowUserToAddRows = false;
                    pagerControl1.DrawControl(Convert.ToInt32(dtcountliutengfei.Rows[0][0]));//数据总条数
                    LoadData();
                }
                else
                {
                    pagerControl1.DrawControl(0);
                    MessageBox.Show("当前时段未查询出数据！");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        { //******调用处理报警数据信息********
            bool check = this.checkBox1.Checked;
            if (check != true)
            {
                this.groupBox2.Visible = false;
                //this.groupBox1.Size = new Size(1507,653);
                this.groupBox1.Size = new Size(1707, 710);
            }
            else
            {
                showWarning();
                //this.groupBox1.Size = new Size(1507,480)
                this.groupBox1.Size = new Size(1707, 543);
                this.groupBox2.Visible = true;
            }

        }
        public void showWarning()
        {
            //******处理报警数据信息********
            DataTable dd = null;
            if (changeguicheck.pageNo == 0)
            {
                dd = cgs.changguicheckliutengfei(changeguicheck.time1,
                    changeguicheck.time2, changeguicheck.cdlist, changeguicheck.measureNolist).Tables[0];

            }
            else
            {
                dd = cgs.changguicheckGlzj(changeguicheck.time1,
                    changeguicheck.time2, changeguicheck.cdlist).Tables[0];
            }


            if (dd != null)
            {
               
                //DataRow[] dr1 = dd1.Select("mcc = '0' and warningistrue='2' or warningistrue='3' or warnState='1' or warnState='3'");

                if (dd.Rows.Count > 0)
                {
                  
                    dd.Columns.RemoveAt(14);
                    dd.Columns.RemoveAt(13);
                    dd.Columns.RemoveAt(12);
                    dd.Columns.RemoveAt(11);

                    dd.Columns["devtime"].SetOrdinal(0);
                    dd.Columns["terminalname"].SetOrdinal(1);
                    //dd.Columns.Add("wdcz", typeof(string));
                    //dd.Columns.Add("sdcz", typeof(string));
                    int num = dd.Rows.Count;
                    double wd;
                    double sd;
                    double wdsx;
                    double wdxx;
                    double sdsx;
                    double sdxx;
                    int powerwarn = 0;

                    for (int i = 0; i < num; i++)
                    {
                        string warningEvent = "";
                        wd = double.Parse(dd.Rows[i][4].ToString());
                        sd = double.Parse(dd.Rows[i][5].ToString());
                        if (dd.Rows[i][6].ToString() != "" && dd.Rows[i][7].ToString() != "" && dd.Rows[i][8].ToString() != "" && dd.Rows[i][9].ToString() != "")
                        {
                            wdsx = double.Parse(dd.Rows[i][6].ToString());
                            wdxx = double.Parse(dd.Rows[i][7].ToString());
                            sdsx = double.Parse(dd.Rows[i][8].ToString());
                            sdxx = double.Parse(dd.Rows[i][9].ToString());
                        }
                        else
                        {
                            wdsx = 0.0;
                            wdxx = 0.0;
                            sdsx = 0.0;
                            sdxx = 0.0;
                        };
                        string pw = dd.Rows[i][10].ToString();
                        if (pw != null && !"".Equals(pw))
                        {
                            powerwarn = Int32.Parse(pw);
                            if (powerwarn == 1)
                            {
                                warningEvent = "断电报警 ";
                            }
                        }
                        if (wd > wdsx)
                        {
                            dd.Rows[i][11] = Math.Round(wd - wdsx, 1).ToString("0.0");
                            warningEvent += "温度上限报警 ";
                        }
                        else if (wd < wdxx)
                        {
                            dd.Rows[i][11] = Math.Round(wd - wdxx, 1).ToString("0.0");
                            warningEvent += "温度下限报警 ";
                        }
                        else
                        {
                            dd.Rows[i][11] = "0.0";
                        }

                        if (sd > sdsx)
                        {
                            dd.Rows[i][12] = Math.Round(sd - sdsx, 1).ToString("0.0");
                            warningEvent += "湿度上限报警 ";
                        }
                        else if (sd < sdxx)
                        {
                            dd.Rows[i][12] = Math.Round(sd - sdxx, 1).ToString("0.0");
                            warningEvent += "湿度下限报警 ";
                        }
                        else
                        {
                            dd.Rows[i][12] = "0.0";
                        }
                        if (warningEvent == "")
                        {
                            warningEvent = "解除报警 ";
                        }
                        dd.Rows[i][10] = warningEvent;
                    }
                    DataView dv = dd.DefaultView;//虚拟视图
                    dv.Sort = "devtime asc";
                    DataTable dtnew = dv.ToTable(true);
                    this.dataGridView2.DataSource = dtnew;
                    this.dataGridView2.Columns[0].HeaderCell.Value = "数据采集时间";
                    this.dataGridView2.Columns[1].HeaderCell.Value = "设备标识名称";
                    this.dataGridView2.Columns[2].HeaderCell.Value = "管理主机编号";
                    this.dataGridView2.Columns[3].HeaderCell.Value = "仪表编号";
                    this.dataGridView2.Columns[4].HeaderCell.Value = "温度";
                    this.dataGridView2.Columns[5].HeaderCell.Value = "湿度";
                    this.dataGridView2.Columns[6].Visible = false;
                    this.dataGridView2.Columns[7].Visible = false;
                    this.dataGridView2.Columns[8].Visible = false;
                    this.dataGridView2.Columns[9].Visible = false;
                    this.dataGridView2.Columns[10].HeaderCell.Value = "报警事件";
                    this.dataGridView2.Columns[11].HeaderCell.Value = "温度超标差值";
                    this.dataGridView2.Columns[12].HeaderCell.Value = "湿度超标差值";
                    this.dataGridView2.Columns[0].Width = 150;
                    this.dataGridView2.Columns[1].Width = 170;
                    this.dataGridView2.Columns[2].Width = 150;
                    this.dataGridView2.Columns[3].Width = 80;
                    this.dataGridView2.Columns[4].Width = 80;
                    this.dataGridView2.Columns[5].Width = 80;
                    this.dataGridView2.Columns[10].Width = 250;
                    this.dataGridView2.RowsDefaultCellStyle.ForeColor = Color.Red;
                    this.dataGridView2.Columns[4].DefaultCellStyle.Format = "0.0";
                    this.dataGridView2.Columns[5].DefaultCellStyle.Format = "0.0";
                    this.dataGridView2.Columns[11].DefaultCellStyle.Format = "0.0";
                    this.dataGridView2.Columns[12].DefaultCellStyle.Format = "0.0";
                    this.dataGridView2.AllowUserToAddRows = false;

                }
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            DataTable dtss = null;
            if (changeguicheck.pageNo == 0)
            {
                dtss = cgs.changguicheckliutengfeiPDF(changeguicheck.time1,
                    changeguicheck.time2, changeguicheck.cdlist, changeguicheck.measureNolist).Tables[0];

            }
            else if (changeguicheck.pageNo == -1)
            {
                MessageBox.Show("你好，请在常规查询中输入条件!");
                return;
            } 
            else
            {
                dtss = cgs.changguicheckGlzjliutengfei(changeguicheck.time1,
                    changeguicheck.time2, changeguicheck.cdlist).Tables[0];
            } 
            if (dtss != null && dtss.Rows.Count > 0)
            {
                dtss.Columns.Remove("warnState");
                dtss.Columns.Remove("mcc");
                string localFilePath = String.Empty;
                SaveFileDialog fileDialog = new SaveFileDialog();

                fileDialog.InitialDirectory = "C://";

                fileDialog.Filter = "All files (*.*)|*.*";

                //设置文件名称：
                fileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "历史数据查询结果.pdf";

                fileDialog.FilterIndex = 2;

                fileDialog.RestoreDirectory = true;
                if (fileDialog.ShowDialog() == DialogResult.OK)

                {   //获得文件路径
                    localFilePath = fileDialog.FileName.ToString();
                    CreateTable(dtss, localFilePath);
                    MessageBox.Show("恭喜，历史数据PDF文件生成成功!");
                    warningPdf();
                }

            }
            else
            {
                MessageBox.Show("无历史数据，请先查询历史数据后再生成PDF文件!");
            }
        }
        private void warningPdf()
        {

            if (this.checkBox1.Checked)
            {
                DialogResult result = MessageBox.Show("是否生成报警数据PDF文件？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (dataGridView2.Rows.Count > 0)
                    {
                        DataTable dt1 = new DataTable();

                        // 列强制转换
                        for (int count = 0; count < dataGridView2.Columns.Count; count++)
                        {
                            DataColumn dc = new DataColumn(dataGridView2.Columns[count].Name.ToString());
                            dt1.Columns.Add(dc);
                        }

                        // 循环行
                        for (int count = 0; count < dataGridView2.Rows.Count; count++)
                        {
                            DataRow dr = dt1.NewRow();
                            for (int countsub = 0; countsub < dataGridView2.Columns.Count; countsub++)
                            {
                                dr[countsub] = Convert.ToString(dataGridView2.Rows[count].Cells[countsub].Value);
                            }
                            dt1.Rows.Add(dr);
                        }
                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            flag = 1;
                            string localFilePath = String.Empty;
                            SaveFileDialog fileDialog = new SaveFileDialog();

                            fileDialog.InitialDirectory = "C://";

                            fileDialog.Filter = "All files (*.*)|*.*";

                            //设置文件名称：
                            fileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "历史报警数据查询结果.pdf";

                            fileDialog.FilterIndex = 2;

                            fileDialog.RestoreDirectory = true;
                            if (fileDialog.ShowDialog() == DialogResult.OK)

                            {   //获得文件路径
                                localFilePath = fileDialog.FileName.ToString();
                                CreateTable(dt1, localFilePath);
                                MessageBox.Show("恭喜，历史报警数据PDF文件生成成功!");
                            }

                        }
                        else
                        {
                            MessageBox.Show("无报警数据，无法生成PDF文件!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("无报警数据，无法生成PDF文件!");
                    }
                }
            }
        }
        private void CreateTable(DataTable dts, string path)
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
                Paragraph pdftitle = null;

                PdfPTable table = null;

                string[] columnsnames = { "序号", "采集时间", "设备标识", "管理主机编号", "仪表编号", "温度", "湿度", "是否为空库" };
                //标题
                if (flag == 1)
                {
                    table = new PdfPTable(10);
                    table.WidthPercentage = 100;//table占宽度百分比 100%
                    pdftitle = new Paragraph(frmMain.companyName + "历史报警数据查询结果" + "\r\n" + "(" + changeguicheck.time1 + "-" + changeguicheck.time2 + ")", fonttitle);
                    table.SetWidths(new int[] { 5, 20, 15, 15, 5, 7, 7, 14, 8, 8 });
                    columnsnames = new string[] { "序号", "采集时间", "设备标识", "管理主机编号", "仪表编号", "温度", "湿度", "报警事件", "温度差值", "湿度差值" };
                    flag = 0;
                    //dts.Columns.Remove("sdcz");
                }
                else
                {
                    table = new PdfPTable(8);
                    pdftitle = new Paragraph(frmMain.companyName + "历史数据查询结果" + "\r\n" + "(" + changeguicheck.time1 + "-" + changeguicheck.time2 + ")", fonttitle);
                    table.SetWidths(new int[] { 5, 20, 15, 20, 10, 10, 10, 10 });
                    dts.Columns.Remove("measureMeterCode");
                    dts.Columns.Remove("warningistrue");
                }
                table.WidthPercentage = 100;//table占宽度百分比 100%

                pdftitle.Alignment = 1;
                doc.Add(pdftitle);
                //标题和内容间的空白行
                Paragraph null1 = new Paragraph("  ", fonttitle);
                null1.Leading = 30;
                doc.Add(null1);

                //列表数据 dataTable  
                //调整列顺序 ，列排序从0开始  
                dts.Columns["devtime"].SetOrdinal(0);
                dts.Columns["terminalname"].SetOrdinal(1);
                //删除列顺序
                dts.Columns.Remove("t_high");
                dts.Columns.Remove("t_low");
                dts.Columns.Remove("h_high");
                dts.Columns.Remove("h_low");

                PdfPCell cell;

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
                //新增最大 最小 平均
                PdfPTable table1 = new PdfPTable(4);
                table1.WidthPercentage = 100;//table占宽度百分比 100%
                table1.SetWidths(new int[] { 5, 20, 20, 20 });
                PdfPCell cell1;
                //   , "温度上限", "温度下限", "湿度上限", "湿度下限"
                string[] columnsnames1 = { "序号", "类型", "温度", "湿度" };
                DataTable dtd = (DataTable)this.dataGridView3.DataSource;
                for (int i = 0; i < columnsnames1.Length; i++)
                {
                    cell1 = new PdfPCell(new Phrase(columnsnames1[i], font));
                    table1.AddCell(cell1);
                }
                for (int rowNum = 0; rowNum != dtd.Rows.Count; rowNum++)
                {
                    table1.AddCell(new Phrase((rowNum + 1).ToString(), font));
                    for (int columNum = 0; columNum != dtd.Columns.Count; columNum++)
                    {
                        table1.AddCell(new Phrase(dtd.Rows[rowNum][columNum].ToString(), font));
                    }
                }
                doc.Add(table);
                doc.Add(null1);
                doc.Add(null1);
                doc.Add(table1);
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
                Console.WriteLine(de.Message);
            }
            catch (IOException io)
            {
                Console.WriteLine(io.Message);
            }
        }
        private void dataGridView2_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {//为报警列表  添加序号
            SolidBrush solidBrush = new SolidBrush(this.dataGridView2.RowHeadersDefaultCellStyle.ForeColor);
            int xh = e.RowIndex + 1;
            e.Graphics.DrawString(xh.ToString(), e.InheritedRowStyle.Font, solidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 4);
        }
        private void pagerControl_OnPageChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            DataTable dtt = null;
            this.dataGridView1.DataSource = null;

            if (changeguicheck.pageNo == 0)
            {
                dtt = cgs.changguicheckFenye(changeguicheck.time1, changeguicheck.time2, changeguicheck.cdlist, pagerControl1.PageIndex, pagerControl1.PageSize, changeguicheck.measureNolist);
                dtt.Columns.Remove("measureNo");
            }
            else
            {
                dtt = cgs.changguicheckGlzjFenye(changeguicheck.time1, changeguicheck.time2, changeguicheck.cdlist, pagerControl1.PageIndex, pagerControl1.PageSize);
            }

            if (dtt.Rows.Count > 0)
            {
                dtt.Columns.Remove("houseinterval");
                dtt.Columns.Remove("carinterval");
                dtt.Columns.Remove("warnState");
                dtt.Columns.Remove("mcc");


                //this.dataGridView3.ColumnHeadersVisible = false;
                //调整列顺序 ，列排序从0开始  
                dtt.Columns["devtime"].SetOrdinal(0);
                dtt.Columns["terminalname"].SetOrdinal(1);
                this.dataGridView1.DataSource = dtt;
                this.dataGridView1.Columns[0].HeaderCell.Value = "数据采集时间";
                this.dataGridView1.Columns[1].HeaderCell.Value = "设备标识名称";
                this.dataGridView1.Columns[2].HeaderCell.Value = "管理主机编号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "仪表编号";
                this.dataGridView1.Columns[4].HeaderCell.Value = "温度";
                this.dataGridView1.Columns[5].HeaderCell.Value = "湿度";
                this.dataGridView1.Columns[6].HeaderCell.Value = "温度上限";
                this.dataGridView1.Columns[7].HeaderCell.Value = "温度下限";
                this.dataGridView1.Columns[8].HeaderCell.Value = "湿度上限";
                this.dataGridView1.Columns[9].HeaderCell.Value = "湿度下限";
                this.dataGridView1.Columns[10].Visible = false;
                this.dataGridView1.Columns[11].Visible = false;
                this.dataGridView1.Columns[12].HeaderCell.Value = "是否为空库";
                this.dataGridView1.Columns[0].Width = 150;
                this.dataGridView1.Columns[1].Width = 170;
                this.dataGridView1.Columns[2].Width = 150;
                this.dataGridView1.Columns[3].Width = 80;
                this.dataGridView1.Columns[4].Width = 80;
                this.dataGridView1.Columns[5].Width = 80;
                this.dataGridView1.Columns[6].Width = 80;
                this.dataGridView1.Columns[7].Width = 80;
                this.dataGridView1.Columns[8].Width = 80;
                this.dataGridView1.Columns[9].Width = 80;

                this.dataGridView1.Columns[4].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[5].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[6].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[7].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[8].DefaultCellStyle.Format = "0.0";
                this.dataGridView1.Columns[9].DefaultCellStyle.Format = "0.0";

                this.dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;

                //  int row = this.dataGridView1.Rows.Count;//得到总行数    

            }
        }
    }
}
