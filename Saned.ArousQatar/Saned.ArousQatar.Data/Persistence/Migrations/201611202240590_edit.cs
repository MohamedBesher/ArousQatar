namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(),
                        Name = c.String(),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Host = c.String(),
                        FromEmail = c.String(),
                        Password = c.String(),
                        SubjectAr = c.String(),
                        SubjectEn = c.String(),
                        MessageBodyAr = c.String(),
                        MessageBodyEn = c.String(),
                        EmailSettingType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(),
                        ClientId = c.String(),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "ConfirmedEmailToken", c => c.String());
            AddColumn("dbo.AspNetUsers", "ResetPasswordlToken", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsDeleted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsDeleted");
            DropColumn("dbo.AspNetUsers", "ResetPasswordlToken");
            DropColumn("dbo.AspNetUsers", "ConfirmedEmailToken");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.EmailSettings");
            DropTable("dbo.Clients");
        }
    }
}
