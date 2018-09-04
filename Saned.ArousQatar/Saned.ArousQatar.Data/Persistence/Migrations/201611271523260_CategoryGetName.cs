namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CategoryGetName : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CategoryGetName",
               p => new
               {

               },
               @"
					SELECT Categories.Id , Categories.Name , Categories.IconName FROM Categories 
				");
        }

        public override void Down()
        {
            DropStoredProcedure("CategoryGetName");
        }
    }
}
