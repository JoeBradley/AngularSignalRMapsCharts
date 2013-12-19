namespace AngularSignalRMapsCharts.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AngularSignalRMapsCharts.DAL.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            // Required for MySQL migration!!!
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());

        }

        protected override void Seed(AngularSignalRMapsCharts.DAL.DataContext context)
        {            
        }
    }
}
