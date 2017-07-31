using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.bean
{
    class houseInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        
        public float t_high { get; set; }
        
        public float t_low { get; set; }
        
        public float h_high { get; set; }
        
        public float h_low { get; set; }
        
        public string imgPath { get; set; }

        public int isUsed { get; set; }
    }
}
