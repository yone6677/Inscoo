namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tpa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderEmployees", "Transfer2TPA", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderEmployees", "Transfer2TPA");
        }
    }
}
