using System;
using System.Data;

namespace LBKJClient.dao
{
    class warningDao
    {
        public DataTable warningheck(String time1, String time2, String cd)
        {
            String sql = "";
            if (cd != null && cd != "0")
            {
                sql = @"select distinct aa.* from (select a.devtime,b.terminalname,a.measureCode,a.meterNo,a.temperature,a.humidity,a.warnState,b.t_high,b.t_low,b.h_high,b.h_low,case when h.warningTime != '' and h.warningTime != null  then   '已处理'  else  '
未处理'  END as  warningTime,'0.0' wdcz,'0.0'sdcz from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.mcc != '1' and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' and a.measureMeterCode='" + cd + "' and b.housetype <> 1 left join lb_warning_handle h on h.measureMeterCode=a.measureMeterCode and h.warningTime=a.devtime  where  a.warnState = '1' or a.warnState = '3' or a.warningistrue = '2' or a.warningistrue = '3') aa";
            }else {
                sql = @"select distinct aa.* from (select a.devtime,b.terminalname,a.measureCode,a.meterNo,a.temperature,a.humidity,a.warnState,b.t_high,b.t_low,b.h_high,b.h_low,case when h.warningTime != '' and h.warningTime != null  then   '已处理'  else  '
未处理'  END as  warningTime,'0.0' wdcz,'0.0'sdcz from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.mcc != '1' and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' and b.housetype != 1 left join lb_warning_handle h on h.measureMeterCode=a.measureMeterCode and h.warningTime=a.devtime  where  a.warnState = '1' or a.warnState = '3' or a.warningistrue = '2' or a.warningistrue = '3') aa";
            }
           
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public bool addWarningHandleInfo(bean.warningHandleBean whb)
        {
            string id = Result.GetNewId();
            int rt = 0;
            String sql = "insert into lb_warning_handle (id,handleUser,warningTime,handleTime,handleType,handleResult,handleTetails,createTime,measureMeterCode) values ('" + id + "','" + whb.handleUser + "', '" + whb.warningTime + "', '" + whb.handleTime + "', '" + whb.handleType + "', '" + whb.handleResult + "', '" + whb.handleTetails + "', '" + whb.createTime + "', '" + whb.measureMeterCode + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public DataTable warningHandlecheck(String time1, String time2)
        {
            string bs = "_"; 
            String sql = "select a.*,h.terminalname from lb_warning_handle a join lb_device_information h on a.measureMeterCode = h.measureCode || '"+bs+"' || h.meterNo and a.handleTime > '" + time1 + "' and a.handleTime < '" + time2 + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable warningCheck(String measureMeterCode, String time)
        {
            string [] mm= measureMeterCode.Split('_');
            string sql = "select a.* from lb_warning_handle a join lb_device_information b on b.measureCode='" + mm[0] + "' and b.meterNo='" + mm[1] + "' and  a.warningTime = '" + time + "' and b.housetype <> 1";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable checkData(String timew, String code, String meter)
        {
            string sql = "select h.measureCode,h.meterNo,h.temperature,h.humidity,h.devtime,d.t_high,d.t_low,d.h_high,d.h_low from lb_base_data_home h join lb_device_information d on d.measureCode=h.measureCode and d.meterNo=h.meterNo where h.devtime > '"+timew+"' and h.measureCode='"+code+"' and h.meterNo='"+meter+"' and h.warningistrue <> 2 and h.warnState <> 1";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
