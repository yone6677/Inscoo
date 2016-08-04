namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderInitialNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "InitialNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "InitialNumber");
        }
    }
}
