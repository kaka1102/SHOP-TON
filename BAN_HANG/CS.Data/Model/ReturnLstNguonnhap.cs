using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class ReturnLstNguonnhap
    {
        public List<Infornguonnhap> Data { get; set; }
        public int Total { get; set; }
    }


    public class Infornguonnhap
    {
        public int id { get; set; }
        public string tennguon { get; set; }
        public string diachi { get; set; }
        public string sdt { get; set; }
        public string code { get; set; }
        public bool isactive { get; set; }
        public string description { get; set; }
        public int id_center { get; set; }
        public string centername { get; set; }
    }
}
