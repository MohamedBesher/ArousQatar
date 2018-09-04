namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CommentGetSingleWithoutUser : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CommentGetSingleWithoutUser",
                p => new
                {
                    id = p.Int()
                },
                @"
                    select * from Comments where Comments.Id = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CommentGetSingleWithoutUser");
        }
    }
}
