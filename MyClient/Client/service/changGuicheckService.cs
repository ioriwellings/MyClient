using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LBKJClient.service
{
    class changGuicheckService
   {
        dao.changGuicheckDao checkdao = new dao.changGuicheckDao();
        public DataSet changguicheck(String time1, String time2,String cd)
        {
            return checkdao.changguicheck(time1, time2,cd);
        }
        public DataTable changguicheckFenye(String time1, String time2, String cd, int PageIndex, int PageSize)
        {
            return checkdao.changguicheckFenye(time1, time2, cd, PageIndex, PageSize);
        }
        public DataSet changguicheckGlzj(String time1, String time2, String glzj)
        {
            return checkdao.changguicheckGlzj(time1, time2, glzj);
        }
        public DataTable changguicheckGlzjFenye(String time1, String time2, String glzj, int PageIndex, int PageSize)
        {
            return checkdao.changguicheckGlzjFenye(time1, time2, glzj, PageIndex, PageSize);
        }
        public DataSet checkcedian(string code)
        {
            return checkdao.checkcedian(code);
        }
        public DataSet checkcedianAll(string code)
        {
            return checkdao.checkcedianAll(code);
        }
        public DataSet checkCedianCar()
        {
            return checkdao.checkCedianCar();
        }
    }
}
