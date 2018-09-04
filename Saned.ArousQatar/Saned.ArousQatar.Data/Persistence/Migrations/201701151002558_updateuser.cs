namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PhotoUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsApprove", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsApprove");
            DropColumn("dbo.AspNetUsers", "PhotoUrl");
        }
    }
}
