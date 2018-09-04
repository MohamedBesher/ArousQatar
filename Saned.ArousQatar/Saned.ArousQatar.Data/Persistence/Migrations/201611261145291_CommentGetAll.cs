namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CommentGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "CommentGetAll",
                p => new
                {
                    id = p.Int()
                },
                @"
                   select Comments.* , AspNetUsers.Name 'UserFirstName'
                      from Comments 
                       inner join AspNetUsers
                      on AspNetUsers.Id = Comments.ApplicationUserId
                      where Comments.AdvertismentId = @id and Comments.CommentParentId is null
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CommentGetAll");
        }
    }
}
