namespace AngularSignalRMapsCharts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Url = c.String(unicode: false),
                        Address = c.String(unicode: false),
                        PostCode = c.Int(nullable: false),
                        DateCompleted = c.DateTime(nullable: false),
                        DateCompletedTicks = c.Double(nullable: false),
                        SEO = c.Double(nullable: false),
                        Web = c.Double(nullable: false),
                        Directories = c.Double(nullable: false),
                        Social = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        Data = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventHistory");
            DropTable("dbo.Company");
        }
    }
}
