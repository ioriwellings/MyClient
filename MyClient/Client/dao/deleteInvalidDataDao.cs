using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using BOYA.CRMS.Common;

namespace LBKJClient.dao
{
    class deleteInvalidDataDao
    {
        private static object obj = new object();
        public void deleteInvalidData()
        {
         String sql1 = " delete from lb_base_data_home where warnState != '1' and warnState != '3'  and warningistrue != '2' and warningistrue != '3'  and houseinterval < 1  and carinterval < 1";

      lock (obj)
            {
                DbHelperMySQL.ExecuteSql(sql1);
            }


        }

    }
}
