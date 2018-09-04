namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameOfAdvertisement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advertisments", "Name", c => c.String());
            AlterStoredProcedure(
                "dbo.Advertisment_Insert",
                p => new
                    {
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
                        CategoryId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentPriceId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Advertisments]([Name], [IsPaided], [Description], [NumberOfViews], [NumberOfLikes], [PaidEdPrice], [IsArchieved], [CreateDate], [StartDate], [EndDate], [CategoryId], [ApplicationUserId], [AdvertismentPriceId])
                      VALUES (@Name, @IsPaided, @Description, @NumberOfViews, @NumberOfLikes, @PaidEdPrice, @IsArchieved, @CreateDate, @StartDate, @EndDate, @CategoryId, @ApplicationUserId, @AdvertismentPriceId)
                      
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
                        CategoryId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentPriceId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Advertisments]
                      SET [Name] = @Name, [IsPaided] = @IsPaided, [Description] = @Description, [NumberOfViews] = @NumberOfViews, [NumberOfLikes] = @NumberOfLikes, [PaidEdPrice] = @PaidEdPrice, [IsArchieved] = @IsArchieved, [CreateDate] = @CreateDate, [StartDate] = @StartDate, [EndDate] = @EndDate, [CategoryId] = @CategoryId, [ApplicationUserId] = @ApplicationUserId, [AdvertismentPriceId] = @AdvertismentPriceId
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advertisments", "Name");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
