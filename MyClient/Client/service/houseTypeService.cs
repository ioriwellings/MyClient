using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace LBKJClient.service
{
    class houseTypeService
    {
        dao.houseTypeDao ht = new dao.houseTypeDao();
        public DataTable queryhouseType()
        {
            return ht.queryhouseType();
        }
        public bool updateHouseTypeById(string imgpath,string id)
        {
            return ht.updateHouseTypeById(imgpath,id);
        }
        public bool deleteHouseTypeById(string id)
        {
            return ht.deleteHouseTypeById(id);
        }
        public bool addHouseManage(bean.houseInfo hi)
        {
            return ht.addHouseManage(hi);
        }
        public bool updateHouseInfoById(bean.houseInfo hi)
        {
            return ht.updateHouseInfoById(hi);
        }
    }
}
