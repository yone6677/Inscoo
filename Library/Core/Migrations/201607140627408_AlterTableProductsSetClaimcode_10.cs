namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableProductsSetClaimcode_10 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Orders", "OrderNum", c => c.String());
            AlterColumn("dbo.Products", "ClaimCode", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ClaimCode", c => c.String(maxLength: 5));
            //DropColumn("dbo.Orders", "OrderNum");
        }
    }
}
