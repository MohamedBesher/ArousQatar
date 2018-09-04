namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AlterStoredProcedure_Advertisements_DeleteAdvertisement : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("Advertisment_Delete", p =>
            new
            {
                Id = p.Int()
            }, @"BEGIN TRY
                    BEGIN TRANSACTION

                    DELETE FROM AdvertismentImages WHERE  AdvertismentId = @Id
	                DELETE FROM  ChatRequests WHERE AdvertismentId = @Id
	                DELETE FROM Comments WHERE AdvertismentId = @Id
	                DELETE FROM Complaints WHERE AdvertismentId = @Id
	                DELETE FROM Favorites WHERE AdvertismentId = @Id
	                DELETE FROM Likes WHERE AdvertismentId = @Id
	                DELETE FROM Advertisments WHERE Id = @Id

                    COMMIT TRANSACTION
                    SELECT 1 
                    END TRY
                    BEGIN CATCH
                    IF @@TRANCOUNT > 0 ROLLBACK;
                    SELECT 0
                    END CATCH");
        }

        public override void Down()
        {
            AlterStoredProcedure("Advertisment_Delete", p =>
            new
            {
                Id = p.Int()
            }, @" DELETE [dbo].[Advertisments] WHERE ([Id] = @Id)");
        }
    }
}
