namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteIsArchieve : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Categories", "IsArchieved");
            AlterStoredProcedure(
                "dbo.Category_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 150),
                        IconName = p.String(maxLength: 50),
                        ImageUrl = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Categories]([Name], [IconName], [ImageUrl])
                      VALUES (@Name, @IconName, @ImageUrl)
                      
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
                        IconName = p.String(maxLength: 50),
                        ImageUrl = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Categories]
                      SET [Name] = @Name, [IconName] = @IconName, [ImageUrl] = @ImageUrl
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "IsArchieved", c => c.Boolean());
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
