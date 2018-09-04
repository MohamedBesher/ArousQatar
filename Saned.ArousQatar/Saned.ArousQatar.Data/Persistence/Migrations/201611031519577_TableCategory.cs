namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsArchieved = c.Boolean(),
                        IconName = c.String(nullable: false, maxLength: 50),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Advertisments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsPaided = c.Boolean(nullable: false),
                        Description = c.String(),
                        NumberOfViews = c.Int(nullable: false),
                        NumberOfLikes = c.Int(nullable: false),
                        PaidEdPrice = c.Decimal(precision: 18, scale: 2),
                        IsArchieved = c.Boolean(),
                        CreateDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        AdvertismentPriceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdvertismentPrices", t => t.AdvertismentPriceId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.AdvertismentPriceId);
            
            CreateTable(
                "dbo.AdvertismentImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsMainImage = c.Boolean(),
                        ImageUrl = c.String(),
                        AdvertismentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisments", t => t.AdvertismentId, cascadeDelete: true)
                .Index(t => t.AdvertismentId);
            
            CreateTable(
                "dbo.AdvertismentPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Period = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        AdvertismentId = c.Long(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisments", t => t.AdvertismentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.AdvertismentId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Complaints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                        AdvertismentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisments", t => t.AdvertismentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.AdvertismentId);
            
            CreateTable(
                "dbo.Favorites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        AdvertismentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisments", t => t.AdvertismentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.AdvertismentId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        AdvertismentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisments", t => t.AdvertismentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.AdvertismentId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateStoredProcedure(
                "dbo.Category_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 150),
                        IsArchieved = p.Boolean(),
                        IconName = p.String(maxLength: 50),
                        ImageUrl = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Categories]([Name], [IsArchieved], [IconName], [ImageUrl])
                      VALUES (@Name, @IsArchieved, @IconName, @ImageUrl)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Categories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Categories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 150),
                        IsArchieved = p.Boolean(),
                        IconName = p.String(maxLength: 50),
                        ImageUrl = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Categories]
                      SET [Name] = @Name, [IsArchieved] = @IsArchieved, [IconName] = @IconName, [ImageUrl] = @ImageUrl
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Categories]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Category_Delete");
            DropStoredProcedure("dbo.Category_Update");
            DropStoredProcedure("dbo.Category_Insert");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Likes", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Favorites", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Favorites", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Complaints", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Complaints", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Comments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Advertisments", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Advertisments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Advertisments", "AdvertismentPriceId", "dbo.AdvertismentPrices");
            DropForeignKey("dbo.AdvertismentImages", "AdvertismentId", "dbo.Advertisments");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Likes", new[] { "AdvertismentId" });
            DropIndex("dbo.Likes", new[] { "ApplicationUserId" });
            DropIndex("dbo.Favorites", new[] { "AdvertismentId" });
            DropIndex("dbo.Favorites", new[] { "ApplicationUserId" });
            DropIndex("dbo.Complaints", new[] { "AdvertismentId" });
            DropIndex("dbo.Complaints", new[] { "ApplicationUserId" });
            DropIndex("dbo.Comments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Comments", new[] { "AdvertismentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AdvertismentImages", new[] { "AdvertismentId" });
            DropIndex("dbo.Advertisments", new[] { "AdvertismentPriceId" });
            DropIndex("dbo.Advertisments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Advertisments", new[] { "CategoryId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Likes");
            DropTable("dbo.Favorites");
            DropTable("dbo.Complaints");
            DropTable("dbo.Comments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AdvertismentPrices");
            DropTable("dbo.AdvertismentImages");
            DropTable("dbo.Advertisments");
            DropTable("dbo.Categories");
        }
    }
}
