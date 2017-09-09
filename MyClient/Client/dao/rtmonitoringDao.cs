using System;
using System.Data;


namespace LBKJClient.dao
{
    class rtmonitoringDao
    {
        DataSet ds;
        public DataSet monitoring()
        {
            ds = new DataSet();
            ds.Clear();
            String sql = "select b.measureCode,b.meterNo,a.temperature,a.humidity,devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.warnState,a.sign,a.measureMeterCode,b.housetype,b.powerflag from lb_device_information b left join lb_base_data_home a on a.measureCode=b.measureCode and a.meterNo=b.meterNo join lb_managehost_info m on b.measureCode=m.measureCode order by m.createTime,b.measureCode,b.meterNo";
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }

        public DataTable queryMonitoringByhousecode(string code)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH") + ":00:00";
            ds = new DataSet();
            ds.Clear();
            String sql = "select b.measureCode,b.meterNo,a.temperature,a.humidity,devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.measureMeterCode,b.pointX,b.pointY,b.housetype from lb_device_information b left join lb_base_data_home a on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime>='" + time + "' join lb_managehost_info m on b.measureCode = m.measureCode where b.house_code='" + code+ "' and b.powerflag < 1  order by m.createTime,a.devtime";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
