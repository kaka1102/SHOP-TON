using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class Add_Bill
    {
        public string type { get; set; }
        public int loaibill { get; set; }  // 1,2,3 = DL,BL,HD
        public string data { get; set; }
        public string billType { get; set; }
        public long bonusTotal { get; set; }
        public long total { get; set; }
        public long discount { get; set; }
        public string billNumber { get; set; }
        public string ngayxuat { get; set; }
        public string nguoixuat { get; set; }
        public string tenkh { get; set; }
        public string diachi { get; set; }
        public string nguoidaidien { get; set; }
        public string dienthoai { get; set; }
        public int id_thaotac { get; set; }
        public int id_center { get; set; }
        public string code_center { get; set; }
        public string tennguoithaotac { get; set; }
        public int id_bill_detail { get; set; }
        public int id_bill { get; set; }
        public string notebill { get; set; }
    }
}
