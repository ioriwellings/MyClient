using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Web.Script.Serialization;
using System.IO.Ports;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Drawing.Text;
using System.Diagnostics;
using NT88Test;
using SmartX1Demo;
using System.Collections;
using System.Net;
using System.Net.Sockets;
namespace LBKJClient
{
    public partial class frmMain : Form
    {
        public delegate void MyDelegate();
        public static string mids = null;
        public static string ipport = null;
        public static bool istrueport = true;
        public int hulie = 1;
        private int logrizhi = 0;
        private int socketserver = 0;
        public delegate void UpdateAcceptTextBoxTextHandler(string text);
        public UpdateAcceptTextBoxTextHandler UpdateTextHandler;
        public delegate void SendDeviceInfoHandler(string text);
        public SerialPort port = new SerialPort();
        Byte[] totalByteRead = new Byte[0];
        string text = string.Empty;
        string zdfilename = null;
        string sdfilename = null;
        string overtime = null;
        string datarefreshtime = null;
        string datasavetime = null;
        JavaScriptSerializer js = new JavaScriptSerializer();
        List<bean.dataSerialization> list;
        String jsons;
        PrivateFontCollection privateFonts = new PrivateFontCollection();

        public delegate void invokeDisplay(string str);
        int autoSizeX = 180;
        int autoSizeY = 155;
        Rectangle rect = new Rectangle();
        basicSetup basicsetup = new basicSetup();//基本设置功能
        historydata historyd = new historydata();//历史数据功能
        graphCheck graphcheck = new graphCheck();
        service.deviceInformationService dis = new service.deviceInformationService();
        service.rtmonitoringService monitoringservice = new service.rtmonitoringService();
        service.manageHostService mhs = new service.manageHostService();//查询GZ02有串口地址的数据
        dataBackUpSet sb = new dataBackUpSet();//数据备份
        service.showReportService lls = new service.showReportService();
        Socket sk;//连接用Socket
        DataSet da;
        DataTable dt;
        DataTable dtcdinfo;
        DataTable dtcdinfo1 = null;
        PictureBox[] picb;
        private int num = 0;
        private double a = 0;
        private double b = 0;
        private double c = 0;
        private double d = 0;
        private double e1 = 0;
        private double f = 0;

