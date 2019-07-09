using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{
   public class ListUserPageListViewModal
    {
        public List<ListUserPageList> ListUserPageList { get; set; }

        public int Total { get; set; }
    }

    public class ListUserPageList
    {
        public long? RowNum { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string EmailSignature { get; set; }
        public string About { get; set; }
        public string Website { get; set; }
        public int? GenderId { get; set; }
        public int? MaritalStatusId { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public int? ParentId { get; set; }
        public DateTime? Dated { get; set; }
        public string UserCode { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModify { get; set; }
        public string FullName { get; set; }
        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public string PositionName { get; set; }
        public int? TeamId { get; set; }
        public string depName { get; set; }
        public string centerName { get; set; }
        public string teamName { get; set; }

        public string rolename { get; set; }
    }
}
