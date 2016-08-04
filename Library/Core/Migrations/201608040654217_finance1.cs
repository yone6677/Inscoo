namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finance1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CashFlow", "ChangeTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.CashFlow", "Changer", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CashFlow", "Changer");
            DropColumn("dbo.CashFlow", "ChangeTime");
        }
    }
}
