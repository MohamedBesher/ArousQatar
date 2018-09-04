namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFavoriteGetAll : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER PROCEDURE [FavoriteGetAll]
    @username [nvarchar](256),
    @index [int],
    @rowNumber [int]
AS
BEGIN
    
    select  Favorites.[Id] , 
    		Advertisments.Name , 
			Advertisments.Id as AdvertismentId,
			COUNT(Favorites.Id) OVER() AS 'OverAllCount',
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
    
    from Favorites
    inner join Advertisments on 
    Advertisments.Id = Favorites.AdvertismentId
    
    	inner join AspNetUsers on
    AspNetUsers.Id = Favorites.ApplicationUserId 
    
    where AspNetUsers.UserName = @username
    
    ORDER BY Advertisments.CreateDate Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
    
END
");
        }
        
        public override void Down()
        {
        }
    }
}
