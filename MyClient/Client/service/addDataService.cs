using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.service
{
    class addDataService
    {
        dao.addDataDao add = new dao.addDataDao();
        public void addData(List<bean.dataSerialization> list)
        {
            add.queryRepeatData(list);
        }
        public void dataSynchronous(List<bean.dataSerialization> list)
        {
            add.dataSynchronous(list);
        }
        public DataTable checkDatasTimes(string time1,string time2)
        {
           return add.checkDatasTimes(time1,time2);
        }
        public DataTable checkLastRecordBIsOr(string measureMeterCode)
        {
            return add.checkLastRecordBIsOr(measureMeterCode);
        }
    }
}
