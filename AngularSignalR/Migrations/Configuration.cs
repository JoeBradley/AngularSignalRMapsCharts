namespace AngularSignalR.Migrations
{
    using AngularSignalR.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AngularSignalR.DAL.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            // Required for MySQL migration!!!
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }

        protected override void Seed(AngularSignalR.DAL.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            var Categories = new List<Category> { 
                new Category { Title = "Mammal",  },
                new Category { Title = "Marsupial" },
                new Category { Title = "Bird" }
            
            };
            Categories.ForEach(c => context.Categories.AddOrUpdate(e => e.Title, c));
            context.SaveChanges();

            var Stores = new List<Store> { 
                new Store { Name = "Berlin", },
                new Store { Name = "Dresden" },
                new Store { Name = "Frankfurt" }
            };
            Stores.ForEach(s => context.Stores.AddOrUpdate(e => e.Name, s));
            context.SaveChanges();

            var Products = new List<Product> {
                new Product { Name = "Lion", ImageUrl ="/web/images/missing-image.jpg", Price = 10.50d },
                new Product { Name = "Wombat", ImageUrl ="/web/images/missing-image.jpg", Price = 15.85d },
                new Product { Name = "Squirrel", ImageUrl ="/web/images/missing-image.jpg", Price = 32.65d }
            };
            Products.ForEach(p => context.Products.AddOrUpdate(s => s.Name, p));
            context.SaveChanges();

            Categories.ForEach(c => c.Products = Products);
            context.SaveChanges();

            Stores.ForEach(c => c.Products = Products);
            context.SaveChanges();


            
        }
    }
}
