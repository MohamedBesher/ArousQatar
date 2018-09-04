namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdvertisementEditHomeAds : DbMigration
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
    FROM [Advertisments]
    WHERE 
   (@IsArchieved is null or IsNull(IsArchieved,0)=@IsArchieved) AND 
   --(@IsActive is null or IsActive = @IsActive)  AND 
   --IsArchieved=@IsArchieved and
   (@IsActive is null or IsActive = @IsActive) and
	 [Advertisments].IsPaided=1 AND  [Advertisments].[IsExpired]=0
	and    GetDate() BETWEEN	[Advertisments].StartDate and  [Advertisments].EndDate
    ORDER BY [IsPaided] DESC,[CreateDate] DESC
    OFFSET @PageSize * ( @PageNumber - 1 ) ROWS
    FETCH NEXT @PageSize ROWS ONLY
    OPTION  ( RECOMPILE );
    
END");
        }
        
        public override void Down()
        {
        }
    }
}
