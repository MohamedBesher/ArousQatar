namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatHeader_ListByAdvertismentId : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("[dbo].[ChatHeader_ListByAdvertismentId]", p=> new
            {
                AdvertismentId= p.Int(),
                PageNumber= p.Int(1),
                PageSize= p.Int(8)
            }, @"    SELECT Id, ( SELECT    UserName
                        FROM      dbo.AspNetUsers
                        WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                        ) AS 'UserName'
    			                     ,
    			                     ( SELECT    PhotoUrl
                        FROM      dbo.AspNetUsers
                        WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                        ) AS 'PhotoUrl',
                        ( SELECT    TOP(1) MessageContent
                        FROM      dbo.ChatMessages
                        WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
    			                      ORDER BY SentDate DESC
                        ) AS 'MessageContent'
    			                    ,OverallCount = COUNT(1) OVER(),
    			                     ( SELECT    TOP(1) SentDate
                        FROM      dbo.ChatMessages
                        WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
    			                      ORDER BY SentDate DESC
                        ) AS 'SentDate'
                        FROM    dbo.ChatHeaders
                        WHERE   RequestId IN ( SELECT   Id
                        FROM     dbo.ChatRequests
                        WHERE    AdvertismentId = @AdvertismentId )
    						                        ORDER BY
    		                                CreateDate Desc
    	                                OFFSET @PageSize * (@PageNumber - 1) ROWS
                        FETCH NEXT @PageSize ROWS ONLY OPTION (RECOMPILE);
            ");
        }
        
        public override void Down()
        {
            DropStoredProcedure("[dbo].[ChatHeader_ListByAdvertismentId]");
        }
    }
}
