//using SQLitePCL;
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
namespace LBKJClient
{
    public partial class frmMain : Form
    {
        public delegate void MyDelegate();
        MyDelegate mydelegate;
        public static string mids = null;
        public static string ipport = null;
        public static bool istrueport = true;
        public int hulie = 1;
        private string rcids = null;
        private int totalReceivedBytes = 0;
        private int totalSendBytes = 0;
        private int logrizhi = 0;
        private bool needWrite = false;
        public delegate void UpdateAcceptTextBoxTextHandler(string text);
        public UpdateAcceptTextBoxTextHandler UpdateTextHandler;
        public delegate void SendDeviceInfoHandler(string text);
        public SendDeviceInfoHandler SendInfotHandler;
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
        service.addDataService adddatas = new service.addDataService();//新增数据
        service.rtmonitoringService monitoringservice = new service.rtmonitoringService();
        service.manageHostService mhs = new service.manageHostService();//查询GZ02有串口地址的数据
        dataBackUpSet sb = new dataBackUpSet();//数据备份
        service.showReportService lls = new service.showReportService();

        DataSet da;
        DataTable dt;
        DataTable dtcdinfo;
        DataTable dtcdinfo1 = null;
        DataTable dtcdinfo2 = null;
        public DataTable dtB = null;//查询测点最后一条记录用
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
        Boolean isCRC = false;
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
                string power = "显示报告,库房平面图,分库浏览,采集器数据同步,库房管理,管理主机设置,数据库管理,查询登录日志,查询报警处理,查询报警记录,查询历史曲线,查询历史数据,密码修改,测点属性设置,报警设置,基本设置,修改标题,修改公司名称,用户管理";
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
            this.menuStrip1.BackColor = Color.FromArgb(181, 220, 255);
            this.toolStrip1.BackColor = Color.FromArgb(200, 233, 253);
            getFromXml();
            rect = Screen.GetWorkingArea(this);
            initPointsInfo();
            overtime = Properties.Settings.Default.overTime;
            //获取通信类别
            getFromXmlcommunication();
            this.timer3.Interval = Int32.Parse(datarefreshtime) * 1000;
            this.timer3.Start();
            dtcdinfo1 = dis.checkPointInfo(1);
            if (getresults != null && !"".Equals(getresults))
            {
                //启动服务器自动同步关服时间到开服时间无线主机历史数据
                if (autosave16 == 1)
                {
                    insertHStopData();
                };

                //自动读取历史数据占用主线程
                if (autosave02 == 1)
                {
                    insertHYStopData();
                };

                result = getresults.Split('-');
                if (result[0] == "1")
                {
                    dtCom = mhs.queryManageHostCom();
                    bool istrue = initPort(result[1]);
                    if (istrue)
                    {
                        port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.mySerialPort_DataReceived);
                        this.timerGetdeviceinfo.Interval = Int32.Parse(datasavetime) * 1000;
                        this.timerGetdeviceinfo.Start();//抓取LBGZ-02管理主机数据
                        timerGetdeviceinfo_Tick(sender, e);
                    }
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
            //this.lblTitle.ForeColor = Color.FromArgb(65, 105, 225); 
            this.lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Font = new Font("微软雅黑", 26F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            lblTitle.Left = (this.lbtitle.Width - this.lblTitle.Width) / 2;
            lblTitle.BringToFront();
            backupDatabase();//数据库自动备份
            this.timer2.Interval = Int32.Parse(datasavetime) * 1000;
            this.timer2.Start();
        }

        DataTable dtcdinfoHY = null;
        string bwnum = "";
        SerialPort portHY = new SerialPort();
        Thread multiHYA;
        service.manageHostService mhsHY = new service.manageHostService();
        public string comCode = "";
        public string addressHY = "";
        public string measureCodeHY = "";
        private void insertHYStopData()
        {
            service.deviceInformationService dis = new service.deviceInformationService();
            dtcdinfoHY = dis.checkPointInfo(1);
            //1获取端口号,设置端口信息
            int baudRate = 9600;
            portHY.BaudRate = baudRate;
            if (getresults != null && !"".Equals(getresults))
            {
                result = getresults.Split('-');
                comCode = result[1];
                portHY.PortName = comCode;
            }

            portHY.DataReceived += new SerialDataReceivedEventHandler(this.mySerialPortHY_DataReceived);


            //2获取主机编号与地址
            DataTable dtHY = mhsHY.queryManageHost();
            if (dtHY.Rows.Count > 0)
            {
                for (int i = 0; i < dtHY.Rows.Count; i++)
                {

                    addressHY = "" + dtHY.Rows[i][2];
                    measureCodeHY = "" + dtHY.Rows[i][7];
                    if (!portHY.IsOpen)
                    {
                        portHY.Open();
                        portHY.DiscardOutBuffer();
                        portHY.DiscardInBuffer();
                    }
                    if (addressHY != "")
                     {
                        byte[] byteSend = getCRCHY(addressHY);
                        portHY.Write(byteSend, 0, byteSend.Length);
                        Thread.Sleep(500);
                        while (bwnum != "" && Int32.Parse(bwnum) != 0)
                        {
                            byteSend = getCRCHY(addressHY);
                            portHY.Write(byteSend, 0, byteSend.Length);
                            Thread.Sleep(500);
                         };
                        portHY.Close();
                    }

                };
            }
        }
        List<bean.dataSerialization> ldsHY = null;
        private void mySerialPortHY_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Boolean isCRC = false;
            try
            {
                SerialPort sp = (SerialPort)sender;
                Byte[] byteRead = new Byte[sp.BytesToRead];
                sp.Read(byteRead, 0, byteRead.Length);
                sp.DiscardInBuffer();
                sp.DiscardOutBuffer();
                //totalHYReceivedBytes += size;               
                totalByteRead = totalByteRead.Concat(byteRead).ToArray();
                if (totalByteRead.Length > 10)
                {
                    byte[] crcByte = totalByteRead.Skip(2).Take(totalByteRead.Length - 4).ToArray();
                    uint crcRet = CRC1.calcrc16(crcByte, (uint)crcByte.Length);
                    uint crcSource = (uint)(totalByteRead[totalByteRead.Length - 2] << 8 | totalByteRead[totalByteRead.Length - 1]);
                    if (crcSource == crcRet)
                    {
                        isCRC = true;
                    }
                    else
                    {
                        isCRC = false;
                    }
                }
                if (isCRC)
                {   //仓储管理主机解析报文并存入数据库
                    //int index_end = this.IndexOf(totalByteRead, new byte[] { 0x40, 0x10 });
                    string no = null;
                    int intervalNum1 = 0;
                    int intervalNum2 = 0;
                    int intervalNum3 = 0;
                    double tt, t1, t2, hh, h1, h2;
                    string str_temperature = null;
                    string str_humidity = null;
                    byte[] byte_date = totalByteRead.Skip(totalByteRead.Length - 8).Take(4).ToArray();
                    byte[] byte_bwnum = totalByteRead.Skip(totalByteRead.Length - 4).Take(2).ToArray();
                    string str_datetime = ToDateString(byte_date);
                    string powerClose = Int32.Parse(totalByteRead[totalByteRead.Length - 10].ToString("X2")) == 40 ? "1" : "0";
                    bwnum = ToBwNumString(byte_bwnum);

                    //转换时间格式
                    string TarStr1 = "yyyy-MM-dd HH:mm:ss";
                    string TarStr = "yyyyMMddHHmmss";
                    IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                    string datetime = DateTime.ParseExact(str_datetime, TarStr, format).ToString(TarStr1);
                    DateTime MyDate = DateTime.ParseExact(str_datetime, TarStr, format);
                    datetime = MyDate.ToString(TarStr1);
                    int mm = MyDate.Minute;
                    if (mm % 2 == 0)
                    {
                        intervalNum1 = 2;
                    }
                    else
                    {
                        intervalNum1 = 0;
                    }
                    if (mm % cartime == 0)
                    {
                        intervalNum2 = 5;
                    }
                    else
                    {
                        intervalNum2 = 0;
                    }

                    if (mm % housetime == 30)
                    {
                        intervalNum3 = 3;
                    }
                    else
                    {
                        intervalNum3 = 0;
                    }

                    ldsHY = new List<bean.dataSerialization>();
                    bean.dataSerialization info = null;
                    int history = 0;
                    int Whistory = 0;
                    string measureMeterCodeB = "";
                    for (int i = 6; i < totalByteRead.Length - 9; i = i + 6)
                    {
                        byte[] newA = totalByteRead.Skip(i).Take(6).ToArray();
                        byte[] byte_temperature = { newA[2], newA[1] };
                        byte[] byte_humidity = { newA[4], newA[3] };
                        no = (Int32.Parse(newA[0].ToString()) + 1).ToString();
                        string binary = Convert.ToString(newA[5], 2).PadLeft(8, '0').Substring(0, 1);
                        str_temperature = BitConverter.ToInt16(byte_temperature, 0).ToString();
                        str_humidity = BitConverter.ToInt16(byte_humidity, 0).ToString();

                        info = new bean.dataSerialization();

                        if (str_temperature.Length > 2)
                        {
                            info.temperature = str_temperature.Insert(2, ".");
                        }
                        else if (str_temperature.Length == 2)
                        {
                            info.temperature = str_temperature.Insert(1, ".");
                        }
                        else
                        {
                            info.temperature = str_temperature;
                        }
                        if (str_humidity.Length > 2)
                        {
                            info.humidity = str_humidity.Insert(2, ".");
                        }
                        else if (str_humidity.Length == 2)
                        {
                            info.humidity = str_humidity.Insert(1, ".");
                        }
                        else
                        {
                            info.humidity = str_humidity;
                        }
                        if (Int32.Parse(binary) > 0)
                        {
                            info.temperature = info.temperature.Insert(0, "-");
                        }

                        if (no.ToString().Length == 1)
                        {
                            info.deviceNum = no.ToString().Insert(0, "0");
                        }
                        else
                        {
                            info.deviceNum = no.ToString();
                        }
                        if (measureCodeHY != null && !"".Equals(measureCodeHY))
                        {
                            info.managerID = measureCodeHY;
                        }
                        info.devicedate = datetime;
                        info.sysdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        info.measureMeterCode = measureCodeHY + "_" + info.deviceNum;

                        measureMeterCodeB = info.measureMeterCode;
                        dtB = adddatas.checkLastRecordBIsOr(info.measureMeterCode);
                        if (dtB.Rows[0][1].ToString() == "1") { Whistory = 1; } else { Whistory = 0; };
                        if (dtB.Rows[0][2].ToString() == "2") { history = 2; } else { history = 0; };

                        if (intervalNum1 == 2)
                        {
                            DataRow[] drs = dtcdinfoHY.Select("measureCode='" + info.managerID + "' and meterNo='" + info.deviceNum + "'");
                            tt = Double.Parse(info.temperature);
                            t1 = Double.Parse(drs[0]["t_high"].ToString());
                            t2 = Double.Parse(drs[0]["t_low"].ToString());
                            hh = Double.Parse(info.humidity);
                            h1 = Double.Parse(drs[0]["h_high"].ToString());
                            h2 = Double.Parse(drs[0]["h_low"].ToString());
                            if (tt > t1 || tt < t2 || hh > h1 || hh < h2)
                            {
                                info.warningistrue = "2";
                            }
                            else if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2) {
                                info.warningistrue = "3";
                                history = 0;
                            } else {
                                history = 0;
                            }
                            if (Int32.Parse(powerClose) == 1)
                            {
                                info.warnState = powerClose;
                            }
                            else if (Int32.Parse(powerClose) != 1 && Whistory == 1) { info.warnState = "3"; Whistory = 0; } else { Whistory = 0; };
                        }
                        if (intervalNum2 == 5)
                        {
                            info.carinterval = "5";
                        }
                        if (intervalNum3 == 3)
                        {
                            info.houseinterval = "30";
                        }
                        if (Int32.Parse(powerClose) == 1)
                        {
                            info.sign = "1";
                        }
                        else if (Int32.Parse(powerClose) != 1 && Whistory == 1) { info.sign = "3"; Whistory = 0; } else { Whistory = 0; };
                        ldsHY.Add(info);
                    }
                    totalByteRead = new Byte[0];
                    if (ldsHY.Count > 0)
                    {
                        multiHYA = new Thread(new ThreadStart(addDataHistory));
                        multiHYA.IsBackground = true;
                        multiHYA.Start();
                    }
                }

            }
            catch (Exception ee)
            {
                //MessageBox.Show(ee.StackTrace);
            }

        }

