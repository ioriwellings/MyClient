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
    class deviceInformationDao
    {
        private static readonly object obj = new object();
        private string dbPath = "Data Source =new.baw";
        public bool addDeviceInformationDao(bean.manageHose hm)
        {
            int flag = 0;
            string wd1 = "35.0";
            string wd2 = "0.0";
            string sd1 = "75.0";
            string sd2 = "35.0";
            string cd = null;
            string m = null;
            int ret = 0;
            String sql = "insert into lb_device_information (measureCode,meterNo,terminalname,house_code,housetype,t_high,t_low,h_high,h_low,powerflag,createtime) values ";
            int num = Int32.Parse(hm.portNumber); 
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            for (int j = 0; j < num; j++) { 
                if (j < 10)
                {
                    if (hm.CommunicationType != "LBCC-16" && hm.CommunicationType!= "RC-8/-10" && hm.CommunicationType != "LB410D")
                    {
                        if (j + 1 == 10) {
                            m = (j + 1).ToString();
                        }
                        else {
                            m = "0" + (j + 1).ToString();
                        }
                    }
                    else {
                        if (hm.CommunicationType == "LBCC-16" || hm.CommunicationType == "LB410D" || hm.CommunicationType == "RC-8/-10")
                        {
                            if (num == 1)
                            {
                                m = "00";
                            }
                            else {
                                m = "0" + j.ToString();
                            }
                        }
                    }
                    cd += hm.hostName + "-"+m;
                }
                else {
                    if(j >= 10 && hm.CommunicationType == "LBCC-16") { m = j.ToString(); }
                    else if (j >= 10 && hm.CommunicationType == "LB410D") { m = j.ToString(); }
                    else if (j >= 10 && hm.CommunicationType == "RC-8/-10") { m = j.ToString(); }
                    else
                    {
                        m = (j + 1).ToString();
                    }
                    cd += hm.hostName + "-" + m;
                }
                if (j > 0)
                {
                    sql += " ,('" + hm.measureCode + "', '" + m + "', '" + cd + "', '" + 1 + "', '" + 0 + "', '" + wd1 + "', '" + wd2 + "', '" + sd1 + "', '" + sd2 + "', '" + flag + "', '" + time + "')";
                }
                else {
                    sql += "('" + hm.measureCode + "', '" + m + "', '" + cd + "', '" + 1 + "', '" + 0 + "', '" + wd1 + "', '" + wd2 + "', '" + sd1 + "', '" + sd2 + "', '" + flag + "', '" + time + "')";
                }
               cd = null;
            }
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
            } catch (SQLiteException ex)
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
        public DataTable checkPointInfo(int flag) {

            DataSet ds = new DataSet();
            ds.Clear();
            String sql;
            if (flag == 0)
            {
                sql = "select a.id,a.measureCode,a.meterNo,a.terminalname,b.hostAddress,b.CommunicationType,b.serialPort,a.t_high,a.t_low,a.h_high,a.h_low,h.name from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode and hostAddress<1 left join lb_house_type h on a.house_code=h.id";
            }
            else {
                sql = "select a.id,a.measureCode,a.meterNo,a.terminalname,b.hostAddress,b.CommunicationType,b.serialPort,a.t_high,a.t_low,a.h_high,a.h_low,h.name,a.powerflag from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode left join lb_house_type h on a.house_code=h.id";
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
            return ds.Tables[0];
        }

        public DataTable checkPointInfoRc()
        {//获取RC-10、-8的设备信息
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "select a.id,a.measureCode,a.meterNo,a.terminalname,b.hostAddress,b.CommunicationType,b.serialPort,a.t_high,a.t_low,a.h_high,a.h_low,h.name from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode and b.CommunicationType = 'RC-8/-10' left join lb_house_type h on a.house_code=h.id";
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
        public bool updateIformationDao(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "";
            if (di.powerflag > 0)
            {
                sql = "update lb_device_information set terminalname='" + di.terminalname + "',t_high='" + di.t_high + "',t_low='" + di.t_low + "',h_high='" + di.h_high + "',h_low='" + di.h_low + "',powerflag='" + di.powerflag + "' where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
            }
            else {
                sql = "update lb_device_information set terminalname='" + di.terminalname + "',house_code='" + di.housecode + "',t_high='" + di.t_high + "',t_low='" + di.t_low + "',h_high='" + di.h_high + "',h_low='" + di.h_low + "',powerflag='" + di.powerflag + "' where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
            }
            
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
        public bool insertDeviceInformation(bean.deviceInformation di)
        {
            int flag = 0;
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int ret = 0;
            string cdname = di.measureCode + "_" + di.meterNo;
            String sql = "insert into lb_device_information (measureCode,meterNo,terminalname,house_code,housetype,t_high,t_low,h_high,h_low,powerflag,createtime) values ('" + di.measureCode + "', '" + di.meterNo + "', '" + cdname + "', '" + di.housecode + "','" + 0 + "', '" + di.t_high + "', '" + di.t_low + "', '" + di.h_high + "', '" + di.h_low + "', '" + flag + "', '" + time + "')";
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
       
        public bool queryDeviceBycode(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "SELECT count(1) FROM lb_device_information where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
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
            return ret == 0 ? true : false;
        }
        public bool deletetDeviceInformation(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "delete from lb_device_information where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
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
        public DataTable queryDeviceByHouseTypeCode(string code)
        {

            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "select a.id,a.measureCode,a.meterNo,a.terminalname,a.pointX,a.pointY from lb_device_information a where a.house_code='" + code+"'";
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
        public bool updateIformationByPoint(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "update lb_device_information set pointX='" + di.pointX + "',pointY='" + di.pointY + "' where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
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
        public bool updateIformationByHouseCode(string code)
        {
            int ret = 0;
            String sql = "update lb_device_information set house_code='' where house_code='" + code + "'";
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
        public bool updateWsdByHouseCode(bean.houseInfo hi)
        {
            int ret = 0;        
            String sql = "update lb_device_information set t_high='" + hi.t_high + "',t_low='" + hi.t_low + "',h_high='" + hi.h_high + "',h_low='" + hi.h_low + "',housetype='" + hi.isUsed + "' where house_code='" + hi.id + "'";

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
        public bool updateWsdByHouseKong(bean.houseInfo hi)
        {
            int ret = 0;
            String sql = "update lb_device_information set housetype='" + hi.isUsed + "' where house_code='" + hi.id + "'";         
            
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

        public DataTable selectBydeviceInfo(string measureCode, string meter)
        {
            String sql = "";
            DataSet ds = new DataSet();
            ds.Clear();
            if (measureCode != "" && !"".Equals(measureCode) && meter != "" && !"".Equals(meter))
            {
                sql = "select a.measureCode,a.meterNo,a.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,b.hostAddress,b.CommunicationType from lb_device_information a join lb_managehost_info b on a.measureCode=b.measureCode and b.measureCode='" + measureCode + "' and a.meterNo='" + meter + "'";
            }
            else
            {
                sql = "select a.measureCode,a.meterNo,a.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,b.hostAddress,b.CommunicationType from lb_device_information a join lb_managehost_info b on a.measureCode=b.measureCode";
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
            return ds.Tables[0];
        }
        public bool updateAllIformationDao(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "update lb_device_information set t_high='" + di.t_high + "',t_low='" + di.t_low + "',h_high='" + di.h_high + "',h_low='" + di.h_low + "' where 1=1";
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
        public DataTable selectHouseTypeK()
        {
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "select CAST(measureCode AS VARCHAR(50)) || '_' || CAST(meterNo AS VARCHAR(10)) as measureMeterCode from lb_device_information where housetype = 1 ";
           
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