using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LBKJClient.bean;
using System.Data.SQLite;
using System.Windows;

namespace LBKJClient.dao
{
    class UserDao
    {
        private static readonly object obj = new object();
        private string dbPath = "Data Source =new.baw";
        public UserInfo add(UserInfo user)
        {
            //String sql = "";
            return user;
        }
        public DataTable exists(UserInfo user)
        {
            string despwd = "";
            if (user.Pwd != null&&!"".Equals(user.Pwd)) {
                despwd = MemoryPassword.MyEncrypt.EncryptDES(user.Pwd);
            }
            String sql = "select id,name,power from userinfo where name = '" + user.UserName + "'AND pwd = '" + despwd + "' AND enable = 1";
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
        public bool updatePassWord(bean.UserInfo user)
        {
            int ret = 0;
            String sql = "update userinfo set pwd='" + user.Pwd + "' where name = '" + user.UserName + "'";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            conn.SetPassword(frmLogin.dataPassw);
            SQLiteCommand cmdSearch = null;
            try
            {
                if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
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
        public DataTable listUser()  
        {
            String sql = "select id,name,pwd,enable,createTime,power from userinfo where 1=1 and id > 2";
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
        public bool deleteUser(string id)
        {
            int ret = 0;
            String sql = "delete from userinfo where id = '" + id + "'";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            conn.SetPassword(frmLogin.dataPassw);
            SQLiteCommand cmdSearch = null;
            try
            {
                if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
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
        public bool addUser(bean.UserInfo ui )
        {
            if (ui.Enable==0) {
                ui.Enable = 1;
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int ret = 0;
            int rt = 0;
            String sql = "select * from userinfo where name='"+ui.UserName+"'";
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
                SQLiteDataReader reader = cmdSearch.ExecuteReader();
                while (reader.Read())
                {
                    ret = reader.GetInt16(0);
                }
                if (ret==0) {
                    cmdSearch.Dispose();//释放命令执行器
                    string sql1 = "insert into userinfo (name,pwd,enable,createTime,power) values ('" + ui.UserName + "', '" + ui.Pwd + "', '" + ui.Enable + "', '" + time + "', '" + ui.Power + "')";
                    cmdSearch = new SQLiteCommand(sql1, conn);
                    Monitor.Enter(obj);
                    rt = cmdSearch.ExecuteNonQuery();
                    Monitor.Exit(obj);
                }
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
        public bool updateUser(bean.UserInfo ui)
        {
            int ret = 0;
            String sql = "update userinfo set name='" + ui.UserName + "',enable='" + ui.Enable + "',power='"+ui.Power+"' where id='" +ui.Id+ "'";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);
            conn.SetPassword(frmLogin.dataPassw);
            SQLiteCommand cmdSearch = null;
            try
            {
                if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
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
