namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CommentGetSingle : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CommentGetSingle", p => new { id = p.Int() },
                @"
                    select Comments.* , AspNetUsers.Name 'UserFirstName'
                    from Comments
                    inner join AspNetUsers
                      on AspNetUsers.Id = Comments.ApplicationUserId
                    where Comments.Id = @id
                 ");
        }

        public override void Down()
        {
            DropStoredProcedure("CommentGetSingle");
        }
    }
}
