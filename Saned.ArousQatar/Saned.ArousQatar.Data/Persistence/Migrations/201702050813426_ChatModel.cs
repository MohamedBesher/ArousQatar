namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatHeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        UserOneId = c.String(maxLength: 128),
                        UserTwoId = c.String(maxLength: 128),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChatRequests", t => t.RequestId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserOneId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserTwoId)
                .Index(t => t.RequestId)
                .Index(t => t.UserOneId)
                .Index(t => t.UserTwoId);
            
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageContent = c.String(),
                        SentDate = c.DateTime(nullable: false),
                        SenderId = c.String(maxLength: 128),
                        ChatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChatHeaders", t => t.ChatId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ChatId);
            
            CreateTable(
                "dbo.ChatRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdvertismentId = c.Int(nullable: false),
                        RequestAuthorId = c.String(maxLength: 128),
                        RequestDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisments", t => t.AdvertismentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.RequestAuthorId)
                .Index(t => t.AdvertismentId)
                .Index(t => t.RequestAuthorId);
            
            CreateStoredProcedure(
                "dbo.ChatMessage_Insert",
                p => new
                    {
                        MessageContent = p.String(),
                        SentDate = p.DateTime(),
                        SenderId = p.String(maxLength: 128),
                        ChatId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ChatMessages]([MessageContent], [SentDate], [SenderId], [ChatId])
                      VALUES (@MessageContent, @SentDate, @SenderId, @ChatId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ChatMessages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ChatMessages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ChatMessage_Update",
                p => new
                    {
                        Id = p.Int(),
                        MessageContent = p.String(),
                        SentDate = p.DateTime(),
                        SenderId = p.String(maxLength: 128),
                        ChatId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ChatMessages]
                      SET [MessageContent] = @MessageContent, [SentDate] = @SentDate, [SenderId] = @SenderId, [ChatId] = @ChatId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ChatMessage_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ChatMessages]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ChatMessage_Delete");
            DropStoredProcedure("dbo.ChatMessage_Update");
            DropStoredProcedure("dbo.ChatMessage_Insert");
            DropForeignKey("dbo.ChatHeaders", "UserTwoId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatHeaders", "UserOneId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatHeaders", "RequestId", "dbo.ChatRequests");
            DropForeignKey("dbo.ChatRequests", "RequestAuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatRequests", "AdvertismentId", "dbo.Advertisments");
            DropForeignKey("dbo.ChatMessages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatMessages", "ChatId", "dbo.ChatHeaders");
            DropIndex("dbo.ChatRequests", new[] { "RequestAuthorId" });
            DropIndex("dbo.ChatRequests", new[] { "AdvertismentId" });
            DropIndex("dbo.ChatMessages", new[] { "ChatId" });
            DropIndex("dbo.ChatMessages", new[] { "SenderId" });
            DropIndex("dbo.ChatHeaders", new[] { "UserTwoId" });
            DropIndex("dbo.ChatHeaders", new[] { "UserOneId" });
            DropIndex("dbo.ChatHeaders", new[] { "RequestId" });
            DropTable("dbo.ChatRequests");
            DropTable("dbo.ChatMessages");
            DropTable("dbo.ChatHeaders");
        }
    }
}
