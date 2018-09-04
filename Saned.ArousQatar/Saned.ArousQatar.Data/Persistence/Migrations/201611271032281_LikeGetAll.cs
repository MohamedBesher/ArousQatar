namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class LikeGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("LikeGetAll", p => new
            {
                id = p.Int()
            },
            @"
                select Likes.* , AspNetUsers.Name 'UserFirstName' 
                from Likes join  AspNetUsers  on
                   AspNetUsers.Id = Likes.ApplicationUserId
                where AdvertismentId = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("LikeGetAll");
        }
    }
}
