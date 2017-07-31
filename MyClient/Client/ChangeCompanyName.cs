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
    public partial class ChangeCompanyName : Form
    {
        public string compName = "";
        public ChangeCompanyName()
        {
            InitializeComponent();
        }

        private void ChangeCompanyName_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.pictureBox1.Image = Image.FromFile(@str + "/images/inputcontent.png");
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/assign.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            compName = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
