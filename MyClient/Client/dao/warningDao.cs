using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace LBKJClient.dao
{
    class warningDao
    {
        private static readonly object obj = new object();
        private string dbPath = "Data Source =new.baw";
        public DataTable warningheck(String time1, String time2, String cd)
        {
            //String sql = "select distinct aa.* from (select a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,h.warningTime from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' and warningistrue='2' and b.housetype <> 1 left join lb_warning_handle h on h.measureMeterCode=a.measureMeterCode and h.warningTime=a.devtime ) aa";
            //if (cd != null && cd != "0")
            //{
            //    string[] cd1 = cd.Split('_');
            //    sql += " where aa.measureCode='" + cd1[0] + "' and aa.meterNo='" + cd1[1] + "'";

            //}
            //sql += " order by aa.measureCode,aa.terminalname";
            String sql = "";
            if (cd != null && cd != "0")
            {
                sql = "select distinct aa.* from (select a.devtime,b.terminalname,a.measureCode,a.meterNo,a.temperature,a.humidity,a.warnState,b.t_high,b.t_low,b.h_high,b.h_low,h.warningTime,'0.0' wdcz,'0.0'sdcz from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' and a.measureMeterCode='" + cd + "' and b.housetype <> 1 left join lb_warning_handle h on h.measureMeterCode=a.measureMeterCode and h.warningTime=a.devtime  where  a.warnState = '1' or a.warnState = '3' or a.warningistrue = '2' or a.warningistrue = '3') aa";
            }else {
                sql = "select distinct aa.* from (select a.devtime,b.terminalname,a.measureCode,a.meterNo,a.temperature,a.humidity,a.warnState,b.t_high,b.t_low,b.h_high,b.h_low,h.warningTime,'0.0' wdcz,'0.0'sdcz from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' and b.housetype <> 1 left join lb_warning_handle h on h.measureMeterCode=a.measureMeterCode and h.warningTime=a.devtime  where  a.warnState = '1' or a.warnState = '3' or a.warningistrue = '2' or a.warningistrue = '3') aa";
            }
            //sql += " where  a.warnState = '1' or a.warnState = '3' or a.warningistrue = '2' or a.warningistrue = '3' order by a.measureCode,aa.terminalname";
           
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

        public bool addWarningHandleInfo(bean.warningHandleBean whb)
        {
            int rt = 0;
            String sql = "insert into lb_warning_handle (handleUser,warningTime,handleTime,handleType,handleResult,handleTetails,createTime,measureMeterCode) values ('" + whb.handleUser + "', '" + whb.warningTime + "', '" + whb.handleTime + "', '" + whb.handleType + "', '" + whb.handleResult + "', '" + whb.handleTetails + "', '" + whb.createTime + "', '" + whb.measureMeterCode + "')";
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
                cmdSearch.CommandText = sql;
                Monitor.Enter(obj);
                rt = cmdSearch.ExecuteNonQuery();
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
        public DataTable warningHandlecheck(String time1, String time2)
        {
            String sql = "select * from lb_warning_handle a where a.handleTime > '" + time1 + "' and  a.handleTime <  '" + time2 + "'";
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
        public DataTable warningCheck(String measureMeterCode, String time)
        {
            string [] mm= measureMeterCode.Split('_');
            string sql = "select a.* from lb_warning_handle a join lb_device_information b on b.measureCode='" + mm[0] + "' and b.meterNo='" + mm[1] + "' and  a.warningTime = '" + time + "' and b.housetype <> 1";
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
        public DataTable checkData(String timew, String code, String meter)
        {
            string sql = "select h.measureCode,h.meterNo,h.temperature,h.humidity,h.devtime,d.t_high,d.t_low,d.h_high,d.h_low from lb_base_data_home h join lb_device_information d on d.measureCode=h.measureCode and d.meterNo=h.meterNo where h.devtime > '"+timew+"' and h.measureCode='"+code+"' and h.meterNo='"+meter+"' and h.warningistrue <> 2 and h.warnState <> 1";
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
