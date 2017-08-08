using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LBKJClient
{
    public partial class graphCheck : Form
    {
        changGuiCheck changeguicheck = new changGuiCheck();
        DataTable dt = null;
        Series series1;
        Series series2;
        string time1 = null;
        string time2 = null;
        public graphCheck()
        {
            InitializeComponent();   
        }
        private void graphCheck_Load(object sender, EventArgs e)
        { //悬停工具提示事件
            chart1.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart1_GetToolTipText);

            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.chart_MouseWheel);
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/check.png");
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            //查询主机编号信息
            service.manageHostService mhs = new service.manageHostService();
            DataTable dt1 = mhs.queryManageHost();
            this.comboBox3.DataSource = dt1;//绑定数据源
            this.comboBox3.DisplayMember = "hostName";//显示给用户的数据集表项
            this.comboBox3.ValueMember = "measureCode";//操作时获取的值 
            this.comboBox3.Text = "--请选择--";
            this.comboBox1.Text = "--请选择--";

            //图表显示  
            if (chart1.Titles!=null) {
                chart1.Titles.Clear();
            }
            if (chart1.Series!=null){
                chart1.Series.Clear();
            }
            //第一线条
            series1 = new Series();
            series1.Name = "温度";
            series1.ChartType = SeriesChartType.Line;
            series1.IsValueShownAsLabel = false;//是否显示值  
            series1.BorderWidth = 1;           //线条宽度  
            series1.IsVisibleInLegend = true; //是否显示数据说明
            //series1.Color = Color.YellowGreen;

            series2 = new Series();
            series2.Name = "湿度";
            series2.ChartType = SeriesChartType.Line;
            series2.IsValueShownAsLabel = false;//是否显示值  
            series2.BorderWidth = 1;           //线条宽度  
            series2.IsVisibleInLegend = true; //是否显示数据说明
            //series2.Color = Color.CadetBlue;

            //曲线图的标题
            chart1.Titles.Add("温湿度数据曲线图");
            chart1.Titles[0].ForeColor = Color.Green;
            //chart1.Legends.First().Enabled = false;
            chart1.Legends[0].Enabled = true;//是否显示图例  
            chart1.BackColor = Color.FromArgb(243, 223, 193);
            chart1.BackColor = ColorTranslator.FromHtml("#D3DFF0");//用网页颜色  
            chart1.BackGradientStyle = GradientStyle.TopBottom;//渐变背景，从上到下  
            chart1.BorderlineDashStyle = ChartDashStyle.Solid;//外框线为实线  
            chart1.BorderlineWidth = 2;
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;   //不显示X轴的margin
            chart1.ChartAreas[0].AxisX.Title = "采集时间";
            chart1.ChartAreas[0].AxisY.Maximum = 100;//设置Y轴最大值
            chart1.ChartAreas[0].AxisY.Minimum = -100;//设置Y轴最大值
            chart1.ChartAreas[0].AxisY.Title = "温湿度";
            chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Stacked;
            chart1.ChartAreas[0].BackColor = Color.Transparent;//数据区域的背景，默认为白色  
            chart1.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            chart1.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);//数据区域，纵向的线条颜色  
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);//数据区域，横向线条的颜色
            chart1.ChartAreas[0].AxisX.Interval = 0; //设置为0表示由控件自动分配
            chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Auto;
            //chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            //chart1.ChartAreas[0].AxisX.ScaleView.Zoom(2, 3);
            //将滚动内嵌到坐标轴中
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            // 设置滚动条的大小
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 20;
            // 设置滚动条的按钮的风格
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 2;
            //显示背景间隔带，效果如下图：
            chart1.ChartAreas[0].AxisY.IsInterlaced = true; 
            chart1.ChartAreas[0].AxisY.InterlacedColor = System.Drawing.Color.FromArgb(239, 242, 245);
            //chart1.Series.Add(series1);
            //chart1.Series.Add(series2);.
        }
        //鼠标滚轮事件(移动/缩放)
        private void chart_MouseWheel(object sender, MouseEventArgs e)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                //按住Ctrl，缩放
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    if (e.Delta < 0)
                        chart1.ChartAreas[0].AxisX.ScaleView.Size += 3;
                    else
                        chart1.ChartAreas[0].AxisX.ScaleView.Size -= 3;
                }
                //不按Ctrl，滚动
                else
                {
                    if (e.Delta < 0)
                    {

                        if (chart1.ChartAreas[0].AxisX.ScaleView.Position < dt.Rows.Count - chart1.ChartAreas[0].AxisX.ScaleView.Size)
                        {
                            chart1.ChartAreas[0].AxisX.ScaleView.Position += 1;
                        }
                    }
                    else
                    {
                        if (chart1.ChartAreas[0].AxisX.ScaleView.Position > 1)
                        {
                            chart1.ChartAreas[0].AxisX.ScaleView.Position -= 1;
                        }
                    }
                }
            }
        }
        private void tubiaoshow(DataTable dt) {
            double avgHumidity = 0;
            if (dt!=null) {
                if (dt.Rows.Count > 0)
                {
                    double t1 = double.Parse(dt.Compute("Max(temperature)", "true").ToString());
                    double t2 = double.Parse(dt.Compute("Min(temperature)", "true").ToString());
                    double h1 = double.Parse(dt.Compute("Max(humidity)", "true").ToString());
                    double h2 = double.Parse(dt.Compute("Min(humidity)", "true").ToString());

                    chart1.ChartAreas[0].AxisY.Maximum = t1 > h1 ? t1+5 : h1+5;//设置Y轴最大值
                    chart1.ChartAreas[0].AxisY.Minimum = t2 > h2 ? h2-5 : t2-5;//设置Y轴最大值

                    this.label12.Text = "温度最大值:【"+t1.ToString()+ "】  温度最小值:【" + t2.ToString()+ "】  湿度最大值:【" + h1.ToString() + "】  湿度最小值:【" + h2.ToString()+ "】";
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    chart1.Series.Clear();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        series1.Points.AddXY(dt.Rows[i]["devtime"], dt.Rows[i]["temperature"]);
                    }
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        series2.Points.AddXY(dt.Rows[i]["devtime"], dt.Rows[i]["humidity"]);
                        avgHumidity +=double.Parse(dt.Rows[i]["humidity"].ToString());
                    }

                    if (avgHumidity / dt.Rows.Count == 0)
                    {
                        series2.Enabled = false;
                    }
                    else {
                        series2.Enabled = true;
                    }                
                }
                if (chart1.Series.Count != 2)
                {
                    series1.Name = this.comboBox1.Text+"温度";
                    series2.Name = this.comboBox1.Text+"湿度";
                    series1.BorderWidth = 2;
                    series2.BorderWidth = 2;
                    chart1.Series.Add(series1);
                    chart1.Series.Add(series2);
                }

            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            chart1.Titles.Clear();
            chart1.Series.Clear();
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string localFilePath = String.Empty;
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.InitialDirectory = "C://";

            fileDialog.Filter = "txt files (*.db)|*.db|All files (*.*)|*.*";

            //设置文件名称：
            fileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "历史数据曲线图.jpg";

            fileDialog.FilterIndex = 2;

            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {   //获得文件路径
                localFilePath = fileDialog.FileName.ToString();
                try
                {
                    chart1.SaveImage(localFilePath, ImageFormat.Bmp);
                    MessageBox.Show("恭喜，图片保存成功！");
                }
                catch
                {
                    MessageBox.Show("图片保存失败，请重新保存！");
                }
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            service.chartsCheck cc = new service.chartsCheck();
            time1 = this.dateTimePicker1.Text.ToString();
            time2 = this.dateTimePicker2.Text.ToString();
            //string inter = this.comboBox2.SelectedItem.ToString();
            if (!"--请选择--".Equals(this.comboBox1.Text))
            {
                if (comboBox1.SelectedValue!=null) {
                string cd = this.comboBox1.SelectedValue.ToString();
                string[] measuremeter = cd.Split('_');
                string measure = "";
                    for (int i = 0; i < measuremeter.Length - 1; i++)
                    {
                        measure += "_" + measuremeter[i];
                    }
                    measure = measure.Substring(1);
                    string meter = measuremeter[measuremeter.Length - 1];

                    dt = cc.chartschecksService(time1, time2, measure, meter);
                chart1.Titles.RemoveAt(0);
                chart1.Titles.Add(frmMain.companyName + time1 + "--" + time2 + "温湿度数据曲线图");
                chart1.Titles[0].ForeColor = Color.Green;
                if (dt != null && dt.Rows.Count > 0)
                {
                        //if (inter != "0")
                        //{
                        //    this.chart1.ChartAreas[0].AxisX.Interval = double.Parse(inter);
                        //}
                        
                        tubiaoshow(dt);
                }
                else
                {
                    dt = null;   
                    tubiaoshow(dt);
                    MessageBox.Show("未查询出数据，请重新选择！");
                }
              }
                else
                {
                    MessageBox.Show("未选择测点或测点数据为空！");
                }
            }
            else {
                    MessageBox.Show("请选择你要查询的测点！");
            }
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string code=this.comboBox3.SelectedValue.ToString();
            if (code!=null&&!"".Equals(code)) {
                service.changGuicheckService cgs = new service.changGuicheckService();
                DataTable dt2=cgs.checkcedianAll(code).Tables[0];
                this.comboBox1.DataSource = dt2;//绑定数据源
                this.comboBox1.DisplayMember = "terminalname";//显示给用户的数据集表项
                this.comboBox1.ValueMember = "measureMeterCode";//操作时获取的值 
                this.comboBox1.Text = "--请选择--";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (this.groupBox1.Visible) {
                this.groupBox1.Visible = false;
                this.groupBox2.Location = new Point(12, 35);
            }
            else {
                this.groupBox2.Location = new Point(12, 167); 
                this.groupBox1.Visible = true;
            }
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            if (this.groupBox1.Visible)
            {
                this.groupBox1.Visible = false;
                this.groupBox2.Location = new Point(12, 35);
            }
            if (changeguicheck.ShowDialog() == DialogResult.OK)
            {
                if (changeguicheck.dt != null)  
                {
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    chart1.Series.Clear();

                    DataTable dtt = changeguicheck.dt;
                    string []cds= changeguicheck.cdlist.Split(',');
                    if (cds.Length > 0)
                    {
                        chart1.Titles.RemoveAt(0);
                        chart1.Titles.Add(frmMain.companyName + changeguicheck.time1 + "--" + changeguicheck.time2 + "温湿度数据曲线图");
                        chart1.Titles[0].ForeColor = Color.Green;
                        double t1 = (double)dtt.Compute("Max(temperature)", "true");
                        double t2 = (double)dtt.Compute("Min(temperature)", "true");
                        double h1 = (double)dtt.Compute("Max(humidity)", "true");
                        double h2 = (double)dtt.Compute("Min(humidity)", "true");

                        chart1.ChartAreas[0].AxisY.Maximum = t1 > h1 ? t1+5: h1+5;//设置Y轴最大值
                        chart1.ChartAreas[0].AxisY.Minimum = t2 > h2 ? h2-5: t2-5;//设置Y轴最大值


                        this.label12.Text = "温度最大值:【" + t1.ToString() + "】温度最小值:【" + t2.ToString() + "】湿度最大值:【" + h1.ToString() + "】湿度最小值:【" + h2.ToString() + "】";
                        for (int i = 0; i < cds.Length; i++)
                        {
                            DataRow[] drs = dtt.Select("measureMeterCode = '" + cds[i] + "'");
                            if (drs.Length < 1) { continue; };
                            series1 = new Series();
                            series1.Name = drs[0]["terminalname"] +"温度";
                            series1.ChartType = SeriesChartType.Line;
                            series1.IsValueShownAsLabel = false;//是否显示值  
                            series1.BorderWidth = 1;           //线条宽度  
                            series1.IsVisibleInLegend = true; //是否显示数据说明
                                                              

                            series2 = new Series();
                            series2.Name = drs[0]["terminalname"] +"湿度";
                            series2.ChartType = SeriesChartType.Line;
                            series2.IsValueShownAsLabel = false;//是否显示值  
                            series2.BorderWidth = 1;           //线条宽度  
                            series2.IsVisibleInLegend = true; //是否显示数据说明

                            for (int j = 0; j < drs.Length; j++)
                            {
                                series1.Points.AddXY(drs[j]["devtime"], drs[j]["temperature"]);
                                series2.Points.AddXY(drs[j]["devtime"], drs[j]["humidity"]);
                                
                            }
                            chart1.Series.Add(series1);
                            chart1.Series.Add(series2);
                        }
                    }
                }
            }
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            //判断鼠标是否移动到数据标记点，是则显示提示信息
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                //分别显示x轴和y轴的数值，其中{1:F3},表示显示的是float类型，精确到小数点后3位。                     
                this.label13.Visible = true;
                this.label13.Text =  dp.YValues[0].ToString("0.0");

                //鼠标相对于窗体左上角的坐标
                //Point formPoint = this.PointToClient(Control.MousePosition);
                //int x = formPoint.X;
                //int y = formPoint.Y;
                //显示提示信息
                //this.panel1.Visible = true;
                //this.panel1.Location = new Point(x, y);
                
            }

            //鼠标离开数据标记点，则隐藏提示信息
            else
            {
                this.label13.Visible = false;
            }
        }
    }
}
