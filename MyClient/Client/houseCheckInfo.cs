using System;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;

namespace LBKJClient
{
    public partial class houseCheckInfo : Form
    {
        PrivateFontCollection privateFonts = new PrivateFontCollection();
        Rectangle rect = new Rectangle();
        service.houseTypeService hts = new service.houseTypeService();
        service.rtmonitoringService rs = new service.rtmonitoringService();
        private static string str = Application.StartupPath;//项目路径
        string overtime = Properties.Settings.Default.overTime;
        private string path = @"config.xml";
        Label[] labels;
        PictureBox[] picb;
        string houseid = null;
        int autoSizeX = 180;
        int autoSizeY = 155;
        private double a = 0;
        private double b = 0;
        private double c = 0;
        private double d = 0;
        private double e1 = 0;
        private double f = 0;
        DataTable dt;
        string datarefreshtime = null;
        public houseCheckInfo()
        {
            InitializeComponent();
        }

        private void houseCheckInfo_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/fanhui.png");
            privateFonts.AddFontFile(@str + @"/fonts/SIMYOU.TTF");//加载路径的字体
            rect = Screen.GetWorkingArea(this);
            this.label1.Text="库房列表";
            this.label1.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = Color.FromArgb(65, 105, 225);
            this.label1.Left = (this.Width - this.label1.Width) / 2;
            this.label1.BringToFront();
            getFromXmldatarefreshtime();
            houseTypeInfo();

        }

