namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class healthfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HealthFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FId = c.Int(nullable: false),
                        HId = c.Int(nullable: false),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                        order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HealthOrderMaster", t => t.HId, cascadeDelete: true)
                .ForeignKey("dbo.HealthOrderMaster", t => t.order_Id)
                .Index(t => t.HId)
                .Index(t => t.order_Id);
            
            AddColumn("dbo.HealthOrderMaster", "Expire", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HealthFile", "order_Id", "dbo.HealthOrderMaster");
            DropForeignKey("dbo.HealthFile", "HId", "dbo.HealthOrderMaster");
            DropIndex("dbo.HealthFile", new[] { "order_Id" });
            DropIndex("dbo.HealthFile", new[] { "HId" });
            DropColumn("dbo.HealthOrderMaster", "Expire");
            DropTable("dbo.HealthFile");
        }
    }
}
