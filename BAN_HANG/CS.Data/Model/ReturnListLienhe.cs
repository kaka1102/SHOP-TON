using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Data.DataContext;

namespace CS.Data.Model
{
 public   class ReturnListLienhe
    {
        public List<item_lienhe> Data { get; set; }
        public int Total { get; set; }
    }
}
