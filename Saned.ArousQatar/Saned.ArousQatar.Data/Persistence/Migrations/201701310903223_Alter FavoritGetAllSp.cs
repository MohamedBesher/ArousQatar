namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFavoritGetAllSp : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("FavoriteGetAll",
                p => new
                {
                    username = p.String(maxLength: 256),
                    index = p.Int(),
                    rowNumber = p.Int()
                },
                @"
                    select Favorites.[Id] , 
                		Advertisments.Name , 
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
                "
                );

        }

        public override void Down()
        {
            DropStoredProcedure("FavoriteGetAll");
        }
    }
}
