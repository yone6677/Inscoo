namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderEmpTemp1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderEmpTemp", "BuyType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderEmpTemp", "BuyType");
        }
    }
}
