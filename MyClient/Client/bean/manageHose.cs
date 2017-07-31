using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.bean
{
    class manageHose
    {
        //管理主机id
        public string measureCode { get; set; }
        //管理主机名称
        public string hostName { get; set; }
        //地址
        public string hostAddress { get; set; }
        //通信协议
        public string CommunicationType { get; set; }
        //通信端口
        public string serialPort { get; set; }
        //测点个数
        public string portNumber { get; set; }
        //使用类别
        public string storeType { get; set; }
        //库房类别
        public string houseType { get; set; }
    }
}
