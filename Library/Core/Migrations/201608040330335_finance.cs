namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CashFlowDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Receivable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualCollected = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Payable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RealPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cId = c.Int(nullable: false),
                        Author = c.String(nullable: false, maxLength: 256),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CashFlow", t => t.cId, cascadeDelete: true)
                .Index(t => t.cId);
            
            CreateTable(
                "dbo.CashFlow",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OId = c.Int(nullable: false),
                        OType = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Difference = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Author = c.String(nullable: false, maxLength: 256),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CashFlowDetails", "cId", "dbo.CashFlow");
            DropIndex("dbo.CashFlowDetails", new[] { "cId" });
            DropTable("dbo.CashFlow");
            DropTable("dbo.CashFlowDetails");
        }
    }
}
