using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.bean
{
    class deviceInformation
    {
        //管理主机id
        public string measureCode { get; set; }
        //仪表编号
        public string meterNo { get; set; }
        //仪表名称
        public string terminalname { get; set; }
        //温度上限
        public float t_high { get; set; }
        //温度下限
        public float t_low { get; set; }
        //湿度上限
        public float h_high { get; set; }
        //湿度下限
        public float h_low { get; set; }
        public string  housecode { get; set; }
        public int pointX { get; set; }
        public int pointY { get; set; }
        public int powerflag { get; set; }
    }

}


