namespace LiveLog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventLog", "Timeago", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventLog", "Timeago");
        }
    }
}
