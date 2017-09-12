using System;
using System.Linq;
using System.Data;


namespace LBKJClient.dao
{
    class changGuicheckDao
    {
        /// <summary>
        /// 视图，统计的数据 View_WarningData
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="cd"></param>
        /// <param name="measureNo"></param>
        /// <returns></returns>
        public DataSet changguicheck(String time1, String time2, String cd, String measureNo)
        {
            string cds1 = null;
            //string measureNos1 = null;
            String sql = @"select * from View_WarningData";
            sql += "  where  devtime > '" + time1 + "' and  devtime <  '" + time2 + "'";

            //if (measureNo != null)
            //{
            //    string[] measureNos = measureNo.Split(',');
            //    for (int i = 0; i < measureNos.Count(); i++)
            //    {
            //        measureNos1 += "," + measureNos[i];
            //    }
            //    measureNos1 = measureNos1.Substring(1);
            //    sql += " and measureNo in ('" + measureNos1 + "')";
            //}
            if (cd != null)
            {
                string[] cds = cd.Split(',');
                for (int i = 0; i < cds.Count(); i++)
                {
                    cds1 += "','" + cds[i];
                }
                cds1 = cds1.Substring(3);
                sql += " and measureMeterCode in ('" + cds1 + "')";
            }
            sql += " order by devtime DESC";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }

        //        public DataSet changguicheck(String time1, String time2, String cd, String measureNo)
        //        {
        //            string cds1 = null;
        //            string measureNos1 = null;
        //            String sql = @"select distinct a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc  
        //from data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo 
        // ";
        //            if (measureNo != null)
        //            {
        //                 sql += "  where  a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' and";
        //                string[] measureNos = measureNo.Split(',');
        //                for (int i = 0; i < measureNos.Count(); i++)
        //                {
        //                    measureNos1 += "," + measureNos[i];
        //                }
        //                measureNos1 = measureNos1.Substring(1);
        //                sql += "  a.measureNo in ('" + measureNos1 + "')";
        //            }
        //            if (cd != null)
        //            {
        //                //  sql += "  where  ";
        //                string[] cds = cd.Split(',');
        //                for (int i = 0; i < cds.Count(); i++)
        //                {
        //                    cds1 += "','" + cds[i];
        //                }
        //                cds1 = cds1.Substring(3);
        //                sql += " and a.measureMeterCode in ('" + cds1 + "')";
        //            }
        //            sql += " order by a.devtime DESC";
        //            DataSet ds = new DataSet();
        //            ds.Clear();
        //            ds = DbHelperMySQL.Query(sql);
        //            return ds;
        //        }
        public DataTable changguicheckFenye(String time1, String time2, String cd, int PageIndex, int PageSize, String measureNo)
        {
            string cds1 = null;
            string measureNos1 = null;
            PageIndex = (PageIndex - 1) * PageSize;
            String sql = @"select * from View_WarningData";

            if (measureNo != null)
            {
                sql += "  where  devtime > '" + time1 + "' and  devtime <  '" + time2 + "' ";
                string[] measureNos = measureNo.Split(',');
                for (int i = 0; i < measureNos.Count(); i++)
                {
                    measureNos1 += "','" + measureNos[i];
                }
                measureNos1 = measureNos1.Substring(3);
                sql += " and measureNo in ('" + measureNos1 + "')";
            }
            if (cd != null)
            {
                string[] cds = cd.Split(',');
                for (int i = 0; i < cds.Count(); i++)
                {
                    cds1 += "','" + cds[i];
                }
                cds1 = cds1.Substring(3);
                sql += " and measureMeterCode in ('" + cds1 + "')";
            }
            sql += " order by devtime DESC";
            sql += " limit " + PageIndex + "," + PageSize + "";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }


        //        public DataTable changguicheckFenye(String time1, String time2, String cd, int PageIndex, int PageSize, String measureNo)
        //        {
        //            string cds1 = null;
        //            string measureNos1 = null;
        //            PageIndex = (PageIndex - 1) * PageSize;
        //            String sql = @"select distinct a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc  
        //from data_home a
        //                join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo
        //where  a.devtime > '"
        //+ time1 + "' and  a.devtime <  '" + time2 + "' ";
        //            if (measureNo != null)
        //            {
        //                //  sql += "  where  ";
        //                string[] measureNos = measureNo.Split(',');
        //                for (int i = 0; i < measureNos.Count(); i++)
        //                {
        //                    measureNos1 += "," + measureNos[i];
        //                }
        //                measureNos1 = measureNos1.Substring(1);
        //                sql += " and a.measureNo in ('" + measureNos1 + "')";
        //            }
        //            if (cd != null)
        //            {
        //                //sql += "  where  ";
        //                string[] cds = cd.Split(',');
        //                for (int i = 0; i < cds.Count(); i++)
        //                {
        //                    cds1 += "','" + cds[i];
        //                }
        //                cds1 = cds1.Substring(3);
        //                sql += " and a.measureMeterCode in ('" + cds1 + "')";
        //            }
        //            sql += " order by a.devtime DESC";
        //            sql += " limit " + PageIndex + "," + PageSize + "";
        //            DataSet ds = new DataSet();
        //            ds.Clear();
        //            ds = DbHelperMySQL.Query(sql);
        //            return ds.Tables[0];
        //        }
        public DataSet changguicheckGlzj(String time1, String time2, String glzj)
        {
            String sql = "select distinct a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc from data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "'";
            if (glzj != null)
            {
                sql += "  where a.measureCode='"+ glzj + "'";
            }
            sql += " order by a.devtime DESC";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);          
            return ds;
        }
        public DataTable changguicheckGlzjFenye(String time1, String time2, String glzj, int PageIndex, int PageSize)
        {
            PageIndex = (PageIndex - 1) * PageSize;
            String sql = "select distinct a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc from data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "'";
            if (glzj != null)
            {
                sql += "  where a.measureCode='" + glzj + "'";
            }
            sql += " order by a.devtime DESC";
            sql += " limit " + PageIndex + "," + PageSize + "";
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
                sql = "select b.measureCode,b.meterNo,b.terminalname,CONCAT(b.measureCode,'_', b.meterNo) AS measureMeterCode from lb_device_information b where 1 = 1 and  b.measureCode || '_'|| b.meterNo='" + code + "'";
            }
            else {
                sql = "select b.measureCode,b.meterNo,b.terminalname,CONCAT(b.measureCode,'_', b.meterNo) AS measureMeterCode from lb_device_information b where 1 = 1  order by  b.measureCode,b.meterNo";
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
                sql = "select a.measureCode,a.meterNo,a.terminalname,b.storeType,b.measureNo from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode order by  a.measureCode,a.meterNo";
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
