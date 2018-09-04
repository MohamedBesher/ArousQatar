namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FavoriteGetSingle : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("FavoriteGetSingle", p => new
            {
                id = p.Int()
            },
                @"
                    select Favorites.* , Advertisments.Name 'AdvertisementName' 
                        , AspNetUsers.Name 'UserFirstName' 
                    from Favorites
                    inner join Advertisments on 
                        Advertisments.Id = Favorites.AdvertismentId
                    inner join AspNetUsers on
                        AspNetUsers.Id = Favorites.ApplicationUserId 

                    where Favorites.Id = @id 
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("FavoritesGetSingle");
        }
    }
}
