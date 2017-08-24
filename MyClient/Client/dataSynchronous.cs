using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;

namespace LBKJClient
{
    public partial class dataSynchronous : Form
    {
        public string starttime, stoptime;
        public DataTable dt = null;
        public DataTable dtB = null;//查询测点最后一条记录用

        private delegate void DelegateFunction(int ipos, string vinfo);
        private string cd = null;
        string ipport = null;
        Thread multi;
        List<bean.dataSerialization> list;
        service.addDataService adddatas = new service.addDataService();//新增数据
        service.deviceInformationService dis = new service.deviceInformationService();
        JavaScriptSerializer js = new JavaScriptSerializer();
        DataTable dtcdinfo = null;
        private string path = @"config.xml";
        public dataSynchronous()
        {
            InitializeComponent();
        }

        private void dataSynchronous_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/assign.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);

            ipport = getFromXml();

            service.changGuicheckService cds = new service.changGuicheckService();
            DataTable dta = cds.checkCedianCar().Tables[0];
            int t = dta.Rows.Count;
            if (t > 0)
            {
                CheckBox ckb = new CheckBox();
                ckb.Text = "全选";
                ckb.Checked = true;
                ckb.Location = new Point(330, 10);
                ckb.Click += new EventHandler(cbk_Click);
                this.tabControl1.TabPages[0].Controls.Add(ckb);
                CheckBox[] checkBox = new CheckBox[t];
                int p = 10;
                for (int i = 0; i < t; i++)
                {
                    checkBox[i] = new CheckBox();
                    checkBox[i].AutoSize = true;
                    checkBox[i].Text = dta.Rows[i]["terminalname"].ToString();
                    string storeType = dta.Rows[i]["storeType"].ToString();
                    checkBox[i].Tag = dta.Rows[i]["measureCode"] + "-" + dta.Rows[i]["meterNo"];
                    checkBox[i].Location = new Point(10, p);
                    this.tabControl1.TabPages[0].Controls.Add(checkBox[i]);
                    p += 20;
                    checkBox[i].Checked = true;
                }
            }

