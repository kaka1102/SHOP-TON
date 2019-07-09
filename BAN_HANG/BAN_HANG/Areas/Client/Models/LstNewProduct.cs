using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAN_HANG.Areas.Client.Models
{
    public class LstNewProduct
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string path_img { get; set; }
        public string description_img { get; set; }
        public float gia_xuat { get; set; }
        public string type_price { get; set; }
        public int id_cate { get; set; }
    }
}