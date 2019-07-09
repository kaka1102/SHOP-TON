using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class ListRoleViewModel
    {
        public List<CustomerRole> Data { get; set; }
        public int Total { get; set; }
    }
    public class CustomerRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public string LevelName { get; set; }
        public string Description { get; set; }

    }
}