            int flag = 3;
            dtcdinfo = dis.checkPointInfo(flag);
        }
        private void cbk_Click(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (!cb.Checked)
            {
                foreach (Control ctr in this.tabControl1.TabPages[0].Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        ck.Checked = false;
                    }
                }
            }
            else
            {
                foreach (Control ctr in this.tabControl1.TabPages[0].Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        ck.Checked = true;
                    }
                }
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否确认同步服务器历史数据？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.label2.Visible = true;
                this.label5.Visible = true;
                foreach (Control ctr in this.tabControl1.TabPages[0].Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (ck.Checked && !"全选".Equals(ck.Text))
                        {
                            cd += "," + ck.Tag.ToString();
                        }
                    }
                }
                cd = cd.Substring(1);
                string time1 = this.dateTimePicker1.Text.ToString();
                string time2 = this.dateTimePicker2.Text.ToString();

                DateTime dt1 = Convert.ToDateTime(time1);
                DateTime dt2 = Convert.ToDateTime(time2);
                if (cd != null && dt1 < dt2)
                {
                    dt = adddatas.checkDatasTimes(time1, time2);
                    this.button1.Visible = false;
                    starttime = dt1.ToString("yyMMddHHmmss");
                    stoptime = dt2.ToString("yyMMddHHmmss");
                    //insertStopData();
                    multi = new Thread(new ThreadStart(insertStopData));
                    multi.IsBackground = true;
                    multi.Start();
                }
                else
                {
                    MessageBox.Show("开始时间比结束时间大，请重新选择日期时间 / 未选择测点, 请重新选择！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否关闭历史数据同步？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                multi.Abort();
                this.Close();
            }
        }

        string json = "";
        int numa = 0;
        private void insertStopData()
        {
            try
            {
                string[] cdlist = cd.Split(',');
                int avg = 100 / cdlist.Length;
                if (cdlist.Length > 0)
                {
                    List<bean.dataSerialization> listed = null;
                    for (int i = 0; i < cdlist.Length; i++)
                    {
                        numa += 1;
                        listed = new List<bean.dataSerialization>();
                        
                        json = "{\"id\":\"" + cdlist[i] + "\",\"sign\":\"1wVebSp57j67GOV7bQ6IDg==\",\"starttime\":\"" + starttime + "\",\"endtime\":\"" + stoptime + "\"}";
                        string jsonData = utils.HttpClient.getDeviceData(json, ipport);
                        if (jsonData != "" && jsonData.Length > 50)
                        {
                            list = js.Deserialize<List<bean.dataSerialization>>(jsonData);
                            insertData(list);
                            SetPos(numa * avg - 1 < 0 ? 0 : numa * avg - 1, numa.ToString() + "    " + cdlist[i] + "   历史数据同步成功！");
                        }
                        else
                        {
                            SetPos(numa * avg - 1 < 0 ? 0 : numa * avg - 1, numa.ToString() + "    " + cdlist[i] + "   历史数据同步失败！");
                            continue;
                        }
                        Thread.Sleep(1000);
                    }

                    SetPos(100, numa.ToString() + "  仪表数据已同步完成！");
                    
                    multi.Abort();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show("数据量太大，请减少时间间隔重新同步数据！");
            }
        }
        string TarStr1 = "yyyy-MM-dd HH:mm:ss";
        string TarStr = "yyMMddHHmmss";
        int intervalNum1 = 0;
        int intervalNum2 = 0;
        int intervalNum3 = 0;
        IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
        DateTime MyDate;
        DateTime MyDate1;
        double tt, t1, t2, hh, h1, h2;
        private void insertData(List<bean.dataSerialization> list)
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
                    //datas.devicedate = GetTime(datas.devicedate);
                    MyDate = DateTime.ParseExact(datas.devicedate, TarStr, format);
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
                }
                if (datas.sysdate != null && datas.sysdate.Length > 0)
                {
                    MyDate1 = DateTime.ParseExact(datas.sysdate, TarStr, format);
                    datas.sysdate = MyDate1.ToString(TarStr1);
                }
                datas.measureMeterCode = datas.managerID + "_" + datas.deviceNum;

                DataRow[] drs = dtcdinfo.Select("measureCode='" + datas.managerID + "' and meterNo='" + datas.deviceNum + "'"); ;
                tt = Double.Parse(datas.temperature);
                t1 = Double.Parse(drs[0]["t_high"].ToString());
                t2 = Double.Parse(drs[0]["t_low"].ToString());
                hh = Double.Parse(datas.humidity);
                h1 = Double.Parse(drs[0]["h_high"].ToString());
                h2 = Double.Parse(drs[0]["h_low"].ToString());
                string CommunicationType = drs[0]["CommunicationType"].ToString();
                if (CommunicationType == "LBCC-16" || CommunicationType == "[管理主机]LB863RSB_N1(LBGZ-02)" || CommunicationType == "LBGZ-04" || CommunicationType == "RC-8/-10")
                {
                    //if (hh != 0)
                    //{

                    if (CommunicationType == "LBGZ-04" && datas.charge == "0")
                    {
                        datas.sign = "1";
                        datas.warnState = "1";
                        Whistory = 1;
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
                        Whistory = 1;
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
                        history = 2;
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
                            history = 2;
                        }
                        if (tt < t1 && tt > t2 && hh < h1 && hh > h2 && history == 2)
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
            if (listed.Count > 0 && dt.Rows.Count > 0)
            {
                for (int i = 0; i < listed.Count; i++)
                {
                    DataRow[] drArr = dt.Select("measureCode='" + listed[i].managerID + "' and meterNo='" + listed[i].deviceNum + "' and devtime='" + listed[i].devicedate + "'");
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
            adddatas.dataSynchronous(listed);
            Thread.Sleep(500);
        }


        private void wtAddData(object data)
        {
            List<bean.dataSerialization> datastr = data as List<bean.dataSerialization>;
            adddatas.dataSynchronous(datastr);

        }
        //设置进度条的Value  
        private void SetPos(int ipos, string con)
        {
            if (this.progressBar1.InvokeRequired)
            {
                DelegateFunction df = new DelegateFunction(SetPos);
                Thread.Sleep(500);
                this.BeginInvoke(df, new object[] { ipos, con });
            }
            else
            {
                this.label5.Text = "已完成" + ipos.ToString() + "%";
                this.progressBar1.Value = Convert.ToInt32(ipos);
                this.textBox1.AppendText(con + "     \n");

                if (ipos == 100)
                {
                    this.label2.Text = "仪表数据已同步完成！";
                    this.textBox1.AppendText("已完成所有仪表数据的同步！");
                }
            }


        }
        private void StartMultiWork(int ipos, string con)
        {
            this.progressBar1.Value = ipos;
            this.textBox1.AppendText(con + "     " + "\n");

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
            return f.ToString();
        }
        private string getFromXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/historyDataIport");
            return node.InnerText;
        }
    }
}
