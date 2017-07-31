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
    public partial class passWordUpdate : Form
    {
        public passWordUpdate()
        {
            InitializeComponent();
        }

        private void passWordUpdate_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.pictureBox1.Image = Image.FromFile(@str + "/images/password.png");
            this.button1.BackgroundImage= Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage= Image.FromFile(@str + "/images/cancel.png");
            this.Text += "  " + frmLogin.name;
            //this.txtName.Text= frmLogin.passw;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string h1 = this.txtName.Text;
            string p1=this.txtPassword.Text;
            string p2=this.textBox1.Text;
            if (h1 != null && !"".Equals(h1)&&p1 !=null&&!"".Equals(p1)&&p2!=null&&!"".Equals(p2)) {
                if (frmLogin.passw.Equals(h1)&& p1.Equals(p2))
                {   
                    bool ret = service.UserService.updatePassWord(frmLogin.name, MemoryPassword.MyEncrypt.EncryptDES(p1));
                    if (ret) {
                        MessageBox.Show("密码修改成功，下次登录需要使用新密码登录！");
                        this.Hide();
                    }
                }
                else {
                    MessageBox.Show("原密码输入不正确或新密码两次输入密码不一致！");
                }
            }
            else
            {
                MessageBox.Show("密码框不能为空！");
            }
        }
    }
}
