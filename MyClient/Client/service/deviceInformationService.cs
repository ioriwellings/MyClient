using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.service
{
    class deviceInformationService
    {
        dao.deviceInformationDao deviceinfo = new dao.deviceInformationDao();
        public bool addDeviceInformation(bean.manageHose hm)
        {
            return deviceinfo.addDeviceInformationDao(hm);
        }
        public bool updateDeviceInformation(bean.manageHose hm)
        {
            return deviceinfo.updateDeviceInformationDao(hm);
        }
        public DataTable checkPointInfo(int flag)
        {
            return deviceinfo.checkPointInfo(flag);
        }
        public DataTable checkPointInfoRc()
        {
            return deviceinfo.checkPointInfoRc();
        }
        public bool updateIformation(bean.deviceInformation di)
        {
            return deviceinfo.updateIformationDao(di);
        }
        public bool insertDeviceInformation(bean.deviceInformation di)
        {
            return deviceinfo.insertDeviceInformation(di);
        }
        public bool queryDeviceBycode(bean.deviceInformation di)
        {
            return deviceinfo.queryDeviceBycode(di);
        }
        public bool deletetDeviceInformation(bean.deviceInformation di)
        {
            return deviceinfo.deletetDeviceInformation(di);
        }
        public DataTable queryDeviceByHouseTypeCode(string code)
        {
            return deviceinfo.queryDeviceByHouseTypeCode(code);
        }
        public bool updateIformationByPoint(bean.deviceInformation di)
        {
            return deviceinfo.updateIformationByPoint(di);
        }
        public bool updateIformationByHouseCode(string code)
        {
            return deviceinfo.updateIformationByHouseCode(code);
        }
        public bool updateWsdByHouseCode(bean.houseInfo hi)
        {
            return deviceinfo.updateWsdByHouseCode(hi);
        }
        public bool updateWsdByHouseKong(bean.houseInfo hi)
        {
            return deviceinfo.updateWsdByHouseKong(hi);
        }
        public DataTable selectBydeviceInfo(string measureCode,string meter)
        {
            return deviceinfo.selectBydeviceInfo(measureCode,meter);
        }
        public bool updateAllIformation(bean.deviceInformation di)
        {
            return deviceinfo.updateAllIformationDao(di);
        }
        public DataTable selectHouseTypeK()
        {
            return deviceinfo.selectHouseTypeK();
        }
        public bool updateBatchHouseType(string cd, string type)
        {
            return deviceinfo.updateBatchHouseType(cd, type);
        }
    }
}
