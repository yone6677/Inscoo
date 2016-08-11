namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnPsffileAtCar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarInsurance", "PdfFileName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarInsurance", "PdfFileName");
        }
    }
}
