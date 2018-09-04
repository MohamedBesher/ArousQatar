namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class LikeGetSingle : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("LikeGetSinlge", p => new
            {
                id = p.Int()
            },
            @"
                select * from Likes where Likes.Id = @id
             ");
        }

        public override void Down()
        {
            DropStoredProcedure("LikeGetSingle");
        }
    }
}
