using System;


namespace LBKJClient.dao
{
    class deleteInvalidDataDao
    {
        public void deleteInvalidData()
        {
         String sql1 = " delete from lb_base_data_home where warnState != '1' and warnState != '3' and warningistrue != '1'  and warningistrue != '2' and warningistrue != '3'  and houseinterval < 1  and carinterval < 1";
         DbHelperMySQL.ExecuteSql(sql1);
        }

    }
}
