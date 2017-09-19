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
        public DataSet changguicheck(String time1, String time2,String cd, String measureNo)
        {
            return checkdao.changguicheck(time1, time2,cd, measureNo);
        }
        public DataSet changguicheck0(String time1, String time2, String cd, String measureNo)
        {
            return checkdao.changguicheck0(time1, time2, cd, measureNo);
        }
        public DataSet changguicheckliutengfeiPDF(String time1, String time2, String cd, String measureNo)
        {
            return checkdao.changguicheckliutengfeiPDF(time1, time2, cd, measureNo);
        }
        public DataSet changguicheckliutengfei(String time1, String time2, String cd, String measureNo)
        {
            return checkdao.changguicheckliutengfei(time1, time2, cd, measureNo);
        }
        public DataSet changguicheckliutengfeiGLZJ(String time1, String time2, String cd, String measureNo)
        {
            return checkdao.changguicheckliutengfeiGLZJ(time1, time2, cd, measureNo);
        }
        public DataTable changguicheckFenye(String time1, String time2, String cd, int PageIndex, int PageSize, String measureNo)
        {
            return checkdao.changguicheckFenye(time1, time2, cd, PageIndex, PageSize,measureNo);
        }
        public DataSet changguicheckGlzjliutengfei(String time1, String time2, String glzj)
        {
            return checkdao.changguicheckGlzjliutengfei(time1, time2, glzj);
        }
        public DataSet changguicheckGlzj0(String time1, String time2, String glzj)
        {
            return checkdao.changguicheckGlzj0(time1, time2, glzj);
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
        public DataSet checkcedianAll0(string code)
        {
            return checkdao.checkcedianAll0(code);
        }
        public DataSet checkCedianCar()
        {
            return checkdao.checkCedianCar();
        }
    }
}
