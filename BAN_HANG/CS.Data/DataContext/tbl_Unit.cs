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
    
    public partial class tbl_Unit
    {
        public int id { get; set; }
        public string unit_name { get; set; }
        public Nullable<bool> isactive { get; set; }
        public string description { get; set; }
        public Nullable<int> id_center { get; set; }
    }
}