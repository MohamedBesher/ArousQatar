namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editComplaint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Complaints", "AdvertismentId", "dbo.Advertisments");
            DropIndex("dbo.Complaints", new[] { "AdvertismentId" });
            RenameColumn(table: "dbo.Complaints", name: "AdvertismentId", newName: "Advertisment_Id");
            AddColumn("dbo.Complaints", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Complaints", "Advertisment_Id", c => c.Int());
            CreateIndex("dbo.Complaints", "ApplicationUserId");
            CreateIndex("dbo.Complaints", "Advertisment_Id");
            AddForeignKey("dbo.Complaints", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Complaints", "Advertisment_Id", "dbo.Advertisments", "Id");
            AlterStoredProcedure(
                "dbo.Complaint_Insert",
                p => new
                    {
                        Message = p.String(),
                        IsArchieved = p.Boolean(),
                        ApplicationUserId = p.String(maxLength: 128),
                        ComplainedId = p.String(maxLength: 128),
                        Advertisment_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Complaints]([Message], [IsArchieved], [ApplicationUserId], [ComplainedId], [Advertisment_Id])
                      VALUES (@Message, @IsArchieved, @ApplicationUserId, @ComplainedId, @Advertisment_Id)
                      
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
                        Advertisment_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Complaints]
                      SET [Message] = @Message, [IsArchieved] = @IsArchieved, [ApplicationUserId] = @ApplicationUserId, [ComplainedId] = @ComplainedId, [Advertisment_Id] = @Advertisment_Id
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Complaint_Delete",
                p => new
                    {
                        Id = p.Int(),
                        Advertisment_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Complaints]
                      WHERE (([Id] = @Id) AND (([Advertisment_Id] = @Advertisment_Id) OR ([Advertisment_Id] IS NULL AND @Advertisment_Id IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Complaints", "Advertisment_Id", "dbo.Advertisments");
            DropForeignKey("dbo.Complaints", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Complaints", new[] { "Advertisment_Id" });
            DropIndex("dbo.Complaints", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Complaints", "Advertisment_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Complaints", "ApplicationUserId");
            RenameColumn(table: "dbo.Complaints", name: "Advertisment_Id", newName: "AdvertismentId");
            CreateIndex("dbo.Complaints", "AdvertismentId");
            AddForeignKey("dbo.Complaints", "AdvertismentId", "dbo.Advertisments", "Id", cascadeDelete: true);
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