        private static string str = Application.StartupPath;//项目路径
        public static string companyName = "XXX";
        private string titlename = "温湿度监控系统";
        private string filepath = "/companynemInfo.txt";
        private string filepath1 = "/titleInfo.txt";
        string cm = null;
        string tm = null;
        int times = 0;
        public static int cartime = 0;
        public static int housetime = 0;
        public static int autosave02 = 0;
        public static int autosave16 = 0;
        //变量声明
        private string username;
        private string password;
        private string path = @"config.xml";
        XmlDocument xmlDoc = new XmlDocument();
        string stoptime, starttime, stoptime1, starttime1;
        private string getresults;
        private string measureCode;
        private DataTable dtCom;
        private string[] result;
        private int comnum = 0;
        public frmMain()
        {
            InitializeComponent();
         }
        private void frmMain_Load(object sender, EventArgs e)
        {
            if (hulie == 1)
            {
                this.timer1.Start();
                string power = "显示报告,库房平面图,分库浏览,库房管理,管理主机设置,数据库管理,查询登录日志,查询报警处理,查询报警记录,查询历史曲线,查询历史数据,密码修改,测点属性设置,报警设置,基本设置,修改标题,修改公司名称,用户管理";
                frmLogin.listpower= power.Split(',').ToList();
            }
            if (frmLogin.name!="admin") {
            foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            {
                if (item.Text != "帮助")
                {
                    //遍历前三个中的子菜单
                    foreach (ToolStripMenuItem item2 in item.DropDownItems)
                    {   
                        if (item2.Text!="退出系统" && item2.Text != "取消自动登录" && item2.Text != "菜单栏" && item2.Text != "工具栏" && item2.Text != "标题栏" && item2.Text != "全屏显示(Esc退出)" && item2.Text != "缩放") {
                            //权限中不包含这个下拉菜单
                            if (!frmLogin.listpower.Contains(item2.Text))
                            {
                                item2.Visible = false; //看不见
                            }
                            else
                            {
                                item2.Visible = true; //看见 （可用）
                            }
                        }
                    }
                }
            }
            foreach (ToolStripLabel item in this.toolStrip1.Items)
            {
                    if (item.Text != "系统退出")
                    {
                        if (!frmLogin.listpower.Contains(item.Text))
                        {
                            item.Visible = false; //看不见
                        }
                        else
                        {
                            item.Visible = true; //看见 （可用）
                        }
                    }
             }
          }
            privateFonts.AddFontFile(@str + @"/fonts/SIMYOU.TTF");//加载路径的字体
            getFromXml();
            rect = Screen.GetWorkingArea(this);
            initPointsInfo();
            overtime = Properties.Settings.Default.overTime;
            //获取通信类别
            getFromXmlcommunication();
            dtcdinfo1 = dis.checkPointInfo(0);
            if (getresults != null && !"".Equals(getresults))
            {
                result = getresults.Split('-');
                if (result[0] == "1")
                {
                    dtCom = mhs.queryManageHostCom();               
                }
            }
            queryMeterIds();

            sdfilename = sb.textBox1.Text;
            zdfilename = sb.textBox2.Text;
            //以下是标题的操作
            cm = textFileUpdate(@str + filepath);
            tm = textFileUpdate(@str + filepath1);
            if (cm != null && !"".Equals(cm)) {
                companyName = cm;
            }
            else {
                textFile(@str + filepath, companyName);
            }
            if (tm != null && !"".Equals(tm))
            {
                titlename = tm;
            }
            else {
                textFile(@str + filepath1, titlename);
            }
      
            this.lblTitle.Text = companyName + titlename;
            this.lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Font = new Font("微软雅黑", 26F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            lblTitle.Left = (this.lbtitle.Width - this.lblTitle.Width) / 2;
            lblTitle.BringToFront();
          //  backupDatabase();//数据库自动备份
            this.timer2.Interval = Int32.Parse(datasavetime) * 1000;
            this.timer2.Start();
        }
        private void queryMeterIds()
        {
            string ids = null;
            int flag = 3;
            dtcdinfo = dis.checkPointInfo(flag);
            if (dtcdinfo.Rows.Count>0) {
                for (int i=0; i< dtcdinfo.Rows.Count; i++) {
                    ids+=":"+ dtcdinfo.Rows[i][1] + "-" + dtcdinfo.Rows[i][2]; 
                }
                mids=ids.Substring(1);
            }         
                if (dtcdinfo1.Rows.Count>0)
            {
                //首页-查询测点的实时温湿度数据              
                querywenshidu();
            }
            else {
                flowLayoutPanel1.Controls.Clear();
            }
        }
        private void getFromXmlcommunication()
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/communicationType");
            getresults = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/datarefreshtime");
            datarefreshtime = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/datasavetime");
            datasavetime = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/databasetimer");
            times =Int32.Parse( node.InnerText);
            node = xmlDoc.SelectSingleNode("config/carsavetime");
            cartime = Int32.Parse(node.InnerText);
            node = xmlDoc.SelectSingleNode("config/datahousesavetime");
            housetime = Int32.Parse(node.InnerText);
            node = xmlDoc.SelectSingleNode("config/log");
            logrizhi = Int32.Parse(node.InnerText);
        }
        private bool initPort(string com)
        {
            try
            {
                if (!port.IsOpen)
                {
                    string portName = com;
                    int baudRate = 9600;

                    port.PortName = portName;
                    port.BaudRate = baudRate;
                    port.DtrEnable = true;
                    port.ReceivedBytesThreshold = 1;
                    port.Open();
                    rb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    rb.eventInfo = "串口连接成功！";
                    rb.type = "0";
                    lls.addReport(rb);
                    return true;
                }
                else {
                    rb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    rb.eventInfo = "串口连接失败！";
                    rb.type = "0";
                    lls.addReport(rb);
                    return false;
                }
        }catch{
                port.Close();
                rb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                rb.eventInfo = "串口连接失败了！";
                rb.type = "0";
                lls.addReport(rb);
                return false;
            }
        

        }

        private void initPointsInfo()
        {
            默认大小ToolStripMenuItem.CheckState = CheckState.Unchecked;
            toolStripMenuItem2.CheckState = CheckState.Unchecked;
            toolStripMenuItem3.CheckState = CheckState.Unchecked;
            toolStripMenuItem4.CheckState = CheckState.Unchecked;
            toolStripMenuItem5.CheckState = CheckState.Unchecked;
            toolStripMenuItem6.CheckState = CheckState.Unchecked;
            
        }

