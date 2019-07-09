using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Data.DataContext;

namespace CS.Data.Model
{
 public   class ReturnListCategory
    {
        public List<tbl_LoaiSP> Data { get; set; }
        public int Total { get; set; }
    }
}
