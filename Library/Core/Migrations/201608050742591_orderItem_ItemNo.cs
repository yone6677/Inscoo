namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderItem_ItemNo : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.WZHumanMaster",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Account = c.String(nullable: false, maxLength: 100),
            //            CompanyName = c.String(nullable: false, maxLength: 100),
            //            InsuranceBeginTime = c.DateTime(),
            //            InsuranceEndTime = c.DateTime(),
            //            Author = c.String(),
            //            CreateTime = c.DateTime(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.OrderItem", "ItemNo", c => c.String(maxLength: 10));
            //AlterColumn("dbo.GenericAttribute", "Value", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.GenericAttribute", "Value", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.OrderItem", "ItemNo");
            DropColumn("dbo.Orders", "CompanyId");
            //DropTable("dbo.WZHumanMaster");
        }
    }
}
