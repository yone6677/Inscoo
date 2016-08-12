namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableCreateAccountCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreateAccountCode",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountEncryCode = c.String(nullable: false, maxLength: 10),
                        IsUsed = c.Boolean(nullable: false),
                        EncryCreateID = c.String(maxLength: 50),
                        EncryBeginDate = c.DateTime(),
                        EncryEndDate = c.DateTime(),
                        EncryRoleName = c.String(maxLength: 50),
                        EncryCompanyName = c.String(maxLength: 150),
                        EncryCommissionMethod = c.String(maxLength: 100),
                        EncryInsurance = c.String(maxLength: 200),
                        EncrySeries = c.String(maxLength: 200),
                        EncryFanBao = c.Boolean(nullable: false),
                        EncryTiYong = c.Boolean(nullable: false),
                        EncryRebate = c.Int(nullable: false),
                        EncryMemo = c.String(maxLength: 200),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CreateAccountCode");
        }
    }
}
