namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTable_CarInsurance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarInsurance", "UniqueKey", c => c.String(maxLength: 50));
            AddColumn("dbo.CarInsurance", "EOrderCode", c => c.String(maxLength: 50));
            AddColumn("dbo.CarInsurance", "Status", c => c.String(maxLength: 5));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarInsurance", "Status");
            DropColumn("dbo.CarInsurance", "EOrderCode");
            DropColumn("dbo.CarInsurance", "UniqueKey");
        }
    }
}
