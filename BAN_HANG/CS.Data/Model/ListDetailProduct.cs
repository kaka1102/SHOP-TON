using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class ListDetailProduct
    {
        public int id { get; set; }
        public string product_name { get; set; }
        public string TYPE { get; set; }
        public float gia_xuat { get; set; }
        public float gia_nhap { get; set; }
        public int quantity { get; set; }
        public string type_price { get; set; }
        public string code { get; set; }
        public string unit_name { get; set; }
        public bool isactive { get; set; }
        public string name_display { get; set; }


        public int id_gia { get; set; }
        public int id_img { get; set; }
        public string url_img { get; set; }
        public string path_img { get; set; }
        public string des_img { get; set; }
        public string attr_name { get; set; }
        public string descripton_attr { get; set; }
        public int id_attr { get; set; }
        public int id_config_attr { get; set; }
        public int soluong { get; set; }
        
    }
}