        private string ToBwNumString(byte[] bytes)
        {
            int num = 0;
            if (bytes != null)
            {
                string bn = ToHexString(bytes).Replace(" ", "");
                num = Convert.ToInt32(bn, 16);
            }
            return num.ToString();

        }

        private void addDataHistory()
        {
            adddatas.addData(ldsHY);
        }
        private byte[] getCRCHY(string text)
        {
            byte[] byteSends = { 0x1B, 0x06, 0x00, 0x03, 0x03, 0x11, 0x66, 0x00, 0x00, 0xBA, 0x10 };
            byte[] byteSend = { 0x00, 0x03, 0x03, 0x11, 0x66, 0x00, 0x00 };
            if (Int32.Parse(text) < 10)
            {
                text = "0" + text;
            }
            byteSend[1] = (byte)Convert.ToInt32("0x" + text, 16);
            uint crcRet = CRC1.calcrc16(byteSend, (uint)byteSend.Length);
            string xx = crcRet.ToString("X");
            if (xx.Length == 3)
            {
                xx = "0" + xx;
            }
            byteSends[3] = (byte)Convert.ToInt32("0x" + text, 16);
            byteSends[9] = (byte)Convert.ToInt32("0x" + xx.Substring(0, 2), 16);
            byteSends[10] = (byte)Convert.ToInt32("0x" + xx.Substring(2), 16);
            return byteSends;
        }
        string json = "";
        Thread multiH;
        DataTable dtH;
        public string ipportH = null;
        private void insertHStopData()
        {
            //获取设备监测历史数据接口(因服务器关机等没有保存的监测信息在服务器开启时既用户再次开机登录时同步到数据库)
            //获取参数 mids,上面方法queryMeterIds()已赋值"measureCode-meterNo:";
            dtcdinfo = dis.checkPointInfo(0);
            //xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/stoptime");
            starttime = node.InnerText;


            node = xmlDoc.SelectSingleNode("config/starttime");
            stoptime = node.InnerText;


            node = xmlDoc.SelectSingleNode("config/historyDataIport");
            ipportH = node.InnerText;



            MyDate = DateTime.ParseExact(starttime, TarStr, formatH);
            starttime1 = MyDate.ToString(TarStr1);

            MyDate = DateTime.ParseExact(stoptime, TarStr, formatH);
            stoptime1 = MyDate.ToString(TarStr1);

            dtH = adddatas.checkDatasTimes(starttime1, stoptime1);
            string[] cdlist;
            if (mids != "" && mids != null) {
                cdlist = mids.Split(':');
                if (cdlist.Length > 0)
                {
                  for (int i = 0; i < cdlist.Length; i++)
                   {
                    json = "{\"id\":\"" + cdlist[i] + "\",\"sign\":\"1wVebSp57j67GOV7bQ6IDg==\",\"starttime\":\"" + starttime + "\",\"endtime\":\"" + stoptime + "\"}";
                    string jsonData = utils.HttpClient.getDeviceData(json, ipportH);
                    if (jsonData != "" && jsonData.Length > 50)
                    {
                        list = js.Deserialize<List<bean.dataSerialization>>(jsonData);
                        insertHData(list);
                    }
                    Thread.Sleep(500);
                }
               }
            };
        }

