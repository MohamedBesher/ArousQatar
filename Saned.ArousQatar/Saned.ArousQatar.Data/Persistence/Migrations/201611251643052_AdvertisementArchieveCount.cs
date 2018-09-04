namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvertisementArchieveCount : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "[dbo].[AdvertisementArchieveCount]",
                p => new
                {

                },
                @"
                    select count(*) from Advertisments
                    where Advertisments.IsArchieved = 1
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("[dbo].[AdvertisementArchieveCount]");
        }
    }
}
