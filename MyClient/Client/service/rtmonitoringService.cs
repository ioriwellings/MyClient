using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LBKJClient.service
{
    class rtmonitoringService
    {
        dao.rtmonitoringDao rt = new dao.rtmonitoringDao();
        public DataSet rtmonitoring(string time)
        {
            return rt.monitoring(time);
        }
        public DataTable queryMonitoringByhousecode(string code)
        {
            return rt.queryMonitoringByhousecode(code);
        }
    }
}
