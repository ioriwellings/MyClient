using System;
using System.Data;

namespace LBKJClient.dao
{
    class loginLogDao
    {
        public DataTable checkLog(string time1, string time2)
        {
            DataSet ds = new DataSet();
            string sql = "select * from lb_login_log a where a.createTime > '" + time1 + "' and  a.createTime <  '" + time2 + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public bool addCheckLog(bean.loginLogBean lb)
        {
            int rt = 0;
            string id = Result.GetNewId();
            String sql = "insert into lb_login_log (id,name,createTime,eventInfo) values ('" + id + "','" + lb.name + "', '" + lb.createTime + "', '" + lb.eventInfo + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
    }
}
