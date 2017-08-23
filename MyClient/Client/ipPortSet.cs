using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace LBKJClient
{
    public partial class ipPortSet : Form
    {
        private string str = Application.StartupPath;//项目路径
        //变量声明
        private string username;
        private string password;
        private string path = @"config.xml";
        public ipPortSet()
        {
            InitializeComponent();
        }

        private void ipPortSet_Load(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");

            getFromXml();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult rr = MessageBox.Show("确定要修改IP端口吗？", "IP端口设置提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {
                username = textBox1.Text;
                password = textBox2.Text;

                if (username!=null&&!"".Equals(username)&&password.Length == 4)
                {
                    saveToXml(username, password);
                    this.Hide();
                    MessageBox.Show("新的IP端口已保存成功");
                }
                else {
                    MessageBox.Show("端口号输入不正确，请重新输入！");
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //#region 把数据保存至xml文件
        /// <summary>
        /// 保存至xml文件
        /// </summary>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        private void saveToXml(string username, string password)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/username");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("username");
                n.InnerText = username;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = username;
            }
            node = xmlDoc.SelectSingleNode("config/password");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("password");
                n.InnerText = password;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = password;
            }
            xmlDoc.Save(path);

        }


        //#region 从xml获得数据，并加载
        private void getFromXml()
        {
            //获得数据
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/username");
            username = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/password");
            password = node.InnerText;
            //加载数据
            textBox1.Text = username;
            textBox2.Text = password;
        }
    }
}
