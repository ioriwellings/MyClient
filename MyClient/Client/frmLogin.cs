using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LBKJClient
{
    public partial class frmLogin : Form
    {
        public static string name = null;
        public static string passw = null;
        public static string dataPassw = "";
        public static List<string> listpower = null;
        private int flag=0;
        //变量声明
        private string username;
        private string password;
        XmlDocument xmlDoc;
        private string path = @"config.xml";
        service.loginLogService lls = new service.loginLogService();
        public frmLogin()
        {
            InitializeComponent();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //用户登录 获取用户的账号和密码并判断          
            DataTable ret = service.UserService.UserExists(txtName.Text, txtPassword.Text);
                if (ret.Rows.Count==1)
                {
                    string power=ret.Rows[0]["power"].ToString();
                    if (power != null&&!"".Equals(power)) {
                        listpower = power.Split(',').ToList();
                    } 
                    name = this.txtName.Text;
                    passw = this.txtPassword.Text;
                    saveToXmlsStarttime(DateTime.Now.ToString("yyMMddHHmmss"));
                    if (this.checkBox1.Checked)
                    {
                        saveToXml(name, MemoryPassword.MyEncrypt.EncryptDES(passw));
                    }
                    else {
                        saveToXml("", "");
                    }
                    if (SplashScreen.Instance != null)
                    {
                        SplashScreen.Instance.BeginInvoke(new MethodInvoker(SplashScreen.Instance.Dispose));
                        SplashScreen.Instance = null;
                    }
                    bean.loginLogBean lb = new bean.loginLogBean();
                    lb.name = name;
                    lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    lb.eventInfo = "登录系统！";
                if(name != "admin" && name != "")
                {
                    lls.addCheckLog(lb);
                }
                   
                    this.Hide();
                    frmMain main = new frmMain();
                    main.hulie = 0;
                    main.Show();
            }
                    else
                    {
                        MessageBox.Show("用户名或密码错误！");
                  } 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        { 

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            //utils.SQLiteHelper.btnEncrypt();
            string str = Application.StartupPath;//项目路径   
            this.pictureBox1.Image = Image.FromFile(@str + "/images/login.png");
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/login1.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/tuichu.png");
            getFromXml();
            //dataPassw = MemoryPassword.MyEncrypt.DecryptDES("uSgoUs6jrMs =");
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
        private void button3_Click(object sender, EventArgs e)
        {
            ipPortSet ips = new ipPortSet();
            ips.ShowDialog();
            
        }
        //#region 把数据保存至xml文件
        /// <summary>
        /// 保存至xml文件
        /// </summary>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        private void saveToXml(string username, string password)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/user");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("user");
                n.InnerText = username;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = username;
            }
            node = xmlDoc.SelectSingleNode("config/passd");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("passd");
                n.InnerText = password;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = password;
            }
            xmlDoc.Save(path);

        }
        private void saveToXmlsStarttime(string starttime)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/starttime");
            if (node == null)
            {
                XmlElement n = xmlDoc.CreateElement("starttime");
                n.InnerText = starttime;
                xmlDoc.SelectSingleNode("config").AppendChild(n);
            }
            else
            {
                node.InnerText = starttime;
            }
            xmlDoc.Save(path);

        }

        //#region 从xml获得数据，并加载
        private void getFromXml()
        {
            //获得数据
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/user");
            username = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/passd");
            password = node.InnerText;
            //加载数据
            if (username!=null&&!"".Equals(username)&&password!=null&&!"".Equals(password)) {
                this.txtPassword.Text = MemoryPassword.MyEncrypt.DecryptDES(password);
                this.checkBox1.Checked = true;
                flag = 1;
            }
        }
        private void AutomaticLogin()
        {
            DataTable ret = service.UserService.UserExists(txtName.Text, txtPassword.Text);

            if (ret.Rows.Count==1)
            {
                string power = ret.Rows[0]["power"].ToString();
                if (power != null && !"".Equals(power))
                {
                    listpower = power.Split(',').ToList();
                }
                name = this.txtName.Text;
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "登录系统！";
                if (name != "admin" && name != "") {
                    lls.addCheckLog(lb);
                }
                    
                SplashScreen.CloseSplashScreen();
                this.Hide();
                frmMain main = new frmMain();
                main.hulie = 0;
                main.Show();
            }   
            else
            {
                MessageBox.Show("用户名或密码错误！");
            }
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            if (flag==1) {
                AutomaticLogin();
            }
        }
        public void cancel_login()
        {
            saveToXml("","");
        }
    }
}
