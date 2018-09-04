namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CommentReplyGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "CommentReplyGetAll",
                p => new
                {
                    id = p.Int()
                },
                @"
                   select Comments.* , AspNetUsers.Name 'UserFirstName'
                      from Comments 
                       inner join AspNetUsers
                      on AspNetUsers.Id = Comments.ApplicationUserId
                      where Comments.CommentParentId = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CommentReplyGetAll");
        }
    }
}
