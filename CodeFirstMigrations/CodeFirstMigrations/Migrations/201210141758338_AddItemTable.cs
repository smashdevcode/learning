namespace CodeFirstMigrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemID = c.Int(nullable: false, identity: true),
						ItemNumber = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ItemID);
			CreateIndex("dbo.Items", "ItemNumber", true);

			// need to populate the Items table
			Sql(@"
				insert Items (ItemNumber)
				select distinct Item
				from OrderItems
				order by Item
			");

            
            AddColumn("dbo.OrderItems", "ItemID", c => c.Int(nullable: false));

			// update the OrderItems.ItemID column
			Sql(@"
				update oi
				set oi.ItemID = i.ItemID
				from OrderItems oi
				join Items i on i.ItemNumber = oi.Item
			");			
			
			AddForeignKey("dbo.OrderItems", "ItemID", "dbo.Items", "ItemID", cascadeDelete: true);
            CreateIndex("dbo.OrderItems", "ItemID");

            DropColumn("dbo.OrderItems", "Item");
        }
        
        public override void Down()
        {
			AddColumn("dbo.OrderItems", "Item", c => c.String(nullable: false, maxLength: 100));

			// update the OrderItems.Item column
			Sql(@"
				update oi
				set oi.Item = i.ItemNumber
				from OrderItems oi
				join Items i on i.ItemID = oi.ItemID
			");

            DropIndex("dbo.OrderItems", new[] { "ItemID" });
            DropForeignKey("dbo.OrderItems", "ItemID", "dbo.Items");
            DropColumn("dbo.OrderItems", "ItemID");
			DropIndex("dbo.Items", new[] { "ItemNumber" });
            DropTable("dbo.Items");
        }
    }
}
