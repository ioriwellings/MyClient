using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;

namespace LBKJClient.dao
{
    class addDataDao
    {
        private static object obj = new object();
        private string dbPath = "Data Source =new.baw";
        public void queryRepeatData(List<bean.dataSerialization> list)
        {
            SQLiteConnection conn = null;
            SQLiteCommand cmd = null;
            String sql = null;
            SQLiteDataReader reader = null;
            int ret = 0;
            try
            {
                conn = new SQLiteConnection(dbPath);
                conn.SetPassword(frmLogin.dataPassw);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                //for (int i = 0; i < list.Count; i += 1)
                //{
                //    sql = "select id from lb_base_data_home d where devtime='" + list[i].devicedate + "' and d.measureCode='" + list[i].managerID + "' and d.meterNo='" + list[i].deviceNum + "'";
                //    cmd = new SQLiteCommand(sql, conn);
                //    reader = cmd.ExecuteReader();
                //    if (reader != null)
                //    {
                //        while (reader.Read())
                //        {
                //            ret = reader.GetInt32(0);
                //        }
                //    }
                //    if (ret > 0)
                //    {
                //        list.RemoveAt(i);
                //        if (i > 0)
                //        {
                //            --i;
                //        }
                //        else
                //        {
                //            i = -1;
                //        }

                //    }
                //    ret = 0;
                //    cmd.Dispose();//释放命令执行器
                //}
                if (list != null&& list.Count > 0)
                {
                    String sql1 = "insert into lb_base_data_home (measureCode,meterNo,devtime,temperature,humidity,lng,lat,createDate,speed,direction,warnState,sign,gpsFlag,measureMeterCode,warningistrue,carinterval,houseinterval) values ";
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (j > 0)
                        {
                            sql1 += " , ('" + list[j].managerID + "', '" + list[j].deviceNum + "','" + list[j].devicedate + "','" + list[j].temperature + "','" + list[j].humidity + "','" + list[j].lng + "','" + list[j].lat + "','" + list[j].sysdate + "','" + list[j].speed + "','" + list[j].direction + "','" + list[j].warnState + "','" + list[j].sign + "','" + list[j].gpsFlag + "','" + list[j].measureMeterCode + "','" + list[j].warningistrue + "','" + list[j].carinterval + "','" + list[j].houseinterval + "')";
                        }
                        else
                        {
                            sql1 += "('" + list[j].managerID + "', '" + list[j].deviceNum + "','" + list[j].devicedate + "','" + list[j].temperature + "','" + list[j].humidity + "','" + list[j].lng + "','" + list[j].lat + "','" + list[j].sysdate + "','" + list[j].speed + "','" + list[j].direction + "','" + list[j].warnState + "','" + list[j].sign + "','" + list[j].gpsFlag + "','" + list[j].measureMeterCode + "','" + list[j].warningistrue + "','" + list[j].carinterval + "','" + list[j].houseinterval + "')";
                        }
                    }
                    cmd = new SQLiteCommand(sql1, conn);
                    //Monitor.Enter(obj);
                    lock (obj)
                    {
                        cmd.ExecuteNonQuery();
                    }
                        //Monitor.Exit(obj);
                }
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                //MessageBox.Show(ex.Message);
                //throw new Exception(ex.Message);
            }
            finally
            {   
                //reader.Close();
                cmd.Dispose();//释放命令执行器
                conn.Close();//断开数据库连接
                conn.Dispose();//释放连接器
                
            }

        }
        public void dataSynchronous(List<bean.dataSerialization> list)
        {
            SQLiteConnection conn = null;
            SQLiteCommand cmd = null;
            try
            {
                conn = new SQLiteConnection(dbPath);
                conn.SetPassword(frmLogin.dataPassw);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                    if (list.Count > 0)
                    {
                        String sql1 = "insert into lb_base_data_home (measureCode,meterNo,warnState,devtime,temperature,humidity,lng,lat,createDate,speed,direction,gpsFlag,measureMeterCode,warningistrue,carinterval,houseinterval) values ";
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (j > 0)
                            {
                                sql1 += " , ('" + list[j].managerID + "', '" + list[j].deviceNum + "','" + list[j].warnState + "','" + list[j].devicedate + "','" + list[j].temperature + "','" + list[j].humidity + "','" + list[j].lng + "','" + list[j].lat + "','" + list[j].sysdate + "','" + list[j].speed + "','" + list[j].direction + "','" + list[j].gpsFlag + "','" + list[j].measureMeterCode + "','" + list[j].warningistrue + "','" + list[j].carinterval + "','" + list[j].houseinterval + "')";
                            }
                            else
                            {
                                sql1 += "('" + list[j].managerID + "', '" + list[j].deviceNum + "','" + list[j].warnState + "','" + list[j].devicedate + "','" + list[j].temperature + "','" + list[j].humidity + "','" + list[j].lng + "','" + list[j].lat + "','" + list[j].sysdate + "','" + list[j].speed + "','" + list[j].direction + "','" + list[j].gpsFlag + "','" + list[j].measureMeterCode + "','" + list[j].warningistrue + "','" + list[j].carinterval + "','" + list[j].houseinterval + "')";
                            }
                        }
                        cmd = new SQLiteCommand(sql1, conn);
                        //Monitor.Enter(obj);
                        lock (obj)
                        {
                            cmd.ExecuteNonQuery();
                        }
                        //Monitor.Exit(obj);
                    }
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                //MessageBox.Show(ex.Message);
                //throw new Exception(ex.Message);
            }
            finally
            {
                //reader.Close();
                //cmd.Dispose();//释放命令执行器
                conn.Close();//断开数据库连接
                conn.Dispose();//释放连接器

            }

        }
        public DataTable checkDatasTimes(string time1, string time2)
        {
            String sql = "select a.measureCode,a.meterNo,a.devtime from lb_base_data_home a where a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "'";
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
        public DataTable checkLastRecordBIsOr(string measureMeterCode)
        {
            String sql = "select max(datetime(aa.devtime)), aa.warnState, aa.warningistrue, aa.devtime from(select id, warnState, warningistrue, devtime from lb_base_data_home where measureMeterCode = '" + measureMeterCode + "') aa where aa.warnState = 1 or aa.warningistrue = 2 or aa.warnState = 3 or aa.warningistrue = 3";
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
