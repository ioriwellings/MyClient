using System;
using System.Data;

namespace LBKJClient.dao
{
    class showReportDao
    {
        public DataTable queryReport()
        {
            DataSet ds = new DataSet();
            string sql = "select * from lb_show_report";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public bool addReport(bean.showReportBean rb)
        {
            int rt = 0;
            int ishave = 0;
            string id = Result.GetNewId();
            String sql0 = "select * from lb_show_report where type = '" + rb.type + "';";
            String sql1 = "insert into lb_show_report (id,createTime,eventInfo,type) values ('" + id + "','" + rb.createTime + "', '" + rb.eventInfo + "', '" + rb.type + "');";
            String sql2 = "delete from lb_show_report where type = '" + rb.type + "';";
            ishave = DbHelperMySQL.ExecuteSql(sql0);
            if (ishave == 0) {
                    rt = DbHelperMySQL.ExecuteSql(sql1);
            } else {
                DbHelperMySQL.ExecuteSql(sql2);
                rt = DbHelperMySQL.ExecuteSql(sql1);
            };
            return rt == 0 ? false : true;
        }
    }
}
