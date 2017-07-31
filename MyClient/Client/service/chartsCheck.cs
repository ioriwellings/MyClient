using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LBKJClient.service
{
    class chartsCheck
    {
        public DataTable chartschecksService(string time1, string time2, string measure, string meter)
        {
            dao.chartsDao chartdao = new dao.chartsDao();
            return chartdao.chartscheckdao(time1, time2, measure, meter);
        }
    }
}
