using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace LBKJClient.dao
{
    class rtmonitoringDao
    {
        DataSet ds;
        private string dbPath = "Data Source =new.baw";
        public DataSet monitoring(string time)
        {
            ds = new DataSet();
            ds.Clear();
            //String sql = "select b.measureCode,b.meterNo,a.temperature,a.humidity,max(datetime(a.devtime)) devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.warnState,a.sign,a.measureMeterCode,b.housetype,b.powerflag from lb_base_data_home a join lb_device_information b on a.measureCode = b.measureCode and a.meterNo = b.meterNo join lb_managehost_info m on b.measureCode=m.measureCode group by b.measureCode,b.meterNo order by m.id";
            String sql = "select b.measureCode,b.meterNo,a.temperature,a.humidity,max(datetime(a.devtime)) devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.warnState,a.sign,a.measureMeterCode,b.housetype,b.powerflag from lb_device_information b left join lb_base_data_home a on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime>='" + time + "' join lb_managehost_info m on b.measureCode=m.measureCode group by b.measureCode,b.meterNo order by m.id";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            SQLiteDataAdapter comm=null;
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
            catch (SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
            finally {
                comm.Dispose();//释放命令执行器
                conn.Close();//断开数据库连接
                conn.Dispose();//释放连接器
            }
            return ds;
        }

        public DataTable queryMonitoringByhousecode(string code)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH") + ":00:00";
            ds = new DataSet();
            ds.Clear();
            String sql = "select b.measureCode,b.meterNo,a.temperature,a.humidity,max(datetime(a.devtime)) devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.measureMeterCode,b.pointX,b.pointY,b.housetype from lb_device_information b left join lb_base_data_home a on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime>='" + time + "' join lb_managehost_info m on b.measureCode = m.measureCode where b.house_code='" + code+ "' and b.powerflag < 1 group by b.measureCode,b.meterNo order by m.id,a.devtime";
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
            catch (SQLiteException ex)
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
