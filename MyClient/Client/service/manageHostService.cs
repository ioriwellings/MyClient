using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace LBKJClient.service
{
    class manageHostService
    {
        dao.manageHostDao manageHost = new dao.manageHostDao();
        public bool addManageHost(bean.manageHose mh)
        {    
            return manageHost.addmanageHostDao(mh);
        }
        public DataTable queryManageHost()
        {
            return manageHost.querymanageHostDao();
        }
        public bool deleteManageHost(string id,string code)
        {
            return manageHost.deletemanageHostDao(id,code);
        }
        public bool updateManageHost(bean.manageHose mh)
        {
            return manageHost.updatemanageHostDao(mh);
        }
        public bool updateManageHostCdNum(bean.manageHose mh)
        {
            return manageHost.updateManageHostCdNum(mh);
        }
        public DataTable queryManageHostCom()
        {
            return manageHost.querymanageHostComDao();
        }
        public DataTable queryManageHoststoreType(string measureCode)
        {
            return manageHost.queryManageHoststoreType(measureCode);
        }
    }
}
