namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvertismentDelete : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AdvertisementDeleteGetSingle",
                p => new
                {
                    id = p.Int()
                }, @"
                    select * from Advertisments where Advertisments.Id = @id
                    ");
        }

        public override void Down()
        {
            DropStoredProcedure("AdvertisementDeleteGetSingle");
        }
    }
}
