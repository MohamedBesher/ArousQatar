namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rescafoldingComplaint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Complaints", "AdvertismentId", c => c.Int());
            CreateIndex("dbo.Complaints", "AdvertismentId");
            AddForeignKey("dbo.Complaints", "AdvertismentId", "dbo.Advertisments", "Id");
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
            DropForeignKey("dbo.Complaints", "AdvertismentId", "dbo.Advertisments");
            DropIndex("dbo.Complaints", new[] { "AdvertismentId" });
            DropColumn("dbo.Complaints", "AdvertismentId");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
