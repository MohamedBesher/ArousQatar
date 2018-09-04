namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CommentDeleteAllReplays : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CommentDeleteAllReplays", p => new
            {
                id = p.Int()
            },
                @"
                Delete Comments 
	                where CommentParentId = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CommentDeleteAllReplays");
        }
    }
}
