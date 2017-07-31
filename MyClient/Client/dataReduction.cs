using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class dataReduction : Form
    {
        public dataReduction()
        {
            InitializeComponent();
        }

        private void dataReduction_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径   
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/assign.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult rr = MessageBox.Show("确定要恢复数据库数据吗？这将会覆盖当前的数据库数据！", "确定要删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {
                string filepath=this.textBox1.Text;

                if (filepath != null && !"".Equals(filepath))
                {   string pathfile = null;
                    string[] sArray = filepath.Split('\\');
                    foreach (string i in sArray)
                    {
                        pathfile += "\\\\" + i;

                    }
                    string pathfiles  =pathfile.Substring(2);
                        try
                    {
                        utils.DataBaseUtil.restoreDatabase(@"new.baw", @pathfiles);
                        Thread.Sleep(200);
                        this.DialogResult = DialogResult.OK;
                    }
                    catch (Exception exc)
                    {
                        //MessageBox.Show(exc.Message);
                        //throw new Exception(exc.Message);
                    }
                }
            }
            }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            string localFilePath = String.Empty;
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = "C://";

            fileDialog.Filter = "txt files (*.baw)|*.baw|All files (*.*)|*.*";

            //设置文件名称：
            fileDialog.FileName = "";

            fileDialog.FilterIndex = 2;

            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {   //获得文件路径
                localFilePath = fileDialog.FileName.ToString();
            }
            this.textBox1.Text = localFilePath;
        }
    }
}
