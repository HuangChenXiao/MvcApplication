﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Custom.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BuyunSiteEntities : DbContext
    {
        public BuyunSiteEntities()
            : base("name=BuyunSiteEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BA_Advertisement> BA_Advertisement { get; set; }
        public virtual DbSet<Cu_Menu> Cu_Menu { get; set; }
        public virtual DbSet<Cu_User> Cu_User { get; set; }
        public virtual DbSet<Cu_UserAuthority> Cu_UserAuthority { get; set; }
        public virtual DbSet<BA_Case> BA_Case { get; set; }
        public virtual DbSet<BA_Contact> BA_Contact { get; set; }
        public virtual DbSet<BA_FriendshipLink> BA_FriendshipLink { get; set; }
        public virtual DbSet<BA_Information> BA_Information { get; set; }
    }
}
