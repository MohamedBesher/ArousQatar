namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatRequests_AddSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("[dbo].[ChatRequests_Add]", p => new
            {
                AdvertismentId = p.Int(),
                RequestAuthorId = p.String(128),
                RequestDate = p.DateTime(),
            }, @"  
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
    
                --SET @UserTwo  = ( SELECT UserId
                --FROM   dbo.Trips
                --WHERE  Id = @TripId
                --);
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
                AdvertismentId = @AdvertismentId
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
                ");
        }
        
        public override void Down()
        {
            DropStoredProcedure("[dbo].[ChatRequests_Add]");
        }
    }
}
