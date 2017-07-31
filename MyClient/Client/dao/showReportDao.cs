using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LBKJClient.dao
{
    class showReportDao
    {
        private static readonly object obj = new object();
        private string dbPath = "Data Source =new.baw";
        public DataTable queryReport()
        {
            DataSet ds = new DataSet();
            string sql = "select * from lb_show_report";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            SQLiteDataAdapter comm = null;
            conn.SetPassword(frmLogin.dataPassw);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                    comm = new SQLiteDataAdapter(sql, conn);
                    comm.Fill(ds);              
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                comm.Dispose();//释放命令执行器
                conn.Close();//断开数据库连接
                conn.Dispose();//释放连接器
            }
            return ds.Tables[0];
        }

        public bool addReport(bean.showReportBean rb)
        {
            int rt = 0;
            int ishave = 0;
            String sql0 = "select * from lb_show_report where type = '" + rb.type + "';";
            String sql1 = "insert into lb_show_report (createTime,eventInfo,type) values ('" + rb.createTime + "', '" + rb.eventInfo + "', '" + rb.type + "');";
            String sql2 = "delete from lb_show_report where type = '" + rb.type + "';";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            SQLiteCommand cmdSearch = null;
            conn.SetPassword(frmLogin.dataPassw);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmdSearch = new SQLiteCommand(conn);
                cmdSearch.CommandText = sql0;
                ishave = cmdSearch.ExecuteNonQuery();
                Monitor.Enter(obj);
                if (ishave == 0) {
                    cmdSearch.CommandText = sql1;
                    rt = cmdSearch.ExecuteNonQuery();
                } else {
                    cmdSearch.CommandText =sql2;
                    cmdSearch.ExecuteNonQuery();                    
                    cmdSearch.CommandText = sql1;                  
                    rt = cmdSearch.ExecuteNonQuery();
                };
                Monitor.Exit(obj);
            }
            catch (SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmdSearch.Dispose();//释放命令执行器
                conn.Close();//断开数据库连接
                conn.Dispose();//释放连接器
            }
            return rt == 0 ? false : true;
        }
    }
}
