namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SelectUserInfo : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AspNetUsers_Select_Info",
                p => new
                {
                    UserName = p.String(256)

                },

                @"SELECT 
Id,
Name,
Email,
PhotoUrl,
(SELECT Count(Id) FROM Advertisments WHERE AspNetUsers.Id=Advertisments.ApplicationUserId ) AS 'AdsCount' ,
PhoneNumber
FROM AspNetUsers
WHERE UserName=@UserName");
        }

        public override void Down()
        {
            DropStoredProcedure("AspNetUsers_Select_Info");
        }
    }
}
