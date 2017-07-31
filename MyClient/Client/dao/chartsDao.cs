using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace LBKJClient.dao
{
    class chartsDao
    {
        private string dbPath = "Data Source =new.baw";
        public DataTable chartscheckdao(string time1, string time2, string measure, string meter)
        {
            string sql = "select * from (select a.temperature,a.humidity,a.devtime,a.warningistrue,a.houseinterval,a.carinterval from lb_base_data_home a where 1=1 and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' and  a.measureCode='" + measure + "' and a.meterNo='" + meter + "' order by a.devtime) aa where aa.warningistrue > 0 or aa.houseinterval > 0 or aa.carinterval > 0";
    
            DataSet ds = new DataSet();
            ds.Clear();
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
    }
}
