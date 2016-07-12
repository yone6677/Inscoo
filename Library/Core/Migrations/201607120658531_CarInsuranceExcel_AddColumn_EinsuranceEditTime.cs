namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarInsuranceExcel_AddColumn_EinsuranceEditTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseFile", "EinsuranceEditTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseFile", "EinsuranceEditTime");
        }
    }
}
