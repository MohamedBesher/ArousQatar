namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AlterStoredProcedureChatHeader_ListByUserId_and_ChatHeader_ListByAdvertismentId : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("ChatHeader_ListByAdvertismentId", p =>
                 new
                 {
                     AdvertismentId = p.Int(),
                     PageNumber = p.Int(defaultValueSql: "1"),
                     PageSize = p.Int(defaultValueSql: "8")
                 }, @" SELECT Id, ( SELECT    UserName
                        FROM      dbo.AspNetUsers
                        WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                        ) AS 'UserName'
    			                                         ,
                        ( SELECT    PhotoUrl
                        FROM      dbo.AspNetUsers
                        WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                        ) AS 'PhotoUrl',
	                     ( SELECT  Id
                        FROM      dbo.AspNetUsers
                        WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                        ) AS 'ReceiverId'
                        ,
	                     ( SELECT  UserName
                        FROM      dbo.AspNetUsers
                        WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                        ) AS 'ReceiverUserName'
                        ,	
	                     ( SELECT  Name
                        FROM      dbo.AspNetUsers
                        WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                        ) AS 'ReceiverName'
	                    ,
                    ( SELECT    PhotoUrl
                        FROM      dbo.AspNetUsers
                        WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                        ) AS 'ReceiverPhotoUrl',

	
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
                        FETCH NEXT @PageSize ROWS ONLY OPTION (RECOMPILE);");

            AlterStoredProcedure("ChatHeader_ListByUserId", p => new
            {
                UserId = p.String(128),
                PageNumber = p.Int(defaultValueSql: "1"),
                PageSize = p.Int(defaultValueSql: "8"),
                AdvertismentId = p.Int()
            }, @"SELECT * ,OverallCount = COUNT(1) OVER() FROM
                (
                SELECT  Id ,
                ( SELECT    UserName
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                ) AS 'UserName',
                ( SELECT    PhotoUrl
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                ) AS 'PhotoUrl' ,
	             ( SELECT  Id
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                ) AS 'ReceiverId'
                ,
	             ( SELECT  UserName
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                ) AS 'ReceiverUserName'
	            ,
	  
	             ( SELECT  Name
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                ) AS 'ReceiverName'
                ,	

            ( SELECT    PhotoUrl
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                ) AS 'ReceiverPhotoUrl',
                ( SELECT TOP ( 1 )
                MessageContent
                FROM      dbo.ChatMessages
                WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
                ORDER BY  SentDate DESC
                ) AS 'MessageContent' ,
                ( SELECT TOP ( 1 )
                SentDate
                FROM      dbo.ChatMessages
                WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
                ORDER BY  SentDate DESC
                ) AS 'SentDate'
                FROM    dbo.ChatHeaders
                WHERE   UserOneId = @UserId
    	                        UNION 		
                SELECT  ChatHeaders.Id ,
                ( SELECT    UserName
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                )AS 'UserName' ,
                ( SELECT    PhotoUrl
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                ) AS 'PhotoUrl' ,
	             ( SELECT  Id
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                ) AS 'ReceiverId'
                ,
	             ( SELECT  UserName
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                ) AS 'ReceiverUserName'
                ,	

	             ( SELECT  Name
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                ) AS 'ReceiverName'
	            ,

            ( SELECT    PhotoUrl
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserTwoId
                ) AS 'ReceiverPhotoUrl',

                ( SELECT TOP ( 1 )
                MessageContent
                FROM      dbo.ChatMessages
                WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
                ORDER BY  SentDate DESC
                ) AS 'MessageContent' ,
                ( SELECT TOP ( 1 )
                SentDate
                FROM      dbo.ChatMessages
                WHERE     dbo.ChatMessages.ChatId = dbo.ChatHeaders.Id
                ORDER BY  SentDate DESC
                ) AS 'SentDate'
                FROM    dbo.ChatHeaders inner join ChatRequests on ChatHeaders.RequestId = ChatRequests.Id
                WHERE   UserTwoId = @UserId and ChatRequests.AdvertismentId = @AdvertismentId
                ) s");
        }

        public override void Down()
        {
        }
    }
}
