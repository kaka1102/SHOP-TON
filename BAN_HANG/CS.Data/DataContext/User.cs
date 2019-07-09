//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CS.Data.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.MenuPermission = new HashSet<MenuPermission>();
            this.RoleUser = new HashSet<RoleUser>();
        }
    
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string EmailSignature { get; set; }
        public string About { get; set; }
        public string Website { get; set; }
        public Nullable<int> GenderId { get; set; }
        public Nullable<int> MaritalStatusId { get; set; }
        public string Phone { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<System.DateTime> Dated { get; set; }
        public string UserCode { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateModify { get; set; }
        public string FullName { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string PositionName { get; set; }
        public Nullable<int> TeamId { get; set; }
        public Nullable<int> on_working { get; set; }
        public Nullable<System.DateTime> parent_create_time { get; set; }
        public Nullable<int> parent_create_by { get; set; }
        public Nullable<System.DateTime> parent_update_time { get; set; }
        public Nullable<int> parent_update_by { get; set; }
        public Nullable<int> user_create_by { get; set; }
        public Nullable<int> user_update_by { get; set; }
        public Nullable<bool> isusingaccount { get; set; }
        public string PhoneExt { get; set; }
        public Nullable<int> CS_number { get; set; }
        public Nullable<int> id_app_cls_push_cs { get; set; }
        public Nullable<int> status_password { get; set; }
    
        public virtual Branch Branch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MenuPermission> MenuPermission { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleUser> RoleUser { get; set; }
    }
}