namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editCommentModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Complaints", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Complaints", new[] { "ApplicationUserId" });
            DropColumn("dbo.Complaints", "ApplicationUserId");
            AlterStoredProcedure(
                "dbo.Complaint_Insert",
                p => new
                    {
                        Message = p.String(),
                        IsArchieved = p.Boolean(),
                        AdvertismentId = p.Int(),
                        ComplainedId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Complaints]([Message], [IsArchieved], [AdvertismentId], [ComplainedId])
                      VALUES (@Message, @IsArchieved, @AdvertismentId, @ComplainedId)
                      
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
                        AdvertismentId = p.Int(),
                        ComplainedId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Complaints]
                      SET [Message] = @Message, [IsArchieved] = @IsArchieved, [AdvertismentId] = @AdvertismentId, [ComplainedId] = @ComplainedId
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Complaints", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Complaints", "ApplicationUserId");
            AddForeignKey("dbo.Complaints", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
