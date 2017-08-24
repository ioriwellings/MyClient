using System;
using System.Linq;
using System.Data;


namespace LBKJClient.dao
{
    class changGuicheckDao
    {
        public DataSet changguicheck(String time1, String time2, String cd)
        {
            string cds1 = null;
            String sql = "select aa.* from (select a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc  from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' order by a.devtime) aa";
            if (cd!=null) {
                sql += "  where  ";
                string[] cds = cd.Split(',');
                for (int i = 0; i < cds.Count(); i++)
                {
                    cds1 += "','" + cds[i];
                }
                cds1 = cds1.Substring(3);
                sql += "aa.measureMeterCode in ('" + cds1 + "')";
            }
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }
        public DataTable changguicheckFenye(String time1, String time2, String cd, int PageIndex, int PageSize)
        {
            string cds1 = null;
            PageIndex = (PageIndex - 1) * PageSize;
            String sql = "select aa.* from (select a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc  from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' order by a.devtime) aa";
            if (cd != null)
            {
                sql += "  where  ";
                string[] cds = cd.Split(',');
                for (int i = 0; i < cds.Count(); i++)
                {
                    cds1 += "," + cds[i];
                }
                cds1 = cds1.Substring(1);
                sql += "aa.measureMeterCode in ('" + cds1 + "') limit " + PageIndex + "," + PageSize + "";
            }

            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataSet changguicheckGlzj(String time1, String time2, String glzj)
        {
            String sql = "select aa.* from (select a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' order by a.devtime) aa";
            if (glzj != null)
            {
                sql += "  where aa.measureCode='"+ glzj + "'";
            }
           
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);          
            return ds;
        }
        public DataTable changguicheckGlzjFenye(String time1, String time2, String glzj, int PageIndex, int PageSize)
        {
            PageIndex = (PageIndex - 1) * PageSize;
            String sql = "select aa.* from (select a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,b.t_high,b.t_low,b.h_high,b.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' order by a.devtime) aa";
            if (glzj != null)
            {
                sql += "  where aa.measureCode='" + glzj + "' limit " + PageIndex + "," + PageSize + "";
            }

            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataSet checkcedian(string code)
        {
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = null;
            if (code != null && !"".Equals(code))
            {
                sql = "select a.measureCode,a.meterNo,b.terminalname,a.measureMeterCode from lb_base_data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo where a.measureCode='" + code + "' group by a.measureCode,a.meterNo order by b.createtime";
            }
            else {
                sql = "select a.measureCode,a.meterNo,b.terminalname,a.measureMeterCode from lb_device_information b left join lb_base_data_home a on a.measureCode=b.measureCode and a.meterNo=b.meterNo group by a.measureCode,a.meterNo,b.terminalname order by b.createtime";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }
        public DataSet checkcedianAll(string code)
        {
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "";
            if (code != null && !"".Equals(code))
            {
                sql = "select measureCode,meterNo,terminalname,CONCAT(measureCode,'_',meterNo) AS measureMeterCode from lb_device_information where measureCode='" + code+"'";
            }
            else {
                sql = "select a.measureCode,a.meterNo,a.terminalname,b.storeType from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }
        public DataSet checkcedianAll0(string code)
        {
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "";
            sql = "select a.measureCode,a.meterNo,a.terminalname,b.storeType from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode and a.terminalname like CONCAT('%', '" + code + "','%')";
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }
        public DataSet checkCedianCar()//获得车载的所有测点信息
        {
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "select a.measureCode,a.meterNo,a.terminalname,b.storeType from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode and b.hostAddress < 1 order by b.createTime,a.measureCode,a.meterNo";
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }
    }
}
