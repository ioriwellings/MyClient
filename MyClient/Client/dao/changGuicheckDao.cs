using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace LBKJClient.dao
{
    class changGuicheckDao
    {
        private string dbPath = "Data Source =new.baw";
        public DataSet changguicheck(String time1, String time2, String cd)
        {
            String sql = "select aa.* from (select a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' order by a.devtime) aa";
            string[] c;
            if (cd!=null) {
                sql += "  where  ";
                string[] cds = cd.Split(',');
                //string[] cd1 = cds[0].Split('_');
                //sql += "aa.measureCode='" + cd1[0] + "' and aa.meterNo='" + cd1[1] + "' ";
                sql += "aa.measureMeterCode='" + cds[0] + "' ";
                for (int i = 1; i < cds.Count(); i++)
                {
                    //c = cds[i].Split('_');
                    //sql += "or aa.measureCode='" + c[0] + "' and aa.meterNo='" + c[1] + "' ";
                    sql += " or aa.measureMeterCode='" + cds[i] + "' ";
                }
            }
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
            return ds;
        }
        public DataSet changguicheckGlzj(String time1, String time2, String glzj)
        {
            String sql = "select aa.* from (select a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' order by a.devtime) aa";
            if (glzj != null)
            {
                sql += "  where  ";
                string[] cds = glzj.Split(',');
                sql += "aa.measureCode='" + cds[0] + "' ";
                for (int i = 1; i < cds.Count(); i++)
                {
                    sql += "or aa.measureCode='" + cds[i] + "' ";
                }
            }
           
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
            return ds;
        }
        public DataSet checkcedian(string code)
        {
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = null;
            if (code != null && !"".Equals(code))
            {
                sql = "select a.measureCode,a.meterNo,b.terminalname,a.measureMeterCode from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo where a.measureCode='" + code + "' group by a.measureCode,a.meterNo order by b.id";
            }
            else {
                sql = "select a.measureCode,a.meterNo,b.terminalname,a.measureMeterCode from lb_device_information b left join lb_base_data_home a on a.measureCode=b.measureCode and a.meterNo=b.meterNo group by a.measureCode,a.meterNo,b.terminalname order by b.id";
            }
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
            return ds;
        }
        public DataSet checkcedianAll(string code)
        {
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "";
            if (code != null && !"".Equals(code))
            {
                sql = "select measureCode,meterNo,terminalname,CAST(measureCode AS VARCHAR(50)) || '_' || CAST(meterNo AS VARCHAR(10)) as measureMeterCode from lb_device_information where measureCode='" + code+"'";
            }
            else {
                sql = "select a.measureCode,a.meterNo,a.terminalname,b.storeType from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode";
            }
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
            return ds;
        }
        public DataSet checkCedianCar()//获得车载的所有测点信息
        {
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "select a.measureCode,a.meterNo,a.terminalname,b.storeType from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode and b.hostAddress < 1";
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
            return ds;
        }
    }
}
