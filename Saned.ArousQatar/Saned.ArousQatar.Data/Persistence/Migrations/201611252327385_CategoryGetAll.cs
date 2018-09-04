namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CategoryGetAll : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure(
           "CategoryGetAll",
               p => new
               {
                   index = p.Int(),
                   rowNumber = p.Int(),
                   filter = p.String(100, defaultValue: ""),
                   IsArchieved = p.Boolean(defaultValueSql: "NULL")
               },
               @"
                    SELECT *,COUNT(Categories.Id) OVER() AS 'OverAllCount' FROM Categories 
                	Where  Categories.IsArchieved =isnull(@IsArchieved , IsArchieved) and 
                        (Categories.Name like '%'+@filter+'%')
                	ORDER BY  Id DESC OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CategoryGetAll");
        }
    }
}
