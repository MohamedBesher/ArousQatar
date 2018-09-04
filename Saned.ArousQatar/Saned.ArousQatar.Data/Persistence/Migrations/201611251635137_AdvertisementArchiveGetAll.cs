namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertisementArchiveGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "[dbo].[AdvertisementArchieveGetAll]",
                p => new
                {
                    index = p.Int(),
                    rowNumber = p.Int(),
                    filter = p.String(100, defaultValue: "")
                },
                @"
					SELECT Advertisments.* ,
					  Categories.Name 'CategoryName',
					  AdvertismentPrices.Period 'AdvertisementPeriod',
					  AdvertismentPrices.Price 'AdvertisementPrice' ,
					  AspNetUsers.Name 'UserName',
					  AspNetUsers.UserName

					  FROM Advertisments

					   left join Categories on 
						Advertisments.CategoryId = Categories.Id 
					   left join AdvertismentPrices on
						Advertisments.AdvertismentPriceId = AdvertismentPrices.Id
					   left join AspNetUsers on
						AspNetUsers.Id = Advertisments.ApplicationUserId

					where
					Advertisments.IsArchieved = 1 and 
					(Advertisments.Name like '%'+@filter +'%' or
					 Advertisments.Description like '%'+@filter +'%' or 
					 Categories.Name like '%'+@filter +'%' or 
					 AdvertismentPrices.Period like '%'+@filter +'%' or
					 AdvertismentPrices.Price like '%'+@filter +'%' or  
					 AspNetUsers.Name like '%'+@filter +'%' or 
					 AspNetUsers.UserName like '%'+@filter +'%')

					ORDER BY Advertisments.Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
				 "
                );
        }

        public override void Down()
        {
            DropStoredProcedure("[dbo].[AdvertisementArchieveGetAll]");
        }
    }
}
