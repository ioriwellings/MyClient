using LBKJClient.dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.service
{
    class loginLogService
    {
        loginLogDao dao = new loginLogDao();
        public DataTable checkLog(string time1, string time2)
        {
            return dao.checkLog(time1, time2);
        }
        public bool addCheckLog(bean.loginLogBean lb)
        {
            return dao.addCheckLog(lb);
        }
    }
}