        bool isbjShow;
        Image newImage = Image.FromFile(@str + "/images/ico06.png");
        Image ddbjImage = Image.FromFile(@str + "/images/ddbj.png");
        Bitmap bit;
        Graphics g;
        Font font;
        Size size;
        Rectangle r;
        StringFormat format;
        private void querywenshidu()
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/autoSizeX");
            if (node.InnerText != "") {
                autoSizeX = Convert.ToInt32(node.InnerText);
            }
            node = xmlDoc.SelectSingleNode("config/autoSizeY");
            if (node.InnerText != "")
            {
                autoSizeY = Convert.ToInt32(node.InnerText);
            }
            format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            int kk = 0;
            monitoringservice = new service.rtmonitoringService();
            string timeHouse = DateTime.Now.ToString("yyyy-MM-dd HH") + ":00:00";
            da =monitoringservice.rtmonitoring();
            DataTable dts1 =da.Tables[0];
            DataRow[] dr1 = dts1.Select("powerflag='0'");
            if (dr1.Count() > 0)
            {
                dt = dr1[0].Table.Clone();
                foreach (DataRow row in dr1)
                {
                    dt.ImportRow(row); // 将DataRow添加到DataTable中
                }
            }
            if (dt != null)
            {
                num = dt.Rows.Count;
                if (num > 0)
                {
                    flowLayoutPanel1.Controls.Clear();
                    picb = new PictureBox[num];
                    string measureMeterCode = null;
                    System.GC.Collect();
                    for (int x = 0; x < num; x++)
                    {
                        isbjShow = false;
                        picb[x] = new PictureBox();
                        picb[x].SizeMode = PictureBoxSizeMode.StretchImage;
                        picb[x].DoubleClick += new EventHandler(picb_DouClick);
                        measureMeterCode = dt.Rows[x]["measureMeterCode"].ToString();
                        picb[x].Tag = dt.Rows[x]["terminalname"].ToString() + "," + dt.Rows[x]["devtime"].ToString() + "," + dt.Rows[x]["measureCode"].ToString() + "," + dt.Rows[x]["meterNo"].ToString();
                        picb[x].BorderStyle = BorderStyle.None;
                        picb[x].Size = new Size(autoSizeX, autoSizeY);//大   小
                        double aa = rect.Width % picb[x].Size.Width;
                        double bb = rect.Width / picb[x].Size.Width;
                        int cc = Convert.ToInt32(aa / bb) - 2;
                        picb[x].Margin = new Padding(cc, 0, 0, 0);

                        if (measureMeterCode.Length != 0)
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
                            string ddbj = dt.Rows[x]["sign"].ToString();
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
                                ddbj = null;
                            }
                            else
                            {
                                if (a != 0 && d != 0)
                                {
                                    font = new Font("微软雅黑", Convert.ToSingle((double)20 * 5.0));
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
                                            isbjShow = true;
                                        }
                                        else
                                        {
                                            g.DrawString("0.0      ", font, Brushes.Blue, r, format);
                                            isbjShow = true;
                                        }
                                        size = TextRenderer.MeasureText("0.0      ", font);
                                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                                        if (d > e1 || d < f)
                                        {
                                            g.DrawString("0.0      ", font, Brushes.Red, r, format);
                                        }
                                        else
                                        {
                                            g.DrawString("0.0      ", font, Brushes.Blue, r, format);
                                        }
                                    }
                                    else
                                    {
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
                                        else
                                        {


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
                                        else
                                        {
                                        }
                                    }
                                }
                            }
                            if (ddbj != null && !"".Equals(ddbj))
                            {   //右下角断电报警图标
                                r = new Rectangle(670, 490, ddbjImage.Width, ddbjImage.Height);
                                g.DrawImage(ddbjImage, r.Right, r.Bottom, ddbjImage.Width, ddbjImage.Height);
                                isbjShow = true;
                            }
                            if (isbjShow)
                            {
                                //右下角报警图标
                                r = new Rectangle(740, 490, newImage.Width, newImage.Height);
                                g.DrawImage(newImage, r.Right, r.Bottom, newImage.Width, newImage.Height);
                                picb[x].MouseClick += new MouseEventHandler(picb_MouseClick);
                            }
                            picb[x].Image = bit;
                            g.Dispose();
                            g = null;
                        }
                        else
                        {
                            string cdname = dt.Rows[x]["terminalname"].ToString();
                            b = Double.Parse(dt.Rows[x]["t_high"].ToString());
                            c = Double.Parse(dt.Rows[x]["t_low"].ToString());
                            e1 = Double.Parse(dt.Rows[x]["h_high"].ToString());
                            f = Double.Parse(dt.Rows[x]["h_low"].ToString());
                            picb[x].Name = dt.Rows[x]["measureCode"].ToString() + "_" + dt.Rows[x]["meterNo"].ToString();
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
        }
        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel7_Click(sender,e);
        }

        private void toolStripLabel1_Click_2(object sender, EventArgs e)
        {
            if (basicsetup.ShowDialog() == DialogResult.OK)
            {
                this.timer2.Stop();
                frmMain_Load(sender,e);
            }
        }

