using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.bean
{
    struct allDataSer {

        public Cntinfo cntinfo { get; set; }
        public List<bean.dataSerialization> deviceinfo { get; set; }//数组处理
    }
    public struct Cntinfo
    {
        public string recordCnt { get; set; }
        public string totalCnt { get; set; }
        public int pageIndex { get; set; }
    }
}
