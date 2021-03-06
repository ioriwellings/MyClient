﻿using System;
using System.Collections.Generic;
using System.Data;


namespace LBKJClient.dao
{
    class addDataDao
    {
        public void queryRepeatData(List<bean.dataSerialization> list)
        {
            string id;
            if (list != null&& list.Count > 0)
                {
                    String sql1 = "insert into lb_base_data_home (id,measureCode,meterNo,devtime,temperature,humidity,lng,lat,createDate,warnState,sign,measureMeterCode,warningistrue,carinterval,houseinterval,mcc) values ";
                    for (int j = 0; j < list.Count; j++)
                    {
                       id = Result.GetNewId();
                      if (j > 0)
                        {
                            sql1 += " , ('" + id + "','" + list[j].managerID + "', '" + list[j].deviceNum + "','" + list[j].devicedate + "','" + list[j].temperature + "','" + list[j].humidity + "','" + list[j].lng + "','" + list[j].lat + "','" + list[j].sysdate + "','" + list[j].warnState + "','" + list[j].sign + "','" + list[j].measureMeterCode + "','" + list[j].warningistrue + "','" + list[j].carinterval + "','" + list[j].houseinterval + "',case when (select max(housetype) from lb_device_information where measureCode = '" + list[j].managerID + "') = '1' then  '1' else '0' end)";
                        }
                        else
                        {
                            sql1 += "('" + id + "','" + list[j].managerID + "', '" + list[j].deviceNum + "','" + list[j].devicedate + "','" + list[j].temperature + "','" + list[j].humidity + "','" + list[j].lng + "','" + list[j].lat + "','" + list[j].sysdate + "','" + list[j].warnState + "','" + list[j].sign + "','" + list[j].measureMeterCode + "','" + list[j].warningistrue + "','" + list[j].carinterval + "','" + list[j].houseinterval + "',case when (select max(housetype) from lb_device_information where measureCode = '" + list[j].managerID + "') = '1' then  '1' else '0' end)";
                        }
                    }
                  DbHelperMySQL.ExecuteSql(sql1);
                }


        }
        public void dataSynchronous(List<bean.dataSerialization> list)
        {
               string id;
                    if (list.Count > 0)
                    {
                        String sql1 = "insert into lb_base_data_home (id,measureCode,meterNo,warnState,devtime,temperature,humidity,lng,lat,createDate,measureMeterCode,warningistrue,carinterval,houseinterval,mcc) values ";
                        for (int j = 0; j < list.Count; j++)
                        {
                          id = Result.GetNewId();
                         if (j > 0)
                            {
                                sql1 += " , ('" + id + "','" + list[j].managerID + "', '" + list[j].deviceNum + "','" + list[j].warnState + "','" + list[j].devicedate + "','" + list[j].temperature + "','" + list[j].humidity + "','" + list[j].lng + "','" + list[j].lat + "','" + list[j].sysdate + "','" + list[j].measureMeterCode + "','" + list[j].warningistrue + "','" + list[j].carinterval + "','" + list[j].houseinterval + "',case when (select max(housetype) from lb_device_information where measureCode = '" + list[j].managerID + "') = '1' then  '1' else '0' end)";
                            }
                            else
                            {
                                sql1 += "('" + id + "','" + list[j].managerID + "', '" + list[j].deviceNum + "','" + list[j].warnState + "','" + list[j].devicedate + "','" + list[j].temperature + "','" + list[j].humidity + "','" + list[j].lng + "','" + list[j].lat + "','" + list[j].sysdate + "','" + list[j].measureMeterCode + "','" + list[j].warningistrue + "','" + list[j].carinterval + "','" + list[j].houseinterval + "',case when (select max(housetype) from lb_device_information where measureCode = '" + list[j].managerID + "') = '1' then  '1' else '0' end)";
                            }
                        }
                      DbHelperMySQL.ExecuteSql(sql1);    
                    }
        }
        public DataTable checkDatasTimes(string time1, string time2)
        {
            String sql = "select a.measureCode,a.meterNo,a.devtime from lb_base_data_home a where a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable checkLastRecordBIsOr(string measureMeterCode, string devicedate)
        {
            String sql = "select max(aa.devtime), aa.warnState, aa.warningistrue, aa.devtime from(select id, warnState, warningistrue, devtime from lb_base_data_home where measureMeterCode = '" + measureMeterCode + "' and devtime < '" + devicedate + "') aa ";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

    }
}
