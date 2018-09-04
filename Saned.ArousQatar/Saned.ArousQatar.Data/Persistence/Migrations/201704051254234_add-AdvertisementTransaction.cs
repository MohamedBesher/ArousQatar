namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdvertisementTransaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdvertismentTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentId = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        AdvertismentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisments", t => t.AdvertismentId, cascadeDelete: true)
                .Index(t => t.AdvertismentId);
            
            AddColumn("dbo.Advertisments", "IsExpired", c => c.Boolean(nullable: false));
            CreateStoredProcedure(
                "dbo.AdvertismentTransaction_Insert",
                p => new
                    {
                        PaymentId = p.String(),
                        CreateDate = p.DateTime(),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[AdvertismentTransactions]([PaymentId], [CreateDate], [AdvertismentId])
                      VALUES (@PaymentId, @CreateDate, @AdvertismentId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AdvertismentTransactions]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AdvertismentTransactions] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertismentTransaction_Update",
                p => new
                    {
                        Id = p.Int(),
                        PaymentId = p.String(),
                        CreateDate = p.DateTime(),
                        AdvertismentId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[AdvertismentTransactions]
                      SET [PaymentId] = @PaymentId, [CreateDate] = @CreateDate, [AdvertismentId] = @AdvertismentId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertismentTransaction_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[AdvertismentTransactions]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Advertisment_Insert",
                p => new
                    {
                        IsActive = p.Boolean(),
                        Name = p.String(),
                        IsPaided = p.Boolean(),
                        Description = p.String(),
                        NumberOfViews = p.Int(),
                        NumberOfLikes = p.Int(),
                        PaidEdPrice = p.Decimal(precision: 18, scale: 2),
                        IsArchieved = p.Boolean(),
                        CreateDate = p.DateTime(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        IsExpired = p.Boolean(),
                        CategoryId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentPriceId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Advertisments]([IsActive], [Name], [IsPaided], [Description], [NumberOfViews], [NumberOfLikes], [PaidEdPrice], [IsArchieved], [CreateDate], [StartDate], [EndDate], [IsExpired], [CategoryId], [ApplicationUserId], [AdvertismentPriceId])
                      VALUES (@IsActive, @Name, @IsPaided, @Description, @NumberOfViews, @NumberOfLikes, @PaidEdPrice, @IsArchieved, @CreateDate, @StartDate, @EndDate, @IsExpired, @CategoryId, @ApplicationUserId, @AdvertismentPriceId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Advertisments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Advertisments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Advertisment_Update",
                p => new
                    {
                        Id = p.Int(),
                        IsActive = p.Boolean(),
                        Name = p.String(),
                        IsPaided = p.Boolean(),
                        Description = p.String(),
                        NumberOfViews = p.Int(),
                        NumberOfLikes = p.Int(),
                        PaidEdPrice = p.Decimal(precision: 18, scale: 2),
                        IsArchieved = p.Boolean(),
                        CreateDate = p.DateTime(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                        IsExpired = p.Boolean(),
                        CategoryId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentPriceId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Advertisments]
                      SET [IsActive] = @IsActive, [Name] = @Name, [IsPaided] = @IsPaided, [Description] = @Description, [NumberOfViews] = @NumberOfViews, [NumberOfLikes] = @NumberOfLikes, [PaidEdPrice] = @PaidEdPrice, [IsArchieved] = @IsArchieved, [CreateDate] = @CreateDate, [StartDate] = @StartDate, [EndDate] = @EndDate, [IsExpired] = @IsExpired, [CategoryId] = @CategoryId, [ApplicationUserId] = @ApplicationUserId, [AdvertismentPriceId] = @AdvertismentPriceId
                      WHERE ([Id] = @Id)"
            );



            



        }

        public override void Down()
        {
            DropStoredProcedure("dbo.AdvertismentTransaction_Delete");
            DropStoredProcedure("dbo.AdvertismentTransaction_Update");
            DropStoredProcedure("dbo.AdvertismentTransaction_Insert");
            DropForeignKey("dbo.AdvertismentTransactions", "AdvertismentId", "dbo.Advertisments");
            DropIndex("dbo.AdvertismentTransactions", new[] { "AdvertismentId" });
            DropColumn("dbo.Advertisments", "IsExpired");
            DropTable("dbo.AdvertismentTransactions");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
