using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Threading;

namespace LBKJClient.dao
{
    class houseTypeDao
    {
        private static readonly object obj = new object();
        private string dbPath = "Data Source =new.baw";
        public DataTable queryhouseType()
        {
            DataSet ds = new DataSet();
            string sql = "select * from lb_house_type";
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
        public bool updateHouseTypeById(string imgpath,string id)
        {
            int ret = 0;
            String sql = "update lb_house_type set imgPath='" + imgpath + "'where id = '" + id + "'";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            conn.SetPassword(frmLogin.dataPassw);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SQLiteCommand cmdSearch = null;
            try
            {
                cmdSearch = new SQLiteCommand(sql, conn);
                Monitor.Enter(obj);
                ret = cmdSearch.ExecuteNonQuery();
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
            return ret == 0 ? false : true;
        }
        public bool deleteHouseTypeById(string id)
        {
            int ret = 0;
            String sql = "delete from lb_house_type where id = '" + id + "'";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            conn.SetPassword(frmLogin.dataPassw);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SQLiteCommand cmdSearch = null;
            try
            {
                cmdSearch = new SQLiteCommand(sql, conn);
                Monitor.Enter(obj);
                ret = cmdSearch.ExecuteNonQuery();
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
            return ret == 0 ? false : true;
        }

        public bool addHouseManage(bean.houseInfo hi)
        { 
            int ret = 0;
            String sql = "insert into lb_house_type (name,t_high,t_low,h_high,h_low) values ('" + hi.name + "', '" + hi.t_high + "', '" + hi.t_low + "', '" + hi.h_high + "', '" + hi.h_low + "')";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            SQLiteCommand cmdSearch = null;
            conn.SetPassword(frmLogin.dataPassw);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            try
            {
                cmdSearch = new SQLiteCommand(sql, conn);
                Monitor.Enter(obj);
                ret = cmdSearch.ExecuteNonQuery();
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
            return ret == 0 ? false : true;
        }
        public bool updateHouseInfoById(bean.houseInfo hi)
        {
            int ret = 0;
            String sql = "update lb_house_type set name='" + hi.name + "', t_high='" + hi.t_high + "', t_low='" + hi.t_low + "', h_high='" + hi.h_high + "', h_low='" + hi.h_low + "',isUsed='"+hi.isUsed+"' where id = '" + hi.id + "'";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            conn.SetPassword(frmLogin.dataPassw);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SQLiteCommand cmdSearch = null;
            try
            {
                cmdSearch = new SQLiteCommand(sql, conn);
                Monitor.Enter(obj);
                ret = cmdSearch.ExecuteNonQuery();
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
            return ret == 0 ? false : true;
        }
    }
}
