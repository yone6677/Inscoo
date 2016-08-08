namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlertTableHealthMaster : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HealthOrderMaster", "Count", c => c.Int(nullable: false));
            AlterColumn("dbo.HealthOrderMaster", "BaokuOrderCode", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HealthOrderMaster", "BaokuOrderCode", c => c.String());
            DropColumn("dbo.HealthOrderMaster", "Count");
        }
    }
}
