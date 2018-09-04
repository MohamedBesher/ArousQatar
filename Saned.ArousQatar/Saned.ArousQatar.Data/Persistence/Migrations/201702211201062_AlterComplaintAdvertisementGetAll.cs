namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AlterComplaintAdvertisementGetAll : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("ComplaintAdvertisementGetAll", p => new
            { 
                index = p.Int(),
                rowNumber = p.Int(),
                filter = p.String(100, defaultValueSql: "")
            }, @"select complain.*, users.UserName 'CamplaintUser' , Advertisments.Name 'AdvertisementName'
                , COUNT(Advertisments.Id) OVER() AS 'OverAllCount'
                from Complaints complain
                left
                join AspNetUsers users
                on
                ApplicationUserId = users.Id
                left
                join Advertisments
                on
                AdvertismentId = Advertisments.Id
                where complain.ComplainedId is  null and complain.IsArchieved = 0
                and(complain.[Message] LIKE '%' + @filter + '%' or
                users.username   LIKE '%' + @filter + '%' or
                Advertisments.name LIKE '%' + @filter + '%')
                ORDER BY complain.Id Desc OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY; ");
        }

        public override void Down()
        {
            AlterStoredProcedure("ComplaintAdvertisementGetAll", @"select complain.*, users.UserName 'CamplaintUser' , Advertisments.Name 'AdvertisementName'
                                from Complaints  complain
                                left join AspNetUsers users    
                                on 
                                ApplicationUserId = users.Id
                                left join Advertisments
                                on 
                                AdvertismentId = Advertisments.Id
                                where complain.ComplainedId is  null and complain.IsArchieved = 0");
        }
    }
}
