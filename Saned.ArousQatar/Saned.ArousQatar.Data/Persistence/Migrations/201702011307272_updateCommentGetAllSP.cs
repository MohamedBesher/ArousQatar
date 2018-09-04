namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCommentGetAllSP : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure(
                "CommentGetAll",
                p => new
                {
                    Id = p.Int(),
                    PageNumber= p.Int(1),
                    PageSize = p.Int(8),
                    Filter=p.String(250, defaultValueSql: "NULL")
                },
                @" select Comments.* , AspNetUsers.Name 'UserFirstName',COUNT(Comments.Id) OVER() AS 'OverAllCount'
                    from Comments 
                    inner join AspNetUsers
                    on AspNetUsers.Id = Comments.ApplicationUserId
                    where Comments.AdvertismentId = @Id and Comments.CommentParentId is null
	                  and (@Filter is null or AspNetUsers.Name LIKE '%'+@Filter +'%' or Comments.[Message] LIKE '%'+@Filter +'%') 
                   ORDER BY Comments.CreateDate DESC OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CommentGetAll");
        }
    }
}
