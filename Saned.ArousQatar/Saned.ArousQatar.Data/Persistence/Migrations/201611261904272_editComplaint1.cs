namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editComplaint1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Complaints", "Advertisment_Id", "dbo.Advertisments");
            DropIndex("dbo.Complaints", new[] { "Advertisment_Id" });
            DropColumn("dbo.Complaints", "Advertisment_Id");
            AlterStoredProcedure(
                "dbo.Complaint_Insert",
                p => new
                    {
                        Message = p.String(),
                        IsArchieved = p.Boolean(),
                        ApplicationUserId = p.String(maxLength: 128),
                        ComplainedId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Complaints]([Message], [IsArchieved], [ApplicationUserId], [ComplainedId])
                      VALUES (@Message, @IsArchieved, @ApplicationUserId, @ComplainedId)
                      
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
                        ComplainedId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Complaints]
                      SET [Message] = @Message, [IsArchieved] = @IsArchieved, [ApplicationUserId] = @ApplicationUserId, [ComplainedId] = @ComplainedId
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Complaint_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Complaints]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Complaints", "Advertisment_Id", c => c.Int());
            CreateIndex("dbo.Complaints", "Advertisment_Id");
            AddForeignKey("dbo.Complaints", "Advertisment_Id", "dbo.Advertisments", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
