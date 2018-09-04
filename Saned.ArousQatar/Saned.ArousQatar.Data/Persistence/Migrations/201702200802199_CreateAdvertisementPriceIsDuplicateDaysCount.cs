namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateAdvertisementPriceIsDuplicateDaysCount : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("AdvertisementGetAll",
             p => new
             {
                 index = p.Int(),
                 rowNumber = p.Int(),
                 filter = p.String(100, defaultValue: ""),
                 IsArchieved = p.Boolean(defaultValueSql: "NULL"),
                 CategoryId = p.Int(defaultValueSql: "NULL"),
                 IsPaided = p.Boolean(defaultValueSql: "NULL"),
                 UserId = p.String(128, defaultValueSql: "NULL")
             }, @"SELECT Categories.Name 'CategoryName',
                                AdvertismentPrices.Period 'AdvertisementPeriod',
                                AdvertismentPrices.Price 'AdvertisementPrice',
                                AspNetUsers.Name 'FullName',
                                AspNetUsers.UserName
                                , COUNT(Advertisments.Id) OVER() AS 'OverAllCount',
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

                            Advertisments.IsArchieved = ISNULL(@IsArchieved, Advertisments.IsArchieved)

                            AND
                            (Advertisments.Name LIKE '%' + @filter + '%' OR

                            Advertisments.[Description] LIKE '%' + @filter + '%' OR

                            Categories.Name LIKE '%' + @filter + '%' OR

                            AdvertismentPrices.[Period] LIKE '%' + @filter + '%' OR

                            AdvertismentPrices.Price like '%' + @filter + '%' OR

                            AspNetUsers.Name LIKE '%' + @filter + '%' or

                            AspNetUsers.UserName LIKE '%' + @filter + '%')

                            AND(@CategoryId IS NULL OR Categories.Id = @CategoryId)

                            AND(@IsPaided IS NULL OR Advertisments.IsPaided = @IsPaided)

                            AND(@UserId IS NULL OR Advertisments.ApplicationUserId = @UserId)
                            ORDER BY Advertisments.CreateDate Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
            ");

            CreateStoredProcedure("AdvertisementPriceIsDuplicateDaysCount",
                p => new
                {
                    DaysCount = p.Int(),
                    AdvertismentPriceId = p.Int(defaultValueSql: "NULL")
                },
                @"SELECT COUNT(Id) FROM AdvertismentPrices
                  WHERE CAST(PeriOd AS INT) = @DaysCount
                  AND(@AdvertismentPriceId IS NULL OR Id != @AdvertismentPriceId) ");
        }

        public override void Down()
        {
            DropStoredProcedure("AdvertisementPriceIsDuplicateDaysCount");
        }
    }
}
