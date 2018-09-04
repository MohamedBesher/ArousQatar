namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ContactInformationGetSingle : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("ContactInformationGetSingle",
               p => new
               {
                   id = p.Int()
               },
               @"
                    select * from ContactInformations where ContactInformations.Id = @id
                "
               );
        }

        public override void Down()
        {
            DropStoredProcedure("ContactInformationGetSingle");
        }
    }
}
