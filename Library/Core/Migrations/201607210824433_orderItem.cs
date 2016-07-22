namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItem", "pid", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItem", "pid");
        }
    }
}
