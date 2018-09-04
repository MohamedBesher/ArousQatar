namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompleteConfigurations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Advertisments", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Advertisments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Comments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Complaints", new[] { "ApplicationUserId" });
            DropIndex("dbo.Favorites", new[] { "ApplicationUserId" });
            DropIndex("dbo.Likes", new[] { "ApplicationUserId" });
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankNumber = c.String(nullable: false, maxLength: 200),
                        BankName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contact = c.String(nullable: false, maxLength: 200),
                        ContactTypeId = c.Int(nullable: false),
                        IconName = c.String(nullable: false, maxLength: 70),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactTypes", t => t.ContactTypeId, cascadeDelete: true)
                .Index(t => t.ContactTypeId);
            
            CreateTable(
                "dbo.ContactTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Advertisments", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AdvertismentImages", "ImageUrl", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Comments", "Message", c => c.String(nullable: false));
            AlterColumn("dbo.Comments", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Complaints", "Message", c => c.String(nullable: false));
            AlterColumn("dbo.Complaints", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Favorites", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Likes", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Advertisments", "ApplicationUserId");
            CreateIndex("dbo.Comments", "ApplicationUserId");
            CreateIndex("dbo.Complaints", "ApplicationUserId");
            CreateIndex("dbo.Favorites", "ApplicationUserId");
            CreateIndex("dbo.Likes", "ApplicationUserId");
            AddForeignKey("dbo.Advertisments", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advertisments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ContactInformations", "ContactTypeId", "dbo.ContactTypes");
            DropIndex("dbo.ContactInformations", new[] { "ContactTypeId" });
            DropIndex("dbo.Likes", new[] { "ApplicationUserId" });
            DropIndex("dbo.Favorites", new[] { "ApplicationUserId" });
            DropIndex("dbo.Complaints", new[] { "ApplicationUserId" });
            DropIndex("dbo.Comments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Advertisments", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Likes", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Favorites", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Complaints", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Complaints", "Message", c => c.String());
            AlterColumn("dbo.Comments", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "Message", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String());
            AlterColumn("dbo.AdvertismentImages", "ImageUrl", c => c.String());
            AlterColumn("dbo.Advertisments", "ApplicationUserId", c => c.String(maxLength: 128));
            DropTable("dbo.ContactTypes");
            DropTable("dbo.ContactInformations");
            DropTable("dbo.BankAccounts");
            CreateIndex("dbo.Likes", "ApplicationUserId");
            CreateIndex("dbo.Favorites", "ApplicationUserId");
            CreateIndex("dbo.Complaints", "ApplicationUserId");
            CreateIndex("dbo.Comments", "ApplicationUserId");
            CreateIndex("dbo.Advertisments", "ApplicationUserId");
            AddForeignKey("dbo.Advertisments", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
