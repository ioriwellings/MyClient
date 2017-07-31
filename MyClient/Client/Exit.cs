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
    public partial class Exit : Form
    {
        public Exit()
        {
            InitializeComponent();
        }

        private void Exit_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/assign.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/cancel.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                string password=this.textBox1.Text;
                if (password != null && !"".Equals(password))
                {
                    //用户登录 获取用户的账号和密码并判断          
                    DataTable ret = service.UserService.UserExists(frmLogin.name, password);
                    if (ret.Rows.Count==1)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else {
                        MessageBox.Show("密码错误,请重新输入！");
                    }
                }
                else {
                    MessageBox.Show("密码框不能为空！");
                }
                
            }
            else if (this.radioButton2.Checked)
            {
                this.DialogResult = DialogResult.OK;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.ReadOnly = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = ""; 
            this.textBox1.ReadOnly = true;
        }
    }
}
