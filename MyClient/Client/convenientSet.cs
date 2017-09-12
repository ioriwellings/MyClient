using System;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class convenientSet : Form
    {
        SerialPort port=new SerialPort();
        public delegate void UpdateAcceptTextBoxTextHandler(string text);
        public UpdateAcceptTextBoxTextHandler UpdateTextHandler;
        public delegate void SendDeviceInfoHandler(string text);
        public SendDeviceInfoHandler SendInfotHandler;
        public delegate void invokeDisplay(string str);
        Byte[] totalByteRead = new Byte[0];
        string text = string.Empty;
        int flag = 0;
        string id = "";
        string address = "";
        string TarStr1 = "yyyy-MM-dd HH:mm:ss";
        string TarStr = "yyMMddHHmmss";
        IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
        DateTime MyDate;

        public convenientSet()
        {
            InitializeComponent();
            this.port.DataReceived += new SerialDataReceivedEventHandler(this.mySerialPort_DataReceived);
        }

        private void convenientSet_Load(object sender, EventArgs e) 
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/cancel.png");

            id = this.textBox4.Text.Split('-').Last();
            address = this.textBox5.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = this.textBox3.Text;
            string codemeter = this.textBox4.Text;
            string t_high = this.numericUpDown4.Value.ToString();
            string t_low = this.numericUpDown2.Value.ToString();
            string h_high = this.numericUpDown1.Value.ToString();
            string h_low = this.numericUpDown5.Value.ToString();
            if (name != null && double.Parse(t_high) > double.Parse(t_low))
            { 
                bean.deviceInformation di = new bean.deviceInformation();
                string[] mm = codemeter.Split('-');
                di.measureCode = mm[0];
                di.meterNo = mm[1];
                di.terminalname = name;
                di.t_high = float.Parse(t_high);
                di.t_low = float.Parse(t_low);
                di.h_high = float.Parse(h_high);
                di.h_low = float.Parse(h_low);
                if (this.checkBox2.Checked) {
                    di.powerflag = Int32.Parse(this.checkBox2.Tag.ToString());
                }
                service.deviceInformationService dis = new service.deviceInformationService();
                bool isok = dis.updateIformation(di);
                if (isok)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                MessageBox.Show("测点名称不能为空，温度上限应大于温度下限！");
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            this.checkBox2.Checked = false;
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            this.checkBox1.Checked = false;
        }
        private void initPort()
        {
            try
            {
                int baudRate = 9600;
                port.PortName = this.comboBox5.Text;
                port.BaudRate = baudRate;
                port.DtrEnable = true;
                port.ReceivedBytesThreshold = 1;
                if (!port.IsOpen)
                {
                    port.Open();
                }
            }
            catch
            {
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.label17.Text = "";
            initPort();
            if (port.IsOpen)
            {
                if (address != null && !"".Equals(address) && id != null && !"".Equals(id))
                {
                    byte[] byteSend = getCRC(address, id);
                    port.Write(byteSend, 0, byteSend.Length);
                    Thread.Sleep(200);
                    port.Close();
                }
            }
        }
        private byte[] getCRC(string text,string id)
        {
            byte[] byteSends = { 0x1B, 0x06, 0x00, 0x01, 0x03, 0x00, 0x33, 0x00, 0x00, 0xAF, 0x22 };
            byte[] byteSend = { 0x00, 0x00, 0x03, 0x00, 0x33, 0x00, 0x00 };
            int idint = Int32.Parse(id);
            if (idint > 9) {
                id = idint.ToString("X2");
            }
            byteSend[3] = (byte)Convert.ToInt32("0x" + id, 16);
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
            byteSends[5] = (byte)Convert.ToInt32("0x" + id, 16);
            byteSends[9] = (byte)Convert.ToInt32("0x" + xx.Substring(0, 2), 16);
            byteSends[10] = (byte)Convert.ToInt32("0x" + xx.Substring(2), 16);
            return byteSends;
        }
        private void mySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (flag!=0) {
               Thread.Sleep(100);
            }
            Boolean isCRC = false;
            Boolean isTruetime = false;
            try
            {
                SerialPort sp = (SerialPort)sender;
                int size = sp.BytesToRead;
                Byte[] byteRead = new Byte[size];
                if (byteRead.Length == 0)
                {
                    port.Close();
                }
                sp.Read(byteRead, 0, size);
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
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
                        this.label17.Text = "读取成功";
                    }
                    else
                    {
                        isCRC = false;
                        //this.label17.Text = "请重新打开再读取！";
                    }
                    if (totalByteRead.Length == 11)
                    {
                        this.label17.Text = "设置成功";
                        isCRC = false;
                        totalByteRead = new Byte[0];
                    }
                    if (totalByteRead.Length == 19 || totalByteRead.Length == 41)
                    {
                        isCRC = false;
                        isTruetime = true;
                    }
                }
                if (isCRC)
                {
                    byte[] up_temper = totalByteRead.Skip(8).Take(2).ToArray();
                    string cc = ((up_temper[0] << 8 | up_temper[1])).ToString();
                    string t_high = getFloat(cc);
                    int high_temper = (int)totalByteRead.Skip(18).Take(1).ToArray()[0];
                    if (high_temper != 0) {
                        this.numericUpDown10.Value = (decimal)Convert.ToDouble("-"+t_high);
                    }
                    else {
                        this.numericUpDown10.Value = (decimal)Convert.ToDouble(t_high);
                    }

                    byte[] down_temper = totalByteRead.Skip(10).Take(2).ToArray();
                    string t_low= getFloat(((down_temper[0] << 8 | down_temper[1])).ToString());
                    int low_humidity = (int)totalByteRead.Skip(19).Take(1).ToArray()[0];
                    if (low_humidity != 0)
                    {
                        this.numericUpDown8.Value = (decimal)Convert.ToDouble("-" + t_low);
                    }
                    else
                    {
                        this.numericUpDown8.Value = (decimal)Convert.ToDouble(t_low);
                    }
                    byte[] up_humidity = totalByteRead.Skip(12).Take(2).ToArray();
                    string h_high= getFloat(((up_humidity[0] << 8 | up_humidity[1])).ToString());
                    this.numericUpDown9.Value = (decimal)Convert.ToDouble(h_high);

                    byte[] down_humidity = totalByteRead.Skip(14).Take(2).ToArray();
                    string h_low= getFloat(((down_humidity[0] << 8 | down_humidity[1])).ToString());
                    this.numericUpDown7.Value = (decimal)Convert.ToDouble(h_low);

                    this.button6.Enabled = true;
                    totalByteRead = new Byte[0];
                }
                if (isTruetime && flag==1) {
                    string year= Convert.ToString(totalByteRead[6], 10);
                    string moth = Convert.ToString(totalByteRead[7], 10);
                    string day = Convert.ToString(totalByteRead[9], 10);
                    string house = Convert.ToString(totalByteRead[10], 10);
                    string minutes = Convert.ToString(totalByteRead[11], 10);
                    if (moth.Length==1) {
                        moth = "0" + moth;
                    }
                    if (day.Length == 1)
                    {
                        day = "0" + day;
                    }
                    if (house.Length == 1)
                    {
                        house = "0" + house;
                    }
                    if (minutes.Length == 1)
                    {
                        minutes = "0" + minutes;
                    }
                    string time = year + moth + day + house + minutes+"00";
                    try
                    {
                        MyDate = DateTime.ParseExact(time, TarStr, format);
                    }
                    catch{
                        MyDate = DateTime.Now;
                    }
                    this.dateTimePicker1.Value = Convert.ToDateTime(MyDate.ToString(TarStr1));
                    byte[] saveInterval = totalByteRead.Skip(12).Take(2).ToArray();
                    string saveIntervaltime = ((saveInterval[0] << 8 | saveInterval[1])).ToString();
                    this.numericUpDown11.Value = (decimal)Convert.ToDouble(saveIntervaltime);
                    string cdnum = Convert.ToString(totalByteRead[14], 10);
                    string bjInterval = Convert.ToString(totalByteRead[15], 10);
                    this.numericUpDown13.Value = (decimal)Convert.ToDouble(cdnum);
                    this.numericUpDown12.Value = (decimal)Convert.ToDouble(bjInterval);
                    this.label17.Text = "日期间隔读取成功";
                    this.button8.Enabled = true;
                    this.button3.Enabled = true;
                    totalByteRead = new Byte[0];
                }
                if (isTruetime && flag == 2)
                {
                    string pno1 = ((char)totalByteRead[6]).ToString() + ((char)totalByteRead[7]).ToString() + ((char)totalByteRead[8]).ToString() + ((char)totalByteRead[9]).ToString() + ((char)totalByteRead[10]).ToString() + ((char)totalByteRead[11]).ToString() + ((char)totalByteRead[12]).ToString() + ((char)totalByteRead[13]).ToString() + ((char)totalByteRead[14]).ToString() + ((char)totalByteRead[15]).ToString() + ((char)totalByteRead[16]).ToString();
                    string pno2 = "";
                    string pno3 = "";
                    if (totalByteRead.Length==41) {
                        pno2 = ((char)totalByteRead[17]).ToString() + ((char)totalByteRead[18]).ToString() + ((char)totalByteRead[19]).ToString() + ((char)totalByteRead[20]).ToString() + ((char)totalByteRead[21]).ToString() + ((char)totalByteRead[22]).ToString() + ((char)totalByteRead[23]).ToString() + ((char)totalByteRead[24]).ToString() + ((char)totalByteRead[25]).ToString() + ((char)totalByteRead[26]).ToString() + ((char)totalByteRead[27]).ToString();
                        pno3 = ((char)totalByteRead[28]).ToString() + ((char)totalByteRead[29]).ToString() + ((char)totalByteRead[30]).ToString() + ((char)totalByteRead[31]).ToString() + ((char)totalByteRead[32]).ToString() + ((char)totalByteRead[33]).ToString() + ((char)totalByteRead[34]).ToString() + ((char)totalByteRead[35]).ToString() + ((char)totalByteRead[36]).ToString() + ((char)totalByteRead[37]).ToString() + ((char)totalByteRead[38]).ToString();
                    }
                    this.textBox6.Text = pno1;
                    this.textBox7.Text = pno2;
                    this.textBox8.Text = pno3;
                    this.label17.Text = "短信手机号读取成功";
                    this.button4.Enabled = true;
                    totalByteRead = new Byte[0];
                }
            }
            catch (Exception ee)
            {
              
            }
            
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
        public String getFloat(String floatStr)
        {
            float f = 0;
            if (floatStr == null || floatStr == "")
            {
                return "0";
            }
            //异常读数处理
            if (floatStr.Trim().Equals("F01") || floatStr.Trim().Equals("9901.0"))
            {
                return "0";
            }
            int index = floatStr.IndexOf(".");
            floatStr = float.Parse(floatStr).ToString();  // -6[-60]   6[60]  -16[-160]
            if (index > -1)
            {
                return floatStr;
            }
            else
            {
                string s1 = floatStr.Substring(0, floatStr.Length - 1);
                string s2 = floatStr.Last().ToString();

                string s3 = s1 + "." + s2;

                try
                {
                    f = float.Parse(s3);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return f.ToString("0.0");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.label17.Text = "";
            string ab1 = "";
            string ab2 = "";
            string ac1 = "";
            string ac2 = "";
            string aa1 = "";
            string aa2 = "";
            string ad1 = "";
            string ad2 = "";
            string s1 = "00";
            string s2 = "00";

            string t_high = this.numericUpDown10.Value.ToString("0.0");
            string t_low = this.numericUpDown8.Value.ToString("0.0");
            string h_high = this.numericUpDown9.Value.ToString("0.0");
            string h_low = this.numericUpDown7.Value.ToString("0.0");

            double t_highdou = double.Parse(t_high);
            double t_lowdou = double.Parse(t_low);
            double h_highdou = double.Parse(h_high);
            double h_lowdou = double.Parse(h_low);

            if ( t_highdou > t_lowdou && h_highdou > h_lowdou)
            {

                if (t_high.IndexOf("-") > -1)
                {
                    t_high = t_high.Replace("-", "");
                    s1 = "11";
                }
                if (t_high.IndexOf(".") > -1)
                {
                    t_high = t_high.Replace(".", "");
                }

                if (t_low.IndexOf("-") > -1)
                {
                    t_low = t_low.Replace("-", "");
                    s2 = "11";
                }
                if (t_low.IndexOf(".") > -1)
                {
                    t_low = t_low.Replace(".", "");
                }

                if (h_high.IndexOf(".") > -1)
                {
                    h_high = h_high.Replace(".", "");
                }

                if (h_low.IndexOf(".") > -1)
                {
                    h_low = h_low.Replace(".", "");
                }
                string ab = Int32.Parse(t_high).ToString("X2");
                string ac = Int32.Parse(t_low).ToString("X2");
                string aa = Int32.Parse(h_high).ToString("X2");
                string ad = Int32.Parse(h_low).ToString("X2");
                
                if (ab.Length == 3)
                {
                    ab1 = "0" + ab.Substring(0, 1);
                    ab2 = ab.Substring(1, 2);
                }
                else if (ab.Length == 2)
                {
                    ab1 = "00";
                    ab2 = ab;
                }
                if (ac.Length == 3)
                {
                    ac1 = "0" + ac.Substring(0, 1);
                    ac2 = ac.Substring(1, 2);
                }
                else if (ac.Length == 2)
                {
                    ac1 = "00";
                    ac2 = ac;
                }
                if (aa.Length == 3)
                {
                    aa1 = "0" + aa.Substring(0, 1);
                    aa2 = aa.Substring(1, 2);
                }
                else if (aa.Length == 2)
                {
                    aa1 = "00";
                    aa2 = aa;
                }
                if (ad.Length == 3)
                {
                    ad1 = "0" + ad.Substring(0, 1);
                    ad2 = ad.Substring(1, 2);
                }
                else if (ad.Length == 2)
                {
                    ad1 = "00";
                    ad2 = ad;
                }
                initPort();
                if (port.IsOpen)
                {
                    if (address != null && !"".Equals(address) && id != null && !"".Equals(id))
                    {
                        byte[] byteSend = getCRCWrite(address, id, ab1, ab2, ac1, ac2, aa1, aa2, ad1, ad2, s1, s2);
                        port.Write(byteSend, 0, byteSend.Length);
                        Thread.Sleep(300);
                        port.Close();
                        this.button6.Enabled = false;
                    }
                }
            }
            else {
                MessageBox.Show("你未设置温湿度上下限，并且上限要大于下限！");
            }
        }
        private byte[] getCRCWrite(string text, string id, string ab1, string ab2, string ac1, string ac2, string aa1, string aa2, string ad1, string ad2,string s1,string s2)
        {                    
            byte[] byteSends = { 0x1B, 0x06, 0x00, 0x01, 0x10, 0x01, 0x33, 0x00, 0x00, 0x0C, 0x01, 0x2C, 0x00, 0x00, 0x02, 0xEE, 0x00, 0x00, 0x14, 0x32, 0x00, 0x00, 0x4B, 0x20 };
            byte[] byteSend = { 0x00, 0x01, 0x10, 0x01, 0x33, 0x00, 0x00, 0x0C, 0x01, 0x2C, 0x00, 0x00, 0x02, 0xEE, 0x00, 0x00, 0x14, 0x32, 0x00, 0x00 };
            if (Int32.Parse(text) < 10)
            {
                text = "0" + text;
            }
            if (Int32.Parse(id) > 9)
            {
                id = Int32.Parse(id).ToString("X2");
            }
            byteSend[1] = (byte)Convert.ToInt32("0x" + text, 16);
            byteSend[3] = (byte)Convert.ToInt32("0x" + id, 16);
            byteSend[8] = (byte)Convert.ToInt32("0x" + ab1, 16);
            byteSend[9] = (byte)Convert.ToInt32("0x" + ab2, 16);
            byteSend[10] = (byte)Convert.ToInt32("0x" + ac1, 16);
            byteSend[11] = (byte)Convert.ToInt32("0x" + ac2, 16);
            byteSend[12] = (byte)Convert.ToInt32("0x" + aa1, 16);
            byteSend[13] = (byte)Convert.ToInt32("0x" + aa2, 16);
            byteSend[14] = (byte)Convert.ToInt32("0x" + ad1, 16);
            byteSend[15] = (byte)Convert.ToInt32("0x" + ad2, 16);
            byteSend[18] = (byte)Convert.ToInt32("0x" + s1, 16);
            byteSend[19] = (byte)Convert.ToInt32("0x" + s2, 16);
            uint crcRet = CRC1.calcrc16(byteSend, (uint)byteSend.Length);
            string xx = crcRet.ToString("X");
            if (xx.Length == 3)
            {
                xx = "0" + xx;
            }
            byteSends[3] = (byte)Convert.ToInt32("0x" + text, 16);
            byteSends[5] = (byte)Convert.ToInt32("0x" + id, 16);
            byteSends[10] = (byte)Convert.ToInt32("0x" + ab1, 16);
            byteSends[11] = (byte)Convert.ToInt32("0x" + ab2, 16);
            byteSends[12] = (byte)Convert.ToInt32("0x" + ac1, 16);
            byteSends[13] = (byte)Convert.ToInt32("0x" + ac2, 16);
            byteSends[14] = (byte)Convert.ToInt32("0x" + aa1, 16);
            byteSends[15] = (byte)Convert.ToInt32("0x" + aa2, 16);
            byteSends[16] = (byte)Convert.ToInt32("0x" + ad1, 16);
            byteSends[17] = (byte)Convert.ToInt32("0x" + ad2, 16);
            byteSends[20] = (byte)Convert.ToInt32("0x" + s1, 16);
            byteSends[21] = (byte)Convert.ToInt32("0x" + s2, 16);
            byteSends[22] = (byte)Convert.ToInt32("0x" + xx.Substring(0, 2), 16);
            byteSends[23] = (byte)Convert.ToInt32("0x" + xx.Substring(2), 16);
            return byteSends;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string dt24 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.dateTimePicker1.Value= Convert.ToDateTime(dt24);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.label17.Text = "";
            initPort();
            if (port.IsOpen)
            {
                if (address != null && !"".Equals(address))
                {
                    flag = 1;
                    byte[] byteSend = getCRCtime(address);
                    port.Write(byteSend, 0, byteSend.Length);
                    Thread.Sleep(200);
                    port.Close();
                }
            }
        }
        private byte[] getCRCtime(string text)
        {
            byte[] byteSends = { 0x1B, 0x06, 0x00, 0x01, 0x03, 0x00, 0x55, 0x00, 0x00, 0xAF, 0x22 };
            byte[] byteSend = { 0x00, 0x01, 0x03, 0x00, 0x55, 0x00, 0x00 };
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

        private void button8_Click(object sender, EventArgs e)
        {
            this.label17.Text = "";
            string jgtime1 = "";
            string jgtime2 = "";
            string TarStr2 = "yy,MM,dd,HH,mm";
            string jgtime = this.numericUpDown11.Value.ToString();
            string cdnum = this.numericUpDown13.Value.ToString();
            string bj = this.numericUpDown12.Value.ToString();
            string time1 = this.dateTimePicker1.Text.ToString();
            if (Int32.Parse(jgtime)>0&& time1 != null && !"".Equals(time1)&& Int32.Parse(cdnum) > 0 && Int32.Parse(bj) > 0) {
                jgtime = Int32.Parse(jgtime).ToString("X2");
                if (jgtime.Length == 3)
                {
                    jgtime1 = "0" + jgtime.Substring(0, 1);
                    jgtime2 = jgtime.Substring(1, 2);
                }
                else if (jgtime.Length == 2)
                {
                    jgtime1 = "00";
                    jgtime2= jgtime;
                }
                MyDate = DateTime.ParseExact(time1, TarStr1, format);
                string[] times=MyDate.ToString(TarStr2).Split(',');
                string year = Int32.Parse(times[0]).ToString("X2");
                string moth = Int32.Parse(times[1]).ToString("X2");
                string day = Int32.Parse(times[2]).ToString("X2");
                string house = Int32.Parse(times[3]).ToString("X2");
                string minutes = Int32.Parse(times[4]).ToString("X2");
                GregorianCalendar gc = new GregorianCalendar();
                int weekOfYear = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                string week = weekOfYear.ToString("X2");
                cdnum = Int32.Parse(cdnum).ToString("X2");
                bj = Int32.Parse(bj).ToString("X2");
                initPort();
                if (port.IsOpen)
                {
                    if (address != null && !"".Equals(address))
                    {
                        byte[] byteSend = getCRCSetTime(address, year, moth, week, day, house, minutes, jgtime1, jgtime2,cdnum,bj);
                        port.Write(byteSend, 0, byteSend.Length);
                        Thread.Sleep(200);
                        port.Close();
                        this.button8.Enabled = false;
                    }
                }
            }
        }
        private byte[] getCRCSetTime(string text, string year, string moth, string week,string day, string house, string minutes, string jgtime1, string jgtime2, string cdnum, string bj)
        {
            byte[] byteSends = { 0x1B, 0x06, 0x00, 0x01, 0x10, 0x00, 0x55, 0x00, 0x00, 0x0B, 0x11, 0x03, 0x0C, 0x17, 0x11, 0x3A, 0x00, 0x01, 0x05, 0x01, 0x00, 0x49, 0x17 };
            byte[] byteSend = { 0x00, 0x01, 0x10, 0x00, 0x55, 0x00, 0x00, 0x0B, 0x11, 0x03, 0x0C, 0x17, 0x11, 0x3A, 0x00, 0x01, 0x03, 0x01, 0x00 };
            if (Int32.Parse(text) < 10)
            {
                text = "0" + text;
            }
            byteSend[1] = (byte)Convert.ToInt32("0x" + text, 16);
            byteSend[8] = (byte)Convert.ToInt32("0x" + year, 16);
            byteSend[9] = (byte)Convert.ToInt32("0x" + moth, 16);
            byteSend[10] = (byte)Convert.ToInt32("0x" + week, 16);
            byteSend[11] = (byte)Convert.ToInt32("0x" + day, 16);
            byteSend[12] = (byte)Convert.ToInt32("0x" + house, 16);
            byteSend[13] = (byte)Convert.ToInt32("0x" + minutes, 16);
            byteSend[14] = (byte)Convert.ToInt32("0x" + jgtime1, 16);
            byteSend[15] = (byte)Convert.ToInt32("0x" + jgtime2, 16);
            byteSend[16] = (byte)Convert.ToInt32("0x" + cdnum, 16);
            byteSend[17] = (byte)Convert.ToInt32("0x" + bj, 16);
            uint crcRet = CRC1.calcrc16(byteSend, (uint)byteSend.Length);
            string xx = crcRet.ToString("X");
            if (xx.Length == 3)
            {
                xx = "0" + xx;
            }
            byteSends[3] = (byte)Convert.ToInt32("0x" + text, 16);
            byteSends[10] = (byte)Convert.ToInt32("0x" + year, 16);
            byteSends[11] = (byte)Convert.ToInt32("0x" + moth, 16);
            byteSends[12] = (byte)Convert.ToInt32("0x" + week, 16);
            byteSends[13] = (byte)Convert.ToInt32("0x" + day, 16);
            byteSends[14] = (byte)Convert.ToInt32("0x" + house, 16);
            byteSends[15] = (byte)Convert.ToInt32("0x" + minutes, 16);
            byteSends[16] = (byte)Convert.ToInt32("0x" + jgtime1, 16);
            byteSends[17] = (byte)Convert.ToInt32("0x" + jgtime2, 16);
            byteSends[18] = (byte)Convert.ToInt32("0x" + cdnum, 16);
            byteSends[19] = (byte)Convert.ToInt32("0x" + bj, 16);
            byteSends[21] = (byte)Convert.ToInt32("0x" + xx.Substring(0, 2), 16);
            byteSends[22] = (byte)Convert.ToInt32("0x" + xx.Substring(2), 16);
            return byteSends;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.label17.Text = "";
            initPort();
            if (port.IsOpen)
            {
                if (address != null && !"".Equals(address))
                {
                    flag = 2;
                    byte[] byteSend = getCRCrReadPhotoNo(address);
                    port.Write(byteSend, 0, byteSend.Length);
                    Thread.Sleep(200);
                    port.Close();

                }
            }
        }
        
        private byte[] getCRCrReadPhotoNo(string text)
        {
            byte[] byteSends = { 0x1B, 0x06, 0x00, 0x01, 0x03, 0x00, 0x11, 0x00, 0x00, 0x0e, 0xd4 };
            byte[] byteSend = { 0x00, 0x03, 0x03, 0x00, 0x11, 0x00, 0x00 };
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
        private void button4_Click(object sender, EventArgs e)
        {
            string pno=this.textBox6.Text;
            Regex regex = new Regex("^[0-9]*[0-9][0-9]*$");
            bool istrue= regex.IsMatch(pno);
            if (pno.Length == 11 && istrue)
            {
                char[] pnoarray = pno.ToArray();
                string p1 = ((int)pnoarray[0]).ToString("X2");
                string p2 = ((int)pnoarray[1]).ToString("X2");
                string p3 = ((int)pnoarray[2]).ToString("X2");
                string p4 = ((int)pnoarray[3]).ToString("X2");
                string p5 = ((int)pnoarray[4]).ToString("X2");
                string p6 = ((int)pnoarray[5]).ToString("X2");
                string p7 = ((int)pnoarray[6]).ToString("X2");
                string p8 = ((int)pnoarray[7]).ToString("X2");
                string p9 = ((int)pnoarray[8]).ToString("X2");
                string p10 = ((int)pnoarray[9]).ToString("X2");
                string p11 = ((int)pnoarray[10]).ToString("X2");

                initPort();
                if (port.IsOpen)
                {
                    if (address != null && !"".Equals(address))
                    {
                        byte[] byteSend = getCRCrSetPhotoNo(address,p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11);
                        port.Write(byteSend, 0, byteSend.Length);
                        Thread.Sleep(200);
                        port.Close();
                        this.button4.Enabled = false;
                    }
                }

            }
            else {
                MessageBox.Show("手机号输入不正确！");
            }
        }
        private byte[] getCRCrSetPhotoNo(string text, string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11)
        {
            byte[] byteSends = { 0x1B, 0x06, 0x00, 0x03, 0x10, 0x00, 0x11, 0x00, 0x00, 0x0B, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x0E, 0xAC };
            byte[] byteSend = { 0x00, 0x03, 0x10, 0x00, 0x11, 0x00, 0x00, 0x0B, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31 };
            if (Int32.Parse(text) < 10)
            {
                text = "0" + text;
            }
            byteSend[1] = (byte)Convert.ToInt32("0x" + text, 16);
            byteSend[8] = (byte)Convert.ToInt32("0x" + p1, 16);
            byteSend[9] = (byte)Convert.ToInt32("0x" + p2, 16);
            byteSend[10] = (byte)Convert.ToInt32("0x" + p3, 16);
            byteSend[11] = (byte)Convert.ToInt32("0x" + p4, 16);
            byteSend[12] = (byte)Convert.ToInt32("0x" + p5, 16);
            byteSend[13] = (byte)Convert.ToInt32("0x" + p6, 16);
            byteSend[14] = (byte)Convert.ToInt32("0x" + p7, 16);
            byteSend[15] = (byte)Convert.ToInt32("0x" + p8, 16);
            byteSend[16] = (byte)Convert.ToInt32("0x" + p9, 16);
            byteSend[17] = (byte)Convert.ToInt32("0x" + p10, 16);
            byteSend[18] = (byte)Convert.ToInt32("0x" + p11, 16);
            uint crcRet = CRC1.calcrc16(byteSend, (uint)byteSend.Length);
            string xx = crcRet.ToString("X");
            if (xx.Length == 3)
            {
                xx = "0" + xx;
            }
            byteSends[3] = (byte)Convert.ToInt32("0x" + text, 16);
            byteSends[10] = (byte)Convert.ToInt32("0x" + p1, 16);
            byteSends[11] = (byte)Convert.ToInt32("0x" + p2, 16);
            byteSends[12] = (byte)Convert.ToInt32("0x" + p3, 16);
            byteSends[13] = (byte)Convert.ToInt32("0x" + p4, 16);
            byteSends[14] = (byte)Convert.ToInt32("0x" + p5, 16);
            byteSends[15] = (byte)Convert.ToInt32("0x" + p6, 16);
            byteSends[16] = (byte)Convert.ToInt32("0x" + p7, 16);
            byteSends[17] = (byte)Convert.ToInt32("0x" + p8, 16);
            byteSends[18] = (byte)Convert.ToInt32("0x" + p9, 16);
            byteSends[19] = (byte)Convert.ToInt32("0x" + p10, 16);
            byteSends[20] = (byte)Convert.ToInt32("0x" + p11, 16);
            byteSends[21] = (byte)Convert.ToInt32("0x" + xx.Substring(0, 2), 16);
            byteSends[22] = (byte)Convert.ToInt32("0x" + xx.Substring(2), 16);
            return byteSends;
        }
    }
}
