using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class RU_GetAllUserViewModel
    {
        public List<lst_RU_GetAllUserViewModel> Data { get; set; }
        public int Total { get; set; }
    }

    public class lst_RU_GetAllUserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Level { get; set; }
        public string RoleName { get; set; }
        public string BRANDNAME { get; set; }
        public int IDROLE { get; set; }
        public int ROLEUSERID { get; set; }

    }
}
