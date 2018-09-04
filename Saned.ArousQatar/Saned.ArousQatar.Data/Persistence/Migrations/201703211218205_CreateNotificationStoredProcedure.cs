namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNotificationStoredProcedure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationMessageId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.PushNotifications", t => t.NotificationMessageId, cascadeDelete: true)
                .Index(t => t.NotificationMessageId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.PushNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        EnglishMessage = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Notified = c.Boolean(nullable: false),
                        ChatRequestId = c.Long(nullable: false),
                        ChatRequest_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChatRequests", t => t.ChatRequest_Id)
                .Index(t => t.ChatRequest_Id);


            Sql(@"Create Proc [Notification_GetList_ByUser] 
@Lang nvarchar(40),
@PageSize int,
@PageNumber int,
@UserId nvarchar(128)
as
select

        CASE

        WHEN  @Lang = 'ar' THEN ISNULL(d.Message, d.EnglishMessage)

        WHEN  @Lang = 'en' THEN ISNULL(d.EnglishMessage, d.Message)

        END AS Msg
 from PushNotifications as d
 join NotificationsLogs on d.Id = NotificationLogs.NotificationMessageId where NotificationLogs.ApplicationUserId = @UserId
 ORDER BY d.CreationDate DESC

    OFFSET @PageSize * (@PageNumber - 1) ROWS

    FETCH NEXT @PageSize ROWS ONLY

    OPTION(RECOMPILE)
");


            Sql(@"CREATE PROCEDURE [Notifications_Mark_AsRead]   
    @NotificationIds nvarchar(max)  
    
AS   
    SET NOCOUNT ON;  
	BEGIN TRY
    BEGIN TRANSACTION
    
		   UPDATE [dbo].[PushNotifications] set [dbo].[PushNotifications].[Notified]=1    	
			  WHERE Id IN(
					SELECT CAST(Item AS INTEGER)
					FROM [SplitString](@NotificationIds, ',')
			  )
     SELECT  @@ROWCOUNT
    COMMIT TRANSACTION

   
    
	END TRY
    
	BEGIN CATCH
    
	IF @@TRANCOUNT > 0 ROLLBACK;
    SELECT 0
    
	END CATCH");


            Sql(@"
CREATE PROCEDURE [Notifications_Select_ListByUserId]
    @UserId [nvarchar](128),
    @Lang [nvarchar](2),
    @PageNumber [int] = 1,
    @PageSize [int] = 10
AS
BEGIN
   bEGIN TRY
    SELECT pn.Id  ,  pn.ChatRequestId , pn.[Notified],
    case
    when @Lang = 'ar' then   pn.[Message] 
    when @Lang = 'en' then  pn.EnglishMessage	
    end as 'Message',
    OverallCount = COUNT(1) OVER() ,
	NotifiedCount =(SELECT COUNT(pn.Id )  FROM
				dbo.NotificationLogs nl	
				join 
				dbo.PushNotifications pn 
				ON nl.NotificationMessageId=pn.Id
				WHERE nl.ApplicationUserId=@UserId  
				and  pn.[Notified]=0)
    FROM
    dbo.NotificationLogs nl	
    join 
    dbo.PushNotifications pn 
    ON nl.NotificationMessageId=pn.Id
    WHERE nl.ApplicationUserId=@UserId
    ORDER BY
    pn.[CreationDate]  DESC	
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY OPTION (RECOMPILE);
END TRY
BEGIN CATCH
    SELECT 
	--ERROR_NUMBER() AS Message
     --,ERROR_SEVERITY() AS ErrorSeverity
     --,ERROR_STATE() AS ErrorState
     --,ERROR_PROCEDURE() AS ErrorProcedure,
     ERROR_LINE() AS OverallCount
     ,ERROR_MESSAGE() AS 'Message';
END CATCH
   
   
END");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotificationLogs", "NotificationMessageId", "dbo.PushNotifications");
            DropForeignKey("dbo.PushNotifications", "ChatRequest_Id", "dbo.ChatRequests");
            DropForeignKey("dbo.NotificationLogs", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.PushNotifications", new[] { "ChatRequest_Id" });
            DropIndex("dbo.NotificationLogs", new[] { "ApplicationUserId" });
            DropIndex("dbo.NotificationLogs", new[] { "NotificationMessageId" });
            DropTable("dbo.PushNotifications");
            DropTable("dbo.NotificationLogs");
        }
    }
}
