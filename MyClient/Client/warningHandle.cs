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
    public partial class warningHandle : Form
    {
        public warningHandle()
        {
            InitializeComponent();
        }

        private void warningHandle_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                bean.warningHandleBean whb = new bean.warningHandleBean();
                whb.handleUser = this.label2.Text;
                whb.warningTime = this.textBox1.Text;
                //whb.handleTime = this.textBox2.Text;
                whb.handleTime = this.dateTimePicker1.Text.ToString();
                whb.handleType = this.textBox3.Text;
                whb.handleResult = this.textBox4.Text;
                whb.measureMeterCode = this.textBox5.Text;
                whb.handleTetails = this.richTextBox1.Text;
                whb.createTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (whb.handleType != null && !"".Equals(whb.handleType) && whb.handleResult != null && !"".Equals(whb.handleResult))
            {
                service.warningCheckService wc = new service.warningCheckService();
                bool bl = wc.addWarningHandleInfo(whb);
                if (bl)
                {
                    this.Close();
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("报警处理信息保存成功！");
                }
            }
            else {
                MessageBox.Show("处理方式和处理结果都不能为空，请重新填写！");
            }
        }
    }
}
