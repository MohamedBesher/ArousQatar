namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Advertisments_SelectAllPaging : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("Advertisments_SelectAllPaging",
                p => new
                {
                    PageNumber = p.Int(1),
                    PageSize = p.Int(defaultValueSql: "NULL"),
                     IsArchieved = p.Boolean(defaultValueSql: "NULL")

                }, @"

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
    IsArchieved=ISNULL(@IsArchieved,IsArchieved)
    ORDER BY [IsPaided] DESC,[CreateDate] DESC
    OFFSET @PageSize * ( @PageNumber - 1 ) ROWS
    FETCH NEXT @PageSize ROWS ONLY
    OPTION  ( RECOMPILE );
");
        }

        public override void Down()
        {
        }
    }
}
