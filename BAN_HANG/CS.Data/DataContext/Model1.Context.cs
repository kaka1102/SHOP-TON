﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_CSEntities1 : DbContext
    {
        public DB_CSEntities1()
            : base("name=DB_CSEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<img_product> img_product { get; set; }
        public virtual DbSet<LevelRole> LevelRole { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuPermission> MenuPermission { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleUser> RoleUser { get; set; }
        public virtual DbSet<service_customer_bill> service_customer_bill { get; set; }
        public virtual DbSet<sys_attr_product> sys_attr_product { get; set; }
        public virtual DbSet<sys_attributes> sys_attributes { get; set; }
        public virtual DbSet<sys_code_product> sys_code_product { get; set; }
        public virtual DbSet<sys_log> sys_log { get; set; }
        public virtual DbSet<sys_parameter> sys_parameter { get; set; }
        public virtual DbSet<sys_product_detail> sys_product_detail { get; set; }
        public virtual DbSet<tbl_price> tbl_price { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<sys_tonkho> sys_tonkho { get; set; }
        public virtual DbSet<sys_Nhap> sys_Nhap { get; set; }
        public virtual DbSet<service_customer_bill_detail> service_customer_bill_detail { get; set; }
        public virtual DbSet<service_customer> service_customer { get; set; }
        public virtual DbSet<tbl_Unit> tbl_Unit { get; set; }
        public virtual DbSet<sys_nguonnhap> sys_nguonnhap { get; set; }
        public virtual DbSet<tbl_detail_category> tbl_detail_category { get; set; }
        public virtual DbSet<tbl_LoaiSP> tbl_LoaiSP { get; set; }
        public virtual DbSet<sys_product> sys_product { get; set; }
        public virtual DbSet<tbl_gioithieu> tbl_gioithieu { get; set; }
        public virtual DbSet<tbl_lienhe> tbl_lienhe { get; set; }
    }
}
