namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newEdition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdvertismentImages", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Comments", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Complaints", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Favorites", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Likes", "AdvertismentId", "dbo.Advertisments");
            DropIndex("dbo.AdvertismentImages", new[] { "AdvertismentId" });
            DropIndex("dbo.Comments", new[] { "AdvertismentId" });
            DropIndex("dbo.Complaints", new[] { "AdvertismentId" });
            DropIndex("dbo.Favorites", new[] { "AdvertismentId" });
            DropIndex("dbo.Likes", new[] { "AdvertismentId" });
            DropPrimaryKey("dbo.Advertisments");
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        StackTrace = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.AdvertismentImages", "AdvertismentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Advertisments", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Comments", "AdvertismentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Complaints", "AdvertismentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Favorites", "AdvertismentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Likes", "AdvertismentId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Advertisments", "Id");
            CreateIndex("dbo.AdvertismentImages", "AdvertismentId");
            CreateIndex("dbo.Comments", "AdvertismentId");
            CreateIndex("dbo.Complaints", "AdvertismentId");
            CreateIndex("dbo.Favorites", "AdvertismentId");
            CreateIndex("dbo.Likes", "AdvertismentId");
            AddForeignKey("dbo.AdvertismentImages", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Complaints", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Favorites", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Likes", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            CreateStoredProcedure(
                "dbo.AdvertismentImage_Insert",
                p => new
                    {
                        IsMainImage = p.Boolean(),
                        ImageUrl = p.String(maxLength: 500),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[AdvertismentImages]([IsMainImage], [ImageUrl], [AdvertismentId])
                      VALUES (@IsMainImage, @ImageUrl, @AdvertismentId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AdvertismentImages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AdvertismentImages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertismentImage_Update",
                p => new
                    {
                        Id = p.Int(),
                        IsMainImage = p.Boolean(),
                        ImageUrl = p.String(maxLength: 500),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[AdvertismentImages]
                      SET [IsMainImage] = @IsMainImage, [ImageUrl] = @ImageUrl, [AdvertismentId] = @AdvertismentId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertismentImage_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[AdvertismentImages]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Advertisment_Insert",
                p => new
                    {
                        IsPaided = p.Boolean(),
                        Description = p.String(),
                        NumberOfViews = p.Int(),
                        NumberOfLikes = p.Int(),
                        PaidEdPrice = p.Decimal(precision: 18, scale: 2),
                        IsArchieved = p.Boolean(),
                        CreateDate = p.DateTime(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        CategoryId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentPriceId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Advertisments]([IsPaided], [Description], [NumberOfViews], [NumberOfLikes], [PaidEdPrice], [IsArchieved], [CreateDate], [StartDate], [EndDate], [CategoryId], [ApplicationUserId], [AdvertismentPriceId])
                      VALUES (@IsPaided, @Description, @NumberOfViews, @NumberOfLikes, @PaidEdPrice, @IsArchieved, @CreateDate, @StartDate, @EndDate, @CategoryId, @ApplicationUserId, @AdvertismentPriceId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Advertisments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Advertisments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Advertisment_Update",
                p => new
                    {
                        Id = p.Int(),
                        IsPaided = p.Boolean(),
                        Description = p.String(),
                        NumberOfViews = p.Int(),
                        NumberOfLikes = p.Int(),
                        PaidEdPrice = p.Decimal(precision: 18, scale: 2),
                        IsArchieved = p.Boolean(),
                        CreateDate = p.DateTime(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        CategoryId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentPriceId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Advertisments]
                      SET [IsPaided] = @IsPaided, [Description] = @Description, [NumberOfViews] = @NumberOfViews, [NumberOfLikes] = @NumberOfLikes, [PaidEdPrice] = @PaidEdPrice, [IsArchieved] = @IsArchieved, [CreateDate] = @CreateDate, [StartDate] = @StartDate, [EndDate] = @EndDate, [CategoryId] = @CategoryId, [ApplicationUserId] = @ApplicationUserId, [AdvertismentPriceId] = @AdvertismentPriceId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Advertisment_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Advertisments]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertismentPrice_Insert",
                p => new
                    {
                        Period = p.Int(),
                        Price = p.Decimal(precision: 18, scale: 2),
                    },
                body:
                    @"INSERT [dbo].[AdvertismentPrices]([Period], [Price])
                      VALUES (@Period, @Price)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AdvertismentPrices]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AdvertismentPrices] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertismentPrice_Update",
                p => new
                    {
                        Id = p.Int(),
                        Period = p.Int(),
                        Price = p.Decimal(precision: 18, scale: 2),
                    },
                body:
                    @"UPDATE [dbo].[AdvertismentPrices]
                      SET [Period] = @Period, [Price] = @Price
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertismentPrice_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[AdvertismentPrices]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Comment_Insert",
                p => new
                    {
                        Message = p.String(),
                        CreateDate = p.DateTime(),
                        AdvertismentId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Comments]([Message], [CreateDate], [AdvertismentId], [ApplicationUserId])
                      VALUES (@Message, @CreateDate, @AdvertismentId, @ApplicationUserId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Comments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Comments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Comment_Update",
                p => new
                    {
                        Id = p.Int(),
                        Message = p.String(),
                        CreateDate = p.DateTime(),
                        AdvertismentId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Comments]
                      SET [Message] = @Message, [CreateDate] = @CreateDate, [AdvertismentId] = @AdvertismentId, [ApplicationUserId] = @ApplicationUserId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Comment_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Comments]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Complaint_Insert",
                p => new
                    {
                        Message = p.String(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Complaints]([Message], [ApplicationUserId], [AdvertismentId])
                      VALUES (@Message, @ApplicationUserId, @AdvertismentId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Complaints]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Complaints] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Complaint_Update",
                p => new
                    {
                        Id = p.Int(),
                        Message = p.String(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Complaints]
                      SET [Message] = @Message, [ApplicationUserId] = @ApplicationUserId, [AdvertismentId] = @AdvertismentId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Complaint_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Complaints]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Favorite_Insert",
                p => new
                    {
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Favorites]([ApplicationUserId], [AdvertismentId])
                      VALUES (@ApplicationUserId, @AdvertismentId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Favorites]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Favorites] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Favorite_Update",
                p => new
                    {
                        Id = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Favorites]
                      SET [ApplicationUserId] = @ApplicationUserId, [AdvertismentId] = @AdvertismentId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Favorite_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Favorites]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Like_Insert",
                p => new
                    {
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Likes]([ApplicationUserId], [AdvertismentId])
                      VALUES (@ApplicationUserId, @AdvertismentId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Likes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Likes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Like_Update",
                p => new
                    {
                        Id = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Likes]
                      SET [ApplicationUserId] = @ApplicationUserId, [AdvertismentId] = @AdvertismentId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Like_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Likes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.BankAccount_Insert",
                p => new
                    {
                        BankNumber = p.String(maxLength: 200),
                        BankName = p.String(maxLength: 100),
                    },
                body:
                    @"INSERT [dbo].[BankAccounts]([BankNumber], [BankName])
                      VALUES (@BankNumber, @BankName)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[BankAccounts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[BankAccounts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.BankAccount_Update",
                p => new
                    {
                        Id = p.Int(),
                        BankNumber = p.String(maxLength: 200),
                        BankName = p.String(maxLength: 100),
                    },
                body:
                    @"UPDATE [dbo].[BankAccounts]
                      SET [BankNumber] = @BankNumber, [BankName] = @BankName
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.BankAccount_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[BankAccounts]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactInformation_Insert",
                p => new
                    {
                        Contact = p.String(maxLength: 200),
                        ContactTypeId = p.Int(),
                        IconName = p.String(maxLength: 70),
                    },
                body:
                    @"INSERT [dbo].[ContactInformations]([Contact], [ContactTypeId], [IconName])
                      VALUES (@Contact, @ContactTypeId, @IconName)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactInformations]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactInformations] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ContactInformation_Update",
                p => new
                    {
                        Id = p.Int(),
                        Contact = p.String(maxLength: 200),
                        ContactTypeId = p.Int(),
                        IconName = p.String(maxLength: 70),
                    },
                body:
                    @"UPDATE [dbo].[ContactInformations]
                      SET [Contact] = @Contact, [ContactTypeId] = @ContactTypeId, [IconName] = @IconName
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactInformation_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ContactInformations]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactType_Insert",
                p => new
                    {
                        Type = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[ContactTypes]([Type])
                      VALUES (@Type)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ContactType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Type = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[ContactTypes]
                      SET [Type] = @Type
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ContactTypes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Error_Insert",
                p => new
                    {
                        Message = p.String(),
                        StackTrace = p.String(),
                        DateCreated = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Errors]([Message], [StackTrace], [DateCreated])
                      VALUES (@Message, @StackTrace, @DateCreated)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Errors]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Errors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Error_Update",
                p => new
                    {
                        Id = p.Int(),
                        Message = p.String(),
                        StackTrace = p.String(),
                        DateCreated = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Errors]
                      SET [Message] = @Message, [StackTrace] = @StackTrace, [DateCreated] = @DateCreated
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Error_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Errors]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Error_Delete");
            DropStoredProcedure("dbo.Error_Update");
            DropStoredProcedure("dbo.Error_Insert");
            DropStoredProcedure("dbo.ContactType_Delete");
            DropStoredProcedure("dbo.ContactType_Update");
            DropStoredProcedure("dbo.ContactType_Insert");
            DropStoredProcedure("dbo.ContactInformation_Delete");
            DropStoredProcedure("dbo.ContactInformation_Update");
            DropStoredProcedure("dbo.ContactInformation_Insert");
            DropStoredProcedure("dbo.BankAccount_Delete");
            DropStoredProcedure("dbo.BankAccount_Update");
            DropStoredProcedure("dbo.BankAccount_Insert");
            DropStoredProcedure("dbo.Like_Delete");
            DropStoredProcedure("dbo.Like_Update");
            DropStoredProcedure("dbo.Like_Insert");
            DropStoredProcedure("dbo.Favorite_Delete");
            DropStoredProcedure("dbo.Favorite_Update");
            DropStoredProcedure("dbo.Favorite_Insert");
            DropStoredProcedure("dbo.Complaint_Delete");
            DropStoredProcedure("dbo.Complaint_Update");
            DropStoredProcedure("dbo.Complaint_Insert");
            DropStoredProcedure("dbo.Comment_Delete");
            DropStoredProcedure("dbo.Comment_Update");
            DropStoredProcedure("dbo.Comment_Insert");
            DropStoredProcedure("dbo.AdvertismentPrice_Delete");
            DropStoredProcedure("dbo.AdvertismentPrice_Update");
            DropStoredProcedure("dbo.AdvertismentPrice_Insert");
            DropStoredProcedure("dbo.Advertisment_Delete");
            DropStoredProcedure("dbo.Advertisment_Update");
            DropStoredProcedure("dbo.Advertisment_Insert");
            DropStoredProcedure("dbo.AdvertismentImage_Delete");
            DropStoredProcedure("dbo.AdvertismentImage_Update");
            DropStoredProcedure("dbo.AdvertismentImage_Insert");
            DropForeignKey("dbo.Likes", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Favorites", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Complaints", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.Comments", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.AdvertismentImages", "AdvertismentId", "dbo.Advertisments");
            DropIndex("dbo.Likes", new[] { "AdvertismentId" });
            DropIndex("dbo.Favorites", new[] { "AdvertismentId" });
            DropIndex("dbo.Complaints", new[] { "AdvertismentId" });
            DropIndex("dbo.Comments", new[] { "AdvertismentId" });
            DropIndex("dbo.AdvertismentImages", new[] { "AdvertismentId" });
            DropPrimaryKey("dbo.Advertisments");
            AlterColumn("dbo.Likes", "AdvertismentId", c => c.Long(nullable: false));
            AlterColumn("dbo.Favorites", "AdvertismentId", c => c.Long(nullable: false));
            AlterColumn("dbo.Complaints", "AdvertismentId", c => c.Long(nullable: false));
            AlterColumn("dbo.Comments", "AdvertismentId", c => c.Long(nullable: false));
            AlterColumn("dbo.Advertisments", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.AdvertismentImages", "AdvertismentId", c => c.Long(nullable: false));
            DropTable("dbo.Errors");
            AddPrimaryKey("dbo.Advertisments", "Id");
            CreateIndex("dbo.Likes", "AdvertismentId");
            CreateIndex("dbo.Favorites", "AdvertismentId");
            CreateIndex("dbo.Complaints", "AdvertismentId");
            CreateIndex("dbo.Comments", "AdvertismentId");
            CreateIndex("dbo.AdvertismentImages", "AdvertismentId");
            AddForeignKey("dbo.Likes", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Favorites", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Complaints", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AdvertismentImages", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
        }
    }
}
