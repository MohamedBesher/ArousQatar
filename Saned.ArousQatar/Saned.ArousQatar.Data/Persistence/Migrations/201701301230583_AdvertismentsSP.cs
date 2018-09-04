namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertismentsSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AdvertisementGetByUserId", p => new {
                UserId = p.String(maxLength: 128),
                PageNumber = p.Int(1),
                PageSize = p.Int(8),
                Filter = p.String(250, defaultValueSql: "NULL"),
                IsArchieved = p.Boolean(defaultValueSql: "NULL")              
            }, @"  SELECT
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
    
    	            FROM [dbo].[Advertisments] 

    	            WHERE 
    	            	[dbo].[Advertisments].IsArchieved = ISNULL(@IsArchieved,Advertisments.IsArchieved) AND 
    	            	[dbo].[Advertisments].[ApplicationUserId] = @UserId AND 
					    (@Filter IS NULL OR Advertisments.Name LIKE '%'+@Filter +'%' OR Advertisments.[Description] LIKE '%'+@Filter +'%')
    	            ORDER BY Advertisments.CreateDate Desc OFFSET @PageSize * ( @PageNumber - 1 ) ROWS FETCH NEXT @PageSize ROWS ONLY;
");



            CreateStoredProcedure("AdvertismentImagesByAdsId", p => new {
                AdId = p.Int(),
            }, @"SELECT*
                FROM [dbo].[AdvertismentImages]
                WHERE [dbo].[AdvertismentImages].AdvertismentId=@AdId
                ORDER BY [IsMainImage] DESC");
        }
        
        public override void Down()
        {
            DropStoredProcedure("AdvertisementGetByUserId");
            DropStoredProcedure("AdvertismentImagesByAdsId");
        }
    }
}
