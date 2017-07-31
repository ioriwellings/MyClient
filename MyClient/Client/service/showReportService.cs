using LBKJClient.dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.service
{
    class showReportService
    {

        showReportDao dao = new showReportDao();
        public DataTable queryReport()
        {
            return dao.queryReport();
        }
        public bool addReport(bean.showReportBean rb)
        {
            return dao.addReport(rb);
        }
    }
}
