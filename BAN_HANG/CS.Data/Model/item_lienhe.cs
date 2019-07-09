using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
 public   class item_lienhe
    {
        public int id { get; set; }
        public string hoten { get; set; }
        public string diachi { get; set; }
        public string dienthoai { get; set; }
        public string email { get; set; }
        public string tieude { get; set; }
        public string noidung { get; set; }
        public Nullable<int> trangthai { get; set; }
        public Nullable<int> id_rep { get; set; }
        public string ngaytraloi { get; set; }
        public string nguoitraloi { get; set; }
        public string ngaygui { get; set; }
    }
}
