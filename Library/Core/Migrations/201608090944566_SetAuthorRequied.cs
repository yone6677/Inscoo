namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetAuthorRequied : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HealthOrderDetail", "Author", c => c.String(nullable: false));
            AlterColumn("dbo.OrderEmployees", "Author", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderEmployees", "Author", c => c.String(maxLength: 64));
            AlterColumn("dbo.HealthOrderDetail", "Author", c => c.String());
        }
    }
}
