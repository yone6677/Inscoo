namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableHealthCheckProduct_AddColumnCommision : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HealthCheckProduct", "CommissionRatioBD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.HealthCheckProduct", "CommissionRatioChannel", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.HealthCheckProduct", "CommissionRatioHR", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.HealthCheckProduct", "CommissionRatioOther", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.HealthCheckProduct", "CommissionRatio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HealthCheckProduct", "CommissionRatio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.HealthCheckProduct", "CommissionRatioOther");
            DropColumn("dbo.HealthCheckProduct", "CommissionRatioHR");
            DropColumn("dbo.HealthCheckProduct", "CommissionRatioChannel");
            DropColumn("dbo.HealthCheckProduct", "CommissionRatioBD");
        }
    }
}
