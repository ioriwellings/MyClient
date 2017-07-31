using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBKJClient.service
{
    class deleteInvalidDataService
    {
        dao.deleteInvalidDataDao delete = new dao.deleteInvalidDataDao();
        public void deleteInvalidData()
        {
            delete.deleteInvalidData();
        }


    }
}
