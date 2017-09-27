using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace LBKJClient
{
    public partial class changGuiCheck : Form
    {
        public DataTable dt=null;
        public string cdlist=null;
        public string measureNolist = null;
        public string time1 = null;
        public string time2 = null;
        public int pageNo = -1;
        public int houseorcartime = 0;
        service.changGuicheckService cgs = new service.changGuicheckService();
        service.manageHostService mhs = new service.manageHostService();
        private string path = @"config.xml";
        private int cartime = 0;
        private int databasetimer = 0;
        public int flag = 0;//判断是否是历史数据功能请求的
        public changGuiCheck()
        {
            InitializeComponent();
        }
        private void changGuiCheck_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/check.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            this.checkBox1.Checked = false;

            this.tabControl1.TabPages[0].Controls.Clear();
            this.tabControl1.TabPages[1].Controls.Clear();
            //让默认的日期时间减一天
            this.dateTimePicker1.Value=this.dateTimePicker2.Value.AddDays(-1);
            //查询所有测点或主机编号信息
            service.changGuicheckService cgs = new service.changGuicheckService();
            DataTable dta = cgs.checkcedianAll(null).Tables[0];
            int t = dta.Rows.Count;
            if (t > 0)
            {
                CheckBox[] checkBox = new CheckBox[t];
                int p = 10;
                for (int i = 0; i < t; i++)
                {
                    checkBox[i] = new CheckBox();
                    checkBox[i].AutoSize = true;
                    checkBox[i].Text = dta.Rows[i]["terminalname"].ToString();
                    checkBox[i].Tag = dta.Rows[i]["measureCode"] + "_" + dta.Rows[i]["meterNo"] + "-" + dta.Rows[i]["measureNo"];
                    checkBox[i].Location = new Point(10, p);
                    this.tabControl1.TabPages[0].Controls.Add(checkBox[i]);
                    p += 20;
                }
                checkBox[0].Checked = true;
            }

            service.manageHostService mhs = new service.manageHostService();
            DataTable dt1 = mhs.queryManageHost();
            int t1 = dt1.Rows.Count;
            if (t1 > 0)
            {
                CheckBox[] checkBox = new CheckBox[t1];
                int p1 = 10;
                for (int i = 0; i < t1; i++)
                {
                    checkBox[i] = new CheckBox();
                    checkBox[i].AutoSize = true;
                    checkBox[i].Text = dt1.Rows[i]["hostName"].ToString();
                    //checkBox[i].Tag = dt1.Rows[i]["measureCode"].ToString() + "_" + dt1.Rows[i]["storeType"];
                    checkBox[i].Tag = dt1.Rows[i]["measureCode"].ToString() + "_" + dt1.Rows[i]["measureNo"];
                    checkBox[i].Location = new Point(10, p1);
                    checkBox[i].Click += new EventHandler(Clickchecked);
                    this.tabControl1.TabPages[1].Controls.Add(checkBox[i]);
                    p1 += 20;
                }
                checkBox[0].Checked = true;
            }

            getFromXml();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dts = null;
            //DataTable dtss = null;
            //DataTable dtss1 = null;
            time1 = this.dateTimePicker1.Text.ToString();
            time2 = this.dateTimePicker2.Text.ToString();
            int page = this.tabControl1.SelectedIndex;
            
            String cd = null;
            String cd1 = null;
            String measureNo = null;
            if (page == 0) {
                foreach (Control ctr in this.tabControl1.TabPages[0].Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (ck.Checked)
                        {
                            //measureCode_meterNo主机号加测点号测点measuremetercode的集合
                            cd += "," + ck.Tag.ToString().Split('-')[0];
                            // measureNo分区号的集合
                            measureNo += "," + ck.Tag.ToString().Split('-')[1];
                            //string measureCode = "";
                            //measureCode += "_" + ck.Tag.ToString().Split('_')[0];                          
                            //measureCode = measureCode.Substring(1);
                            //DataTable dd = mhs.queryManageHoststoreType(measureCode);

                            //if (!"车载".Equals(dd.Rows[0]["storeType"].ToString()))
                            //{
                            //    cartime = 30;
                            //}
                        }
                    }
                }
                if (cd != null)
                {
                    cd = cd.Substring(1);
                    measureNo = measureNo.Substring(1);
                    //历史数据查询分页准备参数
                    if (flag==1) {
                        cdlist = cd;
                        measureNolist = measureNo;
                        pageNo = 0;
                        //houseorcartime = cartime;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    dts = cgs.changguicheck(time1, time2, cd, measureNo).Tables[0];                  
                    if (dts.Rows.Count > 1000)
                    {
                        MessageBox.Show("查询出的数据不可超过1000条，重新选择查询条件！");
                        return;
                    }
                    if (dts.Rows.Count > 0) {
                        //DataRow[] dr1 = dts.Select("warningistrue='2' or warningistrue = '1' or warningistrue='3' or warnState = '3' or warnState = '1'");
                        //DataRow[] dr2 = null;
                        //if (cartime == 30)
                        //{
                        //    dr2 = dts.Select("houseinterval='30'");
                        //}
                        //else if (cartime == 5)
                        //{
                        //    dr2 = dts.Select("carinterval='5'");
                        //}
                        //if (dr1.Count() > 0)
                        //{
                        //    dtss = dr1[0].Table.Clone();
                        //    foreach (DataRow row in dr1)
                        //    {

                        //        dtss.ImportRow(row); // 将DataRow添加到DataTable中
                        //    }
                        //}
                        //if (dr2 != null && dr2.Count() > 0)
                        //{
                        //    if (dr1.Count() < 1)
                        //    {
                        //        dtss = dr2[0].Table.Clone();
                        //    }
                        //    foreach (DataRow row in dr2)
                        //    {
                        //        dtss.ImportRow(row); // 将DataRow添加到DataTable中
                        //    }
                        //}
                        //if (dtss!=null&&dtss.Rows.Count > 0)
                        //{
                        //    DataView dv = dtss.DefaultView;//虚拟视图
                        //    dv.Sort = "devtime asc";
                        //    dt = dv.ToTable(true);
                        //    dt.Columns.Remove("carinterval");
                        //    dt.Columns.Remove("houseinterval");
                        //    dt.Columns.Remove("measureNo");
                        //    cdlist = cd;
                        //}
                        DataView dv = dts.DefaultView;//虚拟视图
                        dv.Sort = "devtime asc";
                        dt = dv.ToTable(true);
                        dt.Columns.Remove("carinterval");
                        dt.Columns.Remove("houseinterval");
                        dt.Columns.Remove("measureNo");
                        cdlist = cd;
                    }
                    else
                    {
                        MessageBox.Show("当前时段未查询出数据！");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未选择要查询的数据，请重新查询！");
                    return;
                }
            } else if (page==1) {
                foreach (Control ctr1 in this.tabControl1.TabPages[1].Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr1 is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck1 = ctr1 as CheckBox;
                        if (ck1.Checked)
                        {
                            string[] ck2= ck1.Tag.ToString().Split('_');
                            // measureCode主机号
                            string measureCode = ck2[0];
                            cd = measureCode;
                            // measureNo分区号
                            measureNo = ck2[1];                          
                            //if (!"车载".Equals(ck2[1].ToString()))
                            //{
                            //    cartime = 30;
                            //}
                        }
                    }
                }
                if (cd != null)
                {
                    if (flag == 1)
                    {
                        cdlist = cd;
                        pageNo = 1;
                        //houseorcartime = cartime;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    DataTable dts1 = cgs.changguicheckGlzj(time1, time2, cd).Tables[0];
                    if (dts1.Rows.Count > 1000)
                    {
                        MessageBox.Show("查询出的数据不可超过1000条，重新选择查询条件！");
                        return;
                    }
                    if (dts1.Rows.Count > 0)
                    {
                        DataView dv = dts1.DefaultView;//虚拟视图
                        dv.Sort = "devtime asc";
                        dt = dv.ToTable(true);
                        dt.Columns.Remove("carinterval");
                        dt.Columns.Remove("houseinterval");
                        //dt.Columns.Remove("measureNo");
                        DataTable dt1 = dv.ToTable(true, "measureMeterCode");
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            cd1 += "," + dt1.Rows[i]["measureMeterCode"];
                        }
                        cd1 = cd1.Substring(1);
                        cdlist = cd1;
                        //DataRow[] dr1 = dts1.Select("warningistrue='2' or warningistrue = '1' or warningistrue='3' or warnState = '3' or warnState = '1'");
                        //DataRow[] dr2 = null;
                        //if (cartime == 30)
                        //{
                        //    dr2 = dts1.Select("houseinterval='30'");
                        //}
                        //else if (cartime == 5)
                        //{
                        //    dr2 = dts1.Select("carinterval='5'");
                        //}
                        //if (dr1.Count() > 0)
                        //{
                        //    dtss1 = dr1[0].Table.Clone();
                        //    foreach (DataRow row in dr1)
                        //    {

                        //        dtss1.ImportRow(row); // 将DataRow添加到DataTable中
                        //    }
                        //}
                        //if (dr2 != null && dr2.Count() > 0)
                        //{
                        //    if (dr1.Count() < 1)
                        //    {
                        //        dtss1 = dr2[0].Table.Clone();
                        //    }
                        //    foreach (DataRow row in dr2)
                        //    {

                        //        dtss1.ImportRow(row); // 将DataRow添加到DataTable中
                        //    }
                        //}
                    }
                    else
                    {
                        MessageBox.Show("当前时段未查询出数据！");
                        return;
                    }
                    //if (dtss1 != null)
                    //{
                    //    if (dtss1.Rows.Count > 0)
                    //    {
                    //        DataView dv = dtss1.DefaultView;//虚拟视图
                    //        dv.Sort = "devtime asc";
                    //        dt = dv.ToTable(true);
                    //        dt.Columns.Remove("carinterval");
                    //        dt.Columns.Remove("houseinterval");
                    //        //dt.Columns.Remove("measureNo");
                    //        DataTable dt1 = dv.ToTable(true, "measureMeterCode");
                    //        for (int i = 0; i < dt1.Rows.Count; i++)
                    //        {
                    //            cd1 += "," + dt1.Rows[i]["measureMeterCode"];
                    //        }
                    //        cd1 = cd1.Substring(1);
                    //        cdlist = cd1;
                    //    }
                    //}
                }
                else
                {
                    MessageBox.Show("未选择要查询的数据，请重新查询！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("无查询项，请重新查询！");
            }
            if (dt!=null&&dt.Rows.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
            else {
                MessageBox.Show("没有此时间段的数据，请重新查询！");
            }
       }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            
        }
        private void getFromXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/carsavetime");
            cartime =Int32.Parse( node.InnerText);
            node = xmlDoc.SelectSingleNode("config/databasetimer");
            databasetimer = Int32.Parse(node.InnerText);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int page = this.tabControl1.SelectedIndex;
            if (page == 0)
            {
                    //判断该控件是不是CheckBox
                  if (this.checkBox1.Checked)
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
                else {
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
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            int page = this.tabControl1.SelectedIndex;
            if (page == 1)
            {
                this.checkBox1.Visible = false;
                this.label4.Visible = false;
                this.textBox1.Visible = false;
                this.button3.Visible = false;
            }
            else {
                this.checkBox1.Visible = true;
                this.label4.Visible = true;
                this.textBox1.Visible = true;
                this.button3.Visible = true;
            }
        }
        private void Clickchecked(object sender, EventArgs e)
        {
            foreach (Control ctr1 in this.tabControl1.TabPages[1].Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr1 is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    (ctr1 as CheckBox).Checked= ctr1 == sender ? true : false;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string terminalname = this.textBox1.Text;
            if (terminalname != "")
            {
                this.tabControl1.TabPages[0].Controls.Clear();
                DataTable dta = cgs.checkcedianAll0(terminalname).Tables[0];
                int t = dta.Rows.Count;
                if (t > 0)
                {
                    CheckBox[] checkBox = new CheckBox[t];
                    int p = 10;
                    for (int i = 0; i < t; i++)
                    {
                        checkBox[i] = new CheckBox();
                        checkBox[i].AutoSize = true;
                        checkBox[i].Text = dta.Rows[i]["terminalname"].ToString();
                        checkBox[i].Tag = dta.Rows[i]["measureCode"] + "_" + dta.Rows[i]["meterNo"] + "-" + dta.Rows[i]["measureNo"];
                        checkBox[i].Location = new Point(10, p);
                        this.tabControl1.TabPages[0].Controls.Add(checkBox[i]);
                        p += 20;
                    }
                    checkBox[0].Checked = true;
                }
            };
        }
    }
}
