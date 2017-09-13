using System;
using System.Data;
using System.Linq;
using System.Windows;

namespace LBKJClient.dao
{
    class deviceInformationDao
    {
        public bool addDeviceInformationDao(bean.manageHose hm)
        {
            string id;
            string id0;
            int flag = 0;
            string wd1 = "35.0";
            string wd2 = "0.0";
            string sd1 = "75.0";
            string sd2 = "35.0";
            string cd = null;
            string m = null;
            int ret = 0;
            String sql = "insert into lb_device_information (id,measureCode,meterNo,terminalname,house_code,housetype,t_high,t_low,h_high,h_low,powerflag,createtime) values ";
            String sql0 = "insert into lb_base_data_home (id,measureCode,meterNo,devtime,temperature,humidity,lng,lat,createDate,warnState,sign,measureMeterCode,warningistrue,carinterval,houseinterval,mcc,measureNo) values ";

            int num = Int32.Parse(hm.portNumber); 
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            for (int j = 0; j < num; j++) { 
                if (j < 10)
                {
                    if (hm.CommunicationType == "串口通讯协议" || hm.CommunicationType == "TCP协议" || hm.CommunicationType == "云平台协议-01")
                    {
                        if (j + 1 == 10) {
                            m = (j + 1).ToString();
                        }
                        else {
                            m = "0" + (j + 1).ToString();
                        }
                    }
                    else {
                        if (hm.CommunicationType == "云平台协议-00")
                        {
                            if (num == 1)
                            {
                                m = "00";
                            }
                            else {
                                m = "0" + j.ToString();
                            }
                        }
                    }
                    cd += hm.hostName + "-"+m;
                }
                else {
                    if(hm.CommunicationType == "串口通讯协议" || hm.CommunicationType == "TCP协议" || hm.CommunicationType == "云平台协议-01") { m = (j + 1).ToString(); }
                    else
                    {
                        m = j.ToString();
                    }
                    cd += hm.hostName + "-" + m;
                }
                id = Result.GetNewId();
                id0 = Result.GetNewId();
                int measureNo = System.Math.Abs(id.GetHashCode());
                measureNo = Int32.Parse(measureNo.ToString().Substring(0, 5));
                if (j > 0)
                {
                    sql += " ,('" + id + "','" + hm.measureCode + "', '" + m + "', '" + cd + "','" + hm.houseType + "', '" + 0 + "', '" + wd1 + "', '" + wd2 + "', '" + sd1 + "', '" + sd2 + "', '" + flag + "', '" + time + "')";
                    sql0 += " , ('" + id0 + "','" + hm.measureCode + "', '" + m + "','" + time + "','0.0','0.0','','','" + time + "','','',CONCAT('" + hm.measureCode + "', '_', '" + m + "'),'','','','0','" + measureNo + "')";
                }
                else {
                    sql += "('" + id + "','" + hm.measureCode + "', '" + m + "', '" + cd + "','" + hm.houseType + "', '" + 0 + "', '" + wd1 + "', '" + wd2 + "', '" + sd1 + "', '" + sd2 + "', '" + flag + "', '" + time + "')";
                    sql0 += "('" + id0 + "','" + hm.measureCode + "', '" + m + "','" + time + "','0.0','0.0','','','" + time + "','','',CONCAT('" + hm.measureCode + "', '_', '" + m + "'),'','','','0','" + measureNo + "')";

                }
                cd = null;
            }
            ret = DbHelperMySQL.ExecuteSql(sql);
            if (ret > 0) { ret = DbHelperMySQL.ExecuteSql(sql0); }
           
            
            return ret == 0 ? false : true;
        }
        public DataTable checkPointInfo(int flag) {

            DataSet ds = new DataSet();
            ds.Clear();
            String sql=null;
            if (flag == 0)
            {
                sql = "select a.id,a.measureCode,a.meterNo,a.terminalname,b.hostAddress,b.CommunicationType,b.serialPort,a.t_high,a.t_low,a.h_high,a.h_low,h.name,a.powerflag from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode left join lb_house_type h on a.house_code=h.id ORDER BY b.createTime,a.measureCode,a.meterNo";
            }
            else if(flag == 3)
            {
                sql = "select a.id,a.measureCode,a.meterNo,a.terminalname,b.hostAddress,b.CommunicationType,b.serialPort,a.t_high,a.t_low,a.h_high,a.h_low,h.name from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode and b.networkType = 'YUN' left join lb_house_type h on a.house_code = h.id ORDER BY b.createTime,a.measureCode,a.meterNo";  
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable checkPointInfoRc()
        {
            //获取RC-10、-8的设备信息
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "select a.id,a.measureCode,a.meterNo,a.terminalname,b.hostAddress,b.CommunicationType,b.serialPort,a.t_high,a.t_low,a.h_high,a.h_low,h.name from lb_device_information a join lb_managehost_info b on a.measureCode = b.measureCode and b.CommunicationType = 'RC-8/-10' left join lb_house_type h on a.house_code=h.id";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool updateIformationDao(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "";
            if (di.powerflag > 0)
            {
                sql = "update lb_device_information set terminalname='" + di.terminalname + "',t_high='" + di.t_high + "',t_low='" + di.t_low + "',h_high='" + di.h_high + "',h_low='" + di.h_low + "',powerflag='" + di.powerflag + "' where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
            }
            else {
                sql = "update lb_device_information set terminalname='" + di.terminalname + "',house_code='" + di.housecode + "',housetype ='0',t_high='" + di.t_high + "',t_low='" + di.t_low + "',h_high='" + di.h_high + "',h_low='" + di.h_low + "',powerflag='" + di.powerflag + "' where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
            }
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public bool insertDeviceInformation(bean.deviceInformation di)
        {
            string id = Result.GetNewId();
            int flag = 0;
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int ret = 0;
            String sql = "insert into lb_device_information (id,measureCode,meterNo,terminalname,house_code,housetype,t_high,t_low,h_high,h_low,powerflag,createtime) values ('" + id + "','" + di.measureCode + "', '" + di.meterNo + "', '" + di.terminalname + "', '" + di.housecode + "','" + 0 + "', '" + di.t_high + "', '" + di.t_low + "', '" + di.h_high + "', '" + di.h_low + "', '" + flag + "', '" + time + "')";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
       
        public bool queryDeviceBycode(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "SELECT count(1) FROM lb_device_information where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == -1 ? true : false;
        }
        public bool deletetDeviceInformation(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "delete from lb_device_information where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public DataTable queryDeviceByHouseTypeCode(string code)
        {

            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "select a.id,a.measureCode,a.meterNo,a.terminalname,a.pointX,a.pointY from lb_device_information a where a.house_code='" + code+"'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool updateIformationByPoint(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "update lb_device_information set pointX='" + di.pointX + "',pointY='" + di.pointY + "' where measureCode='" + di.measureCode + "' and meterNo='" + di.meterNo + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public bool updateIformationByHouseCode(string code)
        {
            int ret = 0;
            String sql = "update lb_device_information set house_code='' where house_code='" + code + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public bool updateWsdByHouseCode(bean.houseInfo hi)
        {
            int ret = 0;        
            String sql = "update lb_device_information set t_high='" + hi.t_high + "',t_low='" + hi.t_low + "',h_high='" + hi.h_high + "',h_low='" + hi.h_low + "',housetype='" + hi.isUsed + "' where house_code='" + hi.id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public bool updateWsdByHouseKong(bean.houseInfo hi)
        {
            int ret = 0;
            String sql = "update lb_device_information set housetype='" + hi.isUsed + "' where house_code='" + hi.id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }

        public DataTable selectBydeviceInfo(string measureCode, string meter)
        {
            String sql = "";
            DataSet ds = new DataSet();
            ds.Clear();
            if (measureCode != "" && !"".Equals(measureCode) && meter != "" && !"".Equals(meter))
            {
                sql = "select a.measureCode,a.meterNo,a.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,b.hostAddress,b.CommunicationType from lb_device_information a join lb_managehost_info b on a.measureCode=b.measureCode and b.measureCode='" + measureCode + "' and a.meterNo='" + meter + "'";
            }
            else
            {
                sql = "select a.measureCode,a.meterNo,a.terminalname,a.t_high,a.t_low,a.h_high,a.h_low,b.hostAddress,b.CommunicationType from lb_device_information a join lb_managehost_info b on a.measureCode=b.measureCode";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool updateBatchHouseType(string cd, string type)
        {
            int ret = 0;
            string terminalnames1 = null;
            string sql = "update lb_device_information set house_code = '" + type + "'";
            if (cd != null)
            {
                sql += "  where  ";
                string[] terminalnames = cd.Split(',');
                for (int i = 0; i < terminalnames.Count(); i++)
                {
                    terminalnames1 += "','" + terminalnames[i];
                }
                terminalnames1 = terminalnames1.Substring(3);
                sql += "terminalname in ('" + terminalnames1 + "')";
            }
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }

        public bool updateAllIformationDao(bean.deviceInformation di)
        {
            int ret = 0;
            String sql = "update lb_device_information set t_high='" + di.t_high + "',t_low='" + di.t_low + "',h_high='" + di.h_high + "',h_low='" + di.h_low + "' where 1=1";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public DataTable selectHouseTypeK()
        {
            DataSet ds = new DataSet();
            ds.Clear();
            String sql = "select CONCAT(measureCode,'_',meterNo) AS measureMeterCode from lb_device_information where housetype = 1 ";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}