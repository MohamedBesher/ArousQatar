namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ContactTypeGetSingle : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("ContactTypeGetSingle",
               p => new
               {
                   id = p.Int()
               },
               @"
                    select * from ContactTypes where ContactTypes.Id = @id
                "
               );
        }

        public override void Down()
        {
            DropStoredProcedure("ContactTypeGetSingle");
        }
    }
}
