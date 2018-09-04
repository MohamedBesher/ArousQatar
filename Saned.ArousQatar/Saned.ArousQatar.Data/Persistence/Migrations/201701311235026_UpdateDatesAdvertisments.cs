namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatesAdvertisments : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Advertisments", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Advertisments", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Advertisments", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Advertisments", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
