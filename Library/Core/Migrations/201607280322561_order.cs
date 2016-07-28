namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ProdWithdraw", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderItem", "ProdTimeLimit", c => c.String(maxLength: 10));
            AddColumn("dbo.OrderItem", "ProdWithdraw", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderItem", "ProdAbatement", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItem", "ProdAbatement");
            DropColumn("dbo.OrderItem", "ProdWithdraw");
            DropColumn("dbo.OrderItem", "ProdTimeLimit");
            DropColumn("dbo.Orders", "ProdWithdraw");
        }
    }
}
