using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using prject.domain.Models;
using project.domain.DTO;
using project.domain.Models;
using System;

namespace project.persistence.Context
{
    public class CASContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public CASContext(DbContextOptions<CASContext> options)
            : base(options)
        {
         
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<TemplateCategory>().HasData(new TemplateCategory() { Name = "DefaultTemplate", Caption = "DefaultTemplate" });
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductUser> ProductUser { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceUser> ServiceUser { get; set; }
    }
}

