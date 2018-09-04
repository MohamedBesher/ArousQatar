namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvertisementGetByUsername : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("[dbo].[AdvertisementGetByUsername]", p => new
            {
                username = p.String(256),
                index = p.Int(),
                rowNumber = p.Int(),
                filter = p.String(100, defaultValue: ""),
                IsArchieved = p.Boolean(defaultValueSql: "NULL")
            },
            @"
                SELECT
	            	[Advertisments].Id,
	            	[Advertisments].Name,
	            	ISNULL([NumberOfViews],0) AS 'NumberOfViews',
                    COUNT(Advertisments.Id) OVER() AS 'OverAllCount',
	            	ISNULL(NumberOfLikes,0) AS 'NumberOfLikes',
	            	(
	            		SELECT
	            		COUNT(Id)
	            		FROM [dbo].[Comments]
	            		WHERE [dbo].[Comments].AdvertismentId=[dbo].[Advertisments].Id
	            	) AS 'Comments',
	            	(
	            		SELECT
	            		TOP 1 [ImageUrl]
	            		FROM [dbo].[AdvertismentImages]
	            		WHERE [dbo].[AdvertismentImages].AdvertismentId=[dbo].[Advertisments].Id
	            		ORDER BY [IsMainImage] DESC
	            	) AS 'ImageUrl'

	            FROM [dbo].[Advertisments] join
	            AspNetUsers ON
	            AspNetUsers.Id = Advertisments.ApplicationUserId
	            WHERE 
	            	[dbo].[Advertisments].IsArchieved = ISNULL(@IsArchieved,Advertisments.IsArchieved)
	            AND 
	            	[dbo].[AspNetUsers].UserName = @username
	            AND 
                (Advertisments.Name LIKE '%'+@filter +'%' OR Advertisments.[Description] LIKE '%'+@filter +'%')

	            ORDER BY Advertisments.CreateDate Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
    
            ");
        }

        public override void Down()
        {

            DropStoredProcedure("[dbo].[AdvertisementGetByUsername]");
        }
    }
}
