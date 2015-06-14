using AngularSignalRMapsCharts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AngularSignalRMapsCharts.DAL
{
    public class DataContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<EventLog> Events { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();          
        }
    }
}