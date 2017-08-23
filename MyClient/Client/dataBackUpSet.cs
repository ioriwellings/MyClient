using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class dataBackUpSet : Form
    {
        public string timetype = null;
        public bool istrue = false;
        private static string str = Application.StartupPath;//项目路径
        private string zdbacktimes = "/automateBackupTimes.txt";
        private string sdbacktimes = "/manualBackupTimes.txt";
        public dataBackUpSet()
        {
            InitializeComponent();
        }
        private void dataBackUpSet_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/save.png");
            this.button3.BackgroundImage = Image.FromFile(@str + "/images/cancel.png");
            this.label6.Text= textFileUpdate(@str + sdbacktimes);
           this.label8.Text = textFileUpdate(@str + zdbacktimes);
            
            if (this.radioButton1.Checked) {
    
                this.label10.Text = Convert.ToDateTime(this.label8.Text).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
            } else if (this.radioButton2.Checked) {

                this.label10.Text = Convert.ToDateTime(this.label8.Text).AddDays(7).ToString("yyyy-MM-dd HH:mm:ss");
            }
           
        }
     
        private String saveFile() {
            
            string localFilePath = String.Empty;
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.InitialDirectory = "C://";

            fileDialog.Filter = "txt files (*.db)|*.db|All files (*.*)|*.*";

            //设置文件名称：
            fileDialog.FileName = "温湿度自动监测系统";

            fileDialog.FilterIndex = 2;

            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {   //获得文件路径
                localFilePath = fileDialog.FileName.ToString();
            }
            return localFilePath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string num = null;

            if (this.radioButton1.Checked)
            {
                num = this.radioButton1.Tag.ToString();
            }
            else if (this.radioButton2.Checked)
            {
                num = this.radioButton2.Tag.ToString();
            }
            if (num!=null&&!"".Equals(num)) {
                if (this.checkBox2.Checked) {
                    istrue = true;
                }
                timetype = num;
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
            
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void textFile(string filepath, string name)
        {
            FileStream myFs = myFs = new FileStream(@filepath, FileMode.Create);
            StreamWriter mySw = new StreamWriter(myFs);
            mySw.Write(name);
            mySw.Close();
            myFs.Close();
        }
        private string textFileUpdate(string filepath)
        {
            FileStream myFs = myFs = new FileStream(@filepath, FileMode.Open);
            StreamReader sd = new StreamReader(myFs);
            string name = sd.ReadLine();
            sd.Close();
            sd.Close();
            return name;
        }
    }
}
