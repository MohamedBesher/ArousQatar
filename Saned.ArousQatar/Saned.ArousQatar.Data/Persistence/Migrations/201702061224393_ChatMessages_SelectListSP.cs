namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatMessages_SelectListSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("[dbo].[ChatMessages_SelectList]", p=> new
            {
                PageNumber=p.Int(1),
                PageSize=p.Int(8),
                LastSendDate=p.DateTime(),
                ChatId =p.Int(),
            }
            , @"    
                SELECT  ChatMessages.Id ,
                MessageContent ,
                SentDate ,
                CONVERT(char(12), SentDate, 114) AS 'FullDate',
                SenderId ,
                ChatId ,
                OverallCount = COUNT(1) OVER ( ),
                UserName
                FROM    dbo.ChatMessages
                INNER JOIN [dbo].[AspNetUsers] ON SenderId = [dbo].[AspNetUsers].Id
                WHERE  ChatId=@ChatId And SentDate >= ISNULL(@LastSendDate, SentDate)
                ORDER BY SentDate DESC
                OFFSET @PageSize * ( @PageNumber - 1 ) ROWS
                FETCH NEXT @PageSize ROWS ONLY
                OPTION  ( RECOMPILE );
            ");
        }
        
        public override void Down()
        {
            DropStoredProcedure("ChatMessages_SelectList");
        }
    }
}
