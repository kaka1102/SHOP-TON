using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
    public class ListAccountUsingRoleById
    {
        public List<AccountUsingRoleById> Data { get; set; }
        public int Total { get; set; }
    }

    public class AccountUsingRoleById
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public int LevelId { get; set; }
        public string RoleName { get; set; }
        public int Level { get; set; }
        public string UserName { get; set; }
        public string LevelName { get; set; }
        public string Description { get; set; }
    }
}
