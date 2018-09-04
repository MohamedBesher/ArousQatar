namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class GetAllCommentsStored : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("Comments_GetPagedComments",
                p => new
                {
                    PageNumber = p.Int(),
                    PageSize = p.Int(),
                    Filter = p.String(250)

                }, @"SELECT Comments.* , Advertisments.Name AS AdvertismentName, AspNetUsers.Name AS UserFirstName FROM Comments
                    INNER JOIN Advertisments ON Advertisments.Id = Comments.AdvertismentId
                    INNER JOIN AspNetUsers ON AspNetUsers.Id = Comments.ApplicationUserId
                    WHERE (@Filter is null or (AspNetUsers.Name LIKE '%'+@Filter +'%' or Comments.[Message] LIKE '%'+@Filter +'%')) 
                    ORDER BY AdvertismentId OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY");

        }

        public override void Down()
        {
            DropStoredProcedure("CommentGetAll");
        }
    }
}
