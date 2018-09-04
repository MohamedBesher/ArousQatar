namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplaintAdvertisementGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "ComplaintAdvertisementGetAll",
                p => new
                {

                }, @"
                    select complain.*, users.UserName 'CamplaintUser' , Advertisments.Name 'AdvertisementName'
                     from Complaints  complain
                     left join AspNetUsers users    
                     on 
                     ApplicationUserId = users.Id
                     left join Advertisments
                     on 
                     AdvertismentId = Advertisments.Id
                    
                    where complain.ComplainedId is  null and complain.IsArchieved = 0
                    ");
        }

        public override void Down()
        {
            DropStoredProcedure("ComplaintAdvertisementGetAll");
        }
    }
}
