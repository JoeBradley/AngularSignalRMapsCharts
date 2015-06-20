namespace LiveLog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EventLog", "Timeago");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventLog", "Timeago", c => c.String());
        }
    }
}
