using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class ReturnListUnit
    {
        public List<InforUnit> Data { get; set; }
        public int Total { get; set; }
    }

    public class InforUnit
    {
        public int id { get; set; }
        public string unit_name { get; set; }
        public bool isactive { get; set; }
        public string description { get; set; }
        public int id_center { get; set; }
        public string centername { get; set; }
    }
}
