namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SelectOnlyActiveAds : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER PROCEDURE [Advertisments_SelectAllPaging]
    @PageNumber [int] = 1,
    @PageSize [int] = NULL,
    @IsArchieved [bit] = NULL,
	@IsActive [bit] = NULL 
AS
BEGIN
    
    
    SELECT 
    Id, OverallCount = COUNT(1) OVER ( ),
    (SELECT TOP(1)
    [ImageUrl] 
    FROM
    [AdvertismentImages]
    WHERE
    [AdvertismentImages].[AdvertismentId]=[Advertisments].Id
    ) AS 'ImageUrl'
    From[Advertisments]
    WHERE 
    IsArchieved=ISNULL(@IsArchieved,IsArchieved) AND 
	(@IsActive is null or IsActive = @IsActive)
    ORDER BY [IsPaided] DESC,[CreateDate] DESC
    OFFSET @PageSize * ( @PageNumber - 1 ) ROWS
    FETCH NEXT @PageSize ROWS ONLY
    OPTION  ( RECOMPILE );
    
END");

            Sql(@"ALTER PROCEDURE [AdvertisementGetByUserId]
    @UserId [nvarchar](128),
    @PageNumber [int] = 1,
    @PageSize [int] = 8,
    @Filter [nvarchar](250) = NULL,
    @IsArchieved [bit] = NULL,
	@IsActive [bit] = NULL
AS
BEGIN
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
    
    	            FROM [dbo].[Advertisments] 
    
    	            WHERE 
    	            	[dbo].[Advertisments].IsArchieved = ISNULL(@IsArchieved,Advertisments.IsArchieved) AND 
    	            	[dbo].[Advertisments].[ApplicationUserId] = @UserId AND 
						(@IsActive IS NULL OR IsActive = @IsActive) AND 
    					    (@Filter IS NULL OR Advertisments.Name LIKE '%'+@Filter +'%' OR Advertisments.[Description] LIKE '%'+@Filter +'%')
    	            ORDER BY Advertisments.CreateDate Desc OFFSET @PageSize * ( @PageNumber - 1 ) ROWS FETCH NEXT @PageSize ROWS ONLY;
    
END");

            Sql(@"ALTER PROCEDURE [AdvertisementGetAllByCategoryId]
    @index [int],
    @rowNumber [int],
    @filter [nvarchar](100) = '',
    @IsArchieved [bit] = NULL,
    @CategoryId [int] = NULL,
	@IsActive [bit] = NULL
AS
BEGIN
    
    SELECT
    		[Id],
    		[Name],
    COUNT(Advertisments.Id) OVER() AS 'OverAllCount',
    		ISNULL([NumberOfViews],0) AS 'NumberOfViews',
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
		  
		[dbo].[Advertisments].CategoryId=ISNULL(@CategoryId,[dbo].[Advertisments].CategoryId) 
    	AND
    	Advertisments.IsArchieved=ISNULL(@IsArchieved,Advertisments.IsArchieved)
    	AND 
    (Advertisments.Name LIKE '%'+@filter +'%' OR Advertisments.[Description] LIKE '%'+@filter +'%')
	and (@IsActive IS NULL OR IsActive = @IsActive)
    	ORDER BY Advertisments.CreateDate Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
    
END");
        }
        
        public override void Down()
        {
        }
    }
}
