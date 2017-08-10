
using System.Data;


namespace LBKJClient.dao
{
    class chartsDao
    {
        public DataTable chartscheckdao(string time1, string time2, string measure, string meter)
        {
            string sql = "select * from (select a.temperature,a.humidity,a.devtime,a.warningistrue,a.houseinterval,a.carinterval from lb_base_data_home a where 1=1 and a.devtime > '" + time1 + "' and  a.devtime <  '" + time2 + "' and  a.measureCode='" + measure + "' and a.meterNo='" + meter + "' order by a.devtime) aa where aa.warningistrue > 0 or aa.houseinterval > 0 or aa.carinterval > 0";
    
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
