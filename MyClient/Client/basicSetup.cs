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
        private string datasxtime = "";
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
            this.numericUpDown1.Value = Convert.ToInt32(datasxtime);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string datasxtime=this.numericUpDown1.Value.ToString();
            string timetype=this.comboBox1.Text;
            if ("秒".Equals(timetype))
            {
                saveToXmldatarefreshtime(datasxtime);
            }
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
            node = xmlDoc.SelectSingleNode("config/datarefreshtime");
            datasxtime = node.InnerText;
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
    }
}
