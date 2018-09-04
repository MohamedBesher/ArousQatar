namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ContactTypeIsRelatedToContactInformation : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("ContactTypeIsRelatedToContactInformation",
                p => new
                {
                    id = p.Int()
                },
                @"
                    select count(*) from ContactInformations where ContactInformations.ContactTypeId = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("ContactTypeIsRelatedToContactInformation");
        }
    }
}
