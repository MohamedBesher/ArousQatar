namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsArchieve : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "IsArchieved", c => c.Boolean());
            AlterStoredProcedure(
                "dbo.Category_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 150),
                        IsArchieved = p.Boolean(),
                        IconName = p.String(maxLength: 50),
                        ImageUrl = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Categories]([Name], [IsArchieved], [IconName], [ImageUrl])
                      VALUES (@Name, @IsArchieved, @IconName, @ImageUrl)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Categories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Categories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Category_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 150),
                        IsArchieved = p.Boolean(),
                        IconName = p.String(maxLength: 50),
                        ImageUrl = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Categories]
                      SET [Name] = @Name, [IsArchieved] = @IsArchieved, [IconName] = @IconName, [ImageUrl] = @ImageUrl
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "IsArchieved");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
