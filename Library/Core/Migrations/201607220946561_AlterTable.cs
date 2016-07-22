namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProdTimeLimit", c => c.String(maxLength: 10));
            AddColumn("dbo.Products", "ProdWithdraw", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "ProdAbatement", c => c.String(maxLength: 50));
            AddColumn("dbo.Products", "ProdQuoteType", c => c.String(maxLength: 50));
            AddColumn("dbo.Products", "ProdPayType", c => c.String(maxLength: 50));
            AddColumn("dbo.Products", "ProdCreateType", c => c.String(maxLength: 150));
            AlterColumn("dbo.HealthOrderMaster", "Author", c => c.String(nullable: false));
            AlterColumn("dbo.HealthCheckProduct", "Author", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "ProdInsuredName", c => c.String(maxLength: 200));
            AlterColumn("dbo.Products", "changer", c => c.String(maxLength: 50));
            AlterColumn("dbo.Products", "ProvisionPath", c => c.String(maxLength: 150));
            AlterColumn("dbo.Products", "Author", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Author", c => c.String());
            AlterColumn("dbo.Products", "ProvisionPath", c => c.String());
            AlterColumn("dbo.Products", "changer", c => c.String());
            AlterColumn("dbo.Products", "ProdInsuredName", c => c.String());
            AlterColumn("dbo.HealthCheckProduct", "Author", c => c.String());
            AlterColumn("dbo.HealthOrderMaster", "Author", c => c.String());
            DropColumn("dbo.Products", "ProdCreateType");
            DropColumn("dbo.Products", "ProdPayType");
            DropColumn("dbo.Products", "ProdQuoteType");
            DropColumn("dbo.Products", "ProdAbatement");
            DropColumn("dbo.Products", "ProdWithdraw");
            DropColumn("dbo.Products", "ProdTimeLimit");
        }
    }
}
