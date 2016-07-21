namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTablesAboutHealth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HealthOrderMaster", "PaymentNoticePdf", c => c.String());
            AddColumn("dbo.HealthCheckProduct", "ProductTypeName", c => c.String());
            AddColumn("dbo.HealthCheckProduct", "ProductOrder", c => c.Int(nullable: false));
            AddColumn("dbo.HealthCheckProduct", "CostPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.HealthCheckProduct", "CommissionRatio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HealthOrderMaster", "CommissionRatio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HealthOrderMaster", "Status", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.FileInfo", "CarInsuranceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FileInfo", "CarInsuranceId", c => c.Int(nullable: false));
            AlterColumn("dbo.HealthOrderMaster", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.HealthOrderMaster", "CommissionRatio", c => c.Int(nullable: false));
            DropColumn("dbo.HealthCheckProduct", "CommissionRatio");
            DropColumn("dbo.HealthCheckProduct", "CostPrice");
            DropColumn("dbo.HealthCheckProduct", "ProductOrder");
            DropColumn("dbo.HealthCheckProduct", "ProductTypeName");
            DropColumn("dbo.HealthOrderMaster", "PaymentNoticePdf");
        }
    }
}
