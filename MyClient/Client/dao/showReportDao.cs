using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BOYA.CRMS.Common;
using Common;

namespace LBKJClient.dao
{
    class showReportDao
    {
        private static readonly object obj = new object();
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

            Monitor.Enter(obj);
            if (ishave == 0) {
                    rt = DbHelperMySQL.ExecuteSql(sql1);
            } else {
                DbHelperMySQL.ExecuteSql(sql2);
                rt = DbHelperMySQL.ExecuteSql(sql1);
            };
                Monitor.Exit(obj);

            return rt == 0 ? false : true;
        }
    }
}
