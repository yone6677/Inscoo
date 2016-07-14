namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order_orderNum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderNum", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderNum");
        }
    }
}
