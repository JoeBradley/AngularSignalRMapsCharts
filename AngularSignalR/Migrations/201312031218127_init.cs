namespace AngularSignalR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Price = c.Double(nullable: false),
                        Likes = c.Int(nullable: false),
                        ImageUrl = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Store",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "EventHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "StoreProduct",
                c => new
                    {
                        StoreID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StoreID, t.ProductID })
                .ForeignKey("Store", t => t.StoreID, cascadeDelete: true)
                .ForeignKey("Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.StoreID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "ProductCategory",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.CategoryID })
                .ForeignKey("Product", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("Category", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropIndex("ProductCategory", new[] { "CategoryID" });
            DropIndex("ProductCategory", new[] { "ProductID" });
            DropIndex("StoreProduct", new[] { "ProductID" });
            DropIndex("StoreProduct", new[] { "StoreID" });
            DropForeignKey("ProductCategory", "CategoryID", "Category");
            DropForeignKey("ProductCategory", "ProductID", "Product");
            DropForeignKey("StoreProduct", "ProductID", "Product");
            DropForeignKey("StoreProduct", "StoreID", "Store");
            DropTable("ProductCategory");
            DropTable("StoreProduct");
            DropTable("EventHistory");
            DropTable("Store");
            DropTable("Product");
            DropTable("Category");
        }
    }
}
