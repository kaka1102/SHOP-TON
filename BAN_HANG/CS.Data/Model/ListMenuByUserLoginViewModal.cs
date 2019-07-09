using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class ListMenuByUserLoginViewModal
    {
        public int id { get; set; }
        public int MenuId { get; set; }
        public string MenuText { get; set; }
        public string MenuURL { get; set; }
        public int ParentId { get; set; }
        public int trangthai { get; set; }
        public bool IsCreate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRead { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsExport { get; set; }
    }
}
