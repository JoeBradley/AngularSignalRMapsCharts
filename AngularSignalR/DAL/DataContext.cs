using AngularSignalR.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AngularSignalR.DAL
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<EventHistory> Events { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Products to Caregories
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .Map(t => t.MapLeftKey("ProductID")
                    .MapRightKey("CategoryID")
                    .ToTable("ProductCategory"));
                

            // Stores to Products
            modelBuilder.Entity<Store>()
                .HasMany(s => s.Products)
                .WithMany(p => p.Stores)
                .Map(t => t.MapLeftKey("StoreID")
                    .MapRightKey("ProductID")
                    .ToTable("StoreProduct"));
        }
    }
}