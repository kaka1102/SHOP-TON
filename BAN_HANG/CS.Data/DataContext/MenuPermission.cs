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
    
    public partial class MenuPermission
    {
        public int Id { get; set; }
        public Nullable<int> MenuId { get; set; }
        public int RoleId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<bool> IsCreate { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<bool> IsUpdate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsExport { get; set; }
    
        public virtual Menu Menu { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
