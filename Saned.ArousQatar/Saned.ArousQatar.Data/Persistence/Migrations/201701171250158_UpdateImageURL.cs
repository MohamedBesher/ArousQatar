namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateImageURL : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AspNetUsers_Update_ImageUrl",
                p => new
                {
                    Id = p.String(128),
                    PhotoUrl = p.String()

                },

                @" Update AspNetUsers SET PhotoUrl=@PhotoUrl WHERE Id=@Id");
        }
    }
}
