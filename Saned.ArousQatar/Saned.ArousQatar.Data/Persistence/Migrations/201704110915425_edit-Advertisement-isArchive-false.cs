namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editAdvertisementisArchivefalse : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Advertisments", "IsArchieved", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Advertisments", "IsArchieved", c => c.Boolean());
        }
    }
}
