using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class frmTest : Form
    {
        public delegate void invokeDisplay(string str);

        public frmTest()
        {
            InitializeComponent();
        }

        private void DoWork()

        {

            invokeDisplay ivk = new invokeDisplay(Updata);

            SQLiteConnection conn = null;
            //string dbPath = "Data Source =" + Environment.CurrentDirectory + "/test.db";
            string dbPath = "Data Source =lbkj.db";

            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            conn.SetPassword("lbkj!@3");
            conn.Open();//打开数据库，若文件不存在会自动创建  
            string sql = "CREATE TABLE IF NOT EXISTS student(id integer, name varchar(20), sex varchar(2));";//建表语句  
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn);
            cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建数据表  
            SQLiteCommand cmdInsert = new SQLiteCommand(conn);

            for (int i = 0; i < 1000; i++)
            {
                cmdInsert.CommandText = "INSERT INTO student VALUES(" + i + ", '小红', '男');";//插入几条数据  
                cmdInsert.ExecuteNonQuery();
                this.BeginInvoke(ivk, new object[] { i.ToString() });
                Thread.Sleep(1);
            }

            conn.Close();
        }

        private void Updata(string str)

        {

            label2.Text = str;

        }

        void threadMethod()
        {
            Action<String> AsyncUIDelegate = delegate (string n) { label1.Text = n; };

            SQLiteConnection conn = null;
            //string dbPath = "Data Source =" + Environment.CurrentDirectory + "/test.db";
            string dbPath = "Data Source =lbkj.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            conn.SetPassword("lbkj!@3");
            conn.Open();//打开数据库，若文件不存在会自动创建  
            string sql = "CREATE TABLE IF NOT EXISTS student(id integer, name varchar(20), sex varchar(2));";//建表语句  
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn);
            cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建数据表  
            SQLiteCommand cmdInsert = new SQLiteCommand(conn);

            for (int i = 0; i < 10000000; i++)
            {
                cmdInsert.CommandText = "INSERT INTO student VALUES(" + i + ", '小红', '男');";//插入几条数据  
                cmdInsert.ExecuteNonQuery();
                label1.Invoke(AsyncUIDelegate, new object[] { i.ToString() });
            }

            conn.Close();

        }


        private void button2_Click(object sender, EventArgs e)
        {
            Thread demoThread = new Thread(new ThreadStart(threadMethod));
            demoThread.IsBackground = true;
            demoThread.Start();//启动线程 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =new.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            //conn.SetPassword("lbkj!@3");
            conn.Open();
            conn.ChangePassword("123");
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(DoWork));
            thread.Start();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =new.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            conn.SetPassword("123");
            //conn.SetPassword("lbkj!@3");
            conn.Open();
            conn.ChangePassword("");
            conn.Close();
            MessageBox.Show("OK");
        }
    }
}
