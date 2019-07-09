using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class lichsu_nhap
    {
        public int id { get; set; }
        public int RowNum { get; set; }
        public string tensp { get; set; }
        public string ngaynhap { get; set; }
        public int soluong { get; set; }
        public string nguoinhap { get; set; }

        public float tongtien { get; set; }
        public float giatrungbinh { get; set; }
        public string tennguon { get; set; }
        public string id_nguonnhap { get; set; }
        public string tendaily { get; set; }
        public int id_center { get; set; }
        public string barcod_sp { get; set; }
        public string group_code { get; set; }
        public string note_code { get; set; }
        public int trangthai { get; set; }
        public string ngayxuat { get; set; }
        public string nguoixuat { get; set; }
        public string tenKH { get; set; }
        public int id_code_p { get; set; }
    }

    public class Return_lichsu_nhap
    {
        public List<lichsu_nhap> Data { get; set; }
        public int Total { get; set; }
    }
}
