using System;
using System.Data;
using LBKJClient.bean;

namespace LBKJClient.dao
{
    class UserDao
    {
        public DataTable exists(UserInfo user)
        {
            string despwd = "";
            if (user.Pwd != null&&!"".Equals(user.Pwd)) {
                despwd = MemoryPassword.MyEncrypt.EncryptDES(user.Pwd);
            }
            String sql = "select id,name,power from userinfo where name = '" + user.UserName + "'AND pwd = '" + despwd + "' AND enable = 1";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool updatePassWord(bean.UserInfo user)
        {
            int ret = 0;
            String sql = "update userinfo set pwd='" + user.Pwd + "' where name = '" + user.UserName + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public DataTable listUser()  
        {
            String sql = "select id,name,pwd,enable,createTime,power from userinfo where 1=1 and name != 'admin'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool deleteUser(string id)
        {
            int ret = 0;
            String sql = "delete from userinfo where id = '" + id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public bool addUser(bean.UserInfo ui )
        {
            if (ui.Enable==0) {
                ui.Enable = 1;
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int ret = 0;
            int rt = 0;
            String sql = "select * from userinfo where name='"+ui.UserName+"'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            string id = Result.GetNewId();
                if (ret== -1) {
                    string sql1 = "insert into userinfo (id,name,pwd,enable,createTime,power) values ('" + id + "','" + ui.UserName + "', '" + ui.Pwd + "', '" + ui.Enable + "', '" + time + "', '" + ui.Power + "')";
                    rt = DbHelperMySQL.ExecuteSql(sql1);
                }

            return rt == 0 ? false : true;
        }
        public bool updateUser(bean.UserInfo ui)
        {
            int ret = 0;
            String sql = "update userinfo set name='" + ui.UserName + "',enable='" + ui.Enable + "',power='"+ui.Power+"' where id='" +ui.Id+ "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
    }
}
