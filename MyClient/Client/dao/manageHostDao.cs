using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace LBKJClient.dao
{
    class manageHostDao
    {
        private static readonly object obj = new object();
        private string dbPath = "Data Source =new.baw";
        public bool addmanageHostDao(bean.manageHose mh)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int ret = 0;
            String sql = "insert into lb_managehost_info (measureCode,hostName,hostAddress,CommunicationType,serialPort,portNumber,storeType,createTime,houseType) values ('" + mh.measureCode + "', '" + mh.hostName + "', '" + mh.hostAddress + "', '" + mh.CommunicationType + "', '" + mh.serialPort + "', '" + mh.portNumber + "', '" + mh.storeType + "', '" + time + "', '" + mh.houseType + "')";
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
                ret = cmdSearch.ExecuteNonQuery();
                Monitor.Exit(obj);
            }
            catch(SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
            finally {
            cmdSearch.Dispose();//释放命令执行器
            conn.Close();//断开数据库连接
            conn.Dispose();//释放连接器
            }
            return ret == 0 ? false : true;
        }

        public DataTable querymanageHostDao() {
            String sql = "SELECT m.*,h.name FROM lb_managehost_info m join lb_house_type h on m.houseType=h.id";
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
        public bool deletemanageHostDao(string id,string code) {
            int ret = 0;
            int cd = 0;
            String sql = "delete from lb_managehost_info where id = '"+id+"'";
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
                cmdSearch = new SQLiteCommand(sql,conn);
                Monitor.Enter(obj);
                ret = cmdSearch.ExecuteNonQuery();
                if (ret>0) {
                    cmdSearch.Dispose();//释放命令执行器
                    string sql1 = "delete from lb_device_information where measureCode = '"+code+"'";
                    cmdSearch = new SQLiteCommand(sql1, conn);
                    Monitor.Enter(obj);
                    cd =cmdSearch.ExecuteNonQuery();  
                }
                Monitor.Exit(obj);
            }
            catch (SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { 
            cmdSearch.Dispose();//释放命令执行器
            conn.Close();//断开数据库连接
            conn.Dispose();//释放连接器
            }
            return cd == 0 ? false : true;
        }
        public bool updatemanageHostDao(bean.manageHose mh)
        {
            int ret = 0;
            int cd = 0;
            String sql = "update lb_managehost_info set hostName='"+mh.hostName+"',hostAddress='"+mh.hostAddress+ "',CommunicationType='"+mh.CommunicationType+ "',serialPort='"+mh.serialPort+ "',portNumber='"+mh.portNumber+ "',storeType='"+mh.storeType+ "',houseType='" + mh.houseType + "' where measureCode = '" + mh.measureCode+"'";
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
                cmdSearch = new SQLiteCommand(sql, conn);
                Monitor.Enter(obj);
                ret = cmdSearch.ExecuteNonQuery();
                //if (ret > 0)
                //{
                //    cmdSearch.Dispose();//释放命令执行器
                //    string sql1 = "delete from lb_device_information where measureCode = '" + mh.measureCode + "'";
                //    cmdSearch = new SQLiteCommand(sql1, conn);
                //    cd = cmdSearch.ExecuteNonQuery();
                //}
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
            return cd == 0 ? false : true;
        }
        public bool updateManageHostCdNum(bean.manageHose mh)
        {
            int ret = 0;
            int cd = 0;
            String sql = "SELECT count(1) FROM lb_device_information where measureCode='" + mh.measureCode + "'";
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
                Monitor.Enter(obj);
                if (ret > 0)
                {
                    cmdSearch.Dispose();//释放命令执行器
                    string sql1 = "update lb_managehost_info set portNumber='" + ret.ToString() + "' where measureCode = '" + mh.measureCode + "'";
                    cmdSearch = new SQLiteCommand(sql1, conn);
                    cd = cmdSearch.ExecuteNonQuery();
                }
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
            return cd == 0 ? false : true;
        }
        //查询管理主机中  使用串口通信的主机信息  判断地址栏不为空
        public DataTable querymanageHostComDao()
        {
            String sql = "select * from lb_managehost_info where hostAddress > 0 and CommunicationType='[管理主机]LB863RSB_N1(LBGZ-02)' order by hostAddress";
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
        public DataTable queryManageHoststoreType(string measureCode)
        {
            String sql = "select id,measureCode,storeType from lb_managehost_info where measureCode = '" + measureCode + "'";
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
    }
}