        private void toolStripLabel7_Click(object sender, EventArgs e)
        {
            Exit exit = new Exit();
            if (exit.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("是否确认退出？退出后将不能获取和保存温湿度数据！", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //新增退出系统删除无标记记录（断电，报警，库房、车载时间间隔）
                    //service.deleteInvalidDataService did = new service.deleteInvalidDataService();
                    //did.deleteInvalidData();

                    service.loginLogService llse = new service.loginLogService();
                    bean.loginLogBean lb = new bean.loginLogBean();
                    lb.name = frmLogin.name;
                    lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    lb.eventInfo = "退出系统！";
                    llse.addCheckLog(lb);
                    saveToXmlsStoptime(DateTime.Now.ToString("yyMMddHHmmss"));
                    if (port.IsOpen)
                    {
                        port.Close();
                    }
                    System.Environment.Exit(0);
                }
            }
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi.Checked)
                tsmi.CheckState = CheckState.Unchecked;
            else
            {
                xmlDoc.Load(path);
                XmlNode node;
                node = xmlDoc.SelectSingleNode("config/autoSizeX");
                node.InnerText = "220";
                xmlDoc.Save(path);
                node = xmlDoc.SelectSingleNode("config/autoSizeY");
                node.InnerText = "185";
                xmlDoc.Save(path);

                initPointsInfo();
                tsmi.CheckState = CheckState.Checked;
                this.timer2.Stop();
                querywenshidu();
                this.timer2.Start();
            }
                
        }

        private void 默认大小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi.Checked)
                tsmi.CheckState = CheckState.Unchecked;
            else
            {
                xmlDoc.Load(path);
                XmlNode node;
                node = xmlDoc.SelectSingleNode("config/autoSizeX");
                node.InnerText = "180";
                xmlDoc.Save(path);
                node = xmlDoc.SelectSingleNode("config/autoSizeY");
                node.InnerText = "155";
                xmlDoc.Save(path);

                initPointsInfo();
                tsmi.CheckState = CheckState.Checked;
                this.timer2.Stop();
                querywenshidu();
                this.timer2.Start();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi.Checked)
                tsmi.CheckState = CheckState.Unchecked;
            else
            {
                xmlDoc.Load(path);
                XmlNode node;
                node = xmlDoc.SelectSingleNode("config/autoSizeX");
                node.InnerText = "280";
                xmlDoc.Save(path);
                node = xmlDoc.SelectSingleNode("config/autoSizeY");
                node.InnerText = "230";
                xmlDoc.Save(path);

                initPointsInfo();
                tsmi.CheckState = CheckState.Checked;
                this.timer2.Stop();
                querywenshidu();
                this.timer2.Start();
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi.Checked)
                tsmi.CheckState = CheckState.Unchecked;
            else
            {
                xmlDoc.Load(path);
                XmlNode node;
                node = xmlDoc.SelectSingleNode("config/autoSizeX");
                node.InnerText = "160";
                xmlDoc.Save(path);
                node = xmlDoc.SelectSingleNode("config/autoSizeY");
                node.InnerText = "140";
                xmlDoc.Save(path);

                initPointsInfo();
                tsmi.CheckState = CheckState.Checked;
                this.timer2.Stop();
                querywenshidu();
                this.timer2.Start();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi.Checked)
                tsmi.CheckState = CheckState.Unchecked;
            else
            {
                xmlDoc.Load(path);
                XmlNode node;
                node = xmlDoc.SelectSingleNode("config/autoSizeX");
                node.InnerText = "260";
                xmlDoc.Save(path);
                node = xmlDoc.SelectSingleNode("config/autoSizeY");
                node.InnerText = "215";
                xmlDoc.Save(path);

                initPointsInfo();
                tsmi.CheckState = CheckState.Checked;
                this.timer2.Stop();
                querywenshidu();
                this.timer2.Start();
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi.Checked)
                tsmi.CheckState = CheckState.Unchecked;
            else
            {
                xmlDoc.Load(path);
                XmlNode node;
                node = xmlDoc.SelectSingleNode("config/autoSizeX");
                node.InnerText = "240";
                xmlDoc.Save(path);
                node = xmlDoc.SelectSingleNode("config/autoSizeY");
                node.InnerText = "200";
                xmlDoc.Save(path);


                initPointsInfo();
                tsmi.CheckState = CheckState.Checked;
                this.timer2.Stop();
                querywenshidu();
                this.timer2.Start();
            }
        }

        private void 修改公司名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeCompanyName changecompanyname = new ChangeCompanyName();

            changecompanyname.textBox1.Text = textFileUpdate(@str+ filepath);

            if (changecompanyname.ShowDialog() == DialogResult.OK)
            {
                if (tm != null && !"".Equals(tm))
                {
                    lblTitle.Text = changecompanyname.compName + tm;
                    cm = changecompanyname.compName;
                }
                else {
                    lblTitle.Text = changecompanyname.compName + titlename;
                    cm = changecompanyname.compName;
                }
               
                textFile(@str + filepath, changecompanyname.compName);

                lblTitle.Left = (this.lbtitle.Width - this.lblTitle.Width) / 2;
                lblTitle.BringToFront();
            }
        }
        private void textFile(string filepath,string name) {
            FileStream myFs = myFs = new FileStream(@filepath, FileMode.Create);
            StreamWriter mySw = new StreamWriter(myFs);
            mySw.Write(name);
            mySw.Close();
            myFs.Close();
        }
        private string textFileUpdate(string filepath)
        {
            FileStream myFs = myFs = new FileStream(@filepath, FileMode.Open);
            StreamReader sd = new StreamReader(myFs);
            string name = sd.ReadLine();
            sd.Close();
            sd.Close();
            return name;
        }

        private void 修改标题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeTitle changetitle= new ChangeTitle();
            changetitle.titletextBox.Text=textFileUpdate(@str + filepath1);
            if (changetitle.ShowDialog() == DialogResult.OK)
            {
                if (cm != null && !"".Equals(cm))
                {
                    lblTitle.Text = cm + changetitle.titleName;
                    tm= changetitle.titleName;
                }
                else
                {
                    lblTitle.Text = companyName + changetitle.titleName;
                    tm = changetitle.titleName;
                }

                textFile(@str + filepath1, changetitle.titleName);

                lblTitle.Left = (this.lbtitle.Width - this.lblTitle.Width) / 2;
                lblTitle.BringToFront();
            }
        }

        private void 基本设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel1_Click_2(sender,e);
        }

        private void 报警设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            warningSetup ws = new warningSetup();
            if (ws.ShowDialog() == DialogResult.OK)
            {
                dtcdinfo1 = dis.checkPointInfo(0);
                this.timer2.Stop();
                timer2_Tick(null, null);
                this.timer2.Start();
            }
        }

        private void 查询历史数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripLabel3_Click(sender,e);
        }

        private void 查询历史曲线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripLabel4_Click(sender, e);
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            historyd.ShowDialog();
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
             graphcheck.ShowDialog();
        }

        private void flowLayoutPanel1_Paint_2(object sender, PaintEventArgs e)
        {

        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            int kk = 0;
            string timeHouse = DateTime.Now.ToString("yyyy-MM-dd HH") + ":00:00";
            da = monitoringservice.rtmonitoring();
            DataTable dts1 = da.Tables[0];
            if (dts1.Rows.Count > 0)
            {
                DataRow[] dr1 = dts1.Select("powerflag='0'");
                if (dr1.Count() > 0)
                {
                    dt = dr1[0].Table.Clone();
                    foreach (DataRow row in dr1)
                    {
                        dt.ImportRow(row); // 将DataRow添加到DataTable中
                    }
                }
                DataRow[] dr2 = dts1.Select("powerflag='1'");
                if (dr2.Length > 0)
                {
                    foreach (DataRow row in dr2)
                    {
                        string mcode = row["measureCode"].ToString();
                        string meterno = row["meterNo"].ToString();
                        int pnum = this.flowLayoutPanel1.Controls.Find(mcode + "_" + meterno, false).Count();
                        if (pnum > 0)
                        {
                            ((PictureBox)this.flowLayoutPanel1.Controls.Find(mcode + "_" + meterno, false)[0]).Visible = false;
                        }
                    }
                }
                num = dt.Rows.Count;
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
                        string ddbj = dt.Rows[i]["sign"].ToString();
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
                            ddbj = null;
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
                                else
                                {
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
                        if (ddbj == "1")
                        {   //右下角断电报警图标
                            r = new Rectangle(670, 490, ddbjImage.Width, ddbjImage.Height);
                            gg.DrawImage(ddbjImage, r.Right, r.Bottom, ddbjImage.Width, ddbjImage.Height);
                            ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick -= new MouseEventHandler(picb_MouseClick);
                            r = new Rectangle(740, 490, newImage.Width, newImage.Height);
                            gg.DrawImage(newImage, r.Right, r.Bottom, newImage.Width, newImage.Height);
                            ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).MouseClick += new MouseEventHandler(picb_MouseClick);
                        }

                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(codemeter, false)[0]).Image = bit;
                        gg.Dispose();
                        gg = null;
                    }
                    else
                    {
                        bit = new Bitmap(@str + "/images/cd.png");
                        gg = Graphics.FromImage(bit);
                        try
                        {
                            ((PictureBox)this.flowLayoutPanel1.Controls.Find(code + "_" + meter, false)[0]).Refresh();
                        }
                        catch
                        {
                            continue;
                        }

                        b = Double.Parse(t_high);
                        c = Double.Parse(t_low);
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
                        size = TextRenderer.MeasureText("- -      ", font);
                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 190, size.Width, size.Height);
                        gg.DrawString("- -      ", font, Brushes.Gray, r, format);

                        size = TextRenderer.MeasureText("- -    ", font);
                        r = new Rectangle((bit.Width - size.Width) / 2 - 130, 410, size.Width, size.Height);
                        gg.DrawString("- -    ", font, Brushes.Gray, r, format);

                        ((PictureBox)this.flowLayoutPanel1.Controls.Find(code + "_" + meter, false)[0]).Image = bit;
                        gg.Dispose();
                        gg = null;
                    }
                }
            }
        }
        bean.showReportBean rb = new bean.showReportBean();
        //根据管理主机唯一地址  计算出校验码的请求数据
        private byte[] getCRC(string text)
        {
            byte[] byteSends = { 0x1B, 0x06, 0x00, 0x01, 0x03, 0x00, 0x22, 0x00, 0x00, 0xFE, 0xDB };
            byte[] byteSend = { 0x00, 0x00, 0x03, 0x00, 0x22, 0x00, 0x00 };
            if (Int32.Parse(text) < 10)
            {
                text = "0" + text;
            }
            byteSend[1] = (byte)Convert.ToInt32("0x" + text, 16);
            uint crcRet = CRC1.calcrc16(byteSend, (uint)byteSend.Length);
            string xx = crcRet.ToString("X");
            byteSends[3] = (byte)Convert.ToInt32("0x" + text, 16);
            byteSends[9] = (byte)Convert.ToInt32("0x" + xx.Substring(0, 2), 16);
            byteSends[10] = (byte)Convert.ToInt32("0x" + xx.Substring(2), 16);
            return byteSends;
        }

        private void 查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            warningForm wf = new warningForm();//报警查询功能
            wf.ShowDialog();
        }

        private void 管理主机设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manageHoseSet mhs = new manageHoseSet();//管理主机设置功能
            if (result!=null) {
                mhs.comCode = result[1];
            }
            mhs.RefreshEvent += this.NeedRefresh;//注册事件
            mhs.ShowDialog();
        }
        private void NeedRefresh(object sender, EventArgs e)
        {
            dtcdinfo1 = dis.checkPointInfo(0);
            this.timer2.Stop();
            queryMeterIds();
            this.timer2.Start();
        }
        private void 测点属性设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            measurePointSet mp = new measurePointSet();
            mp.RefreshEvent += this.NeedRefreshs;//注册事件
            mp.ShowDialog();
        }
        private void NeedRefreshs(object sender, EventArgs e)
        {
            if (sender != null&&!"".Equals(sender)) { 
            if (Int32.Parse(sender.ToString()) == 1)
            {
                dtcdinfo1 = dis.checkPointInfo(0);
                //重新加载本页面
                this.timer2.Stop();
                queryMeterIds();
                this.timer2.Start();
                }
            }
            else
            {
                this.timer2.Stop();
                queryMeterIds();
                this.timer2.Start();
            }
        }
        private void 密码修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmLogin.name=="admin") {
                MessageBox.Show("管理员账号密码不能被修改");
            }
            else {
                passWordUpdate pwu = new passWordUpdate();
                pwu.ShowDialog();
            }
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            测点属性设置ToolStripMenuItem_Click(sender,e);
        }

        private void 用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userManage um = new userManage();
            um.ShowDialog();
        }

        private void toolStripLabel6_Click_1(object sender, EventArgs e)
        {
            imgUp iu = new imgUp();
            iu.ShowDialog();
        }

        private void 备份参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sb.ShowDialog() == DialogResult.OK)
            {
                string timetype = sb.timetype;
                if (timetype != null && !"".Equals(timetype))
                {
                    times = Int32.Parse(timetype);
                    saveToXmldatabasetimer(times.ToString());
                }
            }
        }

        private void 备份数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult rr = MessageBox.Show("是否需要手动备份数据库？", "手动备份数据库提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                string filepath = sdfilename;
                if (filepath != null && !"".Equals(filepath))
                {
                    if (!System.IO.Directory.Exists(filepath))
                        System.IO.Directory.CreateDirectory(filepath);
                    try
                    {
                        string []mysqlinfo = Properties.Settings.Default.mysqlInfo.Split(',');
                        DoBackupNoauto(mysqlinfo[0], mysqlinfo[1], mysqlinfo[2], mysqlinfo[3], mysqlinfo[4], filepath);
                        textFile(@str + "/manualBackupTimes.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //MessageBox.Show("数据备份成功！备份到：" + @filepath + "的路径下");
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("数据备份失败！");
                    }
                }
            }
        }

        private void backupDatabase()
        {
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string filepath = zdfilename;

            // 得到 hour minute second  如果等于某个值就开始执行某个程序。 
            //int intDay= DateTime.Now.Day;
            //int intHour = DateTime.Now.Hour;
            //int intMinute = DateTime.Now.Minute;

            // 定制时间； 比如 在10：30的时候执行某个函数  
            //int iDay = 1;
            //int iHour = 10;
            //int iMinute = 30;

            //if (times == 1 && intHour == iHour && intMinute == iMinute)
            //{
            if (filepath != null && !"".Equals(filepath))
            {
                if (!System.IO.Directory.Exists(filepath))
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }
                string[] mysqlinfo = Properties.Settings.Default.mysqlInfo.Split(',');
                try
                {      
                    DoBackup(mysqlinfo[0], mysqlinfo[1], mysqlinfo[2], mysqlinfo[3], mysqlinfo[4], filepath);
                    textFile(@str + "/automateBackupTimes.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                catch (Exception exc)
                {
                    
                }
            
            }
            //}
        }
        private void saveToXmldatabasetimer(string jtime)
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/databasetimer");
            node.InnerText = jtime;

            xmlDoc.Save(path);
        }
        private void 恢复数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataReduction dr = new dataReduction();
            if (dr.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show("恢复数据库成功!");
                this.timer2.Stop();
                frmMain_Load(sender, e);
                
            }
        }

        private void 库房管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            houseManage hm = new houseManage();
            hm.RefreshEvent += this.NeedRefresh;//注册事件
            hm.ShowDialog();
        }

        private void toolStripLabel8_Click(object sender, EventArgs e)
        {
            houseCheckInfo hci = new houseCheckInfo();
            if (hci.ShowDialog() == DialogResult.OK)
            {
                this.Show();
            }

        }
        private void flowLayoutPanel1_MouseEnter_1(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Focus();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            lblTitle.Left = (this.lbtitle.Width - this.lblTitle.Width) / 2;
            lblTitle.BringToFront();

            if (this.WindowState == FormWindowState.Minimized)
            {
                
            }
        }
        private void picb_DouClick(object sender, EventArgs e)
        {
            convenientSet cs = new convenientSet();
            PictureBox pic = (PictureBox)sender;
            string tag = pic.Tag.ToString();
            string[] tagg = tag.Split(',');
            DataTable dtd = dis.selectBydeviceInfo(tagg[2], tagg[3]);
            if (dtd.Rows.Count > 0)
            {
                DataTable dd = mhs.queryManageHoststoreType(dtd.Rows[0]["measureCode"].ToString());
                cs.textBox9.Text = dd.Rows[0]["hostName"].ToString();
                cs.textBox10.Text = dtd.Rows[0]["meterNo"].ToString();

                cs.textBox3.Text = dtd.Rows[0]["terminalname"].ToString();
                cs.textBox4.Text = dtd.Rows[0]["measureCode"].ToString() + "-" + dtd.Rows[0]["meterNo"].ToString();
                string hostaddress = null;
                try
                {
                    hostaddress = dtd.Rows[0]["hostAddress"].ToString();
                }
                catch
                {
                    hostaddress = null;
                }
                if (hostaddress != null && !"".Equals(hostaddress))
                {
                    cs.textBox5.Text = hostaddress;
                    cs.radioButton1.Checked = true;
                    cs.comboBox5.Enabled = true;
                    if (result != null && result.Length > 0)
                    {
                        cs.comboBox5.Text = result[1];
                    }
                }

                cs.comboBox3.Text = dtd.Rows[0]["CommunicationType"].ToString();
                cs.numericUpDown4.Value = (decimal)Convert.ToDouble(dtd.Rows[0]["t_high"].ToString());
                cs.numericUpDown2.Value = (decimal)Convert.ToDouble(dtd.Rows[0]["t_low"].ToString());
                cs.numericUpDown1.Value = (decimal)Convert.ToDouble(dtd.Rows[0]["h_high"].ToString());
                cs.numericUpDown5.Value = (decimal)Convert.ToDouble(dtd.Rows[0]["h_low"].ToString());
                if (cs.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("测点信息修改成功！");
                    dtcdinfo1 = dis.checkPointInfo(0);
                    this.timer2.Stop();
                    timer2_Tick(null, null);
                    this.timer2.Start();
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
                //wh.textBox2.Text = time;
                wh.dateTimePicker1.Value = Convert.ToDateTime(time);
                wh.textBox5.Text = name;
                wh.ShowDialog();
            }
        }

        private void 查询报警处理情况ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            warningHandleCheck whc = new warningHandleCheck();
            whc.ShowDialog();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            toolStripLabel7_Click(sender, e);
        }
        //加密狗定时程序
        private void timer5_Tick(object sender, EventArgs e)
        {
            int vv = 1;
            //int[] keyHandles = new int[8];
            //int[] keyNumber = new int[8];
            //SmartApp smart = new SmartApp();
            //vv = smart.SmartX1Find("GSPAutoMonitor", keyHandles, keyNumber);
            if (IntPtr.Size == 4)
            {
                vv = NT88_X86.NTFindFirst("longbangrj716");
            }
            else
            {
                vv = NT88_X64.NTFindFirst("longbangrj716");
            }
            if (vv != 0)
            {
                MessageBox.Show("系统未检测到软件加密锁，软件将自动退出！请插入软件加密锁重新打开软件！");
                System.Environment.Exit(0);
            }
        }
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.ShowInTaskbar = true;
            this.notifyIcon1.Visible = false;
        }
        //#region 从xml获得数据，并加载
        private void getFromXml()
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/username");
            username = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/password");
            password = node.InnerText;

            if (username != null && !"".Equals(username) && password != null && !"".Equals(password))
            {
                ipport = username + ":" + password;
            }
            node = xmlDoc.SelectSingleNode("config/scoketServer");
            socketserver = Int32.Parse(node.InnerText);
        }

        private void 取消自动登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要取消自动登录功能吗？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                frmLogin frm = new frmLogin();
                try
                {
                    frm.cancel_login();
                    MessageBox.Show("已取消成功，下次登录需要账户和密码才能登录！");
                }
                catch (Exception exc)
                {
                    
                }
            }
        }
        private void saveToXmlsStoptime(string stoptime)
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/stoptime");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("stoptime");
                n.InnerText = stoptime;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = stoptime;
            }
            xmlDoc.Save(path);

        }
        private void autogetServiceData()
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/stoptime");
            stoptime = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/starttime");
            starttime = node.InnerText;

        }

        private void 帮助主题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pdfpath = @str + @"/冷链5S软件说明书.pdf";
            //打开PDF，看效果
            Process.Start(pdfpath);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }

        private void 说明书ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string paths = @str + @"\Help";
            Process.Start("Explorer.exe", paths);
        }

        private void 全屏显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.FormBorderStyle = FormBorderStyle.None;
            this.menuStrip1.Visible = false;
            this.toolStrip1.Visible = false;
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.F12)
            {
                base.FormBorderStyle = FormBorderStyle.Sizable;
                this.menuStrip1.Visible = true;
                this.toolStrip1.Visible = true;
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            报警设置ToolStripMenuItem_Click(sender, e);
        }

        private void 分库浏览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel8_Click(sender, e);
        }

        private void 查询日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loginLog lg = new loginLog();
            lg.ShowDialog();
        }

        private void 库房平面图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel6_Click_1(sender, e);
        }

        private void toolStripLabel1_MouseHover(object sender, EventArgs e)
        {
            this.toolStripLabel1.BackgroundImage = Image.FromFile(@str + "/images/hover.png");

        }
        private void toolStripLabel1_MouseLeave(object sender, EventArgs e)
        {
            this.toolStripLabel1.BackgroundImage = null;
        }

        private void toolStripLabel2_MouseHover(object sender, EventArgs e)
        {
            this.toolStripLabel2.BackgroundImage = Image.FromFile(@str + "/images/hover.png");
        }

        private void toolStripLabel2_MouseLeave(object sender, EventArgs e)
        {
            this.toolStripLabel2.BackgroundImage = null;
        }

        private void toolStripLabel3_MouseHover(object sender, EventArgs e)
        {
            this.toolStripLabel3.BackgroundImage = Image.FromFile(@str + "/images/hover.png");
        }

        private void toolStripLabel3_MouseLeave(object sender, EventArgs e)
        {
            this.toolStripLabel3.BackgroundImage = null;
        }

        private void toolStripLabel4_MouseHover(object sender, EventArgs e)
        {
            this.toolStripLabel4.BackgroundImage = Image.FromFile(@str + "/images/hover.png");
        }

        private void toolStripLabel4_MouseLeave(object sender, EventArgs e)
        {
            this.toolStripLabel4.BackgroundImage = null;
        }

        private void toolStripLabel5_MouseHover(object sender, EventArgs e)
        {
            this.toolStripLabel5.BackgroundImage = Image.FromFile(@str + "/images/hover.png");
        }

        private void toolStripLabel5_MouseLeave(object sender, EventArgs e)
        {
            this.toolStripLabel5.BackgroundImage = null;
        }

        private void toolStripLabel6_MouseHover(object sender, EventArgs e)
        {
            this.toolStripLabel6.BackgroundImage = Image.FromFile(@str + "/images/hover.png");
        }

        private void toolStripLabel6_MouseLeave(object sender, EventArgs e)
        {
            this.toolStripLabel6.BackgroundImage = null;
        }

        private void toolStripLabel8_MouseHover(object sender, EventArgs e)
        {
            this.toolStripLabel8.BackgroundImage = Image.FromFile(@str + "/images/hover.png");
        }

        private void toolStripLabel8_MouseLeave(object sender, EventArgs e)
        {
            this.toolStripLabel8.BackgroundImage = null;
        }

        private void toolStripLabel7_MouseHover(object sender, EventArgs e)
        {
            this.toolStripLabel7.BackgroundImage = Image.FromFile(@str + "/images/hover.png");
        }

        private void toolStripLabel7_MouseLeave(object sender, EventArgs e)
        {
            this.toolStripLabel7.BackgroundImage = null;
        }

        private void lbtitle_Paint(object sender, PaintEventArgs e)
        {

        }

        int djs = 1800;
        private void timer1_Tick(object sender, EventArgs e)
        {
            djs -= 1;
            if (djs != 0)
            {
                label1.Text = "距离试用30分钟结束时间还有: " + djs + " 秒";
            }
            else
            {
                this.timer1.Stop();
                MessageBox.Show("软件系统试用时间结束，软件将自动退出！");
                System.Environment.Exit(0);
            }
        }

        private void 菜单栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.菜单栏ToolStripMenuItem.Checked = !this.菜单栏ToolStripMenuItem.Checked;
            this.menuStrip1.Visible = this.菜单栏ToolStripMenuItem.Checked;
        }

        private void 工具栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.工具栏ToolStripMenuItem.Checked = !this.工具栏ToolStripMenuItem.Checked;
            this.toolStrip1.Visible = this.工具栏ToolStripMenuItem.Checked;
        }

        private void 标题栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.标题栏ToolStripMenuItem.Checked = !this.标题栏ToolStripMenuItem.Checked;
            this.lbtitle.Visible = this.标题栏ToolStripMenuItem.Checked;
        }

        private void 显示报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showReport rp = new showReport();
            rp.ShowDialog();
        }
        // mysql数据库手动备份
        public void DoBackupNoauto(string host, string port, string user, string password, string database, string filepath)
        {
            string backfile = filepath + database + "_bak_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".sql";
            string cmdStr = "mysqldump -h" + host + " -P" + port + " -u" + user + " -p" + password + " " + database + " > " + backfile;

            try
            {
                string reslut = RunCmd(str + "\\Lib", cmdStr);
                if (reslut.IndexOf("error") == -1 && reslut.IndexOf("命令") == -1)
                {
                    MessageBox.Show("备份成功>" + backfile);
                }
                else
                {
                    MessageBox.Show(reslut + "备份失败>" + backfile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // mysql数据库自动备份
        public void DoBackup(string host, string port, string user, string password, string database, string filepath)
        {
            string backfile = "";
            if (sb.istrue)
            {
                backfile = filepath + database + "_bak_newDatas" + ".sql";
            }
            else {
                backfile = filepath + database + "_bak_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".sql";
            }
            string cmdStr = "mysqldump -h" + host + " -P" + port + " -u" + user + " -p" + password + " " + database + " > " + backfile;

            try
            {
                string reslut = RunCmd(str + "\\Lib", cmdStr);
                if (reslut.IndexOf("error")==-1 && reslut.IndexOf("命令")==-1)
                {
                    MessageBox.Show("备份成功>" + backfile);
                } else {
                    MessageBox.Show(reslut + "备份失败>" + backfile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        public string RunCmd(string workingDirectory, string command)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe"; //确定程序名
            //p.StartInfo.Arguments = "/c " + command; //确定程式命令行
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.UseShellExecute = false; //Shell的使用
            p.StartInfo.RedirectStandardInput = true; //重定向输入
            p.StartInfo.RedirectStandardOutput = true; //重定向输出
            p.StartInfo.RedirectStandardError = true; //重定向输出错误
            p.StartInfo.CreateNoWindow = true; //设置置不显示示窗口
            p.Start();
            p.StandardInput.WriteLine(command); //也可以用这种方式输入入要行的命令 
            p.StandardInput.WriteLine("exit"); //要得加上Exit要不然下一行程式
            //p.WaitForExit();
            //p.Close();
            //return p.StandardOutput.ReadToEnd(); //输出出流取得命令行结果果
            return p.StandardError.ReadToEnd();
        }
    }
}
