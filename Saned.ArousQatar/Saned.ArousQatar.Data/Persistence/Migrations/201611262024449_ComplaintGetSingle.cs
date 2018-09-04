namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ComplaintGetSingle : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("ComplaintGetSingle",
                p => new
                {
                    id = p.Int()
                },
                @"
                    select * from Complaints where Complaints.Id = @id
                "
                );
        }

        public override void Down()
        {
            DropStoredProcedure("ComplaintGetSingle");
        }
    }
}
