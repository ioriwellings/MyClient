using System;
using System.Drawing;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class insertHouseManage : Form
    {
        public insertHouseManage()
        {
            InitializeComponent();
        }

        private void insertHouseManage_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/insert.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string housename = this.textBox1.Text;
            string t1 = this.numericUpDown1.Value.ToString();
            string t2 = this.numericUpDown2.Value.ToString();
            string h1 = this.numericUpDown3.Value.ToString();
            string h2 = this.numericUpDown4.Value.ToString();

            if (housename != null && !"".Equals(housename))
            {
                bean.houseInfo hi = new bean.houseInfo();
                    hi.name = housename;
                    hi.h_high = float.Parse(h1);
                    hi.h_low = float.Parse(h2);
                    hi.t_high = float.Parse(t1);
                    hi.t_low = float.Parse(t2);
                service.houseTypeService hts = new service.houseTypeService();
                bool bl = hts.addHouseManage(hi);
                    if (bl)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
            }
            else
            {
                MessageBox.Show("库房名称为空，请重新输入！");
            }
        }
    }
}
