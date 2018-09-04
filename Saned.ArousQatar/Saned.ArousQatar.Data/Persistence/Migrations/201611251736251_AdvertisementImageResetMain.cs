namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvertisementImageResetMain : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "[dbo].[AdvertisementImageResetMain]",
                p => new
                {
                    @id = p.Int()
                },
                @"
                    update AdvertismentImages set IsMainImage = 0 where AdvertismentImages.AdvertismentId = @id  
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("[dbo].[AdvertisementImageResetMain]");
        }
    }
}
