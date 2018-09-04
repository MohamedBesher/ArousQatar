namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvertisementGetAllByCategoryId : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AdvertisementGetAllByCategoryId", p => new
            {
                index = p.Int(),
                rowNumber = p.Int(),
                filter = p.String(100, defaultValue: ""),
                IsArchieved = p.Boolean(defaultValueSql: "NULL"),
                CategoryId = p.Int(defaultValueSql: "NULL")
            }, @"
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
	WHERE [dbo].[Advertisments].CategoryId=ISNULL(@CategoryId,[dbo].[Advertisments].CategoryId) 
	AND
	Advertisments.IsArchieved=ISNULL(@IsArchieved,Advertisments.IsArchieved)
	AND 
    (Advertisments.Name LIKE '%'+@filter +'%' OR Advertisments.[Description] LIKE '%'+@filter +'%')
	ORDER BY Advertisments.CreateDate Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY;
                ");
        }

        public override void Down()
        {

            DropStoredProcedure("AdvertisementGetAllByCategoryId");
        }
    }
}
