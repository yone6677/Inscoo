namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarInsuranceExcel_AddColumn_EinsuranceNameEinsurancePath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseFile", "EinsurancePath", c => c.String());
            AddColumn("dbo.BaseFile", "EinsuranceUrl", c => c.String());
            AddColumn("dbo.BaseFile", "EinsuranceName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseFile", "EinsuranceName");
            DropColumn("dbo.BaseFile", "EinsuranceUrl");
            DropColumn("dbo.BaseFile", "EinsurancePath");
        }
    }
}
