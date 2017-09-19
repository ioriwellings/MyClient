using System;
using System.Linq;
using System.Data;


namespace LBKJClient.dao
{
    class changGuicheckDao
    {
        public DataSet changguicheckliutengfeiPDF(String time1, String time2, String cd, String measureNo)
        {
            string cds1 = null;
            //string measureNos1 = null;
            String sql = @"SELECT DISTINCT
	`a`.`measureCode` AS `measureCode`,
	`a`.`meterNo` AS `meterNo`,
	`a`.`temperature` AS `temperature`,
	`a`.`humidity` AS `humidity`,
	`a`.`devtime` AS `devtime`,
	`b`.`terminalname` AS `terminalname`,
	`a`.`t_high` AS `t_high`,
	`a`.`t_low` AS `t_low`,
	`a`.`h_high` AS `h_high`,
	`a`.`h_low` AS `h_low`,
	`a`.`warnState` AS `warnState`,
	`a`.`measureMeterCode` AS `measureMeterCode`,
	`a`.`warningistrue` AS `warningistrue`,

	(
		CASE
		WHEN (`a`.`mcc` = '1') THEN
			'空库'
		ELSE
			'非空库'
		END
	) AS `housetype`,
	`a`.`mcc` AS `mcc`
	 
FROM
	(
		`data_home` `a`
		JOIN `lb_device_information` `b` ON (
			(
				(
					`a`.`measureCode` = `b`.`measureCode`
				)
				AND (
					`a`.`meterNo` = `b`.`meterNo`
				)
			)
		)
	) ";
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

        public DataSet changguicheckliutengfei(String time1, String time2, String cd, String measureNo)
        {
            string cds1 = null;
            //string measureNos1 = null;
            String sql = @"SELECT DISTINCT
	`a`.`measureCode` AS `measureCode`,
	`a`.`meterNo` AS `meterNo`,
	`a`.`temperature` AS `temperature`,
	`a`.`humidity` AS `humidity`,
	`a`.`devtime` AS `devtime`,
	`b`.`terminalname` AS `terminalname`,
	`a`.`t_high` AS `t_high`,
	`a`.`t_low` AS `t_low`,
	`a`.`h_high` AS `h_high`,
	`a`.`h_low` AS `h_low`,
	`a`.`warnState` AS `warnState`,
	`a`.`measureMeterCode` AS `measureMeterCode`,
	`a`.`warningistrue` AS `warningistrue`,
	`a`.`carinterval` AS `carinterval`,
	`a`.`houseinterval` AS `houseinterval`,
	(
		CASE
		WHEN (`a`.`mcc` = '1') THEN
			'空库'
		ELSE
			'非空库'
		END
	) AS `housetype`,
	`a`.`mcc` AS `mcc`
	 
FROM
	(
		`lb_warning_data` `a`
		JOIN `lb_device_information` `b` ON (
			(
				(
					`a`.`measureCode` = `b`.`measureCode`
				)
				AND (
					`a`.`meterNo` = `b`.`meterNo`
				)
			)
		)
	) ";
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
        public DataSet changguicheckGlzj(String time1, String time2, String glzj)
        {
            String sql = "select distinct a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc from data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "'";
            if (glzj != null)
            {
                sql += "  where a.measureCode='" + glzj + "'";
            }
            sql += " order by a.devtime DESC";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }
        public DataSet changguicheckliutengfeiGLZJ(String time1, String time2, String cd, String measureNo)
        {
            string cds1 = null;
            //string measureNos1 = null;
            String sql = @"SELECT DISTINCT
	`a`.`measureCode` AS `measureCode`,
	`a`.`meterNo` AS `meterNo`,
	`a`.`temperature` AS `temperature`,
	`a`.`humidity` AS `humidity`,
	`a`.`devtime` AS `devtime`,
	`b`.`terminalname` AS `terminalname`,
	`a`.`t_high` AS `t_high`,
	`a`.`t_low` AS `t_low`,
	`a`.`h_high` AS `h_high`,
	`a`.`h_low` AS `h_low`,
	`a`.`warnState` AS `warnState`,
	`a`.`measureMeterCode` AS `measureMeterCode`,
	`a`.`warningistrue` AS `warningistrue`,
	`a`.`carinterval` AS `carinterval`,
	`a`.`houseinterval` AS `houseinterval`,
	(
		CASE
		WHEN (`a`.`mcc` = '1') THEN
			'空库'
		ELSE
			'非空库'
		END
	) AS `housetype`,
	`a`.`mcc` AS `mcc`
	 
FROM
	(
		`lb_warning_data` `a`
		JOIN `lb_device_information` `b` ON (
			(
				(
					`a`.`measureCode` = `b`.`measureCode`
				)
				AND (
					`a`.`meterNo` = `b`.`meterNo`
				)
			)
		)
	) ";
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
            if (measureNo != null)
            {
                sql += " and measureNo = '" + measureNo + "'";
            }
            sql += " order by devtime DESC";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }


        /// <summary>
        /// 视图，统计的数据 View_WarningData
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="cd"></param>
        /// <param name="measureNo"></param>
        /// <returns></returns>
        /// 
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
        public DataSet changguicheck0(String time1, String time2, String cd, String measureNo)
        {
            /*
             SELECT
	count(1),
	MAX(temperature),
	MIN(temperature),
	AVG(temperature),
	MAX(humidity),
	MIN(humidity),
	AVG(humidity)
FROM
	data_home
WHERE
	devtime > '2017-01-17 20:03:49'
AND devtime < '2017-09-18 20:04:00'
AND measureMeterCode IN (
	'111_01',
	'111_02',
	'111_03',
	'111_04',
	'111_05',
	'111_06',
	'111_07',
	'111_08',
	'111_09',
	'121_01',
	'3hh_01',
	'3hh_02',
	'3hh_03',
	'3hh_04',
	'3hh_05',
	'3hh_06',
	'3hh_07',
	'3hh_08',
	'3hh_09',
	'cc_01',
	'cc_02',
	'cc_03',
	'GZ04083017050025_01',
	'GZ04083017050025_02',
	'GZ04083017050025_03',
	'GZ04083017050025_04',
	'GZ04083017050025_05',
	'GZ04083017050025_06',
	'GZ04083017050025_07',
	'GZ04083017050025_08',
	'GZ04083017050025_09',
	'GZ04083017050025_10',
	'GZ04083017050025_10',
	'GZ04083017050025_11',
	'GZ04083017050025_12',
	'GZ04083017050025_13',
	'GZ04083017050025_14',
	'GZ04083017050025_15',
	'GZ04083017050025_16',
	'GZ04083017050025_17',
	'GZ04083017050025_18',
	'GZ04083017050025_19',
	'GZ04083017050025_20',
	'GZ04083017050025_21',
	'GZ04083017050025_22',
	'GZ04083017050025_23',
	'GZ04083017050025_24',
	'GZ04083017050025_25'
)
UNION ALL
SELECT
	count(1),
	0,
	0,
	0,
	0,
	0,
	0
FROM
	data_home
WHERE mcc = '0'and warningistrue='2' or warningistrue='3' or warnState='1' or warnState='3' and
	devtime > '2017-01-17 20:03:49'
AND devtime < '2017-09-18 20:04:00'
AND measureMeterCode IN (
	'111_01',
	'111_02',
	'111_03',
	'111_04',
	'111_05',
	'111_06',
	'111_07',
	'111_08',
	'111_09',
	'121_01',
	'3hh_01',
	'3hh_02',
	'3hh_03',
	'3hh_04',
	'3hh_05',
	'3hh_06',
	'3hh_07',
	'3hh_08',
	'3hh_09',
	'cc_01',
	'cc_02',
	'cc_03',
	'GZ04083017050025_01',
	'GZ04083017050025_02',
	'GZ04083017050025_03',
	'GZ04083017050025_04',
	'GZ04083017050025_05',
	'GZ04083017050025_06',
	'GZ04083017050025_07',
	'GZ04083017050025_08',
	'GZ04083017050025_09',
	'GZ04083017050025_10',
	'GZ04083017050025_10',
	'GZ04083017050025_11',
	'GZ04083017050025_12',
	'GZ04083017050025_13',
	'GZ04083017050025_14',
	'GZ04083017050025_15',
	'GZ04083017050025_16',
	'GZ04083017050025_17',
	'GZ04083017050025_18',
	'GZ04083017050025_19',
	'GZ04083017050025_20',
	'GZ04083017050025_21',
	'GZ04083017050025_22',
	'GZ04083017050025_23',
	'GZ04083017050025_24',
	'GZ04083017050025_25'
)
*/
            string cds1 = null;
            //string measureNos1 = null;
            String sql = @"SELECT
	count(1),
	MAX(temperature),
	MIN(temperature),
	AVG(temperature),
	MAX(humidity),
	MIN(humidity),
	AVG(humidity)
FROM
	data_home
WHERE ";
            sql += "  devtime > '" + time1 + "' and  devtime <  '" + time2 + "'";


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
            sql += " UNION ALL ";
            sql += @"SELECT
	count(1),
	0,
	0,
	0,
	0,
	0,
	0
FROM
	data_home
WHERE ";
            sql += "  devtime > '" + time1 + "' and  devtime <  '" + time2 + "'";


            if (cd != null)
            {

                sql += " and measureMeterCode in ('" + cds1 + "')";
            }
            sql += " and  mcc = '0'and warningistrue='2' or warningistrue='3' or warnState='1' or warnState='3' ";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }
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

        public DataSet changguicheckGlzjliutengfei(String time1, String time2, String glzj)
        {
            String sql = "select distinct a.measureCode,a.meterNo,a.temperature,a.humidity,a.devtime,b.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,a.warnState,a.measureMeterCode,a.warningistrue,a.carinterval,a.houseinterval,case when a.mcc = '1' then '空库' else '非空库' end as housetype,a.mcc from data_home a join lb_device_information b on a.measureCode=b.measureCode and a.meterNo=b.meterNo and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "'";
            if (glzj != null)
            {
                sql += "  where a.measureCode='" + glzj + "'";
            }
            sql += " order by a.devtime DESC";
            DataSet ds = new DataSet();
             
            ds = DbHelperMySQL.Query(sql);
            return ds;
        }
        public DataSet changguicheckGlzj0(String time1, String time2,String glzj)
        { 
            //string measureNos1 = null;
            String sql = @"SELECT
	count(1),
	MAX(temperature),
	MIN(temperature),
	AVG(temperature),
	MAX(humidity),
	MIN(humidity),
	AVG(humidity)
FROM
	data_home
WHERE ";
            sql += "  devtime > '" + time1 + "' and  devtime <  '" + time2 + "'";


            if (glzj != null)
            {
                sql += "  and measureCode='" + glzj + "'";
            }
            sql += " UNION ALL ";
            sql += @"SELECT
	count(1),
	0,
	0,
	0,
	0,
	0,
	0
FROM
	data_home
WHERE ";
            sql += "  devtime > '" + time1 + "' and  devtime <  '" + time2 + "'";
            if (glzj != null)
            {
                sql += "  and measureCode='" + glzj + "'";
            }
            sql += " and  mcc = '0'and warningistrue='2' or warningistrue='3' or warnState='1' or warnState='3' ";
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
            else
            {
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
                sql = "select measureCode,meterNo,terminalname,CONCAT(measureCode,'_',meterNo) AS measureMeterCode from lb_device_information where measureCode='" + code + "'";
            }
            else
            {
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
