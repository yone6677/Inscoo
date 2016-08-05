namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableHealth_AddColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HealthOrderMaster", "DateTicks", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HealthOrderMaster", "DateTicks");
        }
    }
}
