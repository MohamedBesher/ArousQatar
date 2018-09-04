namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserInfo : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AspNetUsers_Update_Info",
                p => new
                {
                    Id = p.String(128),
                    Name = p.String(150)
                 

                },

                @" Update AspNetUsers SET Name=@Name WHERE Id=@Id");
        }

        public override void Down()
        {
            DropStoredProcedure("AspNetUsers_Update_Info");
        }
    }
}
