﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArmyTechTask.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ArmyTechTaskEntities : DbContext
    {
        public ArmyTechTaskEntities()
            : base("name=ArmyTechTaskEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<Governorate> Governorates { get; set; }
        public virtual DbSet<Neighborhood> Neighborhoods { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentTeacher> StudentTeachers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
    }
}
