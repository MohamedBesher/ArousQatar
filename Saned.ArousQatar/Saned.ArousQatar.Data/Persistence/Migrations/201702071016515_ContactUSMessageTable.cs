namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactUSMessageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactUsMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Phone = c.String(),
                        Email = c.String(),
                        Message = c.String(),
                        IsArchieved = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.ContactUsMessage_Insert",
                p => new
                    {
                        Phone = p.String(),
                        Email = p.String(),
                        Message = p.String(),
                        IsArchieved = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ContactUsMessages]([Phone], [Email], [Message], [IsArchieved], [CreatedDate])
                      VALUES (@Phone, @Email, @Message, @IsArchieved, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactUsMessages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactUsMessages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ContactUsMessage_Update",
                p => new
                    {
                        Id = p.Int(),
                        Phone = p.String(),
                        Email = p.String(),
                        Message = p.String(),
                        IsArchieved = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ContactUsMessages]
                      SET [Phone] = @Phone, [Email] = @Email, [Message] = @Message, [IsArchieved] = @IsArchieved, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactUsMessage_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ContactUsMessages]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ContactUsMessage_Delete");
            DropStoredProcedure("dbo.ContactUsMessage_Update");
            DropStoredProcedure("dbo.ContactUsMessage_Insert");
            DropTable("dbo.ContactUsMessages");
        }
    }
}
