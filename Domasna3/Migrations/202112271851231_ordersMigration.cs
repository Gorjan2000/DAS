namespace Domasna3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ordersMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FoodModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Checked = c.Boolean(nullable: false),
                        UserName = c.String(),
                        Order_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_ID)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        Local = c.String(),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FoodModels", "Order_ID", "dbo.Orders");
            DropIndex("dbo.FoodModels", new[] { "Order_ID" });
            DropTable("dbo.Orders");
            DropTable("dbo.FoodModels");
        }
    }
}
