using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
   public class ds_sp_theo_center
    {
        public int id_product { get; set; }
        public string tensp { get; set; }
        public int id_center { get; set; }
        public int soluong { get; set; }
        public string tennguon { get; set; }
        public string group_code_product { get; set; }
        public string ngaynhap { get; set; }
        public string mota { get; set; }
        public int soluongcon { get; set; }

    }
}