        string TarStr1 = "yyyy-MM-dd HH:mm:ss";
        string TarStr = "yyMMddHHmmss";
        int intervalNum1 = 0;
        int intervalNum2 = 0;
        int intervalNum3 = 0;
        IFormatProvider formatH = new System.Globalization.CultureInfo("zh-CN");
        DateTime MyDate;
        DateTime MyDate1;
        double tt, t1, t2, hh, h1, h2;
        private void insertHData(List<bean.dataSerialization> list)
        {
            List<bean.dataSerialization> listed = new List<bean.dataSerialization>();
            int history = 0;
            string measureMeterCodeB = "";
            foreach (bean.dataSerialization datas in list)
            {
                datas.temperature = getFloat(datas.temperature);

                datas.humidity = getFloat(datas.humidity);

                if (datas.lng != null && !"".Equals(datas.lng) && datas.lng.Length > 3)
                {
                    datas.lng = datas.lng.Insert(3, ".");
                }
                else
                {
                    datas.lng = "";
                }
                if (datas.lat != null && !"".Equals(datas.lat) && datas.lat.Length > 3)
                {
                    datas.lat = datas.lat.Insert(2, ".");
                }
                else
                {
                    datas.lat = "";
                }
                if (datas.devicedate != null && datas.devicedate.Length > 0)
                {
                    MyDate = DateTime.ParseExact(datas.devicedate, TarStr, formatH);
                    datas.devicedate = MyDate.ToString(TarStr1);
                    int mm = MyDate.Minute;
                    if (mm % 2 == 0)
                    {
                        intervalNum1 = 2;
                    }
                    else
                    {
                        intervalNum1 = 0;
                    }
                    if (mm % cartime == 0)
                    {
                        intervalNum2 = 5;
                    }
                    else
                    {
                        intervalNum2 = 0;
                    }
                    if (mm % housetime == 30)
                    {
                        intervalNum3 = 3;
                    }
                    else
                    {
                        intervalNum3 = 0;
                    }
                }
                if (datas.sysdate != null && datas.sysdate.Length > 0)
                {
                    MyDate1 = DateTime.ParseExact(datas.sysdate, TarStr, formatH);
                    datas.sysdate = MyDate1.ToString(TarStr1);
                }
                datas.measureMeterCode = datas.managerID + "_" + datas.deviceNum;
                measureMeterCodeB = datas.measureMeterCode;
                dtB = adddatas.checkLastRecordBIsOr(datas.measureMeterCode);
                if (dtB.Rows[0][2].ToString() == "2") { history = 2; } else { history = 0; };

                DataRow[] drs = dtcdinfo.Select("measureCode='" + datas.managerID + "' and meterNo='" + datas.deviceNum + "'"); ;
                tt = Double.Parse(datas.temperature);
                t1 = Double.Parse(drs[0]["t_high"].ToString());
                t2 = Double.Parse(drs[0]["t_low"].ToString());
                hh = Double.Parse(datas.humidity);
                h1 = Double.Parse(drs[0]["h_high"].ToString());
                h2 = Double.Parse(drs[0]["h_low"].ToString());
                string CommunicationType = drs[0]["CommunicationType"].ToString();
                if (CommunicationType == "LBCC-16" || CommunicationType == "[管理主机]LB863RSB_N1(LBGZ-02)")
                {
                    if (tt > t1 || tt < t2 || hh > h1 || hh < h2)
                    {
                            datas.warningistrue = "2";                        
                    }else if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2)
                    {
                        datas.warningistrue = "3";
                        history = 0;
                    }
                    else {
                        datas.warningistrue = "1";
                        history = 0;
                    }
                }
                else
                {
                    if (intervalNum1 == 2)
                    {
                        if (tt > t1 || tt < t2 || hh > h1 || hh < h2)
                        {
                                datas.warningistrue = "2";
                        }
                        else if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2)
                        {
                            datas.warningistrue = "3";
                            history = 0;
                        }
                        else {
                            history = 0;
                        }
                    }
                    if (intervalNum2 == 5)
                    {
                        datas.carinterval = "5";
                    }
                    if (intervalNum3 == 3)
                    {
                        datas.houseinterval = "30";
                    }
                }
                listed.Add(datas);
            }
            if (listed.Count > 0 && dtH.Rows.Count > 0)
            {
                for (int i = 0; i < listed.Count; i++)
                {
                    
                    DataRow[] drArr = dtH.Select("measureCode='" + listed[i].managerID + "' and meterNo='" + listed[i].deviceNum + "' and devtime='" + listed[i].devicedate + "'");
                    if (drArr.Length > 0)
                    {
                        listed.RemoveAt(i);
                        if (i > 0)
                        {
                            --i;
                        }
                        else
                        {
                            i = -1;
                        }
                    }

                }
            }
            adddatas.addData(listed);
        }
        List<bean.dataSerialization> lds = null;
        private void mySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(240);
            try
            {
                string time111 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
                SerialPort sp = (SerialPort)sender;
                string text = string.Empty;
                Byte[] byteRead = new Byte[sp.BytesToRead];
                if (byteRead.Length == 0)
                {
                    doGetDeviceInfo();
                }
                sp.Read(byteRead, 0, byteRead.Length);
                sp.DiscardInBuffer();
                sp.DiscardOutBuffer();
                //totalReceivedBytes += size;
                totalByteRead = totalByteRead.Concat(byteRead).ToArray();
                text = ToHexString(totalByteRead);
                if (totalByteRead.Length > 10)
                {
                    byte[] crcByte = totalByteRead.Skip(2).Take(totalByteRead.Length - 4).ToArray();
                    uint crcRet = CRC1.calcrc16(crcByte, (uint)crcByte.Length);
                    uint crcSource = (uint)(totalByteRead[totalByteRead.Length - 2] << 8 | totalByteRead[totalByteRead.Length - 1]);
                    if (crcSource == crcRet)
                    {
                        isCRC = true;
                    }
                    else
                    {
                        isCRC = false;
                        if (logrizhi == 1)
                        {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:/log.txt", true))
                            {
                                sw.WriteLine("开始：" + time111 + "  结束：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "   错误：" + text);
                            }
                        }
                    }
                }
                if (isCRC)
                {
                    //仓储管理主机解析报文并存入数据库
                    //int index_end = this.IndexOf(totalByteRead, new byte[] { 0x40, 0x10 });
                    lds = new List<bean.dataSerialization>();
                    string no = null;
                    string str_temperature = null;
                    string str_humidity = null;
                    string datetime = "";
                    double tt, t1, t2, hh, h1, h2;
                    int intervalNum1 = 0;
                    int intervalNum2 = 0;
                    int intervalNum3 = 0;
                    byte[] byte_date = totalByteRead.Skip(totalByteRead.Length - 6).Take(4).ToArray();
                    string str_datetime = ToDateString(byte_date);
                    string powerClose = Int32.Parse(totalByteRead[totalByteRead.Length - 8].ToString("X2")) == 40 ? "1" : "0";
                    //转换时间格式
                    string TarStr1 = "yyyy-MM-dd HH:mm:ss";
                    string TarStr = "yyyyMMddHHmmss";
                    IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                    try
                    {
                        DateTime MyDate=DateTime.ParseExact(str_datetime, TarStr, format);
                        datetime = MyDate.ToString(TarStr1);
                        int mm = MyDate.Minute;
                        if (mm % 2 == 0)
                        {
                            intervalNum1 = 2;
                        }
                        else
                        {
                            intervalNum1 = 0;
                        }
                        if (mm % cartime == 0)
                        {
                            intervalNum2 = 5;
                        }
                        else
                        {
                            intervalNum2 = 0;
                        }

                        if (mm % housetime == 0)
                        {
                            intervalNum3 = 3;
                        }
                        else
                        {
                            intervalNum3 = 0;
                        }
                    }
                    catch {
                        datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    bean.dataSerialization info = null;
                    int history = 0;
                    int Whistory = 0;
                    for (int i = 6; i < totalByteRead.Length-7; i += 6)
                    {
                        byte[] newA = totalByteRead.Skip(i).Take(6).ToArray();
                        byte[] byte_temperature = { newA[2], newA[1] };
                        byte[] byte_humidity = { newA[4], newA[3] };
                        no = newA[0].ToString();
                        string binary = Convert.ToString(newA[5], 2).PadLeft(8, '0').Substring(0,1);
                        str_temperature = BitConverter.ToInt16(byte_temperature, 0).ToString();
                        str_humidity = BitConverter.ToInt16(byte_humidity, 0).ToString();
                       
                        info = new bean.dataSerialization();

                        if (str_temperature.Length > 2)
                        {
                            info.temperature = str_temperature.Insert(2, ".");
                        }
                        else if (str_temperature.Length == 2)
                        {
                            info.temperature = str_temperature.Insert(1, ".");
                        }
                        else
                        {
                            info.temperature = str_temperature;
                        }
                        if (str_humidity.Length > 2)
                        {
                            info.humidity = str_humidity.Insert(2, ".");
                        }
                        else if (str_humidity.Length == 2)
                        {
                            info.humidity = str_humidity.Insert(1, ".");
                        }
                        else
                        {
                            info.humidity = str_humidity;
                        }
                        if (Int32.Parse(binary) > 0)
                        {
                            info.temperature = info.temperature.Insert(0, "-");
                        }
                        if (no.ToString().Length == 1)
                        {
                            info.deviceNum = no.ToString().Insert(0, "0");
                        }
                        else
                        {
                            info.deviceNum = no.ToString();
                        }
                        if (measureCode != null && !"".Equals(measureCode))
                        {
                            info.managerID = measureCode;
                        }
                        info.devicedate = datetime;
                        info.sysdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        info.measureMeterCode = measureCode + "_" + info.deviceNum;
                        dtB = adddatas.checkLastRecordBIsOr(info.measureMeterCode);
                        if (dtB.Rows[0][1].ToString() == "1") { Whistory = 1; } else { Whistory = 0; }; 
                        if (dtB.Rows[0][2].ToString() == "2") { history = 2; } else { history = 0; };
                        if (intervalNum1==2)
                        {
                            DataRow[] drs = dtcdinfo1.Select("measureCode='" + info.managerID + "' and meterNo='" + info.deviceNum + "'");
                            tt = Double.Parse(info.temperature);
                            t1 = Double.Parse(drs[0]["t_high"].ToString());
                            t2 = Double.Parse(drs[0]["t_low"].ToString());
                            hh = Double.Parse(info.humidity);
                            h1 = Double.Parse(drs[0]["h_high"].ToString());
                            h2 = Double.Parse(drs[0]["h_low"].ToString());
                            if (tt > t1 || tt < t2 || hh > h1 || hh < h2)
                            {
                                info.warningistrue = "2";
                            }
                            else if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2)
                            {
                                info.warningistrue = "3";
                                history = 0;
                            }
                            else {
                                history = 0;
                            }
                            if (Int32.Parse(powerClose) == 1)
                            {
                                info.warnState = powerClose;
                               
                            }
                            else if (Int32.Parse(powerClose) != 1 && Whistory == 1) { info.warnState = "3"; Whistory = 0; } else { Whistory = 0; };
                        }
                        if (intervalNum2 == 5) {
                            info.carinterval = "5";
                        }
                        if (intervalNum3 == 3)
                        {
                            info.houseinterval = "30";
                        }
                        if (Int32.Parse(powerClose) == 1)
                        {
                            info.sign = "1";
                        }
                        else if (Int32.Parse(powerClose) != 1 && Whistory == 1) { info.sign = "3"; Whistory = 0; } else { Whistory = 0; };
                        lds.Add(info);
                    }
                    
                    if (lds.Count>0) {
                        //adddatas.addData(lds);//把x86管理主机的数据插入数据库
                        Thread multiAdd = new Thread(new ThreadStart(addData));
                        multiAdd.IsBackground = false;
                        multiAdd.Start();
                        if (logrizhi == 1)
                        {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:/log.txt", true))
                            {
                                sw.WriteLine("开始：" + time111 + "  结束：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "   成功：" + text + "  插入数量：" + lds.Count.ToString() + "  报文数量：" + totalByteRead.Length.ToString());
                            }
                        }
                    }
                    totalByteRead = new Byte[0];
                    doGetDeviceInfo();
                }

            }
            catch (Exception ee)
            {
            }
        }
        private void addData()
        {
            adddatas.addData(lds);
        }

        private string ToDateString(byte[] bytes)
        {
            string hexString = DateTime.Now.Year.ToString();
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    string str_val = bytes[i].ToString();
                    strB.Append(str_val.Length == 1 ? "0" + str_val : str_val);
                }

                hexString += strB.ToString() + "00";
            }
            return hexString;

        }
        private int IndexOf(byte[] srcBytes, byte[] searchBytes)
        {
            if (srcBytes == null) { return -1; }
            if (searchBytes == null) { return -1; }
            if (srcBytes.Length == 0) { return -1; }
            if (searchBytes.Length == 0) { return -1; }
            if (srcBytes.Length < searchBytes.Length) { return -1; }
            for (int i = 0; i < srcBytes.Length - searchBytes.Length; i++)
            {
                if (srcBytes[i] == searchBytes[0])
                {
                    if (searchBytes.Length == 1) { return i; }
                    bool flag = true;
                    for (int j = 1; j < searchBytes.Length; j++)
                    {
                        if (srcBytes[i + j] != searchBytes[j])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) { return i; }
                }
            }
            return -1;
        }
        string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }

                hexString = strB.ToString();
            }
            return hexString;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

  

        /// <summary>
        /// 查询管理主机下的所有仪表
        /// </summary>
        private void queryMeterIds()
        {
            string ids = null;
            int flag = 0;
            dtcdinfo = dis.checkPointInfo(flag);
            if (dtcdinfo.Rows.Count>0) {
                for (int i=0; i< dtcdinfo.Rows.Count; i++) {
                    ids+=":"+ dtcdinfo.Rows[i][1] + "-" + dtcdinfo.Rows[i][2]; 
                }
                mids=ids.Substring(1);
                //获取-16云数据
                mydelegate = new MyDelegate(yunpingtaiDatas);
                mydelegate.BeginInvoke(null, null);
            }
            dtcdinfo2 = dis.checkPointInfoRc();
            if (dtcdinfo2.Rows.Count > 0)
            {
                ids = null;
                for (int i = 0; i < dtcdinfo2.Rows.Count; i++)
                {
                    ids += ";" + dtcdinfo2.Rows[i][1];
                }
                rcids = ids.Substring(1);
                //获取RC-10/-8平台数据
                //yunpingtaiDataRC();
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
            //xmlDoc = new XmlDocument();
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
            node = xmlDoc.SelectSingleNode("config/autosave02");
            autosave02 = Int32.Parse(node.InnerText);
            node = xmlDoc.SelectSingleNode("config/autosave16");
            autosave16 = Int32.Parse(node.InnerText);
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
        }catch(Exception ee) {
                port.Close();
                rb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                rb.eventInfo = "串口连接失败了！"+ee.Message;
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
            //xmlDoc = new XmlDocument();
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
            da =monitoringservice.rtmonitoring(timeHouse);
            DataTable dts1 =da.Tables[0];
            //num = dt.Rows.Count;
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
        private void toolStripLabel1_Click_1(object sender, EventArgs e)
        {
            this.Close();
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
                this.timer3.Stop();
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
                    service.deleteInvalidDataService did = new service.deleteInvalidDataService();
                    did.deleteInvalidData();

                    service.loginLogService lls = new service.loginLogService();
                    bean.loginLogBean lb = new bean.loginLogBean();
                    lb.name = frmLogin.name;
                    lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    lb.eventInfo = "退出系统！";
                    lls.addCheckLog(lb);
                    saveToXmlsStoptime(DateTime.Now.ToString("yyMMddHHmmss"));
                    if (port.IsOpen)
                    {
                        port.Close();
                    }
                    System.Environment.Exit(0);
                }
            }
        }

        private void 用户登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.ShowDialog();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void toolStripMenuItem5_CheckedChanged(object sender, EventArgs e)
        {
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

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel6_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.ShowDialog();
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
                dtcdinfo1 = dis.checkPointInfo(1);
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
            da = monitoringservice.rtmonitoring(timeHouse);
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
                        if (ddbj != null && !"".Equals(ddbj))
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

                        //a = Double.Parse(temperature);
                        b = Double.Parse(t_high);
                        c = Double.Parse(t_low);
                        //d = Double.Parse(humidity);
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

                        //dtime = dt.Rows[i]["devtime"].ToString();

                        //((PictureBox)this.flowLayoutPanel1.Controls.Find(code + "_" + meter, false)[0]).Tag = terminalname + "," + dtime + "," + code + "," + meter;

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
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (mids!=null && mids.Length > 0) {
                mydelegate = new MyDelegate(yunpingtaiDatas);
                mydelegate.BeginInvoke(null, null);
                //yunpingtaiDatas();
            }

            if (rcids!=null && rcids.Length>0) {
                //yunpingtaiDataRC();
            }
            
        }
        public String getFloat(String floatStr)
        {
            float f = 0;
            if (floatStr == null|| floatStr=="")
            {
                return "0";
            }
            //异常读数处理
            if (floatStr.Trim().Equals("F01")|| floatStr.Trim().Equals("9901.0"))
            {
                return "0";
            }
            int index = floatStr.IndexOf(".");
            floatStr = float.Parse( floatStr).ToString();  // -6[-60]   6[60]  -16[-160]
            if (index>-1) {
                return floatStr;
            }
            else {
                string s1 = floatStr.Substring(0, floatStr.Length - 1);
                string s2 = floatStr.Last().ToString();

                string s3 = s1 + "." + s2;

                try
                {
                    f = float.Parse(s3);
                }
                catch (Exception e)
                {
                   // throw new Exception(e.Message);
                }
            }
            return f.ToString();
        }

        bean.showReportBean rb = new bean.showReportBean();
        private void yunpingtaiDatas()
        {
            //string ids = "862609000079371-00:862609000078639-00:862609000078845-00:862609000079462-00:862609000081609-00:862609000081518-00:862609000081500-00:862609000081617-00:862609000081625-00:862609000081443-00:862609000081716-00:862609000081567-00:862609000081369-00:862609000081559-00:862609000081682-00:862609000081419-00:862609000081484-00:862609000081476-00:862609000081351-00:862609000081740-00:862609000081773-00:862609000081492-00:862609000081401-00:862609000081831-00";
            if (mids != null && !"".Equals(mids))
            {
                string[] midsArray=mids.Split(':');
                int midsNo= midsArray.Length;

                if (midsNo < 51)
                {
                    try
                    {
                        jsons = utils.HttpClient.getDeviceData(mids, ipport);
                    }
                    catch (Exception exc)
                    {                
                        rb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        rb.eventInfo = "连接远程服务器异常！";
                        rb.type = "1";
                        lls.addReport(rb);
                    }
                    if (jsons != null && !"".Equals(jsons) && !"errMsg".Equals(jsons.Substring(2, 6).ToString()))
                    {
                        rb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        rb.eventInfo = "连接远程服务器成功！";
                        rb.type = "1";
                        lls.addReport(rb);
                        list = js.Deserialize<List<bean.dataSerialization>>(jsons);

                        string TarStr1 = "yyyy-MM-dd HH:mm:ss";
                        string TarStr = "yyMMddHHmmss";
                        int intervalNum1 = 0;
                        int intervalNum2 = 0;
                        int intervalNum3 = 0;
                        double tt, t1, t2, hh, h1, h2;
                        IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                        DateTime MyDate;
                        DateTime MyDate1;
                        if (list.Count > 0)
                        {
                            List<bean.dataSerialization> listed = new List<bean.dataSerialization>();
                            int history = 0;
                            int Whistory = 0;
                            foreach (bean.dataSerialization datas in list)
                            {
                                datas.temperature = getFloat(datas.temperature);

                                datas.humidity = getFloat(datas.humidity);

                                if (datas.lng != null && !"".Equals(datas.lng) && datas.lng.Length > 3)
                                {
                                    datas.lng = datas.lng.Insert(3, ".");
                                }
                                else
                                {
                                    datas.lng = "";
                                }
                                if (datas.lat != null && !"".Equals(datas.lat) && datas.lat.Length > 3)
                                {
                                    datas.lat = datas.lat.Insert(2, ".");
                                }
                                else
                                {
                                    datas.lat = "";
                                }
                                if (datas.devicedate != null && datas.devicedate.Length > 0)
                                {
                                    try
                                    {
                                        MyDate = DateTime.ParseExact(datas.devicedate, TarStr, format);
                                    }
                                    catch (Exception exc)
                                    {
                                        MyDate = DateTime.Now;
                                    }
                                    datas.devicedate = MyDate.ToString(TarStr1);
                                    int mm = MyDate.Minute;
                                    if (mm % 2 == 0)
                                    {
                                        intervalNum1 = 2;
                                    }
                                    else
                                    {
                                        intervalNum1 = 0;
                                    }
                                    if (mm % cartime == 0)
                                    {
                                        intervalNum2 = 5;
                                    }
                                    else
                                    {
                                        intervalNum2 = 0;
                                    }
                                    if (mm % housetime == 0)
                                    {
                                        intervalNum3 = 3;
                                    }
                                    else
                                    {
                                        intervalNum3 = 0;
                                    }
                                }
                                if (datas.sysdate != null && datas.sysdate.Length > 0)
                                {
                                    MyDate1 = DateTime.ParseExact(datas.sysdate, TarStr, format);
                                    datas.sysdate = MyDate1.ToString(TarStr1);
                                }
                                datas.measureMeterCode = datas.managerID + "_" + datas.deviceNum;


                                 dtB = adddatas.checkLastRecordBIsOr(datas.measureMeterCode);
                                if (dtB.Rows[0][1].ToString() == "1") { Whistory = 1; } else { Whistory = 0; };
                                if (dtB.Rows[0][2].ToString() == "2") { history = 2; } else { history = 0; };


                                DataRow[] drs = dtcdinfo.Select("measureCode='" + datas.managerID + "' and meterNo='" + datas.deviceNum + "'");

                                tt = Double.Parse(datas.temperature);
                                t1 = Double.Parse(drs[0]["t_high"].ToString());
                                t2 = Double.Parse(drs[0]["t_low"].ToString());
                                hh = Double.Parse(datas.humidity);
                                h1 = Double.Parse(drs[0]["h_high"].ToString());
                                h2 = Double.Parse(drs[0]["h_low"].ToString());
                                string CommunicationType = drs[0]["CommunicationType"].ToString();
                                if (CommunicationType == "LBCC-16" || CommunicationType == "[管理主机]LB863RSB_N1(LBGZ-02)" || CommunicationType == "LBGZ-04" || CommunicationType == "RC-8/-10")
                                {
                                    if (CommunicationType == "LBGZ-04" && datas.charge == "0")
                                    {
                                        datas.sign = "1";
                                        datas.warnState = "1";
                                    }
                                    else if (CommunicationType == "LBGZ-04" && datas.charge != "0" && Whistory == 1)
                                    {
                                        datas.sign = "3";
                                        datas.warnState = "3";
                                        Whistory = 0;
                                    }
                                    else
                                    {
                                    }

                                    if (CommunicationType == "RC-8/-10" && datas.charge == "0")
                                    {
                                        datas.sign = "1";
                                        datas.warnState = "1";
                                    }
                                    else if (CommunicationType == "RC-8/-10" && datas.charge != "0" && Whistory == 1)
                                    {
                                        datas.sign = "3";
                                        datas.warnState = "3";
                                        Whistory = 0;
                                    }
                                    else
                                    {
                                        Whistory = 0;
                                    }

                                    if (tt > t1 || tt < t2 || hh > h1 || hh < h2)
                                    {
                                      datas.warningistrue = "2";
                                    }
                                    else if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2)
                                    {
                                        datas.warningistrue = "3";
                                        history = 0;
                                    }
                                    else
                                    {
                                        datas.warningistrue = "1";
                                        history = 0;
                                    }
                                }
                                else
                                {
                                    if (intervalNum1 == 2)
                                    {
                                        if (tt > t1 || tt < t2 || hh > h1 || hh < h2)
                                        {
                                           datas.warningistrue = "2";
                                        }
                                        else if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2)
                                        {
                                            datas.warningistrue = "3";
                                            history = 0;
                                        }
                                        else {
                                            history = 0;
                                        }
                                    }
                                    if (intervalNum2 == 5)
                                    {
                                        datas.carinterval = "5";
                                    }
                                    if (intervalNum3 == 3)
                                    {
                                        datas.houseinterval = "30";
                                    }
                                }
                                listed.Add(datas);
                            }
                            adddatas.addData(listed);
                        }
                    }
                }
                else
                {

                    int fornum = midsNo / 50 + 1;

                    for (int i = 0; i < fornum; i++)
                    {
                        string mid = "";
                        int endnum = (i + 1) * 50;
                        if (i == fornum - 1)
                        {
                            endnum = i * 50 + (midsNo % 50);
                        }
                        for (int j = i * 50; j < endnum; j++)
                        {
                            mid += ":" + midsArray[j].ToString();
                        }
                        string midnew = mid.Substring(1);
                        try
                        {
                            jsons = utils.HttpClient.getDeviceData(midnew, ipport);
                        }
                        catch (Exception exc)
                        {
                        }

                        if (jsons != null && !"".Equals(jsons) && !"errMsg".Equals(jsons.Substring(2, 6).ToString()))
                        {
                            list = js.Deserialize<List<bean.dataSerialization>>(jsons);

                            string TarStr1 = "yyyy-MM-dd HH:mm:ss";
                            string TarStr = "yyMMddHHmmss";
                            int intervalNum1 = 0;
                            int intervalNum2 = 0;
                            int intervalNum3 = 0;
                            double tt, t1, t2, hh, h1, h2;
                            IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                            DateTime MyDate;
                            DateTime MyDate1;
                            if (list.Count > 0)
                            {
                                List<bean.dataSerialization> listed = new List<bean.dataSerialization>();
                                int history = 0;
                                int Whistory = 0;

                                foreach (bean.dataSerialization datas in list)
                                {
                                    datas.temperature = getFloat(datas.temperature);

                                    datas.humidity = getFloat(datas.humidity);

                                    if (datas.lng != null && !"".Equals(datas.lng) && datas.lng.Length > 3)
                                    {
                                        datas.lng = datas.lng.Insert(3, ".");
                                    }
                                    else
                                    {
                                        datas.lng = "";
                                    }
                                    if (datas.lat != null && !"".Equals(datas.lat) && datas.lat.Length > 3)
                                    {
                                        datas.lat = datas.lat.Insert(2, ".");
                                    }
                                    else
                                    {
                                        datas.lat = "";
                                    }
                                    if (datas.devicedate != null && datas.devicedate.Length > 0)
                                    {
                                        try
                                        {
                                            MyDate = DateTime.ParseExact(datas.devicedate, TarStr, format);
                                        }
                                        catch (Exception exc)
                                        {
                                            continue;
                                        }
                                        datas.devicedate = MyDate.ToString(TarStr1);
                                        int mm = MyDate.Minute;
                                        if (mm % 2 == 0)
                                        {
                                            intervalNum1 = 2;
                                        }
                                        else
                                        {
                                            intervalNum1 = 0;
                                        }
                                        if (mm % cartime == 0)
                                        {
                                            intervalNum2 = 5;
                                        }
                                        else
                                        {
                                            intervalNum2 = 0;
                                        }
                                        if (mm % housetime == 0)
                                        {
                                            intervalNum3 = 3;
                                        }
                                        else
                                        {
                                            intervalNum3 = 0;
                                        }
                                    }
                                    if (datas.sysdate != null && datas.sysdate.Length > 0)
                                    {
                                        MyDate1 = DateTime.ParseExact(datas.sysdate, TarStr, format);
                                        datas.sysdate = MyDate1.ToString(TarStr1);
                                    }
                                    datas.measureMeterCode = datas.managerID + "_" + datas.deviceNum;
                                    dtB = adddatas.checkLastRecordBIsOr(datas.measureMeterCode);
                                    if (dtB.Rows[0][1].ToString() == "1") { Whistory = 1; } else { Whistory = 0; };
                                    if (dtB.Rows[0][2].ToString() == "2") { history = 2; } else { history = 0; };

                                    DataRow[] drs = dtcdinfo.Select("measureCode='" + datas.managerID + "' and meterNo='" + datas.deviceNum + "'");
                                    tt = Double.Parse(datas.temperature);
                                    t1 = Double.Parse(drs[0]["t_high"].ToString());
                                    t2 = Double.Parse(drs[0]["t_low"].ToString());
                                    hh = Double.Parse(datas.humidity);
                                    h1 = Double.Parse(drs[0]["h_high"].ToString());
                                    h2 = Double.Parse(drs[0]["h_low"].ToString());
                                    string CommunicationType = drs[0]["CommunicationType"].ToString();
                                    if (CommunicationType == "LBCC-16" || CommunicationType == "[管理主机]LB863RSB_N1(LBGZ-02)" || CommunicationType == "LBGZ-04" || CommunicationType == "RC-8/-10")
                                    {
                                        if (CommunicationType == "LBGZ-04" && datas.charge == "0")
                                        {
                                            datas.sign = "1";
                                            datas.warnState = "1";
                                        }
                                        else if (CommunicationType == "LBGZ-04" && datas.charge != "0" && Whistory == 1)
                                        {
                                            datas.sign = "3";
                                            datas.warnState = "3";
                                            Whistory = 0;
                                        }
                                        else
                                        {
                                            Whistory = 0;
                                        }


                                        if (CommunicationType == "RC-8/-10" && datas.charge == "0")
                                        {
                                            datas.sign = "1";
                                            datas.warnState = "1";
                                        }
                                        else if (CommunicationType == "RC-8/-10" && datas.charge != "0" && Whistory == 1)
                                        {
                                            datas.sign = "3";
                                            datas.warnState = "3";
                                            Whistory = 0;
                                        }
                                        else
                                        {
                                            Whistory = 0;
                                        }

                                        if (tt > t1 || tt < t2 || hh > h1 || hh < h2)
                                            {
                                              datas.warningistrue = "2";                                                
                                            }
                                            else if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2)
                                            {
                                                datas.warningistrue = "3";
                                                history = 0;
                                            }
                                            else
                                            {
                                                datas.warningistrue = "1";
                                                 history = 0;
                                             }
                                    }
                                    else
                                    {
                                        if (intervalNum1 == 2)
                                        {
                                            if (tt > t1 || tt < t2 || hh > h1 || hh < h2)
                                            {
                                                    datas.warningistrue = "2";
                                            }
                                            else if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2)
                                            {
                                                datas.warningistrue = "3";
                                                history = 0;
                                            }
                                            else
                                            {
                                                history = 0;
                                            }
                                        }
                                        if (intervalNum2 == 5)
                                        {
                                            datas.carinterval = "5";
                                        }
                                        if (intervalNum3 == 3)
                                        {
                                            datas.houseinterval = "30";
                                        }
                                    }
                                    listed.Add(datas);
                                }
                                adddatas.addData(listed);
                            }
                        }
                    }
                }
            }
        }
        //private void yunpingtaiDataRC()
        //{
        //    List<bean.dataSerialization> lstData = new List<bean.dataSerialization>();
        //    string url = "cdc.anquan365.org:8080/api/searchDevCurrentStateList.action?devGroupCode=1014000100030008&bm_dcs$device$devNum_orstr=" + rcids;
        //    try
        //    {
        //        string ret = utils.HttpClient.getDeviceData("", url);
        //        if (ret.Length > 0)
        //        {
        //            JavaScriptSerializer js = new JavaScriptSerializer();
        //            var jarr = js.Deserialize<Dictionary<string, object>>(ret);
        //            string str_code = "";
        //            ArrayList dcsvoList = new ArrayList(); ;
        //            foreach (var j in jarr)
        //            {
        //                if (j.Key.Equals("jsonCode"))
        //                    str_code = j.Value.ToString();
        //                else if (j.Key.Equals("dcsvoList"))
        //                {
        //                    string str_v = js.Serialize(j.Value);
        //                    dcsvoList = (ArrayList)j.Value;
        //                }
        //            }

        //            if (str_code.Equals("200"))
        //            {
        //                int history = 0;
        //                foreach (object outElement in dcsvoList)
        //                {
        //                    string str = js.Serialize(outElement);
        //                    string temper = "";
        //                    string rh = "";
        //                    string rdate = "";
        //                    string maaagerid = "";

        //                    jarr = js.Deserialize<Dictionary<string, object>>(str);
        //                    foreach (var j in jarr)
        //                    {
        //                        if (j.Key.Equals("currentTempdata"))
        //                            temper = j.Value.ToString();
        //                        else if (j.Key.Equals("currentRhdata"))
        //                            rh = j.Value.ToString();
        //                        else if (j.Key.Equals("lastReportTime"))
        //                            rdate = j.Value.ToString();
        //                        else if (j.Key.Equals("device"))
        //                        {
        //                            Dictionary<string, object> dic = (Dictionary<string, object>)j.Value;
        //                            foreach (var k in dic)
        //                            {
        //                                maaagerid = k.Value.ToString();
        //                            }
        //                        }
        //                    }
        //                    bean.dataSerialization devInfo = new bean.dataSerialization();
        //                    devInfo.temperature = temper;
        //                    devInfo.humidity = rh;
        //                    devInfo.devicedate = rdate;
        //                    devInfo.managerID = maaagerid;
        //                    devInfo.deviceNum = "00";
        //                    devInfo.sysdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");       
        //                    devInfo.measureMeterCode = devInfo.managerID + "_" + devInfo.deviceNum;


        //                    dtB = adddatas.checkLastRecordBIsOr(devInfo.measureMeterCode);
        //                    if (dtB.Rows[0][2].ToString() == "2") { history = 2; } else { history = 0; };
        //                    DataRow[] drs = dtcdinfo.Select("measureCode='" + devInfo.managerID + "' and meterNo='" + devInfo.deviceNum + "'");
        //                    Double tt = Double.Parse(devInfo.temperature);
        //                    Double t1 = Double.Parse(drs[0]["t_high"].ToString());
        //                    Double t2 = Double.Parse(drs[0]["t_low"].ToString());
        //                    Double hh = Double.Parse(devInfo.humidity);
        //                    Double h1 = Double.Parse(drs[0]["h_high"].ToString());
        //                    Double h2 = Double.Parse(drs[0]["h_low"].ToString());

        //                    if (tt > t1 || tt < t2 || hh > h1 || hh < h2)
        //                    {
        //                        devInfo.warningistrue = "2";                                
        //                    }
        //                    else if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2)
        //                    {
        //                        devInfo.warningistrue = "3";
        //                        history = 0;
        //                    }
        //                    else
        //                    {
        //                        history = 0;
        //                    }
        //                    devInfo.carinterval = "5";
        //                    lstData.Add(devInfo);
        //                }
        //                adddatas.addData(lstData);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    { }


        //}
        private void doGetDeviceInfo()
        {
            //byte[] byteSend3 = { 0x1B, 0x06, 0x00, 0x03, 0x03, 0x00, 0x22, 0x00, 0x00, 0xFF, 0x39 };//地址03主机请求
            //byte[] byteSend2 = { 0x1B, 0x06, 0x00, 0x02, 0x03, 0x00, 0x22, 0x00, 0x00, 0xFE, 0xE8 };//地址02主机请求
            //byte[] byteSend1 = { 0x1B, 0x06, 0x00, 0x01, 0x03, 0x00, 0x22, 0x00, 0x00, 0xFE, 0xDB };//地址01主机请求

            if (port.IsOpen)
            {
                try
                {
                    if (dtCom != null && dtCom.Rows.Count > 0)
                    {
                        //for (int i = 0; i < dtCom.Rows.Count; i++)
                        //{
                        //    string address = dtCom.Rows[i][2].ToString();
                        //    byte[] byteSend = getCRC(address);
                        //    measureCode = dtCom.Rows[i][7].ToString();
                        //    totalByteRead = new Byte[0];
                        //    port.Write(byteSend, 0, byteSend.Length);
                        //    Thread.Sleep(2500);
                        //}
                        if (comnum < dtCom.Rows.Count)
                        {
                            string address = dtCom.Rows[comnum][2].ToString();
                            byte[] byteSend = getCRC(address);
                            measureCode = dtCom.Rows[comnum][7].ToString();
                            totalByteRead = new Byte[0];
                            port.Write(byteSend, 0, byteSend.Length);
                            comnum += 1;
                        }
                        else
                        {
                            port.Close();
                            comnum = 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    //Thread.Sleep(500);
                    //doGetDeviceInfo();
                }
            }
            else
            {
                try
                {
                    //if (istrueport)
                    //{
                        initPort(result[1]);
                        //port.Open();
                        doGetDeviceInfo();
                    //}
                }
                catch (Exception e)
                {
                }
            }
        }
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
            //string aaa = CRC.ByteArrayToHexString(byteSends);
            return byteSends;
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Close();
                //MessageBox.Show("Closed");
            }  
        }
        private void timerGetdeviceinfo_Tick(object sender, EventArgs e)
        {
            //Thread multidoGetDev = new Thread(new ThreadStart(doGetDeviceInfo));
            //multidoGetDev.IsBackground = true;
            //multidoGetDev.Start();
            doGetDeviceInfo();
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
            dtcdinfo1 = dis.checkPointInfo(1);
            this.timer2.Stop();
            queryMeterIds();
            this.timer2.Start();
            //重新加载本页面
            if (result != null&&result[0] == "1")
            {
                this.timerGetdeviceinfo.Stop();
                dtCom = mhs.queryManageHostCom();
                timerGetdeviceinfo_Tick(sender, e);
                this.timerGetdeviceinfo.Start();
            }
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
                dtcdinfo1 = dis.checkPointInfo(1);
                //重新加载本页面
                this.timer2.Stop();
                queryMeterIds();
                this.timer2.Start();
                //重新加载本页面
                if (result!=null&& result[0] == "1")
                {
                    this.timerGetdeviceinfo.Stop();
                    dtCom = mhs.queryManageHostCom();
                    timerGetdeviceinfo_Tick(sender, e);
                    this.timerGetdeviceinfo.Start();
                }
                }
            }
            else
            {
                this.timer2.Stop();
                //timer2_Tick(null, null); 
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
                        utils.DataBaseUtil.backupDatabase(@"new.baw", @filepath + @"Datas" + @time + @".baw");
                        textFile(@str + "/manualBackupTimes.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        MessageBox.Show("数据备份成功！备份到：" + @filepath + "的路径下");
                    }
                    catch (Exception exc)
                    {
                       // MessageBox.Show(exc.Message);
                        //throw new Exception(exc.Message);
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
                try
                {
                    if (sb.istrue)
                    {
                        utils.DataBaseUtil.backupDatabase(@"new.baw", @filepath + @"DatasNews.baw");
                    }
                    else
                    {
                        utils.DataBaseUtil.backupDatabase(@"new.baw", @filepath + @"Datas" + @time + @".baw");
                    }
                    textFile(@str + "/automateBackupTimes.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //MessageBox.Show("数据备份成功！备份到：" + @filepath + "的路径下");
                }
                catch (Exception exc)
                {
                    //MessageBox.Show(exc.Message);
                    //throw new Exception(exc.Message);
                }
            }
            if (filepath != null && !"".Equals(filepath))
            {
                if (!System.IO.Directory.Exists(filepath))
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }
                try
                {
                    if (sb.istrue)
                    {
                        utils.DataBaseUtil.backupDatabase(@"new.baw", @filepath + @"DatasNews.baw");
                    }
                    else
                    {
                        utils.DataBaseUtil.backupDatabase(@"new.baw", @filepath + @"Datas" + @time + @".baw");
                    }
                    textFile(@str + "/automateBackupTimes.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                }
                catch (Exception exc)
                {
                    //MessageBox.Show(exc.Message);
                    //throw new Exception(exc.Message);
                }
            }
            //}
        }
        private void saveToXmldatabasetimer(string jtime)
        {
            //xmlDoc = new XmlDocument();
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
                MessageBox.Show("恢复数据库成功!");
                this.timer2.Stop();
                this.timer3.Stop();
                frmMain_Load(sender, e);
                //this.Close();
                //new System.Threading.Mutex(true, Application.ProductName).ReleaseMutex();
                //Application.Restart();
                //System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }

        private void 库房管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            houseManage hm = new houseManage();
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
                //this.ShowInTaskbar = false;
                //this.Hide();
                //this.notifyIcon1.Visible = true;
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
                    dtcdinfo1 = dis.checkPointInfo(1);
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
            //DialogResult result = MessageBox.Show("软件系统将最小化到右下角的图标托盘中？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (result == DialogResult.Yes)
            //{
            //    //System.Environment.Exit(0);
            //    e.Cancel = true;
            //    this.notifyIcon1.Visible = true;
            //    this.Hide();
            //}设备1
            //else {
            //    e.Cancel = true;
            //}
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
            //xmlDoc = new XmlDocument();
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
                    //MessageBox.Show(exc.Message);
                    //throw new Exception(exc.Message);
                }
            }
        }
        private void saveToXmlsStoptime(string stoptime)
        {
            //xmlDoc = new XmlDocument();
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
            //xmlDoc = new XmlDocument();
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

        private void 数据同步ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataSynchronous dsh = new dataSynchronous();
            dsh.ShowDialog();
        }
    }
}
