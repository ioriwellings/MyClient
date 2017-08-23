using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class warningHandleCheck : Form
    {
        DataTable dt = null;
        public warningHandleCheck()
        {
            InitializeComponent();
        }

        private void warningHandleCheck_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = System.Drawing.Image.FromFile(@str + "/images/check.png");
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string time1 = this.dateTimePicker1.Text.ToString();
            string time2 = this.dateTimePicker2.Text.ToString();
            service.warningCheckService wcs = new service.warningCheckService();
            dt = wcs.warningHandlecheck(time1,time2);
            service.deviceInformationService ds = new service.deviceInformationService();
            DataTable dts= ds.selectHouseTypeK();

            if (dts.Rows.Count>0) {
                for (int i=0;i<dts.Rows.Count;i++) {
                    for (int j = 0; j < dt.Rows.Count; j++) {
                        if (dts.Rows[i]["measureMeterCode"].ToString()== dt.Rows[j]["measureMeterCode"].ToString()) {
                            dt.Rows[i].Delete();
                        }
                    }
                }
                dt.AcceptChanges();
            }
            if (dt.Rows.Count > 0)
            {
                dt.Columns["warningTime"].SetOrdinal(6);
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].HeaderCell.Value = "处理人员";
                this.dataGridView1.Columns[2].HeaderCell.Value = "处理时间";
                this.dataGridView1.Columns[3].HeaderCell.Value = "处理方式";
                this.dataGridView1.Columns[4].HeaderCell.Value = "处理结果";
                this.dataGridView1.Columns[5].HeaderCell.Value = "详细描述";
                this.dataGridView1.Columns[6].HeaderCell.Value = "报警时间";
                this.dataGridView1.Columns[7].Visible = false;
                this.dataGridView1.Columns[8].Visible = false;
                this.dataGridView1.Columns[9].HeaderCell.Value = "测点名称";
                this.dataGridView1.Columns[1].Width = 80;
                this.dataGridView1.Columns[2].Width = 120;
                this.dataGridView1.Columns[3].Width = 150;
                this.dataGridView1.Columns[4].Width = 150;
                this.dataGridView1.Columns[5].Width = 270;
                this.dataGridView1.Columns[6].Width = 120;
                this.dataGridView1.Columns[9].Width = 120;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                for (int count = 0; (count <= (this.dataGridView1.Rows.Count - 2)); count++)
                {
                    this.dataGridView1.Rows[count].HeaderCell.Value = String.Format("{0}", count + 1);
                }
                this.dataGridView1.AllowUserToAddRows = false;
            }
            else {
                MessageBox.Show("当前时间段无查询结果！");
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            if (dt!=null&&dt.Rows.Count > 0)
            {
                dt.Columns.Remove("measureMeterCode");
                dt.Columns.Remove("createTime");
                string localFilePath = String.Empty;
                SaveFileDialog fileDialog = new SaveFileDialog();

                fileDialog.InitialDirectory = "C://";

                fileDialog.Filter = "txt files (*.db)|*.db|All files (*.*)|*.*";

                //设置文件名称：
                fileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "报警处理查询结果.pdf";

                fileDialog.FilterIndex = 2;

                fileDialog.RestoreDirectory = true;
                if (fileDialog.ShowDialog() == DialogResult.OK)

                {   //获得文件路径
                    localFilePath = fileDialog.FileName.ToString();
                    CreateTable(dt.Copy(), localFilePath);
                    MessageBox.Show("恭喜，PDF文件生产成功!");
                }

            }
            else
            {
                MessageBox.Show("无数据，请重试!");
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

                //标题
                Paragraph pdftitle = new Paragraph("报警处理查询结果", fonttitle);
                pdftitle.Alignment = 1;
                doc.Add(pdftitle);
                //标题和内容间的空白行
                Paragraph null1 = new Paragraph("  ", fonttitle);
                null1.Leading = 10;
                doc.Add(null1);
                dts.Columns["warningTime"].SetOrdinal(6);
                PdfPTable table = new PdfPTable(8);
                table.WidthPercentage = 100;//table占宽度百分比 100%
                table.SetWidths(new int[] { 5, 9, 15, 13, 13, 20, 15, 12 });
                PdfPCell cell;
                //   , "温度上限", "温度下限", "湿度上限", "湿度下限"
                string[] columnsnames = { "序号", "处理人员", "处理时间", "处理方式", "处理结果", "详情描述", "报警时间", "测点名称" };
                for (int i = 0; i < columnsnames.Length; i++)
                {
                    cell = new PdfPCell(new Phrase(columnsnames[i], font));
                    table.AddCell(cell);
                }

                for (int rowNum = 0; rowNum != dts.Rows.Count; rowNum++)
                {
                    table.AddCell(new Phrase((rowNum + 1).ToString(), font));
                    for (int columNum = 1; columNum != dts.Columns.Count; columNum++)
                    {
                        table.AddCell(new Phrase(dts.Rows[rowNum][columNum].ToString(), font));
                    }
                }
              
                doc.Add(table);
                //关闭document 
                doc.Close();
                //打开PDF，看效果 
                //Process.Start(path);
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
    }
}
