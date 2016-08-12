namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class health1431 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HealthOrderMaster", "ServicePeriod", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.HealthOrderDetail", "Ticks", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HealthOrderDetail", "Ticks");
            DropColumn("dbo.HealthOrderMaster", "ServicePeriod");
        }
    }
}
