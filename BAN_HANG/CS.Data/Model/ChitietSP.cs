using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class ChitietSP
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double gia_xuat { get; set; }
        public List<ImgProduct> LstImg { get; set; }
        public List<AttrProduct> LstAttrProduct { get; set; }
    }
    public class ImgProduct
    {
        public int id { get; set; }
        public string path_img { get; set; }
        public string description { get; set; }
    }
    public class AttrProduct
    {
        public int id { get; set; }
        public string attr_name { get; set; }
        public string descripton_attr { get; set; }
    }
}
