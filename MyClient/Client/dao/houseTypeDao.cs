using System;
using System.Data;

namespace LBKJClient.dao
{
    class houseTypeDao
    {
        public DataTable queryhouseType()
        {
            DataSet ds = new DataSet();
            string sql = "select * from lb_house_type";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool updateHouseTypeById(string imgpath,string id)
        {
            int ret = 0;
            String sql = "update lb_house_type set imgPath='" + imgpath + "'where id = '" + id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public bool deleteHouseTypeById(string id)
        {
            int ret = 0;
            String sql = "delete from lb_house_type where id = '" + id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }

        public bool addHouseManage(bean.houseInfo hi)
        { 
            int ret = 0;
            string id = Result.GetNewId();
            String sql = "insert into lb_house_type (id,name,t_high,t_low,h_high,h_low) values ('" + id + "','" + hi.name + "', '" + hi.t_high + "', '" + hi.t_low + "', '" + hi.h_high + "', '" + hi.h_low + "')";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public bool updateHouseInfoById(bean.houseInfo hi)
        {
            int ret = 0;
            String sql = "update lb_house_type set name='" + hi.name + "', t_high='" + hi.t_high + "', t_low='" + hi.t_low + "', h_high='" + hi.h_high + "', h_low='" + hi.h_low + "',isUsed='"+hi.isUsed+"' where id = '" + hi.id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
    }
}
