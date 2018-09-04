namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertisementGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "[dbo].[AdvertisementGetAll]",
                p => new
                {
                    index = p.Int(),
                    rowNumber = p.Int(),
                    filter = p.String(100, defaultValue: ""),
                    IsArchieved = p.Boolean(defaultValueSql: "NULL")
                },
                @"
	                SELECT
			                Categories.Name 'CategoryName',
			                AdvertismentPrices.Period 'AdvertisementPeriod',
			                AdvertismentPrices.Price 'AdvertisementPrice' ,
			                AspNetUsers.Name 'FullName',
			                AspNetUsers.UserName
			                ,COUNT(Advertisments.Id) OVER() AS 'OverAllCount',
			                Advertisments.*
				

		                FROM Advertisments
			                left join Categories
		                ON 
			                Advertisments.CategoryId = Categories.Id 
		                LEFT JOIN
			                AdvertismentPrices
		                ON
			                Advertisments.AdvertismentPriceId = AdvertismentPrices.Id
		                LEFT JOIN AspNetUsers
		                ON
			                AspNetUsers.Id = Advertisments.ApplicationUserId
    	                where
		                Advertisments.IsArchieved =ISNULL(@IsArchieved,Advertisments.IsArchieved)
		                 AND 
		                (Advertisments.Name LIKE '%'+@filter +'%' OR
		                Advertisments.[Description] LIKE '%'+@filter +'%' OR 
		                Categories.Name LIKE '%'+@filter +'%' OR 
		                AdvertismentPrices.[Period] LIKE '%'+@filter +'%' OR
		                AdvertismentPrices.Price like '%'+@filter +'%' OR  
		                AspNetUsers.Name LIKE '%'+@filter +'%' or 
		                AspNetUsers.UserName LIKE '%'+@filter +'%')
    
		                ORDER BY Advertisments.CreateDate Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
				 "
                );
        }

        public override void Down()
        {
            DropStoredProcedure("[dbo].[AdvertisementGetAll]");
        }
    }
}
