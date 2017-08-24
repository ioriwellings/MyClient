using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace LBKJClient
{
    public partial class readHistoryData : Form
    {
        SerialPort port =new SerialPort();
        private string path = @"config.xml";
        public delegate void UpdateAcceptTextBoxTextHandler(string text);
        public UpdateAcceptTextBoxTextHandler UpdateTextHandler;
        public delegate void SendDeviceInfoHandler(string text);
        public SendDeviceInfoHandler SendInfotHandler;
        public delegate void invokeDisplay(string str);
        Byte[] totalByteRead = new Byte[0];
        service.addDataService adddatas = new service.addDataService();//新增数据
        public string address = "";
        public string measureCode = "";
        public string comCode = "";
        int flag = 0;
        string allNum = "";
        string bwnum = "";
        Thread multi;
        DataTable dtcdinfo = null;
        public DataTable dtB = null;//查询测点最后一条记录用
        IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
        List<bean.dataSerialization> lds = null;
        public readHistoryData()
        {
            InitializeComponent();
            this.port.DataReceived += new SerialDataReceivedEventHandler(this.mySerialPort_DataReceived);
        }

        private void readHistoryData_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1_click.BackgroundImage = Image.FromFile(@str + "/images/read.png");
            this.button4.BackgroundImage = Image.FromFile(@str + "/images/stop.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/clear.png");
            this.button3.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            service.deviceInformationService dis = new service.deviceInformationService();
            int flag = 0;
            dtcdinfo = dis.checkPointInfo(flag);
        }

        int Hck = 0;
        private void mySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(200);
            Boolean isCRC = false;
            string cddata = "";
            try
            {
                SerialPort sp = (SerialPort)sender;
                int size = sp.BytesToRead;
                Byte[] byteRead = new Byte[size];
                sp.Read(byteRead, 0, byteRead.Length);
                sp.DiscardInBuffer();
                sp.DiscardOutBuffer();
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
                    byte[] byte_date = totalByteRead.Skip(totalByteRead.Length - 9).Take(5).ToArray();
                    byte[] byte_bwnum = totalByteRead.Skip(totalByteRead.Length - 4).Take(2).ToArray();
                    string str_datetime = ToDateString(byte_date);
                    bwnum = ToBwNumString(byte_bwnum);
                    string powerClose = Int32.Parse(totalByteRead[totalByteRead.Length - 10].ToString("X2")) == 40 ? "1" : "0";
                    if (flag==1) {
                        allNum = bwnum;
                    }
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
                    if (mm % frmMain.cartime == 0)
                    {
                        intervalNum2 = 5;
                    }
                    else
                    {
                        intervalNum2 = 0;
                    }

                    if (mm % frmMain.housetime == 0)
                    {
                        intervalNum3 = 3;
                    }
                    else
                    {
                        intervalNum3 = 0;
                    }

                    lds = new List<bean.dataSerialization>();
                    bean.dataSerialization info = null;
                    int history = 0;
                    int Whistory = 0;

                    string measureMeterCodeB = "";
                    for (int i = 6; i < totalByteRead.Length - 9; i = i + 6)
                    {
                        byte[] newA = totalByteRead.Skip(i).Take(6).ToArray();
                        byte[] byte_temperature = { newA[2], newA[1] };
                        byte[] byte_humidity = { newA[4], newA[3] };
                        no =(Int32.Parse(newA[0].ToString())+1).ToString();
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
                        if (measureCode != null && !"".Equals(measureCode))
                        {
                            info.managerID = measureCode;
                        }
                        
                        info.devicedate = datetime;
                        info.sysdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        info.measureMeterCode = measureCode + "_" + info.deviceNum;

                        measureMeterCodeB = info.measureMeterCode;
                        if (Hck != 0)
                        {
                            dtB = adddatas.checkLastRecordBIsOr(info.measureMeterCode, info.devicedate);
                            if (dtB.Rows[0][1].ToString() == "1") { Whistory = 1; } else { Whistory = 0; };
                            if (dtB.Rows[0][2].ToString() == "2") { history = 2; } else { history = 0; };
                        };
                        if (intervalNum1 == 2)
                        {
                            DataRow[] drs = dtcdinfo.Select("measureCode='" + info.managerID + "' and meterNo='" + info.deviceNum + "'");
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
                            else
                            {
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
                        lds.Add(info);
                        cddata += "测点"+(i/6).ToString()+"   温度："+ info.temperature+"   湿度：" + info.humidity+ "\r\n";
                    }
                    this.richTextBox1.AppendText("\r\n"+ "开始读取历史数据  "+ bwnum + "/" + allNum + "\r\n时间： "+ datetime + "\r\n"+cddata+ "===============================================================\r\n");
                    totalByteRead = new Byte[0];
                    if (lds.Count>0){
                        multi = new Thread(new ThreadStart(addDataHistory));
                        multi.IsBackground = false;
                        multi.Start();
                    }
                }

            }
            catch (Exception ee)
            {
                bwnum = (Int32.Parse(bwnum) - 1).ToString();
                this.richTextBox1.AppendText("\r\n" + "无效报文数据！  " + bwnum + "/" + allNum + "\r\n" + "===============================================================\r\n");
            }
        }

        private void addDataHistory()
        {
            Hck++;
            adddatas.addData(lds);
        }

        private string ToDateString(byte[] bytes)
        {
            string hexString = DateTime.Now.Year.ToString();
            hexString = hexString.Substring(0,2);
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
        private string ToBwNumString(byte[] bytes)
        {
            int num = 0;
            if (bytes != null)
            {
                string bn = ToHexString(bytes).Replace(" ","");
                num = Convert.ToInt32(bn, 16);
            }
            return num.ToString();

        }
        private string ToHexString(byte[] bytes)
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
        private void initPort()
        {
            int baudRate = 9600;
            port.PortName = comCode;
            port.BaudRate = baudRate;
            try
            {
                if (!port.IsOpen)
                {
                    port.Open();
                    port.DiscardOutBuffer();
                    port.DiscardInBuffer();
                }
            }
            catch
            {
                return;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            frmMain.istrueport = false;
            initPort();
            flag = 1;
            requestData();
            this.timer1.Start();
            this.button1_click.Enabled = false;
        }
        private void requestData() {
            if (port.IsOpen)
            {
                byte[] byteSend = getCRC(address);
                totalByteRead = new Byte[0];
                port.Write(byteSend, 0, byteSend.Length);
                Thread.Sleep(2000);
                port.Close();
            }
            //else {
            //    Thread.Sleep(2000);
            //    initPort();
            //    requestData();
            //}
        }

        private byte[] getCRC(string text)
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
        private void button3_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            port.Close();
            frmMain.istrueport = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bwnum != "" && Int32.Parse(bwnum) == 0)
            {
                port.Close();
                this.timer1.Stop();
                this.richTextBox1.AppendText("\r\n历史数据已经读取完毕！");
                return;
            }
            flag = 0;
            this.richTextBox1.Focus();
            this.richTextBox1.SelectionStart = this.richTextBox1.TextLength;
            initPort();
            requestData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            port.Close();
            this.timer1.Stop();
            this.button4.Enabled = false;
            frmMain.istrueport = true;
        }

        private void readHistoryData_FormClosing(object sender, FormClosingEventArgs e)
        {
            button4_Click(sender,null);
        }
    }
}
