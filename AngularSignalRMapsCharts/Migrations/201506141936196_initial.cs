namespace AngularSignalRMapsCharts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        Address = c.String(),
                        PostCode = c.Int(nullable: false),
                        DateCompleted = c.DateTime(nullable: false),
                        SEO = c.Double(nullable: false),
                        Web = c.Double(nullable: false),
                        Directories = c.Double(nullable: false),
                        Social = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Details = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventLog");
            DropTable("dbo.Company");
        }
    }
}
