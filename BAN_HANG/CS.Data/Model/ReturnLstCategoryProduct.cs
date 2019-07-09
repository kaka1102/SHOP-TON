using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class ReturnLstCategoryProduct
    {
        public int id_category { get; set; }
        public string category_name { get; set; }
        public string description { get; set; }
        public bool isactive { get; set; }
        public int id_product { get; set; }
        public int id_detail { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string unit_name { get; set; }
        public string des_product { get; set; }
    }
}
