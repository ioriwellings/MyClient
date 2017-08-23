using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace LBKJClient
{
    public partial class basicSetup : Form
    {
        private string path = @"config.xml";
        private XmlDocument xmlDoc;
        private string getresults="";
        private string datasxtime = "";
        private string datahousesavetime = "";
        public basicSetup()
        {
            InitializeComponent();
        }

        private void basicSetup_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            getFromXml();
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
            this.comboBox6.SelectedIndex = 0;
            this.numericUpDown1.Value = Convert.ToInt32(datasxtime);
            this.comboBox4.SelectedItem = datahousesavetime;
            //this.comboBox4.SelectedItem = (Int32.Parse(datasavetime) / 60).ToString();
            //this.comboBox1.SelectedItem = result[1];
            //this.comboBox4.SelectedItem = result[1];

            string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
            this.comboBox5.Items.Clear();
            if (ArryPort.Length > 0)
            {
                for (int i = 0; i < ArryPort.Length; i++)
                {
                    this.comboBox5.Items.Add(ArryPort[i]);
                }
            }
            if (getresults != "" && !"".Equals(getresults))
            {
                string[] result = getresults.Split('-');
                if (result[0] == "1")
                {
                    this.comboBox5.Items.Add(result[1]);
                    this.radioButton1.Checked = true;
                    this.comboBox5.SelectedItem = result[1];
                    //this.comboBox5.SelectedIndex =Int32.Parse(result[1].Substring(3))-1;
                    this.textBox1.Enabled = false;
                    this.textBox2.Enabled = false;
                    this.numericUpDown3.Enabled = false;
                    this.numericUpDown4.Enabled = false;
                }
                if (result[0] == "2")
                {
                    this.radioButton2.Checked = true;
                    this.textBox1.Text = result[1];
                    this.numericUpDown3.Value = Convert.ToInt32(result[2]);
                    this.comboBox5.Enabled = false;
                    this.textBox2.Enabled = false;
                    this.numericUpDown4.Enabled = false;
                }
                if (result[0] == "3")
                {
                    this.radioButton3.Checked = true;
                    this.textBox2.Text = result[1];
                    this.numericUpDown4.Value = Convert.ToInt32(result[2]);
                    this.comboBox5.Enabled = false;
                    this.textBox1.Enabled = false;
                    this.numericUpDown3.Enabled = false;

                }
            }
            else
            {
                this.comboBox5.Enabled = false;
                this.textBox1.Enabled = false;
                this.textBox2.Enabled = false;
                this.numericUpDown3.Enabled = false;
                this.numericUpDown4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                string aa = this.radioButton1.Tag.ToString();
                string txxy = this.comboBox5.SelectedItem.ToString();
                saveToXmlsStoptime(aa + "-" + txxy);

            }
            else if (this.radioButton2.Checked)
            {
                string ipnum = this.textBox1.Text;
                string aa = this.radioButton2.Tag.ToString();
                string port = this.numericUpDown3.Value.ToString();
                saveToXmlsStoptime(aa + "-" + ipnum + "-" + port);

            }
            else if (this.radioButton3.Checked)
            {
                string aa = this.radioButton3.Tag.ToString();
                string ipnum = this.textBox2.Text;
                string port = this.numericUpDown4.Value.ToString();
                saveToXmlsStoptime(aa + "-" + ipnum + "-" + port);

            }

            string datasxtime=this.numericUpDown1.Value.ToString();
            string timetype=this.comboBox1.Text;
            if ("秒".Equals(timetype))
            {
                saveToXmldatarefreshtime(datasxtime);
            }
            string carsxtime = this.numericUpDown5.Value.ToString();
            if (carsxtime != null && !"".Equals(carsxtime))
            {
                saveToXmlcartime(carsxtime);
            }
            else {
                int nom = 5;
                saveToXmlcartime(nom.ToString());
            }
           
            //else if("分".Equals(timetype)){

            //    saveToXmldatarefreshtime((Int32.Parse(datasxtime)*60).ToString());
            //}
            //else if ("时".Equals(timetype))
            //{
            //    saveToXmldatarefreshtime((Int32.Parse(datasxtime) * 60 * 60).ToString());
            //}
            string savetime = this.comboBox4.Text;
            saveToXmldatasavetime(savetime);
            //saveToXmldatasavetime((Int32.Parse(savetime) * 60).ToString());

            this.DialogResult = DialogResult.OK;
            MessageBox.Show("参数设置成功！");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // 从xml获得数据，并加载
        private void getFromXml()
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/communicationType");
            getresults = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/datarefreshtime");
            datasxtime = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/datahousesavetime");
            datahousesavetime = node.InnerText;

        }
        private void saveToXmlsStoptime(string communication)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/communicationType");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("communicationType");
                n.InnerText = communication;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = communication;
            }
            xmlDoc.Save(path);

        }
        private void saveToXmldatarefreshtime(string time)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/datarefreshtime");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("datarefreshtime");
                n.InnerText = time;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = time;
            }
            xmlDoc.Save(path);

        }
        private void saveToXmldatasavetime(string time)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/datahousesavetime");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("datahousesavetime");
                n.InnerText = time;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = time;
            }
            xmlDoc.Save(path);

        }
        private void saveToXmlcartime(string time)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/carsavetime");
            node.InnerText = time;
            xmlDoc.Save(path);

        }
        private void radioButton1_Click(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked) {
                this.comboBox5.Enabled = true;
                this.textBox1.Enabled = false;
                this.textBox2.Enabled = false;
                this.numericUpDown3.Enabled = false;
                this.numericUpDown4.Enabled = false;
            }
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.textBox1.Enabled = true;
                this.numericUpDown3.Enabled = true;
                this.comboBox5.Enabled = false;
                this.textBox2.Enabled = false;
                this.numericUpDown4.Enabled = false;
            }
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked)
            {
                this.comboBox5.Enabled = false;
                this.textBox1.Enabled = false;
                this.numericUpDown3.Enabled = false;
                this.textBox2.Enabled = true;
                this.numericUpDown4.Enabled = true;
            }
        }
    }
}
