namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class apiChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvertismentPrices", "IsArchieved", c => c.Boolean());
            AddColumn("dbo.Comments", "CommentParentId", c => c.Int());
            AddColumn("dbo.Complaints", "IsArchieved", c => c.Boolean());
            AddColumn("dbo.Complaints", "ComplainedId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "CommentParentId");
            CreateIndex("dbo.Complaints", "ComplainedId");
            AddForeignKey("dbo.Comments", "CommentParentId", "dbo.Comments", "Id");
            AddForeignKey("dbo.Complaints", "ComplainedId", "dbo.AspNetUsers", "Id");
            AlterStoredProcedure(
                "dbo.AdvertismentPrice_Insert",
                p => new
                    {
                        Period = p.Int(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        IsArchieved = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[AdvertismentPrices]([Period], [Price], [IsArchieved])
                      VALUES (@Period, @Price, @IsArchieved)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AdvertismentPrices]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AdvertismentPrices] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.AdvertismentPrice_Update",
                p => new
                    {
                        Id = p.Int(),
                        Period = p.Int(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        IsArchieved = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[AdvertismentPrices]
                      SET [Period] = @Period, [Price] = @Price, [IsArchieved] = @IsArchieved
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Comment_Insert",
                p => new
                    {
                        Message = p.String(),
                        CreateDate = p.DateTime(),
                        AdvertismentId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        CommentParentId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Comments]([Message], [CreateDate], [AdvertismentId], [ApplicationUserId], [CommentParentId])
                      VALUES (@Message, @CreateDate, @AdvertismentId, @ApplicationUserId, @CommentParentId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Comments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Comments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Comment_Update",
                p => new
                    {
                        Id = p.Int(),
                        Message = p.String(),
                        CreateDate = p.DateTime(),
                        AdvertismentId = p.Int(),
                        ApplicationUserId = p.String(maxLength: 128),
                        CommentParentId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Comments]
                      SET [Message] = @Message, [CreateDate] = @CreateDate, [AdvertismentId] = @AdvertismentId, [ApplicationUserId] = @ApplicationUserId, [CommentParentId] = @CommentParentId
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Complaint_Insert",
                p => new
                    {
                        Message = p.String(),
                        IsArchieved = p.Boolean(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentId = p.Int(),
                        ComplainedId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Complaints]([Message], [IsArchieved], [ApplicationUserId], [AdvertismentId], [ComplainedId])
                      VALUES (@Message, @IsArchieved, @ApplicationUserId, @AdvertismentId, @ComplainedId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Complaints]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Complaints] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Complaint_Update",
                p => new
                    {
                        Id = p.Int(),
                        Message = p.String(),
                        IsArchieved = p.Boolean(),
                        ApplicationUserId = p.String(maxLength: 128),
                        AdvertismentId = p.Int(),
                        ComplainedId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Complaints]
                      SET [Message] = @Message, [IsArchieved] = @IsArchieved, [ApplicationUserId] = @ApplicationUserId, [AdvertismentId] = @AdvertismentId, [ComplainedId] = @ComplainedId
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Complaints", "ComplainedId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "CommentParentId", "dbo.Comments");
            DropIndex("dbo.Complaints", new[] { "ComplainedId" });
            DropIndex("dbo.Comments", new[] { "CommentParentId" });
            DropColumn("dbo.Complaints", "ComplainedId");
            DropColumn("dbo.Complaints", "IsArchieved");
            DropColumn("dbo.Comments", "CommentParentId");
            DropColumn("dbo.AdvertismentPrices", "IsArchieved");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
