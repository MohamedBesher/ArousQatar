namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContactusMessageTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUsMessages", "Name", c => c.String());
            AlterStoredProcedure(
                "dbo.ContactUsMessage_Insert",
                p => new
                    {
                        Name = p.String(),
                        Phone = p.String(),
                        Email = p.String(),
                        Message = p.String(),
                        IsArchieved = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ContactUsMessages]([Name], [Phone], [Email], [Message], [IsArchieved], [CreatedDate])
                      VALUES (@Name, @Phone, @Email, @Message, @IsArchieved, @CreatedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactUsMessages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactUsMessages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ContactUsMessage_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        Phone = p.String(),
                        Email = p.String(),
                        Message = p.String(),
                        IsArchieved = p.Boolean(),
                        CreatedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ContactUsMessages]
                      SET [Name] = @Name, [Phone] = @Phone, [Email] = @Email, [Message] = @Message, [IsArchieved] = @IsArchieved, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactUsMessages", "Name");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
