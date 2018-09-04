namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterAspNetUsers_GetAllFilterd : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("AspNetUsers_GetAllFilterd", 
                p => new
                {
                    PageNumber = p.Int(),
                    PageSize = p.Int(),
                    Filter = p.String(250),
                    Role = p.String(250 )
                }, @"SELECT   
    [dbo].[AspNetUsers].Id,
    COUNT([dbo].[AspNetUsers].Id) OVER() AS 'OverAllCount', 

                                            [dbo].[AspNetUsers].[PhotoUrl],               
    	                                    [dbo].[AspNetUsers].[PhoneNumber],   
                                            AspNetUsers.Name ,
    	                                    AspNetUsers.[Email],
    (
    SELECT CAST
    (
    CASE WHEN EXISTS(

    SELECT LoginProvider FROM AspNetUserLogins WHERE AspNetUserLogins.UserId= AspNetUsers.Id
    ) THEN 1 
    ELSE 0 
    END
    AS BIT)
    ) As 'IsSoicalLogin'

    	                                   
    FROM
    AspNetUsers
    WHERE
    Id IN
    (
    SELECT UserId From AspNetUserRoles
    INNER JOIN [dbo].[AspNetRoles]
    ON AspNetUserRoles.RoleId=[AspNetRoles].Id
    WHERE[dbo].[AspNetRoles].[Name]= ISNULL(@Role,[dbo].[AspNetRoles].[Name])

    )

    AND(@Filter is null or Name LIKE '%' + @Filter + '%')
    ORDER BY
    [dbo].[AspNetUsers].[Id]
        DESC OFFSET(@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY");

        }
        
        public override void Down()
        {
            AlterStoredProcedure("AspNetUsers_GetAllFilterd",
               p => new
               {
                   PageNumber = p.Int(),
                   PageSize = p.Int(),
                   Filter = p.String(250),
                   Role = p.String(250)
               }, @"  SELECT   
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
    WHERE [dbo].[AspNetRoles].[Name]=ISNULL(@Role,[dbo].[AspNetRoles].[Name])
	
    )
	AND ( @Filter is null or  Name LIKE '%' + @Filter + '%')
    ORDER BY 
    [dbo].[AspNetUsers].[Id] DESC OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY");
        }
    }
}
