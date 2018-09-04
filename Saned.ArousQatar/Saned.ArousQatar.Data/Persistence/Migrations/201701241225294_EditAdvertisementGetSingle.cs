namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditAdvertisementGetSingle : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure(
                "[dbo].[AdvertisementGetSingle]",
                p => new
                {
                    id = p.Int()
                },
                @"
	            	select
	            	Advertisments.Id , [CreateDate],
		            Advertisments.Name,[StartDate],[EndDate],
                	AdvertismentPrices.Period 'AdvertisementPeriod',
                	AdvertismentPrices.Price 'AdvertisementPrice' ,
                	AspNetUsers.Name 'FullName',
                	AspNetUsers.UserName, 
	            	Advertisments.[Description],
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
				 ");
        }

        public override void Down()
        {
        }
    }
}
