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
    
    public partial class RoleUser
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> LevelId { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
