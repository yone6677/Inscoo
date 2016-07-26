namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class claimandfileApi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.claimFileFromWechat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        claim_Id = c.Int(nullable: false),
                        Author = c.String(maxLength: 128),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimFromWechat", t => t.claim_Id, cascadeDelete: true)
                .Index(t => t.claim_Id);
            
            CreateTable(
                "dbo.ClaimFromWechat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseId = c.String(nullable: false, maxLength: 64),
                        CompanyName = c.String(maxLength: 64),
                        ProposerName = c.String(maxLength: 32),
                        ProposerSex = c.String(maxLength: 8),
                        ProposerBirthday = c.DateTime(precision: 7, storeType: "datetime2"),
                        ProposerIdType = c.String(maxLength: 16),
                        ProposerIdNumber = c.String(maxLength: 32),
                        ProposerPhone = c.String(maxLength: 16),
                        ProposerEmail = c.String(maxLength: 128),
                        RecipientName = c.String(maxLength: 32),
                        RecipientSex = c.String(maxLength: 8),
                        RecipientBirthday = c.DateTime(precision: 7, storeType: "datetime2"),
                        RecipientIdType = c.String(maxLength: 16),
                        RecipientIdNumber = c.String(maxLength: 32),
                        RecipientPhone = c.String(maxLength: 16),
                        RecipientEmail = c.String(maxLength: 16),
                        Describe = c.String(maxLength: 512),
                        FullImage = c.String(maxLength: 512),
                        openid = c.String(maxLength: 256),
                        TransformToTPA = c.Boolean(),
                        Author = c.String(maxLength: 128),
                        CreateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BaseFile", "Domain", c => c.String(maxLength: 256));
            AddColumn("dbo.BaseFile", "FromAPi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.claimFileFromWechat", "claim_Id", "dbo.ClaimFromWechat");
            DropIndex("dbo.claimFileFromWechat", new[] { "claim_Id" });
            DropColumn("dbo.BaseFile", "FromAPi");
            DropColumn("dbo.BaseFile", "Domain");
            DropTable("dbo.ClaimFromWechat");
            DropTable("dbo.claimFileFromWechat");
        }
    }
}
