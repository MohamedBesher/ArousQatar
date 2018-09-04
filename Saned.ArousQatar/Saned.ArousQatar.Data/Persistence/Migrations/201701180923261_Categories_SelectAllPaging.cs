namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categories_SelectAllPaging : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("Categories_SelectAllPaging",
                p => new
                {
                    PageNumber = p.Int(1),
                    PageSize = p.Int(defaultValueSql: "NULL"),
                    Filter = p.String(250, defaultValueSql: "NULL"),
                    IsArchieved = p.Boolean(defaultValueSql: "NULL")
                  
                }, @"

 SELECT 
    Id,Name,ImageUrl,
    OverallCount = COUNT(1) OVER ( )
FROM [dbo].[Categories]
WHERE 
    IsArchieved=ISNULL(@IsArchieved,IsArchieved)
   
     AND
    (
    @Filter IS NULL
    OR Name  LIKE '%' + @Filter + '%')

    ORDER BY Id ASC
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
