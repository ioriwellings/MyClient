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
        //tcp通信ip地址Port端口号
        public string tcp_ip_Port { get; set; }
        //GPRS通信
        public string networkType { get; set; }
        //主机记录间隔（分）
        public int RecordM { get; set; }
        //主机报警记录间隔（分）
        public int WarningM { get; set; }
        //主机短信报警手机号
        public int PhoneNo { get; set; }
        //是否需要设置主机记录、报警间隔，手机号（1需要）
        public int State { get; set; }
    }
}
