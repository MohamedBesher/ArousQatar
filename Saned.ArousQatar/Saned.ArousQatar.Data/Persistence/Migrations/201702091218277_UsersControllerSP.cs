namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersControllerSP : DbMigration
    {
        public override void Up()
        {
          


            CreateStoredProcedure("[dbo].[AspNetUsers_GetAll]", @"SELECT  *   
                                    FROM            
                                    AspNetUsers 
                                    ORDER BY 
                                    [dbo].[AspNetUsers].[Id]");

            CreateStoredProcedure("[dbo].[AspNetUsers_GetSingle]", p=> new { Id = p.String(128) }, @"
                                    SELECT *  	                
                                    FROM            
                                    AspNetUsers 
                                    where 
                                    AspNetUsers.Id= @Id
                                    ORDER BY 
                                    [dbo].[AspNetUsers].[Id] ");



            CreateStoredProcedure("[dbo].[AspNetUsers_GetAllFilterd]", p => new {
                PageNumber =p.Int(1),
                PageSize =p.Int(8),
                Role=p.String(250, defaultValueSql:null)
            }, @"
                                     SELECT   
                                       [dbo].[AspNetUsers].Id,   	                
                                        COUNT([dbo].[AspNetUsers].Id) OVER ( ) AS 'OverAllCount', 
	                                    [dbo].[AspNetUsers].[PhotoUrl],               
	                                    [dbo].[AspNetUsers].[PhoneNumber],   
                                        AspNetUsers.Name ,
	                                    AspNetUsers.[Email]
	                                   
                                        FROM            
                                        AspNetUsers 
                                        WHERE   
                                        Id IN 
                                        (
                                        SELECT UserId From  AspNetUserRoles
                                        INNER JOIN [dbo].[AspNetRoles]
                                        ON AspNetUserRoles.RoleId=[AspNetRoles].Id
                                        WHERE Name=ISNULL(@Role,Name)
                                        )
                                        ORDER BY 
                                        [dbo].[AspNetUsers].[Id] DESC OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
                                        ");

            
        }

        public override void Down()
        {
            DropStoredProcedure("[dbo].[AspNetUsers_GetAll]");
            DropStoredProcedure("[dbo].[AspNetUsers_GetSingle]");
            DropStoredProcedure("[dbo].[AspNetUsers_GetAllFilterd]");

        }
    }
}
