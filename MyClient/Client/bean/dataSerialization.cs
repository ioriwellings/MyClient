using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.bean
{
    class dataSerialization
    {
        //管理主机号
        public string managerID { get; set; }
        //仪表号
        public string deviceNum { get; set; }
        //数据采集时间
        public string devicedate { get; set; }
        //温度
        public string temperature { get; set; }
        //湿度
        public string humidity { get; set; }
        //经度
        public string lng { get; set; }
        //纬度
        public string lat { get; set; }
        //数据上报时间
        public string sysdate { get; set; }
        //速度值
        public string speed { get; set; }
        //方向  
        public string direction { get; set; }
        //定位信息
        public string gpsFlag { get; set; }
        //主机号+仪表号
        public string measureMeterCode { get; set; }
        //使用类型
        public string devicetype { get; set; }
        //是否报警
        public string warningistrue { get; set; }
        //车间隔时间
        public string carinterval { get; set; }
        //库间隔时间
        public string houseinterval { get; set; }
        //断电报警2
        public string warnState { get; set; }
        //断电报警1
        public string sign { get; set; }
        //GZ04-DTU是否断电(0断电 1不断电)
        public string charge { get; set; }
    }
}
