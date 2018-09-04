namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CategoryArchieveGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
           "CategoryArchieveGetAll",
               p => new
               {
                   index = p.Int(),
                   rowNumber = p.Int(),
                   filter = p.String(100, defaultValue: "")
               },
               @"
                    SELECT * FROM Categories 
                	Where  Categories.IsArchieved = 1 and 
                        (Categories.Name like '%'+@filter+'%')
                	ORDER BY Id DESC OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CategoryArchieveGetAll");
        }
    }
}
