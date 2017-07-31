using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.bean
{
    class warningHandleBean
    {
        //编号
        public int id { get; set; }
        //处理人
        public string handleUser { get; set; }
        //报警时间
        public string warningTime { get; set; }
        //处理时间
        public string handleTime { get; set; }
        //处理类型
        public string handleType { get; set; }
        //处理结果
        public string handleResult { get; set; }
        //详情
        public string handleTetails { get; set; }
        //时间
        public string createTime{ get; set; }
        //时间
        public string measureMeterCode { get; set; }
    }
}
