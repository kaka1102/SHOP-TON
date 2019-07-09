using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Data.DataContext;

namespace CS.Data.Model
{
  public  class ReturnListBill
    {
        public List<InforBill> Data { get; set; }
        public int Total { get; set; }
    }
   
    public class InforBill
    {
        public int RowNum { get; set; }
        public int id { get; set; }
        public string type { get; set; }
        public Nullable<int> id_customer { get; set; }
        public string tenKH { get; set; }
        public Nullable<double> tongtien { get; set; }
        public Nullable<double> giamgia { get; set; }
        public Nullable<double> thucnhan { get; set; }
        public string method { get; set; }
        public Nullable<int> ispay { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public string created_by { get; set; }
        public Nullable<int> id_nguoitao { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public string updated_by { get; set; }
        public int deleted { get; set; }
        public Nullable<int> status { get; set; }
        public string date_sale { get; set; }
        public Nullable<int> id_center { get; set; }
        public Nullable<int> billtype { get; set; }
        public string note { get; set; }
        public string tennguoixuat { get; set; }
        public string code { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
    }
}
