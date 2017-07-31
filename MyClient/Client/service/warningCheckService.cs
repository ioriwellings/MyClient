using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LBKJClient.service
{
    class warningCheckService
    {
        dao.warningDao warningdao = new dao.warningDao();
        public DataTable warningcheck(String time1, String time2, String cd)
        {
            return warningdao.warningheck(time1, time2, cd);
        }
        public bool addWarningHandleInfo(bean.warningHandleBean whb)
        {
            return warningdao.addWarningHandleInfo(whb);
        }
        public DataTable warningHandlecheck(String time1, String time2)
        {
            return warningdao.warningHandlecheck(time1, time2);
        }
        public DataTable warningCheck(String measureMeterCode, String time)
        {
            return warningdao.warningCheck(measureMeterCode, time);
        }
        public DataTable checkData(String timew, String code, String meter)
        {
            return warningdao.checkData(timew, code, meter);
        }
    }
}
