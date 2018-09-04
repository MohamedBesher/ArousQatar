namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatHeaderSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("[dbo].[ChatHeader_ById]", p=> new
            {
                Id= p.Int()
            }, @"  select * from [dbo].[ChatHeaders] 
                    where [dbo].[ChatHeaders].[Id] = @Id
                ");
            CreateStoredProcedure("[dbo].[ChatRequests_ById]", p => new
            {
                Id = p.Int()
            }, @" select * from [dbo].[ChatRequests]
                    where [dbo].[ChatRequests].[Id] = @Id
               ");
            CreateStoredProcedure("[dbo].[ChatHeader_ListByUserId]", p => new
            {
                UserId=p.String(128),
                PageNumber=p.Int(1),
                PageSize=p.Int(8),
            }, @"  SELECT * ,OverallCount = COUNT(1) OVER() FROM
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
                SELECT  Id ,
                ( SELECT    UserName
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                )AS 'UserName' ,
                ( SELECT    PhotoUrl
                FROM      dbo.AspNetUsers
                WHERE     dbo.AspNetUsers.Id = dbo.ChatHeaders.UserOneId
                ) AS 'PhotoUrl' ,
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
                WHERE   UserTwoId = @UserId
                ) s
                ");
        }
        
        public override void Down()
        {
            DropStoredProcedure("[dbo].[ChatHeader_ById]");
            DropStoredProcedure("[dbo].[ChatRequests_ById]");
            DropStoredProcedure("[dbo].[ChatHeader_ListByUserId]");
        }
    }
}