        int flag = 0;
        int kk = 0;
        double wd = 0;
        double sd = 0;
        int wdnum = 0;
        int sdnum = 0;
        double avgwd = 0;
        double avgsd = 0;
        private void houseTypeInfo() {
            dt = hts.queryhouseType();
            int houseCount = dt.Rows.Count;
            if (houseCount > 0)
            {
                flowLayoutPanel1.Controls.Clear();
                labels = new Label[houseCount];//例如10个
                for (int x = 0; x < labels.Length; x++)
                {
                    labels[x] = new Label();
                    labels[x].Tag = dt.Rows[x]["id"].ToString()+"-"+dt.Rows[x]["name"].ToString();
                    string kkk = dt.Rows[x]["isUsed"].ToString();
                    if (kkk != null && !"".Equals(kkk))
                    {
                        kk = Int32.Parse(kkk);
                    }
                    else
                    {
                        kk = 0;
                    }
                    labels[x].Size = new Size(374, 277);//大小
                    labels[x].BorderStyle = BorderStyle.None;
                    if (rect.Width > 1300 && rect.Width < 1500)
                    {
                        labels[x].Margin = new Padding(30, 10, 0, 0);
                    }

                    labels[x].Cursor = Cursors.Hand;
                    labels[x].Click += new EventHandler(lable_Click);
                    labels[x].Font = new Font(privateFonts.Families[0], 15.0F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    labels[x].TextAlign = ContentAlignment.TopLeft;
                    labels[x].Image = Image.FromFile(@str + "/images/fangzi2.png");

                    DataTable dt1 = rs.queryMonitoringByhousecode(dt.Rows[x]["id"].ToString());
                    int cdCount = dt1.Rows.Count;
                    if (cdCount > 0)
                    {
                        for (int j = 0; j < cdCount; j++)
                        {
                            string wd1 = dt1.Rows[j]["temperature"].ToString();
                            string sd1 = dt1.Rows[j]["humidity"].ToString();
                            if (wd1 == "" || "".Equals(wd1) || wd1 == null)
                            {
                                wd1 = "0";
                            }
                            if (sd1 == "" || "".Equals(sd1) || sd1 == null)
                            {
                                sd1 = "0";
                            }
                            double wd2 = Double.Parse(wd1);
                            double sd2 = Double.Parse(sd1);
                            double a = Double.Parse(dt1.Rows[j]["t_high"].ToString());
                            double b = Double.Parse(dt1.Rows[j]["t_low"].ToString());
                            double c = Double.Parse(dt1.Rows[j]["h_high"].ToString());
                            double d = Double.Parse(dt1.Rows[j]["h_low"].ToString());
                            string times = dt1.Rows[j]["devtime"].ToString();
                            if (times == "" || "".Equals(times) || times == null)
                            {
                                times = "1990-01-01 00:00:00";
                            }
                            DateTime dtime1 = Convert.ToDateTime(times);
                            DateTime dtime2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            TimeSpan ts = dtime2.Subtract(dtime1);

                            if (ts.TotalMinutes >= double.Parse(overtime) || wd2 == 0 && sd2 == 0)
                            {
                                //flag = 1;
                                labels[x].Text = "\n" + "\n" + "\n" + "\n" + "         库房名称：" + dt.Rows[x]["name"].ToString() + "\n" + "\n" + "         平均温度：- - ℃" + "\n" + "\n" + "         平均湿度：- -%RH" + "\n" + "\n" + "         测点个数：  " + cdCount + " 个";
                                labels[x].Image = Image.FromFile(@str + "/images/fangzi.png");
                                continue;
                            }
                            else
                            {

                                if (wd1 != null && !"".Equals(wd1) && !"0".Equals(wd1) && double.Parse(wd1) != 0)
                                {
                                    wd += double.Parse(wd1);
                                    wdnum += 1;
                                }
                                if (sd1 != null && !"".Equals(sd1) && !"0".Equals(sd1) && double.Parse(sd1) != 0)
                                {
                                    sd += double.Parse(sd1);
                                    sdnum += 1;
                                }

                                avgwd = Math.Round(wd / wdnum, 1);
                                avgsd = Math.Round(sd / sdnum, 1);

                                if (wdnum != 0 && sdnum != 0)
                                {
                                    labels[x].Text = "\n" + "\n" + "\n" + "\n" + "         库房名称：" + dt.Rows[x]["name"].ToString() + "\n" + "\n" + "         平均温度：" + avgwd + "℃" + "\n" + "\n" + "         平均湿度：" + avgsd + "%RH" + "\n" + "\n" + "         测点个数：  " + cdCount + " 个";
                                    if (wd2 > a || wd2 < b || sd2 > c || sd2 < d)
                                    {
                                        flag = 1;
                                    }
                                }
                                else
                                {

                                    if (wdnum == 0 && sdnum == 0)
                                    {
                                        labels[x].Text = "\n" + "\n" + "\n" + "\n" + "         库房名称：" + dt.Rows[x]["name"].ToString() + "\n" + "\n" + "         平均温度：- - ℃" + "\n" + "\n" + "         平均湿度：- -%RH" + "\n" + "\n" + "         测点个数：  " + cdCount + " 个";
                                        labels[x].Image = Image.FromFile(@str + "/images/fangzi.png");
                                    }
                                    else
                                    {
                                        if (wdnum == 0)
                                        {
                                            labels[x].Text = "\n" + "\n" + "\n" + "\n" + "         库房名称：" + dt.Rows[x]["name"].ToString() + "\n" + "\n" + "         平均温度：- - ℃" + "\n" + "\n" + "         平均湿度：" + avgsd + "%RH" + "\n" + "\n" + "         测点个数：  " + cdCount + " 个";
                                            labels[x].Image = Image.FromFile(@str + "/images/fangzi2.png");
                                            if (sd2 > c || sd2 < d)
                                            {
                                                flag = 2;
                                            }
                                        }
                                        if (sdnum == 0)
                                        {
                                            labels[x].Text = "\n" + "\n" + "\n" + "\n" + "         库房名称：" + dt.Rows[x]["name"].ToString() + "\n" + "\n" + "         平均温度：" + avgwd + "℃" + "\n" + "\n" + "         平均湿度：- -%RH" + "\n" + "\n" + "         测点个数：  " + cdCount + " 个";
                                            labels[x].Image = Image.FromFile(@str + "/images/fangzi2.png");
                                            if (wd2 > a || wd2 < b)
                                            {
                                                flag = 3;
                                            }
                                        }
                                    }


                                }

                            }
                        }

                    }
                    else
                    {
                        labels[x].Text = "\n" + "\n" + "\n" + "\n" + "         库房名称：" + dt.Rows[x]["name"].ToString() + "\n" + "\n" + "         平均温度：- - ℃" + "\n" + "\n" + "         平均湿度：- -%RH" + "\n" + "\n" + "         测点个数：  " + cdCount + " 个";
                        labels[x].Image = Image.FromFile(@str + "/images/fangzi.png");
                    }
                    if (flag > 0)
                    {
                        labels[x].Image = Image.FromFile(@str + "/images/fangzi3.png");
                    }
                    flag = 0;
                    if (kk == 1)
                    {
                        labels[x].Image = Image.FromFile(@str + "/images/kongku.png");
                    }
                    this.flowLayoutPanel1.Controls.Add(labels[x]);
                    wd = 0;
                    sd = 0;
                    wdnum = 0;
                    sdnum = 0;
                }
            }
        }
        bool isbjShow;
        Image newImage = Image.FromFile(@str + "/images/ico06.png");
        Bitmap bit;
        Graphics g;
        Font font;
        Size size;
        Rectangle r;
        StringFormat format;
        private void lable_Click(object sender, EventArgs e)
        {
            format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            int kk = 0;
            this.button2.Visible = true;
            Label lable= (Label)sender;
            string id= lable.Tag.ToString().Split('-')[0].ToString();
            this.label1.Text = lable.Tag.ToString().Split('-')[1].ToString() + "库房测点列表";
            this.label1.Left = (this.Width - this.label1.Width) / 2;
            if (id!=null&&!"".Equals(id)) {
                dt = rs.queryMonitoringByhousecode(id);
                int  num = dt.Rows.Count;
            if (num > 0)
            {
                houseid = id;
                flowLayoutPanel1.Controls.Clear();
                picb = new PictureBox[num];
                string measureMeterCode=null;
                for (int x = 0; x < num; x++)
                {
                    isbjShow = false;
                    picb[x] = new PictureBox();
                    picb[x].SizeMode = PictureBoxSizeMode.StretchImage;
                    picb[x].DoubleClick += new EventHandler(picb_DouClick);
                    measureMeterCode = dt.Rows[x]["measureMeterCode"].ToString();  
                    picb[x].Tag = dt.Rows[x]["terminalname"].ToString() + "," + dt.Rows[x]["devtime"].ToString() + "," + dt.Rows[x]["measureCode"].ToString() + "," + dt.Rows[x]["meterNo"].ToString();
                    picb[x].BorderStyle = BorderStyle.None;
                    picb[x].Size = new Size(autoSizeX, autoSizeY);//大小
                    double aa = rect.Width % picb[x].Size.Width;
                    double bb = rect.Width / picb[x].Size.Width;
                    int cc = Convert.ToInt32(aa / bb) - 2;
                    picb[x].Margin = new Padding(cc, 0, 0, 0);
                    System.GC.Collect();
                    if (measureMeterCode.Length!=0)
                    {
                        bit = new Bitmap(@str + "/images/cd.png");
                        g = Graphics.FromImage(bit);
                        picb[x].Name = measureMeterCode;
                        
                        a = Double.Parse(dt.Rows[x]["temperature"].ToString());
                        b = Double.Parse(dt.Rows[x]["t_high"].ToString());
                        c = Double.Parse(dt.Rows[x]["t_low"].ToString());
                        d = Double.Parse(dt.Rows[x]["humidity"].ToString());
                        e1 = Double.Parse(dt.Rows[x]["h_high"].ToString());
                        f = Double.Parse(dt.Rows[x]["h_low"].ToString());
                        string cdname = dt.Rows[x]["terminalname"].ToString();
                        string kkk = dt.Rows[x]["housetype"].ToString();
                        if (kkk != null && !"".Equals(kkk))
                        {
                            kk = Int32.Parse(kkk);
                        }
                        else
                        {
                            kk = 0;
                        }
                        font = new Font("微软雅黑", Convert.ToSingle((double)10 * 5.0));
                        size = TextRenderer.MeasureText(b.ToString("0.0"), font);
                        r = new Rectangle(680, 170, size.Width, size.Height);
                        g.DrawString(b.ToString("0.0"), font, Brushes.White, r, format);
                        size = TextRenderer.MeasureText(c.ToString("0.0"), font);
                        r = new Rectangle(680, 260, size.Width, size.Height);
                        g.DrawString(c.ToString("0.0"), font, Brushes.White, r, format);
                        size = TextRenderer.MeasureText(e1.ToString("0.0"), font);
                        r = new Rectangle(680, 405, size.Width, size.Height);
                        g.DrawString(e1.ToString("0.0"), font, Brushes.White, r, format);
                        size = TextRenderer.MeasureText(f.ToString("0.0"), font);
                        r = new Rectangle(680, 495, size.Width, size.Height);
                        g.DrawString(f.ToString("0.0"), font, Brushes.White, r, format);

                            font = new Font("微软雅黑", Convert.ToSingle((double)12 * 5.0));
                            size = TextRenderer.MeasureText(cdname, font);
                            r = new Rectangle((bit.Width - size.Width) / 2, 20, size.Width + 10, size.Height);
                            g.DrawString(cdname, font, Brushes.White, r, format);
                            if (kk == 1)
                            {
                                size = TextRenderer.MeasureText("空库", font);
                                r = new Rectangle(-20, 530, size.Width, size.Height);
                                g.DrawString("空库", font, Brushes.White, r, format);
                            }

                            DateTime dt1 = Convert.ToDateTime(dt.Rows[x]["devtime"].ToString());
                            DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            TimeSpan ts = dt2.Subtract(dt1);

                        if (ts.TotalMinutes >= double.Parse(overtime))
                        {
                                font = new Font("微软雅黑", Convert.ToSingle((double)20 * 5.0));
                                size = TextRenderer.MeasureText("- -      ", font);
                                r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                                g.DrawString("- -      ", font, Brushes.Gray, r, format);

                                size = TextRenderer.MeasureText("- -    ", font);
                                r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                                g.DrawString("- -    ", font, Brushes.Gray, r, format);
                            }
                        else
                        {
                                font = new Font("微软雅黑", Convert.ToSingle((double)20 * 5.0));
                                if (a != 0 && d != 0)
                                {
                                    size = TextRenderer.MeasureText(a.ToString("0.0") + "     ", font);
                                    r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                                    if (kk == 1)
                                    {
                                        g.DrawString(a.ToString("0.0") + "     ", font, Brushes.Gray, r, format);
                                        isbjShow = false;
                                    }
                                    else
                                    {
                                        if (a > b || a < c)
                                        {
                                            g.DrawString(a.ToString("0.0") + "     ", font, Brushes.Red, r, format);
                                            isbjShow = true;
                                        }
                                        else
                                        {
                                            g.DrawString(a.ToString("0.0") + "     ", font, Brushes.Blue, r, format);
                                        }
                                    }
                                    size = TextRenderer.MeasureText(d.ToString("0.0") + "    ", font);
                                    r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                                    if (kk == 1)
                                    {
                                        g.DrawString(d.ToString("0.0") + "    ", font, Brushes.Gray, r, format);
                                        isbjShow = false;
                                    }
                                    else
                                    {
                                        if (d > e1 || d < f)
                                        {
                                            g.DrawString(d.ToString("0.0") + "    ", font, Brushes.Red, r, format);
                                            isbjShow = true;
                                        }
                                        else
                                        {
                                            g.DrawString(d.ToString("0.0") + "    ", font, Brushes.Blue, r, format);
                                        }
                                    }
                                }
                                else
                                {
                                    font = new Font("微软雅黑", Convert.ToSingle((double)20 * 5.0));
                                    if (a == 0 && d == 0)
                                    {
                                        size = TextRenderer.MeasureText("0.0      ", font);
                                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                                        if (a > b || a < c)
                                        {
                                            g.DrawString("0.0      ", font, Brushes.Red, r, format);
                                            isbjShow =true;
                                        }
                                        else
                                        {
                                            g.DrawString("0.0      ", font, Brushes.Blue, r, format);
                                            isbjShow = false;
                                        }
                                        size = TextRenderer.MeasureText("0.0      ", font);
                                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                                        if (d > e1 || d < f)
                                        {
                                            g.DrawString("0.0      ", font, Brushes.Red, r, format);
                                            isbjShow = true;
                                        }
                                        else
                                        {
                                            g.DrawString("0.0      ", font, Brushes.Blue, r, format);
                                            isbjShow = true;
                                        }
                                    }
                                    else {
                                    if (a == 0)
                                    {
                                        size = TextRenderer.MeasureText("0.0      ", font);
                                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                                        g.DrawString("0.0      ", font, Brushes.Gray, r, format);
                                        size = TextRenderer.MeasureText(d.ToString("0.0") + "    ", font);
                                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                                        if (d > e1 || d < f)
                                        {
                                            if (kk == 1)
                                            {
                                                g.DrawString(d.ToString("0.0") + "    ", font, Brushes.Gray, r, format);
                                                isbjShow = false;
                                            }
                                            else
                                            {
                                                g.DrawString(d.ToString("0.0") + "    ", font, Brushes.Red, r, format);
                                                isbjShow = true;
                                            }
                                        }
                                        else
                                        {
                                            if (kk == 1)
                                            {
                                                g.DrawString(d.ToString("0.0") + "    ", font, Brushes.Gray, r, format);
                                                isbjShow = false;
                                            }
                                            else
                                            {
                                                g.DrawString(d.ToString("0.0") + "    ", font, Brushes.Blue, r, format);
                                            }
                                        }
                                    }

                                    if (d == 0)
                                    {
                                        size = TextRenderer.MeasureText("0.0    ", font);
                                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                                        g.DrawString("0.0    ", font, Brushes.Gray, r, format);
                                        size = TextRenderer.MeasureText(a.ToString("0.0") + "     ", font);
                                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                                        if (a > b || a < c)
                                        {
                                            if (kk == 1)
                                            {
                                                g.DrawString(a.ToString("0.0") + "     ", font, Brushes.Gray, r, format);
                                                isbjShow = false;
                                            }
                                            else
                                            {
                                                g.DrawString(a.ToString("0.0") + "     ", font, Brushes.Red, r, format);
                                                isbjShow = true;
                                            }
                                        }
                                        else
                                        {
                                            if (kk == 1)
                                            {
                                                g.DrawString(a.ToString("0.0") + "     ", font, Brushes.Gray, r, format);
                                                isbjShow = false;
                                            }
                                            else
                                            {
                                                g.DrawString(a.ToString("0.0") + "     ", font, Brushes.Blue, r, format);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                        if (isbjShow) {
                            //右下角报警图标
                            r = new Rectangle(740, 490, newImage.Width, newImage.Height);
                            g.DrawImage(newImage, r.Right, r.Bottom, newImage.Width, newImage.Height);
                            picb[x].MouseClick += new MouseEventHandler(picb_MouseClick);
                        }
                        picb[x].Image = bit;
                        g.Dispose();
                        g = null;
                    }
                    else {
                        
                        string cdname = dt.Rows[x]["terminalname"].ToString();
                        b = Double.Parse(dt.Rows[x]["t_high"].ToString());
                        c = Double.Parse(dt.Rows[x]["t_low"].ToString());
                        e1 = Double.Parse(dt.Rows[x]["h_high"].ToString());
                        f = Double.Parse(dt.Rows[x]["h_low"].ToString());
                       
                        bit = new Bitmap(@str + "/images/cd.png");
                        g = Graphics.FromImage(bit);

                            font = new Font("微软雅黑", Convert.ToSingle((double)10 * 5.0));
                            size = TextRenderer.MeasureText(b.ToString("0.0"), font);
                            r = new Rectangle(680, 170, size.Width, size.Height);
                            g.DrawString(b.ToString("0.0"), font, Brushes.White, r, format);
                            size = TextRenderer.MeasureText(c.ToString("0.0"), font);
                            r = new Rectangle(680, 260, size.Width, size.Height);
                            g.DrawString(c.ToString("0.0"), font, Brushes.White, r, format);
                            size = TextRenderer.MeasureText(e1.ToString("0.0"), font);
                            r = new Rectangle(680, 405, size.Width, size.Height);
                            g.DrawString(e1.ToString("0.0"), font, Brushes.White, r, format);
                            size = TextRenderer.MeasureText(f.ToString("0.0"), font);
                            r = new Rectangle(680, 495, size.Width, size.Height);
                            g.DrawString(f.ToString("0.0"), font, Brushes.White, r, format);

                            font = new Font("微软雅黑", Convert.ToSingle((double)12 * 5.0));
                            size = TextRenderer.MeasureText(cdname, font);
                            r = new Rectangle((bit.Width - size.Width) / 2, 20, size.Width + 10, size.Height);
                            g.DrawString(cdname, font, Brushes.White, r, format);

                            font = new Font("微软雅黑", Convert.ToSingle((double)20 * 5.0));
                            size = TextRenderer.MeasureText("- -      ", font);
                            r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                            g.DrawString("- -      ", font, Brushes.Gray, r, format);

                            size = TextRenderer.MeasureText("- -    ", font);
                            r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                            g.DrawString("- -    ", font, Brushes.Gray, r, format);

                            picb[x].Image = bit;
                            g.Dispose();
                            g = null;
                        }
                   
                    this.flowLayoutPanel1.Controls.Add(picb[x]);
                    }
                }
            }
            this.timer1.Interval= Int32.Parse(datarefreshtime) * 1000;
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int kk = 0;
            DataTable dt = rs.queryMonitoringByhousecode(houseid);
            int num = dt.Rows.Count;
            if (num < 1) { return; }
            String codemeter;
            String code;
            String meter;
            String terminalname;
            String temperature;
            String humidity;
            String t_high;
            String t_low;
            String h_high;
            String h_low;
            String dtime;
            Graphics gg;

            font = new Font("微软雅黑", Convert.ToSingle((double)20 * 5.0));
            format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            for (int i = 0; i < num; i++)
            {
                isbjShow = false;
                code = dt.Rows[i]["measureCode"].ToString();
                meter = dt.Rows[i]["meterNo"].ToString();
                codemeter = dt.Rows[i]["measureMeterCode"].ToString();
                terminalname = dt.Rows[i]["terminalname"].ToString();
                temperature = dt.Rows[i]["temperature"].ToString();
                humidity = dt.Rows[i]["humidity"].ToString();
                t_high = dt.Rows[i]["t_high"].ToString();
                t_low = dt.Rows[i]["t_low"].ToString();
                h_high = dt.Rows[i]["h_high"].ToString();
                h_low = dt.Rows[i]["h_low"].ToString();
                System.GC.Collect();
                if (codemeter.Length != 0)
                {
                    bit = new Bitmap(@str + "/images/cd.png");
                    gg = Graphics.FromImage(bit);
                    try
                    {
                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).Refresh();
                    }
                    catch
                    {
                        continue;
                    }
                    a = Double.Parse(temperature);
                    b = Double.Parse(t_high);
                    c = Double.Parse(t_low);
                    d = Double.Parse(humidity);
                    e1 = Double.Parse(h_high);
                    f = Double.Parse(h_low);
                    string kkk = dt.Rows[i]["housetype"].ToString();
                    if (kkk != null && !"".Equals(kkk))
                    {
                        kk = Int32.Parse(kkk);
                    }
                    else
                    {
                        kk = 0;
                    }

                    dtime = dt.Rows[i]["devtime"].ToString();

                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).Tag = terminalname + "," + dtime + "," + code + "," + meter;

                    font = new Font("微软雅黑", Convert.ToSingle((double)10 * 5.0));
                    size = TextRenderer.MeasureText(b.ToString("0.0"), font);
                    r = new Rectangle(680, 170, size.Width, size.Height);
                    gg.DrawString(b.ToString("0.0"), font, Brushes.White, r, format);
                    size = TextRenderer.MeasureText(c.ToString("0.0"), font);
                    r = new Rectangle(680, 260, size.Width, size.Height);
                    gg.DrawString(c.ToString("0.0"), font, Brushes.White, r, format);
                    size = TextRenderer.MeasureText(e1.ToString("0.0"), font);
                    r = new Rectangle(680, 405, size.Width, size.Height);
                    gg.DrawString(e1.ToString("0.0"), font, Brushes.White, r, format);
                    size = TextRenderer.MeasureText(f.ToString("0.0"), font);
                    r = new Rectangle(680, 495, size.Width, size.Height);
                    gg.DrawString(f.ToString("0.0"), font, Brushes.White, r, format);

                    font = new Font("微软雅黑", Convert.ToSingle((double)12 * 5.0));
                    size = TextRenderer.MeasureText(terminalname, font);
                    r = new Rectangle((bit.Width - size.Width) / 2, 20, size.Width + 10, size.Height);
                    gg.DrawString(terminalname, font, Brushes.White, r, format);
                    if (kk == 1)
                    {
                        size = TextRenderer.MeasureText("空库", font);
                        r = new Rectangle(-20, 530, size.Width, size.Height);
                        gg.DrawString("空库", font, Brushes.White, r, format);
                    }
                    font = new Font("微软雅黑", Convert.ToSingle((double)20 * 5.0));
                    DateTime dt1 = Convert.ToDateTime(dtime);
                    DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    TimeSpan ts = dt2.Subtract(dt1);
                    if (ts.TotalMinutes >= double.Parse(overtime))
                    {
                        size = TextRenderer.MeasureText("- -      ", font);
                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                        gg.DrawString("- -      ", font, Brushes.Gray, r, format);

                        size = TextRenderer.MeasureText("- -    ", font);
                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                        gg.DrawString("- -    ", font, Brushes.Gray, r, format);

                    }
                    else
                    {
                        if (a != 0 && d != 0)
                        {
                            if (kk != 1)
                            {
                                if (a > b || a < c || d > e1 || d < f)
                                {
                                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                    r = new Rectangle(740, 490, newImage.Width, newImage.Height);
                                    gg.DrawImage(newImage, r.Right, r.Bottom, newImage.Width, newImage.Height);
                                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick += new MouseEventHandler(picb_MouseClick);
                                }
                                else
                                {
                                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                }
                            }
                            size = TextRenderer.MeasureText(a.ToString("0.0") + "     ", font);
                            r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);

                            if (kk == 1)
                            {
                                gg.DrawString(a.ToString("0.0") + "     ", font, Brushes.Gray, r, format);
                                ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                            }
                            else
                            {
                                if (a > b || a < c)
                                {
                                    gg.DrawString(a.ToString("0.0") + "     ", font, Brushes.Red, r, format);
                                }
                                else
                                {
                                    gg.DrawString(a.ToString("0.0") + "     ", font, Brushes.Blue, r, format);
                                }
                            }

                            size = TextRenderer.MeasureText(d.ToString("0.0") + "    ", font);
                            r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);

                            if (kk == 1)
                            {
                                gg.DrawString(d.ToString("0.0") + "    ", font, Brushes.Gray, r, format);
                                ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                            }
                            else
                            {
                                if (d > e1 || d < f)
                                {
                                    gg.DrawString(d.ToString("0.0") + "    ", font, Brushes.Red, r, format);
                                }
                                else
                                {
                                    gg.DrawString(d.ToString("0.0") + "    ", font, Brushes.Blue, r, format);
                                }
                            }
                        }
                        else
                        {
                            if (a == 0 && d == 0)
                            {
                                size = TextRenderer.MeasureText("0.0      ", font);
                                r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                                if (a > b || a < c)
                                {
                                    gg.DrawString("0.0      ", font, Brushes.Red, r, format);
                                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                    r = new Rectangle(740, 490, newImage.Width, newImage.Height);
                                    gg.DrawImage(newImage, r.Right, r.Bottom, newImage.Width, newImage.Height);
                                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick += new MouseEventHandler(picb_MouseClick);
                                }
                                else
                                {
                                    gg.DrawString("0.0      ", font, Brushes.Blue, r, format);
                                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                }
                                size = TextRenderer.MeasureText("0.0      ", font);
                                r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                                if (d > e1 || d < f)
                                {
                                    gg.DrawString("0.0      ", font, Brushes.Red, r, format);
                                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                    r = new Rectangle(740, 490, newImage.Width, newImage.Height);
                                    gg.DrawImage(newImage, r.Right, r.Bottom, newImage.Width, newImage.Height);
                                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick += new MouseEventHandler(picb_MouseClick);
                                }
                                else
                                {
                                    gg.DrawString("0.0      ", font, Brushes.Blue, r, format);
                                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                }
                            }
                            else {
                            if (a == 0)
                            {
                                size = TextRenderer.MeasureText("0.0      ", font);
                                r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                                gg.DrawString("0.0      ", font, Brushes.Gray, r, format);
                            }
                            else
                            {
                                size = TextRenderer.MeasureText(a.ToString("0.0") + "      ", font);
                                r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                                if (a > b || a < c)
                                {
                                    if (kk == 1)
                                    {
                                        gg.DrawString(a.ToString("0.0") + "      ", font, Brushes.Gray, r, format);
                                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                    }
                                    else
                                    {
                                        gg.DrawString(a.ToString("0.0") + "      ", font, Brushes.Red, r, format);

                                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                        r = new Rectangle(740, 490, newImage.Width, newImage.Height);
                                        gg.DrawImage(newImage, r.Right, r.Bottom, newImage.Width, newImage.Height);
                                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick += new MouseEventHandler(picb_MouseClick);
                                    }
                                }
                                else
                                {
                                    if (kk == 1)
                                    {
                                        gg.DrawString(a.ToString("0.0") + "      ", font, Brushes.Gray, r, format);
                                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                    }
                                    else
                                    {
                                        gg.DrawString(a.ToString("0.0") + "      ", font, Brushes.Blue, r, format);
                                    }
                                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                }
                            }
                            if (d == 0)
                            {
                                size = TextRenderer.MeasureText("0.0    ", font);
                                r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                                gg.DrawString("0.0    ", font, Brushes.Gray, r, format);
                            }
                            else
                            {
                                size = TextRenderer.MeasureText(d.ToString("0.0") + "- -    ", font);
                                r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                                if (d > e1 || d < f)
                                {
                                    if (kk == 1)
                                    {
                                        gg.DrawString(d.ToString("0.0") + "- -    ", font, Brushes.Gray, r, format);
                                    }
                                    else
                                    {
                                        gg.DrawString(d.ToString("0.0") + "- -    ", font, Brushes.Red, r, format);

                                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                        r = new Rectangle(740, 490, newImage.Width, newImage.Height);
                                        gg.DrawImage(newImage, r.Right, r.Bottom, newImage.Width, newImage.Height);
                                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick += new MouseEventHandler(picb_MouseClick);
                                    }
                                }
                                else
                                {
                                    if (kk == 1)
                                    {
                                        gg.DrawString(d.ToString("0.0") + "     ", font, Brushes.Gray, r, format);
                                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                    }
                                    else
                                    {
                                        gg.DrawString(d.ToString("0.0") + "     ", font, Brushes.Blue, r, format);
                                    }
                                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                                }
                            }
                        }
                        }
                    }
                    ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).Image = bit;
                    gg.Dispose();
                    gg = null;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.flowLayoutPanel1.Controls.Clear();
            houseCheckInfo_Load(sender,e);
            this.button2.Visible = false;
        }

        private void flowLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Focus();
        }
        private void picb_DouClick(object sender, EventArgs e)
        {
            service.deviceInformationService dis = new service.deviceInformationService();
            convenientSet cs = new convenientSet();
            PictureBox pic = (PictureBox)sender;
            string tag = pic.Tag.ToString();
            string[] tagg = tag.Split(',');
            DataTable dtd = dis.selectBydeviceInfo(tagg[2], tagg[3]);
            if (dtd.Rows.Count > 0)
            {
                cs.textBox3.Text = dtd.Rows[0]["terminalname"].ToString();
                cs.textBox4.Text = dtd.Rows[0]["measureCode"].ToString() + "-" + dtd.Rows[0]["meterNo"].ToString();
                string hostaddress = dtd.Rows[0]["hostAddress"].ToString();
                if (hostaddress != null && !"".Equals(hostaddress))
                {
                    cs.radioButton1.Checked = true;
                    cs.comboBox5.Enabled = true;
                    cs.comboBox5.Text = "COM" + hostaddress;
                }
                cs.comboBox3.Text = dtd.Rows[0]["CommunicationType"].ToString();
                cs.numericUpDown4.Value = (decimal)Convert.ToDouble(dtd.Rows[0]["t_high"].ToString());
                cs.numericUpDown2.Value = (decimal)Convert.ToDouble(dtd.Rows[0]["t_low"].ToString());
                cs.numericUpDown1.Value = (decimal)Convert.ToDouble(dtd.Rows[0]["h_high"].ToString());
                cs.numericUpDown5.Value = (decimal)Convert.ToDouble(dtd.Rows[0]["h_low"].ToString());
                if (cs.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("测点信息修改成功！");
                    this.timer1.Stop();
                    timer1_Tick(null, null);
                    this.timer1.Start();
                }
            }
        }
        private void picb_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                PictureBox pic = (PictureBox)sender;
                string tag = pic.Tag.ToString();
                string name = pic.Name;
                string[] tagg = tag.Split(',');

                warningHandle wh = new warningHandle();
                wh.Text = tagg[0] + "报警处理";
                wh.label2.Text = frmLogin.name;
                wh.textBox1.Text = tagg[1];
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                wh.dateTimePicker1.Value = Convert.ToDateTime(time);
                wh.textBox5.Text = name;
                wh.ShowDialog();
            }
        }

        private void getFromXmldatarefreshtime()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node = xmlDoc.SelectSingleNode("config/datarefreshtime");
            datarefreshtime = node.InnerText;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            houseTypeInfo();
        }
    }
}
