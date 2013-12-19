namespace AngularSignalRMapsCharts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mc2Mig : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Company", "DateCompletedTicks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Company", "DateCompletedTicks", c => c.Double(nullable: false));
        }
    }
}
