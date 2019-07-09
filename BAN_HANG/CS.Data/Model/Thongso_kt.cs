using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
  public  class Thongso_kt
    {
        public int id_sys_attr_product { get; set; }
        public int id_product { get; set; }
        public int id_attr { get; set; }
        public string attr_name { get; set; }
        public string descripton_attr { get; set; }
    }
}
