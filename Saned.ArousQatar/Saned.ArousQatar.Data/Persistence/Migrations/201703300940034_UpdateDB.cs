namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDB : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER PROCEDURE [dbo].[ChatHeader_ListByUserId]
    @UserId [nvarchar](128),
    @PageNumber [int] = 1,
    @PageSize [int] = 8,
    @AdvertismentId [int]
AS
BEGIN
    SELECT * ,OverallCount = COUNT(1) OVER() FROM
    (
    SELECT  ChatHeaders.Id ,
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
    FROM    dbo.ChatHeaders inner join ChatRequests on ChatHeaders.RequestId = ChatRequests.Id
    WHERE   UserOneId = @UserId and ChatRequests.AdvertismentId = @AdvertismentId
    
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
    ) s
END");

            Sql(@"ALTER PROCEDURE [dbo].[ChatRequests_Add] 
    @AdvertismentId [int],
    @RequestAuthorId [nvarchar](128),
    @RequestDate [datetime]

AS
BEGIN
      
    -- CHECK EXIT
    DECLARE @Id INT ,
    @RequestId INT ,
    @ChatId INT ,
    @TemChatID INT ,
    @TempId INT ,
    @UserOne NVARCHAR(128) ,
    @UserTwo NVARCHAR(128) ,
    @MessageContent NVARCHAR(MAX);
    BEGIN
    BEGIN TRY
    BEGIN TRANSACTION;
    IF NOT EXISTS ( SELECT  Id
    FROM    ChatRequests
    WHERE   AdvertismentId = @AdvertismentId And [RequestAuthorId]=@RequestAuthorId )
    BEGIN		
    INSERT  INTO ChatRequests
    ( AdvertismentId ,
    RequestAuthorId ,
    RequestDate
    )
    VALUES  (@AdvertismentId ,
    @RequestAuthorId ,
    @RequestDate 
    );
    
    SELECT  @Id = Id
    FROM    dbo.ChatRequests
    WHERE   @@ROWCOUNT > 0
    AND Id = SCOPE_IDENTITY();
    SET @RequestId = ( SELECT   t0.Id
    FROM     dbo.ChatRequests AS t0
    WHERE    @@ROWCOUNT > 0
    AND t0.Id = @Id
    );
    
    SET @UserTwo  = @RequestAuthorId;
    SET @UserOne = (SELECT ApplicationUserId
    FROM   [dbo].[Advertisments]
    WHERE  Id = @AdvertismentId
    );
    INSERT  INTO dbo.ChatHeaders
    ( RequestId ,
    UserOneId ,
    UserTwoId ,
    CreateDate
    )
    VALUES  ( @RequestId ,
    @UserOne ,
    @UserTwo ,
    @RequestDate
    );
    
    SELECT  @TemChatID = Id
    FROM    dbo.ChatHeaders
    WHERE   @@ROWCOUNT > 0
    AND Id = SCOPE_IDENTITY();
    SET @ChatId = ( SELECT  t0.Id
    FROM    dbo.ChatHeaders AS t0
    WHERE   @@ROWCOUNT > 0
    AND t0.Id = @TemChatID
    );
    
    INSERT  INTO dbo.ChatMessages
    ( MessageContent ,
    SentDate ,
    SenderId ,
    ChatId
    )
    VALUES  ( @MessageContent ,
    @RequestDate ,
    @RequestAuthorId ,
    @ChatId
    );
    
    SELECT  @ChatId;
    END;
    ELSE
    BEGIN
    SET @ChatId = ( SELECT  Id
    FROM    dbo.ChatHeaders
    WHERE   RequestId = ( SELECT
    Id
    FROM
    dbo.ChatRequests
    WHERE
    AdvertismentId = @AdvertismentId And [RequestAuthorId]=  N'd2a3cac5-0a9a-4a8b-82af-da2af88efc1b'
    )
    );
    SELECT  @ChatId;
    END;
    COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
    ROLLBACK;
    
    END CATCH;
    END;
    
END");
        }
        
        public override void Down()
        {
        }
    }
}
