namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplaintUserArchieveGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "ComplaintUserArchieveGetAll",
                p => new
                {

                }, @"
                    select complain.*, users.UserName 'CamplaintUser' , complained.UserName 'ComplainedUser'
                     from Complaints  complain
                     left join AspNetUsers users    
                     on 
                     ApplicationUserId = users.Id
                     left join AspNetUsers complained 
                     on 
                     ComplainedId = complained.Id
                    
                    where complain.ComplainedId is not null and complain.IsArchieved = 1
                    ");
        }

        public override void Down()
        {
            DropStoredProcedure("ComplaintUserArchieveGetAll");
        }
    }
}
