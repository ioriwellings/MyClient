using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class addUser : Form
    {
        public addUser()
        {
            InitializeComponent();
        }

        private void addUser_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/insert.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            ///新增测点权限分配
            service.changGuicheckService cgs = new service.changGuicheckService();
            DataTable dta = cgs.checkcedianAll(null).Tables[0];

            int t = dta.Rows.Count;
            if (t > 0)
            {
                for (int i = 0; i < t; i++)
                {
                    string tag = dta.Rows[i]["terminalname"].ToString();
                    this.checkedListBox1.Items.Add(tag);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name=this.textBox1.Text;
            string pwd = this.textBox2.Text;
            string pwd1 = this.textBox3.Text;
            string power = "";
            String cd = null;

            if (name!=null&&!"".Equals(name)&& pwd.Equals(pwd1)) {
                foreach (Control ctr in this.groupBox2.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (ck.Checked) {
                            power += "," + ck.Text;
                        }
                    }
                }

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        cd += "," + checkedListBox1.GetItemText(checkedListBox1.Items[i]);
                    }
                }

                if (power != null && !"".Equals(power))
                {
                    power = power.Substring(1);
                    cd = cd.Substring(1);
                    bean.UserInfo ui = new bean.UserInfo();
                    ui.UserName = name;
                    ui.Pwd = MemoryPassword.MyEncrypt.EncryptDES(pwd);
                    ui.Power = power;
                    ui.Enable = 1;
                    service.UserService us = new service.UserService();
                    bool istrue = us.addUser(ui, cd);
                    if (istrue)
                    {
                        this.Hide();
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("用户名已重复，请重新输入！");
                    }
                }
                else {
                        MessageBox.Show("未给新用户设置权限！");
                }
            }
            else
            {
                MessageBox.Show("用户名或密码不能为空或两次密码输入不一致！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in this.groupBox2.Controls)
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

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in this.groupBox2.Controls)
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
