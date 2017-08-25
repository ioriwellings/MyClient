using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class dataReduction : Form
    {
        string str = Application.StartupPath;//项目路径   
        public dataReduction()
        {
            InitializeComponent();
        }

        private void dataReduction_Load(object sender, EventArgs e)
        {
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
                        //utils.DataBaseUtil.restoreDatabase(@"new.baw", @pathfiles);
                        //Thread.Sleep(200);
                        string[] mysqlinfo = Properties.Settings.Default.mysqlInfo.Split(',');
                        Recover(mysqlinfo[0], mysqlinfo[1], mysqlinfo[2], mysqlinfo[3], mysqlinfo[4], pathfiles);
                    }
                    catch (Exception exc)
                    {
                        
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

        // mysql数据库还原
        public void Recover(string host, string port, string user, string password, string database, string filepath)
        {
            string cmdStr = "mysql -h" + host + " -P" + port + " -u" + user + " -p" + password + " " + database + " < " + filepath;
            try
            {
                string reslut = RunCmd(str + "\\Lib", cmdStr);
                if (reslut.IndexOf("error") == -1 && reslut.IndexOf("命令") == -1)
                {
                    MessageBox.Show("恢复数据库成功>" + database);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(reslut + "恢复数据库失败>" + database);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "恢复数据库失败>" + database);
            }
        }
        public string RunCmd(string workingDirectory, string command)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe"; //确定程序名
            //p.StartInfo.Arguments = "/c " + command; //确定程式命令行
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.UseShellExecute = false; //Shell的使用
            p.StartInfo.RedirectStandardInput = true; //重定向输入
            p.StartInfo.RedirectStandardOutput = true; //重定向输出
            p.StartInfo.RedirectStandardError = true; //重定向输出错误
            p.StartInfo.CreateNoWindow = true; //设置置不显示示窗口
            p.Start();
            p.StandardInput.WriteLine(command); //也可以用这种方式输入入要行的命令 
            p.StandardInput.WriteLine("exit"); //要得加上Exit要不然下一行程式
            //p.WaitForExit();
            //p.Close();
            //return p.StandardOutput.ReadToEnd(); //输出出流取得命令行结果果
            return p.StandardError.ReadToEnd();
        }
    }
}
