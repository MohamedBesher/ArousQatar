namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateContactInfoAndContactTypeSp : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure(
                "ContactInformationGetAll",
                p => new
                {
                    PageNumber = p.Int(1),
                    PageSize = p.Int(8),
                    Filter = p.String(250, defaultValueSql: "NULL")
                },
                @" SELECT ContactInformations.* ,[ContactTypes].[Type] as 'ContactTypeName' , COUNT(ContactInformations.Id) OVER() AS 'OverAllCount'
                    FROM ContactInformations
	                left join 
	                [dbo].[ContactTypes]
	                on ContactInformations.[ContactTypeId]=[ContactTypes].Id
                    where 
	                @Filter is null or 
                    ContactInformations.Contact like '%'+@Filter+'%' 
                    ORDER BY Id DESC OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
                ");

            CreateStoredProcedure("ContactTypeAll", @"   
               SELECT *
                FROM 
	            [dbo].[ContactTypes]
                ORDER BY Id DESC ");
        }
        
        public override void Down()
        {
            DropStoredProcedure("ContactInformationGetAll");
            DropStoredProcedure("ContactTypeAll");
        }
    }
}
