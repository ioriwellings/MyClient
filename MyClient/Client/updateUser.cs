using System;
using System.Drawing;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class updateUser : Form
    {
        public updateUser()
        {
            InitializeComponent();
        }

        private void updateUser_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/update.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/cancel.png");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = this.textBox2.Text;
            string name = this.textBox1.Text;
            string sy = this.comboBox1.Text;
            string power = "";
            if (name != null && !"".Equals(name))
            {
                if (name == "admin") { MessageBox.Show("管理员账号不能被修改！"); }
                else
                {
                    foreach (Control ctr in this.groupBox2.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (ck.Checked)
                            {
                                power += "," + ck.Text;
                            }
                        }
                    }
                    if (power != null && !"".Equals(power))
                    {
                        power = power.Substring(1);
                        bean.UserInfo ui = new bean.UserInfo();
                        ui.Id = id;
                        ui.UserName = name;
                        ui.Enable = Int32.Parse(sy);
                        ui.Power = power;
                        service.UserService us = new service.UserService();
                        bool istrue = us.updateUser(ui);
                        if (istrue)
                        {
                            this.Hide();
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("用户名或密码为空，请重新输入！");
                        }
                    }
                }
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
