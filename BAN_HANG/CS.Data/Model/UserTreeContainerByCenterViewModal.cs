using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
  public  class UserTreeContainerByCenterViewModal
    {
        public string text { get; set; }

        public string icon { get; set; }
        public List<UserTreeViewModal> children { get; set; }
    }

    public class UserTreeViewModal
    {
        public string text { get; set; }
        public int id { get; set; }
        public int? idparent { get; set; }
        public string icon { get; set; }
        public List<UserTreeViewModal> children { get; set; }
    }
}
