using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
 public   class SessionUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int Roleid { get; set; }
        public string TitleWeb { get; set; }
        public string Code_brand { get; set; }
    }
}
