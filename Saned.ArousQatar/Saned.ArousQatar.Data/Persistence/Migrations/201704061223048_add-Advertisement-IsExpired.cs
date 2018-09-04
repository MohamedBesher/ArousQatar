namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdvertisementIsExpired : DbMigration
    {
        public override void Up()
        {
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
		(select ap.Period FROM dbo.AdvertismentPrices ap WHERE ap.Id=[Advertisments].AdvertismentPriceId) AS 'AdvertisementPeriod',
    	(select ap.Price FROM dbo.AdvertismentPrices ap WHERE ap.Id	=[Advertisments].AdvertismentPriceId) as  'AdvertisementPrice' ,
		Advertisments.AdvertismentPriceId,
		Advertisments.PaidEdPrice,
		Advertisments.IsPaided	,
		dbo.Advertisments.IsExpired,
		Advertisments.IsActive,
		 
    
    				    Advertisments.StartDate,
    					Advertisments.EndDate,
    					Advertisments.CreateDate,
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
    FROM [Advertisments]
    WHERE 
    IsArchieved=ISNULL(@IsArchieved,IsArchieved) AND 
	(@IsActive is null or IsActive = @IsActive)  AND 
	 [Advertisments].IsPaided=1 AND  [Advertisments].[IsExpired]=0
	 and GetDate() >= [Advertisments].StartDate and GetDate() <= [Advertisments].EndDate
    ORDER BY [IsPaided] DESC,[CreateDate] DESC
    OFFSET @PageSize * ( @PageNumber - 1 ) ROWS
    FETCH NEXT @PageSize ROWS ONLY
    OPTION  ( RECOMPILE );
    
END");
            Sql(@"ALTER PROCEDURE [dbo].[AdvertisementGetByUsername]
    @username [nvarchar](256),
    @index [int],
    @rowNumber [int],
    @filter [nvarchar](100) = '',
    @IsArchieved [bit] = NULL
AS
BEGIN
    
    SELECT
    	            	[Advertisments].Id,
    	            	[Advertisments].Name,
						(select ap.Period FROM dbo.AdvertismentPrices ap WHERE ap.Id=[Advertisments].AdvertismentPriceId) AS 'AdvertisementPeriod',
    	(select ap.Price FROM dbo.AdvertismentPrices ap WHERE ap.Id	=[Advertisments].AdvertismentPriceId) as  'AdvertisementPrice' ,
		Advertisments.AdvertismentPriceId,
		Advertisments.PaidEdPrice,
		Advertisments.IsPaided	,
		dbo.Advertisments.IsExpired,
		Advertisments.IsActive,
		 
    
    				    Advertisments.StartDate,
    					Advertisments.EndDate,
    					Advertisments.CreateDate,
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
    
    
END");
            Sql(@"ALTER PROCEDURE [dbo].[AdvertisementGetSingle]
    @id [int]
AS
BEGIN
    
    	 select
    	            	Advertisments.Id , 
    		            Advertisments.Name,
    	AdvertismentPrices.Period 'AdvertisementPeriod',
    	AdvertismentPrices.Price 'AdvertisementPrice' ,
    	AspNetUsers.Name 'FullName',
		Advertisments.AdvertismentPriceId,Advertisments.PaidEdPrice

		,
    				    AspNetUsers.PhotoUrl ,
    	AspNetUsers.UserName,
		Advertisments.IsPaided	,dbo.Advertisments.IsExpired,Advertisments.IsActive,
		 
    
    				    Advertisments.StartDate,
    					Advertisments.EndDate,
    					Advertisments.CreateDate,
    					Advertisments.ApplicationUserId,
    	            	Advertisments.[Description],
    					Advertisments.CategoryId,
    	            	AspNetUsers.PhoneNumber,
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
    
    						
    	FROM Advertisments
    						
    	left join Categories on 
    	Advertisments.CategoryId = Categories.Id 
    	left join AdvertismentPrices on
    	Advertisments.AdvertismentPriceId = AdvertismentPrices.Id
    	left join AspNetUsers on
    	AspNetUsers.Id = Advertisments.ApplicationUserId
    						
    where Advertisments.Id = @id
    				 
END");
            


        }
        
        public override void Down()
        {
        }
    }
}
