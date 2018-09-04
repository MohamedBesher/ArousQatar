namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAdvertismentPrice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdvertismentPrices", "Period", c => c.String(nullable: false, maxLength: 200));
            AlterStoredProcedure(
                "dbo.AdvertismentPrice_Insert",
                p => new
                    {
                        Period = p.String(maxLength: 200),
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
                        Period = p.String(maxLength: 200),
                        Price = p.Decimal(precision: 18, scale: 2),
                        IsArchieved = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[AdvertismentPrices]
                      SET [Period] = @Period, [Price] = @Price, [IsArchieved] = @IsArchieved
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdvertismentPrices", "Period", c => c.Int(nullable: false));
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
